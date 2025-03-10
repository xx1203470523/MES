using Hymson.MES.CoreServices.Dtos;
using Hymson.MES.Data.Repositories.SysSetting;
using Hymson.Utils;
using System;
using System.Collections.Generic;
using System.Configuration;
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
            PackOfflineValidation = true,
            rule1 = false,
            rule1_input1 = "",
            rule1_input2 = "",
            rule1_input3 = 0,
            rule2 = false,
            rule2_input1 = "",
            rule2_input2 = "",
            rule2_input3 = 0,
            rule3 = false,
            rule3_input1 = "",
            rule3_input2 = "",
            rule3_input3 = 0,
            rule4 = false,
            rule4_input1 = "",
            rule4_input2 = "",
            rule4_input3 = 0
        };

        if (sysSettings == null || !sysSettings.Any())
        {
            // 如果没有任何配置项，返回默认配置
            return result;
        }

        // 将配置项转为字典以优化查询性能（忽略大小写）
        var settingsDict = sysSettings.ToDictionary(
            setting => setting.Name.ToLower(),
            setting => setting.Value
        );

        settingsDict.TryGetValue("strictproductionfollowingtheprocessroute", out var setting1);
        settingsDict.TryGetValue("packofflinevalidation", out var setting2);
        settingsDict.TryGetValue("rule1",out var setting3);
        settingsDict.TryGetValue("rule1_input1", out var setting4);
        settingsDict.TryGetValue("rule1_input2", out var setting5);
        settingsDict.TryGetValue("rule1_input3", out var setting6);
        settingsDict.TryGetValue("rule2", out var setting7);
        settingsDict.TryGetValue("rule2_input1", out var setting8);
        settingsDict.TryGetValue("rule2_input2", out var setting9);
        settingsDict.TryGetValue("rule2_input3", out var setting10);
        settingsDict.TryGetValue("rule3", out var setting11);
        settingsDict.TryGetValue("rule3_input1", out var setting12);
        settingsDict.TryGetValue("rule3_input2", out var setting13);
        settingsDict.TryGetValue("rule3_input3", out var setting14);
        settingsDict.TryGetValue("rule4", out var setting15);
        settingsDict.TryGetValue("rule4_input1", out var setting16);
        settingsDict.TryGetValue("rule4_input2", out var setting17);
        settingsDict.TryGetValue("rule4_input3", out var setting18);


        result.StrictProductionFollowingTheProcessRoute = setting1 == "1" ? true : false;
        result.PackOfflineValidation = setting2 == "1" ? true : false;
        result.rule1 = setting3 == "0" ? true : false;
        result.rule1_input1 = setting4;
        result.rule1_input2 = setting5;
        result.rule1_input3 = Convert.ToInt32(setting6);
        result.rule2 = setting7 == "0" ? true : false;
        result.rule2_input1 = setting8;
        result.rule2_input2 = setting9;
        result.rule2_input3 = Convert.ToInt32(setting10);
        result.rule3 = setting11 == "0" ? true : false;
        result.rule3_input1 = setting12;
        result.rule3_input2 = setting13;
        result.rule3_input3 = Convert.ToInt32(setting14);
        result.rule4 = setting15 == "0" ? true : false;
        result.rule4_input1 = setting16;
        result.rule4_input2 = setting17;
        result.rule4_input3 = Convert.ToInt32(setting18);

        // 尝试解析布尔值，默认值为 false
        return result;
    }
}
