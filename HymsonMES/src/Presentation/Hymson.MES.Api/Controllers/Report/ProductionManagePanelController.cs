using Hymson.MES.Services.Dtos.Report;
using Hymson.MES.Services.Services.Report;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MySqlX.XDevAPI.Common;
using System;
using System.Collections;
using System.Data;
using System.Dynamic;

namespace Hymson.MES.Api.Controllers.Report
{
    /// <summary>
    /// 生产管理看板控制器
    /// </summary>
    [AllowAnonymous]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ProductionManagePanelController : ControllerBase
    {
        private readonly IProductTraceReportService _productTraceReportService;
        public ProductionManagePanelController(IProductTraceReportService productTraceReportService)
        {
            _productTraceReportService = productTraceReportService;
        }
        /// <summary>
        /// 获取综合信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("getOverallInfo")]
        public Task<ProductionManagePanelReportDto> GetOverallInfoAsync()
        {
            //模拟数据
            var managePanelReportDto = new ProductionManagePanelReportDto()
            {
                CompletedQty = 100,
                CompletedRate = 80,
                DayConsume = 120,
                DayShift = 0,
                InputQty = 120,
                OverallPlanAchievingRate = 10,
                OverallYieldRate = 90,
                ProcessRouteCode = "1111",
                ProcessRouteName = "工艺路线",
                ProductCode = "AAA",
                ProductName = "产品",
                UnqualifiedQty = 10,
                UnqualifiedRate = 10,
                WorkLineName = "ajkfaw",
                WorkOrderCode = "W292421AAFA",
                WorkOrderDownTime = new DateTime(),
                WorkOrderQty = 200
            };
            return Task.FromResult(managePanelReportDto);
        }

        /// <summary>
        /// 获取当天Pack达成数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("getPackAchievingInfo")]
        public Task<IEnumerable<ProductionManagePanelPackDto>> GetPackAchievingInfoAsync()
        {
            IEnumerable<ProductionManagePanelPackDto> result = new List<ProductionManagePanelPackDto>
            {
                new ProductionManagePanelPackDto()
                {
                    Sort=1,
                    DateTimeRange = "08:30-10:30",
                    InputQty = 40,
                    TargetQty = 100,
                    AchievingRate = 25,
                },
                new ProductionManagePanelPackDto()
                {
                    Sort=2,
                    DateTimeRange = "10:30-12:30",
                    InputQty = 20,
                    TargetQty = 50,
                    AchievingRate = 15,
                },
                new ProductionManagePanelPackDto()
                {
                    Sort=3,
                    DateTimeRange = "12:30-14:30",
                    InputQty = 90,
                    TargetQty = 90,
                    AchievingRate = 90,
                },
                new ProductionManagePanelPackDto()
                {
                    Sort=4,
                    DateTimeRange = "14:30-16:30",
                    InputQty = 10,
                    TargetQty = 100,
                    AchievingRate = 10,
                },
                new ProductionManagePanelPackDto()
                {
                    Sort=5,
                    DateTimeRange = "16:30-18:30",
                    InputQty = 0,
                    TargetQty = 100,
                    AchievingRate = 0,
                },
            };
            result = result.OrderBy(c => c.Sort).ToList();
            return Task.FromResult(result);
        }
        /// <summary>
        /// 获取Pack详细信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("getPackInfoDynamic")]
        public Task<List<dynamic>> GetPackInfoDynamicAsync()
        {
            Dictionary<string, string> keyValues = new Dictionary<string, string>();
            var packInfo = GetPackAchievingInfoAsync().Result;
            var managePanelPackDtos = packInfo.GroupBy(c => c.DateTimeRange);
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
                    var managePanelPackDto = packInfo.Where(c => c.DateTimeRange == col.ColumnName).FirstOrDefault();
                    if (managePanelPackDto == null) { continue; }
                    inputRow[col.ColumnName] = managePanelPackDto?.InputQty;
                    targetRow[col.ColumnName] = managePanelPackDto?.TargetQty;
                    achievingRow[col.ColumnName] = managePanelPackDto?.AchievingRate;

                }
            }
            dt.Rows.Add(inputRow);
            dt.Rows.Add(targetRow);
            dt.Rows.Add(achievingRow);
            var dynamics = ToDynamicList(dt);
            return Task.FromResult(dynamics);
        }
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
        /// 获取工序稼动率
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("getProcessUtilizationRate")]
        public Task<IEnumerable<ProcessUtilizationRateDto>> GetProcessUtilizationRateAsync()
        {
            IEnumerable<ProcessUtilizationRateDto> processUtilizationRateDtos = new List<ProcessUtilizationRateDto>()
            {
                new ProcessUtilizationRateDto {
                    ProccessCode="QA001",
                    ProcessName="测试工序001",
                    Utilization=80,
                    UtilizationRate=10
                },
                new ProcessUtilizationRateDto {
                    ProccessCode="QA002",
                    ProcessName="测试工序002",
                    Utilization=180,
                    UtilizationRate=60
                },
                new ProcessUtilizationRateDto {
                    ProccessCode="QA003",
                    ProcessName="测试工序003",
                    Utilization=100,
                    UtilizationRate=40
                },
                new ProcessUtilizationRateDto {
                    ProccessCode="QA004",
                    ProcessName="测试工序004",
                    Utilization=200,
                    UtilizationRate=80
                },
                new ProcessUtilizationRateDto {
                    ProccessCode="QA005",
                    ProcessName="测试工序005",
                    Utilization=240,
                    UtilizationRate=100
                },
                new ProcessUtilizationRateDto {
                    ProccessCode="QA006",
                    ProcessName="测试工序006",
                    Utilization=120,
                    UtilizationRate=50
                },
                new ProcessUtilizationRateDto {
                    ProccessCode="QA007",
                    ProcessName="测试工序007",
                    Utilization=70,
                    UtilizationRate=30
                },
                new ProcessUtilizationRateDto {
                    ProccessCode="QA008",
                    ProcessName="测试工序008",
                    Utilization=220,
                    UtilizationRate=90
                },
                new ProcessUtilizationRateDto {
                    ProccessCode="QA009",
                    ProcessName="测试工序009",
                    Utilization=210,
                    UtilizationRate=85
                },
                new ProcessUtilizationRateDto {
                    ProccessCode="QA010",
                    ProcessName="测试工序010",
                    Utilization=80,
                    UtilizationRate=10
                },
                new ProcessUtilizationRateDto {
                    ProccessCode="QA011",
                    ProcessName="测试工序011",
                    Utilization=180,
                    UtilizationRate=80
                },
            };
            return Task.FromResult(processUtilizationRateDtos);
        }

        /// <summary>
        /// 获取设备性能稼动率
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("getEquipmentUtilizationRate")]
        public Task<IEnumerable<EquipmentUtilizationRateDto>> GetEquipmentUtilizationRateAsync()
        {
            IEnumerable<EquipmentUtilizationRateDto> equipmentUtilizationRateDtos = new List<EquipmentUtilizationRateDto>()
            {
                new EquipmentUtilizationRateDto {
                    EquipmentCode="AN001",
                    EquipmentName="电芯分档设备",
                    UtilizationRate=10
                },
                new EquipmentUtilizationRateDto {
                    EquipmentCode="AN002",
                    EquipmentName="电芯堆叠设备",
                    UtilizationRate=60
                },
                new EquipmentUtilizationRateDto {
                    EquipmentCode="AN003",
                    EquipmentName="烤炉",
                    UtilizationRate=40
                },
                new EquipmentUtilizationRateDto {
                    EquipmentCode="AN004",
                    EquipmentName="机型绝缘检测",
                    UtilizationRate=80
                },
                new EquipmentUtilizationRateDto {
                    EquipmentCode="AN005",
                    EquipmentName="极柱清洗",
                    UtilizationRate=50
                },
                new EquipmentUtilizationRateDto {
                    EquipmentCode="AN006",
                    EquipmentName="焊接",
                    UtilizationRate=30
                },
                new EquipmentUtilizationRateDto {
                    EquipmentCode="AN007",
                    EquipmentName="DCIR测试",
                    UtilizationRate=70
                },
                new EquipmentUtilizationRateDto {
                    EquipmentCode="AN008",
                    EquipmentName="捆扎",
                    UtilizationRate=90
                }
            };
            return Task.FromResult(equipmentUtilizationRateDtos);
        }

        /// <summary>
        /// 获取工序良品率波动
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("getProcessYieldRate")]
        public Task<List<ProcessYieldRateDto>> GetProcessYieldRateAsync()
        {
            List<ProcessYieldRateDto> yieldRateDtos = new List<ProcessYieldRateDto>();
            int year = DateTime.Now.Year;
            int month = DateTime.Now.Month;
            int daysInMonth = DateTime.DaysInMonth(year, month);
            for (int i = 1; i <= daysInMonth; i++)
            {
                var dayStr = i < 10 ? i.ToString().PadLeft(2, '0') : i.ToString();
                Random random = new Random(i);
                int minValue = 1;
                int maxValue = 100;
                int randomInRange = random.Next(minValue, maxValue + 1);

                var processYieldRate = new ProcessYieldRateDto
                {
                    Day = dayStr,
                    ProccessCode = "TEST1",
                    ProcessName = "测试工序1",
                    YieldQty = randomInRange,
                    YieldRate = randomInRange
                };
                yieldRateDtos.Add(processYieldRate);

                int randomInRange2 = random.Next(minValue, maxValue + 1);
                var processYieldRate2 = new ProcessYieldRateDto
                {
                    Day = dayStr,
                    ProccessCode = "TEST2",
                    ProcessName = "测试工序2",
                    YieldQty = randomInRange2,
                    YieldRate = randomInRange2
                };
                yieldRateDtos.Add(processYieldRate2);

                int randomInRange3 = random.Next(minValue, maxValue + 1);
                var processYieldRate3 = new ProcessYieldRateDto
                {
                    Day = dayStr,
                    ProccessCode = "TEST3",
                    ProcessName = "测试工序3",
                    YieldQty = randomInRange3,
                    YieldRate = randomInRange3
                };
                yieldRateDtos.Add(processYieldRate3);

                int randomInRange4 = random.Next(minValue, maxValue + 1);
                var processYieldRate4 = new ProcessYieldRateDto
                {
                    Day = dayStr,
                    ProccessCode = "TEST4",
                    ProcessName = "测试工序4",
                    YieldQty = randomInRange4,
                    YieldRate = randomInRange4
                };
                yieldRateDtos.Add(processYieldRate4);
            }
            return Task.FromResult(yieldRateDtos);
        }

        /// <summary>
        /// 获取工序指数
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("getProcessIndicators")]
        public Task<List<ProcessIndicatorsDto>> GetProcessIndicatorsAsync()
        {
            List<ProcessIndicatorsDto> indicatorsDtos = new List<ProcessIndicatorsDto>();
            int year = DateTime.Now.Year;
            int month = DateTime.Now.Month;
            int daysInMonth = DateTime.DaysInMonth(year, month);
            for (int i = 1; i <= daysInMonth; i++)
            {
                var dayStr = i < 10 ? i.ToString().PadLeft(2, '0') : i.ToString();
                Random random = new Random(i);
                int minValue = 1;
                int maxValue = 100;
                int randomInRange = random.Next(minValue, maxValue + 1);
                var processIndicators = new ProcessIndicatorsDto
                {
                    Day = dayStr,
                    ProccessCode = "TEST" + dayStr,
                    ProcessName = "测试工序" + i.ToString(),
                    Indicators = randomInRange
                };
                indicatorsDtos.Add(processIndicators);

                int randomInRange2 = random.Next(minValue, maxValue + 1);
                var processIndicators2 = new ProcessIndicatorsDto
                {
                    Day = dayStr,
                    ProccessCode = "TEST2",
                    ProcessName = "测试工序2",
                    Indicators = randomInRange2
                };
                indicatorsDtos.Add(processIndicators2);

                int randomInRange3 = random.Next(minValue, maxValue + 1);
                var processIndicators3 = new ProcessIndicatorsDto
                {
                    Day = dayStr,
                    ProccessCode = "TEST3",
                    ProcessName = "测试工序3",
                    Indicators = randomInRange3
                };
                indicatorsDtos.Add(processIndicators3);

                int randomInRange4 = random.Next(minValue, maxValue + 1);
                var processIndicators4 = new ProcessIndicatorsDto
                {
                    Day = dayStr,
                    ProccessCode = "TEST4",
                    ProcessName = "测试工序4",
                    Indicators = randomInRange4
                };
                indicatorsDtos.Add(processIndicators4);
            }
            return Task.FromResult(indicatorsDtos);
        }
    }
}
