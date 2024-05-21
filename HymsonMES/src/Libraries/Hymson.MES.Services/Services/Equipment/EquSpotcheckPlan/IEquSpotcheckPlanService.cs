/*
 *creator: Karl
 *
 *describe: 设备点检计划    服务接口 | 代码由框架生成
 *builder:  pengxin
 *build datetime: 2024-05-16 02:14:30
 */
using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.EquSpotcheckPlan;

namespace Hymson.MES.Services.Services.EquSpotcheckPlan
{
    /// <summary>
    /// 设备点检计划 service接口
    /// </summary>
    public interface IEquSpotcheckPlanService
    {
        /// <summary>
        /// 获取分页List
        /// </summary>
        /// <param name="equSpotcheckPlanPagedQueryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<EquSpotcheckPlanDto>> GetPagedListAsync(EquSpotcheckPlanPagedQueryDto equSpotcheckPlanPagedQueryDto);

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="equSpotcheckPlanCreateDto"></param>
        /// <returns></returns>
        Task CreateEquSpotcheckPlanAsync(EquSpotcheckPlanCreateDto equSpotcheckPlanCreateDto);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="equSpotcheckPlanModifyDto"></param>
        /// <returns></returns>
        Task ModifyEquSpotcheckPlanAsync(EquSpotcheckPlanModifyDto equSpotcheckPlanModifyDto);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteEquSpotcheckPlanAsync(long id);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<int> DeletesEquSpotcheckPlanAsync(long[] ids);

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<EquSpotcheckPlanDto> QueryEquSpotcheckPlanByIdAsync(long id);


        #region 关联信息

        /// <summary>
        /// 获取模板关联信息（项目）
        /// </summary>
        /// <param name="spotCheckPlanId"></param>
        /// <returns></returns>
        Task<List<QueryEquRelationListDto>> QueryEquRelationListAsync(long spotCheckPlanId);

        #endregion

    }
}
