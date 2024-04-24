using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Manufacture;

namespace Hymson.MES.Core.Domain.Manufacture
{
    /// <summary>
    /// 数据实体（条码关系表）   
    /// manu_barcode_relation
    /// @author Czhipu
    /// @date 2024-04-24 04:53:42
    /// </summary>
    public class ManuBarCodeRelationEntity : BaseEntity
    {
        /// <summary>
        /// 站点ID
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 操作工序id
        /// </summary>
        public long? ProcedureId { get; set; }

        /// <summary>
        /// 操作资源id
        /// </summary>
        public long? ResourceId { get; set; }

        /// <summary>
        /// 操作设备id
        /// </summary>
        public long? EquipmentId { get; set; }

        /// <summary>
        /// 投入条码
        /// </summary>
        public string InputBarcode { get; set; }

        /// <summary>
        /// 投入条码位置号
        /// </summary>
        public string InputBarcodeLocation { get; set; }

        /// <summary>
        /// 投入条码物料id
        /// </summary>
        public long InputBarcodeMaterialId { get; set; }

        /// <summary>
        /// 投入条码工单id
        /// </summary>
        public long? InputBarcodeWorkorderId { get; set; }

        /// <summary>
        /// 投入数量
        /// </summary>
        public decimal InputQty { get; set; }

        /// <summary>
        /// 产出条码
        /// </summary>
        public string OutputBarcode { get; set; }

        /// <summary>
        /// 产出条码物料id
        /// </summary>
        public long? OutputBarcodeMaterialId { get; set; }

        /// <summary>
        /// 产出条码工单id
        /// </summary>
        public long? OutputBarcodeWorkorderId { get; set; }

        /// <summary>
        /// 产出条码模式 1、分组（无条码时采用分组模式）  2、正常
        /// </summary>
        public ManuBarCodeOutputModeEnum OutputBarcodeMode { get; set; }

        /// <summary>
        /// 关系类型 1、条码绑定 2、条码消耗3、转换4、合并5、拆分6、批次合并7、批次合并
        /// </summary>
        public ManuBarCodeRelationTypeEnum RelationType { get; set; }

        /// <summary>
        /// 业务内容
        /// </summary>
        public string BusinessContent { get; set; }

        /// <summary>
        /// 是否拆解(0:未拆解，1：拆解)
        /// </summary>
        public TrueOrFalseEnum? IsDisassemble { get; set; }

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

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }


    }
}
