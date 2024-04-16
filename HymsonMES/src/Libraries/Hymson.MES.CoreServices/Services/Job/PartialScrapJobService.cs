using FluentValidation;
using FluentValidation.Results;
using Hymson.Infrastructure.Exceptions;
using Hymson.Localization.Services;
using Hymson.MES.Core.Attribute.Job;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Constants.Manufacture;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Job;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.CoreServices.Bos.Common;
using Hymson.MES.CoreServices.Bos.Job;
using Hymson.MES.CoreServices.Services.Common;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Manufacture.ManuSfc.Command;
using Hymson.MES.Data.Repositories.Manufacture.ManuSfcProduce.Command;
using Hymson.MES.Data.Repositories.Quality;
using Hymson.MES.Data.Repositories.Warehouse;
using Hymson.MES.Data.Repositories.Warehouse.WhMaterialInventory.Command;
using Hymson.Snowflake;
using Hymson.Utils;


namespace Hymson.MES.CoreServices.Services.Job
{
    /// <summary>
    /// 部分报废，配合产出上报job 调用,不支持单独调用
    /// </summary>
    [Job("部分报废", JobTypeEnum.Standard)]
    public class PartialScrapJobService : IJobService
    {
        /// <summary>
        /// 服务接口（主数据）
        /// </summary>
        private readonly IMasterDataService _masterDataService;

        /// <summary>
        /// 仓储接口（条码信息）
        /// </summary>
        private readonly IManuSfcRepository _manuSfcRepository;

        /// <summary>
        /// 仓储接口（条码生产信息）
        /// </summary>
        private readonly IManuSfcProduceRepository _manuSfcProduceRepository;

        /// <summary>
        /// 仓储接口（条码步骤）
        /// </summary>
        private readonly IManuSfcStepRepository _manuSfcStepRepository;
        private readonly IManuSfcScrapRepository _manuSfcScrapRepository;
        private readonly IManuSfcInfoRepository _manuSfcInfoRepository;
        private readonly IWhMaterialInventoryRepository _whMaterialInventoryRepository;
        private readonly IQualUnqualifiedCodeRepository _qualUnqualifiedCodeRepository;
        /// <summary>
        /// 
        /// </summary>
        private readonly ILocalizationService _localizationService;

        public PartialScrapJobService(IMasterDataService masterDataService,
            IManuSfcRepository manuSfcRepository,
            IManuSfcScrapRepository manuSfcScrapRepository,
            IManuSfcInfoRepository manuSfcInfoRepository,
            IWhMaterialInventoryRepository whMaterialInventoryRepository,
            ILocalizationService localizationService,
            IQualUnqualifiedCodeRepository qualUnqualifiedCodeRepository,
            IManuSfcProduceRepository manuSfcProduceRepository, IManuSfcStepRepository manuSfcStepRepository)
        {
            _masterDataService = masterDataService;
            _manuSfcRepository = manuSfcRepository;
            _manuSfcProduceRepository = manuSfcProduceRepository;
            _manuSfcStepRepository = manuSfcStepRepository;
            _manuSfcScrapRepository = manuSfcScrapRepository;
            _manuSfcInfoRepository = manuSfcInfoRepository;
            _whMaterialInventoryRepository = whMaterialInventoryRepository;
            _localizationService = localizationService;
            _qualUnqualifiedCodeRepository = qualUnqualifiedCodeRepository;
        }

        /// <summary>
        /// 执行前节点
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<IEnumerable<JobBo>?> BeforeExecuteAsync<T>(T param) where T : JobBaseBo
        {
            await Task.CompletedTask;
            return null;
        }

