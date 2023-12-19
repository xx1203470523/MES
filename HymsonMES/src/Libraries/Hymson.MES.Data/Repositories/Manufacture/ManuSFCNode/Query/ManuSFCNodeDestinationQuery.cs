namespace Hymson.MES.Data.Repositories.Manufacture.Query
{
    /// <summary>
    /// 条码追溯表-正向 查询参数
    /// </summary>
    public class ManuSFCNodeDestinationQuery
    {
        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<long> NodeIds { get; set; }
    }
}
