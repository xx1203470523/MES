using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Manufacture
{
    /// <summary>
    /// 数据实体（条码追溯表-正向）   
    /// manu_sfc_node_destination
    /// @author Czhipu
    /// @date 2023-12-15 09:34:33
    /// </summary>
    public class ManuSFCNodeDestinationEntity : BaseEntity
    {
        /// <summary>
        /// 条码ID;对应manu_sfc_node表Id
        /// </summary>
        public long NodeId { get; set; }

       /// <summary>
        /// 条码ID（去向）;对应manu_sfc_node表Id
        /// </summary>
        public long DestinationId { get; set; }

        /// <summary>
        /// 流转记录ID（对应manu_sfc_circulation表Id）
        /// </summary>
        public long CirculationId { get; set; }

        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

       
    }
}
