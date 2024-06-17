using Elastic.Clients.Elasticsearch;
using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Integrated;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Domain.Plan;
using Hymson.MES.Core.Domain.Warehouse;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Integrated;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.Core.Enums.Warehouse;
using Hymson.MES.CoreServices.Bos.Manufacture;
using Hymson.MES.CoreServices.Services.Common;
using Hymson.MES.CoreServices.Services.Manufacture.ManuGenerateBarcode;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Integrated;
using Hymson.MES.Data.Repositories.Integrated.InteCodeRule.Query;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Manufacture.Query;
using Hymson.MES.Data.Repositories.Plan;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.Warehouse;
using Hymson.MES.Services.Dtos.Manufacture;
using Hymson.MES.Services.Dtos.Report;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;
using Minio.DataModel;
using System.Security.Policy;
using System.Transactions;

namespace Hymson.MES.Services.Services.Manufacture.ManuJointProductAndByproductsReceiveRecord
{
    /// <summary>
    /// 服务（联副产品收货） 
    /// </summary>
    public class ManuJointProductAndByproductsReceiveRecordService : IManuJointProductAndByproductsReceiveRecordService
    {
        /// <summary>
        /// 当前用户
        /// </summary>
        private readonly ICurrentUser _currentUser;
        /// <summary>
        /// 当前站点
        /// </summary>
        private readonly ICurrentSite _currentSite;

        /// <summary>
        /// 参数验证器
        /// </summary>
        private readonly AbstractValidator<ManuJointProductAndByproductsReceiveRecordSaveDto> _validationSaveRules;

        /// <summary>
        /// 仓储接口（联副产品收货）
        /// </summary>
        private readonly IManuJointProductAndByproductsReceiveRecordRepository _manuJointProductAndByproductsReceiveRecordRepository;

        /// <summary>
        /// 工单表 仓储
        /// </summary>
        private readonly IPlanWorkOrderRepository _planWorkOrderRepository;

        /// <summary>
        /// 物料维护 仓储
        /// </summary>
        private readonly IProcMaterialRepository _procMaterialRepository;

        /// <summary>
        /// BOM详情
        /// </summary>
        private readonly IProcBomDetailRepository _procBomDetailRepository;

        private readonly IInteCodeRulesRepository _inteCodeRulesRepository;

        private readonly IInteCodeRulesMakeRepository _inteCodeRulesMakeRepository;

        private readonly IManuGenerateBarcodeService _manuGenerateBarcodeService;

        private readonly IMasterDataService _masterDataService;
       

        /// <summary>
        /// 仓储接口（物料库存）
        /// </summary>
        private readonly IWhMaterialInventoryRepository _whMaterialInventoryRepository;

