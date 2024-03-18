using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.WHMaterialReceipt;
using Hymson.MES.Core.Domain.WHMaterialReceiptDetail;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.Query;
using Hymson.MES.Data.Repositories.Warehouse;
using Hymson.MES.Data.Repositories.WHMaterialReceipt;
using Hymson.MES.Data.Repositories.WhMaterialReceiptDetail;
using Hymson.MES.Services.Dtos.WHMaterialReceipt;
using Hymson.MES.Services.Dtos.WHMaterialReceiptDetail;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;
using System.Transactions;

namespace Hymson.MES.Services.Services.WHMaterialReceipt
{
    /// <summary>
    /// 服务（物料收货表） 
    /// </summary>
    public class WhMaterialReceiptService : IWhMaterialReceiptService
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
        private readonly AbstractValidator<WhMaterialReceiptSaveDto> _validationSaveRules;

        /// <summary>
        /// 仓储接口（物料收货表）
        /// </summary>
        private readonly IWhMaterialReceiptRepository _whMaterialReceiptRepository;

        /// <summary>
        /// 仓储接口（物料收货详细表）
        /// </summary>
        private readonly IWhMaterialReceiptDetailRepository _whMaterialReceiptDetailRepository;

        /// <summary>
        /// 仓储接口（物料维护）
        /// </summary>
        private readonly IProcMaterialRepository _procMaterialRepository;

