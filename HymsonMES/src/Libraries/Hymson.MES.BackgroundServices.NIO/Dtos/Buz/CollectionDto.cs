namespace Hymson.MES.BackgroundServices.NIO.Dtos.Buz
{
    /// <summary>
    /// 控制项
    /// </summary>
    public class CollectionDto : BaseDto
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
        /// 产线唯一标识，最大长度64
        /// </summary>
        public string ProductionLineId { get; set; }

        /// <summary>
        /// 工位唯一标识，最大长度64
        /// </summary>
        public string StationId { get; set; }

        /// <summary>
        /// 采集设备唯一编码，最大长度64
        /// </summary>
        //public string DeviceId { get; set; }

        /// <summary>
        /// 对应控制项主数据中的字段 code，最大长度32
        /// </summary>
        public string VendorFieldCode { get; set; }

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

        /// <summary>
        /// 合作伙伴总成临时序列号，最大长度64
        /// </summary>
        public string VendorProductTempSn { get; set; }

        /// <summary>
        /// 合作伙伴总成电子条码，最大长度64
        /// </summary>
        public string VendorProductCode { get; set; }

        /// <summary>
        /// 合作伙伴总成批次号，最大长度64
        /// </summary>
        public string VendorProductBatch { get; set; }

        /// <summary>
        /// 值的作用类型，用于生产过程中可能采集多次的数据，默认为final。可选值：inprocess - 过程中的数据；final - 最终采用的数据。
        /// </summary>
        public string ProcessType { get; set; }

        /// <summary>
        /// 采集的控制项的值，当主数据 valueType = decimal 时必填
        /// </summary>
        public decimal? DecimalValue { get; set; }

        /// <summary>
        /// 采集的控制项的值，当主数据 valueType = string 时必填，最大长度64
        /// </summary>
        public string StringValue { get; set; }

        /// <summary>
        /// 采集的控制项的值，当主数据 valueType = boolean 时必填
        /// </summary>
        public bool? BooleanValue { get; set; }

        /// <summary>
        /// NIO 产品代码，最大长度128。成品、半成品、原材料统一称呼。同一个型号的产品拥有相同的产品代码。
        /// 例如：所有2022款ET5都有相同的产品代码，螺丝/油漆都有自己的产品代码。
        /// </summary>
        public string NioProductNum { get; set; }

        /// <summary>
        /// NIO 产品电子条码，最大长度64
        /// </summary>
        //public string NioProductCode { get; set; }

        /// <summary>
        /// NIO 产品名称，最大长度64
        /// 例如：ES8尾门总成
        /// </summary>
        public string NioProductName { get; set; }

        /// <summary>
        /// NIO 车型，最大长度32
        /// 例如：ES8, ET7
        /// </summary>
        //public string NioModel { get; set; }

        /// <summary>
        /// 操作员账号，最大长度64
        /// </summary>
        public string OperatorAccount { get; set; }

        /// <summary>
        /// 投入时间，即进入工位时间，Unix 时间戳，单位：秒
        /// </summary>
        public long InputTime { get; set; }

        /// <summary>
        /// 产出时间，即离开工位时间，Unix 时间戳，单位：秒
        /// </summary>
        public long OutputTime { get; set; }

        /// <summary>
        /// 标准化工位通过状态，参考附件分类
        /// </summary>
        public string StationStatus { get; set; }

        /// <summary>
        /// 合作伙伴自定义的工位的通过状态，最大长度32
        /// </summary>
        public string VendorStationStatus { get; set; }

        /// <summary>
        /// 合作伙伴自定义的本条数据的通过状态
        /// </summary>
        public string VendorValueStatus { get; set; }

        ///// <summary>
        ///// 关联部件01的电子条码，最大长度64
        ///// </summary>
        //public string PartCode01 { get; set; }

        ///// <summary>
        ///// 关联部件01的批次号，最大长度64
        ///// </summary>
        //public string PartBatch01 { get; set; }

        ///// <summary>
        ///// 关联部件01的名称，最大长度32
        ///// </summary>
        //public string PartName01 { get; set; }

        ///// <summary>
        ///// 关联部件02的电子条码，最大长度64
        ///// </summary>
        //public string PartCode02 { get; set; }

        ///// <summary>
        ///// 关联部件02的批次号，最大长度64
        ///// </summary>
        //public string PartBatch02 { get; set; }

        ///// <summary>
        ///// 关联部件02的名称，最大长度32
        ///// </summary>
        //public string PartName02 { get; set; }

        ///// <summary>
        ///// 关联部件03的电子条码，最大长度64
        ///// </summary>
        //public string PartCode03 { get; set; }

        ///// <summary>
        ///// 关联部件03的批次号，最大长度64
        ///// </summary>
        //public string PartBatch03 { get; set; }

        ///// <summary>
        ///// 关联部件03的名称，最大长度32
        ///// </summary>
        //public string PartName03 { get; set; }

        ///// <summary>
        ///// 控制项点位代码，最大长度128
        ///// </summary>
        //public string GatherSpot { get; set; }

        /// <summary>
        /// 是否调试
        /// </summary>
        //public bool Debug { get; set; }
    }
}
