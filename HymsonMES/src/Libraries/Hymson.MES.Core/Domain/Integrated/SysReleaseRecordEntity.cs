using Hymson.Infrastructure;
using Hymson.MES.Core.Enums.Integrated;

namespace Hymson.MES.Core.Domain.Integrated
{
    /// <summary>
    /// 发布记录表，数据实体对象   
    /// sys_release_record
    /// @author pengxin
    /// @date 2023-12-19 10:03:09
    /// </summary>
    public class SysReleaseRecordEntity : BaseEntity
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 版本
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// 计划时间
        /// </summary>
        public DateTime PlanTime { get; set; }

        /// <summary>
        /// 发布时间
        /// </summary>
        public DateTime? RealTime { get; set; }

        /// <summary>
        /// 状态;1、预留 2、发布
        /// </summary>
        public SysReleaseRecordStatusEnum Status { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 发布内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 环境类型;1、正式 2、测试
        /// </summary>
        public EnvironmentTypeEnum? EnvironmentType { get; set; }


    }
}
