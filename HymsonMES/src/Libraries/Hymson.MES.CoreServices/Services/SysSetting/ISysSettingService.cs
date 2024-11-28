using Hymson.MES.CoreServices.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.CoreServices.Services.SysSetting;

public interface ISysSettingService
{
    Task<SysSettingConfig> GetSettingsAsync();
}
