using Hymson.Authentication;
using Hymson.Authentication.JwtBearer;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Excel.Abstractions;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Domain.Equipment;
using Hymson.MES.Data.Repositories.Equipment;
using Hymson.MES.Data.Repositories.Equipment.EquEquipment;
using Hymson.MES.Data.Repositories.Equipment.EquEquipment.Query;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Services.Dtos.Common;
using Hymson.MES.Services.Dtos.Equipment;
using Hymson.MES.Services.Dtos.Report;
using Hymson.Minio;
using Hymson.Utils;
using Hymson.MES.Core.Constants;
using System.Data;

namespace Hymson.MES.Services.Services.Report.EquHeartbeatReport
{
    public class EquOeeReportService : IEquOeeReportService
    {
        private readonly ICurrentUser _currentUser;
        private readonly ICurrentSite _currentSite;
        private readonly IEquEquipmentRepository _equipmentRepository;
        private readonly IEquHeartbeatRepository _equHeartbeatRepository;

        private readonly IManuSfcSummaryRepository _manuSfcSummaryRepository;
        private readonly IEquStatusRepository _equStatusRepository;

        private readonly IMinioService _minioService;
        private readonly IExcelService _excelService;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="equipmentRepository"></param>
        /// <param name="equHeartbeatRepository"></param>
        /// <param name="currentUser"></param>
        /// <param name="currentSite"></param>
        /// <param name="minioService"></param>
        /// <param name="excelService"></param>
        public EquOeeReportService(
            IEquEquipmentRepository equipmentRepository,
            IEquHeartbeatRepository equHeartbeatRepository,
            IManuSfcSummaryRepository manuSfcSummaryRepository,
            IEquStatusRepository equStatusRepository,
            ICurrentUser currentUser,
            ICurrentSite currentSite,
            IMinioService minioService,
            IExcelService excelService)
        {
            _equipmentRepository = equipmentRepository;
            _equHeartbeatRepository = equHeartbeatRepository;
            _manuSfcSummaryRepository = manuSfcSummaryRepository;
            _equStatusRepository = equStatusRepository;
            _currentUser = currentUser;
            _currentSite = currentSite;
            _minioService = minioService;
            _excelService = excelService;
        }

