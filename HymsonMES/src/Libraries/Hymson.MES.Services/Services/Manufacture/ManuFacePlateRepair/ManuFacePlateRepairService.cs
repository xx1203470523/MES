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
using Hymson.MES.CoreServices.Services.Common.ManuCommon;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Manufacture.ManuFacePlateRepair.Query;
using Hymson.MES.Data.Repositories.Manufacture.ManuProductBadRecord.Command;
using Hymson.MES.Data.Repositories.Manufacture.ManuSfcProduce.Command;
using Hymson.MES.Data.Repositories.Manufacture.ManuSfcProduce.Query;
using Hymson.MES.Data.Repositories.Plan;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.Quality.IQualityRepository;
using Hymson.MES.Data.Repositories.Warehouse;
using Hymson.MES.Data.Repositories.Warehouse.WhMaterialInventory.Query;
using Hymson.MES.Services.Dtos.Manufacture;
using Hymson.MES.Services.Services.Manufacture.ManuMainstreamProcess.ManuInStation;
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
        /// 接口（进站）
        /// </summary>
        private readonly IManuInStationService _manuInStationService;

        /// <summary>
        /// 仓储接口（条码步骤）
        /// </summary>
        private readonly IManuSfcStepRepository _manuSfcStepRepository;

        /// <summary>
        /// 不合格代码
        /// </summary>
        private readonly IQualUnqualifiedCodeRepository _qualUnqualifiedCodeRepository;
        /// <summary>
        /// 仓储接口（工艺路线工序连线）
        /// </summary>
        private readonly IProcProcessRouteDetailLinkRepository _procProcessRouteDetailLinkRepository;

        /// <summary>
        /// 服务接口（生产通用）
        /// </summary>
        private readonly IManuCommonService _manuCommonService;

        private readonly ILocalizationService _localizationService;
        private readonly AbstractValidator<ManuFacePlateRepairCreateDto> _validationCreateRules;
        private readonly AbstractValidator<ManuFacePlateRepairModifyDto> _validationModifyRules;

        public ManuFacePlateRepairService(ICurrentUser currentUser, ICurrentSite currentSite,
            IManuFacePlateRepairRepository manuFacePlateRepairRepository, IManuSfcProduceRepository manuSfcProduceRepository,
            IWhMaterialInventoryRepository whMaterialInventoryRepository, IPlanWorkOrderRepository planWorkOrderRepository,
            IProcProcedureRepository procProcedureRepository, IProcMaterialRepository procMaterialRepository,
            IManuProductBadRecordRepository manuProductBadRecordRepository, IProcResourceRepository procResourceRepository,
            IProcProcessRouteDetailNodeRepository procProcessRouteNodeRepository, IManuFacePlateButtonService manuFacePlateButtonService,
            IManuOutStationService manuOutStationService, ILocalizationService localizationService,
            IManuInStationService manuInStationService, IManuSfcStepRepository manuSfcStepRepository,
            IQualUnqualifiedCodeRepository qualUnqualifiedCodeRepository,
        AbstractValidator<ManuFacePlateRepairCreateDto> validationCreateRules, AbstractValidator<ManuFacePlateRepairModifyDto> validationModifyRules,
        IProcProcessRouteDetailLinkRepository procProcessRouteDetailLinkRepository, IManuCommonService manuCommonService)
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
            _manuInStationService = manuInStationService;
            _manuInStationService = manuInStationService;
            _manuSfcStepRepository = manuSfcStepRepository;
            _qualUnqualifiedCodeRepository = qualUnqualifiedCodeRepository;
            _procProcessRouteDetailLinkRepository = procProcessRouteDetailLinkRepository;
            _manuCommonService = manuCommonService;
        }


        /// <summary>
        /// 执行作业
        /// </summary>
        /// <param name="manuFacePlateRepairExJobDto"></param>
        /// <returns></returns>
        /// <exception cref="CustomerValidationException"></exception>
        public async Task<List<ManuFacePlateRepairButJobReturnTypeEnum>> ExecuteJobAsync(ManuFacePlateRepairExJobDto manuFacePlateRepairExJobDto)
        {
            #region  验证数据
            if (string.IsNullOrWhiteSpace(manuFacePlateRepairExJobDto.SFC))
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES17303));
            }
            #endregion

            #region 调用作业
            //manuFacePlateRepairExJobDto.SFC = manuFacePlateRepairExJobDto.SFC.Trim();
            var jobDto = new ButtonRequestDto
            {
                FacePlateId = manuFacePlateRepairExJobDto.FacePlateId,
                FacePlateButtonId = manuFacePlateRepairExJobDto.FacePlateButtonId
            };

            //Dictionary<string, string> dic = new Dictionary<string, string>
            //{
            //    { "SFC", manuFacePlateRepairExJobDto.SFC },
            //    { "ProcedureId", $"{manuFacePlateRepairExJobDto.ProcedureId}" },
            //    { "ResourceId", $"{manuFacePlateRepairExJobDto.ResourceId}" }
            //};
            //jobDto.Param = dic;
            var sfcs = new List<string>() { manuFacePlateRepairExJobDto.SFC };
            JobRequestBo bo = new()
            {
                SFCs = sfcs,
                ProcedureId = manuFacePlateRepairExJobDto.ProcedureId,
                ResourceId = manuFacePlateRepairExJobDto.ResourceId,
                SiteId = _currentSite.SiteId ?? 123456,
                UserName = _currentUser.UserName
            };
            // 调用作业
            //var resJob = await _manuFacePlateButtonService.ClickAsync(jobDto);

            var resJob = await _manuFacePlateButtonService.NewClickAsync(jobDto, bo);
            if (resJob == null || resJob.Any() == false) throw new CustomerValidationException(nameof(ErrorCode.MES17320));

            var list = new List<ManuFacePlateRepairButJobReturnTypeEnum>();
            foreach (var item in resJob)
            {
                if (item.Key == ManuFacePlateRepairButJobReturnTypeEnum.RepairStartJobService.ToString())
                {
                    if (!list.Contains(ManuFacePlateRepairButJobReturnTypeEnum.RepairStartJobService))
                        list.Add(ManuFacePlateRepairButJobReturnTypeEnum.RepairStartJobService);
                }
                else if (item.Key == ManuFacePlateRepairButJobReturnTypeEnum.RepairEndJobService.ToString())
                {
                    if (!list.Contains(ManuFacePlateRepairButJobReturnTypeEnum.RepairEndJobService))
                        list.Add(ManuFacePlateRepairButJobReturnTypeEnum.RepairEndJobService);
                }
                else
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES17321)).WithData("key", item.Key);
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
                SiteId = _currentSite.SiteId ?? 123456,
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
                IsRepair = true,
                Operatetype = ManuSfcStepTypeEnum.Repair,
                CurrentStatus = SfcProduceStatusEnum.Activity,
                EquipmentId = sfcProduceEntity.EquipmentId,
                ResourceId = sfcProduceEntity.ResourceId,
                CreatedBy = _currentUser.UserName,
                CreatedOn = HymsonClock.Now(),
                UpdatedBy = _currentUser.UserName,
                UpdatedOn = HymsonClock.Now(),
            };

            // 更改状态，将条码由"排队"改为"活动"
            sfcProduceEntity.Status = SfcProduceStatusEnum.Activity;
            sfcProduceEntity.UpdatedBy = _currentUser.UserName;
            sfcProduceEntity.UpdatedOn = HymsonClock.Now();
            _ = await _manuSfcProduceRepository.UpdateAsync(sfcProduceEntity);

            await _manuSfcStepRepository.InsertAsync(sfcStep);
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
            var manuSfcProduce = await _manuSfcProduceRepository.GetBySFCAsync(new ManuSfcProduceBySfcQuery { Sfc = manuSfcProduceEntit.SFC, SiteId = _currentSite.SiteId ?? 123456 })
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
                SiteId = _currentSite.SiteId ?? 123456,
                SFCs = new List<string> { manuSfcProduceEntit.SFC },
                ProcedureId = manuSfcProduceEntit.ProcedureId
            });

            if (manuSfcProduceEntit.IsScrap == TrueOrFalseEnum.Yes)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16322)).WithData("SFC", manuSfcProduceEntit.SFC);
            }

            // 产品信息
            ManuFacePlateRepairOpenInfoDto manuFacePlateRepairOpenInfoDto = new()
            {
                productInfo = new ManuFacePlateRepairProductInfoDto
                {
                    SFC = manuSfcProduceEntit.SFC,
                    Status = manuSfcProduceEntit.Status == SfcProduceStatusEnum.lineUp ? _localizationService.GetResource(nameof(ErrorCode.MES17323)) : _localizationService.GetResource(nameof(ErrorCode.MES17324)),// manuSfcProduceEntit.Status.ToString(),
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
                SiteId = _currentSite.SiteId ?? 123456
            });
            //if (manuProductBads == null || !manuProductBads.Any()) throw new CustomerValidationException(nameof(ErrorCode.MES17316));
            if (manuProductBads != null && manuProductBads.Any())
            {
                var manuSfcRepairDetailList = await _manuFacePlateRepairRepository.ManuSfcRepairDetailByProductBadIdAsync(new ManuSfcRepairDetailByProductBadIdQuery
                {
                    ProductBadId = manuProductBads.Select(it => it.Id).ToArray(),
                    SiteId = _currentSite.SiteId ?? 123456
                });

                // 组装不合格数据
                var manuFacePlateRepairProductBadInfoList = manuProductBads.Select(s => new ManuFacePlateRepairProductBadInfoDto
                {
                    BadRecordId = s.Id,
                    UnqualifiedId = s.UnqualifiedId,
                    UnqualifiedCode = s.UnqualifiedCode,
                    UnqualifiedCodeName = s.UnqualifiedCodeName,
                    Type = s.Type,
                    CauseAnalyse = manuSfcRepairDetailList == null ? "" : manuSfcRepairDetailList.Where(it => it.ProductBadId == s.Id).FirstOrDefault()?.CauseAnalyse ?? "",
                    RepairMethod = manuSfcRepairDetailList == null ? "" : manuSfcRepairDetailList.Where(it => it.ProductBadId == s.Id).FirstOrDefault()?.RepairMethod ?? "",
                    ResCode = s.ResCode,
                    IsClose = s.Status,
                });
                manuFacePlateRepairOpenInfoDto.productBadInfo = manuFacePlateRepairProductBadInfoList;
                manuFacePlateRepairOpenInfoDto.productInfo.badInfoDtos = manuFacePlateRepairProductBadInfoList;
            }
            // 获取维修业务
            var sfcProduceBusinessEntity = await _manuSfcProduceRepository.GetSfcProduceBusinessBySFCAsync(new SfcProduceBusinessQuery
            {
                SiteId = _currentSite.SiteId ?? 123456,
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

            // var procProcessRouteNodeList = await _procProcessRouteNodeRepository.GetListAsync(new ProcProcessRouteDetailNodeQuery { ProcessRouteId = manuSfcProduceEntit.ProcessRouteId });
            List<ManuFacePlateRepairReturnProcedureDto> manuFacePlateRepairReturnProcedureList = new();
            var procProcedureEntities = await _procProcedureRepository.GetByIdsAsync(procProcessRouteNodeList.Select(s => s.ProcedureId).ToArray());
            foreach (var itemNode in procProcessRouteNodeList)
            {
                procProcedureEntity = procProcedureEntities.FirstOrDefault(f => f.Id == itemNode.ProcedureId);
                if (procProcedureEntity == null) continue;
                if (procProcedureEntity.IsRepairReturn != 1) continue;

                manuFacePlateRepairReturnProcedureList.Add(new ManuFacePlateRepairReturnProcedureDto
                {
                    ProcedureId = itemNode.ProcedureId,
                    ProcedureCode = procProcedureEntity.Code
                });
            }
            //manuFacePlateRepairReturnProcedureList.Add(new ManuFacePlateRepairReturnProcedureDto
            //{
            //    ProcedureId = ProcessRoute.LastProcedureId,
            //    ProcedureCode = "END"
            //});
            manuFacePlateRepairOpenInfoDto.returnProcedureInfo = manuFacePlateRepairReturnProcedureList;

            //尾工序
            var endProcessRouteDetailId = await GetEndProcessRouteDetailId(manuSfcProduceEntit.ProcessRouteId);
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
                SiteId = _currentSite.SiteId ?? 123456,
                Sfc = beginRepairDto.SFC
            }) ?? throw new CustomerValidationException(nameof(ErrorCode.MES16306));
            if (manuSfcProduceEntit.Status != SfcProduceStatusEnum.Activity)
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
                SiteId = _currentSite.SiteId ?? 123456,
                Sfc = confirmSubmitDto.SFC
            });
            if (manuSfcProduceEntit == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES17306));
            }
            #endregion


            #region 工序判断
            var endProcessRouteDetailId = await GetEndProcessRouteDetailId(manuSfcProduceEntit.ProcessRouteId);
            if (confirmSubmitDto.ProcedureId == endProcessRouteDetailId && confirmSubmitDto.ReturnProcedureId <= 0)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES17318));
            }
            #endregion

            #region 数据组装
            var manuSfcRepairRecordEntity = await _manuFacePlateRepairRepository.GetManuSfcRepairBySFCAsync(new GetManuSfcRepairBySfcQuery { SFC = confirmSubmitDto.SFC, SiteId = _currentSite.SiteId ?? 123456 });
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
                manuSfcRepairRecordEntity.SiteId = _currentSite.SiteId ?? 123456;
            }


            //不良录入
            var badRecordsList = new List<ManuProductBadRecordEntity>();
            var createManuSfcRepairDetailList = new List<ManuSfcRepairDetailEntity>();
            var updateManuSfcRepairDetailList = new List<ManuSfcRepairDetailEntity>();
            var BadRecordIds = confirmSubmitDto.confirmSubmitDetail.Select(it => it.BadRecordId).ToArray();
            var badRecordList = await _manuProductBadRecordRepository.GetByIdsAsync(BadRecordIds);

            var sfcProduceRepairBo = await GetSfcProduceRepairBo(manuSfcProduceEntit.SFC);

            var GetManuSfcRepairDetails = await _manuFacePlateRepairRepository.ManuSfcRepairDetailByProductBadIdAsync(new ManuSfcRepairDetailByProductBadIdQuery
            {
                ProductBadId = BadRecordIds,
                SiteId = _currentSite.SiteId ?? 123456
            });
            //if (badRecordList == null || !badRecordList.Any())
            //{
            //    throw new CustomerValidationException(nameof(ErrorCode.MES17331));
            //}
            var updateCommandList = new List<ManuProductBadRecordUpdateCommand>();

            if (badRecordList != null && badRecordList.Any())
            {
                var validationFailures = new List<ValidationFailure>();
                foreach (var item in confirmSubmitDto.confirmSubmitDetail)
                {
                    var validationFailure = new ValidationFailure();
                    var badRecordEntit = badRecordList.Where(it => it.Id == item.BadRecordId).FirstOrDefault();
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

                    if (confirmSubmitDto.ProcedureId == endProcessRouteDetailId)
                    {
                        if (item.IsClose == ProductBadRecordStatusEnum.Open)
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
                            validationFailure.ErrorCode = nameof(ErrorCode.MES17307);
                            validationFailures.Add(validationFailure);
                            continue;
                        }

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
                            SiteId = _currentSite.SiteId ?? 123456,
                            Id = item.BadRecordId,
                            DisposalResult = ProductBadDisposalResultEnum.repair,
                            Remark = confirmSubmitDto.Remark ?? "",
                            Status = item.IsClose,// ProductBadRecordStatusEnum.Close,
                            UserId = _currentUser.UserName,
                            UpdatedOn = HymsonClock.Now(),
                        });


                        badRecordEntit.Status = item.IsClose;// ProductBadRecordStatusEnum.Close;
                        badRecordEntit.UpdatedBy = _currentUser.UserName;
                        badRecordEntit.UpdatedOn = HymsonClock.Now();
                        badRecordsList.Add(badRecordEntit);
                    }

                    var isClose = item.IsClose == ProductBadRecordStatusEnum.Close ? ManuSfcRepairDetailIsIsCloseEnum.Close : ManuSfcRepairDetailIsIsCloseEnum.Open;
                    //维修明细
                    if (GetManuSfcRepairDetails != null && GetManuSfcRepairDetails.Any())
                    {
                        var detail = GetManuSfcRepairDetails.Where(it => it.ProductBadId == item.BadRecordId && it.SfcRepairId == manuSfcRepairRecordEntity.Id).FirstOrDefault();
                        if (detail != null)
                        {
                            detail.RepairMethod = item.RepairMethod;
                            detail.CauseAnalyse = item.CauseAnalyse;
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
                        RepairMethod = item.RepairMethod,
                        CauseAnalyse = item.CauseAnalyse,
                        IsClose = isClose,// ManuSfcRepairDetailIsIsCloseEnum.Close,

                        Id = IdGenProvider.Instance.CreateId(),
                        CreatedBy = _currentUser.UserName,
                        UpdatedBy = _currentUser.UserName,
                        CreatedOn = HymsonClock.Now(),
                        UpdatedOn = HymsonClock.Now(),
                        SiteId = _currentSite.SiteId ?? 123456
                    };
                    createManuSfcRepairDetailList.Add(manuSfcRepairDetailEntity);
                }
                if (validationFailures.Any())
                {
                    throw new ValidationException(_localizationService.GetResource(nameof(ErrorCode.MES17328)), validationFailures);
                }
            }
            //var resources = await _procResourceRepository.GetProcResourceListByProcedureIdAsync(new ProcResourceListByProcedureIdQuery
            //{
            //    SiteId = _currentSite.SiteId ?? 123456,
            //    ProcedureId = confirmSubmitDto.ReturnProcedureId
            //});
            //if (resources == null || !resources.Any())
            //{
            //    throw new CustomerValidationException(nameof(ErrorCode.MES17327));
            //}
            //long resourcesId = resources.FirstOrDefault().Id;

            // 返回工序 
            var updateProcedureCommand = new UpdateProcedureCommand
            {
                Id = manuSfcProduceEntit.Id,
                Status = SfcProduceStatusEnum.lineUp,
                ProcedureId = confirmSubmitDto.ReturnProcedureId,
                ResourceId = null, //resourcesId, //资源这里 直接设置为null 为null生产不检测匹配工序
                ProcessRouteId = sfcProduceRepairBo.ProcessRouteId,
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
                IsRepair = true,
                Operatetype = ManuSfcStepTypeEnum.RepairReturn,
                CurrentStatus = SfcProduceStatusEnum.lineUp,
                EquipmentId = manuSfcProduceEntit.EquipmentId,
                ResourceId = manuSfcProduceEntit.ResourceId,
                CreatedBy = _currentUser.UserName,
                CreatedOn = HymsonClock.Now(),
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
                    //rows += await _manuProductBadRecordRepository.UpdateRangeAsync(badRecordsList);
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
                if (confirmSubmitDto.ProcedureId == endProcessRouteDetailId)
                {
                    //返回工序
                    rows += await _manuSfcProduceRepository.UpdateProcedureIdAsync(updateProcedureCommand);
                    // 删除 manu_sfc_produce_business
                    rows += await _manuSfcProduceRepository.DeleteSfcProduceBusinessBySfcInfoIdAsync(new DeleteSfcProduceBusinesssBySfcInfoIdCommand
                    {
                        SiteId = manuSfcProduceEntit.SiteId,
                        SfcInfoId = manuSfcProduceEntit.Id
                    });
                    //步骤
                    rows += await _manuSfcStepRepository.InsertAsync(sfcStep);
                }

                trans.Complete();
            }
            //if (rows == 0)
            //{
            //    throw new CustomerValidationException(nameof(ErrorCode.MES17310));
            //}
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
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0)
            {
                throw new ValidationException(nameof(ErrorCode.MES10101));
            }

            //验证DTO
            await _validationCreateRules.ValidateAndThrowAsync(manuFacePlateRepairCreateDto);

            //DTO转换实体
            var manuFacePlateRepairEntity = manuFacePlateRepairCreateDto.ToEntity<ManuFacePlateRepairEntity>();
            manuFacePlateRepairEntity.Id = IdGenProvider.Instance.CreateId();
            manuFacePlateRepairEntity.CreatedBy = _currentUser.UserName;
            manuFacePlateRepairEntity.UpdatedBy = _currentUser.UserName;
            manuFacePlateRepairEntity.CreatedOn = HymsonClock.Now();
            manuFacePlateRepairEntity.UpdatedOn = HymsonClock.Now();
            manuFacePlateRepairEntity.SiteId = _currentSite.SiteId ?? 123456;

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
        /// <param name="manuFacePlateRepairDto"></param>
        /// <returns></returns>
        public async Task ModifyManuFacePlateRepairAsync(ManuFacePlateRepairModifyDto manuFacePlateRepairModifyDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0)
            {
                throw new ValidationException(nameof(ErrorCode.MES10101));
            }

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
            return null;
        }


        #region 公用方法
        /// <summary>
        /// 根据SFC获取返修业务
        /// </summary>
        /// <param name="sfc"></param>
        /// <returns></returns>
        /// <exception cref="CustomerValidationException"></exception>
        private async Task<SfcProduceRepairBo> GetSfcProduceRepairBo(string sfc)
        {
            // 获取维修业务
            var sfcProduceBusinessEntity = await _manuSfcProduceRepository.GetSfcProduceBusinessBySFCAsync(new SfcProduceBusinessQuery
            {
                SiteId = _currentSite.SiteId ?? 123456,
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

        private async Task<long> GetEndProcessRouteDetailId(long ProcessRouteId)
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
