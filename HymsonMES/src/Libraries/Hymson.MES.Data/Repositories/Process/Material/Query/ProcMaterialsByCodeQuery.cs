namespace Hymson.MES.Data.Repositories.Process
{
    /// <summary>
    ///根具编码查询物料
    /// </summary>
    public class ProcMaterialsByCodeQuery
    {
        /// <summary>
        /// 站点id
        /// </summary>
        public long? SiteId { get; set; }

        /// <summary>
        /// 编码集合
        /// </summary>
        public IEnumerable<string> MaterialCodes { get; set; }
    }
}
