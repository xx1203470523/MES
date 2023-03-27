namespace Hymson.MES.Services.Dtos.Common
{
    /// <summary>
    /// 下拉框选项实体
    /// </summary>
    public class SelectOptionDto
    {
        /// <summary>
        /// 键
        /// </summary>
        public string Key { get; set; } = "";

        /// <summary>
        /// 显示
        /// </summary>
        public string Label { get; set; } = "";

        /// <summary>
        /// 值
        /// </summary>
        public string Value { get; set; } = "";
    }
}
