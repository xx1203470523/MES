/*
 *creator: Karl
 *
 *describe: 工序表仓储类 | 代码由框架生成
 *builder:  zhaoqing
 *build datetime: 2023-02-13 09:06:05
 */
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Common.Query;

namespace Hymson.MES.Data.Repositories.Process
{
    /// <summary>
    /// 工序表仓储接口
    /// </summary>
    public interface IProcProcedureRepository
    {
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="procProcedurePagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<ProcProcedureView>> GetPagedInfoAsync(ProcProcedurePagedQuery procProcedurePagedQuery);

        /// <summary>
        ///分页查询工艺路线的工序列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<PagedInfo<ProcProcedureEntity>> GetPagedInfoByProcessRouteIdAsync(ProcProcedurePagedQuery query);

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ProcProcedureEntity> GetByIdAsync(long id);

        /// <summary>
        /// 获取此站点所有工序信息 用于缓存整个站点的工序数据 为单个查询 范围查询加速
        /// </summary>
        /// <param name="siteId"></param>
        /// <returns></returns>
        Task<IEnumerable<ProcProcedureEntity>> GetBySiteIdAsync(long siteId);
        /// <summary>
        /// 根据Code查询对象
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<ProcProcedureEntity> GetByCodeAsync(EntityByCodeQuery query);

        /// <summary>
        /// 根据资源ID获取工序（这个方法是有问题的，因为程序没有限制一个资源可以绑定多个工序）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ProcProcedureEntity> GetProcProcedureByResourceIdAsync(ProcProdureByResourceIdQuery param);

        /// <summary>
        /// 根据资源ID获取工序
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<IEnumerable<ProcProcedureEntity>> GetProcProduresByResourceIdAsync(ProcProdureByResourceIdQuery param);

        /// <summary>
        /// 判断工序是否存在
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<bool> IsExistsAsync(ProcProcedureQuery query);

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<ProcProcedureEntity>> GetByIdsAsync(IEnumerable<long> ids);

        /// <summary>
        /// 获取List
        /// </summary>
        /// <param name="procProcedureQuery"></param>
        /// <returns></returns>
        Task<IEnumerable<ProcProcedureEntity>> GetProcProcedureEntitiesAsync(ProcProcedureQuery procProcedureQuery);

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<IEnumerable<ProcProcedureEntity>> GetEntitiesAsync(ProcProcedureQuery query);

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="procProcedureEntity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(ProcProcedureEntity procProcedureEntity);

        /// <summary>
        /// 批量插入
        /// </summary>
        /// <param name="procProcedures"></param>
        /// <returns></returns>
        Task<int> InsertsAsync(IEnumerable<ProcProcedureEntity> procProcedures);

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="procProcedures"></param>
        /// <returns></returns>
        Task<int> UpdatesAsync(IEnumerable<ProcProcedureEntity> procProcedures);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="procProcedureEntity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(ProcProcedureEntity procProcedureEntity);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        Task<int> DeleteRangeAsync(DeleteCommand command);

        /// <summary>
        /// 更新状态
        /// </summary>
        /// <param name="procMaterialEntitys"></param>
        /// <returns></returns>
        Task<int> UpdateStatusAsync(ChangeStatusCommand command);

        /// <summary>
        /// 查询工序单条数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<ProcProcedureEntity> GetEntitieAsync(ProcProcedureQuery query);

    }
}
