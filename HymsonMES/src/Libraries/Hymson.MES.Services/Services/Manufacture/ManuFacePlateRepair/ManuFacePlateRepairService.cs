using FluentValidation;
using FluentValidation.Results;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.Localization.Services;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Constants.Process;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.CoreServices.Bos.Job;
using Hymson.MES.CoreServices.Bos.Manufacture;
using Hymson.MES.CoreServices.Dtos.Common;
using Hymson.MES.CoreServices.Services.Common;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Manufacture.ManuFacePlateRepair.Query;
using Hymson.MES.Data.Repositories.Plan;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.Warehouse;
using Hymson.MES.Data.Repositories.Warehouse.WhMaterialInventory.Query;
using Hymson.MES.Services.Dtos.Manufacture;
using Hymson.MES.Services.Services.Manufacture.ManuMainstreamProcess.OutStation;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;
using System.Text.Json;

namespace Hymson.MES.Services.Services.Manufacture
{
    /// <summary>
    /// 在制品维修 服务
    /// </summary>
    public class ManuFacePlateRepairService : IManuFacePlateRepairService
    {
        private readonly ICurrentUser _currentUser;
        private readonly ICurrentSite _currentSite;

        /// <summary>
        /// 在制品维修 仓储
        /// </summary>
        private readonly IManuFacePlateRepairRepository _manuFacePlateRepairRepository;

        /// <summary>
        /// 条码生产信息（物理删除） 仓储 
        /// </summary>
        private readonly IManuSfcProduceRepository _manuSfcProduceRepository;

        /// <summary>
        /// 物料库存 仓储
        /// </summary>
        private readonly IWhMaterialInventoryRepository _whMaterialInventoryRepository;

        /// <summary>
        /// 工单信息表 仓储
        /// </summary>
        private readonly IPlanWorkOrderRepository _planWorkOrderRepository;

        /// <summary>
        /// 工序表 仓储
        /// </summary>
        private readonly IProcProcedureRepository _procProcedureRepository;

        /// <summary>
        /// 资源表 仓储
        /// </summary>
        private readonly IProcResourceRepository _procResourceRepository;

        /// <summary>
        /// 物料维护 仓储
        /// </summary>
        private readonly IProcMaterialRepository _procMaterialRepository;

        /// <summary>
        /// 产品不良录入 仓储
        /// </summary>
        private readonly IManuProductBadRecordRepository _manuProductBadRecordRepository;

        /// <summary>
        /// 仓储（工艺路线节点）
        /// </summary>
        private readonly IProcProcessRouteDetailNodeRepository _procProcessRouteNodeRepository;

        /// <summary>
        /// 接口（操作面板按钮）
        /// </summary>
        private readonly IManuFacePlateButtonService _manuFacePlateButtonService;

        /// <summary>
        /// 接口（出站）
        /// </summary>
        private readonly IManuOutStationService _manuOutStationService;

        /// <summary>
        /// 仓储接口（工艺路线工序连线）
        /// </summary>
        private readonly IProcProcessRouteDetailLinkRepository _procProcessRouteDetailLinkRepository;

        /// <summary>
        /// 服务接口（生产通用）
        /// </summary>
        private readonly IManuCommonService _manuCommonService;

        /// <summary>
        /// 容器装载表（物理删除）仓储
        /// </summary>
        private readonly IManuContainerPackRepository _manuContainerPackRepository;
        /// <summary>
        /// 仓储接口（条码步骤）
        /// </summary>
        private readonly IManuSfcStepRepository _manuSfcStepRepository;

        /// <summary>
        /// 仓储接口（条码）
        /// </summary>
        private readonly IManuSfcRepository _manuSfcRepository;

        private readonly ILocalizationService _localizationService;
        private readonly AbstractValidator<ManuFacePlateRepairCreateDto> _validationCreateRules;
        private readonly AbstractValidator<ManuFacePlateRepairModifyDto> _validationModifyRules;


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="currentSite"></param>
        /// <param name="manuFacePlateRepairRepository"></param>
        /// <param name="manuSfcProduceRepository"></param>
        /// <param name="whMaterialInventoryRepository"></param>
        /// <param name="planWorkOrderRepository"></param>
        /// <param name="procProcedureRepository"></param>
        /// <param name="procMaterialRepository"></param>
        /// <param name="manuProductBadRecordRepository"></param>
        /// <param name="procResourceRepository"></param>
        /// <param name="procProcessRouteNodeRepository"></param>
        /// <param name="manuFacePlateButtonService"></param>
        /// <param name="manuOutStationService"></param>
        /// <param name="localizationService"></param>
        /// <param name="manuSfcStepRepository"></param>
        /// <param name="manuSfcRepository"></param>
        /// <param name="validationCreateRules"></param>
        /// <param name="validationModifyRules"></param>
        /// <param name="procProcessRouteDetailLinkRepository"></param>
        /// <param name="manuCommonService"></param>
        /// <param name="manuContainerPackRepository"></param>
        public ManuFacePlateRepairService(ICurrentUser currentUser, ICurrentSite currentSite,
            IManuFacePlateRepairRepository manuFacePlateRepairRepository, IManuSfcProduceRepository manuSfcProduceRepository,
            IWhMaterialInventoryRepository whMaterialInventoryRepository, IPlanWorkOrderRepository planWorkOrderRepository,
            IProcProcedureRepository procProcedureRepository, IProcMaterialRepository procMaterialRepository,
            IManuProductBadRecordRepository manuProductBadRecordRepository, IProcResourceRepository procResourceRepository,
            IProcProcessRouteDetailNodeRepository procProcessRouteNodeRepository, IManuFacePlateButtonService manuFacePlateButtonService,
            IManuOutStationService manuOutStationService, ILocalizationService localizationService, IManuSfcStepRepository manuSfcStepRepository,
            IManuSfcRepository manuSfcRepository,
        AbstractValidator<ManuFacePlateRepairCreateDto> validationCreateRules,
        AbstractValidator<ManuFacePlateRepairModifyDto> validationModifyRules,
        IProcProcessRouteDetailLinkRepository procProcessRouteDetailLinkRepository,
        IManuCommonService manuCommonService,
        IManuContainerPackRepository manuContainerPackRepository)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _manuFacePlateRepairRepository = manuFacePlateRepairRepository;
            _manuSfcProduceRepository = manuSfcProduceRepository;
            _whMaterialInventoryRepository = whMaterialInventoryRepository;
            _planWorkOrderRepository = planWorkOrderRepository;
            _procProcedureRepository = procProcedureRepository;
            _procMaterialRepository = procMaterialRepository;
            _manuProductBadRecordRepository = manuProductBadRecordRepository;
            _procResourceRepository = procResourceRepository;
            _procProcessRouteNodeRepository = procProcessRouteNodeRepository;
            _manuFacePlateButtonService = manuFacePlateButtonService;
            _manuOutStationService = manuOutStationService;
            _validationCreateRules = validationCreateRules;
            _validationModifyRules = validationModifyRules;
            _localizationService = localizationService;
            _procProcessRouteDetailLinkRepository = procProcessRouteDetailLinkRepository;
            _manuCommonService = manuCommonService;
            _manuContainerPackRepository = manuContainerPackRepository;
            _manuSfcStepRepository = manuSfcStepRepository;
            _manuSfcRepository = manuSfcRepository;
        }

