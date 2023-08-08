using Hymson.MES.Core.Enums;

namespace Hymson.MES.Data.Repositories.Integrated.Query
{
    /// <summary>
    /// 事件升级消息组关联表 查询参数
    /// </summary>
    public class InteEventTypeUpgradeMessageGroupRelationQuery
    {
        /// <summary>
        /// 工厂
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 事件类型ID
        /// </summary>
        public long EventTypeId { get; set; }

        /// <summary>
        /// 升级场景;1、触发2、接收3、接收升级4、处理5、处理升级6、关闭
        /// </summary>
        public PushSceneEnum PushScene { get; set; }
    }
}
