using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Process
{
    /// <summary>
    /// 标准参数关联类型表数据实体对象
    /// @author admin
    /// @date 2023-02-08
    /// </summary>
    public class ProcParameterLinkTypeEntity: BaseEntity
    {
        /// <summary>
        /// 描述 :所属站点代码 
        /// 空值 : false  
        /// </summary>
        public string SiteCode { get; set; }
        
        /// <summary>
        /// 描述 :标准参数ID 
        /// 空值 : false  
        /// </summary>
        public long ParameterID { get; set; }
        
        /// <summary>
        /// 描述 :参数类型 
        /// 空值 : false  
        /// </summary>
        public int ParameterType { get; set; }
        
        /// <summary>
        /// 描述 :说明 
        /// 空值 : true  
        /// </summary>
        public string Remark { get; set; }
        }
}