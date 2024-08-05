using Hymson.Infrastructure;

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

       
    }

    /// <summary>
    /// NIO推送参数分页Dto
    /// </summary>
    public class NioPushCollectionPagedQueryDto : PagerInfo { }

}
