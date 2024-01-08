using Hymson.Infrastructure;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Manufacture.ManuSfcCirculation.Query;
using Hymson.MES.Services.Dtos.Manufacture;
using Hymson.MES.Services.Dtos.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Services.Manufacture;

public class ManuSfcCirculationService : IManuSfcCirculationService
{
    private readonly IManuSfcCirculationRepository _manuSfcCirculationRepository;

    public ManuSfcCirculationService(IManuSfcCirculationRepository manuSfcCirculationRepository) {
        _manuSfcCirculationRepository = manuSfcCirculationRepository;
    
    }

    /// <summary>
    /// 分页查询数据
    /// </summary>
    /// <param name="pageQueryDto"></param>
    /// <returns></returns>
    public async Task<PagedInfo<ManuSfcCirculationViewDto>> GetPageInfoAsync(ManuSfcCirculationPagedQueryDto pageQueryDto)
    {
        var result = new PagedInfo<ManuSfcCirculationViewDto>(new List<ManuSfcCirculationViewDto>(),pageQueryDto.PageIndex,pageQueryDto.PageSize);
        var pageQuery = pageQueryDto.ToQuery<ManuSfcCirculationPagedQuery>();

        var pageInfo = await _manuSfcCirculationRepository.GetPagedInfoAsync(pageQuery);

        var listData = new List<ManuSfcCirculationViewDto>();
        foreach (var item in pageInfo.Data)
        {
            var manuSfcCirculation = item.ToModel<ManuSfcCirculationViewDto>();
            listData.Add(manuSfcCirculation);
        }

        result.Data = listData;
        result.TotalCount = pageInfo.TotalCount;

        return result;
    }

}
