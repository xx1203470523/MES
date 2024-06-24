using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.Data.Repositories.Integrated.Query
{
    /// <summary>
    /// 资质认证 分页参数
    /// </summary>
    public class InteQualificationAuthenticationPagedQuery : PagerInfo
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        public string? Code { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// 状态 1、启用  2、禁用
        /// </summary>
        public DisableOrEnableEnum? Status { get; set; }

    }
}
