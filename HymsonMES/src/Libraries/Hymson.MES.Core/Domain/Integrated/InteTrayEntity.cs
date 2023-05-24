using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Integrated
{
    /// <summary>
    /// 托盘信息，数据实体对象   
    /// inte_tray
    /// @author chenjianxiong
    /// @date 2023-05-16 10:57:03
    /// </summary>
    public class InteTrayEntity : BaseEntity
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

       /// <summary>
        /// 托盘编码
        /// </summary>
        public string Code { get; set; }

       /// <summary>
        /// 托盘名称
        /// </summary>
        public string Name { get; set; }

       /// <summary>
        /// 装载数量
        /// </summary>
        public decimal MaxLoadQty { get; set; }

       /// <summary>
        /// 最大序号
        /// </summary>
        public int? MaxSeq { get; set; }

       
    }
}
