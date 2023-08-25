using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.Core.Domain.Quality
{
    /// <summary>
    /// 数据实体（尾检检验单样本）   
    /// qual_ipqc_inspection_tail_sample
    /// @author xiaofei
    /// @date 2023-08-24 10:52:14
    /// </summary>
    public class QualIpqcInspectionTailSampleEntity : BaseEntity
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 首检检验单Id
        /// </summary>
        public long IpqcInspectionTailId { get; set; }

        /// <summary>
        /// IPQC检验项目参数表Id
        /// </summary>
        public long IpqcInspectionParameterId { get; set; }

        /// <summary>
        /// 全检参数明细Id
        /// </summary>
        public long InspectionParameterGroupDetailId { get; set; }

        /// <summary>
        /// proc_parameter 参数Id
        /// </summary>
        public long ParameterId { get; set; }

        /// <summary>
        /// 条码
        /// </summary>
        public string Barcode { get; set; }

        /// <summary>
        /// 检验值
        /// </summary>
        public string InspectionValue { get; set; }

        /// <summary>
        /// 是否合格;0、不合格 1、合格
        /// </summary>
        public TrueOrFalseEnum IsQualified { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string? Remark { get; set; }


    }
}
