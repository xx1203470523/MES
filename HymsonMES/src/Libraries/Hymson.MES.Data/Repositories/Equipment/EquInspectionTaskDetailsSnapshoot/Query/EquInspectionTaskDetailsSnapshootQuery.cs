namespace Hymson.MES.Data.Repositories.Equipment.Query
{
    /// <summary>
    /// 点检任务快照详情 查询参数
    /// </summary>
    public class EquInspectionTaskDetailsSnapshootQuery
    {
        /// <summary>
        /// 点检项目Id
        /// </summary>
        public long? InspectionTaskId { get; set; }
    }
}
