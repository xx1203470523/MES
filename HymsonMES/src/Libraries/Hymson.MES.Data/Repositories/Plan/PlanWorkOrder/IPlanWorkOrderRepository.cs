using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Domain.Plan;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Plan.PlanWorkOrder.Command;
using Hymson.MES.Data.Repositories.Plan.PlanWorkOrder.Query;

namespace Hymson.MES.Data.Repositories.Plan
{
    /// <summary>
    /// 工单信息表仓储接口
    /// </summary>
    public interface IPlanWorkOrderRepository
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="planWorkOrderEntity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(PlanWorkOrderEntity planWorkOrderEntity);

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="planWorkOrderEntitys"></param>
        /// <returns></returns>
        Task<int> InsertsAsync(IEnumerable<PlanWorkOrderEntity> planWorkOrderEntitys);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="planWorkOrderEntity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(PlanWorkOrderEntity planWorkOrderEntity);

        /// <summary>
        /// 批量更新 
        /// </summary>
        /// <param name="planWorkOrderEntitys"></param>
        /// <returns></returns>
        Task<int> UpdatesAsync(IEnumerable<PlanWorkOrderEntity> planWorkOrderEntitys);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<int> DeleteAsync(long id);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<int> DeletesAsync(DeleteCommand param);

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<PlanWorkOrderEntity> GetByIdAsync(long id);

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<PlanWorkOrderEntity>> GetByIdsAsync(IEnumerable<long> ids);
        /// <summary>
        /// 获取此站点所有工单信息 用于缓存整个站点的工单数据 为单个查询 范围查询加速
        /// </summary>
        /// <param name="siteId"></param>
        /// <returns></returns>
        Task<IEnumerable<PlanWorkOrderEntity>> GetBySiteIdAsync(long siteId);
        /// <summary>
        /// 根据 workOrderId 获取数据
        /// </summary>
        /// <param name="workOrderId"></param>
        /// <returns></returns>
        Task<PlanWorkOrderRecordEntity> GetByWorkOrderIdAsync(long workOrderId);

        /// <summary>
        /// 根据Code获取数据
        /// </summary>
        /// <param name="workOrderCode"></param>
        /// <returns></returns>
        Task<PlanWorkOrderEntity> GetByCodeAsync(PlanWorkOrderQuery query);

        /// <summary>
        /// 根据IDs批量获取数据  含有物料信息
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<PlanWorkOrderAboutMaterialInfoView>> GetByIdsAboutMaterialInfoAsync(long[] ids);

        /// <summary>
        /// 根据计划产品ID获取工单数据
        /// </summary>
        /// <param name="planProductId"></param>
        /// <returns></returns>
        Task<IEnumerable<PlanWorkOrderEntity>> GetByPlanProductIdAsync(long planProductId);

        /// <summary>
        /// 根据车间ID获取工单数据
        /// </summary>
        /// <param name="workFarmId"></param>
        /// <returns></returns>
        Task<IEnumerable<PlanWorkOrderEntity>> GetByWorkFarmIdAsync(long workFarmId);

        /// <summary>
        /// 根据产线ID获取工单数据
        /// </summary>
        /// <param name="workLineId"></param>
        /// <returns></returns>
        Task<IEnumerable<PlanWorkOrderEntity>> GetByWorkLineIdAsync(long workLineId);

        /// <summary>
        /// 获取List   
        /// 条件具体
        /// </summary>
        /// <param name="planWorkOrderQuery"></param>
        /// <returns></returns>
        Task<IEnumerable<PlanWorkOrderEntity>> GetEqualPlanWorkOrderEntitiesAsync(PlanWorkOrderQuery planWorkOrderQuery);

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<PlanWorkOrderListDetailView>> GetPagedInfoAsync(PlanWorkOrderPagedQuery pageQuery);

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<PlanWorkOrderListDetailView>> GetPagedInfoWithPickAsync(PlanWorkOrderPagedQuery pageQuery);

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<PlanWorkOrderListDetailView>> GetPagedInfoAsyncCode(PlanWorkOrderPagedQuery pageQuery);

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<IEnumerable<PlanWorkOrderEntity>> GetEntitiesAsync(PlanWorkOrderNewQuery query);