        /// <summary>
        /// 仓储接口（供应商维护）
        /// </summary>
        private readonly IWhSupplierRepository _whSupplierRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="currentSite"></param>
        /// <param name="validationSaveRules"></param>
        /// <param name="whMaterialReceiptRepository"></param
        /// <param name="whMaterialReceiptDetailRepository"></param>
        /// <param name="procMaterialRepository"></param>
        /// <param name="whSupplierRepository"></param>
        public WhMaterialReceiptService(ICurrentUser currentUser, ICurrentSite currentSite,
            AbstractValidator<WhMaterialReceiptSaveDto> validationSaveRules,
            IWhMaterialReceiptRepository whMaterialReceiptRepository,
            IWhMaterialReceiptDetailRepository whMaterialReceiptDetailRepository,
            IProcMaterialRepository procMaterialRepository,
            IWhSupplierRepository whSupplierRepository)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _validationSaveRules = validationSaveRules;
            _whMaterialReceiptRepository = whMaterialReceiptRepository;
            _whMaterialReceiptDetailRepository = whMaterialReceiptDetailRepository;
            _procMaterialRepository = procMaterialRepository;
            _whSupplierRepository = whSupplierRepository;
        }


        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        public async Task CreateAsync(WhMaterialReceiptSaveDto saveDto)
        {
            // 判断是否有获取到站点码 
            //if (_currentSite.SiteId == 0) throw new CustomerValidationException(nameof(ErrorCode.MES10101));

            // 验证DTO
            await _validationSaveRules.ValidateAndThrowAsync(saveDto);

            // 更新时间
            var updatedBy = _currentUser.UserName;
            var updatedOn = HymsonClock.Now();

            // DTO转换实体
            var entity = saveDto.ToEntity<WhMaterialReceiptEntity>();
            entity.Id = IdGenProvider.Instance.CreateId();
            entity.CreatedBy = updatedBy;
            entity.CreatedOn = updatedOn;
            entity.UpdatedBy = updatedBy;
            entity.UpdatedOn = updatedOn;
            entity.IsDeleted = 0;
            //entity.SiteId = _currentSite.SiteId ?? 0;
            //临时
            entity.SiteId = saveDto.SiteId;

            //是否重复收货单号
            var isReceip = await _whMaterialReceiptRepository.GetEntitiesAsync(new WhMaterialReceiptQuery { ReceiptNum = entity.ReceiptNum });
            if (isReceip.Any()) throw new CustomerValidationException(nameof(ErrorCode.MES19227)).WithData("ReceiptNum", entity.ReceiptNum);

            var details = new List<WHMaterialReceiptDetailEntity>();
            if (saveDto.Details != null)
            {
                foreach (var item in saveDto.Details)
                {

                    details.Add(new WHMaterialReceiptDetailEntity()
                    {
                        Id = IdGenProvider.Instance.CreateId(),
                        MaterialReceiptId = entity.Id,
                        MaterialId = item.MaterialId ?? 0,
                        SupplierBatch = item.SupplierBatch,
                        PlanQty = item.PlanQty,
                        Qty = item.Qty,
                        InternalBatch = item.InternalBatch,
                        PlanTime = item.PlanTime,
                        Remark = item.Remark,
                        CreatedBy = updatedBy,
                        CreatedOn = updatedOn,
                        UpdatedBy = updatedBy,
                        UpdatedOn = updatedOn,
                        SiteId = entity.SiteId
                    });
                }

            }


            using (TransactionScope ts = TransactionHelper.GetTransactionScope())
            {
                await _whMaterialReceiptRepository.InsertAsync(entity);
                //先删除
                //await _whMaterialReceiptRepository.DeletesDetailByIdAsync(new long[] { entity.Id });
                if (details.Any())
                    await _whMaterialReceiptRepository.InsertDetailAsync(details);

                ts.Complete();
            }
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        public async Task<int> ModifyAsync(WhMaterialReceiptSaveDto saveDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0) throw new CustomerValidationException(nameof(ErrorCode.MES10101));

            // 验证DTO
            await _validationSaveRules.ValidateAndThrowAsync(saveDto);

            // DTO转换实体
            var entity = saveDto.ToEntity<WhMaterialReceiptEntity>();
            entity.UpdatedBy = _currentUser.UserName;
            entity.UpdatedOn = HymsonClock.Now();

            return await _whMaterialReceiptRepository.UpdateAsync(entity);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(long id)
        {
            return await _whMaterialReceiptRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeletesAsync(long[] ids)
        {
            return await _whMaterialReceiptRepository.DeletesAsync(new DeleteCommand
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
        public async Task<WhMaterialReceiptDto?> QueryByIdAsync(long id)
        {
            var whMaterialReceiptEntity = await _whMaterialReceiptRepository.GetByIdAsync(id);
            if (whMaterialReceiptEntity == null) return null;

            return whMaterialReceiptEntity.ToModel<WhMaterialReceiptDto>();
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<WhMaterialReceiptDto>> GetPagedListAsync(WhMaterialReceiptPagedQueryDto pagedQueryDto)
        {
            var pagedQuery = pagedQueryDto.ToQuery<WhMaterialReceiptPagedQuery>();
            if (!pagedQuery.SiteId.HasValue)
            {
                pagedQuery.SiteId = _currentSite.SiteId ?? 0;
            }

            // 转换供应商编码变为供应商ID
            if (!string.IsNullOrWhiteSpace(pagedQueryDto.SupplierCode)
                || !string.IsNullOrWhiteSpace(pagedQueryDto.SupplierName))
            {
                var whSupplierEntities = await _whSupplierRepository.GetWhSupplierEntitiesAsync(new WhSupplierQuery
                {
                    SiteId = pagedQuery.SiteId,
                    Code = pagedQueryDto.SupplierCode,
                    Name = pagedQueryDto.SupplierName
                });
                if (whSupplierEntities != null && whSupplierEntities.Any()) pagedQuery.SupplierIds = whSupplierEntities.Select(s => s.Id);
                else pagedQuery.SupplierIds = Array.Empty<long>();
            }

            // 查询数据
            var pagedInfo = await _whMaterialReceiptRepository.GetPagedListAsync(pagedQuery);

            // 实体到DTO转换 装载数据
            var dtos = await PrepareDtos(pagedInfo.Data);
            return new PagedInfo<WhMaterialReceiptDto>(dtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 查询详情（物料收货表）
        /// </summary>
        /// <param name="receiptId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ReceiptMaterialDetailDto>> QueryDetailByReceiptIdAsync(long receiptId)
        {
            var entities = await _whMaterialReceiptDetailRepository.GetEntitiesAsync(new WhMaterialReceiptDetailQuery
            {
                SiteId = _currentSite.SiteId ?? 0,
                MaterialReceiptId = receiptId
            });

            // 读取收货单
            var receiptEntities = await _whMaterialReceiptRepository.GetByIdsAsync(entities.Select(x => x.MaterialReceiptId));
            var receiptDic = receiptEntities.ToDictionary(x => x.Id, x => x);

            // 读取产品
            //var materialEntities = await _procMaterialRepository.GetByIdsAsync(entities.Where(w => w.MaterialId.HasValue).Select(x => x.MaterialId!.Value));
            var materialEntities = await _procMaterialRepository.GetByIdsAsync(entities.Select(x => x.MaterialId));
            var materialDic = materialEntities.ToDictionary(x => x.Id, x => x);

            // 读取供应商
            var supplierEntities = await _whSupplierRepository.GetByIdsAsync(receiptEntities.Select(x => x.SupplierId));
            var supplierDic = supplierEntities.ToDictionary(x => x.Id, x => x);

            List<ReceiptMaterialDetailDto> dtos = new();
            foreach (var entity in entities)
            {
                var dto = entity.ToModel<ReceiptMaterialDetailDto>();

                // 收货单
                var receiptEntity = receiptDic[entity.MaterialReceiptId];
                if (receiptEntity != null)
                {
                    dto.ReceiptNum = receiptEntity.ReceiptNum;

                    // 供应商                    
                    supplierDic.TryGetValue(receiptEntity.SupplierId, out var supplierEntity);
                    if (supplierEntity != null)
                    {
                        dto.SupplierCode = supplierEntity.Code;
                        dto.SupplierName = supplierEntity.Name;
                    }
                }

                // 产品
                materialDic.TryGetValue(entity.MaterialId, out var materialEntity);
                if (materialEntity != null)
                {
                    dto.MaterialCode = materialEntity.MaterialCode;
                    dto.MaterialName = materialEntity.MaterialName;
                    dto.MaterialVersion = materialEntity.Version ?? "";
                }

                dtos.Add(dto);
            }
            return dtos;
        }



        #region 内部方法
        /// <summary>
        /// 转换为Dto对象
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        private async Task<IEnumerable<WhMaterialReceiptDto>> PrepareDtos(IEnumerable<WhMaterialReceiptEntity> entities)
        {
            List<WhMaterialReceiptDto> dtos = new();

            // 读取供应商
            var supplierEntities = await _whSupplierRepository.GetByIdsAsync(entities.Select(x => x.SupplierId));
            var supplierDic = supplierEntities.ToDictionary(x => x.Id, x => x);

            foreach (var entity in entities)
            {
                var dto = entity.ToModel<WhMaterialReceiptDto>();
                if (dto == null) continue;

                var supplierEntity = supplierDic[entity.SupplierId];
                if (supplierEntity != null)
                {
                    dto.SupplierCode = supplierEntity.Code;
                    dto.SupplierName = supplierEntity.Name;
                }

                dtos.Add(dto);
            }

            return dtos;
        }

        #endregion


    }
}
