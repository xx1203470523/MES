using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.EquipmentServices.Dtos.Parameter
{
    /// <summary>
    /// 设备过程参数
    /// </summary>
    public  class EquipmentProductProcessParameterDto
    {
        /// <summary>
        /// 参数编码
        /// </summary>
        public string ParameterCode { get; set; } = "";

        /// <summary>
        /// 参数值
        /// </summary>
        public string ParameterValue { get; set; } = "";

        /// <summary>
        /// 采集时间
        /// </summary>
        public DateTime CollectionTime { get; set; }
    }
  }
