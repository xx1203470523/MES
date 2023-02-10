using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Integrated
{
    /// <summary>
    /// 异常消息组类型数据实体对象
    /// @author admin
    /// @date 2023-02-08
    /// </summary>
    public class InteAbnormalMessageGroupDetailEntity: BaseEntity
    {
        /// <summary>
        /// 描述 :异常消息组id 
        /// 空值 : false  
        /// </summary>
        public long AbnormalMessageGroupId { get; set; }
        
        /// <summary>
        /// 描述 :消息类型（企业微信，钉钉，邮箱，腕表）（字典类型：inte_abnormal_message_type） 
        /// 空值 : false  
        /// </summary>
        public string MessageType { get; set; }
        
        /// <summary>
        /// 描述 :推送目标（企业微信和钉钉为推送url，邮箱填邮箱地址，腕表填角色权限字符） 
        /// 空值 : false  
        /// </summary>
        public string PushTarget { get; set; }
        
        /// <summary>
        /// 描述 :密钥（类型为企业微信和钉钉可能会用到） 
        /// 空值 : false  
        /// </summary>
        public string SecretKey { get; set; }
        
        /// <summary>
        /// 描述 :关键词（类型为企业微信和钉钉用到） 
        /// 空值 : false  
        /// </summary>
        public string KeyWord { get; set; }
        
        /// <summary>
        /// 描述 :备注 
        /// 空值 : true  
        /// </summary>
        public string Remark { get; set; }
        
        /// <summary>
        /// 描述 :所属站点代码 
        /// 空值 : false  
        /// </summary>
        public string SiteCode { get; set; }
        }
}