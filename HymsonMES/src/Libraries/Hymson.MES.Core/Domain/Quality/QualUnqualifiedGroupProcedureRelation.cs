using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Quality
{
    /// <summary>
    /// 不合格组关联工序表数据实体对象
    /// @author admin
    /// @date 2023-02-08
    /// </summary>
    public class QualUnqualifiedGroupProcedureRelation : BaseEntity
    {
        /// <summary>
        /// 描述 :所属站点代码 
        /// 空值 : false  
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 描述 :所属不合格组ID 
        /// 空值 : false  
        /// </summary>
        public long UnqualifiedGroupId { get; set; }

        /// <summary>
        /// 描述 :所属工序ID 
        /// 空值 : false  
        /// </summary>
        public long ProcedureId { get; set; }

        /// <summary>
        /// 描述 :说明 
        /// 空值 : true  
        /// </summary>
        public string Remark { get; set; } = "";
    }
}