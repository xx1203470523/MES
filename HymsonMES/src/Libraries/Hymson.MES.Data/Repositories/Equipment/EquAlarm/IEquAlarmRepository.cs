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
