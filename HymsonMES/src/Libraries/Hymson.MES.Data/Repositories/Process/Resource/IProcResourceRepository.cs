using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.MES.Data.Repositories.Process.Resource;

namespace Hymson.MES.Data.Repositories.Process
{
    /// <summary>
    /// 
    /// </summary>
    public interface IProcResourceRepository
    {
        /// <summary>
        /// 查询详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ProcResourceView> GetByIdAsync(long id);

        /// <summary>
        /// 根据Code查询对象
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<ProcResourceEntity> GetByCodeAsync(EntityByCodeQuery query);

        /// <summary>
        /// 查询详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ProcResourceEntity> GetResByIdAsync(long id);

        /// <summary>
        /// 查询详情
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<ProcResourceEntity>> GetResByIdsAsync(IEnumerable<long> ids);

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
        Task<IEnumerable<ProcResourceEntity>> GetByIdsAndStatusAsync(ProcResourceQuery query);

        /// <summary>
        /// 批量查询
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<ProcResourceEntity>> GetListByIdsAsync(IEnumerable<long> ids);

        /// <summary>
        /// 根据资源Code查询资源数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ProcResourceEntity> GetResourceByResourceCodeAsync(ProcResourceQuery query);

        /// <summary>
        /// 根据资源Code查询数据
        /// </summary>
        /// <param name="resourceCode"></param>
        /// <returns></returns>
        Task<ProcResourceEntity> GetByResourceCodeAsync(ProcResourceQuery query);

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
        /// 更具线体和工序查询资源
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<PagedInfo<ProcResourceView>> GetPageListBylineIdAndProcProcedureIdAsync(ProcResourcePagedlineIdAndProcProcedureIdQuery query);

        /// <summary>
        /// 获取资源分页列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<PagedInfo<ProcResourceView>> GettPageListByProcedureIdAsync(ProcResourceProcedurePagedQuery query);

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
        /// <param name="procedureId"></param>
        /// <returns></returns>
        Task<IEnumerable<ProcResourceEntity>> GetProcResourceListByProcedureIdAsync(long procedureId);

        /// <summary>
        /// 添加资源类型数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(ProcResourceEntity entity);

        /// <summary>
        /// 批量插入
        /// </summary>
        /// <param name="resourceEntities"></param>
        /// <returns></returns>
        Task<int> InsertsAsync(IEnumerable<ProcResourceEntity> resourceEntities);

        /// <summary>
        /// 更新资源类型维护数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(ProcResourceEntity entity);

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="procResourceEntities"></param>
        /// <returns></returns>
        Task<int> UpdatesAsync(IEnumerable<ProcResourceEntity> procResourceEntities);

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

        /// <summary>
        /// 根据设备Id查询关联的资源数据
        /// </summary>
        /// <param name="equipmentCode"></param>
        /// <returns></returns>
        Task<IEnumerable<ProcResourceEntity>> GetByEquipmentIdsAsync(ProcResourceByEquipmentIdsQuery query);

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="procResourceQuery"></param>
        /// <returns></returns>
        Task<IEnumerable<ProcResourceEntity>> GetProcResouceEntitiesAsync(ProcResourceQuery procResourceQuery);

        /// <summary>
        /// 更新状态
        /// </summary>
        /// <param name="procMaterialEntitys"></param>
        /// <returns></returns>
        Task<int> UpdateStatusAsync(ChangeStatusCommand command);

        /// <summary>
        /// 根据条件查询
        /// </summary>
        /// <param name="procResourceQuery"></param>
        /// <returns></returns>
        Task<IEnumerable<ProcResourceEntity>> GetEntitiesAsync(ProcResourceQuery procResourceQuery);
        
        Task<IEnumerable<ProcResourceEntity>> GetBySiteIdAsync(long siteId);
    }
}
