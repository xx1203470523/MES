using Hymson.MES.Core.Enums.Integrated;
using Hymson.Sequences.Enums;

namespace Hymson.MES.CoreServices.Bos.Manufacture
{
    /// <summary>
    /// 
    /// </summary>
    public class CodeRulesMakeBo
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
        public string SegmentedValue { get; set; }= "";

        /// <summary>
        /// 自定义值
        /// </summary>
        public string CustomValue { get; set; } = "";
    }

    /// <summary>
    /// 
    /// </summary>
    public class BarCodeSerialNumberBo
    {
        /// <summary>
        /// 是否测试
        /// </summary>
        public bool IsTest { get; set; } = false;

        /// <summary>
        /// 是否模拟（默认模拟）
        /// </summary>
        public bool IsSimulation { get; set; } = false;

        /// <summary>
        /// 集合（编码规则）
        /// </summary>
        public IEnumerable<CodeRulesMakeBo> CodeRulesMakeBos { get; set; } = new List<CodeRulesMakeBo>();


        /// <summary>
        /// 
        /// </summary>
        public string CodeRuleKey { get; set; } = "";

        /// <summary>
        /// 起始
        /// </summary>
        public int StartNumber { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// 基数;10 16 32
        /// </summary>
        public int Base { get; set; }

        /// <summary>
        /// 忽略字符
        /// </summary>
        public string IgnoreChar { get; set; } = "";

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
        /// 编码模式 1： 单个    2： 多个
        /// </summary>
        public CodeRuleCodeModeEnum CodeMode { get; set; }

    }

    /// <summary>
    /// 生成的条码信息
    /// </summary>
    public class BarCodeInfo 
    {
        /// <summary>
        /// 条码
        /// </summary>
        public IEnumerable<string> BarCodes { get; set; } = new List<string>();

        /// <summary>
        /// 条码流水号
        /// </summary>
        public string SerialNumber { get; set; } = "";
    }

    /// <summary>
    /// 
    /// </summary>
    public class SerialNumberBo
    {
        /// <summary>
        /// 参数key
        /// </summary>
        public string CodeRuleKey { get; set; } = "";

        /// <summary>
        /// 是否测试
        /// </summary>
        public bool IsTest { get; set; } = false;

        /// <summary>
        /// 是否模拟（默认模拟）
        /// </summary>
        public bool IsSimulation { get; set; } = false;

        /// <summary>
        /// 增量
        /// </summary>
        public int Increment { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// 重置序号;1：从不；2：每天；3：每周；4：每月；5：每年；
        /// </summary>
        public SerialNumberTypeEnum ResetType { get; set; }

        /// <summary>
        /// 起始
        /// </summary>
        public int StartNumber { get; set; }
    }
}