        /// <summary>
        /// 执行作业
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<List<ManuFacePlateRepairButJobReturnTypeEnum>> ExecuteJobAsync(ManuFacePlateRepairExJobDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.SFC)) throw new CustomerValidationException(nameof(ErrorCode.MES17303));

            #region 调用作业
            var jobDto = new ButtonRequestDto
            {
                FacePlateId = dto.FacePlateId,
                FacePlateButtonId = dto.FacePlateButtonId
            };

            JobRequestBo bo = new()
            {
                SiteId = _currentSite.SiteId ?? 0,
                UserName = _currentUser.UserName,
                ProcedureId = dto.ProcedureId,
                ResourceId = dto.ResourceId,
                SFCs = new string[] { dto.SFC },   // 这句后面要改
                PanelRequestBos = new List<PanelRequestBo> { new PanelRequestBo { SFC = dto.SFC } },
                InStationRequestBos = new List<InStationRequestBo> { new InStationRequestBo { SFC = dto.SFC } },
                OutStationRequestBos = new List<OutStationRequestBo> { new OutStationRequestBo { SFC = dto.SFC } }
            };

            var resJob = await _manuFacePlateButtonService.NewClickAsync(jobDto, bo);
            if (resJob == null || !resJob.Any()) throw new CustomerValidationException(nameof(ErrorCode.MES17320));

            var list = new List<ManuFacePlateRepairButJobReturnTypeEnum>();
            foreach (var item in resJob.Select(x => x.Key))
            {
                if (item == ManuFacePlateRepairButJobReturnTypeEnum.RepairStartJobService.ToString())
                {
                    if (!list.Contains(ManuFacePlateRepairButJobReturnTypeEnum.RepairStartJobService))
                        list.Add(ManuFacePlateRepairButJobReturnTypeEnum.RepairStartJobService);
                }
                else if (item == ManuFacePlateRepairButJobReturnTypeEnum.RepairEndJobService.ToString())
                {
                    if (!list.Contains(ManuFacePlateRepairButJobReturnTypeEnum.RepairEndJobService))
                        list.Add(ManuFacePlateRepairButJobReturnTypeEnum.RepairEndJobService);
                }
                else
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES17321)).WithData("key", item);
                }
            }

            return list;

            #endregion
        }


        /// <summary>
        /// 开始维修
        /// </summary>
        /// <param name="beginRepairDto"></param> 
        /// <returns></returns>
        public async Task<ManuFacePlateRepairOpenInfoDto> BeginManuFacePlateRepairAsync(ManuFacePlateRepairBeginRepairDto beginRepairDto)
        {
            #region 验证条码更新在制信息
            if (string.IsNullOrWhiteSpace(beginRepairDto.SFC)) throw new CustomerValidationException(nameof(ErrorCode.MES17303));

            // 验证条码
            beginRepairDto.SFC = beginRepairDto.SFC.Trim();
            var sfcProduceEntity = await _manuSfcProduceRepository.GetBySFCAsync(new ManuSfcProduceBySfcQuery()
            {
                SiteId = _currentSite.SiteId ?? 0,
                Sfc = beginRepairDto.SFC
            });
            if (sfcProduceEntity == null)
            {
                // 是否已入库完成 待使用算完成
                var whMaterialInventoryEntity = await _whMaterialInventoryRepository.GetByBarCodeAsync(new WhMaterialInventoryBarCodeQuery
                {
                    SiteId = _currentSite.SiteId,
                    BarCode = beginRepairDto.SFC
                }) ?? throw new CustomerValidationException(nameof(ErrorCode.MES16306));

                throw whMaterialInventoryEntity.Status switch
                {
                    WhMaterialInventoryStatusEnum.ToBeUsed => new CustomerValidationException(nameof(ErrorCode.MES16310)),
                    _ => new CustomerValidationException(nameof(ErrorCode.MES16311)).WithData("Status", whMaterialInventoryEntity.Status.ToString()),
                };
            }
            #endregion

            // 当前条码是否是已报废
            if (sfcProduceEntity.IsScrap == TrueOrFalseEnum.Yes) throw new CustomerValidationException(nameof(ErrorCode.MES16322)).WithData("SFC", sfcProduceEntity.SFC);

            // 这方法里面包含有验证
            var manuFacePlateRepairOpenInfoDto = await GetManuFacePlateRepairOpenInfoDtoAsync(sfcProduceEntity);

            // 初始化步骤
            var sfcStep = new ManuSfcStepEntity
            {
                Id = IdGenProvider.Instance.CreateId(),
                SiteId = sfcProduceEntity.SiteId,
                SFC = sfcProduceEntity.SFC,
                ProductId = sfcProduceEntity.ProductId,
                WorkOrderId = sfcProduceEntity.WorkOrderId,
                WorkCenterId = sfcProduceEntity.WorkCenterId,
                ProductBOMId = sfcProduceEntity.ProductBOMId,
                ProcedureId = sfcProduceEntity.ProcedureId,
                Qty = sfcProduceEntity.Qty,
                IsRepair = TrueOrFalseEnum.Yes,
                Operatetype = ManuSfcStepTypeEnum.Repair,
                CurrentStatus = SfcStatusEnum.Activity,
                EquipmentId = sfcProduceEntity.EquipmentId,
                ResourceId = sfcProduceEntity.ResourceId,
                CreatedBy = _currentUser.UserName,
                CreatedOn = HymsonClock.Now(),
                UpdatedBy = _currentUser.UserName,
                UpdatedOn = HymsonClock.Now(),
            };

            // 当前状态
            var currentStatus = sfcProduceEntity.Status;

            // 更改状态，将条码由"排队"改为"活动"
            sfcProduceEntity.Status = SfcStatusEnum.Activity;
            sfcProduceEntity.UpdatedBy = _currentUser.UserName;
            sfcProduceEntity.UpdatedOn = HymsonClock.Now();

            ManuSfcUpdateStatusByIdCommand manuSfcUpdateStatusByIdCommand = new ManuSfcUpdateStatusByIdCommand
            {
                Id = sfcProduceEntity.SFCId,
                CurrentStatus = currentStatus,
                Status = SfcStatusEnum.Activity,
                UpdatedBy = _currentUser.UserName,
                UpdatedOn = HymsonClock.Now(),
            };

            using (var trans = TransactionHelper.GetTransactionScope())
            {
                var rows = await _manuSfcProduceRepository.UpdateAsync(sfcProduceEntity);

                rows += await _manuSfcStepRepository.InsertAsync(sfcStep);

                rows += await _manuSfcRepository.ManuSfcUpdateStatuByIdAsync(manuSfcUpdateStatusByIdCommand);

                trans.Complete();
            }
            return manuFacePlateRepairOpenInfoDto;
        }

        /// <summary>
        /// 获取展示信息（重写于 GetManuFacePlateRepairOpenInfoDto 方法）
        /// </summary>
        /// <param name="manuSfcProduceEntit"></param>
        /// <returns></returns>
        private async Task<ManuFacePlateRepairOpenInfoDto> GetManuFacePlateRepairOpenInfoDtoAsync(ManuSfcProduceEntity manuSfcProduceEntit)
        {
            // 获取产品信息
            var manuSfcProduce = await _manuSfcProduceRepository.GetBySFCAsync(new ManuSfcProduceBySfcQuery { Sfc = manuSfcProduceEntit.SFC, SiteId = _currentSite.SiteId ?? 0 })
                ?? throw new CustomerValidationException(nameof(ErrorCode.MES17306));

            // 工单
            var planWorkOrderEntity = await _planWorkOrderRepository.GetByIdAsync(manuSfcProduceEntit.WorkOrderId)
                ?? throw new CustomerValidationException(nameof(ErrorCode.MES17313));

            // 工序
            var procProcedureEntity = await _procProcedureRepository.GetByIdAsync(manuSfcProduceEntit.ProcedureId)
                ?? throw new CustomerValidationException(nameof(ErrorCode.MES17311));

            // 产品
            var procMaterialEntity = await _procMaterialRepository.GetByIdAsync(manuSfcProduceEntit.ProductId)
                ?? throw new CustomerValidationException(nameof(ErrorCode.MES17314));

            // 验证条码锁定
            await _manuCommonService.VerifySfcsLockAsync(new ManuProcedureBo
            {
                SiteId = _currentSite.SiteId ?? 0,
                SFCs = new List<string> { manuSfcProduceEntit.SFC },
                ProcedureId = manuSfcProduceEntit.ProcedureId
            });

            if (manuSfcProduceEntit.IsScrap == TrueOrFalseEnum.Yes)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16322)).WithData("SFC", manuSfcProduceEntit.SFC);
            }
            //包装
            var conPackList = await _manuContainerPackRepository.GetByLadeBarCodesAsync(new ManuContainerPackQuery { LadeBarCodes = new string[] { manuSfcProduceEntit.SFC }, SiteId = _currentSite.SiteId ?? 0 });
            if (conPackList != null && conPackList.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES18019)).WithData("SFCs", string.Join(",", conPackList.Select(it => it.LadeBarCode).ToArray()));
            }
            // 产品信息
            ManuFacePlateRepairOpenInfoDto manuFacePlateRepairOpenInfoDto = new()
            {
                productInfo = new ManuFacePlateRepairProductInfoDto
                {
                    SFC = manuSfcProduceEntit.SFC,
                    Status = manuSfcProduceEntit.Status == SfcStatusEnum.lineUp ? _localizationService.GetResource(nameof(ErrorCode.MES17323)) : _localizationService.GetResource(nameof(ErrorCode.MES17324)),// manuSfcProduceEntit.Status.ToString(),
                    ProcedureCode = procProcedureEntity.Code,
                    OrderCode = planWorkOrderEntity.OrderCode,
                    MaterialCode = procMaterialEntity.MaterialCode,
                    Version = procMaterialEntity.Version,
                    MaterialName = procMaterialEntity.MaterialName,
                }
            };

            // 获取条码不合格信息
            var manuProductBads = await _manuProductBadRecordRepository.GetBadRecordsBySfcAsync(new ManuProductBadRecordQuery
            {
                SFC = manuSfcProduceEntit.SFC,
                Status = ProductBadRecordStatusEnum.Open,
                SiteId = _currentSite.SiteId ?? 0
            });
            if (manuProductBads != null && manuProductBads.Any())
            {
                var manuSfcRepairDetailList = await _manuFacePlateRepairRepository.ManuSfcRepairDetailByProductBadIdAsync(new ManuSfcRepairDetailByProductBadIdQuery
                {
                    ProductBadId = manuProductBads.Select(it => it.Id).ToArray(),
                    SiteId = _currentSite.SiteId ?? 0
                });

                // 组装不合格数据
                var manuFacePlateRepairProductBadInfoList = manuProductBads.Select(s => new ManuFacePlateRepairProductBadInfoDto
                {
                    BadRecordId = s.Id,
                    UnqualifiedId = s.UnqualifiedId,
                    UnqualifiedCode = s.UnqualifiedCode,
                    UnqualifiedCodeName = s.UnqualifiedCodeName,
                    Type = s.Type,
                    CauseAnalyse = manuSfcRepairDetailList == null ? "" : manuSfcRepairDetailList.FirstOrDefault(it => it.ProductBadId == s.Id)?.CauseAnalyse ?? "",
                    RepairMethod = manuSfcRepairDetailList == null ? "" : manuSfcRepairDetailList.FirstOrDefault(it => it.ProductBadId == s.Id)?.RepairMethod ?? "",
                    ResCode = s.ResCode,
                    IsClose = s.Status,
                });
                manuFacePlateRepairOpenInfoDto.productBadInfo = manuFacePlateRepairProductBadInfoList;
                manuFacePlateRepairOpenInfoDto.productInfo.badInfoDtos = manuFacePlateRepairProductBadInfoList;
            }
            // 获取维修业务
            var sfcProduceBusinessEntity = await _manuSfcProduceRepository.GetSfcProduceBusinessBySFCAsync(new SfcProduceBusinessQuery
            {
                SiteId = _currentSite.SiteId ?? 0,
                Sfc = manuSfcProduceEntit.SFC,
                BusinessType = ManuSfcProduceBusinessType.Repair
            }) ?? throw new CustomerValidationException(nameof(ErrorCode.MES17325));
            var sfcProduceRepairBo = JsonSerializer.Deserialize<SfcProduceRepairBo>(sfcProduceBusinessEntity.BusinessContent);
            if (sfcProduceRepairBo == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES17326));
            }

            // 工艺路线节点
            var procProcessRouteNodeList = await _procProcessRouteNodeRepository.GetListAsync(new ProcProcessRouteDetailNodeQuery
            {
                ProcessRouteId = sfcProduceRepairBo.ProcessRouteId
            });

            List<ManuFacePlateRepairReturnProcedureDto> manuFacePlateRepairReturnProcedureList = new();
            var procProcedureEntities = await _procProcedureRepository.GetByIdsAsync(procProcessRouteNodeList.Select(s => s.ProcedureId).ToArray());
            foreach (var itemNode in procProcessRouteNodeList.Select(x => x.ProcedureId))
            {
                procProcedureEntity = procProcedureEntities.FirstOrDefault(f => f.Id == itemNode);
                if (procProcedureEntity == null) continue;
                if (procProcedureEntity.IsRepairReturn != 1) continue;

                manuFacePlateRepairReturnProcedureList.Add(new ManuFacePlateRepairReturnProcedureDto
                {
                    ProcedureId = itemNode,
                    ProcedureCode = procProcedureEntity.Code
                });
            }

            manuFacePlateRepairOpenInfoDto.returnProcedureInfo = manuFacePlateRepairReturnProcedureList;

            //尾工序
            var endProcessRouteDetailId = await GetEndProcessRouteDetailIdAsync(manuSfcProduceEntit.ProcessRouteId);
            manuFacePlateRepairOpenInfoDto.IsReturnProcedure = manuSfcProduceEntit.ProcedureId == endProcessRouteDetailId;

            return manuFacePlateRepairOpenInfoDto;
        }

        /// <summary>
        /// 结束维修
        /// </summary>
        /// <param name="beginRepairDto"></param>
        /// <returns></returns>
        public async Task<ManuFacePlateRepairOpenInfoDto> EndManuFacePlateRepairAsync(ManuFacePlateRepairBeginRepairDto beginRepairDto)
        {
            if (string.IsNullOrWhiteSpace(beginRepairDto.SFC))
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES17303));
            }
            var manuSfcProduceEntit = await _manuSfcProduceRepository.GetBySFCAsync(new ManuSfcProduceBySfcQuery()
            {
                SiteId = _currentSite.SiteId ?? 0,
                Sfc = beginRepairDto.SFC
            }) ?? throw new CustomerValidationException(nameof(ErrorCode.MES16306));
            if (manuSfcProduceEntit.Status != SfcStatusEnum.Activity)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES17322));
            }
            beginRepairDto.SFC = beginRepairDto.SFC.Trim();
            var manuFacePlateRepairOpenInfoDto = await GetManuFacePlateRepairOpenInfoDtoAsync(manuSfcProduceEntit);
            return manuFacePlateRepairOpenInfoDto;
        }

        /// <summary>
        /// 确认提交
        /// </summary>
        /// <param name="confirmSubmitDto"></param>
        /// <returns></returns>
        public async Task ConfirmSubmitManuFacePlateRepairAsync(ManuFacePlateRepairConfirmSubmitDto confirmSubmitDto)
        {
            #region 验证数据  

            if (string.IsNullOrWhiteSpace(confirmSubmitDto.SFC))
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES17303));
            }
            confirmSubmitDto.SFC = confirmSubmitDto.SFC.Trim();


            //获取条码生产信息
            var manuSfcProduceEntit = await _manuSfcProduceRepository.GetBySFCAsync(new ManuSfcProduceBySfcQuery()
            {
                SiteId = _currentSite.SiteId ?? 0,
                Sfc = confirmSubmitDto.SFC
            });
            if (manuSfcProduceEntit == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES17306));
            }
            #endregion

            #region 工序判断
            var endProcessRouteDetailId = await GetEndProcessRouteDetailIdAsync(manuSfcProduceEntit.ProcessRouteId);
            if (confirmSubmitDto.ProcedureId == endProcessRouteDetailId && confirmSubmitDto.ReturnProcedureId <= 0)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES17318));
            }
            #endregion

            #region 数据组装
            var manuSfcRepairRecordEntity = await _manuFacePlateRepairRepository.GetManuSfcRepairBySFCAsync(new GetManuSfcRepairBySfcQuery { SFC = confirmSubmitDto.SFC, SiteId = _currentSite.SiteId ?? 0 });
            //维修记录
            bool isAddmanuSfcRepairRecordEntity = false;
            if (manuSfcRepairRecordEntity == null)
            {
                isAddmanuSfcRepairRecordEntity = true;
                manuSfcRepairRecordEntity = new ManuSfcRepairRecordEntity();
                //维修记录
                manuSfcRepairRecordEntity.SFC = confirmSubmitDto.SFC;
                manuSfcRepairRecordEntity.WorkOrderId = manuSfcProduceEntit.WorkOrderId;
                manuSfcRepairRecordEntity.ProductId = manuSfcProduceEntit.ProductId;
                manuSfcRepairRecordEntity.ResourceId = confirmSubmitDto.ResourceId;
                manuSfcRepairRecordEntity.ProcedureId = confirmSubmitDto.ProcedureId;
                manuSfcRepairRecordEntity.ReturnProcedureId = confirmSubmitDto.ReturnProcedureId;
                manuSfcRepairRecordEntity.Remark = confirmSubmitDto.Remark;

                manuSfcRepairRecordEntity.Id = IdGenProvider.Instance.CreateId();
                manuSfcRepairRecordEntity.CreatedBy = _currentUser.UserName;
                manuSfcRepairRecordEntity.UpdatedBy = _currentUser.UserName;
                manuSfcRepairRecordEntity.CreatedOn = HymsonClock.Now();
                manuSfcRepairRecordEntity.UpdatedOn = HymsonClock.Now();
                manuSfcRepairRecordEntity.SiteId = _currentSite.SiteId ?? 0;
            }


            //不良录入
            var badRecordsList = new List<ManuProductBadRecordEntity>();
            var createManuSfcRepairDetailList = new List<ManuSfcRepairDetailEntity>();
            var updateManuSfcRepairDetailList = new List<ManuSfcRepairDetailEntity>();
            var BadRecordIds = confirmSubmitDto.confirmSubmitDetail.Select(it => it.BadRecordId).ToArray();
            var badRecordList = await _manuProductBadRecordRepository.GetByIdsAsync(BadRecordIds);

            var sfcProduceRepairBo = await GetSfcProduceRepairBoAsync(manuSfcProduceEntit.SFC);

            var GetManuSfcRepairDetails = await _manuFacePlateRepairRepository.ManuSfcRepairDetailByProductBadIdAsync(new ManuSfcRepairDetailByProductBadIdQuery
            {
                ProductBadId = BadRecordIds,
                SiteId = _currentSite.SiteId ?? 0
            });

            var updateCommandList = new List<ManuProductBadRecordUpdateCommand>();

            if (badRecordList != null && badRecordList.Any())
            {
                var validationFailures = new List<ValidationFailure>();
                foreach (var item in confirmSubmitDto.confirmSubmitDetail)
                {
                    var validationFailure = new ValidationFailure();
                    var badRecordEntit = badRecordList.FirstOrDefault(it => it.Id == item.BadRecordId);
                    if (badRecordEntit == null)
                    {
                        if (validationFailure.FormattedMessagePlaceholderValues == null || !validationFailure.FormattedMessagePlaceholderValues.Any())
                        {
                            validationFailure.FormattedMessagePlaceholderValues = new Dictionary<string, object> {
                            { "CollectionIndex", item.UnqualifiedCode}
                        };
                        }
                        else
                        {
                            validationFailure.FormattedMessagePlaceholderValues.Add("CollectionIndex", item.UnqualifiedCode);
                        }
                        validationFailure.ErrorCode = nameof(ErrorCode.MES17316);
                        validationFailures.Add(validationFailure);
                        continue;
                    }

                    if (confirmSubmitDto.ProcedureId == endProcessRouteDetailId && item.IsClose == ProductBadRecordStatusEnum.Open)
                    {
                        if (validationFailure.FormattedMessagePlaceholderValues == null || !validationFailure.FormattedMessagePlaceholderValues.Any())
                        {
                            validationFailure.FormattedMessagePlaceholderValues = new Dictionary<string, object>
                                {
                                    { "CollectionIndex", item.UnqualifiedCode}
                                };
                        }
                        else
                        {
                            validationFailure.FormattedMessagePlaceholderValues.Add("CollectionIndex", item.UnqualifiedCode);
                        }
                        validationFailure.ErrorCode = nameof(ErrorCode.MES17307);
                        validationFailures.Add(validationFailure);
                        continue;
                    }

                    if (item.IsClose == ProductBadRecordStatusEnum.Close)
                    {
                        if (string.IsNullOrWhiteSpace(item.CauseAnalyse))
                        {
                            if (validationFailure.FormattedMessagePlaceholderValues == null || !validationFailure.FormattedMessagePlaceholderValues.Any())
                            {
                                validationFailure.FormattedMessagePlaceholderValues = new Dictionary<string, object> {
                            { "CollectionIndex", item.UnqualifiedCode}
                        };
                            }
                            else
                            {
                                validationFailure.FormattedMessagePlaceholderValues.Add("CollectionIndex", item.UnqualifiedCode);
                            }

                            validationFailure.ErrorCode = nameof(ErrorCode.MES17329);
                            validationFailures.Add(validationFailure);
                            continue;
                        }
                        if (string.IsNullOrWhiteSpace(item.RepairMethod))
                        {

                            if (validationFailure.FormattedMessagePlaceholderValues == null || !validationFailure.FormattedMessagePlaceholderValues.Any())
                            {
                                validationFailure.FormattedMessagePlaceholderValues = new Dictionary<string, object> {
                            { "CollectionIndex", item.UnqualifiedCode}
                        };
                            }
                            else
                            {
                                validationFailure.FormattedMessagePlaceholderValues.Add("CollectionIndex", item.UnqualifiedCode);
                            }
                            validationFailure.ErrorCode = nameof(ErrorCode.MES17330);
                            validationFailures.Add(validationFailure);
                            continue;
                        }
                        updateCommandList.Add(new ManuProductBadRecordUpdateCommand
                        {
                            SiteId = _currentSite.SiteId ?? 0,
                            Id = item.BadRecordId,
                            DisposalResult = ProductBadDisposalResultEnum.Repair,
                            Remark = confirmSubmitDto.Remark ?? "",
                            Status = item.IsClose,// ProductBadRecordStatusEnum.Close,
                            UserId = _currentUser.UserName,
                            UpdatedOn = HymsonClock.Now(),
                        });

                        badRecordEntit.Status = item.IsClose;
                        badRecordEntit.UpdatedBy = _currentUser.UserName;
                        badRecordEntit.UpdatedOn = HymsonClock.Now();
                        badRecordsList.Add(badRecordEntit);
                    }

                    var isClose = item.IsClose == ProductBadRecordStatusEnum.Close ? ManuSfcRepairDetailIsIsCloseEnum.Close : ManuSfcRepairDetailIsIsCloseEnum.Open;
                    //维修明细
                    if (GetManuSfcRepairDetails != null && GetManuSfcRepairDetails.Any())
                    {
                        var detail = GetManuSfcRepairDetails.FirstOrDefault(it => it.ProductBadId == item.BadRecordId && it.SfcRepairId == manuSfcRepairRecordEntity.Id);
                        if (detail != null)
                        {
                            detail.RepairMethod = item.RepairMethod!;
                            detail.CauseAnalyse = item.CauseAnalyse!;
                            detail.IsClose = isClose;
                            detail.UpdatedBy = _currentUser.UserName;
                            detail.UpdatedOn = HymsonClock.Now();

                            updateManuSfcRepairDetailList.Add(detail);
                            continue;
                        }
                    }

                    //维修明细
                    ManuSfcRepairDetailEntity manuSfcRepairDetailEntity = new ManuSfcRepairDetailEntity
                    {
                        SfcRepairId = manuSfcRepairRecordEntity.Id,
                        ProductBadId = item.BadRecordId,
                        RepairMethod = item.RepairMethod!,
                        CauseAnalyse = item.CauseAnalyse!,
                        IsClose = isClose,

                        Id = IdGenProvider.Instance.CreateId(),
                        CreatedBy = _currentUser.UserName,
                        UpdatedBy = _currentUser.UserName,
                        CreatedOn = HymsonClock.Now(),
                        UpdatedOn = HymsonClock.Now(),
                        SiteId = _currentSite.SiteId ?? 0
                    };
                    createManuSfcRepairDetailList.Add(manuSfcRepairDetailEntity);
                }
                if (validationFailures.Any())
                {
                    throw new ValidationException(_localizationService.GetResource(nameof(ErrorCode.MES17328)), validationFailures);
                }
            }

            // 返回工序 
            var updateProcedureCommand = new UpdateProcedureCommand
            {
                Id = manuSfcProduceEntit.Id,
                Status = SfcStatusEnum.lineUp,
                ProcedureId = confirmSubmitDto.ReturnProcedureId,
                ResourceId = null, //resourcesId, //资源这里 直接设置为null 为null生产不检测匹配工序
                ProcessRouteId = sfcProduceRepairBo.ProcessRouteId,
                IsRepair = TrueOrFalseEnum.No,
                UserId = _currentUser.UserName,
                UpdatedOn = HymsonClock.Now()
            };

            // 初始化步骤
            var sfcStep = new ManuSfcStepEntity
            {
                Id = IdGenProvider.Instance.CreateId(),
                SiteId = manuSfcProduceEntit.SiteId,
                SFC = manuSfcProduceEntit.SFC,
                ProductId = manuSfcProduceEntit.ProductId,
                Remark = confirmSubmitDto.Remark,
                WorkOrderId = manuSfcProduceEntit.WorkOrderId,
                WorkCenterId = manuSfcProduceEntit.WorkCenterId,
                ProductBOMId = manuSfcProduceEntit.ProductBOMId,
                ProcedureId = confirmSubmitDto.ReturnProcedureId,
                Qty = manuSfcProduceEntit.Qty,
                IsRepair = TrueOrFalseEnum.Yes,
                Operatetype = ManuSfcStepTypeEnum.RepairReturn,
                CurrentStatus = SfcStatusEnum.lineUp,
                EquipmentId = manuSfcProduceEntit.EquipmentId,
                ResourceId = manuSfcProduceEntit.ResourceId,
                CreatedBy = _currentUser.UserName,
                CreatedOn = HymsonClock.Now(),
                UpdatedBy = _currentUser.UserName,
                UpdatedOn = HymsonClock.Now(),
            };

            ManuSfcUpdateStatusByIdCommand manuSfcUpdateStatusByIdCommand = new ManuSfcUpdateStatusByIdCommand
            {
                Id = manuSfcProduceEntit.SFCId,
                CurrentStatus = manuSfcProduceEntit.Status,
                Status = SfcStatusEnum.lineUp,
                UpdatedBy = _currentUser.UserName,
                UpdatedOn = HymsonClock.Now(),
            };
            #endregion

            #region 事务入库
            var rows = 0;
            using (var trans = TransactionHelper.GetTransactionScope())
            {
                //维修记录
                if (isAddmanuSfcRepairRecordEntity)
                {
                    rows += await _manuFacePlateRepairRepository.InsertRecordAsync(manuSfcRepairRecordEntity);
                }
                if (updateManuSfcRepairDetailList != null && updateManuSfcRepairDetailList.Any())
                {
                    //维修明细
                    rows += await _manuFacePlateRepairRepository.UpdateDetailsAsync(updateManuSfcRepairDetailList);
                }
                if (createManuSfcRepairDetailList != null && createManuSfcRepairDetailList.Any())
                {
                    //维修明细
                    rows += await _manuFacePlateRepairRepository.InsertsDetailAsync(createManuSfcRepairDetailList);
                }
                if (updateCommandList != null && updateCommandList.Any())
                {
                    //不良录入
                    rows += await _manuProductBadRecordRepository.UpdateStatusByIdRangeAsync(updateCommandList);
                }
                //出站 
                rows += await _manuOutStationService.OutStationRepiarAsync(new ManufactureRepairBo
                {
                    SFC = confirmSubmitDto.SFC,
                    ProcedureId = confirmSubmitDto.ProcedureId,
                    ReturnProcedureId = confirmSubmitDto.ReturnProcedureId,
                    ResourceId = confirmSubmitDto.ResourceId
                });
                await _manuSfcRepository.ManuSfcUpdateStatuByIdAsync(manuSfcUpdateStatusByIdCommand);
                if (confirmSubmitDto.ProcedureId == endProcessRouteDetailId)
                {
                    //返回工序
                    rows += await _manuSfcProduceRepository.UpdateProcedureIdAsync(updateProcedureCommand);
                    rows += await _manuSfcProduceRepository.DeleteSfcProduceBusinessBySfcInfoIdAsync(new DeleteSfcProduceBusinesssBySfcInfoIdCommand
                    {
                        SiteId = manuSfcProduceEntit.SiteId,
                        SfcInfoId = manuSfcProduceEntit.Id
                    });

                    // 步骤
                    rows += await _manuSfcStepRepository.InsertAsync(sfcStep);
                }

                trans.Complete();
            }
            #endregion
        }

        /// <summary>
        /// 获取初始信息
        /// </summary>
        /// <param name="facePlateId"></param>
        /// <returns></returns>
        public async Task<ManuFacePlateRepairInitialInfoDto> GetInitialInfoManuFacePlateRepairAsync(long facePlateId)
        {
            //获取面版
            var facePlateEntit = await _manuFacePlateRepairRepository.GetByFacePlateIdAsync(facePlateId);
            if (facePlateEntit == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES17309));
            }
            var procProcedureEntity = await _procProcedureRepository.GetByIdAsync(facePlateEntit.ProcedureId);
            if (procProcedureEntity == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES17311));
            }
            var procResourceEntity = await _procResourceRepository.GetByIdAsync(facePlateEntit.ResourceId);
            if (procResourceEntity == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES17312));
            }
            ManuFacePlateRepairInitialInfoDto manuFacePlateRepairOpenInfoView = new ManuFacePlateRepairInitialInfoDto();
            manuFacePlateRepairOpenInfoView.ProcedureId = procProcedureEntity.Id;
            manuFacePlateRepairOpenInfoView.ProcedureCode = procProcedureEntity.Code;

            manuFacePlateRepairOpenInfoView.ResourceId = procResourceEntity.Id;
            manuFacePlateRepairOpenInfoView.ResourceCode = procResourceEntity.ResCode;
            return manuFacePlateRepairOpenInfoView;
        }


        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="manuFacePlateRepairCreateDto"></param>
        /// <returns></returns>
        public async Task CreateManuFacePlateRepairAsync(ManuFacePlateRepairCreateDto manuFacePlateRepairCreateDto)
        {
            //验证DTO
            await _validationCreateRules.ValidateAndThrowAsync(manuFacePlateRepairCreateDto);

            //DTO转换实体
            var manuFacePlateRepairEntity = manuFacePlateRepairCreateDto.ToEntity<ManuFacePlateRepairEntity>();
            manuFacePlateRepairEntity.Id = IdGenProvider.Instance.CreateId();
            manuFacePlateRepairEntity.CreatedBy = _currentUser.UserName;
            manuFacePlateRepairEntity.UpdatedBy = _currentUser.UserName;
            manuFacePlateRepairEntity.CreatedOn = HymsonClock.Now();
            manuFacePlateRepairEntity.UpdatedOn = HymsonClock.Now();
            manuFacePlateRepairEntity.SiteId = _currentSite.SiteId ?? 0;

            //入库
            await _manuFacePlateRepairRepository.InsertAsync(manuFacePlateRepairEntity);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteManuFacePlateRepairAsync(long id)
        {
            await _manuFacePlateRepairRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeletesManuFacePlateRepairAsync(long[] ids)
        {
            return await _manuFacePlateRepairRepository.DeletesAsync(new DeleteCommand { Ids = ids, DeleteOn = HymsonClock.Now(), UserId = _currentUser.UserName });
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="manuFacePlateRepairPagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ManuFacePlateRepairDto>> GetPagedListAsync(ManuFacePlateRepairPagedQueryDto manuFacePlateRepairPagedQueryDto)
        {
            var manuFacePlateRepairPagedQuery = manuFacePlateRepairPagedQueryDto.ToQuery<ManuFacePlateRepairPagedQuery>();
            var pagedInfo = await _manuFacePlateRepairRepository.GetPagedInfoAsync(manuFacePlateRepairPagedQuery);

            //实体到DTO转换 装载数据
            List<ManuFacePlateRepairDto> manuFacePlateRepairDtos = PrepareManuFacePlateRepairDtos(pagedInfo);
            return new PagedInfo<ManuFacePlateRepairDto>(manuFacePlateRepairDtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pagedInfo"></param>
        /// <returns></returns>
        private static List<ManuFacePlateRepairDto> PrepareManuFacePlateRepairDtos(PagedInfo<ManuFacePlateRepairEntity> pagedInfo)
        {
            var manuFacePlateRepairDtos = new List<ManuFacePlateRepairDto>();
            foreach (var manuFacePlateRepairEntity in pagedInfo.Data)
            {
                var manuFacePlateRepairDto = manuFacePlateRepairEntity.ToModel<ManuFacePlateRepairDto>();
                manuFacePlateRepairDtos.Add(manuFacePlateRepairDto);
            }

            return manuFacePlateRepairDtos;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="manuFacePlateRepairModifyDto"></param>
        /// <returns></returns>
        public async Task ModifyManuFacePlateRepairAsync(ManuFacePlateRepairModifyDto manuFacePlateRepairModifyDto)
        {
            //验证DTO
            await _validationModifyRules.ValidateAndThrowAsync(manuFacePlateRepairModifyDto);

            //DTO转换实体
            var manuFacePlateRepairEntity = manuFacePlateRepairModifyDto.ToEntity<ManuFacePlateRepairEntity>();
            manuFacePlateRepairEntity.UpdatedBy = _currentUser.UserName;
            manuFacePlateRepairEntity.UpdatedOn = HymsonClock.Now();

            await _manuFacePlateRepairRepository.UpdateAsync(manuFacePlateRepairEntity);
        }

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ManuFacePlateRepairDto> QueryManuFacePlateRepairByIdAsync(long id)
        {
            var manuFacePlateRepairEntity = await _manuFacePlateRepairRepository.GetByIdAsync(id);
            if (manuFacePlateRepairEntity != null)
            {
                return manuFacePlateRepairEntity.ToModel<ManuFacePlateRepairDto>();
            }
            return new ManuFacePlateRepairDto();
        }


        #region 公用方法
        /// <summary>
        /// 根据SFC获取返修业务
        /// </summary>
        /// <param name="sfc"></param>
        /// <returns></returns>
        /// <exception cref="CustomerValidationException"></exception>
        private async Task<SfcProduceRepairBo> GetSfcProduceRepairBoAsync(string sfc)
        {
            // 获取维修业务
            var sfcProduceBusinessEntity = await _manuSfcProduceRepository.GetSfcProduceBusinessBySFCAsync(new SfcProduceBusinessQuery
            {
                SiteId = _currentSite.SiteId ?? 0,
                Sfc = sfc,
                BusinessType = ManuSfcProduceBusinessType.Repair
            }) ?? throw new CustomerValidationException(nameof(ErrorCode.MES17325));
            var sfcProduceRepairBo = JsonSerializer.Deserialize<SfcProduceRepairBo>(sfcProduceBusinessEntity.BusinessContent);
            if (sfcProduceRepairBo == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES17326));
            }
            return sfcProduceRepairBo;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ProcessRouteId"></param>
        /// <returns></returns>
        /// <exception cref="CustomerValidationException"></exception>
        private async Task<long> GetEndProcessRouteDetailIdAsync(long ProcessRouteId)
        {
            //获取尾工序
            // 因为可能有分叉，所以返回的下一步工序是集合
            var netxtProcessRouteDetailLinks = await _procProcessRouteDetailLinkRepository.GetPreProcessRouteDetailLinkAsync(new ProcProcessRouteDetailLinkQuery
            {
                ProcessRouteId = ProcessRouteId,
                ProcedureId = ProcessRoute.LastProcedureId
            });
            if (netxtProcessRouteDetailLinks == null || !netxtProcessRouteDetailLinks.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES18017));
            }
            //尾工序不允许多个
            var endProcessRouteDetailId = netxtProcessRouteDetailLinks.FirstOrDefault()?.PreProcessRouteDetailId ?? 0;
            if (endProcessRouteDetailId == 0)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES18017));
            }
            return endProcessRouteDetailId;
        }


        #endregion
    }
}
