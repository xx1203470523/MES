/*
 *creator: Karl
 *
 *describe: 物料维护仓储类 | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-02-08 04:47:44
 */
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Data.Repositories.Common.Command;

namespace Hymson.MES.Data.Repositories.Process
{
    /// <summary>
    /// 物料维护仓储接口
    /// </summary>
    public interface IProcMaterialRepository
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="procMaterialEntity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(ProcMaterialEntity procMaterialEntity);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="procMaterialEntity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(ProcMaterialEntity procMaterialEntity);

        /// <summary>
        /// 更新 同编码的其他物料设置为非当前版本
        /// </summary>
        /// <param name="procMaterialEntity"></param>
        /// <returns></returns>
        Task<int> UpdateSameMaterialCodeToNoVersionAsync(ProcMaterialEntity procMaterialEntity);

        /// <summary>
        /// 更新物料的物料组
        /// </summary>
        /// <param name="procMaterialEntitys"></param>
        /// <returns></returns>
        Task<int> UpdateProcMaterialGroupAsync(IEnumerable<ProcMaterialEntity> procMaterialEntitys);

        /// <summary>
        /// 更新某物料组下的物料为未绑定的
        /// </summary>
        /// <param name="procMaterialEntitys"></param>
        /// <returns></returns>
        Task<int> UpdateProcMaterialUnboundAsync(long groupId);

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
        /// <param name="siteCode"></param>
        /// <returns></returns>
        Task<ProcMaterialView> GetByIdAsync(long id, long SiteId);

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<ProcMaterialEntity>> GetByIdsAsync(long[] ids);

        /// <summary>
        /// 根据物料组ID查询物料
        /// </summary>
        /// <param name="groupIds"></param>
        /// <returns></returns>
        Task<IEnumerable<ProcMaterialEntity>> GetByGroupIdsAsync(long[] groupIds);

        /// <summary>
        /// 获取List
        /// </summary>
        /// <param name="procMaterialQuery"></param>
        /// <returns></returns>
        Task<IEnumerable<ProcMaterialEntity>> GetProcMaterialEntitiesAsync(ProcMaterialQuery procMaterialQuery);

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="procMaterialPagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<ProcMaterialEntity>> GetPagedInfoAsync(ProcMaterialPagedQuery procMaterialPagedQuery);

        /// <summary>
        /// 分页查询  分组
        /// </summary>
        /// <param name="procMaterialPagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<ProcMaterialEntity>> GetPagedInfoForGroupAsync(ProcMaterialPagedQuery procMaterialPagedQuery);
    }
}
