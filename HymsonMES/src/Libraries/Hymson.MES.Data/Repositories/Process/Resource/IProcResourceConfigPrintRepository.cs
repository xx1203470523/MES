/*
 *creator: Karl
 *
 *describe: 资源配置打印机仓储类 | 代码由框架生成
 *builder:  zhaoqing
 *build datetime: 2023-02-09 04:14:52
 */
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Data.Repositories.Process.Resource;

namespace Hymson.MES.Data.Repositories.Process
{
    /// <summary>
    /// 资源配置打印机仓储接口
    /// </summary>
    public interface IProcResourceConfigPrintRepository
    {
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<PagedInfo<ProcResourceConfigPrintView>> GetPagedInfoAsync(ProcResourceConfigPrintPagedQuery query);

        /// <summary>
        /// 根据资源id和打印机Id查询数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<IEnumerable<ProcResourceConfigPrintEntity>> GetByResourceIdAsync(long resourceId);
        /// <summary>
        /// 根据打印机Id查询数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<IEnumerable<ProcResourceConfigPrintEntity>> GetByPrintIdAsync(ProcResourceConfigPrintQuery query);

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="procResourceConfigPrints"></param>
        /// <returns></returns>
        Task InsertRangeAsync(IEnumerable<ProcResourceConfigPrintEntity> procResourceConfigPrints);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="procResourceConfigPrints"></param>
        /// <returns></returns>
        Task<int> UpdateRangeAsync(List<ProcResourceConfigPrintEntity> procResourceConfigPrints);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="idsArr"></param>
        /// <returns></returns>
        Task<int> DeleteRangeAsync(long[] idsArr);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<int> DeleteByResourceIdAsync(long id);
    }
}
