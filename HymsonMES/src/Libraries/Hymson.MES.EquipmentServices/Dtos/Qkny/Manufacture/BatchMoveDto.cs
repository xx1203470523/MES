using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.EquipmentServices.Dtos.Qkny.Manufacture
{
    /// <summary>
    /// 批次转移
    /// </summary>
    public record BatchMoveDto : QknyBaseDto
    {
        /// <summary>
        /// 条码
        /// </summary>
        public string Sfc { get; set; } = "";

        /// <summary>
        /// 转出罐体设备号
        /// </summary>
        public string EquipmentCodeOut { get; set; } = "";

        /// <summary>
        /// 转入罐体设备号
        /// </summary>
        public string EquipmentCodeIn { get; set; } = "";

        /// <summary>
        /// 重量
        /// </summary>
        public decimal Qty { get; set; }
    }

    /// <summary>
    /// 批次转移
    /// </summary>
    public record MultBatchMoveDto : QknyBaseDto
    {
        /// <summary>
        /// 设备ID
        /// </summary>
        public long EquipmentId { get; set; }

        /// <summary>
        /// 转出罐体设备号
        /// </summary>
        public string EquipmentCodeOut { get; set; } = "";

        /// <summary>
        /// 转入罐体设备号
        /// </summary>
        public string EquipmentCodeIn { get; set; } = "";

        /// <summary>
        /// 条码列表
        /// </summary>
        public List<BatchMoveSfcDto> SfcList = new List<BatchMoveSfcDto>();
    }

    /// <summary>
    /// 条码明细
    /// </summary>
    public record BatchMoveSfcDto
    {
        /// <summary>
        /// 条码
        /// </summary>
        public string Sfc { get; set; } = "";

        /// <summary>
        /// 重量
        /// </summary>
        public decimal Qty { get; set; }
    }
}
