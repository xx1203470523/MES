using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.Core.Domain.Process
{
    /// <summary>
    /// BOM明细替代料表数据实体对象
    /// @author admin
    /// @date 2023-02-08
    /// </summary>
    public class ProcBomDetailReplaceMaterialEntity: BaseEntity
    {
        /// <summary>
        /// 描述 :所属站点代码 
        /// 空值 : false  
        /// </summary>
        public long SiteId { get; set; }
        
        /// <summary>
        /// 描述 :所属BomID 
        /// 空值 : false  
        /// </summary>
        public long BomId { get; set; }
        
        /// <summary>
        /// 描述 :所属BOM明细表ID 
        /// 空值 : false  
        /// </summary>
        public long BomDetailId { get; set; }
        
        /// <summary>
        /// 描述 :所属BOM替代物料ID（从物料表选择） 
        /// 空值 : true  
        /// </summary>
        public long ReplaceMaterialId { get; set; }
        
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
        
        /// <summary>
        /// 描述 :说明 
        /// 空值 : true  
        /// </summary>
        public string Remark { get; set; }


        /// <summary>
        /// 数据收集方式 
        /// </summary>
        public MaterialSerialNumberEnum? DataCollectionWay { get; set; }
    }
}