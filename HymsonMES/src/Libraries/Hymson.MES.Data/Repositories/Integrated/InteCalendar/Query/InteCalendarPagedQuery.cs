using Hymson.Infrastructure;

namespace Hymson.MES.Data.Repositories.Integrated.InteCalendar.Query
{
    /// <summary>
    /// 日历维护 分页参数
    /// </summary>
    public class InteCalendarPagedQuery : PagerInfo
    {
        /// <summary>
        /// 站点编码 
        /// </summary>
        public long? SiteId { get; set; }

        /// <summary>
        /// 日历名称
        /// </summary>
        public string CalendarName { get; set; } = "";

        /// <summary>
        /// 名称
        /// </summary>
        public string Code { get; set; } = "";

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; } = "";

        /// <summary>
        /// 类型
        /// </summary>
        public int CalendarType { get; set; } = -1;

        /// <summary>
        /// 
        /// </summary>
        public int UseStatus { get; set; } = -1;
    }
}
