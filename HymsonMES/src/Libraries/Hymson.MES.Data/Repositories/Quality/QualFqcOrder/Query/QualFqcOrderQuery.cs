using Hymson.MES.Core.Enums.Quality;

namespace Hymson.MES.Data.Repositories.Quality.Query
{
    /// <summary>
    /// FQC检验单 查询参数
    /// </summary>
    public class QualFqcOrderQuery
    {
        // <summary>
        /// 排序(默认为 CreatedOn DESC)
        /// </summary>
        public string Sorting { get; set; } = "CreatedOn DESC";

        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 物料Id
        /// </summary>
        public long? MaterialId { get; set; }

        /// <summary>
        /// 工单id
        /// </summary>
        public long? WorkOrderId { get; set; }

        /// <summary>
        /// 状态;1、待检验2、检验中3、已检验4、已关闭
        /// </summary>
        public InspectionStatusEnum? Status { get; set; }

        /// <summary>
        /// 状态列表
        /// </summary>
        public IEnumerable<InspectionStatusEnum>? StatusArr { get; set; }

    }
}
