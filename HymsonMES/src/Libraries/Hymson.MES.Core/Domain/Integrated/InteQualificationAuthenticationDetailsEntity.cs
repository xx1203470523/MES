using Hymson.Infrastructure;
using Hymson.MES.Core.Enums.Integrated;

namespace Hymson.MES.Core.Domain.Integrated
{
    /// <summary>
    /// 数据实体（角色、人员资质认证）   
    /// inte_qualification_authentication_details
    /// @author zhaoqing
    /// @date 2024-06-18 06:01:19
    /// </summary>
    public class InteQualificationAuthenticationDetailsEntity : BaseEntity
    {
        /// <summary>
        /// 站点ID
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 类型 1、人员 2、角色
        /// </summary>
        public QualificationAuthenticationTypeEnum Type { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string? UserNames { get; set; }

        /// <summary>
        /// 角色Id
        /// </summary>
        public string? RoleIds { get; set; }

        public long AuthenticationId { get; set; }

        /// <summary>
        /// 有效期
        /// </summary>
        public DateTime DueDate { get; set; }
    }
}
