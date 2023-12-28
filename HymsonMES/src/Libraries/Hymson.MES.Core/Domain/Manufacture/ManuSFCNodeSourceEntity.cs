using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Manufacture
{
    /// <summary>
    /// 数据实体（条码追溯表-反向）   
    /// manu_sfc_node_source
    /// @author Czhipu
    /// @date 2023-12-15 09:34:23
    /// </summary>
    public class ManuSFCNodeSourceEntity : BaseEntity
    {
        /// <summary>
        /// 条码ID;对应manu_sfc_node表Id
        /// </summary>
        public long NodeId { get; set; }

       /// <summary>
        /// 条码ID（来源）;对应manu_sfc_node表Id
        /// </summary>
        public long SourceId { get; set; }

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
