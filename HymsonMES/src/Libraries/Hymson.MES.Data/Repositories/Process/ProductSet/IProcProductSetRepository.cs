/*
 *creator: Karl
 *
 *describe: 工序和资源半成品产品设置表仓储类 | 代码由框架生成
 *builder:  zhaoqing
 *build datetime: 2023-06-05 11:16:51
 */
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Process.ProductSet.Query;

namespace Hymson.MES.Data.Repositories.Process
{
    /// <summary>
    /// 工序和资源半成品产品设置表仓储接口
    /// </summary>
    public interface IProcProductSetRepository
    {
        #region 
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="procProductSetEntity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(ProcProductSetEntity procProductSetEntity);

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="procProductSetEntitys"></param>
        /// <returns></returns>
        Task<int> InsertsAsync(List<ProcProductSetEntity> procProductSetEntitys);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="procProductSetEntity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(ProcProductSetEntity procProductSetEntity);

        /// <summary>
        /// 批量更新 
        /// </summary>
        /// <param name="procProductSetEntitys"></param>
        /// <returns></returns>
        Task<int> UpdatesAsync(List<ProcProductSetEntity> procProductSetEntitys);

        /// <summary>
        /// 删除  
        /// 最好使用批量删除，可以设置更新人和更新时间
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
        Task<ProcProductSetEntity> GetByIdAsync(long id);

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<ProcProductSetEntity>> GetByIdsAsync(long[] ids);

        /// <summary>
        /// 获取List
        /// </summary>
        /// <param name="procProductSetQuery"></param>
        /// <returns></returns>
        Task<IEnumerable<ProcProductSetEntity>> GetProcProductSetEntitiesAsync(ProcProductSetQuery procProductSetQuery);

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="procProductSetPagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<ProcProductSetEntity>> GetPagedInfoAsync(ProcProductSetPagedQuery procProductSetPagedQuery);

        /// <summary>
        /// 删除（物料删除）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<int> DeleteBySetPointIdAsync(long id);
        #endregion
    }
}
