/*
 *creator: Karl
 *
 *describe: 环境检验参数组明细快照    实体类 | 代码由框架生成  如果数据库字段发生变化,则手动调整
 *builder:  pengxin
 *build datetime: 2024-03-26 02:04:29
 */
using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.Core.Domain.Quality
{
    /// <summary>
    /// 环境检验参数组明细快照，数据实体对象   
    /// qual_env_parameter_group_detail_snapshoot
    /// @author pengxin
    /// @date 2024-03-26 02:04:29
    /// </summary>
    public class QualEnvParameterGroupDetailSnapshootEntity : BaseEntity
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

       /// <summary>
        /// 环境检验参数组Id
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
        /// 参数类型
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
        /// 录入次数
        /// </summary>
        public int EnterNumber { get; set; }

       /// <summary>
        /// 频率
        /// </summary>
        public FrequencyEnum? Frequency { get; set; }

       
    }
}
