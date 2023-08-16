using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.Data.Repositories.Quality.View
{
    public class QualInspectionParameterGroupDetailView : BaseEntity
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 全检检验参数id
        /// </summary>
        public long ParameterGroupId { get; set; }

        /// <summary>
        /// 参数id（产品参数）
        /// </summary>
        public long ParameterId { get; set; }

        /// <summary>
        /// 参数编码
        /// </summary>
        public string ParameterCode { get; set; }

        /// <summary>
        /// 参数名称
        /// </summary>
        public string ParameterName { get; set; }

        /// <summary>
        /// 参数单位
        /// </summary>
        public string ParameterUnit { get; set; }

        /// <summary>
        /// 数据类型
        /// </summary>
        public DataTypeEnum DataType { get; set; }

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
    }
}
