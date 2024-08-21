using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Integrated
{
    /// <summary>
    /// job业务配置配置表，数据实体对象   
    /// inte__job_business_relation
    /// @author zhaoqing
    /// @date 2023-02-14 02:55:48
    /// </summary>
    public class InteJobBusinessRelationEntity : BaseEntity
    {
        /// <summary>
        /// 1资源  2工序 3不合格代码
        /// </summary>
        public int BusinessType { get; set; }

        /// <summary>
        /// 所属不合格代码ID
        /// </summary>
        public long BusinessId { get; set; }

        /// <summary>
        /// 序号
        /// </summary>
        public int OrderNumber { get; set; } = 0;

        /// <summary>
        /// 关联点
        /// </summary>
        public int LinkPoint { get; set; } = -1;

        /// <summary>
        /// 作业ID
        /// </summary>
        public long JobId { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsUse { get; set; }

        /// <summary>
        /// 参数
        /// </summary>
        public string Parameter { get; set; } = "";

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; } = "";

        /// <summary>
        /// 站点id
        /// </summary>
        public long SiteId { get; set; } = 0;
    }
}
