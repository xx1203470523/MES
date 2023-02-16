using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Integrated;
using Hymson.MES.Data.Repositories.Integrated.InteCalendar.Query;

namespace Hymson.MES.Data.Repositories.Integrated.InteCalendar
{
    /// <summary>
    /// 日历维护仓储接口
    /// </summary>
    public interface IInteCalendarRepository
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(InteCalendarEntity entity);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(InteCalendarEntity entity);

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
        /// 
        /// </summary>
        /// <param name="equOrLineId"></param>
        /// <returns></returns>
        Task<bool> IsExistsAsync(long equOrLineId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="equOrLineId"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> IsExistsAsync(long equOrLineId, long id);

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<InteCalendarEntity> GetByIdAsync(long id);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<InteCalendarView>> GetPagedListAsync(InteCalendarPagedQuery pagedQuery);

    }
}
