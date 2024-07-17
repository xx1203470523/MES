using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Integrated
{
    /// <summary>
    /// 数据实体（编码规则物料）   
    /// inte_code_rules_material
    /// @author xiaofei
    /// @date 2024-07-14 11:19:18
    /// </summary>
    public class InteCodeRulesMaterialEntity : BaseEntity
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 编码规则id
        /// </summary>
        public long CodeRulesId { get; set; }

        /// <summary>
        /// 物料Id
        /// </summary>
        public long MaterialId { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        
    }
}
