using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.EquipmentServices.Dtos.InBound
{
    /// <summary>
    /// 进站响应（多个）
    /// </summary>
    public class InBoundMoreResponseDto
    {
        /// <summary>
        /// 进站条码
        /// </summary>
        public InBoundMoreSFC[]? SFCs { get; set; }
    }

    /// <summary>
    /// 进站SFC
    /// </summary>
    public class InBoundMoreSFC
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
