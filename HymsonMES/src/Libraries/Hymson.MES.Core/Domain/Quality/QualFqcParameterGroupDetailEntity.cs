using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.Core.Domain.Quality
{
    /// <summary>
    /// 数据实体（FQC检验参数组明细）   
    /// qual_fqc_parameter_group_detail
    /// @author xiaofei
    /// @date 2024-03-28 08:32:33
    /// </summary>
    public class QualFqcParameterGroupDetailEntity : BaseEntity
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long? SiteId { get; set; }

        /// <summary>
        /// FQC检验参数组Id
        /// </summary>
        public long? ParameterGroupId { get; set; }

        /// <summary>
        /// 标准参数Id
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
        /// 参考值
        /// </summary>
        public decimal? ReferenceValue { get; set; }

        /// <summary>
        /// 录入次数
        /// </summary>
        public int? EnterNumber { get; set; }

        /// <summary>
        /// 是否设备采集
        /// </summary>
        public TrueOrFalseEnum IsDeviceCollect { get; set; }

        /// <summary>
        /// 顺序
        /// </summary>
        public int? DisplayOrder { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string? Remark { get; set; }
    }
}
