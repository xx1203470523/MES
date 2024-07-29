using Elastic.Clients.Elasticsearch.QueryDsl;
using FluentValidation;
using FluentValidation.Results;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.Localization.Services;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Domain.Plan;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Core.Domain.Warehouse;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Integrated;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.Core.Enums.Warehouse;
using Hymson.MES.CoreServices.Services.Manufacture.ManuGenerateBarcode;
using Hymson.MES.CoreServices.Services.Manufacture.WhMaterialInventory;
using Hymson.MES.Data.Repositories.Integrated;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Manufacture.ManuRequistionOrder;
using Hymson.MES.Data.Repositories.Plan;
using Hymson.MES.Data.Repositories.Plan.Query;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.Warehouse;
using Hymson.MES.Data.Repositories.Warehouse.WhMaterialInventory.Command;
using Hymson.MES.Data.Repositories.Warehouse.WhMaterialInventory.Query;
using Hymson.MES.HttpClients;
using Hymson.MES.HttpClients.Options;
using Hymson.MES.Services.Dtos.Warehouse;
using Hymson.Sequences;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;
using Hymson.Web.Framework.WorkContext;
using Microsoft.Extensions.Options;
using System.Transactions;

namespace Hymson.MES.Services.Services.Warehouse
{
    /// <summary>
    /// 物料库存 服务
    /// </summary>
    public class WhMaterialInventoryService : IWhMaterialInventoryService
    {
        private readonly ICurrentUser _currentUser;
        private readonly ICurrentSite _currentSite;

        /// <summary>
        /// 
        /// </summary>
        private readonly ILocalizationService _localizationService;

        /// <summary>
        /// 物料维护 仓储
        /// </summary>
        private readonly IProcMaterialRepository _procMaterialRepository;
        private readonly IProcBomRepository _procBomRepository;
        private readonly IProcBomDetailRepository _procBomDetailRepository;

        /// <summary>
        /// 物料库存 仓储
        /// </summary>
        private readonly IWhMaterialInventoryRepository _whMaterialInventoryRepository;
        private readonly IWhMaterialStandingbookRepository _whMaterialStandingbookRepository;
        private readonly IWhSupplierRepository _whSupplierRepository;

        /// <summary>
        /// 条码信息表 仓储
        /// </summary>
        private readonly IManuSfcRepository _manuSfcRepository;
        private readonly IManuSfcInfoRepository _manuSfcInfoRepository;

        private readonly AbstractValidator<WhMaterialInventoryCreateDto> _validationCreateRules;
        private readonly AbstractValidator<WhMaterialInventoryModifyDto> _validationModifyRules;
        private readonly AbstractValidator<MaterialBarCodeSplitAdjustDto> _validationBarCodeSplitAdjust;
        private readonly AbstractValidator<PickMaterialsRequest> _validationPickMaterialsRequest;
        private readonly IWhMaterialInventoryCoreService _whMaterialInventoryCoreService;
        //条码生成
        private readonly IInteCodeRulesRepository _inteCodeRulesRepository;
        private readonly IInteCodeRulesMakeRepository _inteCodeRulesMakeRepository;
        private readonly IManuGenerateBarcodeService _manuGenerateBarcodeService;
        //物料拆分
        private readonly IPlanWorkOrderRepository _planWorkOrderRepository;
        /// <summary>
        /// 条码关系
        /// </summary>
        private readonly IManuBarCodeRelationRepository _manuBarCodeRelationRepository;
        private readonly IManuSfcProduceRepository _sfcProduceRepository;
        /// <summary>
        /// 条码步骤表仓储 仓储
        /// </summary>
        private readonly IManuSfcStepRepository _manuSfcStepRepository;
        private readonly IManuRequistionOrderRepository _manuRequistionOrderRepository;
        private readonly IManuReturnOrderRepository _manuReturnOrderRepository;
        private readonly IManuReturnOrderDetailRepository _manuReturnOrderDetailRepository;
        private readonly IWMSApiClient _wmsRequest;
        private readonly IPlanWorkPlanRepository _planWorkPlanRepository;
        private readonly IPlanWorkPlanProductRepository _planWorkPlanProductRepository;
        private readonly IPlanWorkPlanMaterialRepository _planWorkPlanMaterialRepository;
        private readonly IManuProductReceiptOrderRepository _manuProductReceiptOrderRepository;
        /// <summary>
        /// 当前系统
        /// </summary>
        private readonly ICurrentSystem _currentSystem;

        private readonly IManuProductReceiptOrderDetailRepository _manuProductReceiptOrderDetailRepository;

        private readonly IOptions<WMSOptions> _wmsOptions;
        private readonly ISequenceService _sequenceService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="currentSite"></param>
        /// <param name="localizationService"></param>
        /// <param name="procMaterialRepository"></param>
        /// <param name="whMaterialInventoryRepository"></param>
        /// <param name="whMaterialStandingbookRepository"></param>
        /// <param name="whSupplierRepository"></param>
        /// <param name="manuSfcRepository"></param>
        /// <param name="manuSfcInfoRepository"></param>
        /// <param name="validationCreateRules"></param>
        /// <param name="validationModifyRules"></param>
        /// <param name="validationBarCodeSplitAdjust"></param>
        /// <param name="validationPickMaterialsRequest"></param>
        /// <param name="whMaterialInventoryCoreService"></param>
        /// <param name="inteCodeRulesRepository"></param>
        /// <param name="inteCodeRulesMakeRepository"></param>
        /// <param name="manuGenerateBarcodeService"></param>
        /// <param name="planWorkOrderRepository"></param>
        /// <param name="sfcProduceRepository"></param>
        /// <param name="manuSfcStepRepository"></param>
        /// <param name="manuRequistionOrderRepository"></param>
        /// <param name="wMSRequest"></param>
        /// <param name="procBomRepository"></param>
        /// <param name="procBomDetailRepository"></param>
        /// <param name="manuReturnOrderDetailRepository"></param>
        /// <param name="manuReturnOrderRepository"></param>
        /// <param name="planWorkPlanMaterialRepository"></param>
        /// <param name="planWorkPlanRepository"></param>
        /// <param name="planWorkPlanProductRepository"></param>
        /// <param name="manuBarCodeRelationRepository"></param>
        /// <param name="manuProductReceiptOrderRepository"></param>
        /// <param name="currentSystem"></param>
        /// <param name="manuProductReceiptOrderDetailRepository"></param>
        /// <param name="wmsOptions"></param>
        /// <param name="sequenceService"></param>
        public WhMaterialInventoryService(ICurrentUser currentUser, ICurrentSite currentSite,
            ILocalizationService localizationService,
            IProcMaterialRepository procMaterialRepository,
            IWhMaterialInventoryRepository whMaterialInventoryRepository,
            IWhMaterialStandingbookRepository whMaterialStandingbookRepository,
            IWhSupplierRepository whSupplierRepository,
            IManuSfcRepository manuSfcRepository,
            IManuSfcInfoRepository manuSfcInfoRepository,
            AbstractValidator<WhMaterialInventoryCreateDto> validationCreateRules,
            AbstractValidator<WhMaterialInventoryModifyDto> validationModifyRules,
            AbstractValidator<MaterialBarCodeSplitAdjustDto> validationBarCodeSplitAdjust,
            AbstractValidator<PickMaterialsRequest> validationPickMaterialsRequest,
            IWhMaterialInventoryCoreService whMaterialInventoryCoreService,
            IInteCodeRulesRepository inteCodeRulesRepository,
            IInteCodeRulesMakeRepository inteCodeRulesMakeRepository,
            IManuGenerateBarcodeService manuGenerateBarcodeService,
            IPlanWorkOrderRepository planWorkOrderRepository,
            IManuSfcProduceRepository sfcProduceRepository,
            IManuSfcStepRepository manuSfcStepRepository,
            IManuRequistionOrderRepository manuRequistionOrderRepository,
            IWMSApiClient wMSRequest,
            IProcBomRepository procBomRepository,
            IProcBomDetailRepository procBomDetailRepository,
            IManuReturnOrderDetailRepository manuReturnOrderDetailRepository,
            IManuReturnOrderRepository manuReturnOrderRepository,
            IPlanWorkPlanMaterialRepository planWorkPlanMaterialRepository,
            IPlanWorkPlanRepository planWorkPlanRepository,
            IPlanWorkPlanProductRepository planWorkPlanProductRepository,
            IManuBarCodeRelationRepository manuBarCodeRelationRepository,
            IManuProductReceiptOrderRepository manuProductReceiptOrderRepository,
            ICurrentSystem currentSystem,
            IManuProductReceiptOrderDetailRepository manuProductReceiptOrderDetailRepository,
            IOptions<WMSOptions> wmsOptions,
            ISequenceService sequenceService)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _localizationService = localizationService;
            _procMaterialRepository = procMaterialRepository;
            _whMaterialInventoryRepository = whMaterialInventoryRepository;
            _whMaterialStandingbookRepository = whMaterialStandingbookRepository;
            _whSupplierRepository = whSupplierRepository;
            _manuSfcRepository = manuSfcRepository;
            _manuSfcInfoRepository = manuSfcInfoRepository;
            _validationCreateRules = validationCreateRules;
            _validationModifyRules = validationModifyRules;
            _whMaterialInventoryCoreService = whMaterialInventoryCoreService;
            _validationBarCodeSplitAdjust = validationBarCodeSplitAdjust;
            _inteCodeRulesRepository = inteCodeRulesRepository;
            _inteCodeRulesMakeRepository = inteCodeRulesMakeRepository;
            _manuGenerateBarcodeService = manuGenerateBarcodeService;
            _planWorkOrderRepository = planWorkOrderRepository;
            _manuBarCodeRelationRepository = manuBarCodeRelationRepository;
            _sfcProduceRepository = sfcProduceRepository;
            _manuSfcStepRepository = manuSfcStepRepository;
            _validationPickMaterialsRequest = validationPickMaterialsRequest;
            _manuRequistionOrderRepository = manuRequistionOrderRepository;
            _wmsRequest = wMSRequest;
            _procBomRepository = procBomRepository;
            _procMaterialRepository = procMaterialRepository;
            _manuReturnOrderDetailRepository = manuReturnOrderDetailRepository;
            _manuReturnOrderRepository = manuReturnOrderRepository;
            _planWorkPlanRepository = planWorkPlanRepository;
            _planWorkPlanMaterialRepository = planWorkPlanMaterialRepository;
            _planWorkPlanProductRepository = planWorkPlanProductRepository;
            _manuProductReceiptOrderRepository = manuProductReceiptOrderRepository;
            _currentSystem = currentSystem;
            _manuProductReceiptOrderDetailRepository = manuProductReceiptOrderDetailRepository;
            _wmsOptions = wmsOptions;
            _sequenceService = sequenceService;
        }


        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="whMaterialInventoryCreateDto"></param>
        /// <returns></returns>
        public async Task CreateWhMaterialInventoryAsync(WhMaterialInventoryCreateDto whMaterialInventoryCreateDto)
        {
            // 验证DTO
            await _validationCreateRules.ValidateAndThrowAsync(whMaterialInventoryCreateDto);

            // DTO转换实体
            var whMaterialInventoryEntity = whMaterialInventoryCreateDto.ToEntity<WhMaterialInventoryEntity>();
            whMaterialInventoryEntity.Id = IdGenProvider.Instance.CreateId();
            whMaterialInventoryEntity.CreatedBy = _currentUser.UserName;
            whMaterialInventoryEntity.UpdatedBy = _currentUser.UserName;
            whMaterialInventoryEntity.CreatedOn = HymsonClock.Now();
            whMaterialInventoryEntity.UpdatedOn = HymsonClock.Now();
            whMaterialInventoryEntity.SiteId = _currentSite.SiteId ?? 0;

            // 入库
            await _whMaterialInventoryRepository.InsertAsync(whMaterialInventoryEntity);
        }

