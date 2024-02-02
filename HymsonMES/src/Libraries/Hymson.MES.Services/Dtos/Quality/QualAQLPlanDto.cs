using Hymson.Infrastructure;
using Hymson.MES.Core.Constants.Common;
using OfficeOpenXml.Attributes;

namespace Hymson.MES.Services.Dtos.Quality
{
    /// <summary>
    /// 导入/导出模板模型
    /// </summary>
    public record QualAQLPlanExcelDto : BaseExcelDto
    {
        /// <summary>
        /// 检验标准
        /// </summary>
        [EpplusTableColumn(Header = "检验标准(必填)", Order = 1)]
        public string Standard { get; set; }

        /// <summary>
        /// 样本代码
        /// </summary>
        [EpplusTableColumn(Header = "样本代码(必填)", Order = 2)]
        public string Code { get; set; }

        /// <summary>
        /// T
        /// </summary>
        [EpplusTableColumn(Header = "T(必填)", Order = 3)]
        public int T { get; set; }

        /// <summary>
        /// VII
        /// </summary>
        [EpplusTableColumn(Header = "VII(必填)", Order = 4)]
        public int VII { get; set; }

        /// <summary>
        /// VI
        /// </summary>
        [EpplusTableColumn(Header = "VI(必填)", Order = 5)]
        public int VI { get; set; }

        /// <summary>
        /// V
        /// </summary>
        [EpplusTableColumn(Header = "V(必填)", Order = 6)]
        public int V { get; set; }

        /// <summary>
        /// IV
        /// </summary>
        [EpplusTableColumn(Header = "IV(必填)", Order = 7)]
        public int IV { get; set; }

        /// <summary>
        /// III
        /// </summary>
        [EpplusTableColumn(Header = "III(必填)", Order = 8)]
        public int III { get; set; }

        /// <summary>
        /// II
        /// </summary>
        [EpplusTableColumn(Header = "II(必填)", Order = 9)]
        public int II { get; set; }

        /// <summary>
        /// I
        /// </summary>
        [EpplusTableColumn(Header = "I(必填)", Order = 10)]
        public int I { get; set; }

        /// <summary>
        /// R
        /// </summary>
        [EpplusTableColumn(Header = "R(必填)", Order = 11)]
        public int R { get; set; }

    }

    /// <summary>
    /// 
    /// </summary>
    public record QualAQLPlanExprotRequestDto
    {
        /// <summary>
        /// 配置编码
        /// </summary>
        public IEnumerable<string> Codes { get; set; } = new List<string> { AQLStandard.MIL };

    }

}
