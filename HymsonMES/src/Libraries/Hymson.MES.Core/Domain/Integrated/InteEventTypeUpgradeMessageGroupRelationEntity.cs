using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.Core.Domain.Integrated
{
    /// <summary>
    /// 数据实体（事件升级消息组关联表）   
    /// inte_event_type_upgrade_message_group_relation
    /// @author Czhipu
    /// @date 2023-08-07 02:35:12
    /// </summary>
    public class InteEventTypeUpgradeMessageGroupRelationEntity : BaseEntity
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
        /// 事件类型id
        /// </summary>
        public long EventTypeUpgradeId { get; set; }

        /// <summary>
        /// 消息组
        /// </summary>
        public long MessageGroupId { get; set; }

        /// <summary>
        /// 推送类型;1、企微2、钉钉3、邮箱
        /// </summary>
        public string PushTypes { get; set; }

        /// <summary>
        /// 站点ID
        /// </summary>
        public long SiteId { get; set; }


    }
}
