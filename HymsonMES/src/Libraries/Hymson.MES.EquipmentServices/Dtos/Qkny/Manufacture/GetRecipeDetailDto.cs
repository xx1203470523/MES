using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.EquipmentServices.Dtos.Qkny.Manufacture
{
    /// <summary>
    /// 获取开机参数明细
    /// </summary>
    public record GetRecipeDetailDto : QknyBaseDto
    {
        /// <summary>
        /// 开机参数编码
        /// </summary>
        public string RecipeCode { get; set; } = "";
    }
}
