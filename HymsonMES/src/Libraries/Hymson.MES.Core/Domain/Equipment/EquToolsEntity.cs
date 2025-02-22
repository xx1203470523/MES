using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.Core.Domain.Equipment
{
    /// <summary>
    /// 数据实体（工具管理）   
    /// equ_tools
    /// @author zhaoqing
    /// @date 2024-06-12 04:14:37
    /// </summary>
    public class EquToolsEntity : BaseEntity
    {
        /// <summary>
        /// 站点ID
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 类型id equ_tools_type的id
        /// </summary>
        public long ToolsId { get; set; }

        /// <summary>
        /// 额定寿命
        /// </summary>
        public decimal RatedLife { get; set; }

        /// <summary>
        /// 额定使用寿命单位
        /// </summary>
        public ToolingTypeEnum? RatedLifeUnit { get; set; }

        /// <summary>
        /// 累计使用寿命
        /// </summary>
        public decimal? CumulativeUsedLife { get; set; }

        /// <summary>
        /// 当前使用寿命
        /// </summary>
        public decimal? CurrentUsedLife { get; set; }

        /// <summary>
        /// 最后校验时间
        /// </summary>
        public DateTime? LastVerificationTime { get; set; }

        /// <summary>
        /// 是否校准 1、是 2、否
        /// </summary>
        public YesOrNoEnum IsCalibrated { get; set; }

        /// <summary>
        /// 校准周期
        /// </summary>
        public decimal? CalibrationCycle { get; set; }

        /// <summary>
        /// 校准周期单位
        /// </summary>
        public ToolingTypeEnum CalibrationCycleUnit { get; set; }

        /// <summary>
        /// 状态 1、启用  2、禁用
        /// </summary>
        public DisableOrEnableEnum Status { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        
    }
}
