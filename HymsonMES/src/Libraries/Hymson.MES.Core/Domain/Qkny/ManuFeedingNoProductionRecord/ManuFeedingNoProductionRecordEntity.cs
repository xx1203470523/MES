using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.ManuFeedingNoProductionRecord
{
    /// <summary>
    /// 数据实体（设备投料非生产投料(洗罐子)）   
    /// manu_feeding_no_production_record
    /// @author User
    /// @date 2024-03-18 11:51:44
    /// </summary>
    public class ManuFeedingNoProductionRecordEntity : BaseEntity
    {
        /// <summary>
        /// 设备id
        /// </summary>
        public long EquipmentId { get; set; }

        /// <summary>
        /// 投料至设备
        /// </summary>
        public string ConsumeEquipmentCode { get; set; }

        /// <summary>
        /// 投料至资源
        /// </summary>
        public string ConsumeResourceCodeCode { get; set; }

        /// <summary>
        /// 条码
        /// </summary>
        public string Sfc { get; set; }

        /// <summary>
        /// 重量
        /// </summary>
        public decimal? Qty { get; set; }

        /// <summary>
        /// 分类
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        
    }
}
