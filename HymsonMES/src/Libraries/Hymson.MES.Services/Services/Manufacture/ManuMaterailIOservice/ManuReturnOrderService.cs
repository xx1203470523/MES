using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Manufacture.ManuRequistionOrder;
using Hymson.MES.Data.Repositories.Manufacture.Query;
using Hymson.MES.Data.Repositories.Plan;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.Warehouse;
using Hymson.MES.Services.Dtos.Manufacture;
using Hymson.Snowflake;
using Hymson.Utils;

namespace Hymson.MES.Services.Services.Manufacture
{
    /// <summary>
    /// 服务（生产退料单） 
    /// </summary>
    public class ManuReturnOrderService : IManuReturnOrderService
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
        private readonly AbstractValidator<ManuReturnOrderSaveDto> _validationSaveRules;

        /// <summary>
        /// 仓储接口（生产退料单）
        /// </summary>
        private readonly IManuReturnOrderRepository _manuReturnOrderRepository;

        /// <summary>
        /// 仓储接口（生产退料明细单）
        /// </summary>
        private readonly IManuReturnOrderDetailRepository _manuReturnOrderDetailRepository;

        /// <summary>
        /// 仓储接口（物料维护）
        /// </summary>
        private readonly IProcMaterialRepository _procMaterialRepository;

