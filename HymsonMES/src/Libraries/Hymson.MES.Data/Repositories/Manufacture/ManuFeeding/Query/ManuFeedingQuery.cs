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
        /// 上料点ID
        /// </summary>
        public long? FeedingPointId { get; set; }

        /// <summary>
        /// ID集合（物料）
        /// </summary>
        public IEnumerable<long>? MaterialIds { get; set; }
    }

    /// <summary>
    /// 查询参数
    /// </summary>
    public class GetByFeedingPointIdAndMaterialIdsQuery
    {
        /// <summary>
        /// 上料点ID
        /// </summary>
        public long FeedingPointId { get; set; }

        /// <summary>
        /// ID集合（物料）
        /// </summary>
        public IEnumerable<long>? MaterialIds { get; set; }
    }

    /// <summary>
    /// 查询参数
    /// </summary>
    public class GetByFeedingPointIdsQuery
    {
        /// <summary>
        /// 上料点ID
        /// </summary>
        public IEnumerable<long> FeedingPointIds { get; set; }
    }

    /// <summary>
    /// 查询参数
    /// </summary>
    public class GetByBarCodeAndMaterialIdQuery
    {
        /// <summary>
        /// 上料点ID
        /// </summary>
        public long FeedingPointId { get; set; }

        /// <summary>
        /// 物料ID
        /// </summary>
        public long ProductId { get; set; }

        /// <summary>
        /// 物料条码编码
        /// </summary>
        public string BarCode { get; set; }
    }
}
