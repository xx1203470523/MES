using Dapper;
using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Core.Attribute.Job;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Constants.Manufacture;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Job;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.Core.Enums.QualUnqualifiedCode;
using Hymson.MES.CoreServices.Bos.Job;
using Hymson.MES.CoreServices.Bos.Manufacture;
using Hymson.MES.CoreServices.Dtos.Manufacture;
using Hymson.MES.CoreServices.Services.Common;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.Snowflake;
using Hymson.Utils;
using System.Net;

namespace Hymson.MES.CoreServices.Services.Job
{
    /// <summary>
    /// 不良录入
    /// </summary>
    [Job("产品不良录入", JobTypeEnum.Standard)]
    public class ProductBadRecordJobService : IJobService
    {
        /// <summary>
        /// 服务接口（主数据）
        /// </summary>
        private readonly IMasterDataService _masterDataService;

        /// <summary>
        /// 仓储接口（条码生产信息）
        /// </summary>
        private readonly IManuSfcProduceRepository _manuSfcProduceRepository;

        /// <summary>
        /// 产品不良录入 仓储
        /// </summary>
        private readonly IManuProductBadRecordRepository _manuProductBadRecordRepository;

        /// <summary>
        /// 条码表 仓储
        /// </summary>
        private readonly IManuSfcRepository _manuSfcRepository;

        /// <summary>
        /// 仓储接口（条码步骤）
        /// </summary>
        private readonly IManuSfcStepRepository _manuSfcStepRepository;


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="masterDataService"></param>
        /// <param name="manuSfcProduceRepository"></param>
        /// <param name="manuProductBadRecordRepository"></param>
        /// <param name="manuSfcRepository"></param>
        /// <param name="manuSfcStepRepository"></param>
        public ProductBadRecordJobService(IMasterDataService masterDataService,
            IManuSfcProduceRepository manuSfcProduceRepository,
            IManuProductBadRecordRepository manuProductBadRecordRepository,
            IManuSfcRepository manuSfcRepository,
            IManuSfcStepRepository manuSfcStepRepository)
        {
            _masterDataService = masterDataService;
            _manuSfcProduceRepository = manuSfcProduceRepository;
            _manuProductBadRecordRepository = manuProductBadRecordRepository;
            _manuSfcRepository = manuSfcRepository;
            _manuSfcStepRepository = manuSfcStepRepository;
        }

        /// <summary>
        /// 参数校验
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task VerifyParamAsync<T>(T param) where T : JobBaseBo
        {
            var bo = param.ToBo<ProductBadRecordRequestBo>();
            if (bo == null)
            {
                return;
            }

            // 验证DTO
            if (bo.SFCs == null || !bo.SFCs.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES15400));
            }

            // 获取生产条码信息
            var sfcProduceEntities = await bo.Proxy!.GetValueAsync(_masterDataService.GetManuSfcProduceInfoEntitiesAsync, bo);
            if (sfcProduceEntities == null || !sfcProduceEntities.Any())
            {
                return;
            }
            var sfcs = sfcProduceEntities.Select(x => x.SFC).ToArray();

            //判断条码报废状态
            var scrapSfcs = sfcProduceEntities.Where(x => x.IsScrap == TrueOrFalseEnum.Yes).Select(x => x.SFC).ToArray();
            if (scrapSfcs.Any())
            {
                var strs = string.Join("','", scrapSfcs);
                throw new CustomerValidationException(nameof(ErrorCode.MES15411)).WithData("sfcs", strs);
            }

            //判断条码锁定状态
            var lockSfcs = sfcProduceEntities.Where(x => x.Status == SfcStatusEnum.Locked).Select(x => x.SFC).ToArray();
            if (lockSfcs.Any())
            {
                var strs = string.Join("','", scrapSfcs);
                throw new CustomerValidationException(nameof(ErrorCode.MES15407)).WithData("sfcs", strs);
            }