        /// <summary>
        /// 参数校验
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task VerifyParamAsync<T>(T param) where T : JobBaseBo
        {
            if (param is not JobRequestBo commonBo) return ;

            // 临时中转变量
            var multiSFCBo = new MultiSFCBo { SiteId = commonBo.SiteId, SFCs = commonBo.OutStationRequestBos.Select(s => s.SFC) };

            // 获取生产条码信息
            var manuSfcProduces = await commonBo.Proxy!.GetDataBaseValueAsync(_masterDataService.GetProduceEntitiesBySFCsAsync, multiSFCBo);
      
            var sfcEntities = await _manuSfcRepository.GetListAsync(new ManuSfcQuery
            {
                SFCs = multiSFCBo.SFCs,
                SiteId = commonBo.SiteId,
                Type = SfcTypeEnum.Produce
            });
            //条码信息表
            var sfcInfoEntities = await _manuSfcInfoRepository.GetBySFCIdsAsync(sfcEntities.Select(x => x.Id));
            var manuSfcProducePagedQuery = new ManuSfcProduceQuery { Sfcs = multiSFCBo.SFCs, SiteId = commonBo.SiteId };
            //在制品信息
           // var manuSfcProduces = await _manuSfcProduceRepository.GetManuSfcProduceEntitiesAsync(manuSfcProducePagedQuery);

            //修改库存信息
            var whMaterialInventoryList = await _whMaterialInventoryRepository.GetByBarCodesAsync(new Data.Repositories.Warehouse.WhMaterialInventory.Query.WhMaterialInventoryBarCodesQuery
            {
                SiteId = commonBo.SiteId,
                BarCodes = multiSFCBo.SFCs
            });

            var validationFailures = new List<ValidationFailure>();
            foreach (var item in commonBo.OutStationRequestBos)
            {
                if (item.OutStationUnqualifiedList == null || !item.OutStationUnqualifiedList.Any())
                    continue;
                var sfc = item.SFC;
                var ScrapQty = item.OutStationUnqualifiedList.Sum(o => o.UnqualifiedQty);
                var sfcEntity = sfcEntities.FirstOrDefault(x => x.SFC == item.SFC);

                if (sfcEntity == null)
                {
                    var validationFailure = new ValidationFailure();
                    if (validationFailure.FormattedMessagePlaceholderValues == null || !validationFailure.FormattedMessagePlaceholderValues.Any())
                    {
                        validationFailure.FormattedMessagePlaceholderValues = new Dictionary<string, object> {
                            { "CollectionIndex", sfc}
                        };
                    }
                    else
                    {
                        validationFailure.FormattedMessagePlaceholderValues.Add("CollectionIndex", sfc);
                    }
                    validationFailure.ErrorCode = nameof(ErrorCode.MES15415);
                    validationFailures.Add(validationFailure);
                    continue;
                }
               
                if (ManuSfcStatus.ForbidScrapSfcStatuss.Contains(sfcEntity.Status))
                {
                    var validationFailure = new ValidationFailure();
                    if (validationFailure.FormattedMessagePlaceholderValues == null || !validationFailure.FormattedMessagePlaceholderValues.Any())
                    {
                        validationFailure.FormattedMessagePlaceholderValues = new Dictionary<string, object> {
                            { "CollectionIndex", sfc}
                        };
                    }
                    else
                    {
                        validationFailure.FormattedMessagePlaceholderValues.Add("CollectionIndex", sfc);
                    }
                    validationFailure.FormattedMessagePlaceholderValues.Add("Status", _localizationService.GetResource($"Hymson.MES.Core.Enums.manu.SfcStatusEnum.{SfcStatusEnum.GetName(sfcEntity.Status)}"));
                    validationFailure.ErrorCode = nameof(ErrorCode.MES15450);
                    validationFailures.Add(validationFailure);
                    continue;
                }

                if (sfcEntity.Status == SfcStatusEnum.Locked)
                {
                    var validationFailure = new ValidationFailure();
                    if (validationFailure.FormattedMessagePlaceholderValues == null || !validationFailure.FormattedMessagePlaceholderValues.Any())
                    {
                        validationFailure.FormattedMessagePlaceholderValues = new Dictionary<string, object> {
                            { "CollectionIndex", sfc}
                        };
                    }
                    else
                    {
                        validationFailure.FormattedMessagePlaceholderValues.Add("CollectionIndex", sfc);
                    }
                    validationFailure.ErrorCode = nameof(ErrorCode.MES15416);
                    validationFailures.Add(validationFailure);
                    continue;
                }
            }
            if (validationFailures.Any())
            {
                throw new ValidationException(_localizationService.GetResource("SFCError"), validationFailures);
            }

        }

