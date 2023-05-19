using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Manufacture
{
    /// <summary>
    /// 上卸料信息表（轻量），数据实体对象   
    /// manu_feeding_lite
    /// @author Czhipu
    /// @date 2023-05-19 09:43:46
    /// </summary>
    public class ManuFeedingLiteEntity : BaseEntity
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

       /// <summary>
        /// 设备Id
        /// </summary>
        public long EquipmentId { get; set; }

       /// <summary>
        /// 资源Id
        /// </summary>
        public long ResourceId { get; set; }

       /// <summary>
        /// 条码
        /// </summary>
        public string BarCode { get; set; }

       /// <summary>
        /// 初始数量
        /// </summary>
        public decimal InitQty { get; set; }

       /// <summary>
        /// 上料数量/卸料数量;允许为空，空则取原材料条码解析数据
        /// </summary>
        public decimal? Qty { get; set; }

       
    }
}
