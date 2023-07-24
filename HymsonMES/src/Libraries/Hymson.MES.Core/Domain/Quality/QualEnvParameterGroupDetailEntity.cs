using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Quality
{
    /// <summary>
    /// 环境检验参数项目表，数据实体对象   
    /// qual_env_parameter_group_detail
    /// @author Czhipu
    /// @date 2023-07-22 10:29:09
    /// </summary>
    public class QualEnvParameterGroupDetailEntity : BaseEntity
    {
        /// <summary>
        /// 环境检验参数id
        /// </summary>
        public long ParameterVerifyEnvId { get; set; }

       /// <summary>
        /// 参数id
        /// </summary>
        public long ParameterId { get; set; }

       /// <summary>
        /// 规格上限
        /// </summary>
        public decimal UpperLimit { get; set; }

       /// <summary>
        /// 中心值（均值）
        /// </summary>
        public decimal? CenterValue { get; set; }

       /// <summary>
        /// 规格下限
        /// </summary>
        public decimal LowerLimit { get; set; }

       /// <summary>
        /// 频率
        /// </summary>
        public bool Frequency { get; set; }

       /// <summary>
        /// 录入次数
        /// </summary>
        public int EntryCount { get; set; }

       /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

       
    }
}
