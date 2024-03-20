using Hymson.MES.CoreServices.Bos.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.CoreServices.Dtos.Qkny
{
    /// <summary>
    /// 载具操作
    /// </summary>
    public class InteVehicleDto
    {
    }

    /// <summary>
    /// 载具绑定
    /// </summary>
    public class InteVehicleBindDto : CoreBaseBo
    {
        /// <summary>
        /// 载具条码
        /// </summary>
        public string ContainerCode { get; set; }

        /// <summary>
        /// 条码列表
        /// </summary>
        public List<InteVehicleSfcDto> SfcList { get; set; } = new List<InteVehicleSfcDto>();
    }

    /// <summary>
    /// 托盘条码信息
    /// </summary>
    public record InteVehicleSfcDto
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

    /// <summary>
    /// 载具解绑
    /// </summary>
    public class InteVehicleUnBindDto : CoreBaseBo
    {
        /// <summary>
        /// 载具条码
        /// </summary>
        public string ContainerCode { get; set; }

        /// <summary>
        /// 条码列表
        /// </summary>
        public List<string> SfcList { get; set; } = new List<string>();
    }
}
