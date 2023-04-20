/*
 *creator: Karl
 *
 *describe: 工单信息表    服务接口 | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-03-20 10:07:17
 */
using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Plan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Services.Plan
{
    /// <summary>
    /// 工单信息表 service接口
    /// </summary>
    public interface IPlanWorkOrderService
    {
        /// <summary>
        /// 获取分页List
        /// </summary>
        /// <param name="planWorkOrderPagedQueryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<PlanWorkOrderListDetailViewDto>> GetPageListAsync(PlanWorkOrderPagedQueryDto planWorkOrderPagedQueryDto);

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="planWorkOrderDto"></param>
        /// <returns></returns>
        Task CreatePlanWorkOrderAsync(PlanWorkOrderCreateDto planWorkOrderCreateDto);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="planWorkOrderDto"></param>
        /// <returns></returns>
        Task ModifyPlanWorkOrderAsync(PlanWorkOrderModifyDto planWorkOrderModifyDto);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeletePlanWorkOrderAsync(long id);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="idsArr"></param>
        /// <returns></returns>
        Task<int> DeletesPlanWorkOrderAsync(long[] idsArr);

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<PlanWorkOrderDetailViewDto> QueryPlanWorkOrderByIdAsync(long id);

        /// <summary>
        /// 查询剩余可下单条码数量
        /// </summary>
        /// <param name="workOrderCode"></param>
        /// <returns></returns>
        Task<int> GetPlanWorkOrderByWorkOrderCodeAsync(string workOrderCode);

        /// <summary>
        /// 修改工单状态
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        Task ModifyWorkOrderStatusAsync(List<PlanWorkOrderChangeStatusDto> parms);

        /// <summary>
        /// 修改工单是否锁定
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        Task ModifyWorkOrderLockedAsync(List<PlanWorkOrderLockedDto> parms);

    }
}
