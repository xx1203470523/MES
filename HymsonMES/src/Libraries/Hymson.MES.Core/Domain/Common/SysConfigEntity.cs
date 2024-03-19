using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.Core.Domain.Common
{
    /// <summary>
    /// 数据实体（配置）   
    /// sys_config
    /// @author Czhipu
    /// @date 2024-01-02 02:16:34
    /// </summary>
    public class SysConfigEntity : BaseEntity
    {
        /// <summary>
        /// 配置类型
        /// </summary>
        public SysConfigEnum Type { get; set; }

        /// <summary>
        /// 配置编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 配置value
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// 站点id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; } = "";

    }
}
