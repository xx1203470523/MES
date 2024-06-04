using Hymson.MES.Core.Domain.Plan;
using Hymson.MES.Data.Repositories.Equipment;
using Hymson.MES.Data.Repositories.Equipment.EquEquipment;
using Hymson.MES.Data.Repositories.Equipment.EquEquipment.Query;
using Hymson.MES.Data.Repositories.Integrated.IIntegratedRepository;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Plan;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.Quality.IQualityRepository;
using Hymson.MES.Services.Dtos.Report;
using Hymson.Utils;
using System.Collections;
using System.Collections.Immutable;
using System.Data;
using System.Dynamic;
using System.Globalization;
using System.Security.Policy;
using System.Text.RegularExpressions;

namespace Hymson.MES.Services.Services.Report.ProductionManagePanel
{
    /// <summary>
    /// 生产管理看板服务
    /// </summary>
    public class ProductionManagePanelService : IProductionManagePanelService
    {
        private readonly IPlanWorkOrderActivationRepository _planWorkOrderActivationRepository;
        private readonly IPlanWorkOrderRepository _planWorkOrderRepository;
        private readonly IProcProcessRouteRepository _procProcessRouteRepository;
        private readonly IProcMaterialRepository _procMaterialRepository;
        private readonly IInteWorkCenterRepository _inteWorkCenterRepository;
        private readonly IManuSfcSummaryRepository _manuSfcSummaryRepository;
        private readonly IProcProcessRouteDetailNodeRepository _procProcessRouteDetailNodeRepository;
        private readonly IProcProcedureRepository _procProcedureRepository;
        private readonly IEquEquipmentRepository _equipmentRepository;
        private readonly IEquStatusRepository _equStatusRepository;
        private readonly IPlanWorkOrderConversionRepository _planWorkOrderConversionRepository;
        private readonly IEquEquipmentTheoryRepository _equEquipmentTheoryRepository;
        private readonly IManuSfcStepNgRepository _manuSfcStepNgRepository;
        private readonly IQualUnqualifiedCodeRepository _qualUnqualifiedCodeRepository;
        private readonly IProcResourceRepository _procResourceRepository;
        private readonly IProcResourceEquipmentBindRepository _procResourceEquipmentBindRepository;
        public ProductionManagePanelService(IPlanWorkOrderActivationRepository planWorkOrderActivationRepository,
            IPlanWorkOrderRepository planWorkOrderRepository, IProcProcessRouteRepository procProcessRouteRepository,
            IProcMaterialRepository procMaterialRepository, IInteWorkCenterRepository inteWorkCenterRepository,
            IManuSfcSummaryRepository manuSfcSummaryRepository,
            IProcProcessRouteDetailNodeRepository procProcessRouteDetailNodeRepository,
            IProcProcedureRepository procProcedureRepository,
            IEquEquipmentRepository equipmentRepository,
            IEquStatusRepository equStatusRepository,
            IPlanWorkOrderConversionRepository planWorkOrderConversionRepository,
            IEquEquipmentTheoryRepository equEquipmentTheoryRepository,
            IManuSfcStepNgRepository manuSfcStepNgRepository,
            IQualUnqualifiedCodeRepository qualUnqualifiedCodeRepository,
            IProcResourceRepository procResourceRepository,
            IProcResourceEquipmentBindRepository procResourceEquipmentBindRepository)
        {
            _planWorkOrderActivationRepository = planWorkOrderActivationRepository;
            _planWorkOrderRepository = planWorkOrderRepository;
            _procProcessRouteRepository = procProcessRouteRepository;
            _procMaterialRepository = procMaterialRepository;
            _inteWorkCenterRepository = inteWorkCenterRepository;
            _manuSfcSummaryRepository = manuSfcSummaryRepository;
            _procProcessRouteDetailNodeRepository = procProcessRouteDetailNodeRepository;
            _procProcedureRepository = procProcedureRepository;
            _equipmentRepository = equipmentRepository;
            _equStatusRepository = equStatusRepository;
            _planWorkOrderConversionRepository = planWorkOrderConversionRepository;
            _equEquipmentTheoryRepository = equEquipmentTheoryRepository;
            _manuSfcStepNgRepository = manuSfcStepNgRepository;
            _qualUnqualifiedCodeRepository = qualUnqualifiedCodeRepository;
            _procResourceRepository = procResourceRepository;
            _procResourceEquipmentBindRepository = procResourceEquipmentBindRepository;
        }

        /// <summary>
        /// 获取SiteId下的激活的第一个工单
        /// </summary>
        /// <param name="siteId"></param>
        /// <returns></returns>
        private async Task<PlanWorkOrderEntity?> GetPlanWorkOrderFirstActivation(long siteId)
        {
            //查询激活工单
            var workOrderActivationQuery = new PlanWorkOrderActivationQuery { SiteId = siteId };
            var planWorkOrderActivations = await _planWorkOrderActivationRepository.GetPlanWorkOrderActivationEntitiesAsync(workOrderActivationQuery);
            var planWorkOrderActivationEntity = planWorkOrderActivations.OrderByDescending(c => c.Id)?.FirstOrDefault();
            if (planWorkOrderActivationEntity == null)
            {
                return null;
            }
            return await _planWorkOrderRepository.GetByIdAsync(planWorkOrderActivationEntity.WorkOrderId);
        }

        #region 班次计算相关时间

        /// <summary>
        /// 开始小时
        /// </summary>
        private int StartHour = 08;
        /// <summary>
        /// 开始分钟
        /// </summary>
        private int StartMinute = 30;
        /// <summary>
        /// 结束小时
        /// </summary>
        private int EndHour = 20;
        /// <summary>
        /// 结束分钟
        /// </summary>
        private int EndMinute = 30;

