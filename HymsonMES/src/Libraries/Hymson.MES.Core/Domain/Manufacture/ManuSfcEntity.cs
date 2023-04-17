using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.Core.Domain.Manufacture
{
    /// <summary>
    /// 条码表，数据实体对象   
    /// manu_sfc
    /// @author wangkeming
    /// @date 2023-03-29 05:40:43
    /// </summary>
    public class ManuSfcEntity : BaseEntity
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 条码
        /// </summary>
        public string SFC { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public decimal Qty { get; set; }

        /// <summary>
        /// 状态;1：在制；2：完成；3：已入库；4：报废
        /// </summary>
        public SfcStatusEnum Status { get; set; }

        /// <summary>
        /// 是否使用
        /// </summary>
        public YesOrNoEnum IsUsed { get; set; }
    }
}
