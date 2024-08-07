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
    }

}
