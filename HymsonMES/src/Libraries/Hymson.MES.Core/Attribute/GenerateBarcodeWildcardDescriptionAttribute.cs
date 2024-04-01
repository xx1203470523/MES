using Hymson.MES.Core.Enums.Integrated;

namespace Hymson.MES.Core.Attribute
{
    /// <summary>
    /// 编码规则通配符描述 特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class GenerateBarcodeWildcardDescriptionAttribute : System.Attribute
    {



        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="description">描述</param>
        /// <param name="codeTypes">编码类型数组 归类</param>
        /// <param name="codeModes">编码模式归类</param>
        public GenerateBarcodeWildcardDescriptionAttribute(string description, CodeRuleCodeTypeEnum[] codeTypes, CodeRuleCodeModeEnum[] codeModes)
        {
            Description = description;
            CodeTypes = codeTypes;
            CodeModes = codeModes;
        }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 编码类型
        /// </summary>
        public CodeRuleCodeTypeEnum[] CodeTypes { get; set; }

        /// <summary>
        /// 编码模式 1： 单个    2： 多个
        /// </summary>
        public CodeRuleCodeModeEnum[] CodeModes { get; set; }



    }
}
