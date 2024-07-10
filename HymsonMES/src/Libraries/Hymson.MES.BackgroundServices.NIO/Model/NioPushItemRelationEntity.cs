using Hymson.Infrastructure;
using Hymson.MES.Core.Enums.Plan;

namespace Hymson.MES.BackgroundServices.NIO
{
    /// <summary>
    /// 数据实体（蔚来推送项目关系表）   
    /// nio_push_item_relation
    /// @author Czhipu
    /// @date 2024-07-10 08:37:36
    /// </summary>
    public class NioPushItemRelationEntity : BaseEntity
    {
        /// <summary>
        /// 业务场景;这里不允许0的数据存在；
        /// </summary>
        public int BuzScene { get; set; }

        /// <summary>
        /// 业务类型;1：转子线；2：定子线；
        /// </summary>
        public BuzTypeEnum BuzType { get; set; }

        /// <summary>
        /// 推送ID;nio_push表ID
        /// </summary>
        public long PushId { get; set; }

        /// <summary>
        /// 推送项目ID;nio_push_Item表ID
        /// </summary>
        public long PushItemId { get; set; }

    }
}
