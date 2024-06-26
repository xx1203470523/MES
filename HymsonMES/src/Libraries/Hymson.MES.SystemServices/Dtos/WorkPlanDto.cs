﻿using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.SystemServices.Dtos
{
    /// <summary>
    /// 工作计划 Dto
    /// </summary>
    public record WorkPlanDto : BaseEntityDto
    {
        /*
        /// <summary>
        /// 部门编号（用这个来区分是哪个产线的工单，值维护成跟产线编码一样） 
        /// </summary>
        public string DepartmentCode { get; set; } = "";
        */

        /// <summary>
        /// 计划单号 
        /// </summary>
        public string PlanCode { get; set; } = "";

        /// <summary>
        /// 需求单号
        /// </summary>
        public string? RequirementNumber { get; set; }

        /// <summary>
        /// 产品编号 
        /// </summary>
        public string ProductCode { get; set; } = "";

        /// <summary>
        /// 产品版本
        /// </summary>
        public string ProductVersion { get; set; } = "";

        /// <summary>
        /// Bom编码
        /// </summary>
        public string BomCode { get; set; } = "";

        /// <summary>
        /// Bom版本
        /// </summary>
        public string BomVersion { get; set; } = "";

        /// <summary>
        /// 计划类型
        /// </summary>
        public PlanWorkOrderTypeEnum Type { get; set; }

        /// <summary>
        /// 工单数量
        /// </summary>
        public decimal Qty { get; set; }

        /// <summary>
        /// 生产时间（计划）
        /// </summary>
        public DateTime? PlanStartTime { get; set; }

        /// <summary>
        /// 结束时间（计划）
        /// </summary>
        public DateTime? PlanEndTime { get; set; }

    }
}
