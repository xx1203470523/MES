/*
 *creator: Karl
 *
 *describe: 物料库存    服务 | 代码由框架生成
 *builder:  pengxin
 *build datetime: 2023-03-06 03:27:59
 */
using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Warehouse;
using Hymson.MES.Core.Enums;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.Warehouse;
using Hymson.MES.Data.Repositories.Warehouse.WhMaterialInventory.Query;
using Hymson.MES.Services.Dtos.Warehouse;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;
//using Hymson.Utils.Extensions;

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
        private readonly AbstractValidator<WhMaterialInventoryCreateDto> _validationCreateRules;
        private readonly AbstractValidator<WhMaterialInventoryModifyDto> _validationModifyRules;
        private readonly ICurrentSite _currentSite;


        public WhMaterialInventoryService(ICurrentUser currentUser,
            IWhMaterialInventoryRepository whMaterialInventoryRepository,
            IWhMaterialStandingbookRepository whMaterialStandingbookRepository,
             IProcMaterialRepository procMaterialRepository,
        AbstractValidator<WhMaterialInventoryCreateDto> validationCreateRules,
            AbstractValidator<WhMaterialInventoryModifyDto> validationModifyRules,
            ICurrentSite currentSite)
        {
            _currentUser = currentUser;
            _whMaterialInventoryRepository = whMaterialInventoryRepository;
            _whMaterialStandingbookRepository = whMaterialStandingbookRepository;
            _procMaterialRepository = procMaterialRepository;

            _validationCreateRules = validationCreateRules;
            _validationModifyRules = validationModifyRules;
            _currentSite = currentSite;
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

            if (whMaterialInventoryLists == null || whMaterialInventoryLists.Count() <= 0)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES15106));
            }
            foreach (var item in whMaterialInventoryLists)
            {
                item.MaterialBarCode = item.MaterialBarCode.Trim();
                var isMaterialBarCodeList = whMaterialInventoryLists.Where(it => it.MaterialBarCode.Trim() == item.MaterialBarCode);
                if (isMaterialBarCodeList.Count() > 1)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES15107)).WithData("MaterialCode", item.MaterialBarCode);
                }
                #region 校验
                //验证DTO
                //await _validationCreateListRules.ValidateAndThrowAsync(item);

                if (item.QuantityResidue <= 0)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES15103)).WithData("MaterialCode", item.MaterialBarCode);
                }
                //DTO转换实体 
                //var whMaterialInventoryEntity = item.ToEntity<WhMaterialInventoryEntity>();
                var materialInfo = await _whMaterialInventoryRepository.GetProcMaterialByMaterialCodeAsync(item.MaterialId);
                if (materialInfo == null)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES15101));
                }
                //var supplierInfo = await _whMaterialInventoryRepository.GetWhSupplierByMaterialIdAsync(materialInfo.Id, item.SupplierId);
                //if (materialInfo == null || supplierInfo.Count() <= 0)
                //{
                //    throw new CustomerValidationException(nameof(ErrorCode.MES15102)).WithData("MaterialCode", item.MaterialCode);
                //}

                var isMaterialBarCode = await GetMaterialBarCodeAnyAsync(item.MaterialBarCode);
                if (isMaterialBarCode)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES15104)).WithData("MaterialCode", item.MaterialBarCode);
                }

                #endregion

                #region 数据组装
                //物料库存
                var whMaterialInventoryEntity = new WhMaterialInventoryEntity();
                whMaterialInventoryEntity.SupplierId = item.SupplierId;// supplierInfo.FirstOrDefault().Id;//item.SupplierId;//
                whMaterialInventoryEntity.MaterialId = materialInfo.Id;
                whMaterialInventoryEntity.MaterialBarCode = item.MaterialBarCode;
                whMaterialInventoryEntity.Batch = item.Batch;
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
                string version = materialInfo.Version;
                if (!string.IsNullOrWhiteSpace(item.Version))
                {
                    version = item.Version;
                }
                whMaterialStandingbookEntity.MaterialVersion = version;
                whMaterialStandingbookEntity.MaterialBarCode = item.MaterialBarCode;
                whMaterialStandingbookEntity.Batch = item.Batch;
                whMaterialStandingbookEntity.Quantity = item.QuantityResidue;
                whMaterialStandingbookEntity.Unit = materialInfo.Unit ?? "";
                whMaterialStandingbookEntity.Type = item.Type; //(int)WhMaterialInventorySourceEnum.MaterialReceiving;
                whMaterialStandingbookEntity.Source = item.Source;
                whMaterialStandingbookEntity.SiteId = _currentSite.SiteId ?? 0;


                whMaterialStandingbookEntity.Id = IdGenProvider.Instance.CreateId();
                whMaterialStandingbookEntity.CreatedBy = _currentUser.UserName;
                whMaterialStandingbookEntity.UpdatedBy = _currentUser.UserName;
                whMaterialStandingbookEntity.CreatedOn = HymsonClock.Now();
                whMaterialStandingbookEntity.UpdatedOn = HymsonClock.Now();

                listStandingbook.Add(whMaterialStandingbookEntity);
                #endregion
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
                MaterialBarCode = materialBarCode
            });
            return pagedInfo.Any();
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
            return null;
        }

        /// <summary>
        /// 根据物料条码查询
        /// </summary>
        /// <param name="barCode"></param>
        /// <returns></returns>
        public async Task<WhMaterialInventoryDto?> QueryWhMaterialInventoryByBarCodeAsync(string barCode)
        {
            var entity = await _whMaterialInventoryRepository.GetByBarCodeAsync(barCode);
            if (entity == null) return null;

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
            var supplierInfo = await _whMaterialInventoryRepository.GetWhSupplierByMaterialIdAsync(materialInfo.Id);
            if (!supplierInfo.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES15102));
            }
            ProcMaterialInfoViewDto dto = new ProcMaterialInfoViewDto();
            dto.MaterialInfo = materialInfo;
            dto.SupplierInfo = supplierInfo;
            return dto;
        }
    }
}
