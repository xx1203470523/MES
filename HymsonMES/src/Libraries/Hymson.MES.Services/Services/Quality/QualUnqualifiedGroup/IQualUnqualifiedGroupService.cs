using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Quality;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Services.Services.Quality.QualUnqualifiedGroup
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
        /// <param name="param"></param>
        /// <returns></returns>
        Task<PagedInfo<QualUnqualifiedGroupDto>> GetPageListAsync(QualUnqualifiedGroupPagedQueryDto param);

        /// <summary>
        /// 查询工序下的不合格组列表
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        Task<IEnumerable<QualUnqualifiedGroupDto>> GetListByProcedureIdAsync([FromQuery] QualUnqualifiedGroupQueryDto queryDto);

        /// <summary>
        /// 查询物料组关联的不合格组列表
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        Task<IEnumerable<QualUnqualifiedGroupDto>> GetListByMaterialGroupIddAsync([FromQuery] QualUnqualifiedGroupQueryDto queryDto);

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<long> CreateQualUnqualifiedGroupAsync(QualUnqualifiedGroupCreateDto param);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task ModifyQualUnqualifiedGroupAsync(QualUnqualifiedGroupModifyDto param);

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
