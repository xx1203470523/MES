using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Domain.Plan;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.CoreServices.Bos.Job;
using Hymson.MES.CoreServices.Dtos.Common;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Plan;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Services.Dtos.Manufacture;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;
using System.Transactions;

namespace Hymson.MES.Services.Services.Manufacture
{
    /// <summary>
    /// 容器装载表（物理删除） 服务
    /// </summary>
    public class ManuContainerPackService : IManuContainerPackService
    {
        private readonly ICurrentUser _currentUser;
        private readonly ICurrentSite _currentSite;

        /// <summary>
        /// 容器条码表 仓储
        /// </summary>
        private readonly IManuContainerBarcodeRepository _manuContainerBarcodeRepository;
        /// <summary>
        /// 容器装载表（物理删除） 仓储
        /// </summary>
        private readonly IManuContainerPackRepository _manuContainerPackRepository;
        private readonly IProcMaterialRepository _procMaterialRepository;
        private readonly IPlanWorkOrderRepository _planWorkOrderRepository;
        /// <summary>
        /// 容器装载记录 仓储
        /// </summary>
        private readonly IManuContainerPackRecordRepository _manuContainerPackRecordRepository;

        private readonly AbstractValidator<ManuContainerPackCreateDto> _validationCreateRules;
        private readonly AbstractValidator<ManuContainerPackModifyDto> _validationModifyRules;
        /// <summary>
        /// 接口（操作面板按钮）
        /// </summary>
        private readonly IManuFacePlateButtonService _manuFacePlateButtonService;
        private readonly IManuSfcRepository _manuSfcRepository;
        private readonly IManuSfcInfoRepository _manuSfcInfoRepository;
        private readonly IManuSfcProduceRepository _manuSfcProduceRepository;

