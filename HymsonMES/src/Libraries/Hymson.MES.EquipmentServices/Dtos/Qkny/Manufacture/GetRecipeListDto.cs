using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.EquipmentServices.Dtos.Qkny.Manufacture
{
    /// <summary>
    /// 获取开机参数列表
    /// </summary>
    public record GetRecipeListDto : QknyBaseDto
    {
        /// <summary>
        /// 产品型号
        /// </summary>
        public string ProductCode { get; set; } = "";
    }
}
