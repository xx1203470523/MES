using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Quality;

namespace Hymson.MES.Services.Services.Quality
{
    /// <summary>
    /// 服务接口（全检参数表）
    /// </summary>
    public interface IQualInspectionParameterGroupService
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        Task<int> CreateQualInspectionParameterGroupAsync(QualInspectionParameterGroupSaveDto saveDto);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        Task<int> ModifyQualInspectionParameterGroupAsync(QualInspectionParameterGroupSaveDto saveDto);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<int> DeleteQualInspectionParameterGroupAsync(long id);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<int> DeletesQualInspectionParameterGroupAsync(long[] ids);

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<QualInspectionParameterGroupInfoDto?> QueryQualInspectionParameterGroupByIdAsync(long id);

        /// <summary>
        /// 根据ID获取项目明细列表
        /// </summary>
        /// <param name="parameterGroupId"></param>
        /// <returns></returns>
        Task<IEnumerable<QualInspectionParameterGroupDetailDto>> QueryDetailsByParameterGroupIdAsync(long parameterGroupId);

        /// <summary>
        /// 获取分页List
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<QualInspectionParameterGroupDto>> GetPagedListAsync(QualInspectionParameterGroupPagedQueryDto pagedQueryDto);

    }
}