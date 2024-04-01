/*
 *creator: Karl
 *
 *describe: 环境检验单    实体类 | 代码由框架生成  如果数据库字段发生变化,则手动调整
 *builder:  pengxin
 *build datetime: 2024-03-22 05:04:53
 */
using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.QualEnvOrder
{
    /// <summary>
    /// 环境检验单，数据实体对象   
    /// qual_env_order
    /// @author pengxin
    /// @date 2024-03-22 05:04:53
    /// </summary>
    public class QualEnvOrderEntity : BaseEntity
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 检验单号
        /// </summary>
        public string InspectionOrder { get; set; }

        /// <summary>
        /// 环境检验参数组快照Id
        /// </summary>
        public long GroupSnapshootId { get; set; }

        /// <summary>
        /// 工作中心Id
        /// </summary>
        public long WorkCenterId { get; set; }

        /// <summary>
        /// 工序Id
        /// </summary>
        public long ProcedureId { get; set; }

        /// <summary>
        /// 是否触发异常
        /// </summary>
        public bool? IsAbnormal { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }


    }
}
