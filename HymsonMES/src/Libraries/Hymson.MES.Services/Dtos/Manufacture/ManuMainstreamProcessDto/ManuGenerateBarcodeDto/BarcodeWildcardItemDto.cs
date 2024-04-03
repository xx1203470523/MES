using Hymson.MES.Core.Enums.Integrated;

namespace Hymson.MES.Services.Dtos.Manufacture.ManuMainstreamProcessDto.ManuGenerateBarcodeDto
{
    /// <summary>
    /// 通配符描述
    /// </summary>
    public class BarcodeWildcardItemDto
    {
        /// <summary>
        /// 通配符描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 通配符
        /// </summary>
        public string? Key { get; set; }

        /// <summary>
        /// 编码类型列表
        /// </summary>
        public CodeRuleCodeTypeEnum[] CodeTypes { get; set; }

        /// <summary>
        /// 编码模式 1： 单个    2： 多个
        /// </summary>
        public CodeRuleCodeModeEnum[] CodeModes { get; set; }
    }
}
