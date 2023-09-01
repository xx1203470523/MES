using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Services.Dtos.Common;
using Hymson.MES.Services.Dtos.Quality;

namespace Hymson.MES.Services.Services.Quality
{
    /// <summary>
    /// 服务接口（IPQC检验项目）
    /// </summary>
    public interface IQualIpqcInspectionService
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        Task<int> CreateAsync(QualIpqcInspectionSaveDto saveDto);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        Task<int> ModifyAsync(QualIpqcInspectionSaveDto saveDto);

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
        Task<QualIpqcInspectionDto?> QueryByIdAsync(long id);

        /// <summary>
        /// 获取分页List
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<QualIpqcInspectionViewDto>> GetPagedListAsync(QualIpqcInspectionPagedQueryDto pagedQueryDto);

        /// <summary>
        /// 根据ID获取关联明细列表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<IEnumerable<QualIpqcInspectionParameterDto>?> QueryDetailsByMainIdAsync(long id);

        /// <summary>
        /// 根据ID获取检验规则列表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<IEnumerable<QualIpqcInspectionRuleDto>?> QueryRulesByMainIdAsync(long id);

        /// <summary>
        /// 状态变更
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task UpdateStatusAsync(ChangeStatusDto param);

        /// <summary>
        /// 状态更新为启用
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="CustomerValidationException"></exception>
        Task UpdateStatusEnable(long id);
    }
}