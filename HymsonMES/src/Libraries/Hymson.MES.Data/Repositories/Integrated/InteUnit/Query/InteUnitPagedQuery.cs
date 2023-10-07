using Hymson.Infrastructure;

namespace Hymson.MES.Data.Repositories.Integrated.Query
{
    /// <summary>
    /// 单位维护 分页参数
    /// </summary>
    public class InteUnitPagedQuery : PagerInfo
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long? Id { get; set; }

        /// <summary>
        /// 单位编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 单位名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }
    }
}
