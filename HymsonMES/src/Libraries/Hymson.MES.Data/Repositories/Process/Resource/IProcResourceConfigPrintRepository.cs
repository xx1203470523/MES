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
        /// 新增
        /// </summary>
        /// <param name="procResourceConfigPrintEntity"></param>
        /// <returns></returns>
        Task InsertAsync(ProcResourceConfigPrintEntity procResourceConfigPrintEntity);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="procResourceConfigPrintEntity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(ProcResourceConfigPrintEntity procResourceConfigPrintEntity);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<int> DeleteAsync(long id);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="idsArr"></param>
        /// <returns></returns>
        Task<int> DeletesAsync(long[] idsArr);

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ProcResourceConfigPrintEntity> GetByIdAsync(long id);

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<PagedInfo<ProcResourceConfigPrintView>> GetPagedInfoAsync(ProcResourceConfigPrintPagedQuery query);
    }
}
