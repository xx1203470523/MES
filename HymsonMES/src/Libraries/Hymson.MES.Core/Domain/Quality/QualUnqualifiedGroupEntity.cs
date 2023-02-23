using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Quality
{
    /// <summary>
    /// 不合格组表数据实体对象
    /// @author admin
    /// @date 2023-02-08
    /// </summary>
    public class QualUnqualifiedGroupEntity : BaseEntity
    {
        /// <summary>
        /// 描述 :所属站点代码 
        /// 空值 : false  
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 描述 :不合格组
        /// 空值 : false  
        /// </summary>
        public string UnqualifiedGroup { get; set; }

        /// <summary>
        /// 描述 :不合格组名称 
        /// 空值 : false  
        /// </summary>
        public string UnqualifiedGroupName { get; set; }

        /// <summary>
        /// 描述 :说明 
        /// 空值 : true  
        /// </summary>
        public string Remark { get; set; }
    }
}