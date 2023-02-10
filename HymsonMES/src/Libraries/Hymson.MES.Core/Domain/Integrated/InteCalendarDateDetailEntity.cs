using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Integrated
{
    /// <summary>
    /// 日历时间详情数据实体对象
    /// @author admin
    /// @date 2023-02-08
    /// </summary>
    public class InteCalendarDateDetailEntity: BaseEntity
    {
        /// <summary>
        /// 描述 :日历表id 
        /// 空值 : false  
        /// </summary>
        public long CalendarId { get; set; }
        
        /// <summary>
        /// 描述 :年月日 
        /// 空值 : false  
        /// </summary>
        public DateTime? Day { get; set; }
        
        /// <summary>
        /// 描述 :生产班制id 
        /// 空值 : false  
        /// </summary>
        public long ClassId { get; set; }
        
        /// <summary>
        /// 描述 :作息类型（字典名称：inte_calendar_type） 
        /// 空值 : true  
        /// </summary>
        public int? RestType { get; set; }
        
        /// <summary>
        /// 描述 :描述 
        /// 空值 : true  
        /// </summary>
        public string Remark { get; set; }
        
        /// <summary>
        /// 描述 :所属站点代码 
        /// 空值 : false  
        /// </summary>
        public string SiteCode { get; set; }
        }
}