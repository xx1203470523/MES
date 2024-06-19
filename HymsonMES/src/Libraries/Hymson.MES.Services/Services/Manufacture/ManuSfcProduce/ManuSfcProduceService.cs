using FluentValidation;
using FluentValidation.Results;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.Localization.Services;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Constants.Manufacture;
using Hymson.MES.Core.Constants.Process;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Domain.Plan;
using Hymson.MES.Core.Domain.Warehouse;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.Core.Enums.Warehouse;
using Hymson.MES.CoreServices.Bos.Common;
using Hymson.MES.CoreServices.Bos.Manufacture;
using Hymson.MES.CoreServices.Services.Common;
using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.MES.Data.Repositories.Integrated;
using Hymson.MES.Data.Repositories.Integrated.IIntegratedRepository;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Plan;
using Hymson.MES.Data.Repositories.Plan.PlanWorkOrder.Command;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.Warehouse;
using Hymson.MES.Data.Repositories.Warehouse.WhMaterialInventory.Command;
using Hymson.MES.Data.Repositories.Warehouse.WhMaterialInventory.Query;
using Hymson.MES.Services.Dtos.Integrated;
using Hymson.MES.Services.Dtos.Manufacture;
using Hymson.MES.Services.Services.Manufacture.ManuMainstreamProcess.ManuCommon;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Hymson.MES.Services.Services.Manufacture.ManuSfcProduce
{
    /// <summary>
    /// 条码生产信息（物理删除） 服务
    /// </summary>
    public partial class ManuSfcProduceService : IManuSfcProduceService
    {
        /// <summary>
        /// 当前对象（登录用户）
        /// </summary>
        private readonly ICurrentUser _currentUser;

        /// <summary>
        /// 当前对象（站点）
        /// </summary>
        private readonly ICurrentSite _currentSite;

        /// <summary>
        /// 条码生产信息（物理删除） 仓储
        /// </summary>
        private readonly IManuSfcProduceRepository _manuSfcProduceRepository;

        /// <summary>
        /// 资源仓储
        /// </summary>
        private readonly IProcResourceRepository _resourceRepository;

        /// <summary>
        /// 条码步骤表仓储 仓储
        /// </summary>
        private readonly IManuSfcStepRepository _manuSfcStepRepository;

        /// <summary>
        /// 条码信息表 仓储
        /// </summary>
        private readonly IManuSfcRepository _manuSfcRepository;

        /// <summary>
        /// 工单激活 仓储
        /// </summary>
        private readonly IPlanWorkOrderActivationRepository _planWorkOrderActivationRepository;

        /// <summary>
        /// 服务接口（生产通用）
        /// </summary>
        private readonly IManuCommonService _manuCommonService;

        /// <summary>
        /// 
        /// </summary>
        private readonly IManuCommonOldService _manuCommonOldService;

        /// <summary>
        /// 工单信息表 仓储
        /// </summary>
        private readonly IPlanWorkOrderRepository _planWorkOrderRepository;

        /// <summary>
        /// 工序仓储
        /// </summary>
        private readonly IProcProcedureRepository _procProcedureRepository;

        /// <summary>
        /// 
        /// </summary>
        private readonly ILocalizationService _localizationService;

        /// <summary>
        ///工艺路线工序节点明细仓储
        /// </summary>
        private readonly IProcProcessRouteDetailNodeRepository _procProcessRouteDetailNodeRepository;

        /// <summary>
        /// 容器装载表（物理删除）仓储
        /// </summary>
        private readonly IManuContainerPackRepository _manuContainerPackRepository;

        /// <summary>
        /// 条码信息表  仓储
        /// </summary>
        private readonly IManuSfcInfoRepository _manuSfcInfoRepository;

        /// <summary>
        /// 物料库存 仓储
        /// </summary>
        private readonly IWhMaterialInventoryRepository _whMaterialInventoryRepository;

        /// <summary>
        /// 物料维护 仓储
        /// </summary>
        private readonly IProcMaterialRepository _procMaterialRepository;

        /// <summary>
        /// 物料台账 仓储
        /// </summary>
        private readonly IWhMaterialStandingbookRepository _whMaterialStandingbookRepository;

        /// <summary>
        /// 工艺路线表 仓储
        /// </summary>
        private readonly IProcProcessRouteRepository _procProcessRouteRepository;

        /// <summary>
        /// BOM表 仓储
        /// </summary>
        private readonly IProcBomRepository _procBomRepository;

        private readonly IInteWorkCenterRepository _inteWorkCenterRepository;

        /// <summary>
        /// 仓储（工艺路线节点）
        /// </summary>
        private readonly IProcProcessRouteDetailNodeRepository _procProcessRouteNodeRepository;

        private readonly IManuDowngradingRepository _manuDowngradingRepository;

        private readonly IManuSfcScrapRepository _manuSfcScrapRepository;

        private readonly AbstractValidator<ManuSfcProduceLockDto> _validationLockRules;
        private readonly AbstractValidator<ManuSfcProduceModifyDto> _validationModifyRules;
        private readonly ILogger<ManuSfcProduceService> _logger;

        private readonly IInteVehiceFreightStackRepository _inteVehiceFreightStackRepository;

        private readonly IInteVehicleRepository _inteVehicleRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="currentSite"></param>
        /// <param name="manuSfcProduceRepository"></param>
        /// <param name="manuSfcStepRepository"></param>
        /// <param name="resourceRepository"></param>
        /// <param name="manuSfcRepository"></param>
        /// <param name="planWorkOrderActivationRepository"></param>
        /// <param name="planWorkOrderRepository"></param>
        /// <param name="manuCommonService"></param>
        /// <param name="manuCommonOldService"></param>
        /// <param name="procProcessRouteDetailNodeRepository"></param>
        /// <param name="procProcedureRepository"></param>
        /// <param name="localizationService"></param>
        /// <param name="manuContainerPackRepository"></param>
        /// <param name="manuSfcInfoRepository"></param>
        /// <param name="whMaterialInventoryRepository"></param>
        /// <param name="procMaterialRepository"></param>
        /// <param name="whMaterialStandingbookRepository"></param>
        /// <param name="procProcessRouteRepository"></param>
        /// <param name="procBomRepository"></param>
        /// <param name="inteWorkCenterRepository"></param>
        /// <param name="validationLockRules"></param>
        /// <param name="validationModifyRules"></param>
        /// <param name="logger"></param>
        /// <param name="procProcessRouteNodeRepository"></param>
        /// <param name="manuDowngradingRepository"></param>
        /// <param name="manuSfcScrapRepository"></param>
        /// <param name="inteVehiceFreightStackRepository"></param>
        /// <param name="inteVehicleRepository"></param>
        public ManuSfcProduceService(ICurrentUser currentUser, ICurrentSite currentSite,
            IManuSfcProduceRepository manuSfcProduceRepository,
            IManuSfcStepRepository manuSfcStepRepository,
            IProcResourceRepository resourceRepository,
            IManuSfcRepository manuSfcRepository,
            IPlanWorkOrderActivationRepository planWorkOrderActivationRepository,
            IPlanWorkOrderRepository planWorkOrderRepository,
            IManuCommonService manuCommonService,
            IManuCommonOldService manuCommonOldService,
            IProcProcessRouteDetailNodeRepository procProcessRouteDetailNodeRepository,
            IProcProcedureRepository procProcedureRepository,
            ILocalizationService localizationService,
            IManuContainerPackRepository manuContainerPackRepository,
            IManuSfcInfoRepository manuSfcInfoRepository,
            IWhMaterialInventoryRepository whMaterialInventoryRepository,
            IProcMaterialRepository procMaterialRepository,
            IWhMaterialStandingbookRepository whMaterialStandingbookRepository,
            IProcProcessRouteRepository procProcessRouteRepository,
            IProcBomRepository procBomRepository,
            IInteWorkCenterRepository inteWorkCenterRepository,
            AbstractValidator<ManuSfcProduceLockDto> validationLockRules,
            AbstractValidator<ManuSfcProduceModifyDto> validationModifyRules,
            ILogger<ManuSfcProduceService> logger,
            IProcProcessRouteDetailNodeRepository procProcessRouteNodeRepository,
            IManuDowngradingRepository manuDowngradingRepository,
            IManuSfcScrapRepository manuSfcScrapRepository, IInteVehiceFreightStackRepository inteVehiceFreightStackRepository,
            IInteVehicleRepository inteVehicleRepository)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _manuSfcProduceRepository = manuSfcProduceRepository;
            _manuSfcStepRepository = manuSfcStepRepository;
            _resourceRepository = resourceRepository;
            _manuSfcRepository = manuSfcRepository;
            _planWorkOrderActivationRepository = planWorkOrderActivationRepository;
            _planWorkOrderRepository = planWorkOrderRepository;
            _manuCommonService = manuCommonService;
            _manuCommonOldService = manuCommonOldService;
            _procProcedureRepository = procProcedureRepository;
            _procProcessRouteDetailNodeRepository = procProcessRouteDetailNodeRepository;
            _localizationService = localizationService;
            _manuContainerPackRepository = manuContainerPackRepository;
            _manuSfcInfoRepository = manuSfcInfoRepository;
            _whMaterialInventoryRepository = whMaterialInventoryRepository;
            _procMaterialRepository = procMaterialRepository;
            _whMaterialStandingbookRepository = whMaterialStandingbookRepository;
            _procProcessRouteRepository = procProcessRouteRepository;
            _procBomRepository = procBomRepository;
            _validationLockRules = validationLockRules;
            _validationModifyRules = validationModifyRules;
            this._logger = logger;
            _procProcessRouteNodeRepository = procProcessRouteNodeRepository;
            _manuDowngradingRepository = manuDowngradingRepository;
            _manuSfcScrapRepository = manuSfcScrapRepository;
            _inteWorkCenterRepository = inteWorkCenterRepository;
            _inteVehiceFreightStackRepository = inteVehiceFreightStackRepository;
            _inteVehicleRepository = inteVehicleRepository;
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="manuSfcProducePagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ManuSfcProduceViewDto>> GetPageListAsync(ManuSfcProducePagedQueryDto manuSfcProducePagedQueryDto)
        {
            var manuSfcProducePagedQuery = manuSfcProducePagedQueryDto.ToQuery<ManuSfcProducePagedQuery>();
            manuSfcProducePagedQuery.SiteId = _currentSite.SiteId;

            //查询多个条码
            if (manuSfcProducePagedQueryDto.Sfcs != null && manuSfcProducePagedQueryDto.Sfcs.Any())
            {
                manuSfcProducePagedQuery.SfcArray = manuSfcProducePagedQueryDto.Sfcs;
            }

            //根据资源查询
            if (manuSfcProducePagedQueryDto.ResourceId.HasValue)
            {
                var resource = await _resourceRepository.GetByIdAsync(manuSfcProducePagedQueryDto.ResourceId.Value);
                manuSfcProducePagedQuery.ResourceTypeId = resource.ResTypeId;
            }

            var pagedInfo = await _manuSfcProduceRepository.GetPagedInfoAsync(manuSfcProducePagedQuery);

            //实体到DTO转换 装载数据
            List<ManuSfcProduceViewDto> manuSfcProduceDtos = new List<ManuSfcProduceViewDto>();
            foreach (var item in pagedInfo.Data)
            {
                manuSfcProduceDtos.Add(new ManuSfcProduceViewDto
                {
                    Id = item.Id,
                    Sfc = item.Sfc,
                    //Lock = item.Lock,
                    //LockProductionId = item.LockProductionId,
                    ProcessRouteId = item.ProcessRouteId,
                    ProductBOMId = item.ProductBOMId,
                    ProcedureId = item.ProcedureId,
                    ProductId = item.ProductId,
                    Status = item.Status,
                    OrderCode = item.OrderCode,
                    Code = item.Code,
                    Name = item.Name,
                    MaterialCode = item.MaterialCode,
                    MaterialName = item.MaterialName,
                    Version = item.Version,
                    IsScrap = item.IsScrap,
                    ResCode = item.ResCode
                });
            }
            return new PagedInfo<ManuSfcProduceViewDto>(manuSfcProduceDtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="manuSfcProducePagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ManuSfcProduceViewDto>> GetPageListNewAsync(ManuSfcProducePagedQueryDto manuSfcProducePagedQueryDto)
        {
            var manuSfcProducePagedQuery = manuSfcProducePagedQueryDto.ToQuery<ManuSfcProducePagedQuery>();
            manuSfcProducePagedQuery.SiteId = _currentSite.SiteId;

            //实体到DTO转换 装载数据
            List<ManuSfcProduceViewDto> manuSfcProduceDtos = new List<ManuSfcProduceViewDto>();
            //查询多个条码
            if (manuSfcProducePagedQueryDto.Sfcs != null && manuSfcProducePagedQueryDto.Sfcs.Any())
            {
                manuSfcProducePagedQuery.SfcArray = manuSfcProducePagedQueryDto.Sfcs;
            }

            var pagedInfo = await _manuSfcProduceRepository.GetPagedListAsync(manuSfcProducePagedQuery);
            if (pagedInfo == null || !pagedInfo.Data.Any())
            {
                return new PagedInfo<ManuSfcProduceViewDto>(manuSfcProduceDtos, 1, 0, 0);
            }

            //查询工单
            var workOrders = await _planWorkOrderRepository.GetByIdsAsync(pagedInfo.Data.Where(x => x.WorkOrderId > 0).Select(x => x.WorkOrderId).Distinct().ToArray());
            //查询工序
            var procedures = await _procProcedureRepository.GetByIdsAsync(pagedInfo.Data.Where(x => x.ProcedureId > 0).Select(x => x.ProcedureId).Distinct().ToArray());
            //查询资源
            var resources = await _resourceRepository.GetListByIdsAsync(pagedInfo.Data.Where(x => x.ResourceId.HasValue && x.ResourceId.Value > 0).Select(x => x.ResourceId.GetValueOrDefault()).Distinct().ToArray());
            //查询物料
            var materials = await _procMaterialRepository.GetByIdsAsync(pagedInfo.Data.Where(x => x.ProductId > 0).Select(x => x.ProductId).Distinct().ToArray());

            //查询工单相关的工作中心信息
            var workCenterIds = workOrders != null && workOrders.Any() ? workOrders.Select(x => x.WorkCenterId ?? 0).Distinct().ToArray() : null;
            var workCenters = workCenterIds != null ? await _inteWorkCenterRepository.GetByIdsAsync(workCenterIds) : null;

            foreach (var item in pagedInfo.Data)
            {
                var workOrder = workOrders != null && workOrders.Any() ? workOrders.FirstOrDefault(x => x.Id == item.WorkOrderId) : null;
                var procedure = procedures != null && procedures.Any() ? procedures.FirstOrDefault(x => x.Id == item.ProcedureId) : null;
                var resource = resources != null && resources.Any() ? resources.FirstOrDefault(x => x.Id == item.ResourceId) : null;
                var material = materials != null && materials.Any() ? materials.FirstOrDefault(x => x.Id == item.ProductId) : null;

                var workCenter = workCenters != null && workCenters.Any() ? workCenters.FirstOrDefault(x => x.Id == workOrder?.WorkCenterId) : null;

                manuSfcProduceDtos.Add(new ManuSfcProduceViewDto
                {
                    Id = item.Id,
                    Sfc = item.SFC,
                    ProcessRouteId = item.ProcessRouteId,
                    ProductBOMId = item.ProductBOMId,
                    ProcedureId = item.ProcedureId,
                    ProductId = item.ProductId,
                    Status = item.Status,
                    OrderCode = workOrder?.OrderCode ?? "",
                    Code = procedure?.Code ?? "",
                    Name = procedure?.Name ?? "",
                    MaterialCode = material?.MaterialCode ?? "",
                    MaterialName = material?.MaterialName ?? "",
                    Version = material?.Version ?? "",
                    IsScrap = item.IsScrap,
                    ResCode = resource?.ResCode ?? ""
                });
            }
            return new PagedInfo<ManuSfcProduceViewDto>(manuSfcProduceDtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="manuSfcProduceCreateDto"></param>
        /// <returns></returns>
        public async Task CreateManuSfcProduceAsync(ManuSfcProduceCreateDto manuSfcProduceCreateDto)
        {
            //验证DTO

            //DTO转换实体
            var manuSfcProduceEntity = manuSfcProduceCreateDto.ToEntity<ManuSfcProduceEntity>();
            manuSfcProduceEntity.Id = IdGenProvider.Instance.CreateId();
            manuSfcProduceEntity.CreatedBy = _currentUser.UserName;
            manuSfcProduceEntity.UpdatedBy = _currentUser.UserName;

            //入库
            await _manuSfcProduceRepository.InsertAsync(manuSfcProduceEntity);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteManuSfcProduceAsync(long id)
        {
            await _manuSfcProduceRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeletesManuSfcProduceAsync(string ids)
        {
            var idsArr = ids.ToSpitLongArray();
            return await _manuSfcProduceRepository.DeleteRangeAsync(idsArr);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="manuSfcProduceModifyDto"></param>
        /// <returns></returns>
        public async Task ModifyManuSfcProduceAsync(ManuSfcProduceModifyDto manuSfcProduceModifyDto)
        {
            //验证DTO
            await _validationModifyRules.ValidateAndThrowAsync(manuSfcProduceModifyDto);

            //DTO转换实体
            var manuSfcProduceEntity = manuSfcProduceModifyDto.ToEntity<ManuSfcProduceEntity>();
            manuSfcProduceEntity.UpdatedBy = _currentUser.UserName;

            await _manuSfcProduceRepository.UpdateAsync(manuSfcProduceEntity);
        }

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ManuSfcProduceDto> QueryManuSfcProduceByIdAsync(long id)
        {
            var manuSfcProduceEntity = await _manuSfcProduceRepository.GetByIdAsync(id);
            if (manuSfcProduceEntity == null)
            {
                return new ManuSfcProduceDto();
            }
            return manuSfcProduceEntity.ToModel<ManuSfcProduceDto>();
        }

        /// <summary>
        /// 根据SFC查询
        /// </summary>
        /// <param name="sfc"></param>
        /// <returns></returns>
        public async Task<ManuSfcProduceDto> QueryManuSfcProduceBySFCAsync(string sfc)
        {
            var manuSfcProduceEntity = await _manuSfcProduceRepository.GetBySFCAsync(new ManuSfcProduceBySfcQuery()
            {
                SiteId = _currentSite.SiteId ?? 0,
                Sfc = sfc
            });
            if (manuSfcProduceEntity == null)
            {
                return new ManuSfcProduceDto();
            }
            return manuSfcProduceEntity.ToModel<ManuSfcProduceDto>();
        }

        #region 在制品步骤控制
        /// <summary>
        /// 分页查询（查询所有条码信息）
        /// </summary>
        /// <param name="manuSfcProducePagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ManuSfcProduceViewDto>> GetManuSfcPagedInfoAsync(ManuSfcProducePagedQueryDto manuSfcProducePagedQueryDto)
        {
            var manuSfcProducePagedQuery = manuSfcProducePagedQueryDto.ToQuery<ManuSfcProducePagedQuery>();
            manuSfcProducePagedQuery.SiteId = _currentSite.SiteId;

            //查询多个条码
            if (manuSfcProducePagedQueryDto.Sfcs != null && manuSfcProducePagedQueryDto.Sfcs.Any())
            {
                manuSfcProducePagedQuery.SfcArray = manuSfcProducePagedQueryDto.Sfcs;
            }

            //根据资源查询
            if (manuSfcProducePagedQueryDto.ResourceId.HasValue)
            {
                var resource = await _resourceRepository.GetByIdAsync(manuSfcProducePagedQueryDto.ResourceId.Value);
                manuSfcProducePagedQuery.ResourceTypeId = resource.ResTypeId;
            }

            var pagedInfo = await _manuSfcRepository.GetManuSfcPagedInfoAsync(manuSfcProducePagedQuery);
            if ((pagedInfo == null || !pagedInfo.Data.Any()) && manuSfcProducePagedQuery.SfcArray != null && manuSfcProducePagedQuery.SfcArray.Length > 0)
                throw new CustomerValidationException(nameof(ErrorCode.MES18022)).WithData("SFC", string.Join(",", manuSfcProducePagedQuery.SfcArray));

            //实体到DTO转换 装载数据
            List<ManuSfcProduceViewDto> manuSfcProduceDtos = new List<ManuSfcProduceViewDto>();
            foreach (var item in pagedInfo!.Data)
            {
                manuSfcProduceDtos.Add(new ManuSfcProduceViewDto
                {
                    Id = item.Id,
                    Sfc = item.Sfc,
                    Lock = item.Lock,
                    LockProductionId = item.LockProductionId,
                    ProductBOMId = item.ProductBOMId,
                    ProcedureId = item.ProcedureId,
                    Status = item.Status,
                    OrderCode = item.OrderCode,
                    Code = item.Code,
                    Name = item.Name,
                    MaterialCode = item.MaterialCode,
                    MaterialName = item.MaterialName,
                    Version = item.Version
                });
            }
            return new PagedInfo<ManuSfcProduceViewDto>(manuSfcProduceDtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 分页查询（查询所有条码信息）
        /// 优化
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ManuSfcProduceSelectViewDto>> GetManuSfcSelectPagedInfoAsync(ManuSfcProduceSelectPagedQueryDto queryDto)
        {
            var pagedQuery = queryDto.ToQuery<ManuSfcProduceSelectPagedQuery>();
            pagedQuery.SiteId = _currentSite.SiteId;

            //查询多个条码
            if (queryDto.Sfcs != null && queryDto.Sfcs.Any())
            {
                pagedQuery.SfcArray = queryDto.Sfcs;
            }
            pagedQuery.SfcStatus = SfcStatusEnum.Scrapping;

            return await GetManuSelectPagedInfoAsync(pagedQuery);
        }

        public async Task<PagedInfo<ManuSfcProduceSelectViewDto>> GetManuSfcPagedInfoAsync(ManuSfcProduceSelectPagedQueryDto queryDto)
        {
            var pagedQuery = queryDto.ToQuery<ManuSfcProduceSelectPagedQuery>();
            pagedQuery.SiteId = _currentSite.SiteId;

            //查询多个条码
            if (queryDto.Sfcs != null && queryDto.Sfcs.Any())
            {
                pagedQuery.SfcArray = queryDto.Sfcs;
            }

            return await GetManuSelectPagedInfoAsync(pagedQuery);
        }

        /// <summary>
        /// 组装选中数据
        /// </summary>
        /// <param name="pagedQuery"></param>
        /// <returns></returns>
        private async Task<PagedInfo<ManuSfcProduceSelectViewDto>> GetManuSelectPagedInfoAsync(ManuSfcProduceSelectPagedQuery pagedQuery)
        {
            var pagedInfo = await _manuSfcRepository.GetManuSfcSelectPagedInfoAsync(pagedQuery);
            if ((pagedInfo == null || !pagedInfo.Data.Any()) && pagedQuery.SfcArray != null && pagedQuery.SfcArray.Length > 0)
                throw new CustomerValidationException(nameof(ErrorCode.MES18022)).WithData("SFC", string.Join(",", pagedQuery.SfcArray));

            //实体到DTO转换 装载数据
            List<ManuSfcProduceSelectViewDto> manuSfcProduceDtos = new List<ManuSfcProduceSelectViewDto>();

            if (pagedInfo != null && pagedInfo.Data.Any())
            {
                //查询工单
                var workOrders = await _planWorkOrderRepository.GetByIdsAsync(pagedInfo.Data.Where(x => x.WorkOrderId > 0).Select(x => x.WorkOrderId).ToArray());

                var workCenters = await _inteWorkCenterRepository.GetByIdsAsync(workOrders.Select(x => x.WorkCenterId ?? 0).Distinct().ToArray());

                //查询工序
                var procedures = await _procProcedureRepository.GetByIdsAsync(pagedInfo.Data.Where(x => x.ProcedureId > 0).Select(x => x.ProcedureId).ToArray());

                //查询资源
                var resources = await _resourceRepository.GetListByIdsAsync(pagedInfo.Data.Where(x => x.ResourceId > 0).Select(x => x.ResourceId).ToArray());

                //查询物料
                var materials = await _procMaterialRepository.GetByIdsAsync(pagedInfo.Data.Where(x => x.ProductId > 0).Select(x => x.ProductId).ToArray());

                foreach (var item in pagedInfo.Data)
                {
                    var workOrder = workOrders != null && workOrders.Any() ? workOrders.FirstOrDefault(x => x.Id == item.WorkOrderId) : null;
                    var procedure = procedures != null && procedures.Any() ? procedures.FirstOrDefault(x => x.Id == item.ProcedureId) : null;
                    var resource = resources != null && resources.Any() ? resources.FirstOrDefault(x => x.Id == item.ResourceId) : null;
                    var material = materials != null && materials.Any() ? materials.FirstOrDefault(x => x.Id == item.ProductId) : null;

                    var workCenter = workCenters.FirstOrDefault(x => x.Id == workOrder?.WorkCenterId);

                    manuSfcProduceDtos.Add(new ManuSfcProduceSelectViewDto
                    {
                        Id = item.Id,
                        Sfc = item.Sfc,
                        Lock = item.Lock,
                        ProcessRouteId = item.ProcessRouteId,
                        LockProductionId = item.LockProductionId,
                        ProductBOMId = item.ProductBOMId,
                        ProcedureId = item.ProcedureId,
                        ProductId = item.ProductId,
                        Status = item.Status,
                        OrderCode = workOrder != null ? workOrder.OrderCode : "",
                        Code = procedure != null ? procedure.Code : "",
                        Name = procedure != null ? procedure.Name : "",
                        ResCode = resource != null ? resource.ResCode : "",
                        ResName = resource != null ? resource.ResName : "",
                        MaterialCode = material != null ? material.MaterialCode : "",
                        MaterialName = material != null ? material.MaterialName : "",
                        Version = material != null ? material.Version ?? "" : "",
                        Unit = material != null ? material.Unit ?? "" : "",

                        IsScrap = item.IsScrap,
                        Qty = item.Qty,
                        WorkCenterId = workCenter?.Id,
                        WorkCenterCode = workCenter?.Code,
                        WorkCenterName = workCenter?.Name,
                    });
                }

            }

            return new PagedInfo<ManuSfcProduceSelectViewDto>(manuSfcProduceDtos, pagedInfo!.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 根据SFC查询在制品步骤列表
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuSfcProduceStepViewDto>> QueryManuSfcProduceStepBySFCsAsync(List<ManuSfcProduceStepSFCDto> param)
        {
            #region 组装

            #region 组装工序
            var manuSfcs = param.Select(it => it.Sfc).ToArray();
            //获取条码信息
            var manuSfcInfos = await _manuCommonService.GetManuSfcInfos(new CoreServices.Bos.Common.MultiSFCBo { SFCs = manuSfcs, SiteId = _currentSite.SiteId ?? 0 }, _localizationService);
            long processRouteId = 0;
            if (manuSfcInfos != null && manuSfcInfos.Any())
            {
                if (manuSfcInfos.GroupBy(it => it.WorkOrderId).Distinct().Count() > 1)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES18002));
                }
                if (manuSfcInfos.GroupBy(it => it.ProcessRouteId).Distinct().Count() > 1)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES18004));
                }
                processRouteId = manuSfcInfos.Select(x => x.ProcessRouteId).FirstOrDefault();
            }
            var nodes = await _procProcessRouteNodeRepository.GetListAsync(new ProcProcessRouteDetailNodeQuery { ProcessRouteId = processRouteId });
            if (nodes == null || !nodes.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES18011));
            }
            var processRouteNodes = nodes.Where(it => it.ProcedureId != ProcessRoute.LastProcedureId).OrderBy(x => x.ManualSortNumber).Distinct();

            //组装工序
            var manuSfcProduceStepList = processRouteNodes.Select(item => new ManuSfcProduceStepViewDto
            {
                ProcedureId = item.ProcedureId,
                ProcedureCode = item.Code,
                ProcedureName = item.Name,
                Step = item.ManualSortNumber
            }).ToList();
            var endProcessRouteDetailId = processRouteNodes.OrderByDescending(it => it.SerialNo).FirstOrDefault()!.ProcedureId;

            #endregion

            #region 组装步骤数据
            var validationFailures = new List<ValidationFailure>();
            //为节点载入步骤数据
            foreach (var item in manuSfcInfos!)
            {
                if (item.ProcedureId.HasValue)
                {
                    var manuSfcProduceStep = manuSfcProduceStepList.FirstOrDefault(it => it.ProcedureId == item.ProcedureId);
                    var validationFailure = new ValidationFailure();
                    if (validationFailure.FormattedMessagePlaceholderValues == null || !validationFailure.FormattedMessagePlaceholderValues.Any())
                    {
                        validationFailure.FormattedMessagePlaceholderValues = new Dictionary<string, object> { { "CollectionIndex", item.SFC } };
                    }
                    else
                    {
                        validationFailure.FormattedMessagePlaceholderValues.Add("CollectionIndex", item.SFC);
                    }
                    if (manuSfcProduceStep == null)
                    {
                        validationFailure.ErrorCode = nameof(ErrorCode.MES18007);
                        validationFailures.Add(validationFailure);
                        continue;
                    }
                    switch (item.Status)
                    {
                        case SfcStatusEnum.lineUp:
                            manuSfcProduceStep.LineUpNumber += 1;
                            break;
                        case SfcStatusEnum.Activity:
                            manuSfcProduceStep.ActivityNumber += 1;
                            break;
                        case SfcStatusEnum.InProductionComplete:
                            manuSfcProduceStep.CompleteNumber += 1;
                            break;
                        default:
                            validationFailure.ErrorCode = nameof(ErrorCode.MES18001);
                            validationFailures.Add(validationFailure);
                            break;
                    }
                }
                else
                {
                    var manuSfcProduceStep = manuSfcProduceStepList.FirstOrDefault(it => it.ProcedureId == endProcessRouteDetailId);
                    if (manuSfcProduceStep == null)
                    {
                        var validationFailure = new ValidationFailure();
                        if (validationFailure.FormattedMessagePlaceholderValues == null || !validationFailure.FormattedMessagePlaceholderValues.Any())
                        {
                            validationFailure.FormattedMessagePlaceholderValues = new Dictionary<string, object> {
                            { "CollectionIndex", item}
                        };
                        }
                        else
                        {
                            validationFailure.FormattedMessagePlaceholderValues.Add("CollectionIndex", item);
                        }
                        validationFailure.ErrorCode = nameof(ErrorCode.MES18007);
                        validationFailures.Add(validationFailure);
                        continue;
                    }
                    manuSfcProduceStep.CompleteNumber += 1;
                }
            }

            //是否存在错误
            if (validationFailures.Any())
            {
                throw new ValidationException(_localizationService.GetResource("SFCError"), validationFailures);
            }
            #endregion

            #endregion
            return manuSfcProduceStepList;
        }

        /// <summary>
        /// 保存在制品步骤
        /// </summary>
        /// <param name="sfcProduceStepDto"></param>
        /// <returns></returns>
        public async Task SaveManuSfcProduceStepAsync(SaveManuSfcProduceStepDto sfcProduceStepDto)
        {
            if (sfcProduceStepDto.ProcedureId == 0) throw new CustomerValidationException(nameof(ErrorCode.MES18012));
            if (sfcProduceStepDto.Sfcs == null || !sfcProduceStepDto.Sfcs.Any()) throw new CustomerValidationException(nameof(ErrorCode.MES18014));

            var manuSfcs = sfcProduceStepDto.Sfcs.Select(it => it.Sfc).ToArray();

            var manuSfcEntitiesTask = _manuSfcRepository.GetListAsync(new ManuSfcQuery
            {
                SiteId = _currentSite.SiteId ?? 0,
                SFCs = manuSfcs,
                Type = SfcTypeEnum.Produce
            });
            var manuSfcProduceEntitiesTask = _manuSfcProduceRepository.GetListBySfcsAsync(new ManuSfcProduceBySfcsQuery { SiteId = _currentSite.SiteId ?? 0, Sfcs = manuSfcs });
            var sfcPackListTask = _manuContainerPackRepository.GetByLadeBarCodesAsync(new ManuContainerPackQuery { LadeBarCodes = manuSfcs, SiteId = _currentSite.SiteId ?? 0 });
            // 入库
            var whMaterialInventoryListTask = _whMaterialInventoryRepository.GetByBarCodesOfHasQtyAsync(new WhMaterialInventoryBarCodesQuery
            {
                SiteId = _currentSite.SiteId ?? 0,
                BarCodes = manuSfcs
            });



            var manuSfcEntities = await manuSfcEntitiesTask;
            var manuSfcProduceEntities = await manuSfcProduceEntitiesTask;
            var sfcPackList = await sfcPackListTask;
            var whMaterialInventoryList = await whMaterialInventoryListTask;
            var manuSfcInfoEntities = await _manuSfcInfoRepository.GetBySFCIdsWithIsUseAsync(manuSfcEntities.Select(x => x.Id));
            var planWorkOrderEntities = await _planWorkOrderRepository.GetByIdsAsync(manuSfcInfoEntities.Select(x => x.WorkOrderId ?? 0));
            var procMaterialEntities = await _procMaterialRepository.GetByIdsAsync(planWorkOrderEntities.Select(x => x.ProductId));
            IEnumerable<ManuSfcProduceBusinessEntity>? sfcProduceBusinessEntities = new List<ManuSfcProduceBusinessEntity>();
            if (manuSfcProduceEntities != null && manuSfcProduceEntities.Any())
            {
                var ids = manuSfcProduceEntities.Select(x => x.Id).ToList();
                if (ids.Any())
                {
                    sfcProduceBusinessEntities = await _manuSfcProduceRepository.GetSfcProduceBusinessBySFCIdsAsync(ids);
                }
            }

            var sfcStepList = new List<ManuSfcStepEntity>();
            var whMaterialStandingbookList = new List<WhMaterialStandingbookEntity>();
            var updateAddInventoryQuantityList = new List<UpdateQuantityRangeCommand>();
            var updateReduceInventoryQuantityList = new List<UpdateQuantityRangeCommand>();
            var addWhMaterialInventoryList = new List<WhMaterialInventoryEntity>();
            var updateProduceInStationSFCCommands = new List<UpdateProduceInStationSFCCommand>();
            var manuSfcProduceList = new List<ManuSfcProduceEntity>();
            var manuSfcUpdateStatusByIdCommands = new List<ManuSfcUpdateStatusByIdCommand>();
            var manuSfcUpdateRouteByIdCommands = new List<ManuSfcUpdateRouteByIdCommand>();
            var deleteSfcProduceBusinessIds = new List<long>();
            var deleteIds = new List<long>();
            var validationFailures = new List<ValidationFailure>();
            foreach (var item in manuSfcs)
            {
                var manuSfcEntity = manuSfcEntities.FirstOrDefault(x => x.SFC == item);

                if (manuSfcEntity == null)
                {
                    var validationFailure = new ValidationFailure();
                    if (validationFailure.FormattedMessagePlaceholderValues == null || !validationFailure.FormattedMessagePlaceholderValues.Any())
                    {
                        validationFailure.FormattedMessagePlaceholderValues = new Dictionary<string, object> {
                            { "CollectionIndex",item}};
                    }
                    else
                    {
                        validationFailure.FormattedMessagePlaceholderValues.Add("CollectionIndex", item);
                    }
                    validationFailure.ErrorCode = nameof(ErrorCode.MES16380);
                    validationFailures.Add(validationFailure);
                    continue;
                }

                if (ManuSfcStatus.ForbidSfcStatuss.Contains(manuSfcEntity.Status))
                {
                    var validationFailure = new ValidationFailure();
                    if (validationFailure.FormattedMessagePlaceholderValues == null || !validationFailure.FormattedMessagePlaceholderValues.Any())
                    {
                        validationFailure.FormattedMessagePlaceholderValues = new Dictionary<string, object> {
                            { "CollectionIndex", manuSfcEntity.SFC}};
                    }
                    else
                    {
                        validationFailure.FormattedMessagePlaceholderValues.Add("CollectionIndex", manuSfcEntity.SFC);
                    }
                    validationFailure.ErrorCode = nameof(ErrorCode.MES16378);
                    validationFailure.FormattedMessagePlaceholderValues.Add("Status", _localizationService.GetResource($"Hymson.MES.Core.Enums.manu.SfcStatusEnum.{SfcStatusEnum.GetName(manuSfcEntity.Status)}"));
                    validationFailures.Add(validationFailure);
                    continue;
                }

                if (sfcPackList.Any(x => x.LadeBarCode == manuSfcEntity.SFC))
                {
                    var validationFailure = new ValidationFailure();
                    if (validationFailure.FormattedMessagePlaceholderValues == null || !validationFailure.FormattedMessagePlaceholderValues.Any())
                    {
                        validationFailure.FormattedMessagePlaceholderValues = new Dictionary<string, object> {
                            { "CollectionIndex", manuSfcEntity.SFC}};
                    }
                    else
                    {
                        validationFailure.FormattedMessagePlaceholderValues.Add("CollectionIndex", manuSfcEntity.SFC);
                    }
                    validationFailure.ErrorCode = nameof(ErrorCode.MES16379);
                    validationFailures.Add(validationFailure);
                    continue;
                }
                var manuSfcInfoEntity = manuSfcInfoEntities.FirstOrDefault(x => x.SfcId == manuSfcEntity.Id);
                var planWorkOrderEntitity = planWorkOrderEntities.FirstOrDefault(x => x.Id == manuSfcInfoEntity?.WorkOrderId);
                if (planWorkOrderEntitity != null && planWorkOrderEntitity.Status == PlanWorkOrderStatusEnum.Pending)
                {
                    var validationFailure = new ValidationFailure();
                    if (validationFailure.FormattedMessagePlaceholderValues == null || !validationFailure.FormattedMessagePlaceholderValues.Any())
                    {
                        validationFailure.FormattedMessagePlaceholderValues = new Dictionary<string, object> {
                            { "CollectionIndex", manuSfcEntity.SFC}};
                    }
                    else
                    {
                        validationFailure.FormattedMessagePlaceholderValues.Add("CollectionIndex", manuSfcEntity.SFC);
                    }
                    validationFailure.FormattedMessagePlaceholderValues.Add("ordercode", planWorkOrderEntitity.OrderCode);
                    validationFailure.ErrorCode = nameof(ErrorCode.MES16302);
                    validationFailures.Add(validationFailure);
                    continue;
                }
                var manuSfcProduceEntity = manuSfcProduceEntities!.FirstOrDefault(x => x.SFC == item);

                sfcStepList.Add(new ManuSfcStepEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SiteId = _currentSite.SiteId ?? 0,
                    SFC = manuSfcEntity.SFC,
                    ProductId = manuSfcInfoEntity?.ProductId ?? 0,
                    ProcedureId = sfcProduceStepDto.ProcedureId,
                    Remark = sfcProduceStepDto.Remark ?? "",
                    WorkOrderId = manuSfcInfoEntity?.WorkOrderId ?? 0,
                    WorkCenterId = manuSfcProduceEntity?.WorkCenterId,
                    ProductBOMId = manuSfcProduceEntity?.ProductBOMId,
                    Qty = manuSfcEntity.Qty,
                    EquipmentId = manuSfcProduceEntity?.EquipmentId,
                    ResourceId = manuSfcProduceEntity?.ResourceId,
                    Operatetype = ManuSfcStepTypeEnum.StepControl,
                    CurrentStatus = manuSfcEntity.Status,
                    CreatedBy = _currentUser.UserName,
                    CreatedOn = HymsonClock.Now(),
                    UpdatedBy = _currentUser.UserName,
                    UpdatedOn = HymsonClock.Now()
                }
                    );

                // 界面的选择步骤操作类型
                switch (sfcProduceStepDto.Type)
                {
                    case SfcStepControlEnum.Complete:

                        if (manuSfcProduceEntity == null)
                        {
                            continue;
                        }
                        else
                        {
                            if (manuSfcProduceEntity.IsRepair == TrueOrFalseEnum.Yes)
                            {
                                var sfcProduceBusinessEntity = sfcProduceBusinessEntities.FirstOrDefault(x => x.SfcProduceId == manuSfcProduceEntity.Id && x.BusinessType == ManuSfcProduceBusinessType.Repair);
                                if (sfcProduceBusinessEntity == null)
                                {
                                    var validationFailure = new ValidationFailure();
                                    if (validationFailure.FormattedMessagePlaceholderValues == null || !validationFailure.FormattedMessagePlaceholderValues.Any())
                                    {
                                        validationFailure.FormattedMessagePlaceholderValues = new Dictionary<string, object> {
                            { "CollectionIndex", manuSfcEntity.SFC}};
                                    }
                                    else
                                    {
                                        validationFailure.FormattedMessagePlaceholderValues.Add("CollectionIndex", manuSfcEntity.SFC);
                                    }
                                    validationFailure.FormattedMessagePlaceholderValues.Add("ordercode", planWorkOrderEntitity!.OrderCode);
                                    validationFailure.ErrorCode = nameof(ErrorCode.MES16383);
                                    validationFailures.Add(validationFailure);
                                    continue;
                                }
                                var sfcProduceRepairBo = JsonSerializer.Deserialize<SfcProduceRepairBo>(sfcProduceBusinessEntity.BusinessContent);
                                manuSfcUpdateRouteByIdCommands.Add(new ManuSfcUpdateRouteByIdCommand
                                {
                                    Id = manuSfcProduceEntity.Id,
                                    ProcessRouteId = sfcProduceRepairBo?.ProcessRouteId ?? 0,
                                    ProcedureId = sfcProduceRepairBo?.ProcedureId ?? 0,
                                    Status = SfcStatusEnum.InProductionComplete,
                                    IsRepair = TrueOrFalseEnum.No,
                                    UpdatedBy = _currentUser.UserName,
                                    UpdatedOn = HymsonClock.Now()
                                });

                                deleteSfcProduceBusinessIds.Add(sfcProduceBusinessEntity?.Id ?? 0);
                                manuSfcUpdateStatusByIdCommands.Add(new ManuSfcUpdateStatusByIdCommand
                                {
                                    Id = manuSfcEntity.Id,
                                    Status = SfcStatusEnum.InProductionComplete,
                                    CurrentStatus = manuSfcEntity.Status,
                                    UpdatedOn = HymsonClock.Now(),
                                    UpdatedBy = _currentUser.UserName
                                });
                            }
                            else
                            {
                                manuSfcUpdateStatusByIdCommands.Add(new ManuSfcUpdateStatusByIdCommand
                                {
                                    Id = manuSfcEntity.Id,
                                    Status = SfcStatusEnum.Complete,
                                    CurrentStatus = manuSfcEntity.Status,
                                    UpdatedOn = HymsonClock.Now(),
                                    UpdatedBy = _currentUser.UserName
                                });
                                deleteIds.Add(manuSfcProduceEntity.Id);

                                if (whMaterialInventoryList != null && whMaterialInventoryList.Any(it => it.MaterialBarCode == item))
                                {
                                    updateAddInventoryQuantityList.Add(new UpdateQuantityRangeCommand
                                    {
                                        Status = WhMaterialInventoryStatusEnum.ToBeUsed,
                                        BarCode = item,
                                        QuantityResidue = manuSfcEntity.Qty,
                                        UpdatedBy = _currentUser.UserName,
                                        UpdatedOn = HymsonClock.Now()
                                    });
                                }
                                else
                                {
                                    // 物料库存
                                    addWhMaterialInventoryList.Add(new WhMaterialInventoryEntity
                                    {
                                        SupplierId = 0,//自制品 没有
                                        MaterialId = manuSfcInfoEntity?.ProductId ?? 0,
                                        MaterialBarCode = item,
                                        //Batch = "",//自制品 没有
                                        MaterialType = MaterialInventoryMaterialTypeEnum.SelfMadeParts,
                                        QuantityResidue = manuSfcEntity.Qty,
                                        Status = WhMaterialInventoryStatusEnum.ToBeUsed,
                                        Source = MaterialInventorySourceEnum.ManuComplete,
                                        SiteId = _currentSite.SiteId ?? 0,
                                        Id = IdGenProvider.Instance.CreateId(),
                                        CreatedBy = _currentUser.UserName,
                                        UpdatedBy = _currentUser.UserName,
                                        CreatedOn = HymsonClock.Now(),
                                        UpdatedOn = HymsonClock.Now()
                                    });

                                    var procMaterialEntitity = procMaterialEntities.FirstOrDefault(x => x.Id == manuSfcInfoEntity?.ProductId);

                                    // 台账数据
                                    whMaterialStandingbookList.Add(new WhMaterialStandingbookEntity
                                    {
                                        MaterialCode = procMaterialEntitity?.MaterialCode ?? "",
                                        MaterialName = procMaterialEntitity?.MaterialName ?? "",
                                        MaterialVersion = procMaterialEntitity?.Version ?? "",
                                        MaterialBarCode = item,
                                        //Batch = "",//自制品 没有
                                        Quantity = manuSfcEntity.Qty,
                                        Unit = procMaterialEntitity?.Unit ?? "",
                                        Type = WhMaterialInventoryTypeEnum.StepControl,
                                        Source = MaterialInventorySourceEnum.ManuComplete,
                                        SiteId = _currentSite.SiteId ?? 0,
                                        Id = IdGenProvider.Instance.CreateId(),
                                        CreatedBy = _currentUser.UserName,
                                        UpdatedBy = _currentUser.UserName,
                                        CreatedOn = HymsonClock.Now(),
                                        UpdatedOn = HymsonClock.Now()
                                    });
                                }
                            }
                        }

                        break;
                    case SfcStepControlEnum.InProductionComplete:
                    case SfcStepControlEnum.lineUp:
                    case SfcStepControlEnum.Activity:
                        SfcStatusEnum status = SfcStatusEnum.lineUp;
                        if (sfcProduceStepDto.Type == SfcStepControlEnum.InProductionComplete)
                        {
                            status = SfcStatusEnum.InProductionComplete;

                        }
                        else if (sfcProduceStepDto.Type == SfcStepControlEnum.Activity)
                        {
                            status = SfcStatusEnum.Activity;
                        }

                        manuSfcUpdateStatusByIdCommands.Add(new ManuSfcUpdateStatusByIdCommand
                        {
                            Id = manuSfcEntity.Id,
                            Status = status,
                            CurrentStatus = manuSfcEntity.Status,
                            UpdatedOn = HymsonClock.Now(),
                            UpdatedBy = _currentSite.Name
                        });

                        if (manuSfcProduceEntity == null)
                        {
                            manuSfcProduceList.Add(new ManuSfcProduceEntity
                            {
                                Id = IdGenProvider.Instance.CreateId(),
                                SiteId = _currentSite.SiteId ?? 0,
                                SFC = item,
                                ProductId = manuSfcInfoEntity?.ProductId ?? 0,
                                WorkOrderId = manuSfcInfoEntity?.WorkOrderId ?? 0,
                                SFCId = manuSfcEntity.Id,
                                BarCodeInfoId = manuSfcInfoEntity?.Id ?? 0,
                                ProcessRouteId = planWorkOrderEntitity?.ProcessRouteId ?? 0,
                                WorkCenterId = planWorkOrderEntitity?.WorkCenterId ?? 0,
                                ProductBOMId = planWorkOrderEntitity?.ProductBOMId ?? 0,
                                Qty = manuSfcEntity.Qty,
                                ProcedureId = sfcProduceStepDto.ProcedureId,
                                Status = status,
                                RepeatedCount = status == SfcStatusEnum.lineUp ? 0 : 1,
                                IsScrap = TrueOrFalseEnum.No,
                                CreatedBy = _currentUser.UserName,
                                UpdatedBy = _currentUser.UserName
                            });
                            if (whMaterialInventoryList != null && whMaterialInventoryList.Any(it => it.MaterialBarCode == item))
                            {
                                updateReduceInventoryQuantityList.Add(new UpdateQuantityRangeCommand
                                {
                                    Status = WhMaterialInventoryStatusEnum.ToBeUsed,
                                    BarCode = item,
                                    QuantityResidue = manuSfcEntity.Qty,
                                    UpdatedBy = _currentUser.UserName,
                                    UpdatedOn = HymsonClock.Now()
                                });
                            }
                        }
                        else
                        {
                            // 1991 如果条码处于首工序，且更改前状态为"排队中"，更改后状态为"活动中"，则需要更新工单的投入数量

                            // 修改在制品条码状态
                            updateProduceInStationSFCCommands.Add(new UpdateProduceInStationSFCCommand
                            {
                                Id = manuSfcProduceEntity.Id,
                                CurrentStatus = manuSfcEntity.Status,
                                Status = status,
                                ProcedureId = sfcProduceStepDto.ProcedureId,
                                RepeatedCount = 0,
                                UpdatedOn = HymsonClock.Now(),
                                UpdatedBy = _currentUser.UserName
                            });
                        }
                        break;
                    default:
                        break;
                }
            }

            if (validationFailures != null && validationFailures.Any())
            {
                throw new ValidationException(_localizationService.GetResource("SFCError"), validationFailures);
            }

            using var trans = TransactionHelper.GetTransactionScope();

            if (manuSfcUpdateStatusByIdCommands != null && manuSfcUpdateStatusByIdCommands.Any())
            {
                var row = await _manuSfcRepository.ManuSfcUpdateStatuByIdRangeAsync(manuSfcUpdateStatusByIdCommands);
                if (row != manuSfcUpdateStatusByIdCommands.Count)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES16381));
                }
            }

            if (updateProduceInStationSFCCommands != null && updateProduceInStationSFCCommands.Any())
            {
                await _manuSfcProduceRepository.UpdateProduceInStationSFCAsync(updateProduceInStationSFCCommands);
            }

            if (sfcStepList != null && sfcStepList.Any())
            {
                await _manuSfcStepRepository.InsertRangeAsync(sfcStepList);
            }

            if (manuSfcProduceList != null && manuSfcProduceList.Any())
            {
                await _manuSfcProduceRepository.InsertRangeAsync(manuSfcProduceList);
            }

            if (whMaterialStandingbookList != null && whMaterialStandingbookList.Any())
            {
                await _whMaterialStandingbookRepository.InsertsAsync(whMaterialStandingbookList);
            }

            if (updateReduceInventoryQuantityList != null && updateReduceInventoryQuantityList.Any())
            {
                await _whMaterialInventoryRepository.UpdateReduceQuantityResidueRangeAsync(updateReduceInventoryQuantityList);
            }

            if (updateAddInventoryQuantityList != null && updateAddInventoryQuantityList.Any())
            {
                await _whMaterialInventoryRepository.UpdateIncreaseQuantityResidueRangeAsync(updateAddInventoryQuantityList);
            }

            if (addWhMaterialInventoryList != null && addWhMaterialInventoryList.Any())
            {
                await _whMaterialInventoryRepository.InsertsAsync(addWhMaterialInventoryList);
            }

            if (deleteIds != null && deleteIds.Any())
            {
                var physicalDeleteSFCProduceByIdsCommand = new PhysicalDeleteSFCProduceByIdsCommand
                {
                    SiteId = _currentSite.SiteId ?? 0,
                    Ids = deleteIds,
                };
                await _manuSfcProduceRepository.DeletePhysicalRangeByIdsAsync(physicalDeleteSFCProduceByIdsCommand);
            }

            if (manuSfcUpdateRouteByIdCommands != null && manuSfcUpdateRouteByIdCommands.Any())
            {
                await _manuSfcProduceRepository.UpdateRouteByIdRangeAsync(manuSfcUpdateRouteByIdCommands);
            }

            if (deleteSfcProduceBusinessIds != null && deleteSfcProduceBusinessIds.Any())
            {
                await _manuSfcProduceRepository.DeleteSfcProduceBusinessByIdAsync(deleteSfcProduceBusinessIds);
            }
            trans.Complete();
        }
        #endregion

        /// <summary>
        /// 获取更改生产列表数据
        /// </summary>
        /// <param name="sfcs"></param>
        /// <returns></returns>
        public async Task<List<ManuUpdateViewDto>> GetManuUpdateListAsync(string[] sfcs)
        {
            var manuUpdateViewDtoList = new List<ManuUpdateViewDto>();
            //在制数据
            var manuSfcProduces = await ManuUpdateVerifyAsync(sfcs);
            //工序
            var procedureIds = manuSfcProduces.Select(it => it.ProcedureId).Distinct().ToArray();
            var proceduresTask = _procProcedureRepository.GetByIdsAsync(procedureIds);
            //产品
            var productIds = manuSfcProduces.Select(it => it.ProductId).Distinct().ToArray();
            var productsTask = _procMaterialRepository.GetByIdsAsync(productIds);
            //工艺路线
            var processRouteIds = manuSfcProduces.Select(it => it.ProcessRouteId).Distinct().ToArray();
            var processRoutesTask = _procProcessRouteRepository.GetByIdsAsync(processRouteIds);
            //bom
            var productBOMIds = manuSfcProduces.Select(it => it.ProductBOMId).Distinct().ToArray();
            var productBOMsTask = _procBomRepository.GetByIdsAsync(productBOMIds);
            //工单
            var workOrderIds = manuSfcProduces.Select(it => it.WorkOrderId).Distinct().ToArray();
            var planWorkOrdersTask = _planWorkOrderRepository.GetByIdsAsync(workOrderIds);

            var procedures = await proceduresTask;
            var products = await productsTask;
            var processRoutes = await processRoutesTask;
            var productBOMs = await productBOMsTask;
            var planWorkOrders = await planWorkOrdersTask;
            foreach (var item in manuSfcProduces)
            {
                var procedure = procedures.FirstOrDefault(it => it.Id == item.ProcedureId);
                var product = products.FirstOrDefault(it => it.Id == item.ProductId);
                var processRoute = processRoutes.FirstOrDefault(it => it.Id == item.ProcessRouteId);
                var productBOM = productBOMs.FirstOrDefault(it => it.Id == item.ProductBOMId);
                var workOrder = planWorkOrders.FirstOrDefault(it => it.Id == item.WorkOrderId);
                var manuUpdateViewDto = new ManuUpdateViewDto()
                {
                    SFC = item.SFC,
                    Status = item.Status,
                    OrderCode = workOrder?.OrderCode ?? "",
                    ProcedureCode = procedure?.Code ?? "",
                    MaterialAndVersion = product == null ? "" : product.MaterialCode + " / " + product.Version,
                    ProcessRouteAndVersion = processRoute == null ? "" : processRoute.Code + " / " + processRoute.Version,
                    BomAndVersion = productBOM == null ? "" : productBOM.BomCode + " / " + productBOM.Version,
                };
                manuUpdateViewDtoList.Add(manuUpdateViewDto);
            }
            return manuUpdateViewDtoList;
        }

        /// <summary>
        /// 获取更改生产工序列表数据
        /// </summary>
        /// <param name="workOrderId"></param>
        /// <returns></returns>
        public async Task<List<ManuUpdateProcedureViewDto>> GetProcedureByOrderIdListAsync(long workOrderId)
        {
            if (workOrderId <= 0)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES18202));
            }
            var order = await _planWorkOrderRepository.GetByIdAsync(workOrderId);
            return await GetProcedureByRouteIdListsync(order.ProcessRouteId);
        }

        /// <summary>
        /// 获取更改生产工序列表数据
        /// </summary>
        /// <param name="processRouteId"></param>
        /// <returns></returns>
        public async Task<List<ManuUpdateProcedureViewDto>> GetProcedureByRouteIdListsync(long processRouteId)
        {
            var processRouteDetailNode = await _procProcessRouteDetailNodeRepository.GetListAsync(new ProcProcessRouteDetailNodeQuery { ProcessRouteId = processRouteId });
            if (processRouteDetailNode == null || !processRouteDetailNode.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES18208));
            }

            var list = new List<ManuUpdateProcedureViewDto>();
            foreach (var item in processRouteDetailNode)
            {
                if (!string.IsNullOrWhiteSpace(item.Code))
                    list.Add(new ManuUpdateProcedureViewDto
                    {
                        ProcedureId = item.ProcedureId,
                        ProcedureCode = item.Code,
                        ProcedureName = item.Name,
                    });
            }
            return list;
        }

        /// <summary>
        /// 验证
        /// </summary>
        /// <param name="sfcs"></param>
        /// <param name="procedureId"></param>
        /// <returns></returns>
        private async Task<IEnumerable<ManuSfcProduceEntity>> ManuUpdateVerifyAsync(string[] sfcs, long procedureId = 0)
        {
            if (sfcs == null || sfcs.Length <= 0)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES18208));
            }
            if (sfcs.Length > 100)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES18200)).WithData("number", 100);
            }
            //验证条码 在制数据
            var manuSfcProduces = await _manuSfcProduceRepository.GetManuSfcProduceEntitiesAsync(new ManuSfcProduceQuery { SiteId = _currentSite.SiteId ?? 0, Sfcs = sfcs });
            if (manuSfcProduces == null || !manuSfcProduces.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16306));
            }

            var workOrderIdDistinct = manuSfcProduces.Select(it => it.WorkOrderId).Distinct();
            if (workOrderIdDistinct.Count() > 1)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES18206));
            }

            //验证条码状态
            //SfcStatusEnum?[] sfcStatusArr = { SfcStatusEnum.Activity, SfcStatusEnum.InProductionComplete, SfcStatusEnum.Complete, SfcStatusEnum.Locked,  SfcStatusEnum.Scrapping, SfcStatusEnum.Delete };
            //var sfcList = await _manuSfcRepository.GetManuSfcInfoEntitiesAsync(new ManuSfcStatusQuery { Sfcs = sfcs, Statuss = sfcStatusArr });
            var lineUpSfcs = manuSfcProduces.Where(x => x.Status != SfcStatusEnum.lineUp);
            if (lineUpSfcs != null && lineUpSfcs.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES18211));
            }

            //验证条码是否在不合格工艺路线排队
            var processRouteIds = manuSfcProduces.Select(x => x.ProcessRouteId).Distinct().ToList();
            var procProcessRoutes = await _procProcessRouteRepository.GetByIdsAsync(processRouteIds);
            var unqualifiedRoutes = procProcessRoutes.Where(x => x.Type == ProcessRouteTypeEnum.UnqualifiedRoute);
            if (unqualifiedRoutes != null && unqualifiedRoutes.Any())
            {
                var sfcInfoIds = manuSfcProduces.Where(it => unqualifiedRoutes.Any(x => x.Id == it.ProcessRouteId)).Select(it => it.SFC).ToArray();
                throw new CustomerValidationException(nameof(ErrorCode.MES18222)).WithData("SFC", string.Join(",", sfcInfoIds));
            }
            // 验证条码锁定
            //await _manuCommonService.VerifySfcsLockAsync(new ManuProcedureBo
            //{
            //    SiteId = _currentSite.SiteId ?? 0,
            //    SFCs = sfcs,
            //    ProcedureId = procedureId
            //});

            //工单
            var WorkOrderIds = manuSfcProduces.Select(it => it.WorkOrderId).ToArray();
            var workOrders = await _planWorkOrderRepository.GetByIdsAsync(WorkOrderIds);
            //PlanWorkOrderStatusEnum[] statusArr = { PlanWorkOrderStatusEnum.NotStarted, PlanWorkOrderStatusEnum.Finish, PlanWorkOrderStatusEnum.InProduction };
            //var workOrdersOrLosck = workOrders.Where(it => statusArr.Contains(it.Status) || it.Status == PlanWorkOrderStatusEnum.Pending);
            var workOrdersOrLosck = workOrders.Where(it => it.Status == PlanWorkOrderStatusEnum.Pending);
            if (workOrdersOrLosck != null && workOrdersOrLosck.Any())
            {
                var sfcInfoIds = manuSfcProduces.Where(it => workOrdersOrLosck.Any(order => order.Id == it.WorkOrderId)).Select(it => it.SFC).ToArray();
                throw new CustomerValidationException(nameof(ErrorCode.MES18205)).WithData("SFC", string.Join(",", sfcInfoIds));
            }

            var closedWorkOrders = workOrders.Where(it => it.Status == PlanWorkOrderStatusEnum.Closed);
            if (closedWorkOrders != null && closedWorkOrders.Any())
            {
                var sfcInfoIds = manuSfcProduces.Where(it => closedWorkOrders.Any(order => order.Id == it.WorkOrderId)).Select(it => it.SFC).ToArray();
                throw new CustomerValidationException(nameof(ErrorCode.MES18221)).WithData("SFC", string.Join(",", sfcInfoIds));
            }
            return manuSfcProduces;
        }

        /// <summary>
        /// 保存生产更改
        /// </summary>
        /// <param name="manuUpdateSaveDto"></param>
        /// <returns></returns>
        public async Task SaveManuUpdateListAsync(ManuUpdateSaveDto manuUpdateSaveDto)
        {
            #region 验证

            //验证必须更改了一项
            if (!manuUpdateSaveDto.WorkOrderId.HasValue && !manuUpdateSaveDto.MaterialId.HasValue && !manuUpdateSaveDto.BomId.HasValue && !manuUpdateSaveDto.ProcessRouteId.HasValue)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES18223));
            }

            //验证工序
            if ((manuUpdateSaveDto.WorkOrderId.HasValue && manuUpdateSaveDto.WorkOrderId.Value > 0) || (manuUpdateSaveDto.ProcessRouteId.HasValue && manuUpdateSaveDto.ProcessRouteId.Value > 0))
            {
                if (!manuUpdateSaveDto.ProcedureId.HasValue || manuUpdateSaveDto.ProcedureId <= 0)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES18203));
                }

                var procedureId = manuUpdateSaveDto.ProcedureId.Value;
                var procedure = await _procProcedureRepository.GetByIdAsync(procedureId);
                if (procedure == null)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES18214));
                }
            }

            #endregion

            #region 逻辑
            //在制数据
            var manuSfcProduces = await ManuUpdateVerifyAsync(manuUpdateSaveDto.Sfcs);
            //老工单
            var workOrderId = manuSfcProduces.First().WorkOrderId;
            var oldWorkOrderQty = 0m;// manuSfcProduces.Sum(it => it.Qty);
            var newWorkOrderQty = 0m;// manuSfcProduces.Sum(it => it.Qty);
            var planWorkOrderEntity = new PlanWorkOrderEntity();

            var newPlanWorkOrderEntity = new PlanWorkOrderEntity();
            if (manuUpdateSaveDto.WorkOrderId.HasValue && manuUpdateSaveDto.WorkOrderId.Value > 0)
            {
                planWorkOrderEntity = await _planWorkOrderRepository.GetByIdAsync(workOrderId);
                if (workOrderId == manuUpdateSaveDto.WorkOrderId)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES18212)).WithData("Code", planWorkOrderEntity.OrderCode);
                }

                //新工单
                newPlanWorkOrderEntity = await _planWorkOrderRepository.GetByIdAsync(manuUpdateSaveDto.WorkOrderId.Value);
                if (newPlanWorkOrderEntity == null)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES18231));
                }

                // PlanWorkOrderStatusEnum[] statusArr = { PlanWorkOrderStatusEnum.NotStarted, PlanWorkOrderStatusEnum.Pending, PlanWorkOrderStatusEnum.Closed };
                PlanWorkOrderStatusEnum[] statusArr = { PlanWorkOrderStatusEnum.NotStarted, PlanWorkOrderStatusEnum.SendDown, PlanWorkOrderStatusEnum.InProduction };
                var workOrdersOrLosck = !statusArr.Contains(newPlanWorkOrderEntity.Status);
                if (workOrdersOrLosck)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES18209)).WithData("Code", newPlanWorkOrderEntity.OrderCode);
                }

                //比对条码的产品和工单的产品是否一致，来决定是否扣减，条码和老工单产品一致增加老工单可下达数量，否则不增加
                //条码和新工单产品一致减少新工单可下达数量，否则不减少
                var oldProduct = planWorkOrderEntity?.ProductId ?? 0;
                var newProduct = newPlanWorkOrderEntity.ProductId;
                oldWorkOrderQty = manuSfcProduces.Where(x => x.ProductId == oldProduct).Sum(x => x.Qty);
                newWorkOrderQty = manuSfcProduces.Where(x => x.ProductId == newProduct).Sum(x => x.Qty);

                var orderRecord = await _planWorkOrderRepository.GetByWorkOrderIdAsync(newPlanWorkOrderEntity.Id);
                var PlanQuantity = newPlanWorkOrderEntity.Qty * (1 + newPlanWorkOrderEntity.OverScale / 100);
                var remainingQuantity = PlanQuantity - orderRecord.PassDownQuantity;
                if (remainingQuantity < newWorkOrderQty)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES18207));
                }
            }

            var productId = manuUpdateSaveDto.MaterialId.HasValue && manuUpdateSaveDto.MaterialId > 0 ? manuUpdateSaveDto.MaterialId.Value : newPlanWorkOrderEntity.ProductId;
            var processRouterId = manuUpdateSaveDto.ProcessRouteId.HasValue && manuUpdateSaveDto.ProcessRouteId > 0 ? manuUpdateSaveDto.ProcessRouteId.Value : newPlanWorkOrderEntity.ProcessRouteId;
            var bomId = manuUpdateSaveDto.BomId.HasValue && manuUpdateSaveDto.BomId > 0 ? manuUpdateSaveDto.BomId.Value : newPlanWorkOrderEntity.ProductBOMId;
            //组装步骤
            var sfcStepList = new List<ManuSfcStepEntity>();
            foreach (var item in manuSfcProduces)
            {
                item.ProcedureId = manuUpdateSaveDto.ProcedureId.HasValue ? manuUpdateSaveDto.ProcedureId.Value : item.ProcedureId;
                item.WorkOrderId = newPlanWorkOrderEntity.Id > 0 ? newPlanWorkOrderEntity.Id : item.WorkOrderId;
                item.ProductId = productId > 0 ? productId : item.ProductId;
                item.ProcessRouteId = processRouterId > 0 ? processRouterId : item.ProcessRouteId;
                item.ProductBOMId = bomId > 0 ? bomId : item.ProductBOMId;
                item.ResourceId = null; //更改步骤后 更改资源为null   为null则生产不限制匹配
                item.Status = SfcStatusEnum.lineUp;

                // 初始化步骤
                var sfcStep = new ManuSfcStepEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SiteId = _currentSite.SiteId ?? 0,
                    SFC = item.SFC,
                    ProductId = item.ProductId,
                    ProcedureId = item.ProcedureId,
                    Remark = manuUpdateSaveDto.Remark,
                    WorkOrderId = item.WorkOrderId,
                    WorkCenterId = item.WorkCenterId,
                    ProductBOMId = item.ProductBOMId,
                    Qty = item.Qty,
                    EquipmentId = item.EquipmentId,
                    ResourceId = item.ResourceId,
                    Operatetype = ManuSfcStepTypeEnum.ManuUpdate,
                    CurrentStatus = SfcStatusEnum.Activity,
                    CreatedBy = _currentUser.UserName,
                    CreatedOn = HymsonClock.Now(),
                    UpdatedBy = _currentUser.UserName,
                    UpdatedOn = HymsonClock.Now(),
                };
                sfcStepList.Add(sfcStep);
            }

            //处理条码信息 
            var newSfcInfos = new List<ManuSfcInfoEntity>();
            var manuSfcs = await _manuSfcRepository.GetListAsync(new ManuSfcQuery
            {
                SiteId = _currentSite.SiteId,
                SFCs = manuUpdateSaveDto.Sfcs
            });
            var sfcIds = manuSfcs.Select(it => it.Id).ToArray();
            var sfcInfos = await _manuSfcInfoRepository.GetBySFCIdsWithIsUseAsync(sfcIds);

            //更改老条码状态 这里不直接改老条码  而是新增新条码  便于追溯
            foreach (var item in sfcInfos)
            {
                item.IsUsed = false;
                item.UpdatedBy = _currentUser.UserName;
                item.UpdatedOn = HymsonClock.Now();

                var newSfcInfo = new ManuSfcInfoEntity()
                {
                    SfcId = item.SfcId,
                    WorkOrderId = manuUpdateSaveDto.WorkOrderId.HasValue && manuUpdateSaveDto.WorkOrderId > 0 ? manuUpdateSaveDto.WorkOrderId.Value : item.WorkOrderId,
                    ProductId = productId > 0 ? productId : item.ProductId,
                    ProcessRouteId = processRouterId > 0 ? processRouterId : item.ProcessRouteId,
                    ProductBOMId = bomId > 0 ? bomId : item.ProductBOMId,
                    IsUsed = true,
                    SiteId = _currentSite.SiteId ?? 0,
                    Id = IdGenProvider.Instance.CreateId(),
                    CreatedBy = _currentUser.UserName,
                    UpdatedBy = _currentUser.UserName,
                    CreatedOn = HymsonClock.Now(),
                    UpdatedOn = HymsonClock.Now()
                };
                newSfcInfos.Add(newSfcInfo);
            }

            using (var trans = TransactionHelper.GetTransactionScope())
            {
                //在制
                await _manuSfcProduceRepository.UpdateRangeAsync(manuSfcProduces);

                //如果更改了工单
                if (newPlanWorkOrderEntity.Id > 0)
                {
                    //新工单
                    await _planWorkOrderRepository.UpdatePassDownQuantityByWorkOrderIdAsync(new UpdatePassDownQuantityCommand
                    {
                        WorkOrderId = newPlanWorkOrderEntity.Id,
                        PlanQuantity = newPlanWorkOrderEntity.Qty * (1 + newPlanWorkOrderEntity.OverScale / 100),
                        PassDownQuantity = newWorkOrderQty,
                        UserName = _currentUser.UserName,
                        UpdateDate = HymsonClock.Now()
                    });

                    if (planWorkOrderEntity != null && planWorkOrderEntity.Id > 0)
                    {
                        //老工单
                        await _planWorkOrderRepository.UpdatePassDownQuantityByWorkOrderIdAsync(new UpdatePassDownQuantityCommand
                        {
                            WorkOrderId = workOrderId,
                            PlanQuantity = planWorkOrderEntity.Qty * (1 + planWorkOrderEntity.OverScale / 100),
                            PassDownQuantity = -oldWorkOrderQty,
                            UserName = _currentUser.UserName,
                            UpdateDate = HymsonClock.Now()
                        });
                    }
                }

                //条码信息更改
                await _manuSfcInfoRepository.UpdatesAsync(sfcInfos);
                await _manuSfcInfoRepository.InsertsAsync(newSfcInfos);

                //步骤
                await _manuSfcStepRepository.InsertRangeAsync(sfcStepList);
                trans.Complete();
            }
            #endregion
        }

        /// <summary>
        /// 获取工艺路线末尾工序
        /// </summary>
        /// <param name="processRouteId"></param>
        /// <returns></returns>
        public async Task<long> GetLastProcedureAsync(long processRouteId)
        {
            long id = 0;
            //获取工艺路线节点
            var processRouteNodes = await _manuCommonOldService.GetProcessRouteAsync(processRouteId);
            if (processRouteNodes.Any())
            {
                id = processRouteNodes?.FirstOrDefault()?.ProcedureIds.Last() ?? 0;
            }
            return id;
        }

        /// <summary>
        /// 验证将来工序
        /// </summary>
        /// <param name="sfcList"></param>
        /// <param name="lockProductionId"></param>
        /// <returns></returns>
        private async Task VeifyQualityLockProductionAsync(ManuSfcProduceEntity[] sfcList, long lockProductionId)
        {
            var processRouteId = sfcList.FirstOrDefault()?.ProcessRouteId ?? 0;
            var sfcProcedureIds = sfcList.Select(x => x.ProcedureId).Distinct().ToList();

            //判断将来工序是不是工艺路线上的工序，而且是选择的条码的接下来的工序
            var processRouteNodes = await _manuCommonOldService.GetProcessRouteAsync(processRouteId);
            if (processRouteNodes == null || !processRouteNodes.Any())
            {
                var procProcedureEntity = await _procProcedureRepository.GetByIdAsync(lockProductionId);
                throw new CustomerValidationException(nameof(ErrorCode.MES15310)).WithData("lockproduction", procProcedureEntity.Code);
            }

            //条码工序
            var procedureIds = processRouteNodes?.FirstOrDefault()?.ProcedureIds.ToList();
            if (procedureIds == null || !procedureIds.Any())
            {
                var procProcedureEntity = await _procProcedureRepository.GetByIdAsync(lockProductionId);
                throw new CustomerValidationException(nameof(ErrorCode.MES15310)).WithData("lockproduction", procProcedureEntity.Code);
            }

            if (!procedureIds.Contains(lockProductionId))
            {
                var procProcedureEntity = await _procProcedureRepository.GetByIdAsync(lockProductionId);
                throw new CustomerValidationException(nameof(ErrorCode.MES15310)).WithData("lockproduction", procProcedureEntity.Code);
            }

            var index = 0;
            sfcProcedureIds.ForEach(item =>
            {
                int sfcIndex = procedureIds.FindLastIndex(x => x.Equals(item));
                index = sfcIndex > index ? sfcIndex : index;
            });
            int sfcIndex = procedureIds.FindLastIndex(x => x.Equals(lockProductionId));
            if (sfcIndex <= index)
            {
                var procProcedureEntity = await _procProcedureRepository.GetByIdAsync(lockProductionId);
                throw new CustomerValidationException(nameof(ErrorCode.MES15317)).WithData("lockproduction", procProcedureEntity.Code);
            }
        }

        /// <summary>
        /// 根据sfcs查询条码信息关联降级等级 
        /// </summary>
        /// <param name="sfcs"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuSfcProduceAboutDowngradingViewDto>> GetManuSfcAboutManuDowngradingBySfcsAsync(string[] sfcs)
        {
            var sfcList = await _manuSfcRepository.GetManuSfcInfoEntitiesAsync(new ManuSfcStatusQuery { SiteId = _currentSite.SiteId ?? 0, Sfcs = sfcs });

            //实体到DTO转换 装载数据
            List<ManuSfcProduceAboutDowngradingViewDto> manuSfcProduceAboutDowngradingDtos = new List<ManuSfcProduceAboutDowngradingViewDto>();

            if (sfcList != null && sfcList.Any())
            {
                //查询工单
                var workOrders = await _planWorkOrderRepository.GetByIdsAsync(sfcList.Where(x => x.WorkOrderId > 0).Select(x => x.WorkOrderId).ToArray());

                //查询物料
                var materials = await _procMaterialRepository.GetByIdsAsync(sfcList.Where(x => x.ProductId > 0).Select(x => x.ProductId).ToArray());

                //查找降级
                var manuDowngradings = await _manuDowngradingRepository.GetBySfcsAsync(new ManuDowngradingBySfcsQuery
                {
                    Sfcs = sfcList.Select(x => x.SFC).ToArray(),
                    SiteId = _currentSite.SiteId ?? 0
                });

                //查询sfc对应的过程
                var sfcProduces = await _manuSfcProduceRepository.GetListBySfcsAsync(new ManuSfcProduceBySfcsQuery
                {
                    SiteId = _currentSite.SiteId ?? 0,
                    Sfcs = sfcList.Select(x => x.SFC).ToArray(),
                });
                //查找对应的工序
                var procedures = await _procProcedureRepository.GetByIdsAsync(sfcProduces.Where(x => x.ProcedureId > 0).Select(x => x.ProcedureId).ToArray());

                foreach (var item in sfcList)
                {
                    var workOrder = workOrders != null && workOrders.Any() ? workOrders.FirstOrDefault(x => x.Id == item.WorkOrderId) : null;
                    var material = materials != null && materials.Any() ? materials.FirstOrDefault(x => x.Id == item.ProductId) : null;

                    var manuDowngrading = manuDowngradings.FirstOrDefault(x => x.SFC == item.SFC);

                    //条码在制
                    var sfcProduce = sfcProduces != null && sfcProduces.Any() ? sfcProduces.FirstOrDefault(x => x.SFC == item.SFC) : null;

                    //工序
                    var procedure = procedures != null && procedures.Any() && sfcProduce != null ? procedures.FirstOrDefault(x => x.Id == sfcProduce.ProcedureId) : null;


                    var viewDto = new ManuSfcProduceAboutDowngradingViewDto
                    {
                        Id = item.Id,
                        Sfc = item.SFC,
                        OrderCode = workOrder != null ? workOrder.OrderCode : "",
                        MaterialCodeVersion = material != null ? material.MaterialCode + "/" + material.Version : "",
                        ProcedureCode = procedure?.Code ?? "",
                        ManuDowngradingCode = manuDowngrading?.Grade ?? ""
                    };

                    //找到对应的状态
                    if (ManuSfcStatus.SfcStatusInProcess.Contains(item.Status))
                    {
                        viewDto.Status = sfcProduce != null ? (int)sfcProduce.Status : (int)item.Status;
                    }
                    else
                    {
                        viewDto.Status = (int)item.Status;
                    }

                    manuSfcProduceAboutDowngradingDtos.Add(viewDto);
                }

            }

            return manuSfcProduceAboutDowngradingDtos;
        }

        /// <summary>
        /// 根据工序ID与资源ID获取活动的在制品
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ActivityManuSfcProduceViewDto>> GetActivityListByProcedureIdAndResIdAsync(ManuSfcProduceByProcedureIdAndResourceIdDto query)
        {
            var sfcProduceList = await _manuSfcProduceRepository.GetActivityListByProcedureIdStatusAsync(new ManuSfcProduceByProcedureIdStatusQuery
            {
                SiteId = _currentSite.SiteId ?? 0,
                ProcedureId = query.ProcedureId,
                ResourceId = query.ResourceId,
                Status = SfcStatusEnum.Activity
            });

            //实体到DTO转换 装载数据
            List<ActivityManuSfcProduceViewDto> manuSfcProduceDtos = new List<ActivityManuSfcProduceViewDto>();

            if (sfcProduceList.Any())
            {
                //查询工单
                var workOrders = await _planWorkOrderRepository.GetByIdsAsync(sfcProduceList.Where(x => x.WorkOrderId > 0).Select(x => x.WorkOrderId).ToArray());

                //查询物料
                var materials = await _procMaterialRepository.GetByIdsAsync(sfcProduceList.Where(x => x.ProductId > 0).Select(x => x.ProductId).ToArray());

                //查询开始时间-根据步骤表查询成功执行开始作业时间
                var sfcSteps = await _manuSfcStepRepository.GetSFCInStepAsync(new SfcInStepQuery()
                {
                    SiteId = _currentSite.SiteId ?? 0,
                    Sfcs = sfcProduceList.Select(x => x.SFC).Distinct().ToArray()
                });


                foreach (var item in sfcProduceList)
                {
                    var workOrder = workOrders != null && workOrders.Any() ? workOrders.FirstOrDefault(x => x.Id == item.WorkOrderId) : null;

                    var material = materials != null && materials.Any() ? materials.FirstOrDefault(x => x.Id == item.ProductId) : null;

                    var sfcStepTemps = sfcSteps.Where(x => x.SFC == item.SFC);

                    var lastNewInStepTime = DateTime.Now;
                    if (!sfcStepTemps.Any())
                    {
                        //加入需求：合并新增或者拆分新增的条码是没有入站步骤的，现在需要查询该条码是否是合并新增或者拆分新增的，是则取生成时间
                        var mergeOrSplitAddStep = await _manuSfcStepRepository.GetSfcMergeOrSplitAddStepAsync(new SfcMergeOrSplitAddStepQuery()
                        {
                            SiteId = _currentSite.SiteId ?? 0,
                            Sfc = item.SFC
                        });
                        if (mergeOrSplitAddStep == null)
                        {
                            throw new CustomerValidationException(nameof(ErrorCode.MES15321)).WithData("sfc", item.SFC);
                        }
                        lastNewInStepTime = mergeOrSplitAddStep.CreatedOn;

                    }
                    else lastNewInStepTime = sfcStepTemps.Max(x => x.CreatedOn);

                    manuSfcProduceDtos.Add(new ActivityManuSfcProduceViewDto
                    {
                        Id = item.Id,
                        Sfc = item.SFC,
                        Lock = item.Lock,
                        CreatedOn = lastNewInStepTime,

                        ProductId = item.ProductId,
                        OrderCode = workOrder != null ? workOrder.OrderCode : "",

                        MaterialCode = material != null ? material.MaterialCode : "",
                        MaterialName = material != null ? material.MaterialName : "",
                        Version = material != null ? material.Version ?? "" : "",

                    });
                }

                manuSfcProduceDtos = manuSfcProduceDtos.OrderByDescending(x => x.CreatedOn).ToList();
            }

            return manuSfcProduceDtos;
        }

        /// <summary>
        /// 根据工序与资源查询活动的载具
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        /// <exception cref="CustomerValidationException"></exception>
        public async Task<IEnumerable<ActivityVehicleViewDto>> GetVehicleActivityListByProcedureIdAndResIdAsync(ActivityVehicleByProcedureIdAndResourceIdDto query)
        {
            //实体到DTO转换 装载数据
            List<ActivityVehicleViewDto> activityVehicleViewDtos = new List<ActivityVehicleViewDto>();

            var sfcProduceList = await _manuSfcProduceRepository.GetActivityListByProcedureIdStatusAsync(new ManuSfcProduceByProcedureIdStatusQuery
            {
                SiteId = _currentSite.SiteId ?? 0,
                ProcedureId = query.ProcedureId,
                ResourceId = query.ResourceId,
                Status = SfcStatusEnum.Activity
            });

            if (!sfcProduceList.Any()) return activityVehicleViewDtos;

            //查询这些条码属于哪些载具
            var vehiceFreightStacks = await _inteVehiceFreightStackRepository.GetInteVehiceFreightStackEntitiesAsync(new InteVehiceFreightStackQuery
            {
                SiteId = _currentSite.SiteId ?? 0,
                Sfcs = sfcProduceList.Select(x => x.SFC).ToList()
            });

            if (!vehiceFreightStacks.Any()) return activityVehicleViewDtos;

            var vehiceFreightStackGroups = vehiceFreightStacks.GroupBy(x => x.VehicleId);

            //查询载具信息
            var vehicles = await _inteVehicleRepository.GetAboutVehicleTypeByIdsAsync(new InteVehicleIdsQuery
            {
                SiteId = _currentSite.SiteId ?? 0,
                Ids = vehiceFreightStackGroups.Select(x => x.Key).ToArray()
            });

            //查询载具里的物料信息
            var materials = await _procMaterialRepository.GetByIdsAsync(sfcProduceList.Where(x => x.ProductId > 0).Select(x => x.ProductId).ToArray());

            //查询开始时间-根据步骤表查询成功执行开始作业时间
            var sfcSteps = await _manuSfcStepRepository.GetSFCInStepAsync(new SfcInStepQuery()
            {
                SiteId = _currentSite.SiteId ?? 0,
                Sfcs = sfcProduceList.Select(x => x.SFC).Distinct().ToArray()
            });

            foreach (var item in vehicles)
            {
                //找到对应的条码下最后的一条
                var itemVehiceFreightStacks = vehiceFreightStackGroups.FirstOrDefault(x => x.Key == item.Id)?.ToList();
                if (itemVehiceFreightStacks == null || !itemVehiceFreightStacks.Any())
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES15322));
                }

                var sfcStepTemps = sfcSteps.Where(x => itemVehiceFreightStacks.Select(y => y.BarCode).Contains(x.SFC));
                if (!sfcStepTemps.Any())
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES15321)).WithData("sfc", item.Code);
                }

                var lastNewInStepTime = sfcStepTemps.Max(x => x.CreatedOn);

                //找载具内的一个条码
                var sfc = sfcProduceList.FirstOrDefault(x => itemVehiceFreightStacks.Select(y => y.BarCode).Contains(x.SFC));

                var material = materials.FirstOrDefault(x => x.Id == sfc?.ProductId);

                activityVehicleViewDtos.Add(new ActivityVehicleViewDto
                {
                    Id = item.Id,
                    Code = item.Code,
                    VehicleTypeId = item.VehicleTypeId,

                    Row = item.Row,
                    Column = item.Column,
                    BarCodeNum = itemVehiceFreightStacks.Count,

                    ProductId = material?.Id ?? 0,
                    MaterialCode = material?.MaterialCode ?? "",
                    MaterialName = material?.MaterialName ?? "",
                    MaterialVersion = material?.Version ?? "",

                    StartTime = lastNewInStepTime
                });

            }

            return activityVehicleViewDtos.OrderByDescending(x => x.StartTime).ToList();
        }

        /// <summary>
        /// 查询工序下排队中的载具分页信息
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<PagedInfo<InteVehicleViewDto>> GetVehicleLineUpPageByProcedureIdPagedInfoAsync(LineUpVehicleByProcedureIdDto query)
        {
            PagedInfo<InteVehicleViewDto> resultPaged = new PagedInfo<InteVehicleViewDto>(new List<InteVehicleViewDto>(), query.PageIndex, query.PageSize, 0);

            var sfcProduceList = await _manuSfcProduceRepository.GetActivityListByProcedureIdStatusAsync(new ManuSfcProduceByProcedureIdStatusQuery
            {
                SiteId = _currentSite.SiteId ?? 0,
                ProcedureId = query.ProcedureId,
                //ResourceId = query.ResourceId,
                Status = SfcStatusEnum.lineUp,

                MaterialCode = query.MaterialCode,
                MaterialVersion = query.MaterialVersion,
            });

            if (!sfcProduceList.Any()) return resultPaged;

            //查询这些条码属于哪些载具
            var vehiceFreightStacks = await _inteVehiceFreightStackRepository.GetInteVehiceFreightStackEntitiesAsync(new InteVehiceFreightStackQuery
            {
                SiteId = _currentSite.SiteId ?? 0,
                Sfcs = sfcProduceList.Select(x => x.SFC).ToList()
            });

            if (!vehiceFreightStacks.Any()) return resultPaged;

            var inteVehiclePagedQuery = query.ToQuery<InteVehiclePagedQuery>();
            inteVehiclePagedQuery.SiteId = _currentSite.SiteId ?? 0;
            inteVehiclePagedQuery.Ids = vehiceFreightStacks.Select(x => x.VehicleId).Distinct().ToArray();

            var pagedInfo = await _inteVehicleRepository.GetPagedInfoAsync(inteVehiclePagedQuery);

            //实体到DTO转换 装载数据
            List<InteVehicleViewDto> inteVehicleDtos = PrepareInteVehicleDtos(pagedInfo);
            return new PagedInfo<InteVehicleViewDto>(inteVehicleDtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pagedInfo"></param>
        /// <returns></returns>
        private static List<InteVehicleViewDto> PrepareInteVehicleDtos(PagedInfo<InteVehicleView> pagedInfo)
        {
            var inteVehicleDtos = new List<InteVehicleViewDto>();
            foreach (var inteVehicleView in pagedInfo.Data)
            {
                var inteVehicleDto = inteVehicleView.ToModel<InteVehicleViewDto>();
                inteVehicleDtos.Add(inteVehicleDto);
            }

            return inteVehicleDtos;
        }

        /// <summary>
        /// 分页查询（查询所有在制条码信息，加入载具）
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ManuUpdateViewDto>> GetManuSfcPageListAsync(ManuSfcProduceVehiclePagedQueryDto queryDto)
        {
            var pagedQuery = queryDto.ToQuery<ManuSfcProduceVehiclePagedQuery>();
            pagedQuery.SiteId = _currentSite.SiteId;

            //查询多个条码
            if (queryDto.Sfcs != null && queryDto.Sfcs.Any())
            {
                pagedQuery.SfcArray = queryDto.Sfcs;
            }

            var pagedInfo = await _manuSfcProduceRepository.GetManuSfcPageListAsync(pagedQuery);

            //实体到DTO转换 装载数据
            List<ManuUpdateViewDto> manuSfcProduceDtos = new List<ManuUpdateViewDto>();
            if (pagedInfo == null || !pagedInfo.Data.Any())
            {
                return new PagedInfo<ManuUpdateViewDto>(manuSfcProduceDtos, 1, 0, 0);
            }

            var data = pagedInfo.Data;
            //工序
            var procedureIds = data.Select(it => it.ProcedureId).Distinct().ToArray();
            var proceduresTask = _procProcedureRepository.GetByIdsAsync(procedureIds);
            //产品
            var productIds = data.Select(it => it.ProductId).Distinct().ToArray();
            var productsTask = _procMaterialRepository.GetByIdsAsync(productIds);
            //工艺路线
            var processRouteIds = data.Select(it => it.ProcessRouteId).Distinct().ToArray();
            var processRoutesTask = _procProcessRouteRepository.GetByIdsAsync(processRouteIds);
            //bom
            var productBOMIds = data.Select(it => it.ProductBOMId.GetValueOrDefault()).Distinct().ToArray();
            var productBOMsTask = _procBomRepository.GetByIdsAsync(productBOMIds);
            //工单
            var workOrderIds = data.Select(it => it.WorkOrderId).Distinct().ToArray();
            var planWorkOrdersTask = _planWorkOrderRepository.GetByIdsAsync(workOrderIds);
            //载具
            var vehicleIds = data.Select(it => it.VehicleId.GetValueOrDefault()).Distinct().ToArray();
            var vehiclesTask = _inteVehicleRepository.GetByIdsAsync(vehicleIds);

            var procedures = await proceduresTask;
            var products = await productsTask;
            var processRoutes = await processRoutesTask;
            var productBOMs = await productBOMsTask;
            var planWorkOrders = await planWorkOrdersTask;
            var vehicles = await vehiclesTask;
            foreach (var item in data)
            {
                var procedure = procedures.FirstOrDefault(it => it.Id == item.ProcedureId);
                var product = products.FirstOrDefault(it => it.Id == item.ProductId);
                var processRoute = processRoutes.FirstOrDefault(it => it.Id == item.ProcessRouteId);
                var productBOM = productBOMs.FirstOrDefault(it => it.Id == item.ProductBOMId);
                var workOrder = planWorkOrders.FirstOrDefault(it => it.Id == item.WorkOrderId);
                var vehicle = vehicles.FirstOrDefault(it => it.Id == item.VehicleId);
                var manuUpdateViewDto = new ManuUpdateViewDto()
                {
                    SFC = item.Sfc,
                    OrderCode = workOrder?.OrderCode ?? "",
                    VehicleCode = vehicle?.Code ?? "",
                    Status = item.Status,
                    ProcedureCode = procedure?.Code ?? "",
                    MaterialAndVersion = product == null ? "" : product.MaterialCode + " / " + product.Version,
                    ProcessRouteAndVersion = processRoute == null ? "" : processRoute.Code + " / " + processRoute.Version,
                    BomAndVersion = productBOM == null ? "" : productBOM.BomCode + " / " + productBOM.Version,
                };
                manuSfcProduceDtos.Add(manuUpdateViewDto);
            }

            return new PagedInfo<ManuUpdateViewDto>(manuSfcProduceDtos, pagedInfo!.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }
    }
}
