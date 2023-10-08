using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.EquipmentServices.Dtos.EquipmentCollect;

/// <summary>
/// 开机参数采集DTO
/// </summary>
public record BootupParamCollectDto : BaseDto
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
    public List<BootupParamCollectDtoItem> ParamList { get; set; }
    public class BootupParamCollectDtoItem
    {
        /// <summary>
        /// 参数编码
        /// </summary>
        public string ParamCode { get; set; }
        /// <summary>
        /// 参数上线
        /// </summary>
        public string ParamUpper { get; set; }
        /// <summary>
        /// 参数下限
        /// </summary>
        public string ParamLower { get; set; }
        /// <summary>
        /// 参数值
        /// </summary>
        public string ParamValue { get; set; }
        /// <summary>
        /// 参数采集到的时间
        /// </summary>
        public DateTime Timestamp { get; set; }
    }
}