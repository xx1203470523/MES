using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.EquipmentServices.Dtos.QueryContainerBindSfc
{
    /// <summary>
    /// 容器绑定条码查询
    /// </summary>
    public record QueryContainerBindSfcDto : BaseDto
    {
        /// <summary>
        /// 托盘码
        /// </summary>
        public string ContaineCode { get; set; } = string.Empty;
    }
}
