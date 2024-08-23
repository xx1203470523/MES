using Hymson.Infrastructure;
using Hymson.MES.Core.Enums.Plan;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.Services.Dtos.NioPushCollection
{
    /// <summary>
    /// NIO推送参数新增/更新Dto
    /// </summary>
    public record NioPushCollectionSaveDto : BaseEntityDto
    {
        /// <summary>
        /// 
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public long NioPushId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string PlantId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string WorkshopId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ProductionLineId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string StationId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string VendorFieldCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string VendorProductNum { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string VendorProductName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string VendorProductSn { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string VendorProductTempSn { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ProcessType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public decimal? DecimalValue { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string? StringValue { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool BooleanValue { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string NioProductNum { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string NioProductName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string OperatorAccount { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public long InputTime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public long OutputTime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string StationStatus { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string VendorStationStatus { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string VendorValueStatus { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Owner { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int DataType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string UpdatedBy { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? UpdatedOn { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public long IsDeleted { get; set; }

        /// <summary>  
        /// 上限值，可能来自t2表  
        /// </summary>  
        public decimal? UpperLimit { get; set; } // 假设是数值类型，根据实际情况选择类型  

        /// <summary>  
        /// 中心值，可能来自t2表  
        /// </summary>  
        public decimal? CenterValue { get; set; } // 假设是数值类型，根据实际情况选择类型  

        /// <summary>  
        /// 下限值，可能来自t2表  
        /// </summary>  
        public decimal? LowerLimit { get; set; } // 假设是数值类型，根据实际情况选择类型  
    }

    /// <summary>
    /// NIO推送参数Dto
    /// </summary>
    public record NioPushCollectionDto : BaseEntityDto
    {
        /// <summary>
        /// 
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public long NioPushId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string PlantId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string WorkshopId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ProductionLineId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string StationId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string VendorFieldCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string VendorProductNum { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string VendorProductName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string VendorProductSn { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string VendorProductTempSn { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ProcessType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public decimal? DecimalValue { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string StringValue { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool? BooleanValue { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string NioProductNum { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string NioProductName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string OperatorAccount { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public long InputTime { get; set; }

       /// <summary>
        /// 
        /// </summary>
        public long OutputTime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string StationStatus { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string VendorStationStatus { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string VendorValueStatus { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Owner { get; set; }

       /// <summary>
        /// 
        /// </summary>
        public int DataType { get; set; }

         /// <summary>
        /// 
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime CreatedOn { get; set; }
 
        /// <summary>
        /// 
        /// </summary>
        public string UpdatedBy { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? UpdatedOn { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public long IsDeleted { get; set; }

        /// <summary>
        /// 推送状态;0：无需推送；1：待推送；2：已推送；3：推送失败；
        /// </summary>
        public PushStatusEnum Status { get; set; }

        /// <summary>
        /// 是否合格
        /// </summary>
        public TrueOrFalseEnum IsOk { get; set; } = TrueOrFalseEnum.Yes;

        /// <summary>  
        /// 上限值，可能来自t2表  
        /// </summary>  
        public decimal? UpperLimit { get; set; } // 假设是数值类型，根据实际情况选择类型  

        /// <summary>  
        /// 中心值，可能来自t2表  
        /// </summary>  
        public decimal? CenterValue { get; set; } // 假设是数值类型，根据实际情况选择类型  

        /// <summary>  
        /// 下限值，可能来自t2表  
        /// </summary>  
        public decimal? LowerLimit { get; set; } // 假设是数值类型，根据实际情况选择类型  

        /// <summary>
        /// 参数名称
        /// </summary>
        public string ParameterName { get; set; }

        /// <summary>
        /// 工序名称
        /// </summary>
        public string ProcedureName { get; set; }
    }

    /// <summary>
    /// NIO推送参数分页Dto
    /// </summary>
    public class NioPushCollectionPagedQueryDto : PagerInfo 
    {
        /// <summary>
        /// 合作伙伴总成序列号
        /// </summary>
        public string ?VendorProductSn { get; set; }

        /// <summary>
        /// 合作伙伴总成临时序列号
        /// </summary>
        public string ?VendorProductTempSn { get; set; }

        /// <summary>
        /// 工位唯一标识
        /// </summary>
        public string ?StationId { get; set; }

        /// <summary>
        /// 对应控制项主数据中的字段
        /// </summary>
        public string ?VendorFieldCode { get; set; }

        /// <summary>
        /// 推送状态;0：无需推送；1：待推送；2：已推送；3：推送失败；
        /// </summary>
        public PushStatusEnum? Status { get; set; }

        /// <summary>
        /// 是否合格
        /// </summary>
        public TrueOrFalseEnum? IsOk { get; set; }

        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTime[] ? CreatedOn { get; set; }
    }

    /// <summary>
    /// NIO参数数据
    /// </summary>
    public class NioCollectionDto
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

        ///// <summary>
        ///// 合作伙伴总成电子条码，最大长度64
        ///// </summary>
        //public string VendorProductCode { get; set; }

        ///// <summary>
        ///// 合作伙伴总成批次号，最大长度64
        ///// </summary>
        //public string VendorProductBatch { get; set; }

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

        /// <summary>
        /// 更改的时间, Unix 时间戳, 以秒为单位
        /// </summary>
        public long UpdateTime { get; set; }

        /// <summary>
        /// 数据归属
        /// </summary>
        public string Owner { get; set; } = "EDS";
    }
}
