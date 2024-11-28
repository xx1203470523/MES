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
}