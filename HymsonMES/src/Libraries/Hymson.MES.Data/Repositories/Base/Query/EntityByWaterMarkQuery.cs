namespace Hymson.MES.Data.Repositories.Common.Query
{
    /// <summary>
    /// 水位查询实体
    /// </summary>
    public partial class EntityByWaterMarkQuery
    {
        /// <summary>
        /// 水位id
        /// </summary>
        public long StartWaterMarkId { set; get; }

        /// <summary>
        /// 条数
        /// </summary>
        public int Rows { set; get; }

    }

    /// <summary>
    /// 水位查询实体带站点ID
    /// </summary>
    public class EntityByWaterSiteIdQuery
    {
        /// <summary>
        /// 水位id
        /// </summary>
        public long StartWaterMarkId { set; get; }

        /// <summary>
        /// 条数
        /// </summary>
        public int Rows { set; get; }

        /// <summary>
        /// 站点ID
        /// </summary>
        public long SiteId { get; set; }
    }

    /// <summary>
    /// 根据时间查询
    /// </summary>
    public class EntityByDateSiteIdQuery
    {
        /// <summary>
        /// 站点ID
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime BeginDate { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// 工序列表
        /// </summary>
        public List<string> ProcedureCodeList { get; set; } = new List<string>();
    }
}
