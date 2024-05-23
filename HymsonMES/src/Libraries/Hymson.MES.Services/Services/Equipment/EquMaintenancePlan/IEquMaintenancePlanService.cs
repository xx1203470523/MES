/*
 *creator: Karl
 *
 *describe: 设备点检计划    服务接口 | 代码由框架生成
 *builder:  pengxin
 *build datetime: 2024-05-16 02:14:30
 */
using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.EquMaintenancePlan;

namespace Hymson.MES.Services.Services.EquMaintenancePlan
{
    /// <summary>
    /// 设备点检计划 service接口
    /// </summary>
    public interface IEquMaintenancePlanService
    {
        /// <summary>
        /// 获取分页List
        /// </summary>
        /// <param name="EquMaintenancePlanPagedQueryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<EquMaintenancePlanDto>> GetPagedListAsync(EquMaintenancePlanPagedQueryDto EquMaintenancePlanPagedQueryDto);

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="EquMaintenancePlanCreateDto"></param>
        /// <returns></returns>
        Task CreateEquMaintenancePlanAsync(EquMaintenancePlanCreateDto EquMaintenancePlanCreateDto);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="EquMaintenancePlanModifyDto"></param>
        /// <returns></returns>
        Task ModifyEquMaintenancePlanAsync(EquMaintenancePlanModifyDto EquMaintenancePlanModifyDto);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteEquMaintenancePlanAsync(long id);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<int> DeletesEquMaintenancePlanAsync(DeletesDto param);

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<EquMaintenancePlanDto> QueryEquMaintenancePlanByIdAsync(long id);


        /// <summary>
        /// 生成(Core)
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task GenerateEquMaintenanceTaskCoreAsync(GenerateDto param);


        /// <summary>
        /// 生成
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task GenerateEquMaintenanceTaskAsync(GenerateDto param);

        #region 关联信息

        /// <summary>
        /// 获取模板关联信息（项目）
        /// </summary>
        /// <param name="MaintenancePlanId"></param>
        /// <returns></returns>
        Task<List<QueryEquRelationListDto>> QueryEquRelationListAsync(long MaintenancePlanId);

        #endregion

    }
}
