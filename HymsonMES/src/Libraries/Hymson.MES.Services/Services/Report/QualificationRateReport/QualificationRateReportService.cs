using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Data.Repositories.Plan;
using Hymson.MES.Data.Repositories.QualificationRateReport;
using Hymson.MES.Data.Repositories.QualificationRateReport.Query;
using Hymson.MES.Services.Dtos.QualificationRateReport;
using Hymson.MES.Core.Constants;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Core.Domain.Plan;
using Hymson.MES.Data.Repositories.Plan.PlanWorkOrder.Query;
using System.Security.Policy;
using Hymson.MES.CoreServices.Dtos.Common;
using Hymson.MES.Services.Dtos.Common;
using Hymson.MES.Services.Dtos.Report.Excel;
using Hymson.MES.Services.Dtos.Report;
using Hymson.Excel.Abstractions;
using Hymson.Minio;
using Hymson.MES.Services.Dtos.Equipment;

namespace Hymson.MES.Services.Services.QualificationRateReport
{
    /// <summary>
    /// 服务（合格率报表） 
    /// </summary>
    public class QualificationRateReportService : IQualificationRateReportService
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
        /// 仓储接口（合格率报表）
        /// </summary>
        private readonly IQualificationRateReportRepository _qualificationRateReportRepository;

        /// <summary>
        /// 工单仓储
        /// </summary>
        private readonly IPlanWorkOrderRepository _planWorkOrderRepository;

        /// <summary>
        /// 产品仓储
        /// </summary>
        private readonly IProcMaterialRepository _procMaterialRepository;

        /// <summary>
        /// 工序仓储
        /// </summary>
        private readonly IProcProcedureRepository _procProcedureRepository;

