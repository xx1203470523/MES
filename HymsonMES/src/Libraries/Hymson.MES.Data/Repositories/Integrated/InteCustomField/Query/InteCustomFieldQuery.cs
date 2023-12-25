using Hymson.MES.Core.Enums.Integrated;

namespace Hymson.MES.Data.Repositories.Integrated.Query
{
    /// <summary>
    /// 自定义字段 查询参数
    /// </summary>
    public class InteCustomFieldQuery
    {
    
    }

    public class InteCustomFieldByBusinessQuery
    {
        public long SiteId {  get; set; }

        public InteCustomFieldBusinessTypeEnum BusinessType { get; set; }
    }
}
