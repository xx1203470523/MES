namespace Hymson.MES.Data.Repositories.Equipment.Query
{
    /// <summary>
    /// 工具类型管理 查询参数
    /// </summary>
    public class EquToolsTypeQuery
    {
        public long SiteId { get; set; }

        public IEnumerable<string>? Codes { get; set; }
    }
}
