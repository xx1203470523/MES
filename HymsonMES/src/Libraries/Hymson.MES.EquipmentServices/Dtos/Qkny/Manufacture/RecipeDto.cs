using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.EquipmentServices.Dtos.Qkny.Manufacture
{
    /// <summary>
    /// 开机参数校验采集
    /// </summary>
    public record RecipeDto : QknyBaseDto
    {
        /// <summary>
        /// 产品编码
        /// </summary>
        public string ProductCode { get; set; } = "";

        /// <summary>
        /// 开机配方编码
        /// </summary>
        public string RecipeCode { get; set; } = "";

        /// <summary>
        /// 版本
        /// </summary>
        public string Version { get; set; } = "";

        /// <summary>
        /// 参数列表
        /// </summary>
        public List<RecipeParamDto> ParamList { get; set; } = new List<RecipeParamDto>();
    }

}
