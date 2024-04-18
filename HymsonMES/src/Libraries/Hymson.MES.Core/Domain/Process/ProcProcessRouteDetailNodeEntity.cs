using Hymson.Infrastructure;
using Hymson.MES.Core.Enums.Process;

namespace Hymson.MES.Core.Domain.Process
{
    /// <summary>
    /// 工艺路线工序节点明细表数据实体对象
    /// @author admin
    /// @date 2023-02-08
    /// </summary>
    public class ProcProcessRouteDetailNodeEntity: BaseEntity
    {
        /// <summary>
        /// 描述 :所属站点代码 
        /// 空值 : false  
        /// </summary>
        public long SiteId { get; set; } = 0;
        
        /// <summary>
        /// 描述 :所属工艺路线ID 
        /// 空值 : false  
        /// </summary>
        public long ProcessRouteId { get; set; }
        
        /// <summary>
        /// 描述 :序号( 程序生成) 
        /// 空值 : true  
        /// </summary>
        public long SerialNo { get; set; }

        /// <summary>
        /// 手动排序号  20230703 海龙说加上这个做排序使用，重复也不管
        /// </summary>
        public string ManualSortNumber { get; set; }

        /// <summary>
        /// 描述 :所属工序ID 
        /// 空值 : false  
        /// </summary>
        public long ProcedureId { get; set; }

        /// <summary>
        /// 描述 :抽检类型 
        /// 空值 : true  
        /// </summary>
        public ProcessRouteInspectTypeEnum? CheckType { get; set; }
        
        /// <summary>
        /// 描述 :抽检比例 
        /// 空值 : true  
        /// </summary>
        public int? CheckRate { get; set; }
        
        /// <summary>
        /// 描述 :是否报工 
        /// 空值 : true  
        /// </summary>
        public int IsWorkReport { get; set; }
        
        /// <summary>
        /// 描述 :包装等级 
        /// 空值 : true  
        /// </summary>
        public string PackingLevel { get; set; }
        
        /// <summary>
        /// 描述 :是否首工序 
        /// 空值 : true  
        /// </summary>
        public int IsFirstProcess { get; set; }
        
        /// <summary>
        /// 描述 :状态 
        /// 空值 : true  
        /// </summary>
        public string Status { get; set; }
        
        /// <summary>
        /// 描述 :扩展字段1(暂存坐标) 
        /// 空值 : true  
        /// </summary>
        public string Extra1 { get; set; }
        
        /// <summary>
        /// 描述 :说明 
        /// 空值 : true  
        /// </summary>
        public string Remark { get; set; } = "";
        }
}