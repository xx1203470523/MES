using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Integrated
{
    /// <summary>
    /// 异常事件数据实体对象
    ///
    /// @author admin
    /// @date 2023-02-08
    /// </summary>
    public class InteAbnormalEventEntity: BaseEntity
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
}