using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.EquipmentServices.Dtos.EquipmentCollect;

public record EquipmentBootupParamVersonCheckDto : BaseDto
{
    /// <summary>
    /// 配方编码
    /// </summary>
    public string RecipeCode { get; set; }
    /// <summary>
    /// 版本
    /// </summary>
    public string Version { get; set; }
}