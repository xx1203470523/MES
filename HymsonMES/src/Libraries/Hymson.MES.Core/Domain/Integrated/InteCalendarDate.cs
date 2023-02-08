using Hymson.Infrastructure;

namespace  Hymson.MES.Core.Domain.Integrated
{
    /// 日历关联时间，数据实体对象
    /// @author zhaoqing
    /// @date 2022-12-27
    /// </summary>
    public class InteCalendarDate : BaseEntity
    {
        /// <summary>
        /// 描述 :日历表id 
        /// 空值 : false  
        /// </summary>
        public long CalendarId { get; set; }

        /// <summary>
        /// 描述 :年份 
        /// 空值 : false  
        /// </summary>
        public string Year { get; set; }

        /// <summary>
        /// 描述 :月份 
        /// 空值 : false  
        /// </summary>
        public string Month { get; set; }

        /// <summary>
        /// 描述 :生产班次表Id 
        /// 空值 : false  
        /// </summary>
        public long? ClassId { get; set; }

        /// <summary>
        /// 描述 :默认作休类型（1-周日 2-周一 4-周二 8-周三 16-周四 32-周五 64-周六）。多个相加起来 
        /// 空值 : false  
        /// </summary>
        public int RestType { get; set; }

        /// <summary>
        /// 描述 :所属站点代码 
        /// 空值 : false  
        /// </summary>
        public string SiteCode { get; set; }
    }
}