        /// <summary>
        /// 当前时间
        /// </summary>
        private DateTime NowTime
        {
            get
            {
                return HymsonClock.Now();
            }
        }
        /// <summary>
        /// 当天开始时间
        /// 白班开始时间
        /// </summary>
        private DateTime DayShiftStartTime
        {
            get
            {
                DateTime checkTime = NowTime; // 当前时间
                DateTime startTime = new DateTime(checkTime.Year, checkTime.Month, checkTime.Day, StartHour, StartMinute, 0); // 开始时间
                return startTime;
            }
        }

        /// <summary>
        /// 当天白班结束时间
        /// </summary>
        private DateTime DayShiftEndTime
        {
            get
            {
                DateTime checkTime = NowTime; // 当前时间
                DateTime endTime = new DateTime(checkTime.Year, checkTime.Month, checkTime.Day, EndHour, EndMinute, 0); // 结束时间
                return endTime;
            }
        }

        /// <summary>
        /// 当前时间是否是在白班范围
        /// </summary>
        /// <returns></returns>
        private bool IsDayShift()
        {
            DateTime checkTime = NowTime; // 当前时间
            DateTime startTime = DayShiftStartTime; // 开始时间
            DateTime endTime = DayShiftEndTime; // 结束时间
            // 检查时间是否在区间内
            if (checkTime >= startTime && checkTime <= endTime)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 当前时间班次开始时间
        /// </summary>
        /// <returns></returns>
        private DateTime StartTime
        {
            get
            {
                if (!IsDayShift() && NowTime.Hour < 9)//如果为夜班 20:30-第二天08:30
                {
                    return DayShiftEndTime.AddDays(-1);
                }
                else if (!IsDayShift() && NowTime.Hour > 9)
                {
                    return DayShiftEndTime;
                }
                else
                {

                    return DayShiftStartTime;
                }
            }
        }
        /// <summary>
        /// 当前时间班次结束时间
        /// </summary>
        /// <returns></returns>
        private DateTime EndTime
        {
            get
            {
                if (IsDayShift())
                {
                    return DayShiftEndTime;
                }
                else if (!IsDayShift() && NowTime.Hour > 9)
                {
                    return DayShiftStartTime.AddDays(1);
                }
                else
                {
                    return DayShiftStartTime;
                }
            }
        }

        /// <summary>
        /// 当天开始时间
        /// </summary>
        private DateTime DayStartTime
        {
            get
            {
                if (!IsDayShift() && NowTime.Hour < 9)
                {
                    return DayShiftStartTime.AddDays(-1);
                }
                else
                {
                    return DayShiftStartTime;
                }
            }
        }
        /// <summary>
        /// 当天结束时间
        /// </summary>
        private DateTime DayEndTime
        {
            get
            {
                if (!IsDayShift() && NowTime.Hour < 9)
                {
                    return DayShiftStartTime;
                }
                else
                {
                    return DayShiftStartTime.AddDays(1);
                }
            }
        }

        /// <summary>
        /// 当月开始时间
        /// 如2023-07-01 00:00:00
        /// </summary>
        private DateTime MonthStartTime
        {
            get
            {
                DateTime checkTime = NowTime; // 当前时间
                DateTime startTime = new DateTime(checkTime.Year, checkTime.Month, 01, 00, 00, 00);
                return startTime;
            }
        }

        /// <summary>
        /// 当月结束时间
        /// 如小于2023-08-01 00:00:00
        /// </summary>
        private DateTime MonthEndTime
        {
            get
            {
                DateTime checkTime = NowTime.AddMonths(1); //结束时间在次月
                DateTime startTime = new DateTime(checkTime.Year, checkTime.Month, 01, 00, 00, 00);
                return startTime;
            }
        }
        #endregion

        #region 获取综合信息
        /// <summary>
        /// 获取综合信息
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="procedureCode"></param>
        /// <returns></returns>
        public async Task<ProductionManagePanelReportDto?> GetOverallInfoAsync(long siteId, string procedureCode)
        {
            //获取最新的一个激活工单
            var planWorkOrderEntity = await GetPlanWorkOrderFirstActivation(siteId);
            var planWorkOrderConversionEntity = await _planWorkOrderConversionRepository.GetOneAsync(new() { PlanWorkOrderId = planWorkOrderEntity.Id });

            var moduleConversion = planWorkOrderConversionEntity?.ModuleConversion ?? 13;
            var packConversion = planWorkOrderConversionEntity?.PackConversion ?? 4;

            if (planWorkOrderEntity == null) { return null; }
            //查询工作中心
            var inteWorkCenterEntity = await _inteWorkCenterRepository.GetByIdAsync(planWorkOrderEntity.WorkCenterId ?? -1);
            //查询工艺路线
            var processRouteEntity = await _procProcessRouteRepository.GetByIdAsync(planWorkOrderEntity.ProcessRouteId);
            //查询产品信息
            var procMaterialEntity = await _procMaterialRepository.GetByIdAsync(planWorkOrderEntity.ProductId);
            //查询工单记录
            var workOrderRecord = await _planWorkOrderRepository.GetByWorkOrderIdAsync(planWorkOrderEntity.Id);
            if (workOrderRecord == null) { return null; }

            //取下线数据做为最终产出数
            var getProcedureEntity = await _procProcedureRepository.GetByCodeAsync(procedureCode, siteId);

            //查询工单维度生产汇总信息
            var manuSfcSummaryEntities = await _manuSfcSummaryRepository.GetManuSfcSummaryEntitiesAsync(new ManuSfcSummaryQuery
            {
                WorkOrderId = planWorkOrderEntity.Id,
                SiteId = siteId,
                QualityStatus = 0,//不合格数,
                ProcedureIds = new long[] { getProcedureEntity.Id }
            });
            //获取首工序
            var processRouteDetailNodeEntity = await _procProcessRouteDetailNodeRepository.GetFirstProcedureByProcessRouteIdAsync(planWorkOrderEntity.ProcessRouteId);
            var firstProcedureSummaryEntities = await _manuSfcSummaryRepository.GetManuSfcSummaryEntitiesAsync(new ManuSfcSummaryQuery
            {
                SiteId = siteId,
                ProcedureIds = new long[] { processRouteDetailNodeEntity.ProcedureId },
                StartTime = DayStartTime,
                EndTime = DayEndTime,
            });
            //完成数量
            decimal completedQty = 0;

            ////查询工单完工数量
            //var processCompletedDataQuery = new ProcessCompletedDataQueryDto { SiteId = siteId, WorkOrderId = planWorkOrderEntity.Id, ProcedureCode = procedureCode };
            //var processCompletedData = await GetProcessCompletedDataAsync(processCompletedDataQuery);
            //if (processCompletedData != null)
            //{
            //    completedQty = processCompletedData.CompletedQty;
            //}

            //TODO 完工数量统计Pack段 模组入箱的数量（暂时这样处理）
            completedQty = await _manuSfcSummaryRepository.GetSumQtyAsync(new()
            {
                QualityStatus = 1,
                ProcedureIds = new long[] { getProcedureEntity.Id },
                SiteId = siteId
            });

            //当天投入量(当前班次)
            //decimal dayConsume = firstProcedureSummaryEntities.Count();
            decimal dayConsume = Math.Round(firstProcedureSummaryEntities.Count().ParseToDecimal(), 0);

            //投入数
            //工单上显示电芯投入数
            decimal inputQty = await _manuSfcSummaryRepository.GetSumQtyAsync(new()
            {
                QualityStatus = 1,
                ProcedureIds = new long[] { processRouteDetailNodeEntity.ProcedureId },
                SiteId = siteId,
                WorkOrderId = planWorkOrderEntity?.Id
            });
            inputQty = Math.Round(inputQty, 0);

            //完成率
            decimal completedRate = decimal.Parse((completedQty / (inputQty == 0 ? 1 : (inputQty / moduleConversion / packConversion)) * 100).ToString("#0.00"));
            //计划数量
            decimal planQuantity = planWorkOrderEntity.Qty * (1 + planWorkOrderEntity.OverScale / 100);
            //计划达成率
            decimal planAchievingRate = decimal.Parse((completedQty / (planQuantity == 0 ? 1 : planQuantity) * 100).ToString("#0.00"));
            //Ng数量
            decimal summaryNoPassQty = manuSfcSummaryEntities.Count();
            //录入报废数量
            decimal unqualifiedQuantity = workOrderRecord?.UnqualifiedQuantity ?? 0;
            //不良数量
            decimal unqualifiedQty = summaryNoPassQty + unqualifiedQuantity;
            //不良率
            decimal unqualifiedRate = Math.Round(unqualifiedQty / (completedQty == 0 ? 1 : completedQty) * 100, 2);

            var managePanelReportDto = new ProductionManagePanelReportDto()
            {
                CompletedQty = completedQty,
                CompletedRate = completedRate,
                DayConsume = dayConsume,
                DayShift = IsDayShift() ? 1 : 0,
                InputQty = inputQty,
                OverallPlanAchievingRate = planAchievingRate < 0 ? 0 : (planAchievingRate > 100 ? 100 : planAchievingRate),
                OverallYieldRate = (100 - (unqualifiedRate > 100 ? 100 : unqualifiedRate)) == 0 ? 100 : (100 - (unqualifiedRate > 100 ? 100 : unqualifiedRate)),//不良率太低
                ProcessRouteCode = processRouteEntity?.Code,
                ProcessRouteName = processRouteEntity?.Name,
                ProductCode = procMaterialEntity?.MaterialCode,
                ProductName = procMaterialEntity?.MaterialName,
                UnqualifiedQty = unqualifiedQty,
                UnqualifiedRate = unqualifiedRate > 100 ? 100 : unqualifiedRate,
                WorkLineName = inteWorkCenterEntity?.Name,
                WorkOrderCode = planWorkOrderEntity?.OrderCode,
                WorkOrderDownTime = planWorkOrderEntity?.CreatedOn.ToString("yyyy-MM-dd HH:mm:ss") ?? string.Empty,
                WorkOrderQty = planWorkOrderEntity?.Qty
            };
            return managePanelReportDto;
        }

        /// <summary>
        /// 根据工单和工序查询完成数量
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        private async Task<ProcessCompletedDataDto> GetProcessCompletedDataAsync(ProcessCompletedDataQueryDto param)
        {
            ProcessCompletedDataDto processCompletedDataDto = new ProcessCompletedDataDto();
            var procProcedureEntitie = await _procProcedureRepository.GetByCodeAsync(param.ProcedureCode, param.SiteId);
            if (procProcedureEntitie == null)
            {
                return processCompletedDataDto;
            }
            //查询汇总数据 只包含良品
            var manuSfcSummaryQuery = new ManuSfcSummaryQuery
            {
                SiteId = param.SiteId,
                ProcedureIds = new long[] { procProcedureEntitie?.Id ?? 0 },
                WorkOrderId = param.WorkOrderId,
                QualityStatus = 1,
            };
            var manuSfcSummaryEntities = await _manuSfcSummaryRepository.GetManuSfcSummaryEntitiesAsync(manuSfcSummaryQuery);
            if (!manuSfcSummaryEntities.Any())
            {
                return processCompletedDataDto;
            }
            processCompletedDataDto = manuSfcSummaryEntities
                .Where(s => s.IsDeleted == 0)
                .GroupBy(s => new { s.WorkOrderId })
                .Select(g => new
                {
                    WorkOrderId = g.Max(c => c.WorkOrderId),
                    ProcedureId = g.Max(c => c.ProcedureId),
                    CompletedQty = g.Sum(s => s.Qty) ?? 0,//总数
                })
                .Select(x =>
                {
                    return new ProcessCompletedDataDto
                    {
                        WorkOrderId = x.WorkOrderId,
                        CompletedQty = x.CompletedQty
                    };
                }).First();
            return processCompletedDataDto;
        }
        #endregion

        #region 获取模组达成信息
        /// <summary>
        /// 获取模组达成信息
        /// 特定工序投入量 和 目标数（固定数值）计算得到达成率
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProductionManagePanelModuleDto>> GetModuleAchievingInfoAsync(ModuleAchievingQueryDto param)
        {
            var procProcedureEntity = await _procProcedureRepository.GetByCodeAsync(param.ProcedureCode, param.SiteId);
            if (procProcedureEntity == null)
            {
                return new List<ProductionManagePanelModuleDto>();
            }
            DateTime startTime = StartTime;//开始时间
            DateTime endTime = EndTime;// 结束时间
            //查询当前时间的投入量
            var manuSfcSummaryQuery = new ManuSfcSummaryQuery
            {
                SiteId = param.SiteId,
                ProcedureIds = new long[] { procProcedureEntity.Id },
                StartTime = startTime,
                EndTime = endTime,
                QualityStatus = 1//良品数
            };
            var manuSfcSummaryEntities = await _manuSfcSummaryRepository.GetManuSfcSummaryEntitiesAsync(manuSfcSummaryQuery);

            //调整目标数取值逻辑，获取最新的一个激活工单
            var planWorkOrderEntity = await GetPlanWorkOrderFirstActivation(param.SiteId);
            var planWorkOrderConversionEntity = await _planWorkOrderConversionRepository.GetOneAsync(new() { PlanWorkOrderId = planWorkOrderEntity.Id });

            var moduleConversion = planWorkOrderConversionEntity?.ModuleConversion ?? 13;
            var packConversion = planWorkOrderConversionEntity?.PackConversion ?? 4;

            //获取工序关联设备，在获取对应理论产出数
            var resource = (await _procResourceRepository.GetProcResourceListByProcedureIdAsync(procProcedureEntity.Id)).FirstOrDefault();
            var equipmentBind = (await _procResourceEquipmentBindRepository.GetByResourceIdsAsync(new long[] { resource?.Id ?? 0 })).FirstOrDefault();
            var equipment = await _equipmentRepository.GetByIdAsync(equipmentBind?.EquipmentId ?? 0);
            var equipmentTheory = await _equEquipmentTheoryRepository.GetOneAsync(new() { EquipmentCode = equipment.EquipmentCode });

            //每俩小时理论产出数
            param.TargetTotal = (equipmentTheory?.TheoryOutputQty ?? param.TargetTotal) * 2;

            //按照2小时为间隔进行分段统计
            TimeSpan interval = new(2, 0, 0);//分段间隔
            //动态生成分段列表
            List<Tuple<DateTime, DateTime>> segments = new List<Tuple<DateTime, DateTime>>();
            DateTime segmentStart = startTime;
            while (segmentStart < endTime)
            {
                DateTime segmentEnd = segmentStart.Add(interval);
                segments.Add(new Tuple<DateTime, DateTime>(segmentStart, segmentEnd));
                segmentStart = segmentEnd;
            }
            //按时间段取投入数平均值
            var dateTimeRangeTarger = decimal.Parse((segments.Count / param.TargetTotal <= 0 ? 1 : param.TargetTotal).ToString("0.00"));
            //统计每个分段的数据数量
            var statistics = segments.Select((segment, index) =>
            {
                //目标数（工序对应设备理论产出数（h）*2）
                var targetQty = param.TargetTotal;
                //投入数
                int count = manuSfcSummaryEntities.Count(c => c.CreatedOn >= segment.Item1 && c.CreatedOn < segment.Item2);
                //达成率
                var achievingRate = decimal.Parse((count / (targetQty == 0 ? 1 : targetQty) * 100).ToString("0.00"));
                return new ProductionManagePanelModuleDto
                {
                    DateTimeRange = $"{segment.Item1:HH:mm}-{segment.Item2:HH:mm}",
                    InputQty = count,
                    TargetQty = targetQty,
                    AchievingRate = achievingRate,
                    Sort = index + 1
                };
            }).ToList();
            return statistics;
        }

        /// <summary>
        /// Table转Dynamic
        /// </summary>
        /// <param name="table"></param>
        /// <param name="filterFields"></param>
        /// <param name="includeOrExclude"></param>
        /// <returns></returns>
        private List<dynamic> ToDynamicList(DataTable table, string[] filterFields = null, bool includeOrExclude = true)
        {
            var modelList = new List<dynamic>();
            var isFilter = filterFields != null && filterFields.Any();
            IEnumerable reservedColumns = table.Columns;
            if (isFilter)
                reservedColumns = table.Columns.Cast<DataColumn>().Where(c => filterFields?.Contains(c.ColumnName) == includeOrExclude);
            foreach (DataRow row in table.Rows)
            {
                dynamic model = new ExpandoObject();
                var dict = (IDictionary<string, object>)model;
                foreach (DataColumn column in reservedColumns)
                {
                    dict[column.ColumnName] = row[column];
                }
                modelList.Add(model);
            }
            return modelList;
        }

        /// <summary>
        /// 动态返回dynamic
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<List<dynamic>> GetModuleInfoDynamicAsync(ModuleAchievingQueryDto param)
        {
            Dictionary<string, string> keyValues = new Dictionary<string, string>();
            var managePanelModuleDtos = await GetModuleAchievingInfoAsync(param); ;
            var managePanelPackDtos = managePanelModuleDtos.GroupBy(c => c.DateTimeRange);
            DataTable dt = new();
            //空列头
            string NullColNmae = " ";
            dt.Columns.Add(NullColNmae, typeof(string));
            foreach (var item in managePanelPackDtos)
            {
                dt.Columns.Add(item.Key, typeof(string));
            }

            DataRow inputRow = dt.NewRow();
            DataRow targetRow = dt.NewRow();
            DataRow achievingRow = dt.NewRow();
            inputRow[NullColNmae] = "投入数";
            targetRow[NullColNmae] = "目标数";
            achievingRow[NullColNmae] = "达成率";
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                var col = dt.Columns[i];
                if (col.ColumnName != NullColNmae)
                {
                    var managePanelPackDto = managePanelModuleDtos.Where(c => c.DateTimeRange == col.ColumnName).FirstOrDefault();
                    if (managePanelPackDto == null) { continue; }
                    inputRow[col.ColumnName] = managePanelPackDto?.InputQty;
                    targetRow[col.ColumnName] = managePanelPackDto?.TargetQty;
                    achievingRow[col.ColumnName] = managePanelPackDto?.AchievingRate;

                }
            }
            dt.Rows.Add(inputRow);
            dt.Rows.Add(targetRow);
            dt.Rows.Add(achievingRow);
            return ToDynamicList(dt);
        }
        #endregion

