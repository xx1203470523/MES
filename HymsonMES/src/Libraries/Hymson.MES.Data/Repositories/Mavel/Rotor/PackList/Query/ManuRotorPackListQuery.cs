namespace Hymson.MES.Data.Repositories.Manufacture.Query
{
    /// <summary>
    /// 转子装箱记录表 查询参数
    /// </summary>
    public class ManuRotorPackListQuery
    {
        /// <summary>
        /// 箱体码
        /// </summary>
        public string? BoxCode { get; set; }

        /// <summary>
        /// 成品码
        /// </summary>
        public string? Sfc { get; set; }

        public string WorkCenterCode {  get; set; }

        public long SiteId { get; set; }

        /// <summary>
        /// 工单
        /// </summary>
        public string OrderCode { get; set; }
    }
}
