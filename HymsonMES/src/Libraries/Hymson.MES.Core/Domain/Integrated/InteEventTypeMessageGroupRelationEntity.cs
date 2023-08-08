using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.Core.Domain.Integrated
{
    /// <summary>
    /// 数据实体（事件类型关联群组）   
    /// inte_event_type_message_group_relation
    /// @author Czhipu
    /// @date 2023-08-07 01:38:11
    /// </summary>
    public class InteEventTypeMessageGroupRelationEntity : BaseEntity
    {
        /// <summary>
        /// 站点ID
        /// </summary>
        public long SiteId { get; set; }

       /// <summary>
        /// 事件类型id
        /// </summary>
        public long EventTypeId { get; set; }

       /// <summary>
        /// 消息组id
        /// </summary>
        public long MessageGroupId { get; set; }

        /// <summary>
        /// 推送类型;1、企微2、钉钉3、邮箱
        /// </summary>
        public string PushTypes { get; set; }


    }
}
