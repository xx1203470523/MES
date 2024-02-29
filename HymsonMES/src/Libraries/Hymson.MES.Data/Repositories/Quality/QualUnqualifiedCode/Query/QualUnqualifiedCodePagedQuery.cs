using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.QualUnqualifiedCode;

namespace Hymson.MES.Data.Repositories.Quality.QualUnqualifiedCode.Query
{
    /// <summary>
    /// 不合格代码分页参数
    /// @author wangkeming
    /// @date 2023-02-11 04:45:25
    /// </summary>
    public class QualUnqualifiedCodePagedQuery : PagerInfo
    {
        // <summary>
        /// 工厂
        /// </summary>
        public long? SiteId { get; set; }
        // <summary>
        /// 不合格代码
        /// </summary>
        public string? UnqualifiedCode { get; set; }

        /// <summary>
        /// 不合格代码名称
        /// </summary>
        public string? UnqualifiedCodeName { get; set; }

        /// <summary>
        /// 描述 :状态 
        /// 空值 : true  
        /// </summary>
        public SysDataStatusEnum? Status { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public QualUnqualifiedCodeTypeEnum? Type { get; set; }

        /// <summary>
        /// 等级
        /// </summary>
        public QualUnqualifiedCodeDegreeEnum? Degree { get; set; }


-
        /// <summary>
        /// 排序
        /// </summary>
        new public string Sorting { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public long? Id { get; set; }

        /// <summary>
        /// 组
        /// </summary>
        public IEnumerable<long>? Ids { get; set; }

        /// <summary>
        /// 所属站点代码组
        /// </summary>
        public IEnumerable<long>? SiteIds { get; set; }

        /// <summary>
        /// 不合格代码模糊条件
        /// </summary>
        public string? UnqualifiedCodeLike { get; set; }

        /// <summary>
        /// 不合格代码名称模糊条件
        /// </summary>
        public string? UnqualifiedCodeNameLike { get; set; }

        /// <summary>
        /// 状态模糊条件
        /// </summary>
        public string? StatusLike { get; set; }

        /// <summary>
        /// 类型模糊条件
        /// </summary>
        public string? TypeLike { get; set; }

        /// <summary>
        /// 等级模糊条件
        /// </summary>
        public string? DegreeLike { get; set; }


        /// <summary>
        /// 不合格工艺路线（所属工艺路线ID）
        /// </summary>
        public string? ProcessRouteId { get; set; }

        /// <summary>
        /// 不合格工艺路线（所属工艺路线ID）模糊条件
        /// </summary>
        public string? ProcessRouteIdLike { get; set; }


        /// <summary>
        /// 说明
        /// </summary>
        public string? Remark { get; set; }

        /// <summary>
        /// 说明模糊条件
        /// </summary>
        public string? RemarkLike { get; set; }


        /// <summary>
        /// 创建人
        /// </summary>
        public string? CreatedBy { get; set; }

        /// <summary>
        /// 创建人模糊条件
        /// </summary>
        public string? CreatedByLike { get; set; }


        /// <summary>
        /// 创建时间开始日期
        /// </summary>
        public DateTime? CreatedOnStart { get; set; }

        /// <summary>
        /// 创建时间结束日期
        /// </summary>
        public DateTime? CreatedOnEnd { get; set; }


        /// <summary>
        /// 最后修改人
        /// </summary>
        public string? UpdatedBy { get; set; }

        /// <summary>
        /// 最后修改人模糊条件
        /// </summary>
        public string? UpdatedByLike { get; set; }


        /// <summary>
        /// 修改时间开始日期
        /// </summary>
        public DateTime? UpdatedOnStart { get; set; }

        /// <summary>
        /// 修改时间结束日期
        /// </summary>
        public DateTime? UpdatedOnEnd { get; set; }
    }
}
