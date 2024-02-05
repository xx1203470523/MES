using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;
using Hymson.MessagePush.Enum;

namespace Hymson.MES.Core.Domain.Common
{
    /// <summary>
    /// 数据实体（消息模板）   
    /// message_template
    /// @author Czhipu
    /// @date 2023-08-19 11:01:42
    /// </summary>
    public class MessageTemplateEntity : BaseEntity
    {
        /// <summary>
        /// 推送类型;1、企业微信2、钉钉3、飞书4、邮箱
        /// </summary>
        public MessageTypeEnum MessageType { get; set; }

       /// <summary>
        /// 业务类型;1、异常消息2、待定
        /// </summary>
        public BusinessTypeEnum BusinessType { get; set; }

       /// <summary>
        /// BusinessType 为1时对应的推送场景;1、触发2、接收3、接收升级4、处理5、处理升级6、关闭
        /// </summary>
        public PushSceneEnum PushScene { get; set; }

       /// <summary>
        /// 模板内容
        /// </summary>
        public string Content { get; set; }

       /// <summary>
        /// 站点ID
        /// </summary>
        public long SiteId { get; set; }

       
    }
}
