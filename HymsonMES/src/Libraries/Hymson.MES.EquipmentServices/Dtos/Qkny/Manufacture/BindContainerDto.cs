using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.EquipmentServices.Dtos.Qkny.Manufacture
{
    /// <summary>
    /// 托盘电芯绑定
    /// </summary>
    public record BindContainerDto : QknyBaseDto
    {
        /// <summary>
        /// 托盘信息
        /// </summary>
        public string ContainerCode { get; set; } = "";

        /// <summary>
        /// 托盘条码列表
        /// </summary>
        public List<ContainerSfcDto> ContainerSfcList { get; set; } = new List<ContainerSfcDto>();
    }

    /// <summary>
    /// 托盘条码信息
    /// </summary>
    public record ContainerSfcDto
    {
        /// <summary>
        /// 条码
        /// </summary>
        public string Sfc { get; set; } = "";

        /// <summary>
        /// 位置
        /// </summary>
        public string Location { get; set; } = "";
    }
}
