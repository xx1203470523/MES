using Hymson.MES.Core.Domain.Integrated;

namespace Hymson.MES.Data.Repositories.Integrated.InteCalendar
{
    /// <summary>
    /// 日历维护仓储接口
    /// </summary>
    public interface IInteCalendarDateDetailRepository
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task InsertAsync(InteCalendarDateDetailEntity entity);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<int> InsertRangeAsync(IEnumerable<InteCalendarDateDetailEntity> entitys);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(InteCalendarDateDetailEntity entity);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="calendarId"></param>
        /// <returns></returns>
        Task<int> DeleteByCalendarIdAsync(long calendarId);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="idsArr"></param>
        /// <returns></returns>
        Task<int> DeleteByCalendarIdsAsync(long[] idsArr);

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<InteCalendarDateDetailEntity> GetByIdAsync(long id);

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="calendarId"></param>
        /// <returns></returns>
        Task<IEnumerable<InteCalendarDateDetailEntity>> GetEntitiesAsync(long calendarId);

        /*
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="inteCalendarPagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<InteCalendarDateDetailEntity>> GetPagedInfoAsync(InteCalendarPagedQuery inteCalendarPagedQuery);
        */

    }
}
