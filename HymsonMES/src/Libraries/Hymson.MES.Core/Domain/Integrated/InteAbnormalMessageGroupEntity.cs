using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Integrated
{
    /// <summary>
    /// 异常消息组数据实体对象
    ///
    /// @author admin
    /// @date 2023-02-08
    /// </summary>
    public class InteAbnormalMessageGroupEntity: BaseEntity
    {
        /// <summary>
        /// 描述 :异常消息组编码 
        /// 空值 : false  
        /// </summary>
        public string AbnormalMessageGroupCode { get; set; }
        
        /// <summary>
        /// 描述 :异常消息组名 
        /// 空值 : false  
        /// </summary>
        public string AbnormalMessageGroupName { get; set; }
        
        /// <summary>
        /// 描述 :工作中心车间id 
        /// 空值 : false  
        /// </summary>
        public long WorkCenterShopId { get; set; }
        
        /// <summary>
        /// 描述 :已经存在消息类型（一个异常消息组只能有一种异常消息类型，用于校验消息类型是否存在） 
        /// 空值 : false  
        /// </summary>
        public string ExistMessageType { get; set; }
        
        /// <summary>
        /// 描述 :所属站点代码 
        /// 空值 : false  
        /// </summary>
        public string SiteCode { get; set; }
        }
}