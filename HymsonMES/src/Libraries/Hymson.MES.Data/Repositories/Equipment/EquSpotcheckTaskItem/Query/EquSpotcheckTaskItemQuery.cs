namespace Hymson.MES.Data.Repositories.Equipment.Query
{
    /// <summary>
    /// 设备点检任务项目 查询参数
    /// </summary>
    public class EquSpotcheckTaskItemQuery
    {
        public long? SiteId { get; set; }
        public long? SpotCheckTaskId { get; set; }
    }
}
