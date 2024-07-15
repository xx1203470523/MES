using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.Core.Domain.Equipment
{
    /// <summary>
    /// 数据实体（工具类型管理）   
    /// equ_tools_type
    /// @author zhaoqing
    /// @date 2024-06-12 04:14:51
    /// </summary>
    public class EquToolsTypeEntity : BaseEntity
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
        /// 额定寿命
        /// </summary>
        public decimal RatedLife { get; set; }

        /// <summary>
        /// 额定寿命单位
        /// </summary>
        public string RatedLifeUnit { get; set; }

        /// <summary>
        /// 是否校准 1、是 2、否
        /// </summary>
        public YesOrNoEnum? IsCalibrated { get; set; }

        /// <summary>
        /// 校准周期
        /// </summary>
        public decimal? CalibrationCycle { get; set; }

        /// <summary>
        /// 校准周期单位
        /// </summary>
        public ToolingTypeEnum? CalibrationCycleUnit { get; set; }

        /// <summary>
        /// 是否所有设备都可用 1、是 2、否
        /// </summary>
        public bool IsAllEquipmentUsed { get; set; }

        /// <summary>
        /// 是否物料都可用 1、是 2、否
        /// </summary>
        public bool IsAllMaterialUsed { get; set; }

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
