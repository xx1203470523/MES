using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.EquipmentServices.Dtos.Qkny.Manufacture
{
    /// <summary>
    /// 涂布获取下发条码(用于涂布CCD面密度)
    /// </summary>
    public record CcdGetBarcodeReturnDto
    {
        /// <summary>
        /// 条码
        /// </summary>
        public string Sfc { get; set; } = "";

        /// <summary>
        /// 物料编码
        /// </summary>
        public string ProductCode { get; set; } = "";

        /// <summary>
        /// 物料数量
        /// </summary>
        public decimal Qty { get; set; }

        /// <summary>
        /// id
        /// </summary>
        public string Id { get; set; } = "";
    }
}
