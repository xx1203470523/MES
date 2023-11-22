using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Core.Constants;
using Hymson.MES.Data.Repositories.Report;
using Hymson.MES.SystemServices.Dtos.ProductTraceReport;
using Hymson.MES.SystemServices.Dtos.ProductTraceReport.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.SystemServices.Services.ProductTrace;

public class PackTraceSFCParameterService : IPackTraceSFCParameterService
{
    #region 依赖注入

    private readonly IPackTraceSFCParameterRepository _packTraceSFCParameterRepository;

    #endregion

    /// <summary>
    /// 构造函数
    /// </summary>
    public PackTraceSFCParameterService(IPackTraceSFCParameterRepository packTraceSFCParameterRepository)
    {
        _packTraceSFCParameterRepository = packTraceSFCParameterRepository;

    }

    /// <summary>
    /// PACK条码追溯SFC设备参数信息
    /// </summary>
    /// <param name="queryDto"></param>
    /// <returns></returns>
    /// <exception cref="CustomerValidationException"></exception>
    public async Task<IEnumerable<PackTraceSFCParameterViewOutput>> PackTraceSFCParamterAsync(PackTraceSFCParameterQueryDto queryDto)
    {
        if (queryDto.SFC?.Any() == false)
        {
            throw new CustomerValidationException(nameof(ErrorCode.MES19203));
        }

        PackTraceSFCParameterQuery query = new() { SFC = queryDto.SFC };

        var list = await _packTraceSFCParameterRepository.GetListAsync(query);

        List<PackTraceSFCParameterViewDto> result = new();
        foreach (var item in list)
        {
            result.Add(new()
            {
                EquipmentName = item.EquipmentName,
                JudgmentResult = item.JudgmentResult,
                LocalTime = item.LocalTime,
                Pack = item.Pack,
                SFC = item.SFC,
                ParameterCode = item.ParameterCode,
                ParameterName = item.ParameterName,
                ParameterValue = item.ParameterValue,
                ProcedureCode = item.ProcedureCode,
                ProcedureName = item.ProcedureName,
                StandardLowerLimit = item.StandardLowerLimit,
                StandardUpperLimit = item.StandardUpperLimit
            });
        }

        List<PackTraceSFCParameterViewOutput> outputResult = new();
        foreach (var item in queryDto.SFC)
        {
            var paramdetail = result.Where(a=>a.Pack == item);

            PackTraceSFCParameterViewOutput detail = new()
            {
                Pack = item,
                TestRecordlist = paramdetail
            };
            outputResult.Add(detail);
        }

        return outputResult;
    }


}

