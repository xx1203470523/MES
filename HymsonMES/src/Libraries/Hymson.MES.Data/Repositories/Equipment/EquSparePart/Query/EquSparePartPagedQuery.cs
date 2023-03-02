using Hymson.Infrastructure;
using Hymson.Infrastructure.Constants;

namespace Hymson.MES.Data.Repositories.Equipment.EquSparePart.Query
{
    /// <summary>
    /// 备件注册 分页参数
    /// </summary>
    public class EquSparePartPagedQuery : PagerInfo
    {
        /// <summary>
        /// 备件/工装
        /// </summary>
        public int Type { get; set; } = DbDefaultValueConstant.IntDefaultValue;

        /// <summary>
        /// 站点编码 
        /// </summary>
        public long? SiteId { get; set; }
    }
}