        /// <summary>
        /// 仓储接口（物料台账）
        /// </summary>
        private readonly IWhMaterialStandingbookRepository _whMaterialStandingbookRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="currentSite"></param>
        /// <param name="validationSaveRules"></param>
        /// <param name="manuJointProductAndByproductsReceiveRecordRepository"></param>
        /// <param name="planWorkOrderRepository"></param>
        /// <param name="procMaterialRepository"></param>
        /// <param name="procBomDetailRepository"></param>
        /// <param name="inteCodeRulesRepository"></param>
        /// <param name="inteCodeRulesMakeRepository"></param>
        /// <param name="manuGenerateBarcodeService"></param>
        /// <param name="masterDataService"></param>
        /// <param name="whMaterialInventoryRepository"></param>
        /// <param name="whMaterialStandingbookRepository"></param>
        public ManuJointProductAndByproductsReceiveRecordService(ICurrentUser currentUser, ICurrentSite currentSite,
            AbstractValidator<ManuJointProductAndByproductsReceiveRecordSaveDto> validationSaveRules,
            IManuJointProductAndByproductsReceiveRecordRepository manuJointProductAndByproductsReceiveRecordRepository,
            IPlanWorkOrderRepository planWorkOrderRepository, IProcMaterialRepository procMaterialRepository,
            IProcBomDetailRepository procBomDetailRepository, IInteCodeRulesRepository inteCodeRulesRepository,
            IInteCodeRulesMakeRepository inteCodeRulesMakeRepository, IManuGenerateBarcodeService manuGenerateBarcodeService,
            IMasterDataService masterDataService,
            IWhMaterialInventoryRepository whMaterialInventoryRepository, 
            IWhMaterialStandingbookRepository whMaterialStandingbookRepository)
        {
            _inteCodeRulesRepository = inteCodeRulesRepository;
            _procBomDetailRepository = procBomDetailRepository;
            _planWorkOrderRepository = planWorkOrderRepository;
            _currentUser = currentUser;
            _currentSite = currentSite;
            _validationSaveRules = validationSaveRules;
            _manuJointProductAndByproductsReceiveRecordRepository = manuJointProductAndByproductsReceiveRecordRepository;
            _procMaterialRepository = procMaterialRepository;
            _inteCodeRulesMakeRepository = inteCodeRulesMakeRepository;
            _manuGenerateBarcodeService = manuGenerateBarcodeService;
            _masterDataService = masterDataService;
            _whMaterialInventoryRepository = whMaterialInventoryRepository;
            _whMaterialStandingbookRepository = whMaterialStandingbookRepository;
        }


        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        public async Task<int> CreateAsync(ManuJointProductAndByproductsReceiveRecordSaveDto saveDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0) throw new CustomerValidationException(nameof(ErrorCode.MES10101));

            // 验证DTO
            await _validationSaveRules.ValidateAndThrowAsync(saveDto);

            // 更新时间
            var updatedBy = _currentUser.UserName;
            var updatedOn = HymsonClock.Now();

            // DTO转换实体
            var entity = saveDto.ToEntity<ManuJointProductAndByproductsReceiveRecordEntity>();
            entity.Id = IdGenProvider.Instance.CreateId();
            entity.CreatedBy = updatedBy;
            entity.CreatedOn = updatedOn;
            entity.UpdatedBy = updatedBy;
            entity.UpdatedOn = updatedOn;
            entity.SiteId = _currentSite.SiteId ?? 0;

            // 保存
            return await _manuJointProductAndByproductsReceiveRecordRepository.InsertAsync(entity);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        public async Task<int> ModifyAsync(ManuJointProductAndByproductsReceiveRecordSaveDto saveDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0) throw new CustomerValidationException(nameof(ErrorCode.MES10101));

            // 验证DTO
            await _validationSaveRules.ValidateAndThrowAsync(saveDto);

            // DTO转换实体
            var entity = saveDto.ToEntity<ManuJointProductAndByproductsReceiveRecordEntity>();
            entity.UpdatedBy = _currentUser.UserName;
            entity.UpdatedOn = HymsonClock.Now();

