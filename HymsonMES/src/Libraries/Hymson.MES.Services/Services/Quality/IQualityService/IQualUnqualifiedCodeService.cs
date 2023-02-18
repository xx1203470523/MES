using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Quality;

namespace Hymson.MES.Services.Services.Quality.IQualityService
{
    /// <summary>
    /// 不合格代码服务 service接口
    /// @author wangkeming
    /// @date 2023-02-11 04:45:25
    /// </summary>
    public interface IQualUnqualifiedCodeService
    {
        /// <summary>
        /// 获取分页List
        /// </summary>
        /// <param name="qualUnqualifiedCodePagedQueryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<QualUnqualifiedCodeDto>> GetPageListAsync(QualUnqualifiedCodePagedQueryDto qualUnqualifiedCodePagedQueryDto);

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<QualUnqualifiedCodeDto> QueryQualUnqualifiedCodeByIdAsync(long id);

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="qualUnqualifiedCodeCreateDto"></param>
        /// <returns></returns>
        Task CreateQualUnqualifiedCodeAsync(QualUnqualifiedCodeCreateDto qualUnqualifiedCodeCreateDto);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="qualUnqualifiedCodeModifyDto"></param>
        /// <returns></returns>
        Task ModifyQualUnqualifiedCodeAsync(QualUnqualifiedCodeModifyDto qualUnqualifiedCodeModifyDto);

        /// <summary>
        ///删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<int> DeletesQualUnqualifiedCodeAsync(string ids);
    }
}
