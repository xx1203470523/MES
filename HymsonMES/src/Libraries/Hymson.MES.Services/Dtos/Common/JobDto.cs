using Hymson.Utils;

namespace Hymson.MES.Services.Dtos.Common
{
    /// <summary>
    /// 按钮Dto（请求）
    /// </summary>
    public class ButtonRequestDto
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
        /// 参数
        /// </summary>
        public Dictionary<string, string>? Param { get; set; }
    }

    /// <summary>
    /// 按钮Dto（响应）
    /// </summary>
    public class ButtonResponseDto
    {
        /// <summary>
        /// 按钮ID
        /// </summary>
        public long FacePlateButtonId { get; set; }

        /// <summary>
        /// 结果
        /// </summary>
        public Dictionary<string, JobResponseDto> Data { get; set; } = new();

    }

    /// <summary>
    /// 作业Dto（请求）
    /// </summary>
    public class JobRequestDto
    {
        /// <summary>
        /// 额外数据序列成的字典
        /// </summary>
        public Dictionary<string, string>? Params { get; set; }
    }

    /// <summary>
    /// 作业Dto（响应）
    /// </summary>
    public class JobResponseDto
    {
        /// <summary>
        /// 内容
        /// </summary>
        public Dictionary<string, string>? Content { get; set; } = new();

        /// <summary>
        /// 消息
        /// </summary>
        public string Message { get; set; } = "";

        /// <summary>
        /// 时间
        /// </summary>
        public DateTime Time { get; set; } = HymsonClock.Now();

    }


}
