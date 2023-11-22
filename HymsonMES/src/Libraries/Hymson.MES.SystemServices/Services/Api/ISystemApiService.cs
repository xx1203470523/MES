using Hymson.MES.Data.Repositories.SystemApi;
using Hymson.MES.SystemServices.Dtos.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.SystemServices.Services.Api;

/// <summary>
/// 条码流转查询服务
/// </summary>
public interface ISystemApiService
{
    /// <summary>
    /// 获取条码信息
    /// </summary>
    /// <param name="sfc"></param>
    /// <returns></returns>
    Task<IEnumerable<GetSFCInfoViewDto>> GetSFCInfoAsync(GetSFCInfoQueryDto sfc);

    /// <summary>
    /// 更新在制状态
    /// </summary>
    /// <param name="isNext">是否更新到下个工序</param>
    /// <param name="SFC">条码</param>
    /// <returns></returns>
    Task UpdateManuSFCProduceStatus(string isNext, string SFC);
}
