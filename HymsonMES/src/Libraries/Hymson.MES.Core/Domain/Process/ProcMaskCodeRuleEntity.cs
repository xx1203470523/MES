using Hymson.Infrastructure;
using Hymson.MES.Core.Enums.Integrated;

namespace Hymson.MES.Core.Domain.Process
{
    /// <summary>
    /// 掩码关联规则表数据实体对象
    /// @author admin
    /// @date 2023-02-08
    /// </summary>
    public class ProcMaskCodeRuleEntity : BaseEntity
    {
        /// <summary>
        /// 描述 :所属站点代码 
        /// 空值 : false  
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 描述 :所属掩码ID 
        /// 空值 : false  
        /// </summary>
        public long MaskCodeId { get; set; }

        /// <summary>
        /// 描述 :序号( 程序生成) 
        /// 空值 : true  
        /// </summary>
        public long SerialNo { get; set; }

        /// <summary>
        /// 描述 :掩码规则 
        /// 空值 : false  
        /// </summary>
        public string Rule { get; set; }

        /// <summary>
        /// 描述 :匹配方式 
        /// 空值 : true  
        /// </summary>
        public MatchModeEnum MatchWay { get; set; }
    }
}