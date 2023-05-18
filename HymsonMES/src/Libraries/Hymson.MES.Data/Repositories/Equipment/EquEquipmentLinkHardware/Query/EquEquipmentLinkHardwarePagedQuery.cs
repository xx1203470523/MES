using Hymson.Infrastructure;

namespace Hymson.MES.Data.Repositories.Equipment.EquEquipmentUnit.Query
{
    /// <summary>
    /// 
    /// </summary>
    public class EquEquipmentLinkHardwarePagedQuery : PagerInfo
    {
        /// <summary>
        /// 站点编码 
        /// </summary>
        public long? SiteId { get; set; }

    }
}
