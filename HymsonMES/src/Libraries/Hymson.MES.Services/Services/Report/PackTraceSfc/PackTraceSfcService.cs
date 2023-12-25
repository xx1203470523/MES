
using Hymson.Excel.Abstractions;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Core.Constants;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Services.Dtos.Common;
using Hymson.MES.Services.Dtos.Report;
using Hymson.MES.Services.Dtos.Report.Excel;
using Hymson.MES.Services.Dtos.Report.PackTraceSfcDto;
using Hymson.Minio;
using Hymson.Utils;
using Org.BouncyCastle.Crypto.Engines;
using System.Drawing.Drawing2D;

namespace Hymson.MES.Services.Services.Report.PackTraceSfc;


public class PackTraceSfcService : IPackTraceSfcService
{
    private readonly IExcelService _excelService;
    private readonly IMinioService _minioService;
    private readonly IManuSfcCirculationRepository _manuSfcCirculationRepository;

    public PackTraceSfcService(IExcelService excelService,
        IMinioService minioService,
        IManuSfcCirculationRepository manuSfcCirculationRepository)
    {
        _excelService = excelService;
        _minioService = minioService;
        _manuSfcCirculationRepository = manuSfcCirculationRepository;
    }

    /// <summary>
    /// 获取分页数据
    /// </summary>
    /// <param name="queryDto"></param>
    /// <returns></returns>
    /// <exception cref="CustomerValidationException"></exception>
    public async Task<PagedInfo<PackTraceSfcViewDto>> GetPageInfoAsync(PackTraceSfcPageQueryDto queryDto)
    {
        PagedInfo<PackTraceSfcViewDto> result = new PagedInfo<PackTraceSfcViewDto>(new List<PackTraceSfcViewDto>(), queryDto.PageIndex, queryDto.PageSize);

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
            CirculationBarCodes = queryDto.SFCs.ToArray(),
            SiteId = 123456
        });
        var manuSfcs = moduleSfcCirculationEntities.Select(a => a.SFC);

        //获取Module码绑定的SFC
        var sfcCirculationEntities = await _manuSfcCirculationRepository.GetPagedInfoAsync(new()
        {
            CirculationBarCodes = manuSfcs.ToArray(),
            PageIndex = queryDto.PageIndex,
            PageSize = queryDto.PageSize,
            SiteId = 123456
        });
        var manuBindSfcs = sfcCirculationEntities.Data.Select(a => a.SFC);

        //组装数据并返回
        List<PackTraceSfcViewDto> list = new List<PackTraceSfcViewDto>();
        foreach (var pack in queryDto.SFCs)
        {
            //获取模组码集合
            var moduleList = moduleSfcCirculationEntities.Where(a => a.CirculationBarCode == pack);

            foreach (var module in moduleList)
            {
                //获取电芯码集合
                var sfcList = sfcCirculationEntities.Data.Where(a => a.CirculationBarCode == module.SFC);

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
        result.TotalCount = sfcCirculationEntities.TotalCount;

        return result;
    }

    /// <summary>
    /// 列表查询
    /// </summary>
    /// <param name="queryDto"></param>
    /// <returns></returns>
    /// <exception cref="CustomerValidationException"></exception>
    public async Task<IEnumerable<PackTraceSfcViewDto>> GetListAsync(PackTraceSfcQueryDto queryDto)
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

    /// <summary>
    /// 导出Excel
    /// </summary>
    /// <param name="queryDto"></param>
    /// <returns></returns>
    /// <exception cref="CustomerValidationException"></exception>
    public async Task<ExportResultDto> ExportExcelAsyc(PackTraceSfcQueryDto queryDto)
    {
        string fileName = string.Format("({0}Pack码追溯电芯码报表)", DateTime.Now.ToString("yyyyMMddHHmmss"));

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
            CirculationBarCodes = queryDto.SFCs.ToArray(),
            SiteId = 123456
        });
        var manuSfcs = moduleSfcCirculationEntities.Select(a => a.SFC);

        //获取Module码绑定的SFC
        var sfcCirculationEntities = await _manuSfcCirculationRepository.GetManuSfcCirculationBarCodeEntitiesAsync(new()
        {
            CirculationBarCodes = manuSfcs.ToArray(),
            SiteId = 123456
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

        List<PackTraceSfcExcelDto> exportExcels = new List<PackTraceSfcExcelDto>();
        foreach (var item in list)
        {
            PackTraceSfcExcelDto exportExcel = new PackTraceSfcExcelDto();

            exportExcel.Pack = item.Pack;
            exportExcel.Module = item.Module;
            exportExcel.SFC = item.SFC;;

            exportExcels.Add(exportExcel);
        }

        var filePath = await _excelService.ExportAsync(exportExcels, fileName);
        //上传到文件服务器
        var uploadResult = await _minioService.PutObjectAsync(filePath);
        return new ExportResultDto
        {
            FileName = fileName,
            Path = uploadResult.AbsoluteUrl,
            RelativePath = uploadResult.RelativeUrl
        };
    }

}