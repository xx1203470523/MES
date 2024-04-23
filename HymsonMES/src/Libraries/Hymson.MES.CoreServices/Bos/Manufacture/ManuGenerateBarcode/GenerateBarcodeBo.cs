using Hymson.MES.Core.Enums.Integrated;
using Hymson.MES.CoreServices.Bos.Common;
using Hymson.Sequences.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.CoreServices.Bos.Manufacture.ManuGenerateBarcode
{
    /// <summary>
    /// 生成条码实体
    /// </summary>
    public class GenerateBarcodeBo: CoreBaseBo
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

        /// <summary>
        /// 产品id
        /// </summary>
        public long ProductId { get; set; }

        /// <summary>
        /// 工作中心Id 为获取线体数据做准备 生成的条码需要线体相关数据进行扩展
        /// </summary>
        public long? InteWorkCenterId { get; set; }

        /// <summary>
        /// 极组条码
        /// </summary>
        public IEnumerable<string>? Sfcs { get; set; }

        /// <summary>
        /// 工单id 为生成极片状态创建而设置
        /// </summary>
        public long? WorkOrderId { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class CodeRuleBo
    {
        /// <summary>
        /// 是否测试
        /// </summary>
        public bool IsTest { get; set; } = false;

        /// <summary>
        /// 数量
        /// </summary>
        public int Count { get; set; } = 1;

        /// <summary>
        /// 产品id
        /// </summary>
        public long ProductId { get; set; }

        /// <summary>
        /// 编码类型;1：过程控制序列码；2：包装序列码；
        /// </summary>
        public CodeRuleCodeTypeEnum CodeType { get; set; }

        /// <summary>
        /// 编码模式 1： 单个    2： 多个
        /// </summary>
        public CodeRuleCodeModeEnum CodeMode { get; set; }


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
        public string? IgnoreChar { get; set; }

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

        public IEnumerable<CodeRulesMakeDto> CodeRulesMakeList { get; set; } = new List<CodeRulesMakeDto>();

        /// <summary>
        /// 站点
        /// </summary>
        public long SiteId { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
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
        public string SegmentedValue { get; set; } = "";

        /// <summary>
        /// 自定义值
        /// </summary>
        public string? CustomValue { get; set; }
    }
}