        #region 获取当天工序直通率和良品率
        /// <summary>
        /// 获取当天工序直通率和良品率
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcessQualityRateDto>> GetProcessQualityRateAsync(ProcessQualityRateQueryDto param)
        {
            List<ProcessQualityRateDto> processQualityRateDtos = new List<ProcessQualityRateDto>();
            var procProcedureEntities = await _procProcedureRepository.GetByCodesAsync(param.ProcedureCodes, param.SiteId);
            if (!procProcedureEntities.Any())
            {
                return processQualityRateDtos;
            }
            var procProcedureIds = procProcedureEntities.Select(c => c.Id).ToArray();
            //查询当天汇总数据 包含良品和不良
            var manuSfcSummaryQuery = new ManuSfcSummaryQuery
            {
                SiteId = param.SiteId,
                ProcedureIds = procProcedureIds,
                StartTime = DayStartTime,
                EndTime = DayEndTime
            };
            var manuSfcSummaryEntities = await _manuSfcSummaryRepository.GetManuSfcSummaryEntitiesAsync(manuSfcSummaryQuery);
            foreach (var item in procProcedureEntities)
            {
                var manuSfcSummaries = manuSfcSummaryEntities.Where(c => c.ProcedureId == item.Id);
                //总投入数
                var inputQty = manuSfcSummaries.Count();
                //直通数
                var firstPassQty = manuSfcSummaries.Where(c => c.FirstQualityStatus == 1).Count();
                //直通率
                var firstPassRate = decimal.Parse((firstPassQty.ParseToDecimal() / (inputQty == 0 ? 1 : inputQty.ParseToDecimal())).ToString("0.00")) * 100;
                //工序良品总数
                var passQty = manuSfcSummaries.Where(c => c.QualityStatus == 1).Count();
                //良率
                var passRate = decimal.Parse((passQty.ParseToDecimal() / (inputQty == 0 ? 1 : inputQty.ParseToDecimal())).ToString("0.00")) * 100;
                var processQualityRateDto = new ProcessQualityRateDto
                {
                    FirstPassYieldRate = firstPassRate,
                    YieldRate = passRate,
                    ProccessCode = item.Code,
                    ProcessName = item.Name
                };
                processQualityRateDtos.Add(processQualityRateDto);
            }
            return processQualityRateDtos;
        }
        #endregion

