/*
 *creator: Karl
 *
 *describe: 在制品维修记录    实体类 | 代码由框架生成  如果数据库字段发生变化,则手动调整
 *builder:  pengxin
 *build datetime: 2023-04-14 02:18:02
 */
using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Manufacture
{
    /// <summary>
    /// 在制品维修记录，数据实体对象   
    /// manu_sfc_repair_record
    /// @author pengxin
    /// @date 2023-04-14 02:18:02
    /// </summary>
    public class ManuSfcRepairRecordEntity : BaseEntity
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
        /// 工单id
        /// </summary>
        public long WorkOrderId { get; set; }

        /// <summary>
        /// 产品id
        /// </summary>
        public long ProductId { get; set; }

        /// <summary>
        /// 维修资源id
        /// </summary>
        public long ResourceId { get; set; }

        /// <summary>
        /// 维修工序id
        /// </summary>
        public long ProcedureId { get; set; }

        /// <summary>
        /// 返回工序Id
        /// </summary>
        public long? ReturnProcedureId { get; set; }

        /// <summary>
        /// 步骤id
        /// </summary>
        public long SfcStepId { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; } = "";
    }
}
