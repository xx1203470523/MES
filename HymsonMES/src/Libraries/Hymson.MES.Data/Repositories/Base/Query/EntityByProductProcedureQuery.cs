using Hymson.MES.Core.Enums;

namespace Hymson.MES.Data.Repositories.Common.Query
{
    /// <summary>
    /// 
    /// </summary>
    public class EntityByProductProcedureQuery
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 产品Id
        /// </summary>
        public long ProductId { get; set; }

        /// <summary>
        /// 工序Id
        /// </summary>
        public long ProcedureId { get; set; }

        /// <summary>
        /// <summary>
        /// 状态
        /// </summary>
        public SysDataStatusEnum Status { get; set; }

    }
}
