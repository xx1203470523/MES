namespace Hymson.MES.BackgroundServices.NIO.Dtos.Buz
{
    /// <summary>
    /// 生产业务
    /// </summary>
    public class ProductionDto : BaseDto
    {
        /// <summary>
        /// 工厂唯一标识，最大长度64
        /// </summary>
        public string PlantId { get; set; }

        /// <summary>
        /// 车间唯一标识，最大长度64
        /// </summary>
        public string WorkshopId { get; set; }

        /// <summary>
        /// 生产线唯一标识，最大长度64
        /// </summary>
        public string ProductionLineId { get; set; }

        /// <summary>
        /// 工位唯一标识，最大长度64
        /// </summary>
        public string StationId { get; set; }

        /// <summary>
        /// 合作伙伴总成产品代码，最大长度128。成品、半成品、原材料统一称呼。同一个型号的产品拥有相同的产品代码。
        /// 例如：所有2022款ET5都有相同的产品代码，螺丝/油漆都有自己的产品代码。
        /// </summary>
        public string VendorProductNum { get; set; }

        /// <summary>
        /// 合作伙伴总成物料名称，最大长度64
        /// </summary>
        public string VendorProductName { get; set; }

        /// <summary>
        /// 合作伙伴总成序列号，最大长度64
        /// </summary>
        public string VendorProductSn { get; set; }

        ///// <summary>
        ///// 合作伙伴总成电子条码，最大长度64
        ///// </summary>
        //public string VendorProductCode { get; set; }

        ///// <summary>
        ///// 合作伙伴总成批次号，最大长度64
        ///// </summary>
        //public string VendorProductBatch { get; set; }

        /// <summary>
        /// 合作伙伴总成临时序列号，最大长度64
        /// </summary>
        public string VendorProductTempSn { get; set; }

        /// <summary>
        /// 工单ID，最大长度64
        /// </summary>
        public string WorkorderId { get; set; }

        /// <summary>
        /// 操作员账号ID，最大长度64
        /// </summary>
        public string OperatorId { get; set; }

        /// <summary>
        /// 操作员姓名，最大长度64
        /// </summary>
        public string OperatorName { get; set; }

        /// <summary>
        /// 料进工位的时间，Unix时间戳，以秒为单位
        /// </summary>
        public long InputTime { get; set; }

        /// <summary>
        /// 料出工位的时间，Unix时间戳，以秒为单位
        /// </summary>
        public long OutputTime { get; set; }

        ///// <summary>
        ///// 设备判定的质量状态，如果设备可以自动判定质量状态，则此字段有效
        ///// </summary>
        //public bool? DeviceDeterminedStatus { get; set; }

        ///// <summary>
        ///// 人工判定的质量状态，如果需要人工判定质量状态，则此字段有效
        ///// </summary>
        //public bool? ManualDeterminedStatus { get; set; }

        /// <summary>
        /// 最终质量状态，根据设备判定和人工判定综合得出
        /// </summary>
        public bool DeterminedStatus { get; set; }
    }

    /// <summary>
    /// Nio推送数据
    /// </summary>
    public class NioProductionDto
    {
        /// <summary>
        /// 标识码
        /// </summary>
        public string SchemaCode { get; set; }

        /// <summary>
        /// 数据
        /// </summary>
        public List<ProductionDto> List = new List<ProductionDto>();
    }
}
