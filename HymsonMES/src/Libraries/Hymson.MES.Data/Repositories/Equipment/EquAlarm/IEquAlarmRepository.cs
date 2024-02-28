using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Equipment;
using Hymson.MES.Data.Repositories.Equipment.EquAlarm.View;

namespace Hymson.MES.Data.Repositories.Equipment
{
    /// <summary>
    /// 设备报警信息仓储接口
    /// </summary>
    public interface IEquAlarmRepository
    {
        #region 查询

        /// <summary>
        /// 单条数据查询
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<EquAlarmEntity> GetOneAsync(EquAlarmQuery query);

        /// <summary>
        /// 数据集查询
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<IEnumerable<EquAlarmEntity>> GetListAsync(EquAlarmQuery query);

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<PagedInfo<EquAlarmEntity>> GetPagedInfoAsync(EquAlarmPagedQuery query);

        #endregion

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(EquAlarmEntity entity);

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task<int> InsertsAsync(IEnumerable<EquAlarmEntity> entities);

        /// <summary>
        /// 分页查询设备报警信息
        /// </summary>
        /// <param name="pageQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<EquAlarmReportView>> GetEquAlarmReportPageListAsync(EquAlarmReportPagedQuery pageQuery);

    }
}
