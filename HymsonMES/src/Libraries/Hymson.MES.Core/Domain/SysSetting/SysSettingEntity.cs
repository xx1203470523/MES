using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.SysSetting
{
    /// <summary>
    /// 数据实体（配置设置）   
    /// sys_setting
    /// @author jesenz
    /// @date 2023-10-16 09:07:30
    /// </summary>
    public class SysSettingEntity : BaseEntity
    {
        /// <summary>
        /// 配置key
        /// </summary>
        public string Name { get; set; }

       /// <summary>
        /// 配置value
        /// </summary>
        public string Value { get; set; }

       /// <summary>
        /// 站点id
        /// </summary>
        public long SiteId { get; set; }

       
    }
}
