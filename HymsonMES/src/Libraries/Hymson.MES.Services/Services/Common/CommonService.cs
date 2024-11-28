using Hymson.Authentication;
using Hymson.MES.Core.Domain.SysSetting;
using Hymson.MES.Data.Repositories.SysSetting;
using Hymson.MES.Services.Dtos.Common;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Services.Common;

public class CommonService : BaseService, ICommonService
{
    private readonly ISysSettingRepository _sysSettingRepository;
    private readonly ICurrentUser _currentUser;

    public CommonService(ISysSettingRepository sysSettingRepository,
        ICurrentUser currentUser)
    {
        _sysSettingRepository = sysSettingRepository;
        _currentUser = currentUser;
    }

    /// <summary>
    /// 获取全局配置
    /// </summary>
    /// <returns></returns>
    public async Task<IEnumerable<SysSettingEntity>> GetSettingsAsync()
    {
        var result = await _sysSettingRepository.GetEntitiesAsync(new() { });
        return result;
    }
     
    /// <summary>
    /// 保存系统配置
    /// </summary>
    /// <param name="settings"></param>
    /// <returns></returns>
    public async Task SaveSettingsAsync(IEnumerable<SysSettingDto> settings)
    {
        var result = await _sysSettingRepository.GetEntitiesAsync(new() { });
        var ids = result.Select(a => a.Id).ToArray();

        List<SysSettingEntity> list = new();
        foreach (var item in settings)
        {
            SysSettingEntity sysSettingEntity = new()
            {
                Name = item.Name,
                Value = item.Value,
                SiteId = 123456
            };

            FillCreateCommand(sysSettingEntity, _currentUser.UserName);
            list.Add(sysSettingEntity);
        }

        //删除全部配置
        await _sysSettingRepository.DeletesAsync(new() { Ids = ids });

        //插入新的配置
        await _sysSettingRepository.InsertRangeAsync(list);

    }
}

