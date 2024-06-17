using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Equipment
{
    /// <summary>
    /// 数据实体（工具管理记录表）   
    /// equ_tools_record
    /// @author zhaoqing
    /// @date 2024-06-12 04:15:05
    /// </summary>
    public class EquToolsRecordEntity : BaseEntity
    {
        /// <summary>
        /// 站点ID
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 工具id
        /// </summary>
        public long? ToolId { get; set; }

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
        public string ToolsId { get; set; }

        /// <summary>
        /// 额定寿命单位
        /// </summary>
        public decimal RatedLife { get; set; }

        /// <summary>
        /// 额定寿命单位
        /// </summary>
        public string RatedLifeUnit { get; set; }

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
        public bool IsCalibrated { get; set; }

        /// <summary>
        /// 校准周期
        /// </summary>
        public decimal? CalibrationCycle { get; set; }

        /// <summary>
        /// 校准周期单位
        /// </summary>
        public string CalibrationCycleUnit { get; set; }

        /// <summary>
        /// 状态 1、启用  2、禁用
        /// </summary>
        public bool Status { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 设备id equ_equipment的 id
        /// </summary>
        public long? EquipmentId { get; set; }

        /// <summary>
        /// 操作类型 1、注册 2、绑定 3、解绑4、寿命扣减5、校准
        /// </summary>
        public string OperationType { get; set; }

        /// <summary>
        /// 操作备注
        /// </summary>
        public string OperationRemark { get; set; }

        
    }
}
