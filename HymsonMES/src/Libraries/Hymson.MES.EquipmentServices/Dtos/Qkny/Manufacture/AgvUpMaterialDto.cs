using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.EquipmentServices.Dtos.Qkny.Manufacture
{
    /// <summary>
    /// 上料呼叫AGV
    /// </summary>
    public record AgvUpMaterialDto : QknyBaseDto
    {
        /// <summary>
        /// 类型
        /// </summary>
        public string TaskType { get; set; } = string.Empty;
    }

    /// <summary>
    /// AGV叫料
    /// </summary>
    public record AgvMaterialDto : QknyBaseDto
    {
        /// <summary>
        /// 类型
        /// </summary>
        public string TaskType { get; set; } = string.Empty;

        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; } = string.Empty;
    }
}
