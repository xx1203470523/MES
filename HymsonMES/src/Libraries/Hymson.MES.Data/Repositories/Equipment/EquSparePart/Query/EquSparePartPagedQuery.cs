using Hymson.Infrastructure;

namespace Hymson.MES.Data.Repositories.Equipment.EquSparePart.Query
{
    /// <summary>
    /// 备件注册 分页参数
    /// </summary>
    public class EquSparePartPagedQuery : PagerInfo
    {
        /// <summary>
        /// 站点编码 
        /// </summary>
        public string SiteCode { get; set; } = "";
    }
}
