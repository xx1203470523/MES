using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Data.Repositories.Common.Command;

namespace Hymson.MES.Data.Repositories.Manufacture
{
    /// <summary>
    /// 仓储接口（条码工序生产汇总表）
    /// </summary>
    public interface IManuSfcSummaryRepository
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(ManuSfcSummaryEntity entity);

        /// <summary>
        /// 新增（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task<int> InsertRangeAsync(IEnumerable<ManuSfcSummaryEntity> entities);

        /// <summary>
        /// 合并存在更新 不存在则新增
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task<int> MergeRangeAsync(IEnumerable<ManuSfcSummaryEntity> entities);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(ManuSfcSummaryEntity entity);

        /// <summary>
        /// 更新（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task<int> UpdateRangeAsync(IEnumerable<ManuSfcSummaryEntity> entities);

        /// <summary>
        /// 出站更新汇总表
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task<int> UpdateSummaryOutStationRangeAsync(IEnumerable<UpdateOutputQtySummaryCommand>? multiUpdateSummaryOutStationCommands);

        /// <summary>
        /// 不合格产出更新汇总表
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task<int> UpdateSummaryUnqualifiedRangeAsync(IEnumerable<MultiUpdateSummaryUnqualifiedCommand> multiUpdateSummaryUnqualifiedCommand);

        /// <summary>
        /// 复判不合格
        /// </summary>
        /// <param name="commands"></param>
        /// <returns></returns>
        Task<int> UpdateSummaryReJudgmentUnqualifiedRangeAsync(IEnumerable<MultiUpdateSummaryReJudgmentUnqualifiedCommand> commands);

        /// <summary>
        /// 复判合格
        /// </summary>
        /// <param name="commands"></param>
        /// <returns></returns>
        Task<int> MultiUpdateSummaryReJudgmentQualifiedRangeAsync(IEnumerable<MultiUpdateSummaryReJudgmentQualifiedCommand> commands);

        /// <summary>
        /// 软删除  
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<int> DeleteAsync(long id);

        /// <summary>
        /// 软删除（批量）
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        Task<int> DeletesAsync(DeleteCommand command);

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ManuSfcSummaryEntity> GetByIdAsync(long id);

        /// <summary>
        /// 根据IDs获取数据（批量）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuSfcSummaryEntity>> GetByIdsAsync(long[] ids);

        /// <summary>
        /// 获取最大数据的
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuSfcSummaryEntity>> GetyLastListBySfsAsync(LastManuSfcSummaryBySfcsQuery query);


        /// <summary>
        /// 获取条码最后数据
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuSfcSummaryEntity>> GetyLastListByProcedureIdsAndSfcsAsync(LastManuSfcSummaryByProcedureIdAndSfcsQuery query);

        /// <summary>
        /// 获取List
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuSfcSummaryEntity>> GetEntitiesAsync(ManuSfcSummaryQuery query);

        /// <summary>
        /// 按工单查询summary
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuSfcSummaryEntity>> GetWorkOrderAsync(ManuSfcProduceVehiclePagedQuery query);

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<ManuSfcSummaryEntity>> GetPagedListAsync(ManuSfcSummaryPagedQuery pagedQuery);

    }
}
