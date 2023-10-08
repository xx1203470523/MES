using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.EquipmentServices.Dtos.EquipmentCollect
{
    public record GetEquipmentBootupRecipeSetDto : BaseDto
    {
        /// <summary>
        /// 产品编码
        /// </summary>
        //public string ProductCode { get; set; }
    }

    public record GetEquipmentBootupParamDetailDto : BaseDto
    {
        /// <summary>
        /// 配方编码
        /// </summary>
        public string RecipeCode { get; set; }
        /// <summary>
        /// 配方版本
        /// </summary>
        public string Version { get; set; }
    }

    /// <summary>
    /// 开机参数
    /// </summary>
    public class BootupParam
    {
        /// <summary>
        /// 配方编码
        /// </summary>
        public string RecipeCode { get; set; }
        /// <summary>
        /// 版本
        /// </summary>
        public string Version { get; set; }
        /// <summary>
        /// 产品编码
        /// </summary>
        public string ProductCode { get; set; }
        /// <summary>
        /// 最后更新时间
        /// </summary>
        public DateTime LastUpdateOnTime { get; set; }
    }

    /// <summary>
    /// 开机参数详细
    /// </summary>
    public class BootupParamDetail
    {
        /// <summary>
        /// 版本
        /// </summary>
        public string Version { get; set; }
        /// <summary>
        /// 最后更新时间
        /// </summary>
        public DateTime LastUpdateOnTime { get; set; }
        /// <summary>
        /// 参数列表
        /// </summary>
        public List<BootupParamDetailItem> ParamList { get; set; }
        public class BootupParamDetailItem
        {
            /// <summary>
            /// 参数编码
            /// </summary>
            public string ParamCode { get; set; }
            /// <summary>
            /// 参数上限
            /// </summary>
            public Decimal? ParamUpper { get; set; }
            /// <summary>
            /// 参数下限
            /// </summary>
            public Decimal? ParamLower { get; set; }
            /// <summary>
            /// 参数值
            /// </summary>
            public string ParamValue { get; set; }
        }
    }


}
