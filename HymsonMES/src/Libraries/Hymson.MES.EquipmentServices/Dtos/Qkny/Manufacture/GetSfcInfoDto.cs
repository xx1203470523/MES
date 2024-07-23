using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.EquipmentServices.Dtos.Qkny.Manufacture
{
    /// <summary>
    /// 获取条码信息
    /// </summary>
    public record GetSfcInfoDto : QknyBaseDto
    {
        /// <summary>
        /// 条码列表
        /// </summary>
        public List<string> SfcList { get; set; } = new List<string>();
    }

    /// <summary>
    /// 电芯信息
    /// </summary>
    public record SortingSfcInfo
    {
        /// <summary>
        /// 条码
        /// </summary>
        public string SFC { get; set; } = "";

        /// <summary>
        /// 电芯等级（1-降级 2-Marking）
        /// </summary>
        public string Grade { get; set; } = "";
    }
}
