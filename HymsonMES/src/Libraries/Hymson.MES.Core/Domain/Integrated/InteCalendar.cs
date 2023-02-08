using Hymson.Infrastructure;

namespace  Hymson.MES.Core.Domain.Integrated
{
    /// <summary>
    /// 数据实体对象（日历）
    /// @author zhaoqing
    /// @date 2022-12-27
    /// </summary>
    public class InteCalendar : BaseEntity
    {
        /// <summary>
        /// 描述 :日历名称 
        /// 空值 : false  
        /// </summary>
        public string CalendarName { get; set; }

        /// <summary>
        /// 描述 :日历类型（字典名称：inte_calendar_type） 
        /// 空值 : false  
        /// </summary>
        public string CalendarType { get; set; }

        /// <summary>
        /// 描述 :设备线体类型（字典名称：inte_calendar_equline_type） 
        /// 空值 : false  
        /// </summary>
        //public string EquLineType { get; set; }

        /// <summary>
        /// 描述 :设备或者线体id 
        /// 空值 : false  
        /// </summary>
        public long EquOrLineId { get; set; }

        /// <summary>
        /// 描述 :所属站点代码 
        /// 空值 : false  
        /// </summary>
        public string SiteCode { get; set; }

        /// <summary>
        /// 启用状态
        /// </summary>
        public int UseStatus { get; set; }
    }
}
