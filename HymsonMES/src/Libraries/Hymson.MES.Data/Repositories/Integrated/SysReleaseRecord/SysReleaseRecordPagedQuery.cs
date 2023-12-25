/*
 *creator: Karl
 *
 *describe: 发布记录表 分页查询类 | 代码由框架生成
 *builder:  pengxin
 *build datetime: 2023-12-19 10:03:09
 */
using Hymson.Infrastructure;
using Hymson.MES.Core.Enums.Integrated;

namespace Hymson.MES.Data.Repositories.Integrated
{
    /// <summary>
    /// 发布记录表 分页参数
    /// </summary>
    public class SysReleaseRecordPagedQuery : PagerInfo
    {
        /// <summary>
        /// 版本
        /// </summary>
        public string? Version { get; set; }

        /// <summary>
        ///发布时间 范围 
        /// </summary>
        public DateTime[]? RealTime { get; set; } 

        /// <summary>
        /// 状态;1、预留 2、发布
        /// </summary>
        public SysReleaseRecordStatusEnum? Status { get; set; }

        /// <summary>
        /// 环境类型;1、正式 2、测试
        /// </summary>
        public EnvironmentTypeEnum? EnvironmentType { get; set; }
    }
}
