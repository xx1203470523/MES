using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.EquipmentServices.Dtos.InBound
{
    /// <summary>
    /// 出站响应（多个）
    /// </summary>
    public class OutBoundMoreResponseDto
    {
        /// <summary>
        /// 出站条码
        /// </summary>
        public OutBoundMoreSFC[]? SFCs { get; set; }
    }

    /// <summary>
    /// 出站SFC
    /// </summary>
    public class OutBoundMoreSFC
    {
        /// <summary>
        /// 1为成功，其他为失败
        /// </summary>
        public int Code { get; set; }
        /// <summary>
        /// 消息
        /// </summary>
        public string Msg { get; set; } = string.Empty;
        /// <summary>
        /// 进站条码
        /// </summary>
        public string SFC { get; set; } = string.Empty;
    }
}
