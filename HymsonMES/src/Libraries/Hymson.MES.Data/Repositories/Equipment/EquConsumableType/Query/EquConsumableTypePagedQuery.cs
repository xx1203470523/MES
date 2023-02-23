using Hymson.Infrastructure;

namespace Hymson.MES.Data.Repositories.Equipment.EquConsumableType.Query
{
    /// <summary>
    /// 分页参数（工装类型）
    /// </summary>
    public class EquConsumableTypePagedQuery : PagerInfo
    {
        /// <summary>
        /// 站点编码 
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 工装类型编码
        /// </summary>
        public string ConsumableTypeCode { get; set; } = "";

        /// <summary>
        /// 工装类型名称
        /// </summary>
        public string ConsumableTypeName { get; set; } = "";

        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; } = 0;
    }
}
