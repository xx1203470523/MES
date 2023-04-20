/*
 *creator: Karl
 *
 *describe: 工单信息表仓储类 | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-03-20 10:07:17
 */
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Plan;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Plan.PlanWorkOrder.Command;
using Hymson.MES.Data.Repositories.Plan.PlanWorkOrder.Query;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        Task<IEnumerable<PlanWorkOrderEntity>> GetByIdsAsync(long[] ids);

        /// <summary>
        /// 根据Code获取数据
        /// </summary>
        /// <param name="workOrderCode"></param>
        /// <returns></returns>
        Task<PlanWorkOrderEntity> GetByCodeAsync(string workOrderCode);

        /// <summary>
        /// 根据IDs批量获取数据  含有物料信息
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<PlanWorkOrderAboutMaterialInfoView>> GetByIdsAboutMaterialInfoAsync(long[] ids);

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
        /// 条件模糊
        /// </summary>
        /// <param name="planWorkOrderQuery"></param>
        /// <returns></returns>
        Task<IEnumerable<PlanWorkOrderEntity>> GetPlanWorkOrderEntitiesAsync(PlanWorkOrderQuery planWorkOrderQuery);

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
        /// <param name="planWorkOrderPagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<PlanWorkOrderListDetailView>> GetPagedInfoAsync(PlanWorkOrderPagedQuery planWorkOrderPagedQuery);

        /// <summary>
        /// 修改工单状态
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        Task<int> ModifyWorkOrderStatusAsync(IEnumerable<PlanWorkOrderEntity> parms);

        /// <summary>
        /// 修改工单是否锁定
        /// </summary>
        /// <param name="parms"></param>
        /// <returns></returns>
        Task<int> ModifyWorkOrderLockedAsync(IEnumerable<PlanWorkOrderEntity> parms);

        /// <summary>
        /// 更新下达数量
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<int> UpdatePassDownQuantityByWorkOrderId(UpdatePassDownQuantityCommand param);

        /// <summary>
        /// 新增工单记录表
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<int> InsertPlanWorkOrderRecordAsync(PlanWorkOrderRecordEntity param);
    }
}
