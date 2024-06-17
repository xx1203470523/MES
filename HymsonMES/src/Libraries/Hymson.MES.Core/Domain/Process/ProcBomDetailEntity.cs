using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Manufacture;

namespace Hymson.MES.Core.Domain.Process
{
    /// <summary>
    /// BOM明细表数据实体对象
    /// @author admin
    /// @date 2023-02-08
    /// </summary>
    public class ProcBomDetailEntity: BaseEntity
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
        /// 描述 :所属工序ID 
        /// 空值 : false  
        /// </summary>
        public long ProcedureId { get; set; }
        
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
        public decimal? Loss { get; set; }

        /// <summary>
        /// 描述 :说明 
        /// 空值 : true  
        /// </summary>
        public string Remark { get; set; } = "";


        /// <summary>
        /// 数据收集方式 
        /// </summary>
        public MaterialSerialNumberEnum? DataCollectionWay { get; set; }

        /// <summary>
        /// 是否启用替代物料
        /// </summary>
        public bool IsEnableReplace { get; set; }

        /// <summary>
        /// 序号
        /// </summary>
        public int Seq { get; set; }
        
        /// <summary>
        /// Bom联副产品类型
        /// </summary>

        public ManuProductTypeEnum BomProductType {  get; set; }
    }
}