using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Process
{
    /// <summary>
    /// BOM明细表数据实体对象
    ///
    /// @author admin
    /// @date 2023-02-08
    /// </summary>
    public class ProcBomDetailEntity: BaseEntity
    {
        /// <summary>
        /// 描述 :所属站点代码 
        /// 空值 : false  
        /// </summary>
        public string SiteCode { get; set; }
        
        /// <summary>
        /// 描述 :所属BomID 
        /// 空值 : false  
        /// </summary>
        public long BomId { get; set; }
        
        /// <summary>
        /// 描述 :所属工序ID 
        /// 空值 : false  
        /// </summary>
        public long ProcedureBomId { get; set; }
        
        /// <summary>
        /// 描述 :所属物料ID 
        /// 空值 : false  
        /// </summary>
        public long MaterialId { get; set; }
        
        /// <summary>
        /// 描述 :参考点 
        /// 空值 : true  
        /// </summary>
        public string ReferencePoint { get; set; }
        
        /// <summary>
        /// 描述 :用量 
        /// 空值 : false  
        /// </summary>
        public decimal Usages { get; set; }
        
        /// <summary>
        /// 描述 :损耗 
        /// 空值 : true  
        /// </summary>
        public decimal Loss { get; set; }
        }
}