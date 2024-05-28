using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.Localization.Services;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Domain.Warehouse;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Integrated;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.CoreServices.Services.Manufacture.WhMaterialInventory;
using Hymson.MES.Data.Repositories.Integrated;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.Warehouse;
using Hymson.MES.Data.Repositories.Warehouse.WhMaterialInventory.Command;
using Hymson.MES.Data.Repositories.Warehouse.WhMaterialInventory.Query;
using Hymson.MES.Services.Dtos.Warehouse;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;
using Hymson.MES.CoreServices.Services.Manufacture.ManuGenerateBarcode;
using Hymson.MES.Data.Repositories.Plan;
using System.Transactions;
using Hymson.MES.CoreServices.Bos.Common;
using System.Net.NetworkInformation;
using Hymson.MES.Core.Domain.Process;
using System.Security.Policy;
using Hymson.MES.Services.Dtos.Manufacture;
using Hymson.MES.Core.Domain.Plan;
using AutoMapper.Execution;

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
        /// <param name="whMaterialInventoryCoreService"></param>
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
            IWhMaterialInventoryCoreService whMaterialInventoryCoreService,
            IInteCodeRulesRepository inteCodeRulesRepository,
            IInteCodeRulesMakeRepository inteCodeRulesMakeRepository,
            IManuGenerateBarcodeService manuGenerateBarcodeService,
            IPlanWorkOrderRepository planWorkOrderRepository,
            IManuBarCodeRelationRepository manuBarCodeRelationRepository)
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
            whMaterialInventoryPagedQuery.Sources = new MaterialInventorySourceEnum[] { MaterialInventorySourceEnum.ManualEntry, MaterialInventorySourceEnum.WMS, MaterialInventorySourceEnum.LoadingPoint };//只查询外部的
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

            if (new MaterialInventorySourceEnum[] { MaterialInventorySourceEnum.ManualEntry, MaterialInventorySourceEnum.WMS, MaterialInventorySourceEnum.LoadingPoint }.Contains(entity.Source))
            {
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
            }
            else
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES15120));
            }
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

            if (!new MaterialInventorySourceEnum[] { MaterialInventorySourceEnum.ManualEntry, MaterialInventorySourceEnum.WMS, MaterialInventorySourceEnum.LoadingPoint }.Contains(oldWhMIEntirty.Source))
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES15123));
            }

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
            for (int i = 1; i <= 2; i++)
            {
                //var orgisfc = i == 1 ? oldWhMEntirty.MaterialBarCode : newSplitSFC;
                //var newsfc = i == 1 ? newSplitSFC : oldWhMEntirty.MaterialBarCode;
                var standingbook = new WhMaterialStandingbookEntity
                {
                    MaterialCode = procMaterialEntitity?.MaterialCode ?? "",
                    MaterialName = procMaterialEntitity?.MaterialName ?? "",
                    MaterialVersion = procMaterialEntitity?.Version ?? "",
                    MaterialBarCode = i == 1 ? oldWhMEntirty.MaterialBarCode : newSplitSFC,
                    Quantity = i == 1 ? remainsQty : adjustDto.Qty,
                    Unit = procMaterialEntitity?.Unit ?? "",
                    Type = WhMaterialInventoryTypeEnum.MaterialBarCodeSplit,
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

            oldWhMEntirty = oldWhMEntirty.Where(x => x.Status == WhMaterialInventoryStatusEnum.ToBeUsed);

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
                IsMergeSFC = true;
            }

            //指定父条码
            if (IsMergeSFC)
            {
                returnSFC = adjustDto.MergeSFC;
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

                if(getNewSplitSFCEntity != null) throw new CustomerValidationException(nameof(ErrorCode.MES15130)).WithData("sfc", newSplitSFC);

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
                var standingbook = new WhMaterialStandingbookEntity
                {
                    MaterialCode = procMaterialEntitity?.MaterialCode ?? "",
                    MaterialName = procMaterialEntitity?.MaterialName ?? "",
                    MaterialVersion = procMaterialEntitity?.Version ?? "",
                    Unit = procMaterialEntitity?.Unit ?? "",

                    MaterialBarCode = entity.MaterialBarCode,
                    Quantity = entity.QuantityResidue,

                    Type = WhMaterialInventoryTypeEnum.MaterialBarCodeMerge,
                    Source = MaterialInventorySourceEnum.Merge,
                    SiteId = _currentSite.SiteId ?? 0,
                    Id = IdGenProvider.Instance.CreateId(),
                    Batch = entity.Batch ?? string.Empty,
                    SupplierId = entity.SupplierId,
                    CreatedBy = _currentUser.UserName,
                    UpdatedBy = _currentUser.UserName,
                    CreatedOn = HymsonClock.Now(),
                    UpdatedOn = HymsonClock.Now(),
                    Remark = $"合并前条码:{entity.MaterialBarCode},合并后:{inputBarcodeSingle.MaterialBarCode}"
                };

                whMaterialStandingbookEntities.Add(standingbook);

                var manuBarCodeRelationEntity = new ManuBarCodeRelationEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SiteId = inputBarcodeSingle!.SiteId,
                    ProcedureId = null,
                    ResourceId = null,
                    EquipmentId = null,
                    InputBarCode = inputBarcodeSingle.MaterialBarCode,
                    InputBarCodeLocation = string.Empty,
                    InputBarCodeMaterialId = inputBarcodeSingle.MaterialId,
                    InputBarCodeWorkOrderId = inputBarcodeSingle.WorkOrderId,
                    InputQty = inputBarcodeSingle.QuantityResidue,
                    OutputBarCode = entity.MaterialBarCode,
                    OutputBarCodeMaterialId = inputBarcodeSingle.MaterialId,
                    OutputBarCodeWorkOrderId = inputBarcodeSingle.WorkOrderId,
                    OutputBarCodeMode = ManuBarCodeOutputModeEnum.Normal,
                    RelationType = ManuBarCodeRelationTypeEnum.SFC_Combined,
                    BusinessContent = "{}",
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
                try
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
                        //if (row == 0)
                        //{
                        //    throw new CustomerValidationException(nameof(ErrorCode.MES12832));
                        //}
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
                catch (Exception ex)
                {

                }

            }

            return returnSFC;
        }

        /// <summary>
        /// 拆分合并条码的验证
        /// </summary>
        /// <param name="sfcs"></param>
        /// <returns></returns>
        public async Task<bool> MergeAdjustVerifySfcAsync(string[] sfcs)
        {
            return true;
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
                throw new CustomerValidationException(nameof(ErrorCode.MES15132)).WithData("type",type.GetDescription());
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

    }
}
