using Hymson.Infrastructure;

namespace  Hymson.MES.Core.Domain.Integrated
{
    /// <summary>
    /// 数据实体对象（异常类型关联异常消息组）
    /// @author zhaoqing
    /// @date 2022-11-18
    /// </summary>
    public class InteAbnormalTypeLinkMessageGroup : BaseEntity
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
        /// 消息类型，只能从异常消息组选择已经配置了的类型
        /// </summary>
        public string MessageType { get; set; }

        /// <summary>
        /// 启用状态
        /// </summary>
        public int UseStatus { get; set; }
    }
}
