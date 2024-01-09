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

    /// <summary>
    /// 删除条码绑定关系
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<int> DeteleteManuSfcCirculationAsync(long id);

    /// <summary>
    /// 创建条码绑定关系
    /// </summary>
    /// <param name="bindDto"></param>
    /// <returns></returns>
    Task CreateManuSfcCirculationAsync(ManuSfcCirculationBindDto bindDto);

}
