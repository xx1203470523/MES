namespace Hymson.MES.Data.Repositories.Manufacture.Query
{
    /// <summary>
    /// 生产退料单 查询参数
    /// </summary>
    public class ManuReturnOrderQuery
    {
        /// <summary>
        /// 站点
        /// </summary>
        public long? SiteId { get; set; }

        /// <summary>
        /// 退料单号
        /// </summary>
        public string? ReqOrderCode { get; set; }

    }
}