            //获取不合格代码信息
            var qualUnqualifiedCodes = await bo.Proxy.GetValueAsync(_masterDataService.GetUnqualifiedEntitiesByIdsAsync!, bo.UnqualifiedIds);
            if (qualUnqualifiedCodes == null || !qualUnqualifiedCodes.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES15405));
            }

            //已经存在的不合格信息不允许重复录入
            var productBadRecordList = await _manuProductBadRecordRepository.GetManuProductBadRecordEntitiesBySFCAsync(new ManuProductBadRecordBySfcQuery
            {
                SFCs = bo.SFCs,
                Status = ProductBadRecordStatusEnum.Open,
                SiteId = bo.SiteId
            });

            var existUnqualifiedIds = productBadRecordList.Select(x => x.UnqualifiedId).Distinct().ToList();
            var sameUnqualifiedIds = existUnqualifiedIds.Intersect(bo.UnqualifiedIds!).ToList();
            if (sameUnqualifiedIds.Any())
            {
                var codes = qualUnqualifiedCodes.Where(x => sameUnqualifiedIds.Contains(x.Id)).Select(x => x.UnqualifiedCode).ToArray();
                var existsCodes = string.Join(',', codes);
                throw new CustomerValidationException(nameof(ErrorCode.MES15409)).WithData("codes", existsCodes);
            }

            //是否有录入缺陷
            var isDefect = qualUnqualifiedCodes.Any(x => x.Type == QualUnqualifiedCodeTypeEnum.Defect && x.UnqualifiedCode.ToUpperInvariant() != ManuProductBadRecord.ScrapCode);
            if (isDefect)
            {
                if (!bo.BadProcessRouteId.HasValue || bo.BadProcessRouteId == 0)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES15408));
                }

                //判断是否有未关闭的维修业务，有不允许添加
                var sfcRepairs = await _manuSfcProduceRepository.GetSfcProduceBusinessListBySFCAsync(new SfcListProduceBusinessQuery { SiteId = bo.SiteId, Sfcs = sfcs, BusinessType = ManuSfcProduceBusinessType.Repair });
                if (sfcRepairs != null && sfcRepairs.Any())
                {
                    var strs = string.Join(",", sfcRepairs.Select(x => x.Sfc));
                    throw new CustomerValidationException(nameof(ErrorCode.MES15410)).WithData("sfcs", strs);
                }
            }
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
        /// 数据组装
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<object?> DataAssemblingAsync<T>(T param) where T : JobBaseBo
        {
            var bo = param.ToBo<ProductBadRecordRequestBo>();
            if (bo == null)
            {
                return default;
            }

            // 获取生产条码信息
            var sfcProduceEntities = await bo.Proxy!.GetValueAsync(_masterDataService.GetManuSfcProduceInfoEntitiesAsync, bo);
            if (sfcProduceEntities == null || !sfcProduceEntities.Any())
            {
                return default;
            }
            var manuSfcs = sfcProduceEntities.AsList();
            var sfcs = manuSfcs.Select(x => x.SFC).ToArray();

            //获取不合格代码信息
            var qualUnqualifiedCodes = await bo.Proxy.GetValueAsync(_masterDataService.GetUnqualifiedEntitiesByIdsAsync!, bo.UnqualifiedIds);
            if (qualUnqualifiedCodes == null || !qualUnqualifiedCodes.Any())
            {
                return default;
            }

            //是否有录入缺陷
            var isDefect = qualUnqualifiedCodes.Any(x => x.Type == QualUnqualifiedCodeTypeEnum.Defect && x.UnqualifiedCode.ToUpperInvariant() != ManuProductBadRecord.ScrapCode);
            var processRouteProcedure = new ProcessRouteProcedureDto();
            if (isDefect)
            {
                processRouteProcedure = await _masterDataService.GetFirstProcedureAsync(bo.BadProcessRouteId ?? 0);
            }

            var sfcStepList = new List<ManuSfcStepEntity>();
            var manuSfcProduceList = new List<ManuSfcProduceBusinessEntity>();
            var scrapCode = qualUnqualifiedCodes.FirstOrDefault(a => a.UnqualifiedCode.ToUpperInvariant() == ManuProductBadRecord.ScrapCode);

            var isOnlyScrap = (scrapCode != null && qualUnqualifiedCodes.Count() == 1);
            var manuProductBadRecords = new List<ManuProductBadRecordEntity>();
            foreach (var item in manuSfcs)
            {
                var manuSfc = manuSfcs.FirstOrDefault(x => x.SFC == item.SFC);

                // 不良录入条码步骤
                var sfcStepEntity = _masterDataService.CreateSFCStepEntity(manuSfc!, ManuSfcStepTypeEnum.BadEntry, bo.SiteId, bo.ProcedureId,bo.ResourceId,null, bo.Remark ?? "");
                sfcStepList.Add(sfcStepEntity);

                foreach (var unqualified in qualUnqualifiedCodes)
                {
                    //报废的不需要记录不良，不需要关闭和展示
                    if (unqualified.UnqualifiedCode.ToUpperInvariant() == ManuProductBadRecord.ScrapCode)
                    {
                        continue;
                    }

                    manuProductBadRecords.Add(new ManuProductBadRecordEntity
                    {
                        Id = IdGenProvider.Instance.CreateId(),
                        SiteId = bo.SiteId,
                        FoundBadOperationId = bo.ProcedureId,
                        FoundBadResourceId = bo.ResourceId,
                        OutflowOperationId = bo.ProcedureId,
                        UnqualifiedId = unqualified.Id,
                        SfcStepId = sfcStepEntity.Id,
                        SFC = item.SFC,
                        SfcInfoId = item.SfcInfoId,
                        Qty = item.Qty,
                        Status = ProductBadRecordStatusEnum.Open,
                        Source = ProductBadRecordSourceEnum.BadManualEntry,
                        Remark = bo.Remark ?? "",
                        DisposalResult = isDefect ? ProductBadDisposalResultEnum.AutoHandle : null,
                        CreatedBy = bo.UserName,
                        UpdatedBy = bo.UserName
                    });
                }


                if (scrapCode != null)
                {
                    var scrapStep = _masterDataService.CreateSFCStepEntity(manuSfc!, ManuSfcStepTypeEnum.Discard, bo.SiteId, bo.ProcedureId,bo.ResourceId,null, bo.Remark ?? "");
                    sfcStepList.Add(scrapStep);
                }

                if (isDefect)
                {
                    // 在制品业务
                    var manuSfcProduceBusinessEntity = new ManuSfcProduceBusinessEntity
                    {
                        Id = IdGenProvider.Instance.CreateId(),
                        SfcProduceId = manuSfc!.Id,
                        BusinessType = ManuSfcProduceBusinessType.Repair,
                        BusinessContent = new SfcProduceRepairBo
                        {
                            ProcessRouteId = manuSfc.ProcessRouteId, //createDto.BadProcessRouteId ?? 0,
                            ProcedureId = manuSfc.ProcedureId //processRouteProcedure.ProcedureId
                        }.ToSerialize(),
                        SiteId = bo.SiteId,
                        CreatedBy = manuSfc.CreatedBy,
                        UpdatedBy = manuSfc.UpdatedBy
                    };
                    manuSfcProduceList.Add(manuSfcProduceBusinessEntity);
                }
            }

            var updateRouteCommand = new ManuSfcUpdateRouteCommand();
            if (isDefect)
            {
                updateRouteCommand = new ManuSfcUpdateRouteCommand
                {
                    ProcessRouteId = bo.BadProcessRouteId ?? 0,
                    ProcedureId = processRouteProcedure.ProcedureId,
                    UpdatedBy = bo.UserName,
                    Status = SfcStatusEnum.lineUp,
                    Ids = manuSfcs.Select(x => x.Id).ToArray()
                };
            }

            var isScrapCode = scrapCode != null;
            var updateCommand = new ManuSfcUpdateCommand();
            var isScrapCommand = new UpdateIsScrapCommand();
            if (isScrapCode)
            {
                updateCommand = new ManuSfcUpdateCommand
                {
                    SiteId = bo.SiteId,
                    Sfcs = sfcs,
                    UserId = bo.UserName,
                    UpdatedOn = HymsonClock.Now(),
                    Status = SfcStatusEnum.Scrapping
                };
                isScrapCommand = new UpdateIsScrapCommand
                {
                    SiteId = bo.SiteId,
                    Sfcs = sfcs,
                    UserId = bo.UserName,
                    UpdatedOn = HymsonClock.Now(),
                    IsScrap = TrueOrFalseEnum.Yes,
                    CurrentIsScrap = TrueOrFalseEnum.No
                };
            }

            return new ProductBadRecordResponseBo
            {
                IsScrapCode = isScrapCode,
                ManuProductBadRecords = manuProductBadRecords,
                SfcStepList = sfcStepList,
                ManuSfcProduceList = manuSfcProduceList,
                UpdateRouteCommand = updateRouteCommand,
                UpdateCommand = updateCommand,
                IsScrapCommand = isScrapCommand
            };
        }

        /// <summary>
        /// 执行入库
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public async Task<JobResponseBo?> ExecuteAsync(object obj)
        {
            JobResponseBo responseBo = new();
            if (obj is not ProductBadRecordResponseBo data)
            {
                return responseBo;
            }

            if (data.IsScrapCode)
            {
                // 修改在制品状态
                responseBo.Rows += await _manuSfcProduceRepository.UpdateIsScrapAsync(data.IsScrapCommand);

                // 修改条码状态
                responseBo.Rows += await _manuSfcRepository.UpdateStatusAsync(data.UpdateCommand);

                if (data.ManuProductBadRecords.Any())
                {
                    // 入库
                    responseBo.Rows += await _manuProductBadRecordRepository.InsertRangeAsync(data.ManuProductBadRecords);
                }

                if (data.SfcStepList.Any())
                {
                    await _manuSfcStepRepository.InsertRangeAsync(data.SfcStepList);
                }
                if (data.ManuSfcProduceList.Any())
                {
                    // 添加维修业务
                    await _manuSfcProduceRepository.InsertSfcProduceBusinessRangeAsync(data.ManuSfcProduceList);

                    // 修改在制品工艺路线和工序信息
                    await _manuSfcProduceRepository.UpdateRouteAsync(data.UpdateRouteCommand);
                }
            }
            else
            {
                if (data.ManuProductBadRecords.Any())
                {
                    // 入库
                    responseBo.Rows += await _manuProductBadRecordRepository.InsertRangeAsync(data.ManuProductBadRecords);
                }
                if (data.SfcStepList.Any())
                {
                    await _manuSfcStepRepository.InsertRangeAsync(data.SfcStepList);
                }
                if (data.ManuSfcProduceList.Any())
                {
                    // 添加维修业务
                    await _manuSfcProduceRepository.InsertSfcProduceBusinessRangeAsync(data.ManuSfcProduceList);

                    // 修改在制品工艺路线和工序信息
                    await _manuSfcProduceRepository.UpdateRouteAsync(data.UpdateRouteCommand);
                }
            }
            return responseBo;
        }

        /// <summary>
        /// 执行后节点
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="param"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<IEnumerable<JobBo>?> AfterExecuteAsync<T>(T param) where T : JobBaseBo
        {
            await Task.CompletedTask;
            return null;
        }

    }
}
