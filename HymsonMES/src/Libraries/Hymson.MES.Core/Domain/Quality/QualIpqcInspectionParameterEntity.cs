using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.Core.Domain.Quality
{
    /// <summary>
    /// 数据实体（IPQC检验项目参数）   
    /// qual_ipqc_inspection_parameter
    /// @author xiaofei
    /// @date 2023-08-08 11:32:10
    /// </summary>
    public class QualIpqcInspectionParameterEntity : BaseEntity
    {
        /// <summary>
        /// IPQC检验项目Id qual_ipqc_inspection 的id
        /// </summary>
        public long IpqcInspectionId { get; set; }

        /// <summary>
        /// 全检参数明细Id
        /// </summary>
        public long InspectionParameterGroupDetailId { get; set; }

        /// <summary>
        /// proc_parameter 参数Id
        /// </summary>
        public long ParameterId { get; set; }

        /// <summary>
        /// 上限
        /// </summary>
        public decimal? UpperLimit { get; set; }

        /// <summary>
        /// 下限
        /// </summary>
        public decimal? LowerLimit { get; set; }

        /// <summary>
        /// 中心值
        /// </summary>
        public decimal? CenterValue { get; set; }

        /// <summary>
        /// 录入次数
        /// </summary>
        public int? EnterNumber { get; set; }

        /// <summary>
        /// 是否设备采集;1、是 2、否
        /// </summary>
        public YesOrNoEnum? IsDeviceCollect { get; set; }

        /// <summary>
        /// 顺序
        /// </summary>
        public int Sequence { get; set; }


    }
}
