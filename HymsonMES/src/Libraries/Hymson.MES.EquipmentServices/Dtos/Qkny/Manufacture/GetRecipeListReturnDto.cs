using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.EquipmentServices.Dtos.Qkny.Manufacture
{
    /// <summary>
    /// 获取开机参数列表返回值
    /// </summary>
    public record GetRecipeListReturnDto
    {
        /// <summary>
        /// 配方编码
        /// </summary>
        public string RecipeCode { get; set; } = "";

        /// <summary>
        /// 配方版本
        /// </summary>
        public string Version { get; set; } = "";

        /// <summary>
        /// 产品型号
        /// </summary>
        public string ProductCode { get; set; } = "";

        /// <summary>
        /// 最后更新时间
        /// </summary>
        public DateTime LastUpdateOnTime { get; set; }
    }
}
