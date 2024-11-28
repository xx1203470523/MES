using Hymson.MES.Core.Domain.SysSetting;
using Hymson.MES.Services.Dtos.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Services.Common;

public interface ICommonService 
{
    /// <summary>
    /// 获取全局配置项
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<SysSettingEntity>> GetSettingsAsync();

    /// <summary>
    /// 保存全局配置项
    /// </summary>
    /// <param name="setting"></param>
    /// <returns></returns>
    Task SaveSettingsAsync(IEnumerable<SysSettingDto> setting);
}
