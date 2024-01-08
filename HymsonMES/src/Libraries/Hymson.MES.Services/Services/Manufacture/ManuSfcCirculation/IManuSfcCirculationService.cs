using Hymson.Infrastructure;
using Hymson.MES.Data.Repositories.Manufacture.ManuSfcCirculation.Query;
using Hymson.MES.Services.Dtos.Manufacture;
using Hymson.MES.Services.Dtos.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Services.Manufacture;

/// <summary>
/// 条码流转表获取
/// </summary>
public interface IManuSfcCirculationService
{
    /// <summary>
    /// 获取分页数据
    /// </summary>
    /// <param name="pageQuery"></param>
    /// <returns></returns>
    Task<PagedInfo<ManuSfcCirculationViewDto>> GetPageInfoAsync(ManuSfcCirculationPagedQueryDto pageQuery);
}
