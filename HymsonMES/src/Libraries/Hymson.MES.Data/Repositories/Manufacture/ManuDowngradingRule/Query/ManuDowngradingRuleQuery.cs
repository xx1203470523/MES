/*
 *creator: Karl
 *
 *describe: 降级规则 查询类 | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-08-07 02:00:57
 */

namespace Hymson.MES.Data.Repositories.Manufacture
{
    /// <summary>
    /// 降级规则 查询参数
    /// </summary>
    public class ManuDowngradingRuleQuery
    {
        public long SiteId { get; set; }

    }

    public class ManuDowngradingRuleCodeQuery 
    {
        public string Code { get; set; }

        public long SiteId { get; set; }
    }
    public class ManuDowngradingRuleCodesQuery
    {
        public string[] Codes { get; set; }

        public long SiteId { get; set; }
    }
}
