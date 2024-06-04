using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.ManuFeedingTransferRecord
{
    /// <summary>
    /// 数据实体（上料信息转移记录）   
    /// manu_feeding_transfer_record
    /// @author Yxx
    /// @date 2024-03-18 11:19:42
    /// </summary>
    public class ManuFeedingTransferRecordEntity : BaseEntity
    {
        /// <summary>
        /// 设备id
        /// </summary>
        public long EquipmentId { get; set; }

        /// <summary>
        /// 条码
        /// </summary>
        public string Sfc { get; set; }

        /// <summary>
        /// 转出罐体设备号
        /// </summary>
        public string EquipmentCodeOut { get; set; }

        /// <summary>
        /// 转入罐体设备号
        /// </summary>
        public string EquipmentCodeIn { get; set; }

        /// <summary>
        /// 重量
        /// </summary>
        public decimal? Qty { get; set; }

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
