using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.EquipmentServices.Dtos.Qkny.Manufacture
{

    /// <summary>
    /// 托盘NG电芯上报
    /// </summary>
    public record ContainerNgReportDto : QknyBaseDto
    {
        /// <summary>
        /// 托盘
        /// </summary>
        public string ContainerCode { get; set; } = "";

        /// <summary>
        /// NG电芯条码
        /// </summary>
        public List<string> NgSfcList { get; set; } = new List<string>();
    }
}
