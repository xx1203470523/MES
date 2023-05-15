using Hymson.MES.Core.Enums.Integrated;

namespace Hymson.MES.Data.Repositories.Integrated.InteCodeRule.Query
{
    /// <summary>
    /// 查询产品编码规则
    /// </summary>
    public class InteCodeRulesByProductQuery
    {
        /// <summary>
        /// 产品id  物料
        /// </summary>
        public long ProductId { get; set; }

        /// <summary>
        /// 编码类型;1：过程控制序列码；2：包装序列码；
        /// </summary>
        public CodeRuleCodeTypeEnum? CodeType { get; set; }
    }
}
