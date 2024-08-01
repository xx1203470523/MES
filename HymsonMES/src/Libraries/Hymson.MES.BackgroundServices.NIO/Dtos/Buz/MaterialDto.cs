namespace Hymson.MES.BackgroundServices.NIO.Dtos.Buz
{
    /// <summary>
    /// 材料清单
    /// </summary>
    public class MaterialDto : BaseDto
    {
        /// <summary>
        /// 合作伙伴总成产品代码，成品、半成品、原材料统一称呼。同一个型号的产品拥有相同的产品代码。最大长度128。
        /// 示例：所有2022款ET5都有相同的产品代码；螺丝/油漆都有自己的产品代码。
        /// </summary>
        public string VendorProductNum { get; set; }

        /// <summary>
        /// 合作伙伴总成序列号，最大长度64。
        /// </summary>
        public string VendorProductSn { get; set; }

        ///// <summary>
        ///// 合作伙伴总成电子条码，最大长度64。
        ///// </summary>
        //public string VendorProductCode { get; set; }

        ///// <summary>
        ///// 合作伙伴总成批次号，最大长度64。
        ///// </summary>
        //public string VendorProductBatch { get; set; }

        ///// <summary>
        ///// 合作伙伴总成物料名称，最大长度64。
        ///// </summary>
        //public string VendorProductName { get; set; }

        ///// <summary>
        ///// 父节点电子条码，最大长度64。
        ///// </summary>
        //public string ParentCode { get; set; }

        ///// <summary>
        ///// 父节点物料名称，最大长度32。
        ///// </summary>
        //public string ParentName { get; set; }

        ///// <summary>
        ///// 父节点批次号，最大长度64。
        ///// </summary>
        //public string ParentBatch { get; set; }

        ///// <summary>
        ///// 父节点产品代码，最大长度128。
        ///// </summary>
        //public string ParentNum { get; set; }

        ///// <summary>
        ///// 父节点序列号，最大长度64。（可选字段）
        ///// </summary>
        //public string ParentSn { get; set; }

        ///// <summary>
        ///// 父节点质量说明，最大长度128。（可选字段）
        ///// </summary>
        //public string ParentQualityControl { get; set; }

        ///// <summary>
        ///// 父节点硬件版本号，最大长度64。
        ///// </summary>
        //public string ParentHardwareRevision { get; set; }

        ///// <summary>
        ///// 父节点软件版本号，最大长度64。
        ///// </summary>
        //public string ParentSoftwareRevision { get; set; }

        ///// <summary>
        ///// 子节点电子条码，最大长度64。
        ///// </summary>
        //public string ChildCode { get; set; }

        ///// <summary>
        ///// 子节点批次号，最大长度64。
        ///// </summary>
        //public string ChildBatch { get; set; }

        /// <summary>
        /// 子节点物料名称，最大长度32。
        /// </summary>
        public string ChildName { get; set; }

        /// <summary>
        /// 子节点产品代码，最大长度128。
        /// </summary>
        public string ChildNum { get; set; }

        /// <summary>
        /// 子节点序列号，最大长度64。（可选字段）
        /// </summary>
        public string ChildSn { get; set; }

        ///// <summary>
        ///// 子节点质量说明，最大长度128。（可选字段）
        ///// </summary>
        //public string ChildQualityControl { get; set; }

        ///// <summary>
        ///// 子节点硬件版本号，最大长度64。
        ///// </summary>
        //public string ChildHardwareRevision { get; set; }

        ///// <summary>
        ///// 子节点软件版本号，最大长度64。
        ///// </summary>
        //public string ChildSoftwareRevision { get; set; }

        ///// <summary>
        ///// 子节点产品类型，电池包、模组、电芯，最大长度64。
        ///// </summary>
        //public string ChildType { get; set; }

        ///// <summary>
        ///// 子节点部件生产时间，Unix 时间戳，单位：秒。
        ///// </summary>
        //public long ChildProductionTime { get; set; }

        ///// <summary>
        ///// 子节点部件生产厂商，最大长度32。
        ///// </summary>
        //public string ChildVendorName { get; set; }

        /// <summary>
        /// 子件物料来源，自制M，采购P
        /// </summary>
        public string ChildSource { get; set; } = "P";
    }
}
