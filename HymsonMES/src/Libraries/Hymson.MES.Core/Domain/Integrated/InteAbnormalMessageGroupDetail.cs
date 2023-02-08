using Hymson.Infrastructure;

namespace  Hymson.MES.Core.Domain.Integrated
{
    /// <summary>
    /// 数据实体对象（异常消息组明细）
    /// @author Czhipu
    /// @date 2022-11-17
    /// </summary>
    public class InteAbnormalMessageGroupDetail : BaseEntity
    {
        /// <summary>
        /// 所属站点代码 
        /// </summary>
        public string SiteCode { get; set; }

        /// <summary>
        /// 异常消息组id
        /// </summary>
        public long AbnormalMessageGroupId { get; set; }

        /// <summary>
        /// 消息类型（企业微信，钉钉，邮箱，腕表）
        /// </summary>
        public string MessageType { get; set; }

        /// <summary>
        /// 推送目标（企业微信和钉钉为推送url，邮箱填邮箱地址，腕表填角色权限字符） 
        /// </summary>
        public string PushTarget { get; set; }

        /// <summary>
        /// 密钥（类型为企业微信和钉钉可能会用到）  
        /// </summary>
        public string SecretKey { get; set; }

        /// <summary>
        /// 关键词（类型为企业微信和钉钉用到） 
        /// </summary>
        public string KeyWord { get; set; }
    }
}
