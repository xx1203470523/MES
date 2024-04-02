/*
 *creator: Karl
 *
 *describe: 环境检验单 分页查询类 | 代码由框架生成
 *builder:  pengxin
 *build datetime: 2024-03-22 05:04:53
 */
using Hymson.Infrastructure;

namespace Hymson.MES.Data.Repositories.QualEnvOrder
{
    /// <summary>
    /// 环境检验单 分页参数
    /// </summary>
    public class QualEnvOrderPagedQuery : PagerInfo
    {

        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }
        /// <summary>
        /// 检验单号
        /// </summary>
        public string? InspectionOrder { get; set; }

        /// <summary>
        /// 工作中心Id
        /// </summary>
        public long? WorkCenterId { get; set; }

        ///// <summary>
        ///// 工作中心Code
        ///// </summary>
        //public string? WorkCenterCode { get; set; }

        ///// <summary>
        ///// 工作中心Name
        ///// </summary>
        //public string? WorkCenterName { get; set; }

        /// <summary>
        /// 工序Id
        /// </summary>
        public long? ProcedureId { get; set; }

        ///// <summary>
        ///// 工序Code
        ///// </summary>
        //public string? ProcedureCode { get; set; }

        ///// <summary>
        ///// 工序Name
        ///// </summary>
        //public string? ProcedureName { get; set; }

        /// <summary>
        /// 检验日期
        /// </summary>
        public DateTime[]? InspectionDate { get; set; }
    }
}
