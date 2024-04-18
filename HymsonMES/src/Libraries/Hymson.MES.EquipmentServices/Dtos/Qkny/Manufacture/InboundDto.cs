using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.EquipmentServices.Dtos.Qkny.Manufacture
{
    /// <summary>
    /// 设备进站
    /// </summary>
    public record InboundDto : QknyBaseDto
    {
        /// <summary>
        /// 条码
        /// </summary>
        public string Sfc { get; set; } = "";
    }

    /// <summary>
    /// 绑定后的极组单个出站
    /// </summary>
    public record InboundBindJzSingleDto : InboundDto
    {
        /// <summary>
        /// 工序类型
        /// 1-首次发生 2-中间 3-末尾工序
        /// </summary>
        public string OperationType { get; set; } = string.Empty;
    }
}