        /// <summary>
        /// 批量创建
        /// </summary>
        /// <param name="requestDtos"></param>
        /// <returns></returns> 
        public async Task CreateWhMaterialInventoryListAsync(IEnumerable<WhMaterialInventoryListCreateDto> requestDtos)
        {
            if (requestDtos == null || !requestDtos.Any()) throw new CustomerValidationException(nameof(ErrorCode.MES15106));

            var rows = await _whMaterialInventoryCoreService.MaterialInventoryAsync(new CoreServices.Bos.Manufacture.MaterialInventoryBo
            {
                SiteId = _currentSite.SiteId ?? 0,
                UserName = _currentUser.UserName,
                IsCheckSupplier = true,
                BarCodeList = requestDtos.Select(x => new CoreServices.Bos.Manufacture.MaterialInventorySfcInfoBo
                {
                    Source = x.Source,
                    MaterialId = x.MaterialId,
                    MaterialBarCode = x.MaterialBarCode,
                    QuantityResidue = x.QuantityResidue,
                    Batch = x.Batch,
                    Type = x.Type,
                    SupplierId = x.SupplierId,
                    Version = x.Version,
                    DueDate = x.DueDate,
                })
            });

            if (rows == 0) throw new CustomerValidationException(nameof(ErrorCode.MES15105));
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteWhMaterialInventoryAsync(long id)
        {
            await _whMaterialInventoryRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeletesWhMaterialInventoryAsync(string ids)
        {
            var idsArr = ids.ToSpitLongArray();
            return await _whMaterialInventoryRepository.DeletesAsync(idsArr);
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="whMaterialInventoryPagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<WhMaterialInventoryPageListViewDto>> GetPageListAsync(WhMaterialInventoryPagedQueryDto whMaterialInventoryPagedQueryDto)
        {
            var whMaterialInventoryPagedQuery = whMaterialInventoryPagedQueryDto.ToQuery<WhMaterialInventoryPagedQuery>();
            whMaterialInventoryPagedQuery.SiteId = _currentSite.SiteId ?? 0;
            var pagedInfo = await _whMaterialInventoryRepository.GetPagedInfoAsync(whMaterialInventoryPagedQuery);

            //实体到DTO转换 装载数据
            List<WhMaterialInventoryPageListViewDto> whMaterialInventoryDtos = PrepareWhMaterialInventoryDtos(pagedInfo);
            return new PagedInfo<WhMaterialInventoryPageListViewDto>(whMaterialInventoryDtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pagedInfo"></param>
        /// <returns></returns>
        private static List<WhMaterialInventoryPageListViewDto> PrepareWhMaterialInventoryDtos(PagedInfo<WhMaterialInventoryPageListView> pagedInfo)
        {
            var whMaterialInventoryDtos = new List<WhMaterialInventoryPageListViewDto>();
            foreach (var whMaterialInventoryEntity in pagedInfo.Data)
            {
                var whMaterialInventoryDto = whMaterialInventoryEntity.ToModel<WhMaterialInventoryPageListViewDto>();
                whMaterialInventoryDtos.Add(whMaterialInventoryDto);
            }

            return whMaterialInventoryDtos;
        }


        /// <summary>
        /// 查询是否已存在物料条码
        /// </summary>
        /// <param name="materialBarCode"></param>
        /// <returns></returns>
        public async Task<bool> CheckMaterialBarCodeAnyAsync(string materialBarCode)
        {
            var pagedInfo = await _whMaterialInventoryRepository.GetWhMaterialInventoryEntitiesAsync(new WhMaterialInventoryQuery
            {
                MaterialBarCode = materialBarCode,
                SiteId = _currentSite.SiteId ?? 0
            });

            if (pagedInfo != null && pagedInfo.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES15104)).WithData("MaterialCode", materialBarCode);
            }

            var sfcEntit = await _manuSfcRepository.GetSingleAsync(new ManuSfcQuery
            {
                SFC = materialBarCode,
                SiteId = _currentSite.SiteId ?? 0,
                Type = SfcTypeEnum.Produce
            });
            if (sfcEntit != null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES152016)).WithData("MaterialCode", materialBarCode);
            }
            return false;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="whMaterialInventoryModifyDto"></param>
        /// <returns></returns>
        public async Task ModifyWhMaterialInventoryAsync(WhMaterialInventoryModifyDto whMaterialInventoryModifyDto)
        {
            //验证DTO
            await _validationModifyRules.ValidateAndThrowAsync(whMaterialInventoryModifyDto);

            //DTO转换实体
            var whMaterialInventoryEntity = whMaterialInventoryModifyDto.ToEntity<WhMaterialInventoryEntity>();
            whMaterialInventoryEntity.UpdatedBy = _currentUser.UserName;
            whMaterialInventoryEntity.UpdatedOn = HymsonClock.Now();

            await _whMaterialInventoryRepository.UpdateAsync(whMaterialInventoryEntity);
        }

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<WhMaterialInventoryDto> QueryWhMaterialInventoryByIdAsync(long id)
        {
            var whMaterialInventoryEntity = await _whMaterialInventoryRepository.GetByIdAsync(id);
            if (whMaterialInventoryEntity != null)
            {
                return whMaterialInventoryEntity.ToModel<WhMaterialInventoryDto>();
            }
            return new WhMaterialInventoryDto();
        }

        /// <summary>
        /// 根据物料条码查询
        /// </summary>
        /// <param name="barCode"></param>
        /// <returns></returns>
        public async Task<WhMaterialInventoryDto?> QueryWhMaterialInventoryByBarCodeAsync(string barCode)
        {
            var entity = await _whMaterialInventoryRepository.GetByBarCodeAsync(new WhMaterialInventoryBarCodeQuery
            {
                SiteId = _currentSite.SiteId,
                BarCode = barCode
            }) ?? throw new CustomerValidationException(nameof(ErrorCode.MES152017)).WithData("Code", barCode);

            /*
            // 只有【待使用】的库存才能上料！
            if (entity.Status != WhMaterialInventoryStatusEnum.ToBeUsed) throw new CustomerValidationException(nameof(ErrorCode.MES152018));
            */

            return entity.ToModel<WhMaterialInventoryDto>();
        }

        public async Task<WhMaterialInventoryPageListViewDto?> QueryWhMaterialBarCodeAsync(string barCode)
        {
            var infoList = await _whMaterialInventoryRepository.GetPagedInfoAsync(new WhMaterialInventoryPagedQuery { MaterialBarCode = barCode, SiteId = _currentSite.SiteId ?? 0, PageIndex = 1, PageSize = 1 });

            if (infoList.TotalCount < 1) throw new CustomerValidationException(nameof(ErrorCode.MES152017)).WithData("Code", barCode);
            var result = new WhMaterialInventoryPageListViewDto();
            if (infoList != null)
            {
                var entityOne = infoList?.Data.FirstOrDefault();
                result = entityOne?.ToModel<WhMaterialInventoryPageListViewDto>();
            }

            return result;
        }


        /// <summary>
        /// 根据物料编码查询物料与供应商信息
        /// </summary>
        /// <param name="materialId"></param>
        /// <returns></returns>
        public async Task<ProcMaterialInfoViewDto> GetMaterialAndSupplierByMateialCodeIdAsync(long materialId)
        {
            var materialInfo = await _whMaterialInventoryRepository.GetProcMaterialByMaterialCodeAsync(materialId);
            if (materialInfo == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES15101));
            }
            var supplierInfo = await _whMaterialInventoryRepository.GetWhSupplierByMaterialIdAsync(new WhSupplierByMaterialCommand { MaterialId = materialInfo.Id, SiteId = _currentSite.SiteId ?? 0 });
            if (!supplierInfo.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES15102));
            }
            ProcMaterialInfoViewDto dto = new ProcMaterialInfoViewDto();
            dto.MaterialInfo = materialInfo;
            dto.SupplierInfo = supplierInfo;
            return dto;
        }

        /// <summary>
        /// 根据查询条件获取分页数据 来源外部的
        /// </summary>
        /// <param name="whMaterialInventoryPagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<WhMaterialInventoryPageListViewDto>> GetOutsidePageListAsync(WhMaterialInventoryPagedQueryDto whMaterialInventoryPagedQueryDto)
        {
            var whMaterialInventoryPagedQuery = whMaterialInventoryPagedQueryDto.ToQuery<WhMaterialInventoryPagedQuery>();
            whMaterialInventoryPagedQuery.SiteId = _currentSite.SiteId ?? 0;
            //whMaterialInventoryPagedQuery.Sources = new MaterialInventorySourceEnum[] { MaterialInventorySourceEnum.ManualEntry, MaterialInventorySourceEnum.WMS, MaterialInventorySourceEnum.LoadingPoint };//只查询外部的
            var pagedInfo = await _whMaterialInventoryRepository.GetPagedInfoAsync(whMaterialInventoryPagedQuery);

            //实体到DTO转换 装载数据
            List<WhMaterialInventoryPageListViewDto> whMaterialInventoryDtos = PrepareWhMaterialInventoryDtos(pagedInfo);
            return new PagedInfo<WhMaterialInventoryPageListViewDto>(whMaterialInventoryDtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 根据物料条码查询 来源外部的数据
        /// </summary>
        /// <param name="barCode"></param>
        /// <returns></returns>
        public async Task<WhMaterialInventoryDetailDto?> QueryOutsideWhMaterialInventoryByBarCodeAsync(string barCode)
        {
            var entity = await _whMaterialInventoryRepository.GetByBarCodeAsync(new WhMaterialInventoryBarCodeQuery { SiteId = _currentSite.SiteId, BarCode = barCode });
            if (entity == null) throw new CustomerValidationException(nameof(ErrorCode.MES15124));

            //if (new MaterialInventorySourceEnum[] { MaterialInventorySourceEnum.ManualEntry, MaterialInventorySourceEnum.WMS, MaterialInventorySourceEnum.LoadingPoint }.Contains(entity.Source))
            //{
            var detailDto = entity.ToModel<WhMaterialInventoryDetailDto>();

            //查询关联信息
            var materialInfo = (await _procMaterialRepository.GetByIdsAsync(new long[] { entity!.MaterialId })).FirstOrDefault();
            var supplierInfo = (await _whSupplierRepository.GetByIdsAsync(new long[] { entity.SupplierId })).FirstOrDefault();

            detailDto.MaterialCode = materialInfo?.MaterialCode ?? "";
            detailDto.MaterialName = materialInfo?.MaterialName ?? "";
            detailDto.MaterialVersion = materialInfo?.Version ?? "";

            detailDto.SupplierCode = supplierInfo?.Code ?? "";
            detailDto.SupplierName = supplierInfo?.Name ?? "";


            return detailDto;
            //}
            //else
            //{
            //    throw new CustomerValidationException(nameof(ErrorCode.MES15120));
            //}
        }

        /// <summary>
        /// 获取物料库存相关的信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="CustomerValidationException"></exception>
        public async Task<WhMaterialInventoryDetailDto> QueryWhMaterialInventoryDetailByIdAsync(long id)
        {
            var entitys = await _whMaterialInventoryRepository.GetByIdsAsync(new long[] { id });
            if (entitys != null && entitys.Any())
            {
                var entity = entitys.FirstOrDefault();

                var detailDto = entity!.ToModel<WhMaterialInventoryDetailDto>();

                //查询关联信息
                var materialInfo = (await _procMaterialRepository.GetByIdsAsync(new long[] { entity!.MaterialId })).FirstOrDefault();
                var supplierInfo = (await _whSupplierRepository.GetByIdsAsync(new long[] { entity.SupplierId })).FirstOrDefault();

                detailDto.MaterialCode = materialInfo?.MaterialCode ?? "";
                detailDto.MaterialName = materialInfo?.MaterialName ?? "";
                detailDto.MaterialVersion = materialInfo?.Version ?? "";

                detailDto.SupplierCode = supplierInfo?.Code ?? "";
                detailDto.SupplierName = supplierInfo?.Name ?? "";

                return detailDto;
            }
            else
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES15124));
            }
        }

        /// <summary>
        /// 修改外部来源库存
        /// </summary>
        /// <param name="modifyDto"></param>
        /// <returns></returns>
        public async Task UpdateOutsideWhMaterialInventoryAsync(OutsideWhMaterialInventoryModifyDto modifyDto)
        {
            //查询到库存的信息
            var oldWhMIEntirty = await _whMaterialInventoryRepository.GetByIdAsync(modifyDto.Id);

            if (oldWhMIEntirty == null || oldWhMIEntirty.IsDeleted > 0)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES15124));
            }

            //if (!new MaterialInventorySourceEnum[] { MaterialInventorySourceEnum.ManualEntry, MaterialInventorySourceEnum.WMS, MaterialInventorySourceEnum.LoadingPoint }.Contains(oldWhMIEntirty.Source))
            //{
            //    throw new CustomerValidationException(nameof(ErrorCode.MES15123));
            //}

            if (modifyDto.QuantityResidue < 0 || modifyDto.QuantityResidue > oldWhMIEntirty.ReceivedQty)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES15121));
            }

            if (oldWhMIEntirty.Status == WhMaterialInventoryStatusEnum.InUse || oldWhMIEntirty.Status == WhMaterialInventoryStatusEnum.Locked)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES15122)).WithData("materialBarCode", oldWhMIEntirty.MaterialBarCode).WithData("status", _localizationService.GetResource($"{typeof(WhMaterialInventoryStatusEnum).FullName}.{oldWhMIEntirty.Status.ToString()}"));
            }

            var whMaterialInventoryEntity = new WhMaterialInventoryEntity();
            whMaterialInventoryEntity.UpdatedBy = _currentUser.UserName;
            whMaterialInventoryEntity.UpdatedOn = HymsonClock.Now();

            whMaterialInventoryEntity.Id = modifyDto.Id;
            whMaterialInventoryEntity.MaterialId = modifyDto.MaterialId;
            whMaterialInventoryEntity.QuantityResidue = modifyDto.QuantityResidue;
            whMaterialInventoryEntity.Batch = modifyDto.Batch;
            whMaterialInventoryEntity.SupplierId = modifyDto.SupplierId;

            //判断要修改的条码在不在在制品表，如果在记录步骤信息
            var sfcProduceEntity = await _sfcProduceRepository.GetBySFCAsync(new ManuSfcProduceBySfcQuery
            {
                SiteId = _currentSite.SiteId ?? 0,
                Sfc = oldWhMIEntirty.MaterialBarCode
            });
            var manuSfcStepEntity = new ManuSfcStepEntity();
            if (sfcProduceEntity != null)
            {
                //步骤表-不良录入
                manuSfcStepEntity = new ManuSfcStepEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SFC = sfcProduceEntity.SFC,
                    ProductId = sfcProduceEntity.ProductId,
                    WorkOrderId = sfcProduceEntity.WorkOrderId,
                    WorkCenterId = sfcProduceEntity.WorkCenterId,
                    ProductBOMId = sfcProduceEntity.ProductBOMId,
                    ProcessRouteId = sfcProduceEntity.ProcessRouteId,
                    Qty = whMaterialInventoryEntity.QuantityResidue,
                    EquipmentId = sfcProduceEntity.EquipmentId,
                    ResourceId = sfcProduceEntity.ResourceId,
                    ProcedureId = sfcProduceEntity.ProcedureId,
                    Operatetype = ManuSfcStepTypeEnum.InventoryModify,
                    CurrentStatus = sfcProduceEntity.Status,
                    SiteId = _currentSite.SiteId ?? 0,
                    CreatedOn = HymsonClock.Now(),
                    CreatedBy = _currentUser.UserName,
                    UpdatedOn = HymsonClock.Now(),
                    UpdatedBy = _currentUser.UserName,
                };
            }
            #region  处理得到记录
            //查询到物料信息
            var materialInfo = await _procMaterialRepository.GetByIdAsync(modifyDto.MaterialId);

            var whMaterialStandingbookEntity = new WhMaterialStandingbookEntity();
            whMaterialStandingbookEntity.MaterialCode = materialInfo.MaterialCode;
            whMaterialStandingbookEntity.MaterialName = materialInfo.MaterialName;
            whMaterialStandingbookEntity.MaterialVersion = materialInfo.Version ?? "";
            whMaterialStandingbookEntity.Unit = materialInfo.Unit ?? "";

            whMaterialStandingbookEntity.MaterialBarCode = oldWhMIEntirty.MaterialBarCode;
            whMaterialStandingbookEntity.Type = WhMaterialInventoryTypeEnum.InventoryModify;
            whMaterialStandingbookEntity.Source = MaterialInventorySourceEnum.InventoryModify;
            whMaterialStandingbookEntity.SiteId = _currentSite.SiteId ?? 0;

            whMaterialStandingbookEntity.Batch = whMaterialInventoryEntity.Batch;
            whMaterialStandingbookEntity.Quantity = whMaterialInventoryEntity.QuantityResidue;
            whMaterialStandingbookEntity.SupplierId = whMaterialInventoryEntity.SupplierId;

            whMaterialStandingbookEntity.Id = IdGenProvider.Instance.CreateId();
            whMaterialStandingbookEntity.CreatedBy = _currentUser.UserName;
            whMaterialStandingbookEntity.UpdatedBy = _currentUser.UserName;
            whMaterialStandingbookEntity.CreatedOn = HymsonClock.Now();
            whMaterialStandingbookEntity.UpdatedOn = HymsonClock.Now();
            #endregion

            using (var trans = TransactionHelper.GetTransactionScope())
            {
                //插入步骤表
                if (sfcProduceEntity != null)
                {
                    await _manuSfcStepRepository.InsertAsync(manuSfcStepEntity);
                }
                await _whMaterialInventoryRepository.UpdateOutsideWhMaterilInventoryAsync(whMaterialInventoryEntity);

                await _whMaterialStandingbookRepository.InsertAsync(whMaterialStandingbookEntity);
                trans.Complete();
            }
        }

        /// <summary>
        /// 物料条码拆分
        /// </summary>
        /// <param name="adjustDto"></param>
        /// <returns></returns>
        public async Task<string> BarcodeSplitAdjustAsync(MaterialBarCodeSplitAdjustDto adjustDto)
        {
            await _validationBarCodeSplitAdjust.ValidateAndThrowAsync(adjustDto);

            //查询到库存的信息
            var oldWhMEntirty = await _whMaterialInventoryRepository.GetByBarCodeAsync(new WhMaterialInventoryBarCodeQuery
            {
                SiteId = _currentSite.SiteId ?? 0,
                BarCode = adjustDto.SFC
            });

            if (oldWhMEntirty.QuantityResidue < adjustDto.Qty) throw new CustomerValidationException(nameof(ErrorCode.MES15126));

            if (HasDecimalPart(adjustDto.Qty)) throw new CustomerValidationException(nameof(ErrorCode.MES15133));


            var remainsQty = oldWhMEntirty.QuantityResidue - adjustDto.Qty;

            //新条码编码
            var newSplitSFC = await GeneratewhSfcAdjustAsync(CodeRuleCodeTypeEnum.WhSfcSplitAdjust, oldWhMEntirty.SiteId, oldWhMEntirty.CreatedBy);

            if (newSplitSFC == null || string.IsNullOrEmpty(newSplitSFC))
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16200));
            }

            //查询到库存的信息
            var getNewSplitSFCEntity = await _whMaterialInventoryRepository.GetByBarCodeAsync(new WhMaterialInventoryBarCodeQuery
            {
                SiteId = _currentSite.SiteId ?? 0,
                BarCode = newSplitSFC
            });

            if (getNewSplitSFCEntity != null) throw new CustomerValidationException(nameof(ErrorCode.MES15130)).WithData("sfc", newSplitSFC);

            //SFC及SFC信息
            var manuSfcEntity = await _manuSfcRepository.GetSingleAsync(new ManuSfcQuery
            {
                SFC = oldWhMEntirty.MaterialBarCode,
                SiteId = _currentSite.SiteId ?? 0,
            });

            var manuSfcInfoEntity = new ManuSfcInfoEntity();
            var planWorkOrderEntity = new PlanWorkOrderEntity();
            var procMaterialEntitity = new ProcMaterialEntity();

            var updateStatusAndQtyByIdCommand = new UpdateStatusAndQtyByIdCommand();
            var manuSfc = new ManuSfcEntity();
            var manuSfcInfo = new ManuSfcInfoEntity();

            procMaterialEntitity = await _procMaterialRepository.GetByIdAsync(oldWhMEntirty.MaterialId, oldWhMEntirty.SiteId);

            if (manuSfcEntity != null)
            {
                manuSfcInfoEntity = await _manuSfcInfoRepository.GetBySFCIdWithIsUseAsync(manuSfcEntity.Id);
            }


            if (manuSfcInfoEntity != null && manuSfcInfoEntity.Id != 0)
            {
                planWorkOrderEntity = await _planWorkOrderRepository.GetByIdAsync(manuSfcInfoEntity.WorkOrderId ?? 0);



                //更新MANUSFC条码表-父条码
                updateStatusAndQtyByIdCommand = new UpdateStatusAndQtyByIdCommand
                {
                    Id = manuSfcEntity.Id,
                    Status = SfcStatusEnum.Invalid,
                    CurrentStatus = manuSfcEntity.Status,
                    Qty = remainsQty,
                    CurrentQty = manuSfcEntity.Qty,
                    UpdatedBy = _currentUser.UserName,
                    UpdatedOn = HymsonClock.Now()
                };

                //插入生产条码信息
                manuSfc = new ManuSfcEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SFC = newSplitSFC,
                    IsUsed = YesOrNoEnum.No,
                    Qty = adjustDto.Qty,
                    SiteId = _currentSite.SiteId ?? 0,
                    Status = manuSfcEntity.Status,
                    CreatedBy = _currentUser.UserName,
                    CreatedOn = HymsonClock.Now(),
                    UpdatedBy = _currentUser.UserName,
                    UpdatedOn = HymsonClock.Now()
                };

                manuSfcInfo = new ManuSfcInfoEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SiteId = manuSfcInfoEntity?.SiteId ?? 0,
                    SfcId = manuSfc.Id,
                    WorkOrderId = manuSfcInfoEntity?.WorkOrderId ?? 0,
                    ProductId = manuSfcInfoEntity?.ProductId ?? 0,
                    ProcessRouteId = planWorkOrderEntity?.ProcessRouteId ?? 0,
                    ProductBOMId = planWorkOrderEntity?.ProductBOMId ?? 0,
                    IsUsed = true,
                    CreatedBy = _currentUser.UserName,
                    UpdatedBy = _currentUser.UserName,
                };
            }

            //更新库存表
            var updateQuantityRangeCommand = new UpdateQuantityRangeCommand
            {
                Status = oldWhMEntirty.Status,
                BarCode = oldWhMEntirty.MaterialBarCode,
                QuantityResidue = adjustDto.Qty,
                UpdatedBy = _currentUser.UserName,
                UpdatedOn = HymsonClock.Now()
            };




            var subWhMEntirty = oldWhMEntirty.ToCopy();

            subWhMEntirty.Id = IdGenProvider.Instance.CreateId();
            subWhMEntirty.MaterialBarCode = newSplitSFC;
            subWhMEntirty.QuantityResidue = adjustDto.Qty;


            //物料台账+2
            var whMaterialStandingbookEntities = new List<WhMaterialStandingbookEntity>();
            //流转关系表
            var manuBarCodeRelationEntitys = new List<ManuBarCodeRelationEntity>();

            for (int i = 1; i <= 2; i++)
            {
                var standingbook = new WhMaterialStandingbookEntity
                {
                    MaterialCode = procMaterialEntitity?.MaterialCode ?? "",
                    MaterialName = procMaterialEntitity?.MaterialName ?? "",
                    MaterialVersion = procMaterialEntitity?.Version ?? "",
                    MaterialBarCode = i == 1 ? oldWhMEntirty.MaterialBarCode : newSplitSFC,
                    Quantity = i == 1 ? remainsQty : adjustDto.Qty,
                    Unit = procMaterialEntitity?.Unit ?? "",
                    Type = i == 1 ? WhMaterialInventoryTypeEnum.MaterialBarCodeSplit : WhMaterialInventoryTypeEnum.SplitAdd,
                    Source = MaterialInventorySourceEnum.Disassembly,
                    SiteId = _currentSite.SiteId ?? 0,
                    Id = IdGenProvider.Instance.CreateId(),
                    SupplierId = oldWhMEntirty.SupplierId,
                    Batch = oldWhMEntirty.Batch ?? string.Empty,
                    CreatedBy = _currentUser.UserName,
                    UpdatedBy = _currentUser.UserName,
                    CreatedOn = HymsonClock.Now(),
                    UpdatedOn = HymsonClock.Now(),
                    Remark = $"拆分前条码{oldWhMEntirty.MaterialBarCode},拆分后{newSplitSFC}"
                };

                whMaterialStandingbookEntities.Add(standingbook);


            }


            var manuBarCodeRelationEntity = new ManuBarCodeRelationEntity
            {
                Id = IdGenProvider.Instance.CreateId(),
                SiteId = oldWhMEntirty!.SiteId,
                ProcedureId = null,
                ResourceId = null,
                EquipmentId = null,
                InputBarCode = oldWhMEntirty.MaterialBarCode,
                InputBarCodeLocation = string.Empty,
                InputBarCodeMaterialId = oldWhMEntirty.MaterialId,
                InputBarCodeWorkOrderId = oldWhMEntirty.WorkOrderId,
                InputQty = adjustDto.Qty,
                OutputBarCode = newSplitSFC,
                OutputBarCodeMaterialId = oldWhMEntirty.MaterialId,
                OutputBarCodeWorkOrderId = oldWhMEntirty.WorkOrderId,
                OutputBarCodeMode = ManuBarCodeOutputModeEnum.Normal,
                RelationType = ManuBarCodeRelationTypeEnum.SFC_Split,
                BusinessContent = new
                {
                    InputMaterialStandingBookId = whMaterialStandingbookEntities.Where(x => x.MaterialBarCode == oldWhMEntirty.MaterialBarCode).FirstOrDefault()?.Id,
                    OutputMaterialStandingBookId = whMaterialStandingbookEntities.Where(x => x.MaterialBarCode == newSplitSFC).FirstOrDefault()?.Id
                }.ToSerialize(),
                IsDisassemble = TrueOrFalseEnum.No,
                DisassembledBy = _currentUser.UserName,
                DisassembledOn = HymsonClock.Now(),
                SubstituteId = 0,
                Remark = "物料条码拆分",
                CreatedOn = HymsonClock.Now(),
                CreatedBy = _currentUser.UserName,
                UpdatedBy = _currentUser.UserName,
                UpdatedOn = HymsonClock.Now(),
                IsDeleted = 0
            };
            manuBarCodeRelationEntitys.Add(manuBarCodeRelationEntity);

            //MANU BARCODE RELATION INSERT

            using (TransactionScope ts = TransactionHelper.GetTransactionScope())
            {
                //MANUSFC
                if (updateStatusAndQtyByIdCommand != null && updateStatusAndQtyByIdCommand.Id != 0)
                {
                    var row = await _manuSfcRepository.UpdateStatusAndQtyByIdAsync(updateStatusAndQtyByIdCommand);
                    if (row != 1)
                    {
                        throw new CustomerValidationException(nameof(ErrorCode.MES12832));
                    }
                }

                //INVENTORY UPDATE
                if (updateQuantityRangeCommand != null)
                {
                    await _whMaterialInventoryRepository.UpdateReduceQuantityResidueAsync(updateQuantityRangeCommand);
                }

                if (manuSfc != null && manuSfc.Id != 0)
                {
                    //MANUSFC AND MANUSFCINFO
                    await _manuSfcRepository.InsertAsync(manuSfc);
                }
                if (manuSfcInfo != null && manuSfcInfo.Id != 0)
                {
                    await _manuSfcInfoRepository.InsertAsync(manuSfcInfo);
                }

                //INVENTORY INSERT
                if (subWhMEntirty != null && subWhMEntirty.Id != 0)
                {
                    await _whMaterialInventoryRepository.InsertAsync(subWhMEntirty);
                }
                //台账
                if (whMaterialStandingbookEntities.Any())
                {
                    await _whMaterialStandingbookRepository.InsertsAsync(whMaterialStandingbookEntities);
                }
                //流程
                if (manuBarCodeRelationEntitys != null)
                {
                    //插入manu_barcode_relation
                    await _manuBarCodeRelationRepository.InsertRangeAsync(manuBarCodeRelationEntitys);
                }
                ts.Complete();
            }
            return newSplitSFC;

        }

        /// <summary>
        /// 物料条码合并
        /// </summary>
        /// <param name="adjustDto"></param>
        /// <returns></returns>
        public async Task<string> BarcodeMergeAdjustAsync(MaterialBarCodeMergeAdjust adjustDto)
        {
            if (adjustDto == null || adjustDto.SFCs == null || !adjustDto.SFCs.Any()) throw new CustomerValidationException(nameof(ErrorCode.MES12801));
            if (adjustDto.SFCs.Count() == 1) throw new CustomerValidationException(nameof(ErrorCode.MES12814));
            if (!string.IsNullOrEmpty(adjustDto.Remark) && adjustDto.Remark.Length > 255) throw new CustomerValidationException(nameof(ErrorCode.MES12827));

            //查询到库存的信息
            var oldWhMEntirty = await _whMaterialInventoryRepository.GetByBarCodesAsync(new WhMaterialInventoryBarCodesQuery
            {
                SiteId = _currentSite.SiteId ?? 0,
                BarCodes = adjustDto.SFCs
            });

            if (oldWhMEntirty.Any())
            {
                if (oldWhMEntirty.Count() != adjustDto.SFCs.Count())
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES15134));
                }

                oldWhMEntirty = oldWhMEntirty.Where(x => x.Status == WhMaterialInventoryStatusEnum.ToBeUsed);
            }

            if (oldWhMEntirty.Count() == 0)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES15129));
            }

            //处理父条码
            var qty = oldWhMEntirty.Sum(x => x.QuantityResidue);
            if (qty <= 0) throw new CustomerValidationException(nameof(ErrorCode.MES15128));

            string returnSFC = string.Empty;

            var updateSFCSpecifyCommand = new UpdateStatusAndQtyBySfcsCommand();
            var newSFCEntity = new WhMaterialInventoryEntity();
            var updateInventoryCommands = new List<UpdateQuantityRangeCommand>();
            //物料关系记录
            var manuBarCodeRelationEntitys = new List<ManuBarCodeRelationEntity>();
            //物料台账列表
            var standbookList = oldWhMEntirty.ToList();

            bool IsMergeSFC = false;

            if (adjustDto.MergeSFC != null || !string.IsNullOrWhiteSpace(adjustDto.MergeSFC))
            {
                //校验是否为合并列表中
                bool isExist = adjustDto.SFCs.Any(x => x.Contains(adjustDto.MergeSFC));
                if (!isExist)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES15135));
                }
                IsMergeSFC = true;
                returnSFC = adjustDto.MergeSFC;
            }

            //指定父条码
            if (IsMergeSFC)
            {

                var newEntity = oldWhMEntirty.Where(w => w.MaterialBarCode == adjustDto.MergeSFC).FirstOrDefault();

                foreach (var entity in oldWhMEntirty)
                {
                    decimal residueqty = 0;
                    var status = WhMaterialInventoryStatusEnum.Invalid;
                    if (entity.MaterialBarCode == adjustDto.MergeSFC)
                    {
                        residueqty = qty;
                        status = newEntity!.Status;
                    }
                    var updateQuantityRangeCommand = new UpdateQuantityRangeCommand
                    {
                        Status = status,
                        BarCode = entity.MaterialBarCode,
                        QuantityResidue = residueqty,
                        UpdatedBy = _currentUser.UserName,
                        UpdatedOn = HymsonClock.Now()
                    };

                    updateInventoryCommands.Add(updateQuantityRangeCommand);
                }
            }
            //新的
            else
            {
                var entityFirst = oldWhMEntirty.FirstOrDefault();
                newSFCEntity = entityFirst!.ToCopy();
                newSFCEntity.Id = IdGenProvider.Instance.CreateId();
                newSFCEntity.CreatedOn = HymsonClock.Now();
                newSFCEntity.CreatedBy = _currentUser.UserName;
                newSFCEntity.UpdatedOn = HymsonClock.Now();
                newSFCEntity.UpdatedBy = _currentUser.UserName;

                //新条码编码
                var newSplitSFC = await GeneratewhSfcAdjustAsync(CodeRuleCodeTypeEnum.WhSfcMergeAdjust, newSFCEntity.SiteId, newSFCEntity.CreatedBy);

                //查询到库存的信息
                var getNewSplitSFCEntity = await _whMaterialInventoryRepository.GetByBarCodeAsync(new WhMaterialInventoryBarCodeQuery
                {
                    SiteId = _currentSite.SiteId ?? 0,
                    BarCode = newSplitSFC
                });

                if (getNewSplitSFCEntity != null) throw new CustomerValidationException(nameof(ErrorCode.MES15130)).WithData("sfc", newSplitSFC);

                returnSFC = newSplitSFC;
                newSFCEntity.MaterialBarCode = newSplitSFC;
                newSFCEntity.QuantityResidue = qty;
                newSFCEntity.ReceivedQty = qty;

                foreach (var entity in oldWhMEntirty)
                {
                    var updateQuantityRangeCommand = new UpdateQuantityRangeCommand
                    {
                        Status = WhMaterialInventoryStatusEnum.Invalid,
                        BarCode = entity.MaterialBarCode,
                        QuantityResidue = 0,
                        UpdatedBy = _currentUser.UserName,
                        UpdatedOn = HymsonClock.Now()
                    };

                    updateInventoryCommands.Add(updateQuantityRangeCommand);
                }
            }


            //更新MANUSFC
            var updateSFCOtherCommand = new UpdateStatusAndQtyBySfcsCommand()
            {
                SiteId = _currentSite.SiteId,
                SFCs = oldWhMEntirty.Where(w => w.MaterialBarCode != adjustDto.MergeSFC).Select(s => s.MaterialBarCode),
                Status = SfcStatusEnum.Invalid,
                Qty = 0,
                UpdatedBy = _currentUser.UserName,
                UpdatedOn = HymsonClock.Now()
            };

            var inputBarcodeSingle = new WhMaterialInventoryEntity();
            var beforeBarcode = standbookList.Select(s => s.MaterialBarCode).Distinct();
            string afterBarcode = string.Empty;

            if (IsMergeSFC)
            {
                //SFC及SFC信息
                var manuSfcEntity = await _manuSfcRepository.GetSingleAsync(new ManuSfcQuery
                {
                    SFC = oldWhMEntirty.FirstOrDefault()!.MaterialBarCode,
                    SiteId = _currentSite.SiteId ?? 0,
                });
                if (manuSfcEntity != null)
                {
                    updateSFCSpecifyCommand = new UpdateStatusAndQtyBySfcsCommand()
                    {
                        SiteId = _currentSite.SiteId,
                        SFCs = oldWhMEntirty.Where(w => w.MaterialBarCode == adjustDto.MergeSFC).Select(s => s.MaterialBarCode),
                        Status = manuSfcEntity.Status,
                        Qty = qty,
                        UpdatedBy = _currentUser.UserName,
                        UpdatedOn = HymsonClock.Now()
                    };
                }


                inputBarcodeSingle = standbookList.Where(w => w.QuantityResidue != 0).FirstOrDefault();

            }
            else
            {
                inputBarcodeSingle = newSFCEntity;
                //台账
                standbookList.Add(newSFCEntity);
            }

            var procMaterialEntitity = await _procMaterialRepository.GetByIdAsync(oldWhMEntirty.FirstOrDefault()!.MaterialId);

            //物料台账
            var whMaterialStandingbookEntities = new List<WhMaterialStandingbookEntity>();



            foreach (var entity in standbookList)
            {
                decimal quantityResidue = 0;
                var type = WhMaterialInventoryTypeEnum.MaterialBarCodeMerge;
                //指定条码
                //处理备注内容及数量
                if (IsMergeSFC)
                {
                    if (entity.MaterialBarCode == adjustDto.MergeSFC)
                    {
                        quantityResidue = qty;
                    }
                    afterBarcode = adjustDto.MergeSFC ?? string.Empty;
                }
                else
                {
                    if (entity.MaterialBarCode == inputBarcodeSingle?.MaterialBarCode)
                    {
                        //新条码时处理
                        quantityResidue = entity.QuantityResidue;
                        type = WhMaterialInventoryTypeEnum.CombinedAdd;
                    }
                    beforeBarcode = beforeBarcode.Where(x => x != inputBarcodeSingle?.MaterialBarCode);
                    afterBarcode = inputBarcodeSingle?.MaterialBarCode ?? string.Empty;
                }



                var standingbook = new WhMaterialStandingbookEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    MaterialCode = procMaterialEntitity?.MaterialCode ?? "",
                    MaterialName = procMaterialEntitity?.MaterialName ?? "",
                    MaterialVersion = procMaterialEntitity?.Version ?? "",
                    Unit = procMaterialEntitity?.Unit ?? "",

                    MaterialBarCode = entity.MaterialBarCode,
                    Quantity = quantityResidue,

                    Type = type,
                    Source = MaterialInventorySourceEnum.Merge,
                    SiteId = _currentSite.SiteId ?? 0,
                    Batch = entity.Batch ?? string.Empty,
                    SupplierId = entity.SupplierId,
                    CreatedBy = _currentUser.UserName,
                    UpdatedBy = _currentUser.UserName,
                    CreatedOn = HymsonClock.Now(),
                    UpdatedOn = HymsonClock.Now(),
                    Remark = $"合并前条码:{string.Join(',', beforeBarcode)},合并后:{afterBarcode}"
                };

                whMaterialStandingbookEntities.Add(standingbook);
            }

            foreach (var entity in standbookList)
            {
                //if (entity.MaterialBarCode != inputBarcodeSingle?.MaterialBarCode)
                //{
                //    continue;
                //}            

                var manuBarCodeRelationEntity = new ManuBarCodeRelationEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SiteId = inputBarcodeSingle!.SiteId,
                    ProcedureId = null,
                    ResourceId = null,
                    EquipmentId = null,
                    InputBarCode = entity.MaterialBarCode,
                    InputBarCodeLocation = string.Empty,
                    InputBarCodeMaterialId = entity.MaterialId,
                    InputBarCodeWorkOrderId = entity.WorkOrderId,
                    InputQty = entity.QuantityResidue,
                    OutputBarCode = inputBarcodeSingle.MaterialBarCode,
                    OutputBarCodeMaterialId = inputBarcodeSingle.MaterialId,
                    OutputBarCodeWorkOrderId = inputBarcodeSingle.WorkOrderId,
                    OutputBarCodeMode = ManuBarCodeOutputModeEnum.Normal,
                    RelationType = ManuBarCodeRelationTypeEnum.SFC_Combined,
                    BusinessContent = new
                    {

                        InputMaterialStandingBookId = whMaterialStandingbookEntities.Where(x => x.MaterialBarCode == entity.MaterialBarCode).FirstOrDefault()?.Id,
                        OutputMaterialStandingBookId = whMaterialStandingbookEntities.Where(x => x.MaterialBarCode == inputBarcodeSingle.MaterialBarCode).FirstOrDefault()?.Id
                    }.ToSerialize(),
                    IsDisassemble = TrueOrFalseEnum.No,
                    DisassembledBy = _currentUser.UserName,
                    DisassembledOn = HymsonClock.Now(),
                    SubstituteId = 0,
                    Remark = "物料条码合并",
                    CreatedOn = HymsonClock.Now(),
                    CreatedBy = _currentUser.UserName,
                    UpdatedBy = _currentUser.UserName,
                    UpdatedOn = HymsonClock.Now(),
                    IsDeleted = 0
                };
                manuBarCodeRelationEntitys.Add(manuBarCodeRelationEntity);
            }

            using (TransactionScope ts = TransactionHelper.GetTransactionScope())
            {

                //更新其它SFC MANUSFC
                if (updateSFCOtherCommand != null)
                {
                    await _manuSfcRepository.UpdateStatusAndQtyBySfcsAsync(updateSFCOtherCommand);
                }

                //更新指定SFC
                if (updateSFCSpecifyCommand.SFCs != null)
                {
                    await _manuSfcRepository.UpdateStatusAndQtyBySfcsAsync(updateSFCSpecifyCommand);
                }

                //INVENTORY UPDATE
                if (updateInventoryCommands != null)
                {
                    await _whMaterialInventoryRepository.UpdateQuantityResidueRangeAsync(updateInventoryCommands);
                }

                //INVENTORY INSERT
                if (newSFCEntity != null && newSFCEntity.Id != 0)
                {
                    await _whMaterialInventoryRepository.InsertAsync(newSFCEntity);
                }
                //台账
                if (whMaterialStandingbookEntities.Any())
                {
                    await _whMaterialStandingbookRepository.InsertsAsync(whMaterialStandingbookEntities);
                }
                //流程
                if (manuBarCodeRelationEntitys != null)
                {
                    //插入manu_barcode_relation
                    await _manuBarCodeRelationRepository.InsertRangeAsync(manuBarCodeRelationEntitys);
                }
                ts.Complete();

            }

            return returnSFC;
        }

        /// <summary>
        /// 获取条码
        /// </summary>
        /// <param name="type"></param>
        /// <param name="siteId"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        /// <exception cref="CustomerValidationException"></exception>
        private async Task<string> GeneratewhSfcAdjustAsync(CodeRuleCodeTypeEnum type, long siteId, string userName)
        {
            var codeRules = await _inteCodeRulesRepository.GetListAsync(new InteCodeRulesReQuery
            {
                SiteId = siteId,
                CodeType = type
            });
            if (codeRules == null || !codeRules.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES15131)).WithData("type", type.GetDescription());
            }
            if (codeRules.Count() > 1)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES15132)).WithData("type", type.GetDescription());
            }

            var orderCodes = await _manuGenerateBarcodeService.GenerateBarcodeListByIdAsync(new CoreServices.Bos.Manufacture.ManuGenerateBarcode.GenerateBarcodeBo
            {
                SiteId = siteId,
                UserName = userName,
                CodeRuleId = codeRules.First().Id,
                Count = 1
            });

            return orderCodes.First();
        }

        /// <summary>
        /// 是否包含小数
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        private bool HasDecimalPart(decimal number)
        {
            return number != Math.Truncate(number);
        }

        /// <summary>
        /// 派工单领料
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task PickMaterialsRequestAsync(PickMaterialsRequest request)
        {
            //验证DTO
            await _validationPickMaterialsRequest.ValidateAndThrowAsync(request);
            //派工单校验
            var planWorkOrderEntity = await _planWorkOrderRepository.GetByCodeAsync(new PlanWorkOrderQuery
            {
                SiteId = _currentSite.SiteId ?? 0,
                OrderCode = request.WorkCode,
            });
            if (planWorkOrderEntity != null)
            {
                if (planWorkOrderEntity.Status == PlanWorkOrderStatusEnum.Finish)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES16048)).WithData("WorkOrder", request.WorkCode);
                }
            }
            else
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16016)).WithData("WorkOrder", request.WorkCode);
            }
            //查询生产计划
            var planWorkPlanEntity = await _planWorkPlanRepository.GetByIdAsync(planWorkOrderEntity.WorkPlanId ?? 0)
                ?? throw new CustomerValidationException(nameof(ErrorCode.MES16052)).WithData("WorkOrder", request.WorkCode);

            //创建领料申请单
            ManuRequistionOrderEntity manuRequistionOrderEntity = new ManuRequistionOrderEntity
            {
                Id = IdGenProvider.Instance.CreateId(),
                SiteId = _currentSite.SiteId ?? 0,
                //Status = WhWarehouseRequistionStatusEnum.Approvaling,
                Type = ManuRequistionTypeEnum.WorkOrderPicking,
                WorkOrderId = planWorkOrderEntity.Id,
                //WorkPlanCode = planWorkPlanEntity.WorkPlanCode,
                //WorkPlanId = planWorkPlanEntity.Id
            };


            //获取派工单指定BOM记录

            //var bomDetailEntities = await _procBomDetailRepository.GetByBomIdAsync(planWorkOrderEntity.ProductBOMId);

            var planWorkPlanMaterialEntities = await _planWorkPlanMaterialRepository.GetEntitiesByPlanIdAsync(new Data.Repositories.Plan.Query.PlanWorkPlanByPlanIdQuery
            {
                SiteId = _currentSite.SiteId ?? 0,
                PlanId = planWorkPlanEntity.Id,
                PlanProductId = planWorkOrderEntity.ProductId,
            });

            var materialEntities = await _procMaterialRepository.GetByIdsAsync(planWorkPlanMaterialEntities.Select(b => b.MaterialId).Distinct().ToArray());
            var productionPickMaterialDtos = new List<HttpClients.Requests.ProductionPickMaterialDto>();
            var validationFailures = new List<ValidationFailure>();
            foreach (var item in planWorkPlanMaterialEntities)
            {
                var materialEntity = materialEntities.FirstOrDefault(m => m.Id == item.MaterialId);
                if (materialEntity == null)
                {
                    var validationFailure = new ValidationFailure();
                    if (validationFailure.FormattedMessagePlaceholderValues == null || !validationFailure.FormattedMessagePlaceholderValues.Any())
                    {
                        validationFailure.FormattedMessagePlaceholderValues = new Dictionary<string, object> { { "CollectionIndex", item.MaterialId } };
                    }
                    else
                    {
                        validationFailure.FormattedMessagePlaceholderValues.Add("CollectionIndex", item.MaterialId);
                    }
                    validationFailure.FormattedMessagePlaceholderValues.Add("MaterialId", item.MaterialId);
                    validationFailure.ErrorCode = nameof(ErrorCode.MES10250);
                    validationFailures.Add(validationFailure);
                    continue;
                }

                HttpClients.Requests.ProductionPickMaterialDto productionPickMaterialDto = new HttpClients.Requests.ProductionPickMaterialDto
                {
                    MaterialCode = materialEntity.MaterialCode,
                    Quantity = (request.Qty * item.Usages * (1 + item.Loss)).ToString(),
                    UnitCode = materialEntity.Unit
                };
                productionPickMaterialDtos.Add(productionPickMaterialDto);
            }
            if (validationFailures.Any())
            {
                throw new ValidationException("", validationFailures);
            }
            //下达WMS领料申请
            var response = await _wmsRequest.MaterialPickingRequestAsync(new HttpClients.Requests.MaterialPickingRequestDto
            {
                SyncCode = $"{request.WorkCode}_{manuRequistionOrderEntity.Id}",
                SendOn = HymsonClock.Now().ToString(),
                details = productionPickMaterialDtos
            });
            if (response)
            {
                await _manuRequistionOrderRepository.InsertAsync(manuRequistionOrderEntity);
            }


        }

        public async Task<bool> PickMaterialsCancelAsync(PickMaterialsCancel request)
        {
            //var requistionOrderEntity = await _manuRequistionOrderRepository.GetByIdAsync(request.RequistionOrderId);
            //if (requistionOrderEntity.Status == WhWarehouseRequistionStatusEnum.Approvaling)
            //{
            //    var response = await _wmsRequest.MaterialPickingCancelAsync(new HttpClients.Requests.MaterialPickingCancelDto
            //    {
            //        SendOn = HymsonClock.Now().ToString(),
            //        SyncCode = $"{request.WorkCode}_{request.RequistionOrderId}",
            //    });
            //    return response;
            //}
            //else
            //{
            //    return false;
            //}
            return false;
        }

        public async Task MaterialReturnRequestAsync(MaterialReturnRequest request)
        {
            //获取派工单对象
            var planWorkOrderEntity = await _planWorkOrderRepository.GetByCodeAsync(new PlanWorkOrderQuery
            {
                SiteId = _currentSite.SiteId ?? 0,
                OrderCode = request.WorkCode
            }) ?? throw new CustomerValidationException(nameof(ErrorCode.MES16016)).WithData("WorkOrder", request.WorkCode);
            var whMaterialInventoryEntities = await _whMaterialInventoryRepository.GetByWorkOrderIdAsync(new WhMaterialInventoryWorkOrderIdQuery
            {
                SiteId = _currentSite.SiteId ?? 0,
                WorkOrderId = planWorkOrderEntity.Id
            });
            var materialEntities = await _procMaterialRepository.GetByIdsAsync(whMaterialInventoryEntities.Select(b => b.MaterialId).Distinct().ToArray());
            var returnMaterialDtos = new List<HttpClients.Requests.ProductionReturnMaterialItemDto>();

            foreach (var item in whMaterialInventoryEntities)
            {
                var materialEntity = materialEntities.FirstOrDefault(m => m.Id == item.MaterialId);


                HttpClients.Requests.ProductionReturnMaterialItemDto returnMaterialDto = new HttpClients.Requests.ProductionReturnMaterialItemDto
                {
                    MaterialCode = materialEntity.MaterialCode,
                    Quantity = item.QuantityResidue.ToString(),
                    UnitCode = materialEntity.Unit
                };
                returnMaterialDtos.Add(returnMaterialDto);
            }
            //创建领料申请单
            ManuReturnOrderEntity manuReturnOrderEntity = new ManuReturnOrderEntity
            {
                Id = IdGenProvider.Instance.CreateId(),
                SiteId = _currentSite.SiteId ?? 0,
                Status = WhWarehouseMaterialReturnStatusEnum.ApplicationSuccessful,
                Type = ManuReturnTypeEnum.WorkOrderReturn,
                //SourceWorkOrderCode = request.WorkCode,
            };
            var response = await _wmsRequest.MaterialReturnRequestAsync(new HttpClients.Requests.MaterialReturnRequestDto
            {

                SyncCode = $"{request.WorkCode}_{manuReturnOrderEntity.Id}",
                SendOn = HymsonClock.Now().ToString(),//TODO：这个信息需要调研
                Details = returnMaterialDtos
            });
            if (response)
                await _manuReturnOrderRepository.InsertAsync(manuReturnOrderEntity);
            else
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16051)).WithData("msg", "请求发送失败");
            }
        }

        /// <summary>
        /// 成品入库申请
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task ProductReceiptRequestAsync(ProductReceiptRequest request)
        {
            //获取派工单对象
            var planWorkOrderEntity = await _planWorkOrderRepository.GetByCodeAsync(new PlanWorkOrderQuery
            {
                SiteId = _currentSite.SiteId ?? 0,
                OrderCode = request.WorkCode
            }) ?? throw new CustomerValidationException(nameof(ErrorCode.MES16016)).WithData("WorkOrder", request.WorkCode);
            var whMaterialInventoryEntities = await _whMaterialInventoryRepository.GetByWorkOrderIdAsync(new WhMaterialInventoryWorkOrderIdQuery
            {
                SiteId = _currentSite.SiteId ?? 0,
                WorkOrderId = planWorkOrderEntity.Id
            });
            //工单ERP订单信息
            List<string> orderItem = request.WorkCode.Split('-').ToList();
            string erpOrder = orderItem[0] + "-" + orderItem[1];
            PlanWorkPlanQuery planQuery = new PlanWorkPlanQuery();
            planQuery.WorkPlanCode = erpOrder;
            var erpInfo = await _planWorkPlanRepository.GetProductAsync(planQuery);
            if (erpInfo == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES17756))
                    .WithData("orderCode", erpOrder);
            }

            if (request.Items.Any())
            {

                var Sfcs = request.Items.Select(m => m.Sfc);
                var manuProductReceipts = await _manuProductReceiptOrderDetailRepository.GetListAsync(new QueryManuProductReceiptOrderDetail
                {
                    SFCs = Sfcs,
                    SiteId = _currentSite.SiteId ?? 0,
                });
                var sfcList = manuProductReceipts.Select(x => x.Sfc).ToArray();
                if (manuProductReceipts.Any())
                {
                    // var sfcStrings = sfcList.Except(request.Items.Select(x => x.Sfc));
                    throw new CustomerValidationException(nameof(ErrorCode.MES17754));
                }

            }
            var materialEntities = await _procMaterialRepository.GetByIdsAsync(whMaterialInventoryEntities.Select(b => b.MaterialId).Distinct().ToArray());
            var returnMaterialDtos = new List<HttpClients.Requests.ProductReceiptItemDto>();
            var manuProductReceiptOrderDetails = new List<ManuProductReceiptOrderDetailEntity>();
            //创建入库申请单
            ManuProductReceiptOrderEntity manuProductReceiptOrderEntity = new ManuProductReceiptOrderEntity
            {
                Id = IdGenProvider.Instance.CreateId(),
                SiteId = _currentSite.SiteId ?? 0,
                Status = ProductReceiptStatusEnum.Approvaling,
                WorkOrderCode = request.OrderCodeId,
                WarehouseOrderCode = request.WorkCode,
                CreatedBy = _currentSystem.Name,
                UpdatedBy = _currentSystem.Name,
            };
            foreach (var item in request.Items)
            {
                var sequence = await _sequenceService.GetSerialNumberAsync(Sequences.Enums.SerialNumberTypeEnum.ByDay, "FAI");
                var CompletionOrderCode = $"{"WG"}{DateTime.UtcNow.ToString("yyyyMMdd")}{sequence.ToString().PadLeft(3, '0')}";
                // var materialEntity = materialEntities.FirstOrDefault(m => m.Id == item.MaterialId);

                HttpClients.Requests.ProductReceiptItemDto returnMaterialDto = new HttpClients.Requests.ProductReceiptItemDto
                {
                    ProductionOrderNumber = erpOrder,
                    ProductionOrderDetailID = erpInfo.ErpProductId,
                    BoxCode = item.BoxCode,
                    LotCode = item.Batch,
                    MaterialCode = request.MaterialCode,
                    Quantity = item.Qty.ToString(),
                    UnitCode = item.Unit,
                };
                ManuProductReceiptOrderDetailEntity manuProductReceiptOrderDetailEntity = new ManuProductReceiptOrderDetailEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    Batch = item.Batch,
                    WarehouseCode = item.WarehouseCode ?? "",
                    Sfc = item.Sfc ?? "",
                    ProductReceiptId = manuProductReceiptOrderEntity.Id,
                    MaterialCode = request.MaterialCode,
                    MaterialName = request.MaterialName,
                    Qty = item.Qty,
                    ContaineCode = item.BoxCode,
                    Status = item.Type ?? 0,
                    SiteId = _currentSite.SiteId ?? 0,
                    Unit = item.Unit,
                    CompletionOrderCode= CompletionOrderCode,
                    CreatedBy = _currentSystem.Name,
                    UpdatedBy = _currentSystem.Name,
                };
                manuProductReceiptOrderDetails.Add(manuProductReceiptOrderDetailEntity);
                returnMaterialDtos.Add(returnMaterialDto);
            }
            //using (var trans = TransactionHelper.GetTransactionScope())
            //{
            //    await _manuProductReceiptOrderRepository.InsertAsync(manuProductReceiptOrderEntity);
            //    await _manuProductReceiptOrderDetailRepository.InsertRangeAsync(manuProductReceiptOrderDetails);
            //    trans.Complete();
            //}0
            //校验仓库是否是一样的
            List<string> whCodeList = request.Items
                .Where(m => string.IsNullOrEmpty(m.WarehouseCode) == false)
                .Select(m => m.WarehouseCode!).Distinct().ToList();
            if(whCodeList.Count > 1)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES17755)).WithData("msg", string.Join(",", whCodeList));
            }
            //特殊处理。。。
            string whName = whCodeList[0];
            string whCode = string.Empty;
            if(whName == "成品仓") //不是不良品仓
            {
                whCode = _wmsOptions.Value.ProductReceipt.FinishWarehouseCode;
            }
            if (whName == "待检验仓") //待检验仓
            {
                whCode = _wmsOptions.Value.ProductReceipt.PendInspection;
            }
            else
            {
                whCode = _wmsOptions.Value.ProductReceipt.NgWarehouseCode;
            }

            var response = await _wmsRequest.ProductReceiptRequestAsync(new HttpClients.Requests.ProductReceiptRequestDto
            {
                WarehouseCode = whCode,
                SyncCode = $"{request.WorkCode}_{manuProductReceiptOrderEntity.Id}",
                SendOn = HymsonClock.Now().ToString(),//TODO：这个信息需要调研
                Details = returnMaterialDtos
            });
            if (response.Code != 1)
            {
                using (var trans = TransactionHelper.GetTransactionScope())
                {
                    await _manuProductReceiptOrderRepository.InsertAsync(manuProductReceiptOrderEntity);
                    await _manuProductReceiptOrderDetailRepository.InsertRangeAsync(manuProductReceiptOrderDetails);
                    trans.Complete();
                }
            }
            else
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16056)).WithData("msg", response.Message);
            }
        }

        /// <summary>
        /// 成品入库取消
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<bool> ProductReceiptCancelAsync(MaterialReturnCancel request)
        {
            var manuProductReceiptOrder = await _manuProductReceiptOrderRepository.GetByIdAsync(request.ReturnOrderId);
            if (manuProductReceiptOrder.Status == ProductReceiptStatusEnum.Approvaling)
            {
                var response = await _wmsRequest.ProductReceiptCancelAsync(new HttpClients.Requests.ProductReceiptCancelDto
                {
                    SendOn = HymsonClock.Now().ToString(),//TODO：这个信息需要调研
                    SyncCode = $"{request.WorkCode}_{request.ReturnOrderId}",
                });
                return response;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> MaterialReturnCancelAsync(MaterialReturnCancel request)
        {
            var returnOrderEntity = await _manuReturnOrderRepository.GetByIdAsync(request.ReturnOrderId);
            if (returnOrderEntity.Status == WhWarehouseMaterialReturnStatusEnum.ApplicationSuccessful)
            {
                var response = await _wmsRequest.MaterialReturnCancelAsync(new HttpClients.Requests.MaterialReturnCancelDto
                {
                    SendOn = HymsonClock.Now().ToString(),//TODO：这个信息需要调研
                    SyncCode = $"{request.WorkCode}_{request.ReturnOrderId}",
                });
                return response;
            }
            else
            {
                return false;
            }
        }

        public async Task PickMaterialsRequestAsync(PickMaterialsRequestV2 request)
        {
            //派工单校验

            var planWorkOrderEntity = await _planWorkOrderRepository.GetByIdAsync(request.OrderId);
            if (planWorkOrderEntity != null)
            {
                if (planWorkOrderEntity.Status == PlanWorkOrderStatusEnum.Finish)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES16048)).WithData("WorkOrder", planWorkOrderEntity.OrderCode);
                }
            }
            else
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16016)).WithData("WorkOrder", planWorkOrderEntity.OrderCode);
            }
            //查询生产计划
            var planWorkPlanEntity = await _planWorkPlanRepository.GetByIdAsync(planWorkOrderEntity.WorkPlanId ?? 0)
                ?? throw new CustomerValidationException(nameof(ErrorCode.MES16052)).WithData("WorkOrder", planWorkOrderEntity.OrderCode);
            //查询生产计划产品
            var planWorkPlanProductEntity = await _planWorkPlanProductRepository.GetByPlanIdAndProductIdAsync(planWorkPlanEntity.Id, planWorkOrderEntity.ProductId)
              ?? throw new CustomerValidationException(nameof(ErrorCode.MES16052)).WithData("WorkOrder", planWorkOrderEntity.OrderCode);
            //创建领料申请单
            ManuRequistionOrderEntity manuRequistionOrderEntity = new ManuRequistionOrderEntity
            {
                Id = IdGenProvider.Instance.CreateId(),
                SiteId = _currentSite.SiteId ?? 0,
                //Status = WhWarehouseRequistionStatusEnum.Approvaling,
                Type = ManuRequistionTypeEnum.WorkOrderPicking,
                WorkOrderId = planWorkOrderEntity.Id,
                //WorkPlanCode = planWorkPlanEntity.WorkPlanCode,
                //WorkPlanId = planWorkPlanEntity.Id
            };


            //获取派工单指定BOM记录

            //var bomDetailEntities = await _procBomDetailRepository.GetByBomIdAsync(planWorkOrderEntity.ProductBOMId);

            var planWorkPlanMaterialEntities = await _planWorkPlanMaterialRepository.GetEntitiesByPlanIdAsync(new Data.Repositories.Plan.Query.PlanWorkPlanByPlanIdQuery
            {
                SiteId = _currentSite.SiteId ?? 0,
                PlanId = planWorkPlanEntity.Id,
                PlanProductId = planWorkOrderEntity.ProductId,
            });

            var materialEntities = await _procMaterialRepository.GetByIdsAsync(planWorkPlanMaterialEntities.Select(b => b.MaterialId).Distinct().ToArray());
            var productionPickMaterialDtos = new List<HttpClients.Requests.ProductionPickMaterialDto>();
            var validationFailures = new List<ValidationFailure>();
            foreach (var item in request.Items)
            {
                var materialEntity = materialEntities.FirstOrDefault(m => m.Id == item.MaterialId);
                if (materialEntity == null)
                {
                    var validationFailure = new ValidationFailure();
                    if (validationFailure.FormattedMessagePlaceholderValues == null || !validationFailure.FormattedMessagePlaceholderValues.Any())
                    {
                        validationFailure.FormattedMessagePlaceholderValues = new Dictionary<string, object> { { "CollectionIndex", item.MaterialId } };
                    }
                    else
                    {
                        validationFailure.FormattedMessagePlaceholderValues.Add("CollectionIndex", item.MaterialId);
                    }
                    validationFailure.FormattedMessagePlaceholderValues.Add("MaterialId", item.MaterialId);
                    validationFailure.ErrorCode = nameof(ErrorCode.MES10250);
                    validationFailures.Add(validationFailure);
                    continue;
                }
                var planWorkPlanMaterialEntity = planWorkPlanMaterialEntities.FirstOrDefault(p => p.MaterialId == item.MaterialId);
                HttpClients.Requests.ProductionPickMaterialDto productionPickMaterialDto = new HttpClients.Requests.ProductionPickMaterialDto
                {
                    MaterialCode = materialEntity.MaterialCode,
                    Quantity = item.Usages.ToString(),
                    UnitCode = materialEntity.Unit,
                    ProductionOrder = planWorkPlanEntity.WorkPlanCode,
                    ProductionOrderComponentID = planWorkPlanMaterialEntity.Id,
                    ProductionOrderDetailID = planWorkPlanProductEntity.Id

                };
                productionPickMaterialDtos.Add(productionPickMaterialDto);
            }
            if (validationFailures.Any())
            {
                throw new ValidationException("", validationFailures);
            }
            //下达WMS领料申请
            var response = await _wmsRequest.MaterialPickingRequestAsync(new HttpClients.Requests.MaterialPickingRequestDto
            {
                SyncCode = $"{planWorkOrderEntity.OrderCode}_{manuRequistionOrderEntity.Id}",
                SendOn = HymsonClock.Now().ToString(),
                details = productionPickMaterialDtos
            });
            if (response)
            {
                await _manuRequistionOrderRepository.InsertAsync(manuRequistionOrderEntity);
            }
        }

        /// <summary>
        /// 根据工单查询工单的领料列表
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<WhMaterialInventoryDetailDto>> GetPickMaterialsByOrderidAsync(long orderId)
        {
            var whMaterialInventories = new List<WhMaterialInventoryDetailDto>();
            var inventoryEntities = await _whMaterialInventoryRepository.GetWhMaterialInventoryEntitiesAsync(new WhMaterialInventoryQuery
            {
                SiteId = _currentSite.SiteId ?? 0,
                WorkOrderId = orderId,
                Status = WhMaterialInventoryStatusEnum.ToBeUsed
            });

            inventoryEntities = inventoryEntities.Where(x => x.QuantityResidue > 0);
            if (inventoryEntities == null || !inventoryEntities.Any())
            {
                return whMaterialInventories;
            }

            var materialIds = inventoryEntities.Select(x => x.MaterialId).Distinct().ToArray();
            var procMaterialEntities = await _procMaterialRepository.GetByIdsAsync(materialIds);

            foreach (var whMaterial in inventoryEntities)
            {
                var material = procMaterialEntities.FirstOrDefault(x => x.Id == whMaterial.MaterialId);

                var whMaterialInventoryDto = whMaterial.ToModel<WhMaterialInventoryDetailDto>();
                whMaterialInventoryDto.MaterialCode = material?.MaterialCode ?? "";
                whMaterialInventoryDto.MaterialName = material?.MaterialName ?? "";
                whMaterialInventoryDto.Specifications = material?.Specifications ?? "";
                whMaterialInventories.Add(whMaterialInventoryDto);
            }
            return whMaterialInventories;
        }
    }
}
