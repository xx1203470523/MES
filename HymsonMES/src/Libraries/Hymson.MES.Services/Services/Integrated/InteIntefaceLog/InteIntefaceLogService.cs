using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Integrated;
using Hymson.Elasticsearch;
using Hymson.Infrastructure.Enums;
using Hymson.MES.Core.Enums.Integrated;
using Hymson.Logging;

namespace Hymson.MES.Services.Services.Integrated.InteIntefaceLog
{
    public class InteIntefaceLogService : IInteIntefaceLogService
    {
        /// <summary>
        /// ES日志查询
        /// </summary>
        private readonly ILogDataService _logDataService;

        /// <summary>
        public InteIntefaceLogService(ILogDataService logDataService)
        {
            _logDataService = logDataService;
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<InteIntefaceLogDto>> GetPagedListAsync(InteIntefaceLogPagedQueryDto pagedQueryDto)
        {
            var logDataPagedQuery = new LogDataPagedQuery()
            {
                PageIndex = pagedQueryDto.PageIndex,
                PageSize = pagedQueryDto.PageSize,
            };
            string? queryType = string.Empty;

            if (pagedQueryDto.QueryType.HasValue)
            {
                queryType = pagedQueryDto?.QueryType!.ToString();
            }

            if (pagedQueryDto?.QueryType == InterfaceLogQueryTyeEnum.SystemLog)
            {
                logDataPagedQuery.ServiceType = ServiceTypeEnum.MES;
            }

            if (pagedQueryDto?.TimeStamp != null)
            {
                logDataPagedQuery.BeginTime = pagedQueryDto.TimeStamp[0];
                logDataPagedQuery.EndTime = pagedQueryDto.TimeStamp[1].AddDays(1);
            }

            if (pagedQueryDto?.Id != null)
            {
                logDataPagedQuery.Id = pagedQueryDto.Id;
            }
            else
            {
                logDataPagedQuery.Type = queryType;
            }

            if (pagedQueryDto?.InterfaceCode != null)
            {
                logDataPagedQuery.InterfaceCode = pagedQueryDto.InterfaceCode;
            }

            if (pagedQueryDto?.InterfaceName != null)
            {
                logDataPagedQuery.Message = pagedQueryDto.InterfaceName;
            }

            var data = new Dictionary<string, string> { };

            if (pagedQueryDto?.Requestor != null)
            {
                data.Add("Name", pagedQueryDto.Requestor);
            }

            if (pagedQueryDto?.Responsetor != null)
            {
                data.Add("ReceiverType", pagedQueryDto.Responsetor);
            }

            if (pagedQueryDto?.ResponseResult != null)
            {
                var isSuccess = (int)pagedQueryDto.ResponseResult;
                data.Add("IsSuccess", isSuccess.ToString() ?? "0");
            }

            logDataPagedQuery.Data = data;

            var getlogdate = await _logDataService.GetLogDataPagedAsync(logDataPagedQuery);

            List<InteIntefaceLogDto> dtos = new();

            foreach (var item in getlogdate.Data)
            {
                dtos.Add(MapToDto(item));
            }

            return new PagedInfo<InteIntefaceLogDto>(dtos, getlogdate.PageIndex, getlogdate.PageSize, getlogdate.TotalCount);
        }

        private InteIntefaceLogDto MapToDto(TraceLogEntry entry)
        {
            var dto = new InteIntefaceLogDto
            {
                Id = entry.Id,
                InterfaceCode = entry.InterfaceCode ?? entry.Data["Path"],
                InterfaceName = entry.Message,
                Requestor = entry.Data.ContainsKey("Name") ? entry.Data["Name"] : string.Empty,
                Responsetor = entry.Data.ContainsKey("ReceiverType") ? entry.Data["ReceiverType"] : string.Empty,
                RequestTime = entry.Data.ContainsKey("RequestTime") ? entry.Data["RequestTime"] : string.Empty,
                ResponseTime = entry.Data.ContainsKey("ResponseTime") ? entry.Data["ResponseTime"] : string.Empty,
                Cost = entry.Data.ContainsKey("Cost") ? entry.Data["Cost"] : string.Empty,
                Ip = entry.Data.ContainsKey("Ip") ? entry.Data["Ip"] : string.Empty,
                Method = entry.Data["Method"],
                RequestBody = entry.Data["RequestBody"],
                ResponseBody = entry.Data["ResponseBody"],
                Path = entry.Data["Path"]
            };

            if (entry.Data.ContainsKey("IsSuccess") && int.TryParse(entry.Data["IsSuccess"], out int isSuccessResult))
            {
                dto.ResponseResult = (ResponseResultEnum)isSuccessResult;
            }

            return dto;
        }

    }
}
