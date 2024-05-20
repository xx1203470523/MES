using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Excel.Abstractions;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Mapper;
using Hymson.Localization.Services;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Plan;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Services.Dtos.Report;
using Hymson.Minio;

namespace Hymson.MES.Services.Services.Report
{
    /// <summary>
    /// 不良记录报表 服务
    /// </summary>
    public class ComUsageReportService : IComUsageReportService
    {
        private readonly ICurrentUser _currentUser;
        private readonly ICurrentSite _currentSite;

        /// <summary>
        /// 表 仓储
        /// </summary>
        private readonly IManuSfcCirculationRepository _circulationRepository;

        private readonly IPlanWorkOrderRepository _planWorkOrderRepository;
        private readonly IProcMaterialRepository _procMaterialRepository;
        private readonly IExcelService _excelService;
        private readonly IMinioService _minioService;
        private readonly ILocalizationService _localizationService;
        private readonly IManuBarCodeRelationRepository _manuBarCodeRelationRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="currentSite"></param>
        /// <param name="planWorkOrderRepository"></param>
        /// <param name="procMaterialRepository"></param>
        /// <param name="circulationRepository"></param>
        /// <param name="excelService"></param>
        /// <param name="minioService"></param>
        /// <param name="localizationService"></param>
        /// <param name="manuBarCodeRelationRepository"></param>
        public ComUsageReportService(ICurrentUser currentUser, ICurrentSite currentSite, IPlanWorkOrderRepository planWorkOrderRepository, IProcMaterialRepository procMaterialRepository, IManuSfcCirculationRepository circulationRepository, IExcelService excelService, IMinioService minioService, ILocalizationService localizationService, IManuBarCodeRelationRepository manuBarCodeRelationRepository)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _manuBarCodeRelationRepository = manuBarCodeRelationRepository;
            _planWorkOrderRepository = planWorkOrderRepository;
            _procMaterialRepository = procMaterialRepository;
            _circulationRepository = circulationRepository;
            _excelService = excelService;
            _minioService = minioService;
            _localizationService = localizationService;
        }

        /// <summary>
        /// 根据查询条件获取车间作业控制报表分页数据
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ComUsageReportViewDto>> GetComUsagePageListAsync(ComUsageReportPagedQueryDto param)
        {
            var pagedQuery = param.ToQuery<ComUsageReportPagedQuery>();
            pagedQuery.SiteId = _currentSite.SiteId ?? 123456;
            var pagedInfo = await _manuBarCodeRelationRepository.GetReportPagedInfoAsync(pagedQuery);

            List<ComUsageReportViewDto> listDto = new List<ComUsageReportViewDto>();

            if (pagedInfo.Data == null || !pagedInfo.Data.Any())
            {
                return new PagedInfo<ComUsageReportViewDto>(listDto, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
            }

            var circulationProductIds = pagedInfo.Data.Select(x => x.InputBarCodeMaterialId).Distinct().ToList();
            var productIds = pagedInfo.Data.Select(x => x.OutputBarCodeMaterialId).Distinct().ToList();

            //合并 成 materialIds
            circulationProductIds.AddRange(productIds.OfType<long>());
            var materialIds = circulationProductIds.ToArray();
            var materials = await _procMaterialRepository.GetByIdsAsync(materialIds);

            //var workOrderIds = pagedInfo.Data.Select(x => x.WorkOrderId).Distinct().ToArray();
            //var workOrders = await _planWorkOrderRepository.GetByIdsAsync(workOrderIds);

            foreach (var item in pagedInfo.Data)
            {
                var product = materials != null && materials.Any() ? materials.FirstOrDefault(x => x.Id == item.OutputBarCodeMaterialId) : null;
                var circulationProduct = materials != null && materials.Any() ? materials.FirstOrDefault(x => x.Id == item.InputBarCodeMaterialId) : null;

                //var workOrder = materials != null && materials.Any() ? workOrders.FirstOrDefault(x => x.Id == item.WorkOrderId) : null;

                listDto.Add(new ComUsageReportViewDto()
                {
                    SFC = item.OutputBarCode,
                    ProductCodeVersion = product != null ? product.MaterialCode + "/" + product.Version : "",
                    //  OrderCode= workOrder!=null? workOrder.OrderCode : "",
                    CirculationBarCode = item.InputBarCode,
                    CirculationProductCodeVersion = circulationProduct != null ? circulationProduct.MaterialCode + "/" + circulationProduct.Version : "",
                });
            }

            return new PagedInfo<ComUsageReportViewDto>(listDto, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }


        /// <summary>
        /// 根据查询条件导出车间作业控制报表
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<ComUsageExportResultDto> ExprotComUsagePageListAsync(ComUsageReportPagedQueryDto param)
        {
            var pagedQuery = param.ToQuery<ComUsageReportPagedQuery>();
            pagedQuery.SiteId = _currentSite.SiteId.Value;
            pagedQuery.PageSize = 1000;
            var pagedInfo = await _circulationRepository.GetReportPagedInfoAsync(pagedQuery);

            List<ComUsageReportExcelExportDto> listDto = new List<ComUsageReportExcelExportDto>();

            if (pagedInfo.Data == null || !pagedInfo.Data.Any())
            {
                var filePathN = await _excelService.ExportAsync(listDto, _localizationService.GetResource("ComUsageReport"), _localizationService.GetResource("ComUsageReport"));
                //上传到文件服务器
                var uploadResultN = await _minioService.PutObjectAsync(filePathN);
                return new ComUsageExportResultDto
                {
                    FileName = _localizationService.GetResource("ComUsageReport"),
                    Path = uploadResultN.AbsoluteUrl,
                };
            }

            var circulationProductIds = pagedInfo.Data.Select(x => x.CirculationProductId).Distinct().ToList();
            var productIds = pagedInfo.Data.Select(x => x.ProductId).Distinct().ToList();

            //合并 成 materialIds
            circulationProductIds.AddRange(productIds);
            var materialIds = circulationProductIds.ToArray();
            var materials = await _procMaterialRepository.GetByIdsAsync(materialIds);

            var workOrderIds = pagedInfo.Data.Select(x => x.WorkOrderId).Distinct().ToArray();
            var workOrders = await _planWorkOrderRepository.GetByIdsAsync(workOrderIds);

            foreach (var item in pagedInfo.Data)
            {
                var product = materials != null && materials.Any() ? materials.FirstOrDefault(x => x.Id == item.ProductId) : null;
                var circulationProduct = materials != null && materials.Any() ? materials.FirstOrDefault(x => x.Id == item.CirculationProductId) : null;

                var workOrder = materials != null && materials.Any() ? workOrders.FirstOrDefault(x => x.Id == item.WorkOrderId) : null;

                listDto.Add(new ComUsageReportExcelExportDto()
                {
                    SFC = item.SFC,
                    ProductCodeVersion = product != null ? product.MaterialCode + "/" + product.Version : "",
                    OrderCode = workOrder != null ? workOrder.OrderCode : "",
                    CirculationBarCode = item.CirculationBarCode,
                    CirculationProductCodeVersion = circulationProduct != null ? circulationProduct.MaterialCode + "/" + circulationProduct.Version : "",
                });
            }

            var filePath = await _excelService.ExportAsync(listDto, _localizationService.GetResource("ComUsageReport") , _localizationService.GetResource("ComUsageReport"));
            //上传到文件服务器
            var uploadResult = await _minioService.PutObjectAsync(filePath);
            return new ComUsageExportResultDto
            {
                FileName = _localizationService.GetResource("ComUsageReport"),
                Path = uploadResult.AbsoluteUrl,
            };
        }

    }
}
