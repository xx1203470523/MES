namespace Hymson.MES.Data.Repositories.Manufacture.ManuFeeding.Query
{
    /// <summary>
    /// 查询参数
    /// </summary>
    public class GetByResourceIdAndMaterialIdQuery
    {
        /// <summary>
        /// ID（资源）
        /// </summary>
        public long ResourceId { get; set; }

        /// <summary>
        /// ID（物料）
        /// </summary>
        public long MaterialId { get; set; }
    }

    /// <summary>
    /// 查询参数
    /// </summary>
    public class GetByResourceIdAndMaterialIdsQuery
    {
        /// <summary>
        /// ID（资源）
        /// </summary>
        public long ResourceId { get; set; }

        /// <summary>
        /// ID集合（物料）
        /// </summary>
        public IEnumerable<long>? MaterialIds { get; set; }
    }
}
