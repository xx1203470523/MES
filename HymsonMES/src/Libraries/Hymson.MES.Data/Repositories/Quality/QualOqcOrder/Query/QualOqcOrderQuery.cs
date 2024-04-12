namespace Hymson.MES.Data.Repositories.Quality.Query
{
    /// <summary>
    /// OQC检验单 查询参数
    /// </summary>
    public class QualOqcOrderQuery
    {
        /// <summary>
        /// 出货单明细Ids
        /// </summary>
        public IEnumerable<long>? ShipmentMaterialIds { get; set; }

        /// <summary>
        /// SiteId
        /// </summary>
        public long SiteId { get; set; }
    }
}
