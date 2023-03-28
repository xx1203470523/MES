using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Quality;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Services.Services.Quality.IQualityService
{
    /// <summary>
    /// 不合格代码服务接口
    /// @author wangkeming
    /// @date 2023-02-11 04:45:25
    /// </summary>
    public interface IQualUnqualifiedGroupService
    {
        /// <summary>
        /// 获取分页List
        /// </summary>
        /// <param name="qualUnqualifiedGroupPagedQueryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<QualUnqualifiedGroupDto>> GetPageListAsync(QualUnqualifiedGroupPagedQueryDto qualUnqualifiedGroupPagedQueryDto);

        /// <summary>
        /// 查询工序下的不合格组列表
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        Task<IEnumerable<QualUnqualifiedGroupDto>> GetListByProcedureIdAsync([FromQuery] QualUnqualifiedGroupQueryDto queryDto);

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="qualUnqualifiedGroupCreateDto"></param>
        /// <returns></returns>
        Task CreateQualUnqualifiedGroupAsync(QualUnqualifiedGroupCreateDto qualUnqualifiedGroupCreateDto);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="qualUnqualifiedGroupModifyDto"></param>
        /// <returns></returns>
        Task ModifyQualUnqualifiedGroupAsync(QualUnqualifiedGroupModifyDto qualUnqualifiedGroupModifyDto);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<int> DeletesQualUnqualifiedGroupAsync(long[] ids);

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<QualUnqualifiedGroupDto> QueryQualUnqualifiedGroupByIdAsync(long id);

        /// <summary>
        /// 获取不合格组中不合格代码
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<List<QualUnqualifiedGroupCodeRelationDto>> GetQualUnqualifiedCodeGroupRelationByIdAsync(long id);

        /// <summary>
        /// 获取不合格组中工序
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<List<QualUnqualifiedGroupProcedureRelationDto>> GetQualUnqualifiedCodeProcedureRelationByIdAsync(long id);
    }
}
