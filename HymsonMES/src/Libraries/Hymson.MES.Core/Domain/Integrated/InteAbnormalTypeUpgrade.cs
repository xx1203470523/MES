using Hymson.Infrastructure;

namespace  Hymson.MES.Core.Domain.Integrated
{
    /// <summary>
    /// 数据实体对象（异常类型升级）
    /// @author zhaoqing
    /// @date 2022-11-18
    /// </summary>
    public class InteAbnormalTypeUpgrade : BaseEntity
    {
        /// <summary>
        /// 描述 :所属站点代码 
        /// 空值 : false  
        /// </summary>
        public string SiteCode { get; set; }
        /// <summary>
        /// 异常类型Id（inte_abnormal_type表id）
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
        public int UseStatus { get; set; }
        /// <summary>
        /// 描述 :等级 
        /// 空值 : false  
        /// </summary>
        public int Grade { get; set; }
        /// <summary>
        /// 描述 :升级时长（分钟） 
        /// 空值 : false  
        /// </summary>
        public int UpgradeTime { get; set; }
        /// <summary>
        /// 描述 :升级类型（0-接收升级 1-关闭升级） 
        /// 空值 : false  
        /// </summary>
        public int UpgradeType { get; set; }
    }
}
