using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.SystemServices.Dtos
{
   /// <summary>
   /// 班次
   /// </summary>
    public record InteClassDto : BaseEntityDto
    {
        /// <summary>
        /// 班次编码
        /// </summary>
        public string ClassCode { get; set; }

        /// <summary>
        /// 班次名称
        /// </summary>
        public string ClassName { get; set; }

        /// <summary>
        /// 工作小时数
        /// </summary>
        public decimal WorkingHours { get; set; }

        /// <summary>
        /// 班次描述
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public string StartTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public string EndTime { get; set; }

        /// <summary>
        /// 班次状态
        /// </summary>
        //public SysDataStatusEnum Status { get; set; }
    }
}
