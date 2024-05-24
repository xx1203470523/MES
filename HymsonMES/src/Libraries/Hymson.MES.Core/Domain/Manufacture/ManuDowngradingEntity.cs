using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Manufacture
{
    /// <summary>
    /// 降级录入，数据实体对象   
    /// manu_downgrading
    /// @author Karl
    /// @date 2023-08-10 10:15:26
    /// </summary>
    public class ManuDowngradingEntity : BaseEntity
    {
        /// <summary>
        /// 站点ID
        /// </summary>
        public long SiteId { get; set; }

       /// <summary>
        /// 条码
        /// </summary>
        public string SFC { get; set; }

       /// <summary>
        /// 品级
        /// </summary>
        public string Grade { get; set; }

       /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; } = "";

        /// <summary>
        /// 序号
        /// </summary>
        public int SerialNumber { get; set; } = 0;
    }
}
