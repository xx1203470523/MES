﻿using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Excel.Abstractions;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Data.Repositories.Equipment;
using Hymson.MES.Services.Dtos.Common;
using Hymson.MES.Services.Dtos.Equipment;
using Hymson.MES.Services.Dtos.Report;
using Hymson.Minio;

namespace Hymson.MES.Services.Services.Report.EquAlarmReport
{
    public class EquAlarmReportService : IEquAlarmReportService
    {
        private readonly ICurrentUser _currentUser;
        private readonly ICurrentSite _currentSite;
        private readonly IEquAlarmRepository _equAlarmRepository;
        private readonly IMinioService _minioService;
        private readonly IExcelService _excelService;
        public EquAlarmReportService(IEquAlarmRepository equAlarmRepository,
            ICurrentUser currentUser,
            ICurrentSite currentSite,
            IMinioService minioService,
            IExcelService excelService)
        {
            _equAlarmRepository = equAlarmRepository;
            _currentUser = currentUser;
            _currentSite = currentSite;
            _minioService = minioService;
            _excelService = excelService;
        }

        /// <summary>
        /// 根据查询条件获取设备报警报表分页数据
        /// </summary>
        /// <param name="pageQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<EquAlarmReportViewDto>> GetEquAlarmReportPageListAsync(EquAlarmReportPagedQueryDto pageQuery)
        {
            var pagedQuery = pageQuery.ToQuery<EquAlarmReportPagedQuery>();
            pagedQuery.SiteId = _currentSite.SiteId ?? 0;
            var pagedInfo = await _equAlarmRepository.GetEquAlarmReportPageListAsync(pagedQuery);
            var dtos = pagedInfo.Data.Select(s =>
            {
                return s.ToModel<EquAlarmReportViewDto>();
            });
            return new PagedInfo<EquAlarmReportViewDto>(dtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }
        /// <summary>
        /// 数据导出
        /// </summary>
        /// <param name="pageQuery"></param>
        /// <returns></returns>
        public async Task<ExportResultDto> EquAlarmReportExportAsync(EquAlarmReportPagedQueryDto pageQuery)
        {
            string fileName = "设备报警";
            pageQuery.PageSize = 10000;
            var pagedQuery = pageQuery.ToQuery<EquAlarmReportPagedQuery>();
            pagedQuery.SiteId = _currentSite.SiteId ?? 0;
            var pagedInfo = await _equAlarmRepository.GetEquAlarmReportPageListAsync(pagedQuery);
            var equAlarmReports = pagedInfo.Data;
            var equHeartbeaReportExports = new List<EquAlarmReportExportDto>();
            foreach (var equAlarmReport in equAlarmReports)
            {
                equHeartbeaReportExports.Add(equAlarmReport.ToExcelModel<EquAlarmReportExportDto>());
            }
            var filePath = await _excelService.ExportAsync(equHeartbeaReportExports, fileName, fileName);
            //上传到文件服务器
            var uploadResult = await _minioService.PutObjectAsync(filePath);
            return new ExportResultDto
            {
                FileName = fileName,
                Path = uploadResult.RelativeUrl,
                RelativePath = uploadResult.RelativeUrl
            };
        }

        /// <summary>
        /// 获取设备报警持续时间
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<EquAlarmDurationTimeDto>> GetEquAlarmDurationTimeAsync(EquAlarmDurationTimeQueryDto query)
        {
            List<EquAlarmDurationTimeDto> result = new();
            var equAlarmEntities = await _equAlarmRepository.GetListAsync(new()
            {
                EquipmentId = query.EquipmentId,
                CreatedOnEnd = query.EndTime,
                CreatedOnStart = query.BeginTime
            });

            List<EquAlarmComputedDto> list = new List<EquAlarmComputedDto>();
            foreach (var item in equAlarmEntities)
            {
                EquAlarmComputedDto newitem = new EquAlarmComputedDto();
                newitem.Status = item.Status;
                newitem.EquipmentId = item.EquipmentId;

                var exitsItem = list.FirstOrDefault(a => a.EquipmentId == item.EquipmentId);

                if (exitsItem != null)
                {
                    //触发
                    if (item.Status == Core.Enums.EquipmentAlarmStatusEnum.Trigger)
                    {
                        if (item.Status == exitsItem.Status) continue;
                        else
                        {
                            exitsItem.Status = item.Status;
                            exitsItem.BeginTime = item.LocalTime;
                            exitsItem.EndTime = null;
                        }
                    }
                    else
                    {
                        if (item.Status == exitsItem.Status) continue;
                        else
                        {
                            if (exitsItem.BeginTime != null && exitsItem.EndTime != null)
                            {
                                exitsItem.DurationTime += exitsItem.EndTime.GetValueOrDefault().Subtract(exitsItem.BeginTime.GetValueOrDefault()).Milliseconds;
                                exitsItem.BeginTime = null;
                                exitsItem.EndTime = null;
                                exitsItem.Status = null;
                            }
                        }
                    }
                }
                else
                {

                    if (newitem.Status == Core.Enums.EquipmentAlarmStatusEnum.Trigger)
                        newitem.BeginTime = item.LocalTime;
                    else
                        newitem.EndTime = item.LocalTime;

                    if (newitem.BeginTime != null && newitem.EndTime != null)
                        newitem.DurationTime = newitem.EndTime.GetValueOrDefault().Subtract(newitem.BeginTime.GetValueOrDefault()).Milliseconds;
                }

                list.Add(newitem);

            }

            foreach (var item in list)
            {
                result.Add(new() { 
                    EquipmentId = item.EquipmentId,
                    DurationTime = item.DurationTime
                });
            }

            return result;
        }
    }
}
