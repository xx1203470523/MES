namespace Hymson.MES.Data.Repositories.Equipment.Query
{
    /// <summary>
    /// 设备点检任务结果处理 查询参数
    /// </summary>
    public class EquSpotcheckTaskProcessedQuery
    {
        /// <summary>
        /// 主键
        /// </summary>
        public IEnumerable<long>? SpotCheckTaskIds { get; set; }
    }
}