        #region 获取工序良品数据信息
        /// <summary>
        /// 获取工序良品数据信息
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        private async Task<IEnumerable<ProcessYieldRateDto>> GetProcessYieldRateDataAsync(ProcessYieldRateQueryDto param)
        {
            List<ProcessYieldRateDto> processYieldRateDtos = new List<ProcessYieldRateDto>();
            var procProcedureEntities = await _procProcedureRepository.GetByCodesAsync(param.ProcedureCodes, param.SiteId);
            if (!procProcedureEntities.Any())
            {
                return processYieldRateDtos;
            }
            var procProcedureIds = procProcedureEntities.Select(c => c.Id).ToArray();
            //查询汇总数据 包含良品和不良
            var manuSfcSummaryQuery = new ManuSfcSummaryQuery
            {
                SiteId = param.SiteId,
                ProcedureIds = procProcedureIds,
                StartTime = MonthStartTime,
                EndTime = MonthEndTime
            };
            var manuSfcSummaryEntities = await _manuSfcSummaryRepository.GetManuSfcSummaryEntitiesAsync(manuSfcSummaryQuery);
            var dailyPassList = manuSfcSummaryEntities
                .Where(s => s.IsDeleted == 0)
                .GroupBy(s => new { s.CreatedOn.Date, s.ProcedureId })
                .Select(g => new
                {
                    ProcedureId = g.Max(c => c.ProcedureId),
                    Day = g.Key,
                    PassQty = g.Where(c => c.QualityStatus == 1).Sum(s => s.Qty) ?? 0,//良品数
                    Total = g.Sum(s => s.Qty) ?? 0,//总数
                })
                .Select(x =>
                {
                    var procProcedure = procProcedureEntities.Where(c => c.Id == x.ProcedureId).FirstOrDefault();
                    return new ProcessYieldRateDto
                    {
                        ProccessCode = procProcedure?.Code ?? string.Empty,
                        ProcessName = procProcedure?.Name ?? string.Empty,
                        Day = x.Day.Date.Day.ToString().PadLeft(2, '0'),
                        YieldQty = x.PassQty,
                        Total = x.Total,
                        //YieldRate = decimal.Parse((x.PassQty.ParseToDecimal() / (x.Total == 0 ? 1 : x.Total.ParseToDecimal())).ToString("0.00")) * 100
                        //调整为 良品数/直通数（复投次数）
                        YieldRate = decimal.Parse((x.PassQty.ParseToDecimal() / (x.Total == 0 ? 1 : x.Total.ParseToDecimal())).ToString("0.00")) * 100
                    };
                })
                .OrderBy(x => x.Day)
                .ToList();
            return dailyPassList;
        }

