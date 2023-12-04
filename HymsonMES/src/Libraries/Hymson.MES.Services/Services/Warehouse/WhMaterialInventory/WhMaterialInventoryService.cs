/*
 *creator: Karl
 *
 *describe: 物料库存    服务 | 代码由框架生成
 *builder:  pengxin
 *build datetime: 2023-03-06 03:27:59
 */
using FluentValidation;
using FluentValidation.Results;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.Localization.Services;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Warehouse;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Warehouse;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Manufacture.ManuSfc.Query;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.Warehouse;
using Hymson.MES.Data.Repositories.Warehouse.WhMaterialInventory.Command;
using Hymson.MES.Data.Repositories.Warehouse.WhMaterialInventory.Query;
using Hymson.MES.Services.Dtos.Warehouse;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;

namespace Hymson.MES.Services.Services.Warehouse
{
    /// <summary>
    /// 物料库存 服务
    /// </summary>
    public class WhMaterialInventoryService : IWhMaterialInventoryService
    {
        private readonly ICurrentUser _currentUser;

        /// <summary>
        /// 物料库存 仓储
        /// </summary>
        private readonly IWhMaterialInventoryRepository _whMaterialInventoryRepository;
        private readonly IWhMaterialStandingbookRepository _whMaterialStandingbookRepository;

        /// <summary>
        /// 物料维护 仓储
        /// </summary>
        private readonly IProcMaterialRepository _procMaterialRepository;
        /// <summary>
        /// 条码信息表 仓储
        /// </summary>
        private readonly IManuSfcRepository _manuSfcRepository;
        private readonly IWhSupplierRepository _whSupplierRepository;


        /// <summary>
        /// 
        /// </summary>
        private readonly ILocalizationService _localizationService;
        private readonly AbstractValidator<WhMaterialInventoryCreateDto> _validationCreateRules;
        private readonly AbstractValidator<WhMaterialInventoryModifyDto> _validationModifyRules;
        private readonly ICurrentSite _currentSite;


        public WhMaterialInventoryService(ICurrentUser currentUser,
            IWhMaterialInventoryRepository whMaterialInventoryRepository,
            IWhMaterialStandingbookRepository whMaterialStandingbookRepository,
             IProcMaterialRepository procMaterialRepository,
              IManuSfcRepository manuSfcRepository,
        AbstractValidator<WhMaterialInventoryCreateDto> validationCreateRules,
            AbstractValidator<WhMaterialInventoryModifyDto> validationModifyRules,
            ICurrentSite currentSite,
            ILocalizationService localizationService, IWhSupplierRepository whSupplierRepository)
        {
            _currentUser = currentUser;
            _whMaterialInventoryRepository = whMaterialInventoryRepository;
            _whMaterialStandingbookRepository = whMaterialStandingbookRepository;
            _procMaterialRepository = procMaterialRepository;
            _manuSfcRepository = manuSfcRepository;

            _validationCreateRules = validationCreateRules;
            _validationModifyRules = validationModifyRules;
            _currentSite = currentSite;
            _localizationService = localizationService;
            _whSupplierRepository = whSupplierRepository;
        }


        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="whMaterialInventoryCreateDto"></param>
        /// <returns></returns>
        public async Task CreateWhMaterialInventoryAsync(WhMaterialInventoryCreateDto whMaterialInventoryCreateDto)
        {
            //验证DTO
            await _validationCreateRules.ValidateAndThrowAsync(whMaterialInventoryCreateDto);

            //DTO转换实体
            var whMaterialInventoryEntity = whMaterialInventoryCreateDto.ToEntity<WhMaterialInventoryEntity>();
            whMaterialInventoryEntity.Id = IdGenProvider.Instance.CreateId();
            whMaterialInventoryEntity.CreatedBy = _currentUser.UserName;
            whMaterialInventoryEntity.UpdatedBy = _currentUser.UserName;
            whMaterialInventoryEntity.CreatedOn = HymsonClock.Now();
            whMaterialInventoryEntity.UpdatedOn = HymsonClock.Now();
            whMaterialInventoryEntity.SiteId = _currentSite.SiteId ?? 0;

            //入库
            await _whMaterialInventoryRepository.InsertAsync(whMaterialInventoryEntity);
        }

