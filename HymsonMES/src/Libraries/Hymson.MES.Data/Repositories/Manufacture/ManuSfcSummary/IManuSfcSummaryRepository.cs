/*
 *creator: Karl
 *
 *describe: 生产汇总表仓储类 | 代码由框架生成
 *builder:  chenjianxiong
 *build datetime: 2023-06-15 10:37:18
 */
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Data.Repositories.Common.Command;

namespace Hymson.MES.Data.Repositories.Manufacture
{
    /// <summary>
    /// 生产汇总表仓储接口
    /// </summary>
    public interface IManuSfcSummaryRepository
    {
        #region 查询

        /// <summary>
        /// 单条数据查询
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<ManuSfcSummaryEntity> GetOneAsync(ManuSfcSummaryQuery query);

        /// <summary>
        /// 数据集查询
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuSfcSummaryEntity>> GetListAsync(ManuSfcSummaryQuery query);

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<PagedInfo<ManuSfcSummaryEntity>> GetPagedInfoAsync(ManuSfcSummaryPagedQuery query);

        #endregion


        #region 
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="manuSfcSummaryEntity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(ManuSfcSummaryEntity manuSfcSummaryEntity);

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="manuSfcSummaryEntitys"></param>
        /// <returns></returns>
        Task<int> InsertsAsync(List<ManuSfcSummaryEntity> manuSfcSummaryEntitys);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="manuSfcSummaryEntity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(ManuSfcSummaryEntity manuSfcSummaryEntity);

        /// <summary>
        /// 批量更新 
        /// </summary>
        /// <param name="manuSfcSummaryEntitys"></param>
        /// <returns></returns>
        Task<int> UpdatesAsync(List<ManuSfcSummaryEntity> manuSfcSummaryEntitys);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="manuSfcSummaryEntity"></param>
        /// <returns></returns>
        Task<int> UpdateNGAsync(ManuSfcSummaryQueryDto manuSfcSummaryEntity);

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
        Task<ManuSfcSummaryEntity> GetByIdAsync(long id);

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuSfcSummaryEntity>> GetByIdsAsync(long[] ids);

        /// <summary>
        /// 获取List
        /// </summary>
        /// <param name="manuSfcSummaryQuery"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuSfcSummaryEntity>> GetManuSfcSummaryEntitiesAsync(ManuSfcSummaryQuery manuSfcSummaryQuery);

        /// <summary>
        /// 存在更新，不存在新增
        /// </summary>
        /// <param name="manuSfcSummaryEntitys"></param>
        /// <returns></returns>
        Task<int> InsertOrUpdateRangeAsync(List<ManuSfcSummaryEntity> manuSfcSummaryEntitys);
        #endregion
    }
}
