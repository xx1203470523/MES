/*
 *creator: Karl
 *
 *describe: 工单激活    服务接口 | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-03-29 10:23:51
 */
using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Manufacture;
using Hymson.MES.Services.Dtos.Plan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Services.Plan
{
    /// <summary>
    /// 工单激活 service接口
    /// </summary>
    public interface IPlanWorkOrderActivationService
    {
        /// <summary>
        /// 获取分页List
        /// </summary>
        /// <param name="planWorkOrderActivationPagedQueryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<PlanWorkOrderActivationListDetailViewDto>> GetPageListAsync(PlanWorkOrderActivationPagedQueryDto planWorkOrderActivationPagedQueryDto);

        /// <summary>
        /// 根据查询条件获取分页数据--(根据资源先找到线体)
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<PagedInfo<PlanWorkOrderActivationListDetailViewDto>> GetPageListAboutResAsync(PlanWorkOrderActivationAboutResPagedQueryDto param);

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="planWorkOrderActivationCreateDto"></param>
        /// <returns></returns>
        Task CreatePlanWorkOrderActivationAsync(PlanWorkOrderActivationCreateDto planWorkOrderActivationCreateDto);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="planWorkOrderActivationModifyDto"></param>
        /// <returns></returns>
        Task ModifyPlanWorkOrderActivationAsync(PlanWorkOrderActivationModifyDto planWorkOrderActivationModifyDto);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeletePlanWorkOrderActivationAsync(long id);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="idsArr"></param>
        /// <returns></returns>
        Task<int> DeletesPlanWorkOrderActivationAsync(long[] idsArr);

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<PlanWorkOrderActivationDto> QueryPlanWorkOrderActivationByIdAsync(long id);

        /// <summary>
        /// 激活/取消激活 工单
        /// </summary>
        /// <param name="activationWorkOrderDto"></param>
        /// <returns></returns>
        Task ActivationWorkOrderAsync(ActivationWorkOrderDto activationWorkOrderDto);

        /// <summary>
        /// 【PDA】设备编码扫描
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        Task<EquipmentCodeScanOutputDto> EquipmentCodeScanAsync(string code);
        
        /// <summary>
        /// 获取未激活工单
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<IEnumerable<EquipmentActivityWorkOrderOutputDto>> GetNotActivityWorkOrderAsync(ActivationWorkOrderPagedQueryDto query);

        /// <summary>
        /// 获取已激活工单
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<IEnumerable<EquipmentActivityWorkOrderOutputDto>> GetActivityWorkOrderAsync(ActivationWorkOrderPagedQueryDto query);
    }
}