        /// <summary>
        /// 数据组装
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<object?> DataAssemblingAsync<T>(T param) where T : JobBaseBo
        {

            if (param is not JobRequestBo commonBo) return default;

            // 临时中转变量
            var multiSFCBo = new MultiSFCBo { SiteId = commonBo.SiteId, SFCs = commonBo.OutStationRequestBos.Select(s => s.SFC) };

            // 获取生产条码信息
            var manuSfcProduces = await commonBo.Proxy!.GetDataBaseValueAsync(_masterDataService.GetProduceEntitiesBySFCsAsync, multiSFCBo);
            //条码表
            var sfcEntities = await _manuSfcRepository.GetListAsync(new ManuSfcQuery
            {
                SFCs = multiSFCBo.SFCs,
                SiteId = commonBo.SiteId,
                Type = SfcTypeEnum.Produce
            });
            //条码信息表
            var sfcInfoEntities = await _manuSfcInfoRepository.GetBySFCIdsAsync(sfcEntities.Select(x => x.Id));

            var qualUnqualifiedCodeEntities = await _qualUnqualifiedCodeRepository.GetByCodesAsync(new QualUnqualifiedCodeByCodesQuery
            {
                SiteId = commonBo.SiteId,
                Codes = commonBo.OutStationRequestBos.SelectMany(s => s.OutStationUnqualifiedList?.Select(o => o.UnqualifiedCode))
            });
            List<ManuSfcScrapEntity> manuSfcScrapEntities = new();
            List<ManuSFCPartialScrapByIdCommand> manuSFCPartialScrapByIdCommandList = new();
          //  List<ManuSfcProducePartialScrapByIdCommand> manuSfcProducePartialScrapByIdCommandList = new();
            List<ManuSfcStepEntity> manuSfcStepEntities = new();
            List<ScrapPartialWhMaterialInventoryByIdCommand> scrapPartialWhMaterialInventoryEmptyByIdCommandList = new();
          
            foreach (var item in commonBo.OutStationRequestBos)
            {
                if (item.OutStationUnqualifiedList == null || !item.OutStationUnqualifiedList.Any())
                    continue;
                var sfc = item.SFC;
                var ScrapQty = item.OutStationUnqualifiedList.Sum(o => o.UnqualifiedQty??0);
                var manuSfcProduce = manuSfcProduces.FirstOrDefault(x => x.SFC == sfc);
                var sfcEntity = sfcEntities.FirstOrDefault(x => x.SFC == sfc);
                var sfcInfoEntity = sfcInfoEntities.FirstOrDefault(x => x.SfcId == sfcEntity.Id);
                //var manuSFCPartialScrapByIdCommand = new ManuSFCPartialScrapByIdCommand
                //{
                //    Id = sfcEntity.Id,
                //    ScrapQty = ScrapQty,
                //    Qty = manuSfcProduce.Qty,
                //    UpdatedOn = HymsonClock.Now(),
                //    UpdatedBy = commonBo.UserName,
                //    CurrentQty = sfcEntity.Qty,
                //    CurrentStatus = sfcEntity.Status,
                //};
                //if (manuSfcProduce.Qty==0)
                //{
                //    manuSFCPartialScrapByIdCommand.Status = SfcStatusEnum.Scrapping;
                //}
                //else
                //{
                //    manuSFCPartialScrapByIdCommand.Status = sfcEntity.Status;
                //}

               // manuSFCPartialScrapByIdCommandList.Add(manuSFCPartialScrapByIdCommand);
                   
                //manuSfcProducePartialScrapByIdCommandList.Add(new ManuSfcProducePartialScrapByIdCommand
                //{
                //    Id = manuSfcProduce.Id,
                //    ScrapQty = ScrapQty,
                //    Qty = manuSfcProduce.Qty,
                //    UpdatedOn = HymsonClock.Now(),
                //    UpdatedBy = commonBo.UserName
                //});

               
                foreach (var unqualifiedBo in item.OutStationUnqualifiedList)
                {
                    //记录步骤表
                    var stepEntity = new ManuSfcStepEntity
                    {
                        Id = IdGenProvider.Instance.CreateId(),
                        SFC = sfc,
                        ProductId = manuSfcProduce.ProductId,
                        WorkOrderId = manuSfcProduce.WorkOrderId,
                        WorkCenterId = manuSfcProduce?.WorkCenterId,
                        ProductBOMId = manuSfcProduce?.ProductBOMId,
                        Qty = sfcEntity.Qty,
                        ScrapQty = unqualifiedBo.UnqualifiedQty,
                        EquipmentId = manuSfcProduce?.EquipmentId,
                        ResourceId = manuSfcProduce?.ResourceId,
                        ProcedureId = manuSfcProduce?.ProcedureId,
                        Operatetype = ManuSfcStepTypeEnum.PartialDiscard,
                        CurrentStatus = sfcEntity.Status,
                        Remark = "",
                        SiteId = commonBo.SiteId,
                        CreatedOn = HymsonClock.Now(),
                        CreatedBy = commonBo.UserName,
                        UpdatedOn = HymsonClock.Now(),
                        UpdatedBy = commonBo.UserName
                    };
                    manuSfcStepEntities.Add(stepEntity);
                    //记录报废信息
                    var manuSfcScrapEntity = new ManuSfcScrapEntity()
                    {
                        Id = IdGenProvider.Instance.CreateId(),
                        SFC = sfc,
                        SfcinfoId = sfcInfoEntity?.Id ?? 0,
                        SfcStepId = stepEntity.Id,
                        ProcedureId = commonBo.ProcedureId,
                        UnqualifiedId = qualUnqualifiedCodeEntities.FirstOrDefault(x=>x.UnqualifiedCode== unqualifiedBo.UnqualifiedCode)?.Id,
                        OutFlowProcedureId = commonBo.ProcedureId,
                        ScrapQty = unqualifiedBo.UnqualifiedQty,
                        IsCancel = false,
                        Remark = "",
                        SiteId = commonBo.SiteId,
                        CreatedOn = HymsonClock.Now(),
                        CreatedBy = commonBo.UserName,
                        UpdatedOn = HymsonClock.Now(),
                        UpdatedBy = commonBo.UserName
                    };
                    manuSfcScrapEntities.Add(manuSfcScrapEntity);
                }
               
              
            }

            return new SFCPartialScrapResponseBo
            {
                manuSfcScrapEntities = manuSfcScrapEntities,
                manuSfcStepEntities = manuSfcStepEntities,
              //  manuSFCPartialScrapByIdCommandList = manuSFCPartialScrapByIdCommandList,
               // manuSfcProducePartialScrapByIdCommandList = manuSfcProducePartialScrapByIdCommandList
            };
        }
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public async Task<JobResponseBo?> ExecuteAsync(object obj)
        {
            JobResponseBo responseBo = new();
            if (obj is not SFCPartialScrapResponseBo data) return responseBo;

            if (data.manuSfcStepEntities != null && data.manuSfcStepEntities.Any())
            {
                await _manuSfcStepRepository.InsertRangeAsync(data.manuSfcStepEntities);
            }

            if (data.manuSfcScrapEntities != null && data.manuSfcScrapEntities.Any())
            {
                await _manuSfcScrapRepository.InsertRangeAsync(data.manuSfcScrapEntities);
            }

            //if (data.manuSFCPartialScrapByIdCommandList != null && data.manuSFCPartialScrapByIdCommandList.Any())
            //{
            //    await _manuSfcRepository.UpdateManuSfcQtyByIdRangeAsync(data.manuSFCPartialScrapByIdCommandList);
            //}
            return responseBo;
        }

        /// <summary>
        /// 执行后节点
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<IEnumerable<JobBo>?> AfterExecuteAsync<T>(T param) where T : JobBaseBo
        {
            await Task.CompletedTask;
            return null;
        }
    }
}
