using Hymson.Infrastructure;
using Hymson.MES.Core.Enums.Integrated;

namespace Hymson.MES.Services.Dtos.Process
{
    /// <summary>
    /// 保存Dto（掩码维护）
    /// </summary>
    public record ProcMaskCodeSaveDto : BaseEntityDto
    {
        /// <summary>
        /// 唯一标识
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; } = "";

        /// <summary>
        /// 名称 
        /// </summary>
        public string Name { get; set; } = "";

        /// <summary>
        /// 描述
        /// </summary>
        public string Remark { get; set; } = "";

        /// <summary>
        /// 掩码规则列表
        /// </summary>
        public List<ProcMaskCodeRuleDto> Details { get; set; } = new();
    }

    /// <summary>
    ///  Dto（掩码维护）
    /// </summary>
    public record ProcMaskCodeDto : BaseEntityDto
    {
        /// <summary>
        /// 唯一标识
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 编码 
        /// </summary>
        public string Code { get; set; } = "";

        /// <summary>
        /// 编码 
        /// </summary>
        public string MaskCode { get; set; } = "";

        /// <summary>
        /// 名称 
        /// </summary>
        public string Name { get; set; } = "";

        /// <summary>
        /// 描述
        /// </summary>
        public string Remark { get; set; } = "";

        /// <summary>
        /// 最后修改人
        /// </summary>
        public string UpdatedBy { get; set; } = "";

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? UpdatedOn { get; set; }

        /// <summary>
        /// 掩码规则列表
        /// </summary>
        public IEnumerable<ProcMaskCodeRuleDto> Details { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public record ProcMaskCodeRuleDto : BaseEntityDto
    {
        /// <summary>
        /// 序号( 程序生成)
        /// </summary>
        public long? SerialNo { get; set; }

        /// <summary>
        /// 描述 :掩码规则 
        /// </summary>
        public string Rule { get; set; } = "";

        /// <summary>
        /// 描述 :匹配方式 
        /// </summary>
        public MatchModeEnum MatchWay { get; set; }
    }

    /// <summary>
    /// 分页Dto（掩码维护）
    /// </summary>
    public class ProcMaskCodePagedQueryDto : PagerInfo
    {
        /// <summary>
        /// 编码 
        /// </summary>
        public string? Code { get; set; }

        /// <summary>
        /// 名称 
        /// </summary>
        public string? Name { get; set; }

    }
}
