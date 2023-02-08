using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Integrated
{
    /// <summary>
    /// 异常类型接收升级数据实体对象
    /// @author admin
    /// @date 2023-02-08
    /// </summary>
    public class InteAbnormalTypeUpgradeEntity: BaseEntity
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
        /// 描述 :消息类型，只能从异常消息组选择已经配置了的类型（字典类型：inte_abnormal_message_type） 
        /// 空值 : false  
        /// </summary>
        public string MessageType { get; set; }
        
        /// <summary>
        /// 描述 :启用状态 
        /// 空值 : false  
        /// </summary>
        public byte UseStatus { get; set; }
        
        /// <summary>
        /// 描述 :等级（字典类型：inte_abnormal_grade） 
        /// 空值 : false  
        /// </summary>
        public int Grade { get; set; }
        
        /// <summary>
        /// 描述 :升级时长（分钟） 
        /// 空值 : false  
        /// </summary>
        public int UpgradeTime { get; set; }
        
        /// <summary>
        /// 描述 :升级类型（0-接收升级 1-关闭升级）（字典类型：inte_abnormal_upgrade_type） 
        /// 空值 : false  
        /// </summary>
        public byte UpgradeType { get; set; }
        
        /// <summary>
        /// 描述 :备注 
        /// 空值 : false  
        /// </summary>
        public string Remark { get; set; }
        
        /// <summary>
        /// 描述 :所属站点代码 
        /// 空值 : false  
        /// </summary>
        public string SiteCode { get; set; }
        }
}