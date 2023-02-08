using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Integrated
{
    /// <summary>
    /// 作业表数据实体对象
    /// @author admin
    /// @date 2023-02-08
    /// </summary>
    public class InteJobEntity: BaseEntity
    {
        /// <summary>
        /// 描述 :所属站点代码 
        /// 空值 : false  
        /// </summary>
        public string SiteCode { get; set; }
        
        /// <summary>
        /// 描述 :作业编号 
        /// 空值 : false  
        /// </summary>
        public string JobCode { get; set; }
        
        /// <summary>
        /// 描述 :作业名称 
        /// 空值 : false  
        /// </summary>
        public string JobName { get; set; }
        
        /// <summary>
        /// 描述 :类程序 
        /// 空值 : true  
        /// </summary>
        public string ClassProgram { get; set; }
        
        /// <summary>
        /// 描述 :参数说明 
        /// 空值 : false  
        /// </summary>
        public string ParameterDescribe { get; set; }
        
        /// <summary>
        /// 描述 :备注 
        /// 空值 : true  
        /// </summary>
        public string Remark { get; set; }
        }
}