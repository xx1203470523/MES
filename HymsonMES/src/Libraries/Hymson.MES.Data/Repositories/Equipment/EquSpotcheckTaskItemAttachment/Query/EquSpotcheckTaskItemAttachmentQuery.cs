namespace Hymson.MES.Data.Repositories.Equipment.Query
{
    /// <summary>
    /// 设备点检任务项目附件 查询参数
    /// </summary>
    public class EquSpotcheckTaskItemAttachmentQuery
    {
        /// <summary>
        /// 站点ID
        /// </summary>
        public long? SiteId { get; set; }
        public long? SpotCheckTaskId { get; set; }
    }
}
