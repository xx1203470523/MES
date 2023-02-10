using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Integrated
{
    /// <summary>
    /// 数据实体对象（工作中心）
    /// @author Czhipu
    /// @date 2022-10-09
    /// </summary>
    public class InteWorkCenter : BaseEntity
    {
        /// <summary>
        /// 描述 :所属站点代码 
        /// 空值 : false  
        /// </summary>
        public string SiteCode { get; set; }
        /// <summary>
        /// 描述 :代码 
        /// 空值 : false  
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 描述 :名称 
        /// 空值 : false  
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 描述 :状态 
        /// 空值 : true  
        /// </summary>
        public string Status { get; set; }
        /// <summary>
        /// 描述 :类型 
        /// 空值 : true  
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 描述 :来源 
        /// 空值 : true  
        /// </summary>
        public string Source { get; set; }
        /// <summary>
        /// 描述 :是否混线
        /// 空值 : 0  
        /// </summary>
        public bool IsMixLine { get; set; }
    }
}
