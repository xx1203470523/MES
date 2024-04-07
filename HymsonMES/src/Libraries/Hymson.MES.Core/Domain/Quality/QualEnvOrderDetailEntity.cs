/*
 *creator: Karl
 *
 *describe: 环境检验单检验明细    实体类 | 代码由框架生成  如果数据库字段发生变化,则手动调整
 *builder:  pengxin
 *build datetime: 2024-03-22 05:04:43
 */
using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.Core.Domain.QualEnvOrderDetail
{
    /// <summary>
    /// 环境检验单检验明细，数据实体对象   
    /// qual_env_order_detail
    /// @author pengxin
    /// @date 2024-03-22 05:04:43
    /// </summary>
    public class QualEnvOrderDetailEntity : BaseEntity
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 环境检验单Id
        /// </summary>
        public long EnvOrderId { get; set; }

        /// <summary>
        /// 环境检验参数组明细Id
        /// </summary>
        public long GroupDetailSnapshootId { get; set; }

        /// <summary>
        /// 应检时间
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// 截止录入时间
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// 实际录入时间
        /// </summary>
        public DateTime? RealTime { get; set; }

        /// <summary>
        /// 检验值
        /// </summary>
        public string? InspectionValue { get; set; }

        /// <summary>
        /// 是否合格(0-否 1-是)
        /// </summary>
        public TrueOrFalseEnum? IsQualified { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string? Remark { get; set; }


    }
}