        /// <summary>
        /// 仓储接口（条码步骤）
        /// </summary>
        private readonly IManuSfcStepRepository _manuSfcStepRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="currentSite"></param>
        /// <param name="manuContainerBarcodeRepository"></param>
        /// <param name="manuContainerPackRepository"></param>
        /// <param name="planWorkOrderRepository"></param>
        /// <param name="manuContainerPackRecordRepository"></param>
        /// <param name="manuSfcRepository"></param>
        /// <param name="manuSfcInfoRepository"></param>
        /// <param name="manuSfcProduceRepository"></param>
        /// <param name="validationCreateRules"></param>
        /// <param name="validationModifyRules"></param>
        /// <param name="procMaterialRepository"></param>
        /// <param name="manuFacePlateButtonService"></param>
        /// <param name="manuSfcStepRepository"></param>
        public ManuContainerPackService(ICurrentUser currentUser, ICurrentSite currentSite,
            IManuContainerBarcodeRepository manuContainerBarcodeRepository,
            IManuContainerPackRepository manuContainerPackRepository,
            IPlanWorkOrderRepository planWorkOrderRepository,
            IManuContainerPackRecordRepository manuContainerPackRecordRepository,
            IManuSfcRepository manuSfcRepository,
            IManuSfcInfoRepository manuSfcInfoRepository,
            IManuSfcProduceRepository manuSfcProduceRepository,
            AbstractValidator<ManuContainerPackCreateDto> validationCreateRules,
            AbstractValidator<ManuContainerPackModifyDto> validationModifyRules,
            IProcMaterialRepository procMaterialRepository,
            IManuFacePlateButtonService manuFacePlateButtonService,
            IManuSfcStepRepository manuSfcStepRepository)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _manuContainerBarcodeRepository = manuContainerBarcodeRepository;
            _manuContainerPackRepository = manuContainerPackRepository;
            _planWorkOrderRepository = planWorkOrderRepository;
            _validationCreateRules = validationCreateRules;
            _validationModifyRules = validationModifyRules;
            _procMaterialRepository = procMaterialRepository;
            _manuContainerPackRecordRepository = manuContainerPackRecordRepository;
            _manuSfcInfoRepository = manuSfcInfoRepository;
            _manuSfcRepository = manuSfcRepository;
            _manuSfcProduceRepository = manuSfcProduceRepository;
            _manuFacePlateButtonService = manuFacePlateButtonService;
            _manuSfcStepRepository = manuSfcStepRepository;
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="manuContainerPackCreateDto"></param>
        /// <returns></returns>
        public async Task CreateManuContainerPackAsync(ManuContainerPackCreateDto manuContainerPackCreateDto)
        {
            //验证DTO
            await _validationCreateRules.ValidateAndThrowAsync(manuContainerPackCreateDto);

            //DTO转换实体
            var manuContainerPackEntity = manuContainerPackCreateDto.ToEntity<ManuContainerPackEntity>();
            manuContainerPackEntity.Id = IdGenProvider.Instance.CreateId();
            manuContainerPackEntity.CreatedBy = _currentUser.UserName;
            manuContainerPackEntity.UpdatedBy = _currentUser.UserName;
            manuContainerPackEntity.CreatedOn = HymsonClock.Now();
            manuContainerPackEntity.UpdatedOn = HymsonClock.Now();
            manuContainerPackEntity.SiteId = _currentSite.SiteId ?? 0;

            //入库
            await _manuContainerPackRepository.InsertAsync(manuContainerPackEntity);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteManuContainerPackAsync(long id)
        {
            await _manuContainerPackRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task DeletesManuContainerPackAsync(ManuContainerPackUnpackDto param)
        {
            var manuContainerPackList = await _manuContainerPackRepository.GetByIdsAsync(param.Ids);
            if (manuContainerPackList == null || !manuContainerPackList.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16732));
            }
            var manuContainerBarcodeEntity = await _manuContainerBarcodeRepository.GetByIdAsync(manuContainerPackList.FirstOrDefault()!.ContainerBarCodeId.GetValueOrDefault());

            IEnumerable<ManuSfcEntity> manuSfclist = new List<ManuSfcEntity>();
            IEnumerable<ManuSfcInfoEntity> manuSfcInfolist = new List<ManuSfcInfoEntity>();
            IEnumerable<ManuSfcProduceEntity> manuSfcProduceList = new List<ManuSfcProduceEntity>();
            if (manuContainerBarcodeEntity.PackLevel == (int)LevelEnum.One)
            {
                manuSfclist = await _manuSfcRepository.GetListAsync(new ManuSfcQuery
                {
                    SiteId = _currentSite.SiteId,
                    SFCs = manuContainerPackList.Select(x => x.LadeBarCode),
                    Type = SfcTypeEnum.Produce
                });
                var manuSfcInfolistTask = _manuSfcInfoRepository.GetBySFCIdsWithIsUseAsync(manuSfclist.Select(x => x.Id));
                var manuSfcProduceListTask = _manuSfcProduceRepository.GetManuSfcProduceEntitiesAsync(new ManuSfcProduceQuery
                {
                    Sfcs = manuContainerPackList.Select(x => x.LadeBarCode),
                    SiteId = _currentSite.SiteId ?? 0
                });
                manuSfcInfolist = await manuSfcInfolistTask;
                manuSfcProduceList = await manuSfcProduceListTask;
            }
            var manuSfcStepList = new List<ManuSfcStepEntity>();
            var lst = new List<ManuContainerPackRecordEntity>();
            foreach (var item in manuContainerPackList)
            {
                lst.Add(new ManuContainerPackRecordEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SiteId = item.SiteId,
                    CreatedBy = _currentUser.UserName,
                    UpdatedBy = _currentUser.UserName,
                    ResourceId = param.ResourceId ?? 0,
                    ProcedureId = param.ProcedureId ?? 0,
                    ContainerBarCodeId = item.ContainerBarCodeId,
                    LadeBarCode = item.LadeBarCode,
                    OperateType = ManuContainerPackRecordOperateTypeEnum.Remove
                });

                if (manuContainerBarcodeEntity.PackLevel == (int)LevelEnum.One)
                {
                    var manuSfcEntity = manuSfclist.FirstOrDefault(x => x.SFC == item.LadeBarCode);
                    if (manuSfcEntity != null)
                    {
                        var manuSfcInfoEntity = manuSfcInfolist.FirstOrDefault(x => x.SfcId == manuSfcEntity.Id);
                        if (manuSfcInfoEntity != null)
                        {
                            var manuSfcProduceEntity = manuSfcProduceList.FirstOrDefault(x => x.SFC == item.LadeBarCode);
                            manuSfcStepList.Add(new ManuSfcStepEntity
                            {
                                Id = IdGenProvider.Instance.CreateId(),
                                SiteId = _currentSite.SiteId ?? 0,
                                CreatedBy = _currentUser.UserName,
                                UpdatedBy = _currentUser.UserName,
                                SFC = item.LadeBarCode,
                                ProductId = manuSfcInfoEntity.ProductId,
                                WorkOrderId = manuSfcInfoEntity.WorkOrderId ?? 0,
                                ResourceId = param.ResourceId,
                                ProcedureId = param.ProcedureId,
                                Operatetype = ManuSfcStepTypeEnum.Unpack,
                                Qty = manuSfcEntity.Qty,
                                WorkCenterId = manuSfcProduceEntity == null ? null : manuSfcProduceEntity.WorkCenterId,
                                ProductBOMId = manuSfcProduceEntity == null ? null : manuSfcProduceEntity.WorkCenterId,
                                CurrentStatus = manuSfcProduceEntity == null ? SfcStatusEnum.Complete : manuSfcProduceEntity.Status
                            });
                        }
                    }
                }
            }
            using (TransactionScope ts = TransactionHelper.GetTransactionScope())
            {
                if (manuSfcStepList != null && manuSfcStepList.Any())
                {
                    await _manuSfcStepRepository.InsertRangeAsync(manuSfcStepList);
                }
                await _manuContainerPackRecordRepository.InsertsAsync(lst);
                await _manuContainerPackRepository.DeleteTrueAsync(new DeleteCommand { Ids = param.Ids, DeleteOn = HymsonClock.Now(), UserId = _currentUser.UserName });
                ts.Complete();
            }
        }

