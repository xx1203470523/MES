using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Services.Dtos.Common;
using Hymson.MES.Services.Dtos.Process;

namespace Hymson.MES.Services.Services.Process
{
    /// <summary>
    /// 服务接口（产品检验参数组）
    /// </summary>
    public interface IProcProductParameterGroupService
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        Task<int> CreateAsync(ProcProductParameterGroupSaveDto saveDto);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        Task<int> ModifyAsync(ProcProductParameterGroupSaveDto saveDto);

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
        Task<ProcProductParameterGroupInfoDto?> QueryByIdAsync(long id);

        /// <summary>
        /// 根据ID获取关联明细列表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<IEnumerable<ProcProductParameterGroupDetailDto>> QueryDetailsByMainIdAsync(long id);

        /// <summary>
        /// 获取分页List
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<ProcProductParameterGroupDto>> GetPagedListAsync(ProcProductParameterGroupPagedQueryDto pagedQueryDto);

        /// <summary>
        /// 状态变更
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task UpdateStatusAsync(ChangeStatusDto param);

        /// <summary>
        /// 根据条码与工序查询当前版本的产品参数收集详情
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        /// <exception cref="CustomerValidationException"></exception>
        Task<IEnumerable<ProcProductParameterGroupDetailDto>> GetBySfcAndProcedureIdAsync(ProcProductParameterGroupBySfcAndProcedureIdQueryDto queryDto);
    }
}