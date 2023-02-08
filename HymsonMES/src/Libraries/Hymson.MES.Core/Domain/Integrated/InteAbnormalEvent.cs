using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Integrated
{
    /// <summary>
    /// 异常事件，数据实体对象
    /// @author luoxichang
    /// @date 2022-11-18
    /// </summary>
    public class InteAbnormalEvent : BaseEntity
    {
        /// <summary>
        /// 描述 :异常事件编码 
        /// 空值 : false  
        /// </summary>
        public string AbnormalEventCode { get; set; }

        /// <summary>
        /// 描述 :异常事件名称 
        /// 空值 : false  
        /// </summary>
        public string AbnormalEventName { get; set; }

        /// <summary>
        /// 描述 :异常类型id（inte_abnormal_type表id） 
        /// 空值 : false  
        /// </summary>
        public long AbnormalTypeId { get; set; }

        /// <summary>
        /// 描述 :所属站点代码 
        /// 空值 : false  
        /// </summary>
        public string SiteCode { get; set; }
    }
    public class CustomInteAbnormalEvent : InteAbnormalEvent
    {
        /// <summary>
        /// 描述 :异常事件类型名称 
        /// 空值 : false  
        /// </summary>
        public string AbnormalTypeName { get; set; }
    }
}