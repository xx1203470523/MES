using Hymson.MES.CoreServices.Dtos;
using Hymson.MES.Data.Repositories.SysSetting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.CoreServices.Services.SysSetting;

public class SysSettingService : ISysSettingService
{
    private readonly ISysSettingRepository _sysSettingRepository;

    public SysSettingService(ISysSettingRepository sysSettingRepository)
    {
        _sysSettingRepository = sysSettingRepository;
    }

    /// <summary>
    /// 获取全局配置
    /// </summary>
    /// <returns></returns>
    public async Task<SysSettingConfig> GetSettingsAsync()
    {
        // 获取所有配置项
        var sysSettings = await _sysSettingRepository.GetEntitiesAsync(new());
        var result = new SysSettingConfig
        {
            StrictProductionFollowingTheProcessRoute = true,
            PackOfflineValidation = true
        };

        if (sysSettings == null || !sysSettings.Any())
        {
            // 如果没有任何配置项，返回默认配置
            return result;
        }

        // 将配置项转为字典以优化查询性能（忽略大小写）
        var settingsDict = sysSettings.ToDictionary(
            setting => setting.Name.ToLower(),
            setting => setting.Value == "1"?true:false
        );

        settingsDict.TryGetValue("strictproductionfollowingtheprocessroute", out var setting1);
        settingsDict.TryGetValue("packofflinevalidation", out var setting2);

        result.StrictProductionFollowingTheProcessRoute = setting1;
        result.PackOfflineValidation = setting2;

        // 尝试解析布尔值，默认值为 false
        return result;
    }
}