        /// <summary>
        /// 修改工单状态
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        Task<int> ModifyWorkOrderStatusAsync(IEnumerable<UpdateStatusCommand> parms);

        /// <summary>
        /// 修改工单是否锁定
        /// </summary>
        /// <param name="parms"></param>
        /// <returns></returns>
        Task<int> ModifyWorkOrderLockedAsync(IEnumerable<UpdateLockedCommand> parms);

        /// <summary>
        /// 更新下达数量
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<int> UpdatePassDownQuantityByWorkOrderIdAsync(UpdatePassDownQuantityCommand param);

        /// <summary>
        /// 退还下达数量
        /// </summary>
        /// <param name="commands"></param>
        /// <returns></returns>
        Task<int> RefundPassDownQuantityByWorkOrderIdsAsync(IEnumerable<UpdatePassDownQuantityCommand> commands);

        /// <summary>
        /// 更新数量（投入数量）
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<int> UpdateInputQtyByWorkOrderIdAsync(UpdateQtyByWorkOrderIdCommand param);

        /// <summary>
        /// 更新数量（投入数量）
        /// </summary>
        /// <param name="commands"></param>
        /// <returns></returns>
        Task<int> UpdateInputQtyByWorkOrderIdsAsync(IEnumerable<UpdateQtyByWorkOrderIdCommand>? commands);

        /// <summary>
        /// 更新数量（完工数量）
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<int> UpdateFinishProductQuantityByWorkOrderIdAsync(UpdateQtyByWorkOrderIdCommand param);

        /// <summary>
        /// 更新数量（完工数量）
        /// </summary>
        /// <param name="commands"></param>
        /// <returns></returns>
        Task<int> UpdateFinishProductQuantityByWorkOrderIdsAsync(IEnumerable<UpdateQtyByWorkOrderIdCommand>? commands);

        /// <summary>
        /// 新增工单记录表
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<int> InsertPlanWorkOrderRecordAsync(PlanWorkOrderRecordEntity param);

        /// <summary>
        /// 更新生产订单记录的实际开始时间
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        Task<int> UpdatePlanWorkOrderRealStartByWorkOrderIdAsync(UpdateWorkOrderRealTimeCommand command);

        /// <summary>
        /// 更新生产订单记录的实际开始时间（批量）
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        Task<int> UpdatePlanWorkOrderRealStartByWorkOrderIdAsync(IEnumerable<UpdateWorkOrderRealTimeCommand> commands);

        /// <summary>
        /// 更新生产订单记录的实际结束时间
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        Task<int> UpdatePlanWorkOrderRealEndByWorkOrderIdAsync(UpdateWorkOrderRealTimeCommand command);

        /// <summary>
        /// 获取激活工单列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<IEnumerable<PlanWorkOrderView>> GetActivationWorkOrderDataAsync(PlanWorkOrderPagedQuery query);

        /// <summary>
        /// 获取工单列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<IEnumerable<PlanWorkOrderView>> GetWorkOrderDataAsync(PlanWorkOrderPagedQuery query);

        /// <summary>
        /// 修改工单计划数量
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<int> UpdateWorkOrderPlantQuantityByWorkOrderIdAsync(UpdateQtyByWorkOrderIdCommand param);

        #region 马威

        /// <summary>
        /// 根据ID批量获取数据，含有物料信息
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<PlanWorkOrderMavelView> GetByIdMavelAsync(long id);

        /// <summary>
        /// 获取工单数据
        /// </summary>
        /// <param name="siteId"></param>
        /// <returns></returns>
        Task<IEnumerable<PlanWorkOrderMaterialMavleView>> GetWorkOrderMavelAsync(long siteId);

        /// <summary>
        /// 更新工单完成数量
        /// </summary>
        /// <param name="planWorkOrderEntitys"></param>
        /// <returns></returns>
        Task<int> UpdatesCompleteQtyMavleAsync(IEnumerable<PlanWorkOrderEntity> planWorkOrderEntitys);

        #endregion
    }
}
