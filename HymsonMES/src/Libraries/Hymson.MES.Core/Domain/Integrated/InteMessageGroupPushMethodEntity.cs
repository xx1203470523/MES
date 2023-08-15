using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;
using Hymson.MessagePush.Enum;

namespace Hymson.MES.Core.Domain.Integrated
{
    /// <summary>
    /// 数据实体（消息组推送方式）   
    /// inte_message_group_push_method
    /// @author Czhipu
    /// @date 2023-08-02 09:34:42
    /// </summary>
    public class InteMessageGroupPushMethodEntity : BaseEntity
    {
        /// <summary>
        /// 站点ID
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 消息组Id
        /// </summary>
        public long MessageGroupId { get; set; }

        /// <summary>
        /// 推送类型;1、企微2、钉钉3、邮箱
        /// </summary>
        public MessageTypeEnum Type { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 秘钥
        /// </summary>
        public string SecretKey { get; set; }

        /// <summary>
        /// 关键词
        /// </summary>
        public string KeyWord { get; set; }


    }
}
