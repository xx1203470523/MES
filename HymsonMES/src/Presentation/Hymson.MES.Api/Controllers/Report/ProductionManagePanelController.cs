using Hymson.MES.Services.Dtos.Report;
using Hymson.MES.Services.Services.Report.ProductionManagePanel;
using Hymson.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Data;
using System.Dynamic;
using System.Security.Policy;

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
        private readonly IProductionManagePanelService _productionManagePanelService;
        public ProductionManagePanelController(IProductionManagePanelService productionManagePanelService)
        {
            _productionManagePanelService = productionManagePanelService;
        }
        /// <summary>
        /// 获取综合信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("getOverallInfo")]
        public async Task<ProductionManagePanelReportDto?> GetOverallInfoAsync(long siteId)
        {
            return await _productionManagePanelService.GetOverallInfoAsync(siteId);
        }

        /// <summary>
        /// 获取当天模组达成数据
        /// </summary>
        /// <param name="param">站点ID</param>
        /// <returns></returns>
        [HttpGet]
        [Route("getModuleAchievingInfo")]
        public async Task<IEnumerable<ProductionManagePanelModuleDto>> GetModuleAchievingInfoAsync([FromQuery] ModuleAchievingQueryDto param)
        {
            return await _productionManagePanelService.GetModuleAchievingInfoAsync(param);
        }
        /// <summary>
        /// 获取模组达成详细信息
        /// </summary>
        /// <param name="param">/param>
        /// <returns></returns>
        [HttpGet]
        [Route("getModuleInfoDynamic")]
        public async Task<List<dynamic>> GetModuleInfoDynamicAsync([FromQuery] ModuleAchievingQueryDto param)
        {
            Dictionary<string, string> keyValues = new Dictionary<string, string>();
            var managePanelModuleDtos = await _productionManagePanelService.GetModuleAchievingInfoAsync(param); ;
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
            var dynamics = ToDynamicList(dt);
            return dynamics;
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
        /// 获取工序良品率相关信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("getProcessQualityRate")]
        public async Task<IEnumerable<ProcessQualityRateDto>> GetProcessQualityRateAsync([FromQuery] ProcessQualityRateQuery param)
        {
            return await _productionManagePanelService.GetProcessQualityRateAsync(param);
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
            int year = HymsonClock.Now().Year;
            int month = HymsonClock.Now().Month;
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
            int year = HymsonClock.Now().Year;
            int month = HymsonClock.Now().Month;
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
                    ProccessCode = "TEST",
                    ProcessName = "测试工序",
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
