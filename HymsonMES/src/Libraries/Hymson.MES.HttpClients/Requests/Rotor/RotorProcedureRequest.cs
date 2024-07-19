using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.HttpClients.Requests.Rotor
{
    /// <summary>
    /// 转子LMS工序结构
    /// </summary>
    public class RotorProcedureRequest
    {
        /// <summary>
        /// 工站编码
        /// </summary>
        public string WorkStationNo { get; set; } = string.Empty;

        /// <summary>
        /// 工站名称
        /// </summary>
        public string WorkStationName { get; set; } = string.Empty;

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; } = string.Empty;

        /// <summary>
        /// 是否启用
        /// 默认值为1，表示启用
        /// </summary>
        public bool Enable { get; set; } = true;

        /// <summary>
        /// 产线UID
        /// </summary>
        public string LineUID { get; set; } = string.Empty;
    }
}
