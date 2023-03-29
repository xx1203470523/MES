using Hymson.MES.Core.Enums.Integrated;
using Hymson.Sequences.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Dtos.Manufacture.ManuMainstreamProcessDto.ManuGenerateBarcodeDto
{
    /// <summary>
    /// 生成条码实体
    /// </summary>
    public class GenerateBarcodeDto
    {
        /// <summary>
        /// 规则id
        /// </summary>
        public long CodeRuleId { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// 是否测试
        /// </summary>
        public bool IsTest { get; set; } = false;
    }

    public class CodeRuleDto
    {
        /// <summary>
        /// 是否测试
        /// </summary>
        public bool IsTest { get; set; } = false;

        /// <summary>
        /// 数量
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// 产品id
        /// </summary>
        public long ProductId { get; set; }

        /// <summary>
        /// 编码类型;1：过程控制序列码；2：包装序列码；
        /// </summary>
        public CodeRuleCodeTypeEnum CodeType { get; set; }

        /// <summary>
        /// 包装等级;1：一级；2：二级；3：三级；
        /// </summary>
        public CodeRulePackTypeEnum? PackType { get; set; }

        /// <summary>
        /// 基数;10 16 32
        /// </summary>
        public int Base { get; set; }

        /// <summary>
        /// 忽略字符
        /// </summary>
        public string IgnoreChar { get; set; }

        /// <summary>
        /// 增量
        /// </summary>
        public int Increment { get; set; }

        /// <summary>
        /// 序列长度;0:表示无限长度
        /// </summary>
        public int OrderLength { get; set; }

        /// <summary>
        /// 重置序号;1：从不；2：每天；3：每周；4：每月；5：每年；
        /// </summary>
        public SerialNumberTypeEnum ResetType { get; set; }

        /// <summary>
        /// 初始值
        /// </summary>
        public int StartNumber { get; set; }

        public IEnumerable<CodeRulesMakeDto> CodeRulesMakeList { get; set; }
    }

    public class CodeRulesMakeDto
    {
        /// <summary>
        /// 序号
        /// </summary>
        public int Seq { get; set; }

        /// <summary>
        /// 取值方式;1：固定值；2：可变值；
        /// </summary>
        public CodeValueTakingTypeEnum ValueTakingType { get; set; }

        /// <summary>
        /// 分段值
        /// </summary>
        public string SegmentedValue { get; set; }

    }
}
