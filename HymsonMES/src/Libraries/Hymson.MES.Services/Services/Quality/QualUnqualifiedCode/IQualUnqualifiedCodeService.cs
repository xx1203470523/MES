using Hymson.Infrastructure;
using Hymson.MES.CoreServices.Dtos.Common;
using Hymson.MES.Services.Dtos.Common;
using Hymson.MES.Services.Dtos.Quality;

namespace Hymson.MES.Services.Services.Quality.QualUnqualifiedCode
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
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<QualUnqualifiedCodeDto>> GetPageListAsync(QualUnqualifiedCodePagedQueryDto pagedQueryDto);

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<QualUnqualifiedCodeDto> QueryQualUnqualifiedCodeByIdAsync(long id);

        /// <summary>
        /// 获取不合格代码组
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<List<UnqualifiedCodeGroupRelationDto>> GetQualUnqualifiedCodeGroupRelationByIdAsync(long id);

        /// <summary>
        /// 根据不合格代码组id查询不合格代码列表
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        Task<IEnumerable<QualUnqualifiedCodeDto>> GetListByGroupIdAsync(long groupId);

        /// <summary>
        /// 根据工序id查询不合格代码列表
        /// </summary>
        /// <param name="procedureId"></param>
        /// <returns></returns>
        Task<IEnumerable<QualUnqualifiedCodeDto>> GetListByProcedureIdAsync(long procedureId);

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<long> CreateQualUnqualifiedCodeAsync(QualUnqualifiedCodeCreateDto param);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task ModifyQualUnqualifiedCodeAsync(QualUnqualifiedCodeModifyDto param);

        /// <summary>
        ///删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<int> DeletesQualUnqualifiedCodeAsync(long[] ids);

        /// <summary>
        /// 查询编码
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<SelectOptionDto>> QueryCodesAsync();

        /// <summary>
        /// 状态变更
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task UpdateStatusAsync(ChangeStatusDto param);

    }
}
