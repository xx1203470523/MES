using Hymson.Infrastructure;
using Hymson.MES.Core.Constants.Common;
using OfficeOpenXml.Attributes;

namespace Hymson.MES.Services.Dtos.Quality
{
    /// <summary>
    /// 导入/导出模板模型
    /// </summary>
    public record QualAQLLevelExcelDto : BaseExcelDto
    {
        /// <summary>
        /// 检验标准
        /// </summary>
        [EpplusTableColumn(Header = "检验标准(必填)", Order = 1)]
        public string Standard { get; set; }

        /// <summary>
        /// 批次最小数量
        /// </summary>
        [EpplusTableColumn(Header = "批次最小数量(必填)", Order = 2)]
        public int Min { get; set; }

        /// <summary>
        /// 批次最大数量
        /// </summary>
        [EpplusTableColumn(Header = "批次最大数量(必填)", Order = 3)]
        public int Max { get; set; }

        /// <summary>
        /// VII
        /// </summary>
        [EpplusTableColumn(Header = "VII(必填)", Order = 4)]
        public string VII { get; set; }

        /// <summary>
        /// VI
        /// </summary>
        [EpplusTableColumn(Header = "VI(必填)", Order = 5)]
        public string VI { get; set; }

        /// <summary>
        /// V
        /// </summary>
        [EpplusTableColumn(Header = "V(必填)", Order = 6)]
        public string V { get; set; }

        /// <summary>
        /// IV
        /// </summary>
        [EpplusTableColumn(Header = "IV(必填)", Order = 7)]
        public string IV { get; set; }

        /// <summary>
        /// III
        /// </summary>
        [EpplusTableColumn(Header = "III(必填)", Order = 8)]
        public string III { get; set; }

        /// <summary>
        /// II
        /// </summary>
        [EpplusTableColumn(Header = "II(必填)", Order = 9)]
        public string II { get; set; }

        /// <summary>
        /// I
        /// </summary>
        [EpplusTableColumn(Header = "I(必填)", Order = 10)]
        public string I { get; set; }

    }

    /// <summary>
    /// 
    /// </summary>
    public record QualAQLLevelExprotRequestDto
    {
        /// <summary>
        /// 配置编码
        /// </summary>
        public IEnumerable<string> Codes { get; set; } = new List<string> { AQLStandard.MIL };

    }

}
