using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.EquipmentServices.Dtos.InBound
{
    /// <summary>
    /// 进站（多个）
    /// </summary>
    public record InBoundMoreDto : BaseDto
    {
        /// <summary>
        /// 进站条码
        /// </summary>
        public string[] SFCs { get; set; }

        /// <summary>
        /// 是否验证虚拟条码
        /// 为兼容永泰虚拟条码上报参数场景
        /// </summary>
        public bool IsVerifyVirtualSFC { get; set; } = false;
    }
}