        /// <summary>
        /// 查询OEE表分页数据
        /// </summary>
        /// <param name="pageQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<EquOeeReportViewDto>> GetEquOeeReportPageListAsync(EquOeeReportPagedQueryDto pageQuery)
        {
            var pagedQuery = pageQuery.ToQuery<EquOeeReportPagedQuery>();
            pagedQuery.SiteId = _currentSite.SiteId ?? 123456;
            var now = HymsonClock.Now();
            var checkTime = pageQuery.QueryTime[0];
            var DayStartTime = new DateTime(checkTime.Year, checkTime.Month, checkTime.Day, 08, 30, 0); // 开始时间
            var DayEndTime = new DateTime(checkTime.Year, checkTime.Month, checkTime.Day, 20, 30, 0); // 开始时间
            if (pageQuery.DayShift == 0) //白班+夜班
            {
                DayStartTime = new DateTime(checkTime.Year, checkTime.Month, checkTime.Day, 08, 30, 0); // 开始时间
                checkTime = pageQuery.QueryTime[1].AddDays(1); //第二天早上8点30
                DayEndTime = new DateTime(checkTime.Year, checkTime.Month, checkTime.Day, 08, 30, 0); // 开始时间
            }
            else if (pageQuery.DayShift == 1) //白班
            {
                DayStartTime = new DateTime(checkTime.Year, checkTime.Month, checkTime.Day, 08, 30, 0); // 开始时间
                DayEndTime = new DateTime(checkTime.Year, checkTime.Month, checkTime.Day, 20, 30, 0); // 开始时间
            }
            else if (pageQuery.DayShift == 2) //夜班
            {
                DayStartTime = new DateTime(checkTime.Year, checkTime.Month, checkTime.Day, 20, 30, 0); // 开始时间
                checkTime = checkTime.AddDays(1); //第二天早上8点30
                DayEndTime = new DateTime(checkTime.Year, checkTime.Month, checkTime.Day, 08, 30, 0); // 开始时间
            }

            if (DayStartTime >= now)
            {
                DayStartTime = now;
            }
            if (DayEndTime>= now)
            {
                DayEndTime = now;
            }

            var workingHours = Convert.ToInt32((DayEndTime - DayStartTime).TotalMinutes);//12;//正常出勤时间/计划时间(小时)

            var dtos = new List<EquOeeReportViewDto>();
            //查询需要统计的设备信息
            var equEquipmentEntities = await _equipmentRepository.GetPagedListAsync(new EquEquipmentPagedQuery { SiteId = pagedQuery.SiteId, EquipmentCodes = pagedQuery.EquipmentCodes, PageIndex = pageQuery.PageIndex, PageSize = 1000 });
            var equIds = equEquipmentEntities.Data.Select(c => c.Id).ToArray();
            if (!equIds.Any())
            {
                return new PagedInfo<EquOeeReportViewDto>(dtos, pageQuery.PageIndex, pageQuery.PageSize);
            }
           

            //查询设备当天异常时长汇总（除了运行以外的状态）
            var equipmentStopTimeDtos = await GetEquipmentStopTimeAsync(pagedQuery.SiteId, equIds, DayStartTime, DayEndTime);
            //查询设备当天生产数据汇总
            var equipmentYieldDtos = await GetEquipmentYieldAsync(pagedQuery.SiteId, equIds, DayStartTime, DayEndTime);
            var equTheoryDtos = await GetEquipmentTheoryAsync(pageQuery.EquipmentCodes);

           
            //统计设备OEE
            foreach (var equEquipmentEntity in equEquipmentEntities.Data)
            {
                //当前设备
                string equCode = equEquipmentEntity.EquipmentCode;

                var equTheoryEntity = equTheoryDtos.Where(c => c.EquipmentCode == equCode)?.FirstOrDefault();
                if (equTheoryEntity == null)
                {
                    continue;
                }
                var TheoryOutputQty = equTheoryEntity?.TheoryOutputQty;//理论ct(s)
                var theory = equTheoryEntity?.OutputQty;//每小时产量
               
                //获取当前设备异常时长
                var equipmentStopTime = equipmentStopTimeDtos?.Where(c => c.EquipmentId == equEquipmentEntity?.Id)?.FirstOrDefault();
                //获取当前设备生产数据
                var equipmentYield = equipmentYieldDtos?.Where(c => c.EquipmentId == equEquipmentEntity?.Id)?.FirstOrDefault();
                //计划工作分钟
                var workingSeconds = workingHours * 60;
                var lostTimeDuration = (equipmentStopTime?.StopSeconds ?? 0); // 停机时长
                if (workingSeconds < lostTimeDuration)
                {
                    lostTimeDuration = workingSeconds;
                }
                //操作时间
                var operateTime = workingSeconds - lostTimeDuration;
                var equipmentUtilizationRateDto = new EquOeeReportViewDto
                {
                    EquipmentId = equEquipmentEntity?.Id ?? 0,
                    EquipmentCode = equCode,
                    EquipmentName = equEquipmentEntity?.EquipmentName ?? "",
                    PlanTimeDuration = workingSeconds,
                    WorkTimeDuration = operateTime,
                    LostTimeDuration = lostTimeDuration,
                    TheoryOutputQty = theory,
                    OutputQty = (equipmentYield?.Total ?? 0),
                    OutputSumQty = (equipmentYield?.Total ?? 0),
                    QualifiedQty = (equipmentYield?.YieldQty ?? 0),
                };

                equipmentUtilizationRateDto.AvailableRatio = operateTime / (workingSeconds == 0 ? 1 : workingSeconds);
                if (theory == null || theory == 0)
                {
                    equipmentUtilizationRateDto.WorkpieceRatio = 0;
                }
                else
                {
                    if ((equipmentYield?.Total ?? 0) > theory)
                    {
                        equipmentUtilizationRateDto.WorkpieceRatio = 1;
                    }
                    else
                    {
                        equipmentUtilizationRateDto.WorkpieceRatio = (equipmentYield?.Total ?? 0) / (theory ?? 1);
                    }
                }

                if (equipmentYield == null || equipmentYield?.Total == 0)
                {
                    equipmentUtilizationRateDto.QualifiedRatio = 0;
                }
                else
                {
                    equipmentUtilizationRateDto.QualifiedRatio = (equipmentYield?.YieldQty ?? 0) / (equipmentYield?.Total ?? 1);
                }

                equipmentUtilizationRateDto.Oee = decimal.Parse((equipmentUtilizationRateDto.AvailableRatio * equipmentUtilizationRateDto.WorkpieceRatio * equipmentUtilizationRateDto.QualifiedRatio).ToString("0.00") ?? "0");


                dtos.Add(equipmentUtilizationRateDto);
            }
            var res = new PagedInfo<EquOeeReportViewDto>(dtos, pageQuery.PageIndex, dtos.Count);
            return res;
        }

