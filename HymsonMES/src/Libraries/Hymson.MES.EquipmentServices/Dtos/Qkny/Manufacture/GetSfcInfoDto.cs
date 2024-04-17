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
    /// 电芯降级信息
    /// </summary>
    public record SortingSfcInfo
    {
        /// <summary>
        /// 条码
        /// </summary>
        public string SFC { get; set; } = "";

        ///// <summary>
        ///// 是否为降级品
        ///// </summary>
        //public bool IsDowngrading { get; set; } = false;

        /// <summary>
        /// 降级等级
        /// </summary>
        public string Grade { get; set; } = "";
    }
}
