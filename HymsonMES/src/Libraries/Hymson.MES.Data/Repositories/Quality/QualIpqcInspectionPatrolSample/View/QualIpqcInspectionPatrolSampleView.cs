using Hymson.MES.Core.Domain.Quality;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.Data.Repositories.Quality.View
{
    public class QualIpqcInspectionPatrolSampleView: QualIpqcInspectionPatrolSampleEntity
    {
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
        /// 参数类型
        /// </summary>
        public DataTypeEnum DataType { get; set; }

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
        public YesOrNoEnum? IsDeviceCollect { get; set; } = YesOrNoEnum.No;

        /// <summary>
        /// 顺序
        /// </summary>
        public int Sequence { get; set; }
    }
}
