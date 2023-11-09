using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Plan;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.Report;
using Hymson.MES.Services.Dtos.Report;
using Hymson.MES.Core.Constants;
using Microsoft.AspNetCore.Mvc;
using Hymson.MES.Services.Dtos.Common;
using Hymson.MES.Services.Dtos.Plan;
using Hymson.Excel.Abstractions;
using Hymson.Minio;
using Hymson.MES.Services.Dtos.Report.Excel;

namespace Hymson.MES.Services.Services.Report
{
    public class ProductDetailService : IProductDetailService
    {

        private readonly IMinioService _minioService;
        private readonly IExcelService _excelService;

        #region 仓储层
        private readonly ICurrentSite _currentSite;
        private readonly IProcMaterialRepository _procMaterialRepository;
        private readonly IPlanWorkOrderRepository _planWorkOrderRepository;
        private readonly IManuSfcSummaryRepository _manuSfcSummaryRepository;
        private readonly IProductDetailReportRepository _productDetailRepository;
        #endregion


        public ProductDetailService(IMinioService minioService,
            IExcelService excelService,
            ICurrentSite currentSite,
            IProcMaterialRepository procMaterialRepository,
            IPlanWorkOrderRepository planWorkOrderRepository,
            IManuSfcSummaryRepository manuSfcSummaryRepository,
            IProductDetailReportRepository productDetailRepository)
        {
            _minioService = minioService;
            _excelService = excelService;
            _currentSite = currentSite;
            _procMaterialRepository = procMaterialRepository;
            _planWorkOrderRepository = planWorkOrderRepository;
            _manuSfcSummaryRepository = manuSfcSummaryRepository;
            _productDetailRepository = productDetailRepository;
        }

        /// <summary>
        /// 产能报表-分页查询
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ProductDetailReportOutputDto>> GetPageInfoAsync(ProductDetailReportQueryDto queryDto)
        {
            var result = new PagedInfo<ProductDetailReportOutputDto>(new List<ProductDetailReportOutputDto>(), queryDto.PageIndex, queryDto.PageSize);

            if (!string.IsNullOrEmpty(queryDto.OrderCode))
            {
                var searchPlanWorkOrderEntity = await _planWorkOrderRepository.GetByCodeAsync(new() { OrderCode = queryDto.OrderCode })
                    ?? throw new CustomerValidationException(nameof(ErrorCode.MES17313));
                queryDto.OrderId = searchPlanWorkOrderEntity.Id;
            }

            var query = queryDto.ToQuery<ProductDetailReportQuery>();

            var pageData = await _productDetailRepository.GetPageInfoAsync(query);

            var productIds = pageData.Data.Select(a => a.ProductId);
            var productEntities = await _procMaterialRepository.GetByIdsAsync(productIds);

            var workOrderIds = pageData.Data.Select(a => a.WorkOrderId.GetValueOrDefault());
            var workOrderEntities = await _planWorkOrderRepository.GetByIdsAsync(workOrderIds.ToArray());

            List<ProductDetailReportOutputDto> list = new List<ProductDetailReportOutputDto>();
            foreach (var item in pageData.Data)
            {
                var product = productEntities.FirstOrDefault(a => a.Id == item.ProductId);

                var planWorkOrder = workOrderEntities.FirstOrDefault(a => a.Id == item.WorkOrderId);

                item.MaterialCode = product?.MaterialCode;
                item.MaterialName = product?.MaterialName;
                item.OrderCode = planWorkOrder?.OrderCode;

                item.Type = query.Type switch
                {
                    13 => "按每日查询",
                    10 => "按每月查询",
                    _ => ""
                };

                item.StartDate = query.Type switch
                {
                    13 => item.StartDate?.Substring(11, 2) + ":00",
                    10 => item.StartDate,
                    _ => ""
                };

                item.EndDate = query.Type switch
                {
                    13 => item.EndDate?.Substring(11, 2) + ":00",
                    10 => item.EndDate,
                    _ => ""
                };

                list.Add(item.ToModel<ProductDetailReportOutputDto>());
            }
            result.TotalCount = pageData.TotalCount;
            result.Data = list;

            return result;
        }

        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        public async Task<ExportResultDto> ExportExcelAsync(ProductDetailReportQueryDto queryDto)
        {
            string fileName = string.Format("({0})产能报表", DateTime.Now.ToString("yyyyMMddHHmmss"));
            queryDto.PageSize = 1000000;

            var pageData = await GetPageInfoAsync(queryDto);

            List<ProductDetailExportDto> exportExcels = new List<ProductDetailExportDto>();
            foreach (var item in pageData.Data)
            {
                ProductDetailExportDto exportExcel = new ProductDetailExportDto();

                exportExcel.StartDate = item.StartDate;
                exportExcel.EndDate = item.EndDate;
                exportExcel.UpdatedOn = item.UpdatedOn;
                exportExcel.OutputQty = item.OutputQty;
                exportExcel.FeedingQty = item.FeedingQty;
                exportExcel.MaterialCode = item.MaterialCode;
                exportExcel.MaterialName = item.MaterialName;
                exportExcel.OrderCode = item.OrderCode;
                exportExcel.Type = item.Type;

                exportExcels.Add(exportExcel);
            }

            var filePath = await _excelService.ExportAsync(exportExcels, fileName);
            //上传到文件服务器
            var uploadResult = await _minioService.PutObjectAsync(filePath);
            return new ExportResultDto
            {
                FileName = fileName,
                Path = uploadResult.AbsoluteUrl,
            };
        }
    }
}
