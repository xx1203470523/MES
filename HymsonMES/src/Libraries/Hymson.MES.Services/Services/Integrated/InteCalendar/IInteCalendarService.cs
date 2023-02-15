using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Integrated;

namespace Hymson.MES.Services.Services.Integrated.InteCalendar
{
    /// <summary>
    /// 日历维护 service接口
    /// </summary>
    public interface IInteCalendarService
    {
        /// <summary>
        /// 添加（日历）
        /// </summary>
        /// <param name="createDto"></param>
        /// <returns></returns>
        Task<int> CreateAsync(InteCalendarCreateDto createDto);

        /// <summary>
        /// 更新（日历）
        /// </summary>
        /// <param name="modifyDto"></param>
        /// <returns></returns>
        Task<int> ModifyAsync(InteCalendarModifyDto modifyDto);

        /// <summary>
        /// 删除（日历）
        /// </summary>
        /// <param name="idsArr"></param>
        /// <returns></returns>
        Task<int> DeletesAsync(long[] idsArr);
        
        /// <summary>
        /// 查询列表（日历）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        Task<PagedInfo<InteCalendarDto>> GetPagedListAsync(InteCalendarPagedQueryDto parm);

        /// <summary>
        /// 查询详情（日历）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<InteCalendarDetailDto> GetDetailAsync(long id);

    }
}
