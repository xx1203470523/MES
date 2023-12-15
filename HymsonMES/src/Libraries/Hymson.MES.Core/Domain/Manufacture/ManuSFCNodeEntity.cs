using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Manufacture
{
    /// <summary>
    /// 数据实体（条码追溯表）   
    /// manu_sfc_node
    /// @author Czhipu
    /// @date 2023-12-15 09:34:10
    /// </summary>
    public class ManuSFCNodeEntity : BaseEntity
    {
        /// <summary>
        /// 产品ID
        /// </summary>
        public long ProductId { get; set; }

       /// <summary>
        /// 条码
        /// </summary>
        public string SFC { get; set; }

       /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

       /// <summary>
        /// 位置
        /// </summary>
        public string Location { get; set; }

       /// <summary>
        /// 反向树
        /// </summary>
        public string SourceNodes { get; set; }

       /// <summary>
        /// 正向树
        /// </summary>
        public string DestinationNodes { get; set; }

       /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

       
    }
}
