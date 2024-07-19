using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.HttpClients.Responses.Rotor
{
    /// <summary>
    /// 转子线LMS接口返回结构
    /// </summary>
    public class RotorResponse
    {
        /// <summary>
        /// 状态码 200是成功
        /// </summary>
        public int StatusCode { get; set; }

        /// <summary>
        /// 返回消息
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// 时间戳
        /// </summary>
        public string Timestamp { get; set; } = string.Empty;

        /// <summary>
        /// 总数
        /// </summary>
        public int Total { get; set; } = 0;

        /// <summary>
        /// 数据
        /// </summary>
        public int Data { get; set; } = 0;

        /// <summary>
        /// 是否成功
        /// </summary>
        public bool IsSuccess
        {
            get
            {
                return StatusCode == 200;
            }
        }
    }
}