        /// <summary>
        /// 统计工序良率
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcessYieldRateDto>> GetProcessYieldRateAsync(ProcessYieldRateQueryDto param)
        {
            List<ProcessYieldRateDto> yieldRateDtos = new List<ProcessYieldRateDto>();
            int year = HymsonClock.Now().Year;
            int month = HymsonClock.Now().Month;
            int daysInMonth = DateTime.DaysInMonth(year, month);
            var processYieldRateDtos = await GetProcessYieldRateDataAsync(param);
            for (int i = 1; i <= daysInMonth; i++)
            {
                var dayStr = i < 10 ? i.ToString().PadLeft(2, '0') : i.ToString();
                var processYields = processYieldRateDtos.Where(c => c.Day == dayStr)
                    .Select(c => new ProcessYieldRateDto
                    {
                        Day = dayStr,
                        ProccessCode = c.ProccessCode,
                        ProcessName = c.ProcessName,
                        YieldQty = c.YieldQty,
                        YieldRate = c.YieldRate

                    });
                //添加不存在数据的月份指标
                if (!processYields.Any())
                {
                    foreach (var item in param.ProcedureCodes)
                    {
                        //查询工序信息（有缓存）
                        var procProcedure = await _procProcedureRepository.GetByCodeAsync(item, param.SiteId);
                        yieldRateDtos.Add(new ProcessYieldRateDto
                        {
                            Day = dayStr,
                            ProccessCode = item,
                            ProcessName = procProcedure?.Name ?? item,
                            Total = 0,
                            YieldQty = 0,
                            YieldRate = 0
                        });
                    }
                }
                //添加存在数据的指标
                yieldRateDtos.AddRange(processYields);
            }
            return yieldRateDtos;
        }
        #endregion

