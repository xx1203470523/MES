using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Integrated
{
    /// <summary>
    /// 消息通知表数据实体对象
    /// @author admin
    /// @date 2023-02-08
    /// </summary>
    public class InteMessageNotifyEntity: BaseEntity
    {
        /// <summary>
        /// 描述 :消息来源 
        /// 空值 : false  
        /// </summary>
        public string Source { get; set; }
        
        /// <summary>
        /// 描述 :消息类型 
        /// 空值 : false  
        /// </summary>
        public string MessageType { get; set; }
        
        /// <summary>
        /// 描述 :消息标题 
        /// 空值 : false  
        /// </summary>
        public string Title { get; set; }
        
        /// <summary>
        /// 描述 :消息内容 
        /// 空值 : false  
        /// </summary>
        public string Context { get; set; }
        
        /// <summary>
        /// 描述 :通知类型（字典类型：inte_message_notify_type） 
        /// 空值 : false  
        /// </summary>
        public int NotifyType { get; set; }
        
        /// <summary>
        /// 描述 :消息状态（字典类型：inte_message_status） 
        /// 空值 : false  
        /// </summary>
        public string MessageStatus { get; set; }
        
        /// <summary>
        /// 描述 :备注 
        /// 空值 : false  
        /// </summary>
        public string Remark { get; set; } = "";
        
        /// <summary>
        /// 描述 :所属站点代码 
        /// 空值 : false  
        /// </summary>
        public long SiteId { get; set; }
        }
}