        /// <summary>
        /// 批量创建
        /// </summary>
        /// <param name="whMaterialInventoryLists"></param>
        /// <returns></returns> 
        public async Task CreateWhMaterialInventoryListAsync(List<WhMaterialInventoryListCreateDto> whMaterialInventoryLists)
        {
            var list = new List<WhMaterialInventoryEntity>();
            var listStandingbook = new List<WhMaterialStandingbookEntity>();

            if (whMaterialInventoryLists == null || whMaterialInventoryLists.Count <= 0)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES15106));
            }
            var validationFailures = new List<ValidationFailure>();
            var procMaterials = await _procMaterialRepository.GetByIdsAsync(whMaterialInventoryLists.Select(it => it.MaterialId).Distinct().ToArray());
            if (procMaterials == null || !procMaterials.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES15101));
            }
            foreach (var item in whMaterialInventoryLists)
            {
                #region 校验

                item.MaterialBarCode = item.MaterialBarCode.Trim();
                var isMaterialBarCodeList = whMaterialInventoryLists.Where(it => it.MaterialBarCode.Trim() == item.MaterialBarCode);
                if (isMaterialBarCodeList.Count() > 1)
                {
                    validationFailures.Add(new ValidationFailure
                    {
                        FormattedMessagePlaceholderValues = new Dictionary<string, object> { { "CollectionIndex", item.MaterialBarCode } },
                        ErrorCode = nameof(ErrorCode.MES15107)
                    });

                    continue;
                }

                if (item.SupplierId <= 0)
                {
                    var validationFailure = new ValidationFailure();
                    if (validationFailure.FormattedMessagePlaceholderValues == null || !validationFailure.FormattedMessagePlaceholderValues.Any())
                    {
                        validationFailure.FormattedMessagePlaceholderValues = new Dictionary<string, object> {
                            { "CollectionIndex", item.MaterialBarCode}
                        };
                    }
                    else
                    {
                        validationFailure.FormattedMessagePlaceholderValues.Add("CollectionIndex", item.MaterialBarCode);
                    }
                    validationFailure.ErrorCode = nameof(ErrorCode.MES15108);
                    validationFailures.Add(validationFailure);
                    continue;
                }
                if (item.QuantityResidue <= 0)
                {
                    var validationFailure = new ValidationFailure();
                    if (validationFailure.FormattedMessagePlaceholderValues == null || !validationFailure.FormattedMessagePlaceholderValues.Any())
                    {
                        validationFailure.FormattedMessagePlaceholderValues = new Dictionary<string, object> {
                            { "CollectionIndex", item.MaterialBarCode}
                        };
                    }
                    else
                    {
                        validationFailure.FormattedMessagePlaceholderValues.Add("CollectionIndex", item.MaterialBarCode);
                    }
                    validationFailure.ErrorCode = nameof(ErrorCode.MES15103);
                    validationFailures.Add(validationFailure);
                    continue;
                }
                var materialInfo = procMaterials.FirstOrDefault(it => it.Id == item.MaterialId);
                if (materialInfo == null)
                {
                    var validationFailure = new ValidationFailure();
                    if (validationFailure.FormattedMessagePlaceholderValues == null || !validationFailure.FormattedMessagePlaceholderValues.Any())
                    {
                        validationFailure.FormattedMessagePlaceholderValues = new Dictionary<string, object> {
                            { "CollectionIndex", item.MaterialBarCode}
                        };
                    }
                    else
                    {
                        validationFailure.FormattedMessagePlaceholderValues.Add("CollectionIndex", item.MaterialBarCode);
                    }
                    validationFailure.ErrorCode = nameof(ErrorCode.MES15101);
                    validationFailures.Add(validationFailure);
                    continue;
                }
                await GetMaterialBarCodeAnyAsync(item.MaterialBarCode);

                #endregion

                #region 数据组装
                //物料库存
                var whMaterialInventoryEntity = new WhMaterialInventoryEntity();
                whMaterialInventoryEntity.SupplierId = item.SupplierId;// supplierInfo.FirstOrDefault().Id;//item.SupplierId;//
                whMaterialInventoryEntity.MaterialId = materialInfo.Id;
                whMaterialInventoryEntity.MaterialBarCode = item.MaterialBarCode;
                whMaterialInventoryEntity.Batch = item.Batch;
                whMaterialInventoryEntity.MaterialType = MaterialInventoryMaterialTypeEnum.PurchaseParts;
                whMaterialInventoryEntity.QuantityResidue = item.QuantityResidue;
                whMaterialInventoryEntity.Status = WhMaterialInventoryStatusEnum.ToBeUsed;
                whMaterialInventoryEntity.DueDate = item.DueDate;
                whMaterialInventoryEntity.Source = item.Source;
                whMaterialInventoryEntity.SiteId = _currentSite.SiteId ?? 0;


                whMaterialInventoryEntity.Id = IdGenProvider.Instance.CreateId();
                whMaterialInventoryEntity.CreatedBy = _currentUser.UserName;
                whMaterialInventoryEntity.UpdatedBy = _currentUser.UserName;
                whMaterialInventoryEntity.CreatedOn = HymsonClock.Now();
                whMaterialInventoryEntity.UpdatedOn = HymsonClock.Now();
                list.Add(whMaterialInventoryEntity);


                //台账数据
                var whMaterialStandingbookEntity = new WhMaterialStandingbookEntity();
                whMaterialStandingbookEntity.MaterialCode = materialInfo.MaterialCode;
                whMaterialStandingbookEntity.MaterialName = materialInfo.MaterialName;
                string version = materialInfo.Version ?? "";
                if (!string.IsNullOrWhiteSpace(item.Version))
                {
                    version = item.Version;
                }
                whMaterialStandingbookEntity.MaterialVersion = version;
                whMaterialStandingbookEntity.MaterialBarCode = item.MaterialBarCode;
                whMaterialStandingbookEntity.Batch = item.Batch;
                whMaterialStandingbookEntity.Quantity = item.QuantityResidue;
                whMaterialStandingbookEntity.Unit = materialInfo.Unit ?? "";
                whMaterialStandingbookEntity.Type = item.Type;
                whMaterialStandingbookEntity.Source = item.Source;
                whMaterialStandingbookEntity.SiteId = _currentSite.SiteId ?? 0;

                whMaterialStandingbookEntity.SupplierId = item.SupplierId;

                whMaterialStandingbookEntity.Id = IdGenProvider.Instance.CreateId();
                whMaterialStandingbookEntity.CreatedBy = _currentUser.UserName;
                whMaterialStandingbookEntity.UpdatedBy = _currentUser.UserName;
                whMaterialStandingbookEntity.CreatedOn = HymsonClock.Now();
                whMaterialStandingbookEntity.UpdatedOn = HymsonClock.Now();

                listStandingbook.Add(whMaterialStandingbookEntity);
                #endregion
            }
            // 是否存在错误
            if (validationFailures.Any())
            {
                throw new ValidationException(_localizationService.GetResource("SFCError"), validationFailures);
            }
            #region 入库
            // 保存实体
            var rows = 0;
            using (var trans = TransactionHelper.GetTransactionScope())
            {
                rows += await _whMaterialInventoryRepository.InsertsAsync(list);
                rows += await _whMaterialStandingbookRepository.InsertsAsync(listStandingbook);
                trans.Complete();
            }
            if (rows == 0)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES15105));
            }
            #endregion
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
        public async Task<bool> GetMaterialBarCodeAnyAsync(string materialBarCode)
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

            var sfcEntit = await _manuSfcRepository.GetBySFCAsync(new GetBySfcQuery { SFC = materialBarCode, SiteId = _currentSite.SiteId });
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

            // 只有【待使用】的库存才能上料！
            if (entity.Status != WhMaterialInventoryStatusEnum.ToBeUsed) throw new CustomerValidationException(nameof(ErrorCode.MES152018));

            return entity.ToModel<WhMaterialInventoryDto>();
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
            whMaterialInventoryEntity.Batch = modifyDto.Batch ?? "";
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
    }
}