        #region 获取工序指数数据信息
        /// <summary>
        /// 获取工序指数数据信息
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        private async Task<IEnumerable<ProcessIndicatorsDto>> GetProcessIndicatorsDataAsync(ProcessIndicatorsQueryDto param)
        {
            List<ProcessIndicatorsDto> processIndicatorsDtos = new List<ProcessIndicatorsDto>();
            var procProcedureEntities = await _procProcedureRepository.GetByCodesAsync(param.ProcedureCodes, param.SiteId);
            if (!procProcedureEntities.Any())
            {
                return processIndicatorsDtos;
            }
            var procProcedureIds = procProcedureEntities.Select(c => c.Id).ToArray();
            //查询汇总数据 只包含良品
            var manuSfcSummaryQuery = new ManuSfcSummaryQuery
            {
                SiteId = param.SiteId,
                ProcedureIds = procProcedureIds,
                StartTime = MonthStartTime,
                EndTime = MonthEndTime,
                QualityStatus = 1,
            };
            var manuSfcSummaryEntities = await _manuSfcSummaryRepository.GetManuSfcSummaryEntitiesAsync(manuSfcSummaryQuery);
            var dailyPassList = manuSfcSummaryEntities
                .Where(s => s.IsDeleted == 0)
                .GroupBy(s => new { s.CreatedOn.Date, s.ProductId })
                .Select(g => new
                {
                    ProcedureId = g.Max(c => c.ProcedureId),
                    Day = g.Key,
                    Total = g.Sum(s => s.Qty) ?? 0,//总数
                })
                .Select(x =>
                {
                    var procProcedure = procProcedureEntities.Where(c => c.Id == x.ProcedureId).FirstOrDefault();
                    return new ProcessIndicatorsDto
                    {
                        ProccessCode = procProcedure?.Code ?? string.Empty,
                        ProcessName = procProcedure?.Name ?? string.Empty,
                        Day = x.Day.Date.Day.ToString().PadLeft(2, '0'),
                        Indicators = x.Total,
                    };
                })
                .OrderBy(x => x.Day).ToList();
            return dailyPassList;
        }

        /// <summary>
        /// 统计工序指数信息
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcessIndicatorsDto>> GetProcessIndicatorsAsync(ProcessIndicatorsQueryDto param)
        {
            List<ProcessIndicatorsDto> processIndicators = new List<ProcessIndicatorsDto>();
            int year = HymsonClock.Now().Year;
            int month = HymsonClock.Now().Month;
            int daysInMonth = DateTime.DaysInMonth(year, month);
            var processIndicatorsDtos = await GetProcessIndicatorsDataAsync(param);
            for (int i = 1; i <= daysInMonth; i++)
            {
                var dayStr = i < 10 ? i.ToString().PadLeft(2, '0') : i.ToString();
                var processIndicator = processIndicatorsDtos.Where(c => c.Day == dayStr)
                    .Select(c => new ProcessIndicatorsDto
                    {
                        Day = dayStr,
                        ProccessCode = c.ProccessCode,
                        ProcessName = c.ProcessName,
                        Indicators = c.Indicators
                    });
                //添加不存在数据的月份指标
                if (!processIndicator.Any())
                {
                    foreach (var item in param.ProcedureCodes)
                    {
                        //查询工序信息（有缓存）
                        var procProcedure = await _procProcedureRepository.GetByCodeAsync(item, param.SiteId);
                        processIndicators.Add(new ProcessIndicatorsDto
                        {
                            Day = dayStr,
                            ProccessCode = item,
                            ProcessName = procProcedure?.Name ?? item,
                            Indicators = 0
                        });
                    }
                }
                //添加存在数据的指标
                processIndicators.AddRange(processIndicator);
            }
            return processIndicators;
        }
        #endregion

