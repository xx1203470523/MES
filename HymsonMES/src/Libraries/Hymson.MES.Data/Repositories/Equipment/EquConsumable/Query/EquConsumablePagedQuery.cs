using Hymson.Infrastructure;

namespace Hymson.MES.Data.Repositories.Equipment.EquSparePart.Query
{
    /// <summary>
    /// 分页参数（工装注册）
    /// </summary>
    public class EquConsumablePagedQuery : PagerInfo
    {
        /// <summary>
        /// 站点编码 
        /// </summary>
        public string SiteCode { get; set; } = "";
    }
}
