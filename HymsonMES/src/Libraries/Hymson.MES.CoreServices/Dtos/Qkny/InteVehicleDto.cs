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

    /// <summary>
    /// 托盘NG电芯
    /// </summary>
    public class InteVehicleNgSfcDto : CoreBaseBo
    {
        /// <summary>
        /// 托盘
        /// </summary>
        public string ContainerCode { get; set; } = "";

        /// <summary>
        /// 当前工序
        /// </summary>
        public long OperationId { get; set; }

        /// <summary>
        /// NG电芯条码
        /// </summary>
        public List<InteVehicleSfcDetailDto> NgSfcList { get; set; } = new List<InteVehicleSfcDetailDto>();
    }

    /// <summary>
    /// NG条码信息
    /// </summary>
    public class InteVehicleSfcDetailDto
    {
        /// <summary>
        /// 条码
        /// </summary>
        public string Sfc { get; set; } = string.Empty;

        /// <summary>
        /// 不合格代码
        /// </summary>
        public string NgCode { get; set; } = string.Empty;

        /// <summary>
        /// 工序id
        /// </summary>
        public long OperationId { get; set; }

        /// <summary>
        /// 资源ID
        /// </summary>
        public long ResourceId { get; set; }

        /// <summary>
        /// 设备id
        /// </summary>
        public long EquipmentId { get; set; }
    }
}
