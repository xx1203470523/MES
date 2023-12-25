using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.Data.Repositories.Process.Query
{
    /// <summary>
    /// 配方维护 分页参数
    /// </summary>
    public class ProcFormulaPagedQuery : PagerInfo
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }


        /// <summary>
        /// 配方编码
        /// </summary>
        public string? Code { get; set; }

        /// <summary>
        /// 配方名称
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public SysDataStatusEnum? Status { get; set; }

        /// <summary>
        /// 物料ID
        /// </summary>
        public long? MaterialId { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        public string? MaterialCode {  get; set; }

        /// <summary>
        /// 物料名称
        /// </summary>
        public string? MaterialName { get; set; }

        /// <summary>
        /// 工序Id
        /// </summary>
        public long? ProcedureId { get; set; }

        /// <summary>
        /// 工序编码
        /// </summary>
        public string ProcedureCode {  get; set; }

        /// <summary>
        /// 工序名称
        /// </summary>
        public string? ProcedureName { get; set; }
    }
}
