using Hymson.MES.Services.Dtos.Report;
using Hymson.MES.Services.Services.Report;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
            return Task.FromResult(new ProductionManagePanelReportDto()
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
            });
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

        [HttpGet]
        [Route("getPackInfoDynamicAsync")]
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
    }
}
