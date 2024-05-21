using Hymson.MES.Core.Enums;

namespace Hymson.MES.Data.Repositories.Manufacture
{
    /// <summary>
    /// 条码信息
    /// </summary>
    public class ManuSfcView
    {
        /// <summary>
        /// Id
        /// </summary>
        public long Id { get; set; }

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
        /// 工艺路线
        /// </summary>
        public long? ProcessRouteId { get; set; }

        /// <summary>
        /// Bom id
        /// </summary>
        public long? ProductBOMId { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public decimal Qty { get; set; }

        /// <summary>
        /// 状态;1：在制；2：完成；3：已入库；4：报废；
        /// </summary>
        public SfcStatusEnum Status { get; set; }

        /// <summary>
        /// 是否使用
        /// </summary>
        public YesOrNoEnum IsUsed { get; set; } = YesOrNoEnum.No;

        /// <summary>
        /// manu_sfc_info 表 id
        /// </summary>
        public long SFCInfoId { get; set; }
        
    }
}
