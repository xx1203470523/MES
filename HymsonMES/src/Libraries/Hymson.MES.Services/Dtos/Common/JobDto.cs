namespace Hymson.MES.Services.Dtos.Common
{
    /// <summary>
    /// 作业Dto
    /// </summary>
    public class JobDto
    {
        /// <summary>
        /// 面板ID
        /// </summary>
        public long FacePlateId { get; set; }

        /// <summary>
        /// 按钮ID
        /// </summary>
        public long FacePlateButtonId { get; set; }

        /// <summary>
        /// 额外数据序列成的字符串
        /// </summary>
        public string? Extra { get; set; }
    }

}
