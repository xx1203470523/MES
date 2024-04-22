using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.EquipmentServices.Dtos.Qkny.Common
{
    /// <summary>
    /// 设备过程参数
    /// </summary>
    public record EquipmentProcessParamDto : QknyBaseDto
    {
        /// <summary>
        /// 位置
        /// 使用场景：烘烤，一个设备里面分多层的场景
        /// </summary>
        public string Location { get; set; } = string.Empty;

        /// <summary>
        /// 参数列表
        /// </summary>
        public List<QknyParamBaseDto> ParamList { get; set; } = new List<QknyParamBaseDto>();
    }
}
