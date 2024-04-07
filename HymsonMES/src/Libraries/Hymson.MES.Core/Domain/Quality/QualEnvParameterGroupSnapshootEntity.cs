using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.Core.Domain.Quality
{
    /// <summary>
    /// 数据实体（环境检验参数组快照）   
    /// qual_env_parameter_group_snapshoot
    /// @author xiaofei
    /// @date 2024-04-02 09:21:41
    /// </summary>
    public class QualEnvParameterGroupSnapshootEntity : BaseEntity
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
        /// 工作中心Id
        /// </summary>
        public long WorkCenterId { get; set; }

        /// <summary>
        /// 工序Id
        /// </summary>
        public long ProcedureId { get; set; }

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
