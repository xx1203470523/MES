/*
 *creator: Karl
 *
 *describe: 条码步骤表    实体类 | 代码由框架生成  如果数据库字段发生变化,则手动调整
 *builder:  zhaoqing
 *build datetime: 2023-03-18 05:41:06
 */
using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Manufacture
{
    /// <summary>
    /// 条码步骤表，数据实体对象   
    /// manu_sfc_step
    /// @author zhaoqing
    /// @date 2023-03-18 05:41:06
    /// </summary>
    public class ManuSfcStepEntity : BaseEntity
    {
        /// <summary>
        /// 条码
        /// </summary>
        public string Sfc { get; set; }

       /// <summary>
        /// 产品信息
        /// </summary>
        public long ProductId { get; set; }

       /// <summary>
        /// 工单号
        /// </summary>
        public long WorkOrdeId { get; set; }

       /// <summary>
        /// 工作中心
        /// </summary>
        public long? WorkCenterId { get; set; }

       /// <summary>
        /// BOMId
        /// </summary>
        public long? ProductBOMId { get; set; }

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
        /// 步骤类型;1：创建；2：复用；3：进站；4：出站；5：将来锁定；6：及时锁定；7：解锁；8：完成；9：报废；10：异常标识；11：删除；12：拆解；13：合并；14：转换；
        /// </summary>
        public bool Type { get; set; }

       /// <summary>
        /// 当前状态;1：排队；2：激活；3：完工；
        /// </summary>
        public bool Status { get; set; }

       /// <summary>
        /// 锁;1：未锁定；2：即使锁：3：未来锁；
        /// </summary>
        public bool? Lock { get; set; }

       /// <summary>
        /// 是否复用
        /// </summary>
        public bool? IsMultiplex { get; set; }

       /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

       
    }
}
