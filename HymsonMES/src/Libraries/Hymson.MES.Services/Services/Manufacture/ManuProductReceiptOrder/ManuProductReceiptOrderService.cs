using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Enums;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Manufacture.Query;
using Hymson.MES.Services.Dtos.Manufacture;
using Hymson.Snowflake;
using Hymson.Utils;

namespace Hymson.MES.Services.Services.Manufacture
{
    /// <summary>
    /// 服务（工单完工入库） 
    /// </summary>
    public class ManuProductReceiptOrderService : IManuProductReceiptOrderService
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
        private readonly AbstractValidator<ManuProductReceiptOrderSaveDto> _validationSaveRules;

        /// <summary>
        /// 仓储接口（工单完工入库）
        /// </summary>
        private readonly IManuProductReceiptOrderRepository _manuProductReceiptOrderRepository;

        private readonly IManuProductReceiptOrderDetailRepository _manuProductReceiptOrderDetailRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="currentSite"></param>
        /// <param name="validationSaveRules"></param>
        /// <param name="manuProductReceiptOrderRepository"></param>
        public ManuProductReceiptOrderService(ICurrentUser currentUser, ICurrentSite currentSite, AbstractValidator<ManuProductReceiptOrderSaveDto> validationSaveRules,
            IManuProductReceiptOrderRepository manuProductReceiptOrderRepository, IManuProductReceiptOrderDetailRepository manuProductReceiptOrderDetailRepository)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _validationSaveRules = validationSaveRules;
            _manuProductReceiptOrderRepository = manuProductReceiptOrderRepository;
            _manuProductReceiptOrderDetailRepository = manuProductReceiptOrderDetailRepository;
        }


        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        public async Task<int> CreateAsync(ManuProductReceiptOrderSaveDto saveDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0) throw new CustomerValidationException(nameof(ErrorCode.MES10101));

            // 验证DTO
            await _validationSaveRules.ValidateAndThrowAsync(saveDto);

            // 更新时间
            var updatedBy = _currentUser.UserName;
            var updatedOn = HymsonClock.Now();

            // DTO转换实体
            var entity = saveDto.ToEntity<ManuProductReceiptOrderEntity>();
            entity.Id = IdGenProvider.Instance.CreateId();
            entity.CreatedBy = updatedBy;
            entity.CreatedOn = updatedOn;
            entity.UpdatedBy = updatedBy;
            entity.UpdatedOn = updatedOn;
            entity.SiteId = _currentSite.SiteId ?? 0;

            // 保存
            return await _manuProductReceiptOrderRepository.InsertAsync(entity);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        public async Task<int> ModifyAsync(ManuProductReceiptOrderSaveDto saveDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0) throw new CustomerValidationException(nameof(ErrorCode.MES10101));

            // 验证DTO
            await _validationSaveRules.ValidateAndThrowAsync(saveDto);

            // DTO转换实体
            var entity = saveDto.ToEntity<ManuProductReceiptOrderEntity>();
            entity.UpdatedBy = _currentUser.UserName;
            entity.UpdatedOn = HymsonClock.Now();

            return await _manuProductReceiptOrderRepository.UpdateAsync(entity);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(long id)
        {
            return await _manuProductReceiptOrderRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeletesAsync(long[] ids)
        {
            return await _manuProductReceiptOrderRepository.DeletesAsync(new DeleteCommand
            {
                Ids = ids,
                DeleteOn = HymsonClock.Now(),
                UserId = _currentUser.UserName
            });
        }

        /// <summary>
        /// 根据工单查询入库记录
        /// </summary>
        /// <param name="workOrderId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuProductReceiptOrderDetailDto>> QueryByWorkIdAsync(long workOrderId)
        {
            var manuProductReceiptOrders = new List<ManuProductReceiptOrderDetailDto>();
            var manuProductReceiptOrderLists = await _manuProductReceiptOrderRepository.GetByWorkOrderIdsSqlAsync(workOrderId);
            if (manuProductReceiptOrderLists.Any() == false)
            {
                return manuProductReceiptOrders;
            }
            var manuProductReceiptOrderIds = manuProductReceiptOrderLists.Select(x => x.Id).ToArray();
            var productReceiptOrderDetailEntities = await _manuProductReceiptOrderDetailRepository.GetByProductReceiptIdsAsync(manuProductReceiptOrderIds);
            if (productReceiptOrderDetailEntities.Any())
            {
                foreach (var entity in productReceiptOrderDetailEntities)
                {
                    var manuProductReceiptOrder = manuProductReceiptOrderLists.FirstOrDefault(x => x.Id == entity.ProductReceiptId);
                    var manuProductReceipt = new ManuProductReceiptOrderDetailDto
                    {
                        CompletionOrderCode = manuProductReceiptOrder?.CompletionOrderCode ?? "",
                        StorageStatus = manuProductReceiptOrder?.Status ?? ProductReceiptStatusEnum.Approvaling,
                        Batch = entity.Batch,
                        Qty = entity.Qty,
                        ContaineCode = entity.ContaineCode,
                        WarehouseCode = entity.WarehouseCode,
                        Sfc = entity.Sfc,
                        Status = entity.Status,
                        Unit = entity.Unit,
                        CreateDate = entity.CreatedOn,
                        CreatedBy = entity.CreatedBy
                    };
                    manuProductReceiptOrders.Add(manuProductReceipt);
                }
            }
            return manuProductReceiptOrders;
        }

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ManuProductReceiptOrderDto?> QueryByIdAsync(long id)
        {
            var manuProductReceiptOrderEntity = await _manuProductReceiptOrderRepository.GetByIdAsync(id);
            if (manuProductReceiptOrderEntity == null) return null;

            return manuProductReceiptOrderEntity.ToModel<ManuProductReceiptOrderDto>();
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ManuProductReceiptOrderDto>> GetPagedListAsync(ManuProductReceiptOrderPagedQueryDto pagedQueryDto)
        {
            var pagedQuery = pagedQueryDto.ToQuery<ManuProductReceiptOrderPagedQuery>();
            pagedQuery.SiteId = _currentSite.SiteId ?? 0;
            var pagedInfo = await _manuProductReceiptOrderRepository.GetPagedListAsync(pagedQuery);

            // 实体到DTO转换 装载数据
            var dtos = pagedInfo.Data.Select(s => s.ToModel<ManuProductReceiptOrderDto>());
            return new PagedInfo<ManuProductReceiptOrderDto>(dtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

    }
}
