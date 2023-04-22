/*
 *creator: Karl
 *
 *describe: 在制品维修    服务 | 代码由框架生成
 *builder:  pengxin
 *build datetime: 2023-04-12 10:32:46
 */
using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Constants.Process;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Manufacture.ManuFacePlateRepair.Query;
using Hymson.MES.Data.Repositories.Manufacture.ManuSfcProduce.Command;
using Hymson.MES.Data.Repositories.Plan;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.Warehouse;
using Hymson.MES.Services.Bos.Manufacture;
using Hymson.MES.Services.Dtos.Common;
using Hymson.MES.Services.Dtos.Manufacture;
using Hymson.MES.Services.Services.Manufacture.ManuMainstreamProcess.OutStation;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;

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
        /// 工艺路线表 仓储
        /// </summary>
        private readonly IProcProcessRouteRepository _procProcessRouteRepository;

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

        private readonly AbstractValidator<ManuFacePlateRepairCreateDto> _validationCreateRules;
        private readonly AbstractValidator<ManuFacePlateRepairModifyDto> _validationModifyRules;

        public ManuFacePlateRepairService(ICurrentUser currentUser, ICurrentSite currentSite,
            IManuFacePlateRepairRepository manuFacePlateRepairRepository, IManuSfcProduceRepository manuSfcProduceRepository,
            IWhMaterialInventoryRepository whMaterialInventoryRepository, IPlanWorkOrderRepository planWorkOrderRepository,
            IProcProcedureRepository procProcedureRepository, IProcMaterialRepository procMaterialRepository,
            IManuProductBadRecordRepository manuProductBadRecordRepository, IProcResourceRepository procResourceRepository,
            IProcProcessRouteDetailNodeRepository procProcessRouteNodeRepository, IManuFacePlateButtonService manuFacePlateButtonService,
            IManuOutStationService manuOutStationService,
        AbstractValidator<ManuFacePlateRepairCreateDto> validationCreateRules, AbstractValidator<ManuFacePlateRepairModifyDto> validationModifyRules)
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
        }

        /// <summary>
        /// 执行作业
        /// </summary>
        /// <param name="manuFacePlateRepairExJobDto"></param>
        /// <returns></returns>
        /// <exception cref="CustomerValidationException"></exception>
        public async Task<List<ManuFacePlateRepairButJobReturnTypeEnum>> ExecuteexecuteJobAsync(ManuFacePlateRepairExJobDto manuFacePlateRepairExJobDto)
        {
            #region  验证数据
            if (string.IsNullOrWhiteSpace(manuFacePlateRepairExJobDto.SFC))
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES17303));
            }
            #endregion

            #region 调用作业
            manuFacePlateRepairExJobDto.SFC = manuFacePlateRepairExJobDto.SFC.Trim();
            var jobDto = new ButtonRequestDto
            {
                FacePlateId = manuFacePlateRepairExJobDto.FacePlateId,
                FacePlateButtonId = manuFacePlateRepairExJobDto.FacePlateButtonId
            };

            Dictionary<string, string> dic = new Dictionary<string, string>
            {
                { "SFC", manuFacePlateRepairExJobDto.SFC },
                { "ProcedureId", $"{manuFacePlateRepairExJobDto.ProcedureId}" },
                { "ResourceId", $"{manuFacePlateRepairExJobDto.ResourceId}" }
            };
            jobDto.Param = dic;
            // 调用作业
            var resJob = await _manuFacePlateButtonService.ClickAsync(jobDto);
            if (resJob == null || resJob.Any() == false) throw new CustomerValidationException(nameof(ErrorCode.MES17320));

            var list = new List<ManuFacePlateRepairButJobReturnTypeEnum>();
            foreach (var item in resJob)
            {
                if (item.Key == ManuFacePlateRepairButJobReturnTypeEnum.JobManuRepairStartService.ToString())
                {
                    if (!list.Contains(ManuFacePlateRepairButJobReturnTypeEnum.JobManuRepairStartService))
                        list.Add(ManuFacePlateRepairButJobReturnTypeEnum.JobManuRepairStartService);
                }
                else if (item.Key == ManuFacePlateRepairButJobReturnTypeEnum.JobManuRepairEndService.ToString())
                {
                    if (!list.Contains(ManuFacePlateRepairButJobReturnTypeEnum.JobManuRepairEndService))
                        list.Add(ManuFacePlateRepairButJobReturnTypeEnum.JobManuRepairEndService);
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
            if (string.IsNullOrWhiteSpace(beginRepairDto.SFC))
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES17303));
            }
            beginRepairDto.SFC = beginRepairDto.SFC.Trim();
            //验证条码
            var manuSfcProduceEntit = await _manuSfcProduceRepository.GetBySFCAsync(beginRepairDto.SFC);
            if (manuSfcProduceEntit == null)
            {
                //是否已入库完成 待使用算完成
                var whMaterialInventoryEntit = await _whMaterialInventoryRepository.GetByBarCodeAsync(beginRepairDto.SFC);
                if (whMaterialInventoryEntit == null)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES16306));
                }
                else if (whMaterialInventoryEntit.Status != WhMaterialInventoryStatusEnum.ToBeUsed)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES16311)).WithData("Status", whMaterialInventoryEntit.Status.ToString());
                }
                else
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES16310));
                }
            }

            //验证在制信息(JOb已经把状态改了，所以这里不用验证了)
            //if (manuSfcProduceEntit.ProcedureId != beginRepairDto.ProcedureId || manuSfcProduceEntit.Status != SfcProduceStatusEnum.lineUp)
            //{
            //    throw new CustomerValidationException(nameof(ErrorCode.MES16308));
            //}
            #endregion

            #region 获取展示信息 
            //ManuFacePlateRepairOpenInfoDto manuFacePlateRepairOpenInfoDto = new ManuFacePlateRepairOpenInfoDto();

            ////产品信息
            ////工单
            //var planWorkOrderEntit = await _planWorkOrderRepository.GetByIdAsync(manuSfcProduceEntit.WorkOrderId);
            //if (planWorkOrderEntit == null)
            //{
            //    throw new CustomerValidationException(nameof(ErrorCode.MES17313));
            //}
            ////工序
            //var procProcedureEntit = await _procProcedureRepository.GetByIdAsync(manuSfcProduceEntit.ProcedureId);
            //if (procProcedureEntit == null)
            //{
            //    throw new CustomerValidationException(nameof(ErrorCode.MES17311));
            //}
            ////产品
            //var procMaterialEntit = await _procMaterialRepository.GetByIdAsync(manuSfcProduceEntit.ProductId);
            //if (procMaterialEntit == null)
            //{
            //    throw new CustomerValidationException(nameof(ErrorCode.MES17314));
            //}
            //var model = new ManuFacePlateRepairProductInfoDto
            //{
            //    SFC = manuSfcProduceEntit.SFC,
            //    Status = manuSfcProduceEntit.Status.ToString(),
            //    ProcedureCode = procProcedureEntit.Code,
            //    OrderCode = planWorkOrderEntit.OrderCode,
            //    MaterialCode = procMaterialEntit.MaterialCode,
            //    Version = procMaterialEntit.Version,
            //    MaterialName = procMaterialEntit.MaterialName,
            //};
            //manuFacePlateRepairOpenInfoDto.productInfo = model;

            ////获取不合格信息
            //var query = new ManuProductBadRecordQuery
            //{
            //    SFC = manuSfcProduceEntit.SFC,
            //    Status = ProductBadRecordStatusEnum.Open,
            //    SiteId = _currentSite.SiteId ?? 0
            //};
            //var manuProductBads = await _manuProductBadRecordRepository.GetBadRecordsBySfcAsync(query);
            //if (manuProductBads == null)
            //{
            //    throw new CustomerValidationException(nameof(ErrorCode.MES17316));
            //}
            //var manuFacePlateRepairProductBadInfoList = new List<ManuFacePlateRepairProductBadInfoDto>();
            //foreach (var item in manuProductBads)
            //{
            //    var manuFacePlateRepairProductBadInfoDto = new ManuFacePlateRepairProductBadInfoDto
            //    {
            //        BadRecordId = item.Id,
            //        UnqualifiedId = item.UnqualifiedId,
            //        UnqualifiedCode = item.UnqualifiedCode,
            //        UnqualifiedCodeName = item.UnqualifiedCodeName,
            //        ResCode = item.ResCode,
            //        IsClose = item.Status,
            //    };
            //    manuFacePlateRepairProductBadInfoList.Add(manuFacePlateRepairProductBadInfoDto);
            //}
            //manuFacePlateRepairOpenInfoDto.productBadInfo = manuFacePlateRepairProductBadInfoList;
            //manuFacePlateRepairOpenInfoDto.productInfo.badInfoDtos = manuFacePlateRepairProductBadInfoList;
            ////返回工序信息
            ////工艺路线
            ////var procProcessRouteEntit = _procProcessRouteRepository.GetByIdAsync(procMaterialEntit.ProcessRouteId ?? 0);
            ////工艺路线节点

            //var nodeQuery = new ProcProcessRouteDetailNodeQuery { ProcessRouteId = manuSfcProduceEntit.ProcessRouteId };
            //var procProcessRouteNodeList = await _procProcessRouteNodeRepository.GetListAsync(nodeQuery);
            //// var procProcessRouteNodeList = await _procProcessRouteNodeRepository.GetListAsync(new ProcProcessRouteDetailNodeQuery { ProcessRouteId = manuSfcProduceEntit.ProcessRouteId });
            //var manuFacePlateRepairReturnProcedureList = new List<ManuFacePlateRepairReturnProcedureDto>();

            //foreach (var itemNode in procProcessRouteNodeList)
            //{
            //    procProcedureEntit = await _procProcedureRepository.GetByIdAsync(itemNode.ProcedureId);
            //    if (procProcedureEntit != null)
            //        if (procProcedureEntit.IsRepairReturn == 0)
            //        {
            //            var manuFacePlateRepairReturnProcedureDto = new ManuFacePlateRepairReturnProcedureDto
            //            {
            //                ProcedureId = itemNode.ProcedureId,
            //                ProcedureCode = procProcedureEntit.Code
            //            };
            //            manuFacePlateRepairReturnProcedureList.Add(manuFacePlateRepairReturnProcedureDto);
            //        }
            //}
            //manuFacePlateRepairOpenInfoDto.returnProcedureInfo = manuFacePlateRepairReturnProcedureList;

            #endregion
            var manuFacePlateRepairOpenInfoDto = await GetManuFacePlateRepairOpenInfoDto(manuSfcProduceEntit);

            #region 启动维修 更新状态

            //这里Job做了

            // 更改状态，将条码由"排队"改为"活动"
            //var comModel = new UpdateStatusCommand
            //{
            //    Id = manuSfcProduceEntit.Id,
            //    Status = SfcProduceStatusEnum.Activity,
            //    UserId = _currentUser.UserName,
            //    UpdatedOn = HymsonClock.Now()
            //};
            //var manuSfcProduceUpdate = await _manuSfcProduceRepository.UpdateStatusAsync(comModel);
            //if (manuSfcProduceUpdate <= 0)
            //{
            //    throw new CustomerValidationException(nameof(ErrorCode.MES17317));
            //}
            #endregion

            return manuFacePlateRepairOpenInfoDto;
        }

        /// <summary>
        /// 获取展示信息 
        /// </summary>
        /// <param name="manuSfcProduceEntit"></param>
        /// <returns></returns>
        /// <exception cref="CustomerValidationException"></exception>
        private async Task<ManuFacePlateRepairOpenInfoDto> GetManuFacePlateRepairOpenInfoDto(ManuSfcProduceEntity manuSfcProduceEntit)
        {
            //获取产品信息
            var manuSfcProduce = await _manuSfcProduceRepository.GetPagedInfoAsync(new ManuSfcProducePagedQuery { PageSize = 1, PageIndex = 1, SiteId = _currentSite.SiteId, Sfc = sfc });
            var manuSfcProduceInfo = manuSfcProduce.Data.FirstOrDefault();
            if (manuSfcProduceInfo == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES17306));
            }
            ManuFacePlateRepairOpenInfoDto manuFacePlateRepairOpenInfoDto = new ManuFacePlateRepairOpenInfoDto();

            //产品信息
            //工单
            var planWorkOrderEntit = await _planWorkOrderRepository.GetByIdAsync(manuSfcProduceEntit.WorkOrderId);
            if (planWorkOrderEntit == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES17313));
            }
            //工序
            var procProcedureEntit = await _procProcedureRepository.GetByIdAsync(manuSfcProduceEntit.ProcedureId);
            if (procProcedureEntit == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES17311));
            }
            //产品
            var procMaterialEntit = await _procMaterialRepository.GetByIdAsync(manuSfcProduceEntit.ProductId);
            if (procMaterialEntit == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES17314));
            }
            var model = new ManuFacePlateRepairProductInfoDto
            {
                SFC = manuSfcProduceEntit.SFC,
                Status = manuSfcProduceEntit.Status.ToString(),
                ProcedureCode = procProcedureEntit.Code,
                OrderCode = planWorkOrderEntit.OrderCode,
                MaterialCode = procMaterialEntit.MaterialCode,
                Version = procMaterialEntit.Version,
                MaterialName = procMaterialEntit.MaterialName,
            };
            manuFacePlateRepairOpenInfoDto.productInfo = model;

            //获取不合格信息
            var query = new ManuProductBadRecordQuery
            {
                SFC = manuSfcProduceEntit.SFC,
                Status = ProductBadRecordStatusEnum.Open,
                SiteId = _currentSite.SiteId ?? 0
            };
            var manuProductBads = await _manuProductBadRecordRepository.GetBadRecordsBySfcAsync(query);
            if (manuProductBads == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES17316));
            }
            var manuFacePlateRepairProductBadInfoList = new List<ManuFacePlateRepairProductBadInfoDto>();
            foreach (var item in manuProductBads)
            {
                var manuFacePlateRepairProductBadInfoDto = new ManuFacePlateRepairProductBadInfoDto
                {
                    BadRecordId = item.Id,
                    UnqualifiedId = item.UnqualifiedId,
                    UnqualifiedCode = item.UnqualifiedCode,
                    UnqualifiedCodeName = item.UnqualifiedCodeName,
                    ResCode = item.ResCode,
                    IsClose = item.Status,
                };
                manuFacePlateRepairProductBadInfoList.Add(manuFacePlateRepairProductBadInfoDto);
            }
            manuFacePlateRepairOpenInfoDto.productBadInfo = manuFacePlateRepairProductBadInfoList;
            manuFacePlateRepairOpenInfoDto.productInfo.badInfoDtos = manuFacePlateRepairProductBadInfoList;
            //返回工序信息
            //工艺路线
            //var procProcessRouteEntit = _procProcessRouteRepository.GetByIdAsync(procMaterialEntit.ProcessRouteId ?? 0);
            //工艺路线节点

            var nodeQuery = new ProcProcessRouteDetailNodeQuery { ProcessRouteId = manuSfcProduceEntit.ProcessRouteId };
            var procProcessRouteNodeList = await _procProcessRouteNodeRepository.GetListAsync(nodeQuery);
            // var procProcessRouteNodeList = await _procProcessRouteNodeRepository.GetListAsync(new ProcProcessRouteDetailNodeQuery { ProcessRouteId = manuSfcProduceEntit.ProcessRouteId });
            var manuFacePlateRepairReturnProcedureList = new List<ManuFacePlateRepairReturnProcedureDto>();

            foreach (var itemNode in procProcessRouteNodeList)
            {
                procProcedureEntit = await _procProcedureRepository.GetByIdAsync(itemNode.ProcedureId);
                if (procProcedureEntit != null)
                    if (procProcedureEntit.IsRepairReturn == 1)
                    {
                        var manuFacePlateRepairReturnProcedureDto = new ManuFacePlateRepairReturnProcedureDto
                        {
                            ProcedureId = itemNode.ProcedureId,
                            ProcedureCode = procProcedureEntit.Code
                        };
                        manuFacePlateRepairReturnProcedureList.Add(manuFacePlateRepairReturnProcedureDto);
                    }
            }
            manuFacePlateRepairReturnProcedureList.Add(new ManuFacePlateRepairReturnProcedureDto
            {
                ProcedureId = ProcessRoute.LastProcedureId,
                ProcedureCode = "END"
            });
            manuFacePlateRepairOpenInfoDto.returnProcedureInfo = manuFacePlateRepairReturnProcedureList;

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
            var manuSfcProduceEntit = await _manuSfcProduceRepository.GetBySFCAsync(beginRepairDto.SFC);
            if (manuSfcProduceEntit == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16306));
            }
            if (manuSfcProduceEntit.Status != SfcProduceStatusEnum.Activity)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES17322));
            }
            beginRepairDto.SFC = beginRepairDto.SFC.Trim();
            var manuFacePlateRepairOpenInfoDto = await GetManuFacePlateRepairOpenInfoDto(manuSfcProduceEntit);
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

            if (confirmSubmitDto.ReturnProcedureId <= 0)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES17318));
            }
            //检查缺陷是否关闭
            var isCloseALL = confirmSubmitDto.confirmSubmitDetail.Where(it => it.IsClose == ProductBadRecordStatusEnum.Open).Any();
            if (isCloseALL)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES17307));
            }
            //获取面版
            //var facePlateEntit = await _manuFacePlateRepairRepository.GetByFacePlateIdAsync(confirmSubmitDto.FacePlateId);
            //if (facePlateEntit == null)
            //{
            //    throw new CustomerValidationException(nameof(ErrorCode.MES17309));
            //}
            //获取条码生产信息
            var manuSfcProduceEntit = await _manuSfcProduceRepository.GetBySFCAsync(confirmSubmitDto.SFC);
            if (manuSfcProduceEntit == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES17306));
            }
            #endregion

            #region 数据组装
            //维修记录
            var manuSfcRepairRecordEntity = new ManuSfcRepairRecordEntity();
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
            //不良录入
            var badRecordsList = new List<ManuProductBadRecordEntity>();
            var manuSfcRepairDetailList = new List<ManuSfcRepairDetailEntity>();
            foreach (var item in confirmSubmitDto.confirmSubmitDetail)
            {
                var badRecordEntit = await _manuProductBadRecordRepository.GetByIdAsync(item.BadRecordId);
                if (badRecordEntit == null)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES17316));
                }
                badRecordEntit.Status = ProductBadRecordStatusEnum.Close;

                //维修明细
                ManuSfcRepairDetailEntity manuSfcRepairDetailEntity = new ManuSfcRepairDetailEntity();
                manuSfcRepairDetailEntity.SfcRepairId = manuSfcRepairRecordEntity.Id;
                manuSfcRepairDetailEntity.ProductBadId = item.BadRecordId;
                manuSfcRepairDetailEntity.RepairMethod = item.RepairMethod;
                manuSfcRepairDetailEntity.CauseAnalyse = item.CauseAnalyse;
                manuSfcRepairDetailEntity.IsClose = ManuSfcRepairDetailIsIsCloseEnum.Close;

                manuSfcRepairDetailEntity.Id = IdGenProvider.Instance.CreateId();
                manuSfcRepairDetailEntity.CreatedBy = _currentUser.UserName;
                manuSfcRepairDetailEntity.UpdatedBy = _currentUser.UserName;
                manuSfcRepairDetailEntity.CreatedOn = HymsonClock.Now();
                manuSfcRepairDetailEntity.UpdatedOn = HymsonClock.Now();
                manuSfcRepairDetailEntity.SiteId = _currentSite.SiteId ?? 0;

                badRecordsList.Add(badRecordEntit);
                manuSfcRepairDetailList.Add(manuSfcRepairDetailEntity);
            }

            //  返回工序(出站更改)
            //var updateProcedureCommand = new UpdateProcedureCommand
            //{
            //    Id = manuSfcProduceEntit.Id,
            //    ProcedureId = confirmSubmitDto.ReturnProcedureId,
            //    UserId = _currentUser.UserName,
            //    UpdatedOn = HymsonClock.Now()
            //};
            #endregion

            #region 事务入库
            var rows = 0;
            using (var trans = TransactionHelper.GetTransactionScope())
            {
                //维修记录
                rows += await _manuFacePlateRepairRepository.InsertRecordAsync(manuSfcRepairRecordEntity);
                //维修明细
                rows += await _manuFacePlateRepairRepository.InsertsDetailAsync(manuSfcRepairDetailList);
                //不良录入
                rows += await _manuProductBadRecordRepository.UpdateRangeAsync(badRecordsList);
                //返回工序(出站更改)
                //rows += await _manuSfcProduceRepository.UpdateUpdateProcedureIdSqlAsync(updateProcedureCommand);

                //出站 
                rows += await _manuOutStationService.OutStationRepiarAsync(new ManufactureRepairBo { SFC = confirmSubmitDto.SFC, ProcedureId = confirmSubmitDto.ProcedureId, ReturnProcedureId = confirmSubmitDto.ReturnProcedureId, ResourceId = confirmSubmitDto.ResourceId });
                trans.Complete();
            }
            if (rows == 0)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES17310));
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
            if (facePlateEntit == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES17311));
            }
            var procResourceEntity = await _procResourceRepository.GetByIdAsync(facePlateEntit.ResourceId);
            if (facePlateEntit == null)
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
    }
}
