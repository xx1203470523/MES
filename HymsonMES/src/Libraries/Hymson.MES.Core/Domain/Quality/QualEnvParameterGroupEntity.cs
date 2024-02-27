using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.Core.Domain.Quality
{
    /// <summary>
    /// 环境检验参数表，数据实体对象   
    /// qual_env_parameter_group
    /// @author Czhipu
    /// @date 2023-07-21 09:57:18
    /// </summary>
    public class QualEnvParameterGroupEntity : BaseEntity
    {
        /// <summary>
        /// 参数集编码
        /// </summary>
        public string Code { get; set; }

       /// <summary>
        /// 参数集名称
        /// </summary>
        public string Name { get; set; }

       /// <summary>
        /// 版本
        /// </summary>
        public string Version { get; set; }

       /// <summary>
        /// 状态
        /// </summary>
        public SysDataStatusEnum Status { get; set; }

       /// <summary>
        /// 工作中心（车间或者线体）
        /// </summary>
        public long WorkCenterId { get; set; }

       /// <summary>
        /// 工序id
        /// </summary>
        public long ProcedureId { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; } = "";

       /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

       
    }
}
