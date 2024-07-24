using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Manufacture;

namespace Hymson.MES.Core.Domain.Manufacture
{
    /// <summary>
    /// 条码流转表，数据实体对象   
    /// manu_sfc_circulation
    /// @author zhaoqing
    /// @date 2023-03-27 03:50:00
    /// </summary>
    public class ManuSfcCirculationEntity : BaseEntity
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 当前工序id
        /// </summary>
        public long ProcedureId { get; set; }

        /// <summary>
        /// 资源id
        /// </summary>
        public long? ResourceId { get; set; }

        /// <summary>
        /// 设备id
        /// </summary>
        public long? EquipmentId { get; set; }

        /// <summary>
        /// 扣料上料点id
        /// </summary>
        public long? FeedingPointId { get; set; }

        /// <summary>
        /// 流转前条码
        /// </summary>
        public string SFC { get; set; }

        /// <summary>
        /// 流转前工单id
        /// </summary>
        public long WorkOrderId { get; set; }

        /// <summary>
        /// 流转前产品id
        /// </summary>
        public long ProductId { get; set; }

        /// <summary>
        /// 流转后条码信息
        /// </summary>
        public string CirculationBarCode { get; set; }

        /// <summary>
        /// 流转后工单id
        /// </summary>
        public long? CirculationWorkOrderId { get; set; }

        /// <summary>
        /// 流转后产品id-替代料
        /// </summary>
        public long CirculationProductId { get; set; }

        /// <summary>
        /// 流转后主产品id
        /// </summary>
        public long? CirculationMainProductId { get; set; }

        /// <summary>
        /// 流转后主产品供应商id
        /// </summary>
        public long? CirculationMainSupplierId { get; set; }

        /// <summary>
        /// 流转条码数量
        /// </summary>
        public decimal? CirculationQty { get; set; }


        /// <summary>
        /// 流转条码位置号
        /// </summary>
        public string? Location { get; set; }

        /// <summary>
        /// 流转类型;1：拆分；2：合并；3：转换;4：消耗;5：拆解;6：组件添加 7.换件
        /// </summary>
        public SfcCirculationTypeEnum CirculationType { get; set; }

        /// <summary>
        /// 是否拆解
        /// </summary>
        public TrueOrFalseEnum IsDisassemble { get; set; } = TrueOrFalseEnum.No;

        /// <summary>
        /// 拆解人
        /// </summary>
        public string DisassembledBy { get; set; }

        /// <summary>
        /// 拆解时间
        /// </summary>
        public DateTime? DisassembledOn { get; set; }

        /// <summary>
        /// 换件id manu_sfc_circulation id
        /// </summary>
        public long? SubstituteId { get; set; }
    }
}