        #region 获取设备稼动率数据
        /// <summary>
        /// 统计设备每日稼动率信息
        /// 永泰设备每日OEE计算公式
        /// 必要条件：
        ///     正常出勤时间 默认 10H/班，共两班，可外部传入更改。
        ///     计划时间 = 正常出勤时间，两个班次共计20H。
        ///     操作时间 = 计划时间 - 设备异常时长（停机时间，故障停机设备调试时间）。
        ///     理想加工周期 = 设备加工完产品需要的时间，默认100，可以外部传入更改。
        /// 计算方法：
        ///     设备OEE=可用率*表现性*质量指数 = （操作时间/计划工作时间）* （理想加工周期 * 生产数量/实际运行时间） * （良品总数/总产量）*100%
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<EquipmentUtilizationRateDto>> GetEquipmentUtilizationRateAsync(EquipmentUtilizationRateQueryDto param)
        {
            //获取设备理论产能
            var equipmentTheoryEntities = await _equEquipmentTheoryRepository.GetListAsync(new());

            List<EquipmentUtilizationRateDto> equipmentUtilizationRateDtos = new List<EquipmentUtilizationRateDto>();
            //查询需要统计的设备信息
            var equEquipmentEntities = await _equipmentRepository.GetEntitiesAsync(new EquEquipmentQuery { SiteId = param.SiteId, EquipmentCodes = param.EquipmentCodes });
            var equIds = equEquipmentEntities.Select(c => c.Id).ToArray();
            if (!equIds.Any())
            {
                return equipmentUtilizationRateDtos;
            }
            //查询设备当天异常时长汇总（除了运行以外的状态）
            var equipmentStopTimeDtos = await GetEquipmentStopTimeAsync(param.SiteId, equIds);
            //查询设备当天生产数据汇总
            var equipmentYieldDtos = await GetEquipmentYieldAsync(param.SiteId, equIds);
            //统计设备OEE
            foreach (var equCode in param.EquipmentCodes)
            {
                var equipmentTheory = equipmentTheoryEntities.FirstOrDefault(a => a.EquipmentCode == equCode);

                //var theoryCycle = param.TheoryCycle;//理想加工周期
                //var workingHours = param.WorkingHours;//正常出勤时间/计划时间(小时)
                var theoryCycle = equipmentTheory?.TheoryOutputQty ?? param.TheoryCycle;//理论每小时产出数
                var workingHours = equipmentTheory?.TheoryOnTime ?? param.WorkingHours;//理论每天开机时长
                //当前设备
                var equEquipmentEntity = equEquipmentEntities.Where(c => c.EquipmentCode == equCode).First();
                //获取当前设备异常时长
                var equipmentStopTime = equipmentStopTimeDtos.Where(c => c.EquipmentId == equEquipmentEntity.Id).FirstOrDefault();
                //获取当前设备生产数据
                var equipmentYield = equipmentYieldDtos.Where(c => c.EquipmentId == equEquipmentEntity.Id).FirstOrDefault();
                //计划工作秒
                var workingSeconds = workingHours * 3600;
                //操作时间
                var operateTime = workingSeconds - (equipmentStopTime?.StopSeconds ?? 0);

                //可用率
                var availability = operateTime / workingSeconds;
                //表现性 实际运行实际取操作时间
                //var expressive = theoryCycle * (equipmentYield?.Total ?? 0) / (operateTime == 0 ? 1 : operateTime);
                var expressive = (equipmentYield?.Total??0) / (theoryCycle * workingHours);
                //质量指数
                var qualityIndex = (equipmentYield?.Total ?? 0) / (equipmentYield?.Total ?? 1);
                //OEE
                //var utilizationRate = decimal.Parse((availability * expressive * qualityIndex).ToString("0.00"));

                //设备性能稼动率 = ((设备理论开机时长-设备停机时长) / 设备理论开机时长)
                var equStopTime = equipmentStopTime?.StopSeconds ?? 0;
                var utilizationRate = Math.Round(((workingSeconds - equStopTime) / workingSeconds) * expressive, 2);

                var equipmentUtilizationRateDto = new EquipmentUtilizationRateDto
                {
                    EquipmentCode = equCode,
                    EquipmentName = equEquipmentEntity.EquipmentName,
                    UtilizationRate = utilizationRate
                };
                equipmentUtilizationRateDtos.Add(equipmentUtilizationRateDto);
            }
            return equipmentUtilizationRateDtos;
        }

