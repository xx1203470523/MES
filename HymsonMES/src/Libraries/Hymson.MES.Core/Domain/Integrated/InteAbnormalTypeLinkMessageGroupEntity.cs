using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Integrated
{
    /// <summary>
    /// 异常类型关联异常消息组数据实体对象
    /// @author admin
    /// @date 2023-02-08
    /// </summary>
    public class InteAbnormalTypeLinkMessageGroupEntity: BaseEntity
    {
        /// <summary>
        /// 描述 :异常类型Id（inte_abnormal_type表id） 
        /// 空值 : false  
        /// </summary>
        public long AbnormalTypeId { get; set; }
        
        /// <summary>
        /// 描述 :异常消息组id（inte_abnormal_message_group） 
        /// 空值 : false  
        /// </summary>
        public long AbnormalMessageGroupId { get; set; }
        
        /// <summary>
        /// 描述 :消息类型，只能从异常消息组选择已经配置了的类型 
        /// 空值 : false  
        /// </summary>
        public string MessageType { get; set; }
        
        /// <summary>
        /// 描述 :启用状态 
        /// 空值 : false  
        /// </summary>
        public byte UseStatus { get; set; }
        
        /// <summary>
        /// 描述 :备注 
        /// 空值 : true  
        /// </summary>
        public string Remark { get; set; }
        
        /// <summary>
        /// 描述 :所属站点代码 
        /// 空值 : false  
        /// </summary>
        public long SiteId { get; set; }
        }
}