            return await _manuJointProductAndByproductsReceiveRecordRepository.UpdateAsync(entity);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(long id)
        {
            return await _manuJointProductAndByproductsReceiveRecordRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeletesAsync(long[] ids)
        {
            return await _manuJointProductAndByproductsReceiveRecordRepository.DeletesAsync(new DeleteCommand
            {
                Ids = ids,
                DeleteOn = HymsonClock.Now(),
                UserId = _currentUser.UserName
            });
        }

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ManuJointProductAndByproductsReceiveRecordDto?> QueryByIdAsync(long id)
        {
            var manuJointProductAndByproductsReceiveRecordEntity = await _manuJointProductAndByproductsReceiveRecordRepository.GetByIdAsync(id);
            if (manuJointProductAndByproductsReceiveRecordEntity == null) return null;

            return manuJointProductAndByproductsReceiveRecordEntity.ToModel<ManuJointProductAndByproductsReceiveRecordDto>();
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ManuJointProductAndByproductsReceiveRecordDto>> GetPagedListAsync(ManuJointProductAndByproductsReceiveRecordPagedQueryDto pagedQueryDto)
        {
            var pagedQuery = pagedQueryDto.ToQuery<ManuJointProductAndByproductsReceiveRecordPagedQuery>();
            pagedQuery.SiteId = _currentSite.SiteId ?? 0;
            var pagedInfo = await _manuJointProductAndByproductsReceiveRecordRepository.GetPagedListAsync(pagedQuery);

            // 实体到DTO转换 装载数据
            var dtos = pagedInfo.Data.Select(s => s.ToModel<ManuJointProductAndByproductsReceiveRecordDto>());
            return new PagedInfo<ManuJointProductAndByproductsReceiveRecordDto>(dtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 根据工单Id查询Bom联副产品列表
        /// </summary>
        /// <param name="workOrderId"></param>
        /// <returns></returns>
        public async Task<ManuJointProductAndByproductsReceiveRecordResult> GetWorkIdByBomJointProductAndByProductsListAsync(long workOrderId)
        {
            ManuJointProductAndByproductsReceiveRecordResult manuJointProductAndByproductsReceiveRecordResult = new ManuJointProductAndByproductsReceiveRecordResult();
            var planWorkOrder = await _planWorkOrderRepository.GetByIdAsync(workOrderId);
            if (planWorkOrder == null) return manuJointProductAndByproductsReceiveRecordResult;

            //产品中的主物料信息
            var procMaterialEntity = await _procMaterialRepository.GetByIdAsync(planWorkOrder.ProductId);

            //查询Bom中得物料信息
            var procBomDetails = await _procBomDetailRepository.GetByBomIdAsync(planWorkOrder.ProductBOMId);
            if (procBomDetails == null || !procBomDetails.Any()) return manuJointProductAndByproductsReceiveRecordResult;

            //查询组件物料信息
            var procMaterialIds = procBomDetails.Select(s => s.MaterialId).ToArray();
            var procMaterials = await _procMaterialRepository.GetByIdsAsync(procMaterialIds);

            //查询已收货数量
            var jointProductAndByproductsReceiveRecords = await _manuJointProductAndByproductsReceiveRecordRepository.GetEntitiesAsync(new ManuJointProductAndByproductsReceiveRecordQuery
            {
                SiteId = _currentSite.SiteId ?? 0,
                WorkOrderid = workOrderId,
                procMaterialIds = procMaterialIds,
            });

            //查询联产品信息
            var JointProductList = procBomDetails.Where(x => x.BomProductType == ManuProductTypeEnum.JointProducts).ToList();
            List<JointProductResult> jointProductList = new List<JointProductResult>();
            foreach (var jointProduct in JointProductList)
            {
                var receivedQty = jointProductAndByproductsReceiveRecords.Where(x => x.ProductId == jointProduct.MaterialId && x.WorkOrderid == workOrderId).Sum(x => x.Qty);
                var procMaterial = procMaterials.Where(x => x.Id == jointProduct.MaterialId).FirstOrDefault();

                jointProductList.Add(new JointProductResult()
                {
                    ProductId = procMaterial != null ? procMaterial.Id:0,
                    ProductCodeVersion = procMaterial != null ? procMaterial.MaterialCode + "/" + procMaterial.Version : "",
                    ProductName = procMaterial != null ? procMaterial.MaterialName : "",
                    ReceivedQty = receivedQty,
                });
            }

            //查询副产品信息
            var ByProductList = procBomDetails.Where(x => x.BomProductType == ManuProductTypeEnum.ByProduct).ToList();
            List<ByproductsResult> byProductList = new List<ByproductsResult>();
            foreach (var byProduct in ByProductList)
            {
                var procMaterial = procMaterials.Where(x => x.Id == byProduct.MaterialId).FirstOrDefault();
                var receivedQty = jointProductAndByproductsReceiveRecords.Where(x => x.ProductId == byProduct.MaterialId && x.WorkOrderid == workOrderId).Sum(x => x.Qty);

                byProductList.Add(new ByproductsResult()
                {
                    ProductId = procMaterial != null ? procMaterial.Id : 0,
                    ProductCodeVersion = procMaterial != null ? procMaterial.MaterialCode + "/" + procMaterial.Version : "",
                    ProductName = procMaterial != null ? procMaterial.MaterialName : "",
                    ReceivedQty = receivedQty,
                });
            }

            //返回前台信息
            manuJointProductAndByproductsReceiveRecordResult.ProductCodeVersion = procMaterialEntity.MaterialCode + "/" + procMaterialEntity.Version;
            manuJointProductAndByproductsReceiveRecordResult.ProductName = procMaterialEntity.MaterialName;
            manuJointProductAndByproductsReceiveRecordResult.JointProductList = jointProductList;
            manuJointProductAndByproductsReceiveRecordResult.ByproductsList = byProductList;
            return manuJointProductAndByproductsReceiveRecordResult;
        }

        /// <summary>
        /// 保存联副产品收货信息
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        public async Task<ManuJointProductAndByproductsReceiveRecordSaveResultDto> SaveJointProductAndByproductsInfoAsync(ManuJointProductAndByproductsReceiveRecordSaveDto saveDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0) throw new CustomerValidationException(nameof(ErrorCode.MES10101));

            // 验证DTO
            await _validationSaveRules.ValidateAndThrowAsync(saveDto);

            var planWorkOrderEntity = await _masterDataService.GetProduceWorkOrderByIdAsync(new WorkOrderIdBo
            {
                WorkOrderId = saveDto.WorkOrderid,
                IsVerifyActivation = false
            });
            // 产品ID
            var productId = saveDto.ProductId;
            var procMaterialEntity = await _procMaterialRepository.GetByIdAsync(productId);
            var inteCodeRulesEntity = await _inteCodeRulesRepository.GetInteCodeRulesByProductIdAsync(new InteCodeRulesByProductQuery
            {
                ProductId = saveDto.ProductId,
                CodeType = CodeRuleCodeTypeEnum.WorkshopInventory// CodeRuleCodeTypeEnum.ProcessControlSeqCode
            }) ?? throw new CustomerValidationException(nameof(ErrorCode.MES16501)).WithData("product", procMaterialEntity.MaterialCode);
            var BatchQty = string.IsNullOrEmpty(procMaterialEntity.Batch) ? 0 : decimal.Parse(procMaterialEntity.Batch);
            if (BatchQty == 0)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16502)).WithData("product", procMaterialEntity.MaterialCode);
            }
            var qty = string.IsNullOrEmpty(procMaterialEntity.Batch) ? 0 : decimal.Parse(procMaterialEntity.Batch);
            // 读取基础数据
            var codeRulesMakeList = await _inteCodeRulesMakeRepository.GetInteCodeRulesMakeEntitiesAsync(new InteCodeRulesMakeQuery
            {
                SiteId = _currentSite.SiteId ?? 0,
                CodeRulesId = inteCodeRulesEntity.Id
            });
            if (codeRulesMakeList == null || !codeRulesMakeList.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16501)).WithData("product", procMaterialEntity.MaterialCode);
            }

