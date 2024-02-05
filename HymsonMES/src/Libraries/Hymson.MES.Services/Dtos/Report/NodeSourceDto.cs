using Hymson.MES.Services.Dtos.Common;

namespace Hymson.MES.Services.Dtos.Report
{
    /// <summary>
    /// 
    /// </summary>
    public partial record NodeSourceDto : BaseDto
    {
        /// <summary>
        /// 条码ID
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 条码
        /// </summary>
        public string SFC { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 位置
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// 最终树展示的文本
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// 条码ID集合（来源/去向）
        /// </summary>
        public List<NodeSourceDto> Children { get; set; } = new();
    }

}
