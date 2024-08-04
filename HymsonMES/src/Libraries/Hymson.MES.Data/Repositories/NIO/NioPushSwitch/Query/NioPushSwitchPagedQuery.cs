using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Mavel;

namespace Hymson.MES.Data.Repositories.NioPushSwitch.Query
{
    /// <summary>
    /// 蔚来推送开关 分页参数
    /// </summary>
    public class NioPushSwitchPagedQuery : PagerInfo
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 业务场景;0：表示总开关；
        /// </summary>
        public BuzSceneEnum? BuzScene { get; set; }

        /// <summary>
        /// 是否启用推送;0：不推送；1：推送；
        /// </summary>
        public TrueOrFalseEnum? IsEnabled { get; set; }

    }
}
