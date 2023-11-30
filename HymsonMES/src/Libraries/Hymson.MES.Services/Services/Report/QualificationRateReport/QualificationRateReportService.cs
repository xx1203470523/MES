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

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="currentSite"></param>
        /// <param name="qualificationRateReportRepository"></param>
        /// <param name="planWorkOrderRepository"></param>
        /// <param name="procMaterialRepository"></param>
        /// <param name="procProcedureRepository"></param>
        public QualificationRateReportService(ICurrentUser currentUser, ICurrentSite currentSite, 
            IQualificationRateReportRepository qualificationRateReportRepository,
            IPlanWorkOrderRepository planWorkOrderRepository,
            IProcMaterialRepository procMaterialRepository,
            IProcProcedureRepository procProcedureRepository)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _qualificationRateReportRepository = qualificationRateReportRepository;
            _planWorkOrderRepository = planWorkOrderRepository;
            _procMaterialRepository = procMaterialRepository;
            _procProcedureRepository = procProcedureRepository;
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
            IEnumerable<PlanWorkOrderEntity>? workOrderInfos=new List<PlanWorkOrderEntity>();

            workOrderInfos = await _planWorkOrderRepository.GetsByCodeAsync(new PlanWorkOrderQuery() { OrderCode = pagedQueryDto.OrderCode??"", SiteId = pagedQuery.SiteId });
            //工单为空 直接返回
            if (workOrderInfos == null || !workOrderInfos.Any())  return new PagedInfo<QualificationRateReportDto>(listDto, 1, 0, 0);
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
                listDto.Add(new QualificationRateReportDto
                {
                    OrderCode= workOrderInfo!=null ? workOrderInfo.OrderCode : "",
                    MaterialName= productInfo!=null ? productInfo.MaterialName : "",
                    ProcedureName=procedureInfo != null ? procedureInfo.Name : "",
                    StartOn= pagedQueryDto.Type==1?
                        (item.StartOn.Year+"-"+ item.StartOn.Month+"-"+ item.StartOn.Day).ToString()
                        :(item.StartOn.Year + "-" + item.StartOn.Month + "-" + item.StartOn.Day + "  "+item.StartHour).ToString(),
                    EndOn= pagedQueryDto.Type == 1 ?
                        (item.StartOn.Year + "-" + item.StartOn.Month + "-" + item.StartOn.AddDays(1).Day).ToString()
                        : (item.StartOn.Year + "-" + item.StartOn.Month + "-" + item.StartOn.Day + "  " + (item.StartHour+1).ToString()).ToString(),
                    QualifiedQuantity =item.QualifiedQuantity,
                    UnQualifiedQuantity=item.UnQualifiedQuantity,
                    QualifiedRate=item.QualifiedQuantity/(item.QualifiedQuantity+item.UnQualifiedQuantity)*100,
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
    }
}
