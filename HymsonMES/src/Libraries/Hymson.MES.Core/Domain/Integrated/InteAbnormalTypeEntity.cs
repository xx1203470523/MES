using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Integrated
{
    /// <summary>
    /// 异常类型数据实体对象
    ///
    /// @author admin
    /// @date 2023-02-08
    /// </summary>
    public class InteAbnormalTypeEntity: BaseEntity
    {
        /// <summary>
        /// 描述 :异常类型编码 
        /// 空值 : false  
        /// </summary>
        public string AbnormalTypeCode { get; set; }
        
        /// <summary>
        /// 描述 :异常类型名称 
        /// 空值 : false  
        /// </summary>
        public string AbnormalTypeName { get; set; }
        
        /// <summary>
        /// 描述 :工作中心车间id 
        /// 空值 : false  
        /// </summary>
        public long WorkCenterShopId { get; set; }
        
        /// <summary>
        /// 描述 :所属站点代码 
        /// 空值 : false  
        /// </summary>
        public string SiteCode { get; set; }
        }
}