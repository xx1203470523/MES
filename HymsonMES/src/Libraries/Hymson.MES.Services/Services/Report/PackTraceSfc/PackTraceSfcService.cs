
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Core.Constants;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Services.Dtos.Report;
using Hymson.MES.Services.Dtos.Report.PackTraceSfcDto;
using Hymson.Utils;

namespace Hymson.MES.Services.Services.Report.PackTraceSfc;


public class PackTraceSfcService : IPackTraceSfcService
{
    private readonly IManuSfcCirculationRepository _manuSfcCirculationRepository;

    public PackTraceSfcService(IManuSfcCirculationRepository manuSfcCirculationRepository)
    {
        _manuSfcCirculationRepository = manuSfcCirculationRepository;
    }

    public async Task<PagedInfo<PackTraceSfcViewDto>> GetPageInfoAsync(PackTraceSfcQueryDto queryDto)
    {
        PagedInfo<PackTraceSfcViewDto> result = new PagedInfo<PackTraceSfcViewDto>(new List<PackTraceSfcViewDto>(),queryDto.PageIndex,queryDto.PageSize);

        if (queryDto.SFC == null)
        {
            if (queryDto.EndTime == null || queryDto.BeginTime == null)
                throw new CustomerValidationException(nameof(ErrorCode.MES19401));
        }
        else
        {
            queryDto.SFCs = queryDto.SFC.Split(",");
        }

        if (queryDto.SFCs?.Any() != true) throw new CustomerValidationException(nameof(ErrorCode.MES19401));

        //获取Pack码绑定的Module码
        var moduleSfcCirculationEntities = await _manuSfcCirculationRepository.GetManuSfcCirculationBarCodeEntitiesAsync(new()
        {
            CirculationBarCodes = queryDto.SFCs.ToArray()
        });
        var manuSfcs = moduleSfcCirculationEntities.Select(a => a.SFC);

        //获取Module码绑定的SFC
        var sfcCirculationEntities = await _manuSfcCirculationRepository.GetManuSfcCirculationBarCodeEntitiesAsync(new()
        {
            CirculationBarCodes = manuSfcs.ToArray()
        });
        var manuBindSfcs = sfcCirculationEntities.Select(a => a.SFC);

        //组装数据并返回
        List<PackTraceSfcViewDto> list = new List<PackTraceSfcViewDto>();
        foreach (var pack in queryDto.SFCs)
        {
            //获取模组码集合
            var moduleList = moduleSfcCirculationEntities.Where(a => a.CirculationBarCode == pack);

            foreach (var module in moduleList)
            {
                //获取电芯码集合
                var sfcList = sfcCirculationEntities.Where(a => a.CirculationBarCode == module.SFC);

                foreach (var sfc in sfcList)
                {
                    list.Add(new PackTraceSfcViewDto()
                    {
                        Pack = pack,
                        Module = module.SFC,
                        SFC = sfc.SFC
                    });
                }

            }
        }

        result.Data = list;

        return result;
    }

    public async Task<IEnumerable<PackTraceSfcViewDto>> ExportPortAsyc(PackTraceSfcQueryDto queryDto)
    {
        if (queryDto.SFC == null)
        {
            if (queryDto.EndTime == null || queryDto.BeginTime == null)
                throw new CustomerValidationException(nameof(ErrorCode.MES19401));
        }
        else
        {
            queryDto.SFCs = queryDto.SFC.Split(",");
        }

        if (queryDto.SFCs?.Any() != true) throw new CustomerValidationException(nameof(ErrorCode.MES19401));

        //获取Pack码绑定的Module码
        var moduleSfcCirculationEntities = await _manuSfcCirculationRepository.GetManuSfcCirculationBarCodeEntitiesAsync(new()
        {
            CirculationBarCodes = queryDto.SFCs.ToArray()
        });
        var manuSfcs = moduleSfcCirculationEntities.Select(a => a.SFC);

        //获取Module码绑定的SFC
        var sfcCirculationEntities = await _manuSfcCirculationRepository.GetManuSfcCirculationBarCodeEntitiesAsync(new()
        {
            CirculationBarCodes = manuSfcs.ToArray()
        });
        var manuBindSfcs = sfcCirculationEntities.Select(a => a.SFC);

        //组装数据并返回
        List<PackTraceSfcViewDto> list = new List<PackTraceSfcViewDto>();
        foreach (var pack in queryDto.SFCs)
        {
            //获取模组码集合
            var moduleList = moduleSfcCirculationEntities.Where(a => a.CirculationBarCode == pack);

            foreach (var module in moduleList)
            {
                //获取电芯码集合
                var sfcList = sfcCirculationEntities.Where(a => a.CirculationBarCode == module.SFC);

                foreach (var sfc in sfcList)
                {
                    list.Add(new PackTraceSfcViewDto()
                    {
                        Pack = pack,
                        Module = module.SFC,
                        SFC = sfc.SFC
                    });
                }

            }
        }

        return list;
    }

}