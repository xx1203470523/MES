using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Qual;
using Hymson.MES.Services.Dtos.Quality;

namespace Hymson.MES.Services.Services.Quality
{
    /// <summary>
    /// 服务接口（OQC检验参数组）
    /// </summary>
    public interface IQualOqcParameterGroupService
    {
        Task<QualOqcParameterGroupOutputDto> GetOneAsync(QualOqcParameterGroupQueryDto queryDto);

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="createDto"></param>
        /// <returns></returns>
        Task CreateAsync(QualOqcParameterGroupDto createDto);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="updateDto"></param>
        /// <returns></returns>
        Task ModifyAsync(QualOqcParameterGroupUpdateDto updateDto);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<int> DeleteAsync(long id);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<int> DeletesAsync(long[] ids);

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<QualOqcParameterGroupDto?> QueryByIdAsync(long id);

        /// <summary>
        /// 获取分页List
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<QualOqcParameterGroupDto>> GetPagedListAsync(QualOqcParameterGroupPagedQueryDto pagedQueryDto);

        /// <summary>
        ///分页查询-包含参数明细
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<QualOqcParameterGroupOutputDto>> GetPagedAsync(QualOqcParameterGroupPagedQueryDto queryDto);

        /// <summary>
        /// 删除OQC检验项目
        /// </summary>
        /// <param name="deleteDto"></param>
        /// <returns></returns>
        Task DeleteAsync(QualOqcParameterGroupDeleteDto deleteDto);

    }
}