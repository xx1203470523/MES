using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.EquipmentServices.Dtos.SfcCirculation
{
    /// <summary>
    /// 绑定条码流转表
    /// </summary>
    public record SfcCirculationBindDto : BaseDto
    {
        /// <summary>
        /// 模组/Pack条码
        /// </summary>
        public string SFC { get; set; } = string.Empty;
        /// <summary>
        /// 模组绑电芯条码/Pack绑模组条码
        /// </summary>
        public CirculationBindDto[] BindSFCs { get; set; }
        /// <summary>
        /// 绑定时的型号
        /// </summary>
        public string? ModelCode { get; set; }

        /// <summary>
        /// 是否为模组虚拟条码参数
        /// 为兼容永泰虚拟条码场景
        /// </summary>
        public bool IsVirtualSFC { get; set; } = false;
    }

    public class CirculationBindDto
    {
        /// <summary>
        /// 绑定位置
        /// </summary>
        public string Location { get; set; }
        /// <summary>
        /// 绑定的条码
        /// </summary>
        public string SFC { get; set; }

        /// <summary>
        /// 绑定的批次条码名称
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// 绑定时的型号，查询时返回用
        /// </summary>
        public string? ModelCode { get; set; }
    }
    /// <summary>
    /// 工单产品
    /// </summary>
    public class PlanWorkOrderDto
    {
        /// <summary>
        /// 工单号
        /// </summary>
        public string OrderCode { get; set; }

        /// <summary>
        /// 描述 :物料编码 
        /// 空值 : false  
        /// </summary>
        public string MaterialCode { get; set; }

        /// <summary>
        /// 描述 :物料名称 
        /// 空值 : false  
        /// </summary>
        public string MaterialName { get; set; }
    }
}
