using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Integrated
{
    /// <summary>
    /// 日历数据实体对象
    ///
    /// @author admin
    /// @date 2023-02-08
    /// </summary>
    public class InteCalendarEntity: BaseEntity
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
        /// 描述 :启用状态 
        /// 空值 : true  
        /// </summary>
        public byte UseStatus { get; set; }
        }
}