using Hymson.Infrastructure;
using Hymson.MES.Core.Enums.Quality;

namespace Hymson.MES.Core.Domain.Quality
{
    /// <summary>
    /// 数据实体（检验规则（首检才有））   
    /// qual_ipqc_inspection_rule
    /// @author xiaofei
    /// @date 2023-08-08 11:32:49
    /// </summary>
    public class QualIpqcInspectionRuleEntity : BaseEntity
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// IPQC检验项目Id qual_ipqc_inspection 的id
        /// </summary>
        public long IpqcInspectionId { get; set; }

        /// <summary>
        /// 检验方式;1、停机 2、不停机
        /// </summary>
        public IPQCRuleWayEnum Way { get; set; }

        /// <summary>
        /// 指定规则;1、固定 2、随机 3、顺序
        /// </summary>
        public IPQCSpecifyRuleEnum SpecifyRule { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public int Qty { get; set; }

        /// <summary>
        /// 单位;1、台、2、%
        /// </summary>
        public IPQCRuleUnitEnum Unit { get; set; }


    }
}
