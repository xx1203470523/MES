using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.Core.Domain.Integrated
{
    /// <summary>
    /// 数据实体（事件升级）   
    /// inte_event_type_upgrade
    /// @author Czhipu
    /// @date 2023-08-07 02:34:44
    /// </summary>
    public class InteEventTypeUpgradeEntity : BaseEntity
    {
        /// <summary>
        /// 事件类型id
        /// </summary>
        public long EventTypeId { get; set; }

       /// <summary>
        /// 升级场景;1、触发2、接收3、接收升级4、处理5、处理升级6、关闭
        /// </summary>
        public PushSceneEnum PushScene { get; set; }

       /// <summary>
        /// 级别;1、第一等级2、第二等级3、第三等级
        /// </summary>
        public UpgradeLevelEnum Level { get; set; }

       /// <summary>
        /// 升级时长
        /// </summary>
        public int Duration { get; set; }

       /// <summary>
        /// 站点ID
        /// </summary>
        public long SiteId { get; set; }

       
    }
}
