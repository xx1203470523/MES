using Hymson.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.EquipmentServices.Dtos.SfcCirculation
{
    /// <summary>
    /// 条码组件CCS添加Dto
    /// </summary>
    public record SfcCirculationCCSBindDto : BaseDto
    {
        /// <summary>
        /// 模组
        /// </summary>
        public string SFC { get; set; } = string.Empty;
        /// <summary>
        /// 绑定CCS条码信息
        /// </summary>
        public CirculationCCSDto[] BindSFCs { get; set; }
        /// <summary>
        /// 绑定时的型号
        /// </summary>
        public string ModelCode { get; set; }
    }
    public class CirculationCCSDto
    {
        /// <summary>
        /// 绑定位置
        /// CCS中为A,B,C,D
        /// </summary>
        public string Location { get; set; }
        /// <summary>
        /// CCS条码
        /// </summary>
        public string SFC { get; set; }

        /// <summary>
        /// 绑定的批次条码名称
        /// </summary>
        public string? Name { get; set; }
    }
    /// <summary>
    /// 绑定CSS位置
    /// </summary>
    public class CirculationBindCCSLocationDto
    {
        /// <summary>
        /// 当前需要绑定CSS位置
        /// </summary>
        public string CurrentLocation { get; set; } = string.Empty;
        /// <summary>
        /// 所有待绑定位置
        /// </summary>
        public string[] Locations { get; set; } = Array.Empty<string>();
        /// <summary>
        /// 绑定时的型号
        /// </summary>
        public string ModelCode { get; set; }
    }
}
