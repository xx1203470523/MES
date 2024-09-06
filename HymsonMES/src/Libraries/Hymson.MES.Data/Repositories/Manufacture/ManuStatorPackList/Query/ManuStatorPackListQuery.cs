namespace Hymson.MES.Data.Repositories.Manufacture.Query
{
    /// <summary>
    /// 定子装箱记录表 查询参数
    /// </summary>
    public class ManuStatorPackListQuery
    {
        /// <summary>
        /// 箱体条码
        /// </summary>
        public string? BoxCode { get; set; }

        /// <summary>
        /// 成品码
        /// </summary>
        public string? ProductCode { get; set; }
    }
}
