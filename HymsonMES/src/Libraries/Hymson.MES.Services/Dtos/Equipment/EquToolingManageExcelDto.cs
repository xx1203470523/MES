using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;
using OfficeOpenXml.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Dtos.Equipment
{
    /// <summary>
    /// 工具导入模板
    /// </summary>
    public record EquToolingManageExcelDto : BaseExcelDto
    {
        /// <summary>
        /// 工具编号
        /// </summary>
        [EpplusTableColumn(Header = "工具编号(必填)", Order = 1)]
        public string Code { get; set; }

        /// <summary>
        /// 工具描述
        /// </summary>
        [EpplusTableColumn(Header = "工具描述(必填)", Order = 2)]
        public string Name { get; set; }


        /// <summary>
        /// 状态 1、启用  2、禁用
        /// </summary>
        [EpplusTableColumn(Header = "状态(必填 已启用,已禁用)", Order = 3)]
        public DisableOrEnableEnum Status { get; set; }

        /// <summary>
        /// 额定使用寿命
        /// </summary>
        [EpplusTableColumn(Header = "额定使用寿命", Order = 4)]
        public decimal RatedLife { get; set; }

        /// <summary>
        /// 是否校准 1、是 2、否
        /// </summary>
        [EpplusTableColumn(Header = "是否校准(必填 是，否)", Order = 5)]
        public YesOrNoEnum IsCalibrated { get; set; }

        /// <summary>
        /// 最后校验时间
        /// </summary>
        [EpplusTableColumn(Header = "最后校验时间", Order = 6)]
        public DateTime? LastVerificationTime { get; set; }

        /// <summary>
        /// 校准周期
        /// </summary>
        [EpplusTableColumn(Header = "校准周期（天，周，月）", Order = 7)]
        public decimal? CalibrationCycle { get; set; }

        /// <summary>
        /// 校准周期单位
        /// </summary>
        [EpplusTableColumn(Header = "校准周期单位", Order = 8)]
        public ToolingTypeEnum? CalibrationCycleUnit { get; set; }

        /// <summary>
        /// 工具类型
        /// </summary>
        [EpplusTableColumn(Header = "工具类型", Order = 9)]
        public string ToolTypeCode { get; set; }
    }
}
