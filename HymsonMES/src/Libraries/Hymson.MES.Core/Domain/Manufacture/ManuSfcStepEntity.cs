/*
 *creator: Karl
 *
 *describe: 条码步骤表    实体类 | 代码由框架生成  如果数据库字段发生变化,则手动调整
 *builder:  zhaoqing
 *build datetime: 2023-03-22 05:17:57
 */
using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Manufacture;

namespace Hymson.MES.Core.Domain.Manufacture
{
    /// <summary>
    /// 条码步骤表，数据实体对象   
    /// manu_sfc_step
    /// @author zhaoqing
    /// @date 2023-03-22 05:17:57
    /// </summary>
    public class ManuSfcStepEntity : BaseEntity
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 条码
        /// </summary>
        public string SFC { get; set; }

        /// <summary>
        /// 产品信息
        /// </summary>
        public long ProductId { get; set; }

        /// <summary>
        /// 工单号
        /// </summary>
        public long WorkOrderId { get; set; }

        /// <summary>
        /// BOM id
        /// </summary>
        public long? ProductBOMId { get; set; }

        /// <summary>
        /// 工作中心
        /// </summary>
        public long? WorkCenterId { get; set; }

        /// <summary>
        /// 当前数量
        /// </summary>
        public decimal Qty { get; set; }

        /// <summary>
        /// 设备id
        /// </summary>
        public long? EquipmentId { get; set; }

        /// <summary>
        /// 资源id
        /// </summary>
        public long? ResourceId { get; set; }

        /// <summary>
        /// 工序id
        /// </summary>
        public long? ProcedureId { get; set; }

        /// <summary>
        /// 步骤类型; 跟枚举的对应不上了，具体以枚举的为准
        /// </summary>
        public ManuSfcStepTypeEnum Operatetype { get; set; }

        /// <summary>
        /// 当前状态
        /// </summary>
        public SfcStatusEnum CurrentStatus { get; set; }

        /// <summary>
        /// 复投次数
        /// </summary>
        public int? RepeatedCount { get; set; }


        public bool? IsRepair { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string? Remark { get; set; }


        // 2023-09-21 10:00:00 add by Czhipu
        /// <summary>
        /// 载具条码
        /// </summary>
        public string? VehicleCode { get; set; }

    }
}
