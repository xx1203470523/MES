using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.QualUnqualifiedCode;

namespace Hymson.MES.Core.Domain.Quality
{
    /// <summary>
    /// 不合格代码表数据实体对象
    /// @author admin
    /// @date 2023-02-08
    /// </summary>
    public class QualUnqualifiedCodeEntity : BaseEntity
    {
        /// <summary>
        /// 描述 :所属站点代码 
        /// 空值 : false  
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 描述 :不合格代码 
        /// 空值 : false  
        /// </summary>
        public string UnqualifiedCode { get; set; }

        /// <summary>
        /// 描述 :不合格代码名称 
        /// 空值 : false  
        /// </summary>
        public string UnqualifiedCodeName { get; set; }

        /// <summary>
        /// 描述 :状态 
        /// 空值 : true  
        /// </summary>
        public SysDataStatusEnum Status { get; set; }

        /// <summary>
        /// 描述 :类型 
        /// 空值 : true  
        /// </summary>
        public QualUnqualifiedCodeTypeEnum Type { get; set; }


        /// <summary>
        /// 描述 :等级 
        /// 空值 : true  
        /// </summary>
        public QualUnqualifiedCodeDegreeEnum Degree { get; set; }

        /// <summary>
        /// 描述 :不合格工艺路线（所属工艺路线ID） 
        /// 空值 : true  
        /// </summary>
        public long? ProcessRouteId { get; set; }

        /// <summary>
        /// 描述 :说明 
        /// 空值 : true  
        /// </summary>
        public string Remark { get; set; } = "";
    }
}