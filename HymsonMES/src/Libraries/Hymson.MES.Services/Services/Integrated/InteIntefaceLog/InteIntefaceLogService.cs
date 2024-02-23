﻿using Hymson.Authentication.JwtBearer.Security;
using Hymson.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hymson.Infrastructure;
using Hymson.MES.Data.Repositories.Integrated.Query;
using Hymson.MES.Services.Dtos.Integrated;
using Hymson.Elasticsearch;
using Hymson.Infrastructure.Enums;
using Elastic.Clients.Elasticsearch.QueryDsl;
using Hymson.MES.Core.Enums.Integrated;
using static Mysqlx.Expect.Open.Types.Condition.Types;
using Elastic.Clients.Elasticsearch.Core.Bulk;

namespace Hymson.MES.Services.Services.Integrated.InteIntefaceLog
{
    public class InteIntefaceLogService : IInteIntefaceLogService
    {

        /// <summary>
        /// 当前用户
        /// </summary>
        private readonly ICurrentUser _currentUser;
        /// <summary>
        /// 当前站点
        /// </summary>
        private readonly ICurrentSite _currentSite;

        /// <summary>
        /// ES日志查询
        /// </summary>
        private readonly ILogDataService _logDataService;

        /// <summary>
        /// 获取TOKEN Info
        /// </summary>
        private readonly IInteSystemTokenService _inteSystemTokenService;

        /// <summary>
        public InteIntefaceLogService(ICurrentUser currentUser, ICurrentSite currentSite, ILogDataService logDataService, IInteSystemTokenService inteSystemTokenService)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _logDataService = logDataService;
            _inteSystemTokenService = inteSystemTokenService;
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

            if (pagedQueryDto.TimeStamp != null)
            {
                logDataPagedQuery.BeginTime = pagedQueryDto.TimeStamp[0];
                logDataPagedQuery.EndTime = pagedQueryDto.TimeStamp[1].AddDays(1);
            }

            if (pagedQueryDto.Id != null)
            {
                logDataPagedQuery.Id = pagedQueryDto.Id;
            }


            if (pagedQueryDto.InterfaceCode != null)
            {
                logDataPagedQuery.InterfaceCode = pagedQueryDto.InterfaceCode;
            }

            if (pagedQueryDto.InterfaceName != null)
            {
                logDataPagedQuery.Message = pagedQueryDto.InterfaceName;
            }

            var Data = new Dictionary<string, string> { };

            if (pagedQueryDto.Requestor != null)
            {
                Data.Add("Name", pagedQueryDto.Requestor);
            }

            if (pagedQueryDto.Responsetor != null)
            {
                Data.Add("ReceiverType", pagedQueryDto.Responsetor);
            }

            if (pagedQueryDto.ResponseResult != null)
            {
                Data.Add("IsSuccess", pagedQueryDto?.ResponseResult.ToString() ?? "0");
            }


            string queryType = Enum.GetName(typeof(InterfaceLogQueryTyeEnum), pagedQueryDto?.QueryType ?? 0) ?? "EquipmentLog";

            logDataPagedQuery.Type = queryType;
            var getlogdate = await _logDataService.GetLogDataPagedAsync(logDataPagedQuery);

            var ids = getlogdate.Data.Select(x => x.Id).Distinct().ToArray();
            long[] longArray = ConvertToLongArray(ids);
            var logDataInfo = await _inteSystemTokenService.QueryInteSystemTokenByIdsAsync(longArray);

            // 实体到DTO转换 装载数据
            List<InteIntefaceLogDto> dtos = new();
            foreach (var item in getlogdate.Data)
            {
                var currentData = logDataInfo.Where(x => x.Id.ToString() == item.Id).FirstOrDefault();
                var dict = item.Data;

                // 提取特定键的值并赋给对应变量
                string method = dict["Method"];
                string queryString = dict["QueryString"];
                string cost = dict["Cost"];
                string id = dict["Id"];
                string name = dict["Name"];
                string responseBody = dict["ResponseBody"];
                string receiverType = string.Empty;
                if (dict.ContainsKey("ReceiverType"))
                {
                    receiverType = dict["ReceiverType"];
                }

                string path = dict["Path"];
                string ip = dict["Ip"];
                string businessType = dict["BusinessType"];
                string requestBody = dict["RequestBody"];

                string requestTime = string.Empty;
                if (dict.ContainsKey("RequestTime"))
                {
                    requestTime = dict["RequestTime"];
                }

                string responseTime = string.Empty;
                if (dict.ContainsKey("ResponseTime"))
                {
                    responseTime = dict["ResponseTime"];
                }

                string isSuccess = string.Empty;
                if (dict.ContainsKey("IsSuccess"))
                {
                    isSuccess = dict["IsSuccess"];
                }


                int.TryParse(isSuccess, out int isResult);
                ResponseResultEnum isSuccessEnum = (ResponseResultEnum)isResult;


                dtos.Add(new InteIntefaceLogDto()
                {
                    Id = item.Id,
                    InterfaceCode = item.InterfaceCode,
                    InterfaceName = name,
                    Requestor = item.Message,
                    Responsetor = receiverType,
                    RequestTime = requestTime,
                    //返回时间
                    ResponseTime = responseTime,
                    ResponseResult = isSuccessEnum,
                    RequestBody = requestBody,
                    Path = path,
                    ResponseBody = responseBody,
                    Method = method

                });
            }

            return new PagedInfo<InteIntefaceLogDto>(dtos, getlogdate.PageIndex, getlogdate.PageSize, getlogdate.TotalCount);
        }

        //public async Task<InteIntefaceLogDto?> QueryByIdAsync(long id)
        //{
        //    string _id=id.ToString();
        //    var traceLogEntry = await _logDataService.GetTraceLogEntryByIdAsync(_id);
        //}

        private long[] ConvertToLongArray(string[] stringArray)
        {
            long[] longArray = new long[stringArray.Length];
            for (int i = 0; i < stringArray.Length; i++)
            {
                if (long.TryParse(stringArray[i], out long result))
                {
                    longArray[i] = result;
                }
                else
                {
                    // 处理无法转换为 long 的情况，这里简单地将其设置为 0
                    longArray[i] = 0;
                }
            }
            return longArray;
        }
    }
}
