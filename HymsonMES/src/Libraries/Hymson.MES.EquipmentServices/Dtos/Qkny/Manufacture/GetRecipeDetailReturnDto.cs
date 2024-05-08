using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.EquipmentServices.Dtos.Qkny.Manufacture
{
    /// <summary>
    /// 获取开机参数详情
    /// </summary>
    public record GetRecipeDetailReturnDto
    {
        /// <summary>
        /// 版本
        /// </summary>
        public string Version { get; set; } = "";

        /// <summary>
        /// 最后更新时间
        /// </summary>
        public DateTime LastUpdateOnTime { get; set; }

        /// <summary>
        /// 参数列表
        /// </summary>
        public List<RecipeParamDto> ParamList { get; set; } = new List<RecipeParamDto>();
    }

    /// <summary>
    /// 开机参数详情列表
    /// </summary>
    public record RecipeParamDto
    {
        /// <summary>
        /// 参数编码
        /// </summary>
        public string ParamCode { get; set; } = "";

        /// <summary>
        /// 参数名称
        /// </summary>
        public string ParamName { get; set; } = "";

        /// <summary>
        /// 参数值
        /// </summary>
        public string ParamValue { get; set; } = "";

        /// <summary>
        /// 参数上限
        /// </summary>
        public string ParamUpper { get; set; } = "";

        /// <summary>
        /// 参数下限
        /// </summary>
        public string ParamLower { get; set; } = "";
    }

}