        /// <summary>
        /// 导出OEE
        /// </summary>
        /// <param name="pageQuery"></param>
        /// <returns></returns>
        public async Task<ExportResultDto> EquOeeReportExportAsync(EquOeeReportPagedQueryDto pageQuery)
        {
            string fileName = "设备OEE";
            pageQuery.PageSize = 10000;
            var pagedInfo = await GetEquOeeReportPageListAsync(pageQuery);
            var equOeeReports = pagedInfo.Data;
            var equOeeExports = new List<EquOeeReportExportDto>();
            foreach (var equOeeReport in equOeeReports)
            {
                equOeeExports.Add(new EquOeeReportExportDto()
                {
                    EquipmentCode = equOeeReport.EquipmentCode,
                    EquipmentName = equOeeReport.EquipmentName,
                    PlanTimeDuration = equOeeReport.PlanTimeDuration ?? 0,
                    WorkTimeDuration = equOeeReport.WorkTimeDuration ?? 0,
                    LostTimeDuration = equOeeReport.LostTimeDuration ?? 0,
                    TheoryOutputQty = equOeeReport.TheoryOutputQty ?? 0,
                    OutputQty = equOeeReport.OutputQty ?? 0,
                    OutputSumQty = equOeeReport.OutputSumQty ?? 0,
                    QualifiedQty = equOeeReport.QualifiedQty ?? 0,
                    AvailableRatio = equOeeReport.AvailableRatio,
                    WorkpieceRatio = equOeeReport.WorkpieceRatio,
                    QualifiedRatio = equOeeReport.QualifiedRatio,
                    Oee = equOeeReport.Oee
                });
            }
            var filePath = await _excelService.ExportAsync(equOeeExports, fileName, fileName);
            //上传到文件服务器
            var uploadResult = await _minioService.PutObjectAsync(filePath);
            return new ExportResultDto
            {
                FileName = fileName,
                Path = uploadResult.RelativeUrl,
            };
        }

        /// <summary>
        /// 查询设备理论产量
        /// </summary>
        /// <param name="equipmentCodes"></param>
        /// <returns></returns>
        private async Task<IEnumerable<EquEquipmentTheoryEntity>> GetEquipmentTheoryAsync(string[]? equipmentCodes)
        {
            //查询设备状态变化记录
            var equTheoryQuery = new EquEquipmentTheoryQuery
            {
                EquipmentCodes = equipmentCodes
            };
            var equTheoryEntities = await _equStatusRepository.GetEquipmentTheoryAsync(equTheoryQuery);
            return equTheoryEntities;
        }

        /// <summary>
        /// 查询当天设备停机时长汇总
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="equipentIds"></param>
        /// <returns></returns>
        private async Task<IEnumerable<EquipmentStopTimeDto>> GetEquipmentStopTimeAsync(long siteId, long[] equipentIds, DateTime DayStartTime, DateTime DayEndTime)
        {
            //查询设备状态变化记录
            var equStatusQuery = new EquStatusStatisticsQuery
            {
                SiteId = siteId,
                EquipmentIds = equipentIds,
                StartTime = DayStartTime,
                EndTime = DayEndTime
            };
            var equStatusStatisticsEntities = await _equStatusRepository.GetEquStatusStatisticsEntitiesAsync(equStatusQuery);
            var equipmentStopTimeDtos = equStatusStatisticsEntities
                            .Where(c => c.EquipmentStatus != Core.Enums.EquipmentStateEnum.AutoRun)//过滤正常状态
                            .OrderBy(c => c.CreatedOn)
                            .GroupBy(c => c.EquipmentId)
                            .Select(group => new EquipmentStopTimeDto
                            {
                                EquipmentId = group.Max(c => c.EquipmentId),
                                StopSeconds = group.Sum(c => (c.EndTime - c.BeginTime)?.TotalMinutes ?? 0).ParseToDecimal()//停机总时长
                            });
            return equipmentStopTimeDtos;
        }

        /// <summary>
        /// 获取设备当天生产数据
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="equipentIds"></param>
        /// <returns></returns>
        private async Task<IEnumerable<EquipmentYieldDto>> GetEquipmentYieldAsync(long siteId, long[] equipentIds, DateTime DayStartTime, DateTime DayEndTime)
        {
            //查询当天汇总数据 包含良品和不良
            var manuSfcSummaryQuery = new ManuSfcSummaryQuery
            {
                SiteId = siteId,
                EquipmentIds = equipentIds,
                StartTime = DayStartTime,
                EndTime = DayEndTime
            };
            var manuSfcSummaryEntities = await _manuSfcSummaryRepository.GetManuSfcSummaryEntitiesAsync(manuSfcSummaryQuery);
            var equipmentYieldDtos = manuSfcSummaryEntities
                                    .GroupBy(c => c.EquipmentId)
                                    .Select(group => new EquipmentYieldDto
                                    {
                                        EquipmentId = group.Max(c => c.EquipmentId),
                                        Total = group.Count(),//总数
                                        YieldQty = group.Where(c => c.QualityStatus == 1).Count()//良品数
                                    });
            return equipmentYieldDtos;
        }

    }
}
