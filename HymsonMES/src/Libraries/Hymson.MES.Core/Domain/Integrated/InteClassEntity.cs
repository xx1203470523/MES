using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Integrated
{
    /// <summary>
    /// 生产班次数据实体对象
    /// @author admin
    /// @date 2023-02-08
    /// </summary>
    public class InteClassEntity : BaseEntity
    {
        /// <summary>
        /// 描述 :班次名称 
        /// 空值 : false  
        /// </summary>
        public string ClassName { get; set; }

        /// <summary>
        /// 描述 :班次类型（字典名称：manu_class_type） 
        /// 空值 : false  
        /// </summary>
        public string ClassType { get; set; }

        /// <summary>
        /// 描述 :描述 
        /// 空值 : true  
        /// </summary>
        public string Remark { get; set; }
        
        /// <summary>
        /// 描述 :所属站点代码 
        /// 空值 : false  
        /// </summary>
        public string SiteCode { get; set; }
    }
}