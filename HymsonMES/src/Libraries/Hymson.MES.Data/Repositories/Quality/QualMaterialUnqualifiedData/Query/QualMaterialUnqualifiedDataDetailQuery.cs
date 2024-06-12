namespace Hymson.MES.Data.Repositories.Quality.Query
{
    /// <summary>
    /// 车间物料不良记录明细 查询参数
    /// </summary>
    public class QualMaterialUnqualifiedDataDetailQuery
    {
        /// <summary>
        /// 不良Id
        /// </summary>
        public long? MaterialUnqualifiedDataId { get; set; }

        /// <summary>
        /// 不良Id列表
        /// </summary>
        public IEnumerable<long>? MaterialUnqualifiedDataIds { get; set; }
    }
}