            var barcodeList = await _manuGenerateBarcodeService.GenerateBarCodeSerialNumberReturnBarCodeInfosAsync(new BarCodeSerialNumberBo
            {
                IsTest = false,
                IsSimulation = false,
                CodeRulesMakeBos = codeRulesMakeList.Select(s => new CodeRulesMakeBo
                {
                    Seq = s.Seq,
                    ValueTakingType = s.ValueTakingType,
                    SegmentedValue = s.SegmentedValue,
                    CustomValue = s.CustomValue,
                }),
                ProductId = procMaterialEntity.Id,
                CodeRuleKey = $"{inteCodeRulesEntity.Id}",
                Count = 1,
                Base = inteCodeRulesEntity.Base,
                Increment = inteCodeRulesEntity.Increment,
                IgnoreChar = inteCodeRulesEntity.IgnoreChar ?? "",
                OrderLength = inteCodeRulesEntity.OrderLength,
                ResetType = inteCodeRulesEntity.ResetType,
                StartNumber = inteCodeRulesEntity.StartNumber,
                CodeMode = inteCodeRulesEntity.CodeMode,
                SiteId = _currentSite.SiteId ?? 0,
            });

            List<ManuJointProductAndByproductsReceiveRecordEntity> manuJointProductAndByproductsList = new List<ManuJointProductAndByproductsReceiveRecordEntity>();
            List<WhMaterialStandingbookEntity> materialStandingbookList = new List<WhMaterialStandingbookEntity>();
            List<WhMaterialInventoryEntity> whMaterialInventorieList = new List<WhMaterialInventoryEntity>();
            var sfcs = new List<string>();
            foreach (var item in barcodeList)
            {
                foreach (var sfc in item.BarCodes)
                {
                    sfcs.Add(sfc);
                    //联副产品信息
                    manuJointProductAndByproductsList.Add(new ManuJointProductAndByproductsReceiveRecordEntity
                    {
                        Id = IdGenProvider.Instance.CreateId(),
                        SiteId = _currentSite.SiteId ?? 0,
                        WorkOrderid = planWorkOrderEntity.Id,
                        ProductId = productId,
                        BarCode = sfc,
                        Qty = saveDto.Qty,
                        WarehouseId = saveDto.WarehouseId,
                        CreatedBy = _currentUser.UserName,
                        UpdatedBy = _currentUser.UserName
                    });

                    // 新增 wh_material_inventory
                    whMaterialInventorieList.Add(new WhMaterialInventoryEntity
                    {
                        Id = IdGenProvider.Instance.CreateId(),
                        SupplierId = 0,//自制品 没有
                        MaterialId = productId,
                        MaterialBarCode = sfc,
                        Batch = "",//自制品 没有
                        MaterialType = MaterialInventoryMaterialTypeEnum.SelfMadeParts,
                        QuantityResidue = saveDto.Qty,
                        Status = WhMaterialInventoryStatusEnum.ToBeUsed,
                        Source = MaterialInventorySourceEnum.ManuComplete,
                        SiteId = _currentSite.SiteId ?? 0,
                        CreatedBy = _currentUser.UserName,
                        UpdatedBy = _currentUser.UserName
                    });

                    // 新增 wh_material_standingbook
                    materialStandingbookList.Add(new WhMaterialStandingbookEntity
                    {
                        Id = IdGenProvider.Instance.CreateId(),
                        MaterialCode = procMaterialEntity.MaterialCode ?? "",
                        MaterialName = procMaterialEntity.MaterialName,
                        MaterialVersion = procMaterialEntity.Version??"",
                        MaterialBarCode = sfc,
                        Batch = procMaterialEntity.Batch,
                        Quantity = saveDto.Qty,
                        Unit = procMaterialEntity.Unit ?? "",
                        Type = WhMaterialInventoryTypeEnum.MaterialScrapping,
                        Source = MaterialInventorySourceEnum.ManualEntry,
                        SiteId = _currentSite.SiteId ?? 0,
                        CreatedBy = _currentUser.UserName,
                        UpdatedBy = _currentUser.UserName
                    });
                }
            }
            // 开启事务
            using var trans = TransactionHelper.GetTransactionScope(TransactionScopeOption.Required, IsolationLevel.ReadCommitted);

            await _whMaterialInventoryRepository.InsertsAsync(whMaterialInventorieList);
            await _whMaterialStandingbookRepository.InsertsAsync(materialStandingbookList);
            await _manuJointProductAndByproductsReceiveRecordRepository.InsertRangeAsync(manuJointProductAndByproductsList);
           
            trans.Complete();

            ManuJointProductAndByproductsReceiveRecordSaveResultDto manuJointProductAndByproducts = new ManuJointProductAndByproductsReceiveRecordSaveResultDto();
            manuJointProductAndByproducts.ProductCodeVersion = procMaterialEntity.MaterialCode + "/" + procMaterialEntity.Version;
            manuJointProductAndByproducts.SFC = string.Join(",", sfcs);
            return manuJointProductAndByproducts;
        }
    }
}