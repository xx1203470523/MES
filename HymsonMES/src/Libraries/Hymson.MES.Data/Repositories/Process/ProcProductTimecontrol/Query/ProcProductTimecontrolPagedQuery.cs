/*
 *creator: Karl
 *
 *describe: 产品工序时间 分页查询类 | 代码由框架生成
 *builder:  zhaoqing
 *build datetime: 2023-07-27 01:54:06
 */
using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.Data.Repositories.Process
{
    /// <summary>
    /// 产品工序时间 分页参数
    /// </summary>
    public class ProcProductTimecontrolPagedQuery : PagerInfo
    {
        /// <summary>
        /// 站点id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        public string? MaterialCode { get; set; }

        /// <summary>
        /// 工序编码
        /// </summary>
        public string? ProcedureCode { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public SysDataStatusEnum? Status { get; set; }
    }
}
