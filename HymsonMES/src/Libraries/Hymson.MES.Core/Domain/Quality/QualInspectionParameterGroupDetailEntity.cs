using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Quality
{
    /// <summary>
    /// 数据实体（全检参数项目表）   
    /// qual_inspection_parameter_group_detail
    /// @author Czhipu
    /// @date 2023-07-25 02:14:42
    /// </summary>
    public class QualInspectionParameterGroupDetailEntity : BaseEntity
    {
        /// <summary>
        /// 全检检验参数id
        /// </summary>
        public long ParameterGroupId { get; set; }

        /// <summary>
        /// 参数id（产品参数）
        /// </summary>
        public long ParameterId { get; set; }

        /// <summary>
        /// 规格上限
        /// </summary>
        public decimal? UpperLimit { get; set; }

        /// <summary>
        /// 中心值（均值）
        /// </summary>
        public decimal? CenterValue { get; set; }

        /// <summary>
        /// 规格下限
        /// </summary>
        public decimal? LowerLimit { get; set; }

        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }


    }
}
