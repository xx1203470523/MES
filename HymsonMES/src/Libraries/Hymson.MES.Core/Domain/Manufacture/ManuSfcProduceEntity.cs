using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.Core.Domain.Manufacture
{
    /// <summary>
    /// 条码生产信息（物理删除），数据实体对象   
    /// manu_sfc_produce
    /// @author zhaoqing
    /// @date 2023-03-18 05:37:27
    /// </summary>
    public class ManuSfcProduceEntity : BaseEntity
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 条码id
        /// </summary>
        public long SFCId { get; set; }

        /// <summary>
        /// 条码
        /// </summary>
        public string SFC { get; set; }

        /// <summary>
        /// 产品id
        /// </summary>
        public long ProductId { get; set; }

        /// <summary>
        /// 工单id
        /// </summary>
        public long WorkOrderId { get; set; }

        /// <summary>
        /// 条码数据id
        /// </summary>
        public long BarCodeInfoId { get; set; }

        /// <summary>
        /// 工艺路线
        /// </summary>
        public long ProcessRouteId { get; set; }

        /// <summary>
        /// 工作中心
        /// </summary>
        public long WorkCenterId { get; set; }

        /// <summary>
        /// BOMId
        /// </summary>
        public long ProductBOMId { get; set; }

        /// <summary>
        /// 当前数量
        /// </summary>
        public decimal Qty { get; set; }

        /// <summary>
        ///报废数量
        /// </summary>
        public decimal? ScrapQty { get; set; }

        /// <summary>
        /// 设备Id
        /// </summary>
        public long? EquipmentId { get; set; }

        /// <summary>
        /// 资源id
        /// </summary>
        public long? ResourceId { get; set; }

        /// <summary>
        /// 工序id
        /// </summary>
        public long ProcedureId { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public SfcStatusEnum Status { get; set; }

        /// <summary>
        /// 锁;1：未锁定；2：即时锁；3：将来锁；
        /// </summary>
        public QualityLockEnum? Lock { get; set; }

        /// <summary>
        /// 将来锁，锁定的工序id
        /// </summary>
        public long? LockProductionId { get; set; }

        /// <summary>
        /// 是否可疑
        /// </summary>
        public bool? IsSuspicious { get; set; }

        /// <summary>
        /// 复投次数;复投次数
        /// </summary>
        public int RepeatedCount { get; set; }

        /// <summary>
        /// 是否报废
        /// </summary>
        public TrueOrFalseEnum IsScrap { get; set; }

        /// <summary>
        /// 锁定前状态
        /// </summary>
        public SfcStatusEnum? BeforeLockedStatus { get; set; }

        /// <summary>
        /// 汇总表id
        /// </summary>
        public long? SfcSummaryId { get; set; }

       /// <summary>
       /// 是否维修
       /// </summary>
        public TrueOrFalseEnum? IsRepair { get; set; }
    }
}
