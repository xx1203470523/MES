namespace Hymson.MES.Data.Repositories.Integrated.Query
{
    /// <summary>
    /// 字段国际化 查询参数
    /// </summary>
    public class InteCustomFieldInternationalizationQuery
    {
    
    }

    public class InteCustomFieldInternationalizationByCustomFieldIdsQuery
    {
        public long SiteId {  get; set; }

        public long[] CustomFieldIds {  get; set; }
    }
}
