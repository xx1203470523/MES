using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.EquipmentServices.Dtos.Parameter
{
    /// <summary>
    /// 产品过程参数
    /// </summary>
    public  class ProductProcessParameterDto
    {

        /// <summary>
        /// 资源编码
        /// </summary>
        public string ResourceCode { get; set; }

        /// <summary>
        /// 产品列表
        /// </summary>
        public IEnumerable<ProductDto> Products { get; set; }
    }

    /// <summary>
    /// 产品信息
    /// </summary>
    public class ProductDto
    {
        /// <summary>
        /// 条码
        /// </summary>
        public string SFC { get; set; }

        /// <summary>
        /// 参数
        /// </summary>
        public IEnumerable<ProductParameterDto> Parameters { get; set; }
    }

    /// <summary>
    /// 参数信息
    /// </summary>
    public class ProductParameterDto
    {
        /// <summary>
        /// 参数编码
        /// </summary>
        public string ParameterCode { get; set; }

        /// <summary>
        /// 参数值
        /// </summary>
        public string ParameterValue { get; set; }

        /// <summary>
        /// 采集时间
        /// </summary>
        public DateTime CollectionTime { get; set; }
    }
    }
