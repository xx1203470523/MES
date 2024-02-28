using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Data.Repositories.Common.Command;

namespace Hymson.MES.Data.Repositories.Process
{
    /// <summary>
    /// 物料组维护表仓储接口
    /// </summary>
    public interface IProcMaterialGroupRepository
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="procMaterialGroupEntity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(ProcMaterialGroupEntity procMaterialGroupEntity);
        
        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="procMaterialGroupEntitys"></param>
        /// <returns></returns>
        Task<int> InsertsAsync(List<ProcMaterialGroupEntity> procMaterialGroupEntitys);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="procMaterialGroupEntity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(ProcMaterialGroupEntity procMaterialGroupEntity);
        
        /// <summary>
        /// 批量更新 
        /// </summary>
        /// <param name="procMaterialGroupEntitys"></param>
        /// <returns></returns>
        Task<int> UpdatesAsync(List<ProcMaterialGroupEntity> procMaterialGroupEntitys);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<int> DeleteAsync(long id);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<int> DeletesAsync(DeleteCommand param);

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ProcMaterialGroupEntity> GetByIdAsync(long id);

        /// <summary>
        /// 根据ID和站点获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <param name="siteId"></param>
        /// <returns></returns>
        Task<ProcMaterialGroupEntity> GetByIdAndSiteIdAsync(long id, long SiteId);

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<ProcMaterialGroupEntity>> GetByIdsAsync(long[] ids);

        /// <summary>
        /// 获取List
        /// </summary>
        /// <param name="procMaterialGroupQuery"></param>
        /// <returns></returns>
        Task<IEnumerable<ProcMaterialGroupEntity>> GetProcMaterialGroupEntitiesAsync(ProcMaterialGroupQuery procMaterialGroupQuery);
        
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="procMaterialGroupPagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<ProcMaterialGroupEntity>> GetPagedInfoAsync(ProcMaterialGroupPagedQuery procMaterialGroupPagedQuery);

        /// <summary>
        /// 分页查询 自定义
        /// </summary>
        /// <param name="pagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<CustomProcMaterialGroupView>> GetPagedCustomInfoAsync(ProcMaterialGroupCustomPagedQuery pagedQuery);
    }
}
