using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Integrated;

namespace Hymson.MES.Services.Services.Integrated.IIntegratedService
{
    /// <summary>
    /// 作业表服务接口
    /// @author admin
    /// @date 2023-02-21
    /// </summary>
    public interface IInteJobService
    {
        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="pram"></param>
        /// <returns></returns>
        Task<PagedInfo<InteJobDto>> GetPageListAsync(InteJobPagedQueryDto pram);

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<InteJobDto> QueryInteJobByIdAsync(long id);

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="param">新增参数</param>
        /// <returns></returns>
        Task<long> CreateInteJobAsync(InteJobCreateDto param);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<int> DeleteRangInteJobAsync(long[] ids);


        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task ModifyInteJobAsync(InteJobModifyDto param);

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="jobId"></param>
        /// <returns></returns>
        Task<IEnumerable<InteJobConfigDto>> GetConfigByJobIdAsync(long jobId);
    }
}
