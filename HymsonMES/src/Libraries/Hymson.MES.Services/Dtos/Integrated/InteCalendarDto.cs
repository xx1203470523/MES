using Hymson.Infrastructure;
using System.ComponentModel.DataAnnotations;

namespace Hymson.MES.Services.Dtos.Integrated
{
    /// <summary>
    ///  新增对象（日历维护）
    /// </summary>
    public record InteCalendarCreateDto : BaseEntityDto
    {
        /// <summary>
        ///日历名称
        /// </summary>
        [Required(ErrorMessage = "日历名称不能为空")]
        public string CalendarName { get; set; }

        /// <summary>
        ///日历类型（字典名称：inte_calendar_type）
        /// </summary>
        [Required(ErrorMessage = "日历类型（字典名称：inte_calendar_type）不能为空")]
        public string CalendarType { get; set; }

        /// <summary>
        ///设备或者线体id
        /// </summary>
        [Required(ErrorMessage = "设备或者线体id不能为空")]
        public long EquOrLineId { get; set; }

        /// <summary>
        ///日历描述
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 年
        /// </summary>
        public string Year { get; set; }

        /// <summary>
        /// 月
        /// </summary>
        public string Month { get; set; }

        /// <summary>
        /// 生产班次表Id
        /// </summary>
        public long? ClassId { get; set; }

        /// <summary>
        /// 默认作休类型
        /// </summary>
        //public string RestType { get; set; }

        /// <summary>
        /// 勾选的工作日
        /// </summary>
        public string Weekdays { get; set; }

        /// <summary>
        /// 日历详情
        /// </summary>
        public List<InteCalendarDateDetailDto> CalendarDataList { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool UseStatus { get; set; } = false;
    }

    /// <summary>
    ///  更新对象（日历维护）
    /// </summary>
    public record InteCalendarModifyDto : BaseEntityDto
    {
        /// <summary>
        ///主键id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        ///日历名称
        /// </summary>
        [Required(ErrorMessage = "日历名称不能为空")]
        public string CalendarName { get; set; }

        /// <summary>
        ///日历类型（字典名称：inte_calendar_type）
        /// </summary>
        [Required(ErrorMessage = "日历类型（字典名称：inte_calendar_type）不能为空")]
        public string CalendarType { get; set; }

        /// <summary>
        ///设备或者线体id
        /// </summary>
        [Required(ErrorMessage = "设备或者线体id不能为空")]
        public long EquOrLineId { get; set; }

        /// <summary>
        ///日历描述
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 年
        /// </summary>
        public string Year { get; set; }

        /// <summary>
        /// 月
        /// </summary>
        public string Month { get; set; }

        /// <summary>
        /// 生产班次表Id
        /// </summary>
        public long? ClassId { get; set; }

        /// <summary>
        /// 默认作休类型
        /// </summary>
        //public string RestType { get; set; }

        /// <summary>
        /// 勾选的工作日
        /// </summary>
        public string Weekdays { get; set; }

        /// <summary>
        /// 日历详情
        /// </summary>
        public List<InteCalendarDateDetailDto> CalendarDataList { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool UseStatus { get; set; } = false;
    }
    
    /// <summary>
    /// 查询返回对象
    /// </summary>
    public record InteCalendarDto : BaseEntityDto
    {
        /// <summary>
        /// 
        /// </summary>
        public long Id { get; set; }

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


        /// <summary>
        /// 名称
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
    }

    /// <summary>
    /// 查询详情返回对象
    /// </summary>
    public class InteCalendarDetailDto
    {
        /// <summary>
        /// 唯一标识
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 日历名称
        /// </summary>
        public string CalendarName { get; set; }

        /// <summary>
        /// 日历类型
        /// </summary>
        public string CalendarType { get; set; }

        /// <summary>
        /// 描述 :设备或者线体id 
        /// 空值 : false  
        /// </summary>
        public long EquOrLineId { get; set; }

        /// <summary>
        /// 描述 :设备或者线体名称
        /// 空值 : false  
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 描述 :设备或者线体编码
        /// 空值 : false  
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 日历描述
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 年
        /// </summary>
        public string Year { get; set; }

        /// <summary>
        /// 月
        /// </summary>
        public string[] Smonth { get; set; }

        /// <summary>
        /// 生产班次表Id
        /// </summary>
        public long? ClassId { get; set; }

        /// <summary>
        /// 选择的工作日
        /// </summary>
        public string[] WeekDay { get; set; }

        /// <summary>
        /// 描述 :班次名称 
        /// 空值 : false  
        /// </summary>
        public string ClassName { get; set; }

        /// <summary>
        /// 描述 :班次描述
        /// 空值 : false  
        /// </summary>
        public string ClassRemark { get; set; }

        /// <summary>
        /// 日历时间详情
        /// </summary>
        public List<QueryInteCalendarDetailDto> CalendarDataList { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool UseStatus { get; set; }
    }

    /// <summary>
    /// 日历详情
    /// </summary>
    public record QueryInteCalendarDetailDto : BaseEntityDto
    {
        ///// <summary>
        ///// 描述 :日历时间主表 
        ///// 空值 : false  
        ///// </summary>
        //public long CalendarDateId { get; set; }

        /// <summary>
        /// 描述 :年月日中的日 
        /// 空值 : false  
        /// </summary>
        public string Day { get; set; }

        /// <summary>
        /// 描述 :生产班制id 
        /// 空值 : false  
        /// </summary>
        public string ClassId { get; set; }

        /// <summary>
        /// 描述 :作息类型：工作日、休息日
        /// 空值 : false  
        /// </summary>
        public string RestType { get; set; }
    }

    /// <summary>
    /// 日历详情
    /// </summary>
    public class InteCalendarDateDetailDto
    {
        ///// <summary>
        ///// 描述 :日历时间主表 
        ///// 空值 : false  
        ///// </summary>
        //public long CalendarDateId { get; set; }

        /// <summary>
        /// 描述 :年月日中的日 
        /// 空值 : false  
        /// </summary>
        public DateTime Day { get; set; }

        /// <summary>
        /// 描述 :生产班制id 
        /// 空值 : false  
        /// </summary>
        public string ClassId { get; set; }

        /// <summary>
        /// 描述 :默认作休类型（1-周日 2-周一 4-周二 8-周三 16-周四 32-周五 64-周六）。多个相加起来 
        /// 空值 : false  
        /// </summary>
        public int? RestType { get; set; }
    }

    /// <summary>
    /// 查询对象（日历）
    /// </summary>
    public class InteCalendarPagedQueryDto : PagerInfo
    {
        /// <summary>
        /// 所属站点代码
        /// </summary>
        public string SiteCode { get; set; } = "";
        /// <summary>
        /// 日历名称
        /// </summary>
        public string CalendarName { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public string CalendarType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int UseStatus { get; set; } = -1;
    }
}