        private readonly IMinioService _minioService;
        private readonly IExcelService _excelService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="currentSite"></param>
        /// <param name="qualificationRateReportRepository"></param>
        /// <param name="planWorkOrderRepository"></param>
        /// <param name="procMaterialRepository"></param>
        /// <param name="procProcedureRepository"></param>
        /// <param name="minioService"></param>
        /// <param name="excelService"></param>
        public QualificationRateReportService(ICurrentUser currentUser, ICurrentSite currentSite,
            IQualificationRateReportRepository qualificationRateReportRepository,
            IPlanWorkOrderRepository planWorkOrderRepository,
            IProcMaterialRepository procMaterialRepository,
            IProcProcedureRepository procProcedureRepository,
            IMinioService minioService,
            IExcelService excelService)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _qualificationRateReportRepository = qualificationRateReportRepository;
            _planWorkOrderRepository = planWorkOrderRepository;
            _procMaterialRepository = procMaterialRepository;
            _procProcedureRepository = procProcedureRepository;
            _minioService = minioService;
            _excelService = excelService;
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<QualificationRateReportDto>> GetPagedListAsync(QualificationRateReportPagedQueryDto pagedQueryDto)
        {
            var pagedQuery = pagedQueryDto.ToQuery<QualificationRateReportPagedQuery>();
            pagedQuery.SiteId = _currentSite.SiteId ?? 0;
            List<QualificationRateReportDto> listDto = new List<QualificationRateReportDto>();
            //工单
            IEnumerable<PlanWorkOrderEntity>? workOrderInfos = new List<PlanWorkOrderEntity>();

            workOrderInfos = await _planWorkOrderRepository.GetsByCodeAsync(new PlanWorkOrderQuery() { OrderCode = pagedQueryDto.OrderCode ?? "", SiteId = pagedQuery.SiteId });
            //工单为空 直接返回
            if (workOrderInfos == null || !workOrderInfos.Any()) return new PagedInfo<QualificationRateReportDto>(listDto, pagedQueryDto.PageIndex, pagedQueryDto.PageSize);
            pagedQuery.WorkOrderIds = workOrderInfos.Select(x => x.Id).ToArray();

            var pagedInfo = await _qualificationRateReportRepository.GetPagedInfoAsync(pagedQuery);

            //工序
            var procedureIds = pagedInfo.Data.Select(x => x.ProcedureId).Distinct().ToArray();
            var procedureInfos = await _procProcedureRepository.GetByIdsAsync(procedureIds);
            //产品
            var productIds = pagedInfo.Data.Select(x => x.ProductId).Distinct().ToArray();
            var productInfos = await _procMaterialRepository.GetByIdsAsync(productIds);

            foreach (var item in pagedInfo.Data)
            {
                var procedureInfo = procedureInfos.FirstOrDefault(x => x.Id == item.ProcedureId);
                var workOrderInfo = workOrderInfos.FirstOrDefault(x => x.Id == item.WorkOrderId);
                var productInfo = productInfos.FirstOrDefault(x => x.Id == item.ProductId);

                string startOn = "";
                string endOn = "";
                if (pagedQueryDto.Type == 1)//月
                {
                    var date = Convert.ToDateTime(item.StartOn.ToString());

                    startOn = date.Year + "-" + date.Month + "-" + date.Day;
                    endOn = date.Year + "-" + date.Month + "-" + date.AddDays(1).Day;
                }
                else
                if (pagedQueryDto.Type == 2)//年
                {
                    startOn = item.StartYear + "-" + item.StartMonth;
                    if (item.StartMonth.HasValue && item.StartMonth == 12)
                    {
                        endOn = (item.StartYear + 1) + "-1";
                    }
                    else
                    {
                        endOn = item.StartYear + "-" + (item.StartMonth + 1);
                    }
                }
                else//日
                {
                    var date = Convert.ToDateTime(item.StartOn.ToString());

                    startOn = date.Year + "-" + date.Month + "-" + date.Day + "  " + item.StartHour;
                    endOn = date.Year + "-" + date.Month + "-" + date.Day + "  " + (item.StartHour + 1);
                }
                listDto.Add(new QualificationRateReportDto
                {
                    OrderCode = workOrderInfo != null ? workOrderInfo.OrderCode : "",
                    MaterialName = productInfo != null ? productInfo.MaterialName : "",
                    ProcedureName = procedureInfo != null ? procedureInfo.Name : "",
                    StartOn = startOn,
                    EndOn = endOn,
                    QualifiedQuantity = item.QualifiedQuantity,
                    OneQualifiedQuantity = item.OneQualifiedQuantity,
                    UnQualifiedQuantity = item.UnQualifiedQuantity,
                    QualifiedRate = Math.Round(item.QualifiedQuantity / (item.QualifiedQuantity + item.UnQualifiedQuantity) * 100, 2),
                    OneQualifiedRate = Math.Round(item.OneQualifiedQuantity / (item.QualifiedQuantity + item.UnQualifiedQuantity) * 100, 2)
                });
            }
            return new PagedInfo<QualificationRateReportDto>(listDto, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 获取所有工序
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<SelectOptionDto>> GetProcdureListAsync()
        {
            var procducesInfo = await _qualificationRateReportRepository.GetProcdureInfoAsync();

            return procducesInfo.Select(s => new SelectOptionDto
            {
                Key = $"{s.Id}",
                Label = $"【{s.Code}】 {s.Name}",
                Value = $"{s.Id}"
            });
        }

        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        public async Task<ExportResultDto> ExportExcelAsync(QualificationRateReportPagedQueryDto queryDto)
        {
            string fileName = string.Format("({0})合格率报表", DateTime.Now.ToString("yyyyMMddHHmmss"));
            queryDto.PageIndex = 1;
            queryDto.PageSize = 1000000;

            var pageData = await GetPagedListAsync(queryDto);

            List<QualificationRateExportDto> exportExcels = new();

            foreach (var item in pageData.Data)
            {
                QualificationRateExportDto exportExcel = new()
                {
                    OrderCode = item.OrderCode,
                    MaterialName = item.MaterialName,
                    ProcedureName = item.ProcedureName,
                    StartOn = item.StartOn,
                    EndOn = item.EndOn,
                    QualifiedQuantity = Math.Round(item.QualifiedQuantity),
                    UnQualifiedQuantity = Math.Round(item.UnQualifiedQuantity),
                    QualifiedRate = Math.Round(item.QualifiedRate, 2),
                    OneQualifiedRate = Math.Round(item.OneQualifiedRate.GetValueOrDefault(), 2),
                };

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
}
