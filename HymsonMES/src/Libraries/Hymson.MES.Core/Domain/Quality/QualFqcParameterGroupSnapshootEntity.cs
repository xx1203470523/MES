using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Quality;

namespace Hymson.MES.Core.Domain.Quality
{
    /// <summary>
    /// 数据实体（FQC检验参数组快照）   
    /// qual_fqc_parameter_group_snapshoot
    /// @author Jam
    /// @date 2024-03-27 04:05:59
    /// </summary>
    public class QualFqcParameterGroupSnapshootEntity : BaseEntity
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 参数集编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 参数集名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 产品Id
        /// </summary>
        public long MaterialId { get; set; }

        /// <summary>
        /// 样本数量
        /// </summary>
        public int SampleQty { get; set; }

        /// <summary>
        /// 批次数量
        /// </summary>
        public int LotSize { get; set; }

        /// <summary>
        /// 批次单位
        /// </summary>
        public FQCLotUnitEnum LotUnit { get; set; }

        /// <summary>
        /// 是否同工单
        /// </summary>
        public TrueOrFalseEnum IsSameWorkOrder { get; set; }

        /// <summary>
        /// 是否同产线
        /// </summary>
        public TrueOrFalseEnum IsSameWorkCenter { get; set; }

        /// <summary>
        /// 版本
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public SysDataStatusEnum Status { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}
