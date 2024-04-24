using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.EquipmentServices.Dtos.Qkny.Manufacture
{
    /// <summary>
    /// 车间库存接收
    /// </summary>
    public record MaterialInventoryDto : QknyBaseDto
    {
        /// <summary>
        /// 条码列表
        /// </summary>
        public List<MaterialInventorySfcDto> BarCodeList { get; set; } = new List<MaterialInventorySfcDto>();
    }

    /// <summary>
    /// 条码列表
    /// </summary>
    public record MaterialInventorySfcDto
    {
        /// <summary>
        /// 物料条码
        /// </summary>
        public string BarCode { get; set; } = string.Empty;

        /// <summary>
        /// 是否原材料
        /// </summary>
        public bool IsRawMaterial { get; set; }

        /// <summary>
        /// 物料编码，非原材料才需要给
        /// </summary>
        public string MaterialCode { get; set; } = string.Empty;

        /// <summary>
        /// 条码数量，非原材料才需要给
        /// </summary>
        public decimal Qty { get; set; }
    }
}
