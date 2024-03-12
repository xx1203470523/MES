using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Quality;

namespace Hymson.MES.Core.Domain.Quality
{
    /// <summary>
    /// 数据实体（OQC检验参数组明细快照）   
    /// qual_oqc_parameter_group_detail_snapshoot
    /// @author xiaofei
    /// @date 2024-03-04 10:53:05
    /// </summary>
    public class QualOqcParameterGroupDetailSnapshootEntity : BaseEntity
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// OQC检验参数组Id
        /// </summary>
        public long ParameterGroupId { get; set; }

        /// <summary>
        /// 标准参数Id
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
        /// 参数数据类型
        /// </summary>
        public DataTypeEnum ParameterDataType { get; set; }

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
        public string ReferenceValue { get; set; }

        /// <summary>
        /// 检验类型
        /// </summary>
        public OQCInspectionTypeEnum InspectionType { get; set; }

        /// <summary>
        /// 录入次数
        /// </summary>
        public int EnterNumber { get; set; }

        /// <summary>
        /// 显示顺序
        /// </summary>
        public int DisplayOrder { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}
