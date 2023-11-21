using Hymson.MES.SystemServices.Dtos.ProductTraceReport;
using Hymson.MES.SystemServices.Dtos.ProductTraceReport.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.SystemServices.Services.ProductTrace;

/// <summary>
/// PACK码追溯电芯码查询设备采集参数
/// </summary>
public interface IPackTraceSFCParameterService
{
    /// <summary>
    /// 获取PACK条码追溯SFC设备参数信息
    /// </summary>
    /// <param name="queryDto"></param>
    /// <returns></returns>
    public Task<IEnumerable<PackTraceSFCParameterViewDto>> PackTraceSFCParamterAsync(PackTraceSFCParameterQueryDto queryDto);
}
