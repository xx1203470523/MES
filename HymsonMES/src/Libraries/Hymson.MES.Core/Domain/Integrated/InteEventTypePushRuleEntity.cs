using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Integrated
{
    /// <summary>
    /// 数据实体（事件类型推送规则）   
    /// inte_event_type_push_rule
    /// @author Czhipu
    /// @date 2023-08-07 01:37:53
    /// </summary>
    public class InteEventTypePushRuleEntity : BaseEntity
    {
        /// <summary>
        /// 站点ID
        /// </summary>
        public long SiteId { get; set; }

       /// <summary>
        /// 事件类型id
        /// </summary>
        public long? EventTypeId { get; set; }

       /// <summary>
        /// 推送场景;1、触发2、接收3、接收升级4、处理5、处理升级6、关闭
        /// </summary>
        public bool PushScene { get; set; }

       
    }
}
