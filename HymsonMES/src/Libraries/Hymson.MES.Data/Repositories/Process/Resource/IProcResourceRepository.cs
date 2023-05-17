using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Process.Resource;

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
        /// 查询详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ProcResourceEntity> GetResByIdAsync(long id);

        /// <summary>
        /// 查询某些资源类型下关联的资源列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<IEnumerable<ProcResourceEntity>> GetByResTypeIdsAsync(ProcResourceQuery query);

        /// <summary>
        /// 查询要删除的资源列表是否有启用状态的
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<IEnumerable<ProcResourceEntity>> GetByIdsAsync(ProcResourceQuery query);

        /// <summary>
        /// 批量查询
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<ProcResourceEntity>> GetListByIdsAsync(long[] ids);

        /// <summary>
        /// 根据资源Code查询数据
        /// </summary>
        /// <param name="resourceCode"></param>
        /// <returns></returns>
        Task<IEnumerable<ProcResourceEntity>> GetByResourceCodeAsync(ProcResourceQuery query);

        /// <summary>
        /// 根据设备Code查询数据
        /// </summary>
        /// <param name="equipmentCode"></param>
        /// <returns></returns>
        Task<IEnumerable<ProcResourceEntity>> GetByEquipmentCodeAsync(ProcResourceQuery query);

        /// <summary>
        /// 判断资源是否存在
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<bool> IsExistsAsync(ProcResourceQuery query);

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
        Task<PagedInfo<ProcResourceEntity>> GettPageListByProcedureIdAsync(ProcResourceProcedurePagedQuery query);

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
        Task<IEnumerable<ProcResourceEntity>> GetListForGroupAsync(ProcResourcePagedQuery query);

        /// <summary>
        /// 查询工序关联的资源列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<IEnumerable<ProcResourceEntity>> GetProcResourceListByProcedureIdAsync(ProcResourceListByProcedureIdQuery query);

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
        /// 更新资源类型数据,置为0
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<int> ResetResTypeAsync(ProcResourceUpdateCommand entity);

        /// <summary>
        /// 清空资源的资源类型
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        Task<int> ClearResourceTypeIdsAsync(ClearResourceTypeIdsCommand command);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        Task<int> DeleteRangeAsync(DeleteCommand command);
    }
}
