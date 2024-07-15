namespace Hymson.MES.Data.Repositories.Equipment.Query
{
    /// <summary>
    /// 备件类型 查询参数
    /// </summary>
    public class EquToolingTypeQuery
    {
        /// <summary>
        /// 工单
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 条码
        /// </summary>
        public IEnumerable<string>? Codes { get; set; }
    }
}
