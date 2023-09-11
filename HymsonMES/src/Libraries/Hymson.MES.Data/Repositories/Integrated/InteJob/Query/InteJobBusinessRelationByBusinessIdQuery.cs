using Hymson.MES.Core.Enums;

namespace Hymson.MES.Data.Repositories.Integrated.InteJob.Query
{
    /// <summary>
    /// 
    /// </summary>
    public class InteJobBusinessRelationByBusinessIdQuery
    {
        /// <summary>
        /// 关联点
        /// </summary>
        public ResourceJobLinkPointEnum? LinkPoint { get; set; }

        /// <summary>
        /// 关联的业务表的ID
        /// </summary>
        public long BusinessId { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsUse { get; set; }
    }
}