        /// <summary>
        /// 查询当天设备停机时长汇总
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="equipentIds"></param>
        /// <returns></returns>
        private async Task<IEnumerable<EquipmentStopTimeDto>> GetEquipmentStopTimeAsync(long siteId, long[] equipentIds)
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
                                StopSeconds = group.Sum(c => (c.EndTime - c.BeginTime)?.TotalSeconds ?? 0).ParseToDecimal()//停机总时长
                            });
            return equipmentStopTimeDtos;
        }

        /// <summary>
        /// 获取设备当天生产数据
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="equipentIds"></param>
        /// <returns></returns>
        private async Task<IEnumerable<EquipmentYieldDto>> GetEquipmentYieldAsync(long siteId, long[] equipentIds)
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
        #endregion

        /// <summary>
        /// 每日不良率Top3
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<ManuSummaryBadRecordTop3Dto>> getUnqualifiedTop3(ManuSummaryBadRecordTop3QueryDto query)
        {
            //获取当前时间
            DateTime startDate;
            DateTime endDate;

            if (query.Type == 0)
            {
                if (DateTime.Now < DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd 08:30:00")))
                {
                    startDate = DateTime.Parse(DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd 08:30:00"));
                    endDate = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd 08:30:00"));
                }
                else
                {
                    startDate = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd 08:30:00"));
                    endDate = DateTime.Parse(DateTime.Now.AddDays(1).ToString("yyyy-MM-dd 08:30:00"));
                }
            }
            else
            {
                //获取当前时间
                startDate = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-01 08:30:00"));
                endDate = DateTime.Parse(DateTime.Now.AddMonths(1).ToString("yyyy-MM-01 08:30:00"));
            }


            var manuSummaryEntities = await _manuSfcSummaryRepository.GetManuSfcSummaryEntitiesAsync(new() { StartTime = startDate, EndTime = endDate, SiteId = 123456 });

            var procedureIds = manuSummaryEntities.Select(a => a.ProcedureId).Distinct();
            var procedureEntities = await _procProcedureRepository.GetByIdsAsync(procedureIds.ToArray());

            var summaryBadTop3 = manuSummaryEntities.GroupBy(a => a.ProcedureId)
                .Select(a => new { a.Key, Qty = a.Sum(b => b.Qty), BadQty = a.Sum(b => b.QualityStatus) })
                .OrderBy(a => a.BadQty)
                .Take(3);

            List<ManuSummaryBadRecordTop3Dto> result = new();
            foreach (var item in summaryBadTop3)
            {
                var procedure = procedureEntities.FirstOrDefault(a => a.Id == item.Key);
                result.Add(new()
                {
                    ProcedureCode = procedure?.Code,
                    ProcedureName = procedure?.Name,
                    UnqualifiedQty = item.BadQty.GetValueOrDefault(),
                    UnqualifiedRate = Math.Round(item.BadQty.GetValueOrDefault() == 0 || item.Qty.GetValueOrDefault() == 0 ? 1 : (item.BadQty / item.Qty).GetValueOrDefault(), 2)
                });
            }

            if (result.Count < 3)
            {
                var procedures = (await _procProcedureRepository.GetAllAsync(123456)).ToList();
                procedures = procedures.Where(a => !procedureIds.Any(b => b == a.Id)).ToList();
                //如果数量不足三个，则补充数据进来
                while (true)
                {
                    var random = new Random();
                    var procedure = procedures[random.Next(procedures.Count)];
                    result.Add(new()
                    {
                        ProcedureCode = procedure?.Code,
                        ProcedureName = procedure?.Name,
                        UnqualifiedQty = 0,
                        UnqualifiedRate = 0
                    });

                    procedures.Remove(procedure);

                    if (result.Count() > 2) break;
                }
            }


            return result;
        }

        /// <summary>
        /// 每月不良代码Top3
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<ManuSummarySfcBadRecordTop3Dto>> GetBadTop3Async()
        {
            //获取当前时间
            DateTime startDate = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-01 08:30:00"));
            DateTime endDate = DateTime.Parse(DateTime.Now.AddMonths(1).ToString("yyyy-MM-01 08:30:00"));

            var manuStepNgEntities = await _manuSfcStepNgRepository.GetManuSfcStepNgEntitiesAsync(new() { StartDate = startDate, EndDate = endDate });
            var manuSfcStepTop3 = manuStepNgEntities.GroupBy(a => a.UnqualifiedCode)
                .Select(a => new { a.Key, Qty = a.Count() })
                .OrderBy(a => a.Qty)
                .Take(3);

            var unqualifiedCodes = manuSfcStepTop3.Select(a => a.Key);
            var unqualifiedEntities = await _qualUnqualifiedCodeRepository.GetByCodesAsync(new() { Codes = unqualifiedCodes.ToArray(), Site = 123456 });

            List<ManuSummarySfcBadRecordTop3Dto> result = new();
            foreach (var item in manuSfcStepTop3)
            {
                var unqualified = unqualifiedEntities.FirstOrDefault(a => a.UnqualifiedCode == item.Key);

                result.Add(new()
                {
                    UnqualifiedCode = unqualified?.UnqualifiedCode,
                    UnqualifiedName = unqualified?.UnqualifiedCodeName,
                    UnqualifiedQty = item.Qty
                });
            }

            if (result.Count < 3)
            {
                var allUnqualifiedCodes = (await _qualUnqualifiedCodeRepository.GetListAsync(new())).ToList();
                allUnqualifiedCodes = allUnqualifiedCodes.Where(a => !unqualifiedCodes.Any(b => b == a.UnqualifiedCode)).ToList();
                //如果数量不足三个，则补充数据进来
                while (true)
                {
                    var random = new Random();
                    var unCode = allUnqualifiedCodes[random.Next(allUnqualifiedCodes.Count)];
                    result.Add(new()
                    {
                        UnqualifiedCode = unCode?.UnqualifiedCode,
                        UnqualifiedName = unCode?.UnqualifiedCodeName,
                        UnqualifiedQty = 0,
                    });

                    allUnqualifiedCodes.Remove(unCode);

                    if (result.Count() > 2) break;
                }
            }

            return result;
        }
    }
}