        /// <summary>
        /// 仓储接口（生产工单）
        /// </summary>
        private readonly IPlanWorkOrderRepository _planWorkOrderRepository;

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
        /// <param name="manuReturnOrderRepository"></param>
        /// <param name="manuReturnOrderDetailRepository"></param>
        /// <param name="procMaterialRepository"></param>
        /// <param name="planWorkOrderRepository"></param>
        /// <param name="whSupplierRepository"></param>
        public ManuReturnOrderService(ICurrentUser currentUser, ICurrentSite currentSite,
            AbstractValidator<ManuReturnOrderSaveDto> validationSaveRules,
            IManuReturnOrderRepository manuReturnOrderRepository,
            IManuReturnOrderDetailRepository manuReturnOrderDetailRepository,
            IProcMaterialRepository procMaterialRepository,
            IPlanWorkOrderRepository planWorkOrderRepository,
            IWhSupplierRepository whSupplierRepository)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _validationSaveRules = validationSaveRules;
            _manuReturnOrderRepository = manuReturnOrderRepository;
            _manuReturnOrderDetailRepository = manuReturnOrderDetailRepository;
            _procMaterialRepository = procMaterialRepository;
            _planWorkOrderRepository = planWorkOrderRepository;
            _whSupplierRepository = whSupplierRepository;
        }


        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        public async Task<int> CreateAsync(ManuReturnOrderSaveDto saveDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0) throw new CustomerValidationException(nameof(ErrorCode.MES10101));

            // 验证DTO
            await _validationSaveRules.ValidateAndThrowAsync(saveDto);

            // 更新时间
            var updatedBy = _currentUser.UserName;
            var updatedOn = HymsonClock.Now();

            // DTO转换实体
            var entity = saveDto.ToEntity<ManuReturnOrderEntity>();
            entity.Id = IdGenProvider.Instance.CreateId();
            entity.CreatedBy = updatedBy;
            entity.CreatedOn = updatedOn;
            entity.UpdatedBy = updatedBy;
            entity.UpdatedOn = updatedOn;
            entity.SiteId = _currentSite.SiteId ?? 0;

            // 保存
            return await _manuReturnOrderRepository.InsertAsync(entity);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        public async Task<int> ModifyAsync(ManuReturnOrderSaveDto saveDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0) throw new CustomerValidationException(nameof(ErrorCode.MES10101));

            // 验证DTO
            await _validationSaveRules.ValidateAndThrowAsync(saveDto);

            // DTO转换实体
            var entity = saveDto.ToEntity<ManuReturnOrderEntity>();
            entity.UpdatedBy = _currentUser.UserName;
            entity.UpdatedOn = HymsonClock.Now();

            return await _manuReturnOrderRepository.UpdateAsync(entity);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(long id)
        {
            return await _manuReturnOrderRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeletesAsync(long[] ids)
        {
            return await _manuReturnOrderRepository.DeletesAsync(new DeleteCommand
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
        public async Task<ManuReturnOrderDto?> QueryByIdAsync(long id)
        {
            var manuReturnOrderEntity = await _manuReturnOrderRepository.GetByIdAsync(id);
            if (manuReturnOrderEntity == null) return null;

            return manuReturnOrderEntity.ToModel<ManuReturnOrderDto>();
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ManuReturnOrderDto>> GetPagedListAsync(ManuReturnOrderPagedQueryDto pagedQueryDto)
        {
            var pagedQuery = pagedQueryDto.ToQuery<ManuReturnOrderPagedQuery>();
            pagedQuery.SiteId = _currentSite.SiteId ?? 0;
            pagedQuery.Type = ManuReturnTypeEnum.WorkOrderReturn;
            var pagedInfo = await _manuReturnOrderRepository.GetPagedListAsync(pagedQuery);

            // 转换工单编码变为工单ID
            if (!string.IsNullOrWhiteSpace(pagedQueryDto.WorkOrderCode))
            {
                var workOrderEntities = await _planWorkOrderRepository.GetEntitiesAsync(new PlanWorkOrderNewQuery
                {
                    SiteId = pagedQuery.SiteId,
                    OrderCode = pagedQueryDto.WorkOrderCode
                });
                if (workOrderEntities != null && workOrderEntities.Any()) pagedQuery.SourceWorkOrderIds = workOrderEntities.Select(s => s.Id);
                else pagedQuery.SourceWorkOrderIds = Array.Empty<long>();
            }

            // 实体到DTO转换 装载数据
            var dtos = pagedInfo.Data.Select(s => s.ToModel<ManuReturnOrderDto>());
            return new PagedInfo<ManuReturnOrderDto>(dtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 查询详情（生产退料表）
        /// </summary>
        /// <param name="returnId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuReturnOrderDetailDto>> QueryDetailByReturnIdAsync(long returnId)
        {
            List<ManuReturnOrderDetailDto> dtos = new();

            var returnDetailEntities = await _manuReturnOrderDetailRepository.GetEntitiesAsync(new ManuReturnOrderDetailQuery
            {
                SiteId = _currentSite.SiteId ?? 0,
                ReturnOrderId = returnId
            });

            // 读取产品
            var materialEntities = await _procMaterialRepository.GetByIdsAsync(returnDetailEntities.Select(x => x.MaterialId));
            var materialDic = materialEntities.ToDictionary(x => x.Id, x => x);

            // 读取供应商
            var supplierEntities = await _whSupplierRepository.GetByIdsAsync(returnDetailEntities.Select(s => s.SupplierId));

            foreach (var entity in returnDetailEntities)
            {
                var dto = entity.ToModel<ManuReturnOrderDetailDto>();
                if (dto == null) continue;

                // 供应商
                var supplierEntity = supplierEntities.FirstOrDefault(f => f.Id == entity.SupplierId);
                if (supplierEntity != null)
                {
                    dto.SupplierCode = supplierEntity.Code;
                    dto.SupplierName = supplierEntity.Name;
                }

                // 产品
                materialDic.TryGetValue(entity.MaterialId, out var materialEntity);
                if (materialEntity != null)
                {
                    dto.MaterialCode = materialEntity.MaterialCode;
                    dto.MaterialName = materialEntity.MaterialName;
                }
                else
                {
                    dto.MaterialCode = "-";
                    dto.MaterialName = "-";
                }

                dtos.Add(dto);
            }
            return dtos;
        }

        /// <summary>
        /// 根据工单查询退料明细
        /// </summary>
        /// <param name="workOrderId"></param>
        /// <returns></returns>
        public async Task<List<OrderManuReturnDetailDto>> GetReturnDetailByOrderIdAsync(long workOrderId)
        {
            var details = new List<OrderManuReturnDetailDto>();
            var planWorkOrderEntity = await _planWorkOrderRepository.GetByIdAsync(workOrderId);
            if (planWorkOrderEntity == null)
            {
                return details;
            }

            var returnOrderEntities = await _manuReturnOrderRepository.GetEntitiesAsync(new ManuReturnOrderQuery
            {
                SiteId = planWorkOrderEntity.SiteId,
                WorkOrderId = workOrderId
            });
            if (returnOrderEntities == null || !returnOrderEntities.Any())
            {
                return details;
            }

            var returnOrderIds = returnOrderEntities.Select(x => x.Id).ToArray();
            var returnOrderDetailEntities = await _manuReturnOrderDetailRepository.GetEntitiesAsync(new ManuReturnOrderDetailQuery
            {
                SiteId = planWorkOrderEntity.SiteId,
                ReturnOrderIds = returnOrderIds
            });

            var materIds = returnOrderDetailEntities.Select(x => x.MaterialId).ToArray();
            var procMaterialEntities = await _procMaterialRepository.GetByIdsAsync(materIds);
            foreach (var item in returnOrderDetailEntities)
            {
                var retrurnOrder = returnOrderEntities.FirstOrDefault(x => x.Id == item.ReturnOrderId);
                var material = procMaterialEntities.FirstOrDefault(x => x.Id == item.MaterialId);
                details.Add(new OrderManuReturnDetailDto
                {
                    ReturnOrderCode = retrurnOrder?.ReturnOrderCode ?? "",
                    MaterialCode = material?.MaterialCode ?? "",
                    MaterialName = material?.MaterialName ?? "",
                    Version = material?.Version ?? "",
                    MaterialBarCode = item.MaterialBarCode,
                    Batch = item.Batch ?? "",
                    Qty = item.Qty,
                    ReturnTime = retrurnOrder?.CreatedOn ?? item.CreatedOn,
                    Status = retrurnOrder?.Status
                });
            }
            return details;
        }
    }
}
