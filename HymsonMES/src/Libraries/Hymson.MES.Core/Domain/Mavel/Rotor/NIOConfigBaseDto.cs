using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Core.Domain.Mavel.Rotor
{
    /// <summary>
    /// NIO配置基础数据
    /// </summary>
    public class NIOConfigBaseDto
    {
        /// <summary>
        /// 1-转子 2-定子
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// 工厂唯一标识
        /// </summary>
        public string PlantId { get; set; }

        /// <summary>
        /// 工厂名称
        /// </summary>
        public string PlantName { get; set; }

        /// <summary>
        /// 车间唯一标识
        /// </summary>
        public string WorkshopId { get; set; }

        /// <summary>
        /// 车间名称
        /// </summary>
        public string WorkshopName { get; set; }

        /// <summary>
        /// 生产线唯一标识
        /// </summary>
        public string ProductionLineId { get; set; }

        /// <summary>
        /// 生产线名称
        /// </summary>
        public string ProductionLineName { get; set; }

        /// <summary>
        /// 合合作伙伴产品代码
        /// </summary>
        public string VendorProductCode { get; set; }

        /// <summary>
        /// 合作伙伴产品名称
        /// </summary>
        public string VendorProductName { get; set; }

        /// <summary>
        /// NIO产品代码
        /// </summary>
        public string NioProductCode { get; set; }

        /// <summary>
        /// NIO产品名称
        /// </summary>
        public string NioProductName { get; set; }

        /// <summary>
        /// 产品一次良率
        /// </summary>
        public string PassRateTarget { get; set; }
    }
}
