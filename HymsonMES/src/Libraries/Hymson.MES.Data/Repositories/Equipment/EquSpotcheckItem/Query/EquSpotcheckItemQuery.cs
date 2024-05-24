namespace Hymson.MES.Data.Repositories.Equipment.Query
{
    /// <summary>
    /// 设备点检项目 查询参数
    /// </summary>
    public class EquSpotcheckItemQuery
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long? SiteId { get; set; }
        public string? Code { get; set; }   
    }
}
