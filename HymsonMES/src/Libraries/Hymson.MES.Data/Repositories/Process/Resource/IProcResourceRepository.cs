using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Process;

namespace Hymson.MES.Data.Repositories.Process
{
    public interface IProcResourceRepository
    {
        /// <summary>
        /// 查询详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ProcResourceView> GetByIdAsync(long id);

        /// <summary>
        /// 查询某些资源类型下关联的资源列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<IEnumerable<ProcResourceEntity>> GetByResTypeIdsAsync(ProcResourceQuery query);

        /// <summary>
        ///  查询资源维护表列表(关联资源类型，展示资源类型名称)
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<PagedInfo<ProcResourceView>> GetPageListAsync(ProcResourcePagedQuery query);

        /// <summary>
        /// 获取资源分页列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<PagedInfo<ProcResourceEntity>> GetListAsync(ProcResourcePagedQuery query);

        /// <summary>
        /// 查询资源维护表列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<PagedInfo<ProcResourceEntity>> GetListForGroupAsync(ProcResourcePagedQuery query);

        /// <summary>
        /// 添加资源类型数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(ProcResourceEntity entity);

        /// <summary>
        /// 更新资源类型维护数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(ProcResourceEntity entity);

        /// <summary>
        /// 更新资源类型数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<int> UpdateResTypeAsync(ProcResourceUpdateCommand entity);

        /// <summary>
        /// 更新资源类型数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<int> UpdateResTypeAsync(long resTypeId);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<int> DeleteAsync(long[] idsArr);
    }
}
