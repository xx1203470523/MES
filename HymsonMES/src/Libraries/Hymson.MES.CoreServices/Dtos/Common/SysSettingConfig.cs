using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.CoreServices.Dtos;

/// <summary>
/// 系统配置对象
/// </summary>
public class SysSettingConfig
{
    /// <summary>
    /// 是否开启严格工艺路线管控
    /// </summary>
    public bool StrictProductionFollowingTheProcessRoute { get; set; } = true;

    /// <summary>
    /// 是否开启Pack下线校验管控
    /// </summary>
    public bool PackOfflineValidation { get; set; } = true;

    /// <summary>
    /// 液冷板码校验
    /// </summary>
    public string SfcValidateRule { get; set; } = "CZHC$";

    /// <summary>
    /// 是否开启校验1
    /// </summary>
    public bool rule1 { get; set; } 

    /// <summary>
    /// 校验1_变量1（正则表达式）
    /// </summary>
    public string rule1_input1 { get; set; }

    /// <summary>
    /// 校验1_变量2（长度）
    /// </summary>
    public string rule1_input2 { get; set; }

    /// <summary>
    /// 校验1_变量3（管控周期）
    /// </summary>
    public int rule1_input3 { get; set; }

    /// <summary>
    /// 是否开启校验2
    /// </summary>
    public bool rule2 { get; set; }

    /// <summary>
    /// 校验2_变量1（正则表达式）
    /// </summary>
    public string rule2_input1 { get; set; }

    /// <summary>
    /// 校验2_变量2（长度）
    /// </summary>
    public string rule2_input2 { get; set; }

    /// <summary>
    /// 校验2_变量3（管控周期）
    /// </summary>
    public int rule2_input3 { get; set; }

    /// <summary>
    /// 是否开启校验3
    /// </summary>
    public bool rule3 { get; set; }

    /// <summary>
    /// 校验3_变量1（正则表达式）
    /// </summary>
    public string rule3_input1 { get; set; }

    /// <summary>
    /// 校验3_变量2（长度）
    /// </summary>
    public string rule3_input2 { get; set; }

    /// <summary>
    /// 校验3_变量3（管控周期）
    /// </summary>
    public int rule3_input3 { get; set; }

    /// <summary>
    /// 是否开启校验4
    /// </summary>
    public bool rule4 { get; set; }

    /// <summary>
    /// 校验4_变量1（正则表达式）
    /// </summary>
    public string rule4_input1 { get; set; }

    /// <summary>
    /// 校验4_变量2（长度）
    /// </summary>
    public string rule4_input2 { get; set; }

    /// <summary>
    /// 校验4_变量3（管控周期）
    /// </summary>
    public int rule4_input3 { get; set; }


}