        /// <summary>
        /// 根据容器Id 删除所有容器装载记录（物理删除）
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task DeleteAllByContainerBarCodeIdAsync(ContainerUnpackDto param)
        {
            var manuContainerBarcodeEntity = await _manuContainerBarcodeRepository.GetByIdAsync(param.ContainerBarCodeId);

            var manuContainerPackList = await _manuContainerPackRepository.GetByContainerBarCodeIdIdAsync(param.ContainerBarCodeId);
            IEnumerable<ManuSfcEntity> manuSfclist = new List<ManuSfcEntity>();
            IEnumerable<ManuSfcInfoEntity> manuSfcInfolist = new List<ManuSfcInfoEntity>();
            IEnumerable<ManuSfcProduceEntity> manuSfcProduceList = new List<ManuSfcProduceEntity>();
            if (manuContainerBarcodeEntity.PackLevel == (int)LevelEnum.One)
            {
                manuSfclist = await _manuSfcRepository.GetListAsync(new ManuSfcQuery
                {
                    SiteId = _currentSite.SiteId,
                    SFCs = manuContainerPackList.Select(x => x.LadeBarCode),
                    Type = SfcTypeEnum.Produce
                });
                var manuSfcInfolistTask = _manuSfcInfoRepository.GetBySFCIdsWithIsUseAsync(manuSfclist.Select(x => x.Id));
                var manuSfcProduceListTask = _manuSfcProduceRepository.GetManuSfcProduceEntitiesAsync(new ManuSfcProduceQuery
                {
                    Sfcs = manuContainerPackList.Select(x => x.LadeBarCode),
                    SiteId = _currentSite.SiteId ?? 0
                });
                manuSfcInfolist = await manuSfcInfolistTask;
                manuSfcProduceList = await manuSfcProduceListTask;
            }
            var manuSfcStepList = new List<ManuSfcStepEntity>();
            var lst = new List<ManuContainerPackRecordEntity>();
            foreach (var item in manuContainerPackList)
            {
                lst.Add(new ManuContainerPackRecordEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SiteId = item.SiteId,
                    CreatedBy = _currentUser.UserName,
                    UpdatedBy = _currentUser.UserName,
                    ResourceId = param.ResourceId ?? 0,
                    ProcedureId = param.ProcedureId ?? 0,
                    ContainerBarCodeId = item.ContainerBarCodeId,
                    LadeBarCode = item.LadeBarCode,
                    OperateType = ManuContainerPackRecordOperateTypeEnum.Remove
                });

                if (manuContainerBarcodeEntity.PackLevel == (int)LevelEnum.One)
                {
                    var manuSfcEntity = manuSfclist.FirstOrDefault(x => x.SFC == item.LadeBarCode);
                    if (manuSfcEntity != null)
                    {
                        var manuSfcInfoEntity = manuSfcInfolist.FirstOrDefault(x => x.SfcId == manuSfcEntity.Id);
                        if (manuSfcInfoEntity != null)
                        {
                            var manuSfcProduceEntity = manuSfcProduceList.FirstOrDefault(x => x.SFC == item.LadeBarCode);
                            manuSfcStepList.Add(new ManuSfcStepEntity
                            {
                                Id = IdGenProvider.Instance.CreateId(),
                                SiteId = _currentSite.SiteId ?? 0,
                                CreatedBy = _currentUser.UserName,
                                UpdatedBy = _currentUser.UserName,
                                SFC = item.LadeBarCode,
                                ProductId = manuSfcInfoEntity.ProductId,
                                WorkOrderId = manuSfcInfoEntity.WorkOrderId ?? 0,
                                ResourceId = param.ResourceId,
                                ProcedureId = param.ProcedureId,
                                Operatetype = ManuSfcStepTypeEnum.Unpack,
                                Qty = manuSfcEntity.Qty,
                                WorkCenterId = manuSfcProduceEntity == null ? null : manuSfcProduceEntity.WorkCenterId,
                                ProductBOMId = manuSfcProduceEntity == null ? null : manuSfcProduceEntity.WorkCenterId,
                                CurrentStatus = manuSfcProduceEntity == null ? SfcStatusEnum.Complete : manuSfcProduceEntity.Status
                            });
                        }
                    }
                }
            }
            //生成删除记录
            using (TransactionScope ts = TransactionHelper.GetTransactionScope())
            {
                if (manuSfcStepList != null && manuSfcStepList.Any())
                {
                    await _manuSfcStepRepository.InsertRangeAsync(manuSfcStepList);
                }
                await _manuContainerPackRecordRepository.InsertsAsync(lst);
                await _manuContainerPackRepository.DeleteAllAsync(param.ContainerBarCodeId);
                ts.Complete();
            }
            //物理删除
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="manuContainerPackPagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ManuContainerPackDto>> GetPagedListAsync(ManuContainerPackPagedQueryDto manuContainerPackPagedQueryDto)
        {
            var manuContainerPackPagedQuery = manuContainerPackPagedQueryDto.ToQuery<ManuContainerPackPagedQuery>();
            manuContainerPackPagedQuery.SiteId = _currentSite.SiteId ?? 0;
            var pagedInfo = await _manuContainerPackRepository.GetPagedInfoAsync(manuContainerPackPagedQuery);


            //实体到DTO转换 装载数据
            List<ManuContainerPackDto> manuContainerPackDtos = await PrepareManuContainerPackDtosAsync(pagedInfo);

            var containerPackEntities = new List<ManuContainerPackEntity>();
            var barCodes = manuContainerPackDtos.Select(x => x.LadeBarCode).ToArray();
            IEnumerable<ManuContainerBarcodeEntity> packBarCodesEntities = new List<ManuContainerBarcodeEntity>();
            if (barCodes.Any())
            {
                packBarCodesEntities = await _manuContainerBarcodeRepository.GetByCodesAsync(new ManuContainerBarcodeQuery
                {
                    BarCodes = barCodes,
                    SiteId = _currentSite.SiteId ?? 0
                });
                if (packBarCodesEntities.Any())
                {
                    var packBarCodeIds = packBarCodesEntities.Select(x => x.Id).ToArray();
                    containerPackEntities = (await _manuContainerPackRepository.GetByContainerBarCodeIdsAsync(packBarCodeIds, _currentSite.SiteId ?? 0)).ToList();
                }
            }

            foreach (var item in manuContainerPackDtos)
            {
                var barCode = packBarCodesEntities?.FirstOrDefault(x => x.BarCode == item.LadeBarCode);
                var count = barCode == null ? 1 : containerPackEntities.Count(x => x.ContainerBarCodeId == barCode?.Id);
                item.Count = count;
            }
            return new PagedInfo<ManuContainerPackDto>(manuContainerPackDtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pagedInfo"></param>
        /// <returns></returns>
        private async Task<List<ManuContainerPackDto>> PrepareManuContainerPackDtosAsync(PagedInfo<ManuContainerPackView> pagedInfo)
        {
            var manuContainerPackDtos = new List<ManuContainerPackDto>();
            //工单信息
            IEnumerable<PlanWorkOrderEntity> planWorkOrderList = new List<PlanWorkOrderEntity>();
            //批量查询物料信息
            IEnumerable<ProcMaterialEntity> procMaterialList = new List<ProcMaterialEntity>();
            if (pagedInfo.Data.Any())
            {
                var productIds = pagedInfo.Data.Select(c => c.ProductId).ToArray();
                if (productIds.Any())
                {
                    procMaterialList = await _procMaterialRepository.GetByIdsAsync(productIds);
                }

                //批量查询工单信息
                var workOrderIds = pagedInfo.Data.Select(c => c.WorkOrderId).ToArray();
                if (workOrderIds.Any())
                {
                    planWorkOrderList = await _planWorkOrderRepository.GetByIdsAsync(workOrderIds);
                }
            }

            //转换Dto
            foreach (var manuContainerPackView in pagedInfo.Data)
            {
                var manuContainerPackDto = manuContainerPackView.ToModel<ManuContainerPackDto>();
                //转换物料编码
                var procMater = procMaterialList.Where(c => c.Id == manuContainerPackView.ProductId)?.FirstOrDefault();
                if (procMater != null)
                {
                    manuContainerPackDto.MaterialCode = procMater.MaterialCode;
                }
                //转换工单编码
                var planWorkOrder = planWorkOrderList.Where(c => c.Id == manuContainerPackView.WorkOrderId)?.FirstOrDefault();
                if (planWorkOrder != null)
                {
                    manuContainerPackDto.WorkOrderCode = planWorkOrder.OrderCode;
                }

                manuContainerPackDtos.Add(manuContainerPackDto);
            }

            return manuContainerPackDtos;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="manuContainerPackModifyDto"></param>
        /// <returns></returns>
        public async Task ModifyManuContainerPackAsync(ManuContainerPackModifyDto manuContainerPackModifyDto)
        {
            //验证DTO
            await _validationModifyRules.ValidateAndThrowAsync(manuContainerPackModifyDto);

            //DTO转换实体
            var manuContainerPackEntity = manuContainerPackModifyDto.ToEntity<ManuContainerPackEntity>();
            manuContainerPackEntity.UpdatedBy = _currentUser.UserName;
            manuContainerPackEntity.UpdatedOn = HymsonClock.Now();

            await _manuContainerPackRepository.UpdateAsync(manuContainerPackEntity);
        }

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ManuContainerPackDto> QueryManuContainerPackByIdAsync(long id)
        {
            var manuContainerPackEntity = await _manuContainerPackRepository.GetByIdAsync(id);
            if (manuContainerPackEntity != null)
            {
                return manuContainerPackEntity.ToModel<ManuContainerPackDto>();
            }
            return new ManuContainerPackDto();
        }


        /// <summary>
        /// 执行作业
        /// </summary>
        /// <param name="manuFacePlateContainerPackExJobDto"></param>
        /// <returns></returns>
        /// <exception cref="CustomerValidationException"></exception>
        public async Task<Dictionary<string, JobResponseBo>> ExecuteJobAsync(ManuFacePlateContainerPackExJobDto manuFacePlateContainerPackExJobDto)
        {
            #region  验证数据

            #endregion

            #region 调用作业
            manuFacePlateContainerPackExJobDto.SFC = manuFacePlateContainerPackExJobDto.SFC.Trim();
            var jobDto = new ButtonRequestDto
            {
                FacePlateId = manuFacePlateContainerPackExJobDto.FacePlateId,
                FacePlateButtonId = manuFacePlateContainerPackExJobDto.FacePlateButtonId,
                Param = new Dictionary<string, string>()
            };
            var sfcs = new List<string>() { manuFacePlateContainerPackExJobDto.SFC };
            JobRequestBo bo = new()
            {
                SFCs = sfcs,
                ProcedureId = manuFacePlateContainerPackExJobDto.ProcedureId,
                ResourceId = manuFacePlateContainerPackExJobDto.ResourceId,
                ContainerId = manuFacePlateContainerPackExJobDto.ContainerId,
                SiteId = _currentSite.SiteId ?? 0,
                UserName = _currentUser.UserName
            };

            // 调用作业
            var resJob = await _manuFacePlateButtonService.NewClickAsync(jobDto, bo);
            if (resJob == null || !resJob.Any()) throw new CustomerValidationException(nameof(ErrorCode.MES16709));
            return resJob;
            #endregion
        }
    }
}
