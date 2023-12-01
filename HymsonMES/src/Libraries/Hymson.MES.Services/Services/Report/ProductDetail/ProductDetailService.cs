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
using Hymson.MES.Data.Repositories.Equipment.EquEquipment;
using Hymson.MES.CoreServices.Dtos.Common;
using Hymson.MES.Core.Enums.Integrated;
using Hymson.MES.Core.Enums;

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
        private readonly IEquEquipmentRepository _equEquipmentRepository;
        private readonly IProcResourceRepository _procResourceRepository;
        private readonly IProcProcedureRepository _procProcedureRepository;
        #endregion


        public ProductDetailService(IMinioService minioService,
            IExcelService excelService,
            ICurrentSite currentSite,
            IProcMaterialRepository procMaterialRepository,
            IPlanWorkOrderRepository planWorkOrderRepository,
            IManuSfcSummaryRepository manuSfcSummaryRepository,
            IProductDetailReportRepository productDetailRepository,
            IEquEquipmentRepository equEquipmentRepository,
            IProcProcedureRepository procProcedureRepository,
            IProcResourceRepository procResourceRepository)
        {
            _minioService = minioService;
            _excelService = excelService;
            _currentSite = currentSite;
            _procMaterialRepository = procMaterialRepository;
            _planWorkOrderRepository = planWorkOrderRepository;
            _manuSfcSummaryRepository = manuSfcSummaryRepository;
            _productDetailRepository = productDetailRepository;
            _equEquipmentRepository = equEquipmentRepository;
            _procResourceRepository = procResourceRepository;
            _procProcedureRepository= procProcedureRepository;
        }

        /// <summary>
        /// 产能报表-分页查询
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ProductDetailReportOutputDto>> GetPageInfoAsync(ProductDetailReportPageQueryDto queryDto)
        {
            var result = new PagedInfo<ProductDetailReportOutputDto>(new List<ProductDetailReportOutputDto>(), queryDto.PageIndex, queryDto.PageSize);

            if (!string.IsNullOrEmpty(queryDto.OrderCode))
            {
                var searchPlanWorkOrderEntitys = await _planWorkOrderRepository.GetsByCodeAsync(new() { OrderCode = queryDto.OrderCode,SiteId= _currentSite .SiteId??0 })
                    ?? throw new CustomerValidationException(nameof(ErrorCode.MES17313));
                queryDto.OrderId = searchPlanWorkOrderEntitys.Select(a => a.Id).ToArray();
            }

            var query = queryDto.ToQuery<ProductDetailReportPageQuery>();

            var pageData = await _productDetailRepository.GetPageInfoAsync(query);

            var productIds = pageData.Data.Select(a => a.ProductId);
            var productEntities = await _procMaterialRepository.GetByIdsAsync(productIds);

            var procedureIds=pageData.Data.Select(a => a.ProcedureId).ToArray();
            var proceduretEntities = await _procProcedureRepository.GetByIdsAsync(procedureIds);

            var workOrderIds = pageData.Data.Select(a => a.WorkOrderId.GetValueOrDefault());
            var workOrderEntities = await _planWorkOrderRepository.GetByIdsAsync(workOrderIds.ToArray());



            List<ProductDetailReportOutputDto> list = new List<ProductDetailReportOutputDto>();
            foreach (var item in pageData.Data)
            {
                var product = productEntities.FirstOrDefault(a => a.Id == item.ProductId);

                var procedure = proceduretEntities.FirstOrDefault(a => a.Id == item.ProcedureId);

                var planWorkOrder = workOrderEntities.FirstOrDefault(a => a.Id == item.WorkOrderId);

                item.MaterialCode = product?.MaterialCode;
                item.MaterialName = product?.MaterialName;
                item.OrderCode = planWorkOrder?.OrderCode;
                item.ProcedureName = procedure?.Name;
                

                item.Type = query.Type switch
                {
                    13 => "按每日查询",
                    10 => "按每月查询",
                    7 => "按每年查询",
                    _ => ""
                };

                item.SearchDate = query.Type switch
                {
                    13 => item.StartDate,
                    10 => item.StartDate?.Substring(0, 7),
                    7 => item.StartDate?.Substring(0, 4),
                    _ => item.StartDate,
                };

                item.StartDate = query.Type switch
                {
                    13 => item.StartDate?.Substring(11, 2) + ":00",
                    10 => item.StartDate,
                    7 => item.StartDate?.Substring(0,7),
                    _ => ""
                };

                item.EndDate = query.Type switch
                {
                    13 => item.EndDate?.Substring(11, 2) + ":00",
                    10 => item.EndDate,
                    7 => item.StartDate?.Substring(0, 7),
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
        public async Task<ExportResultDto> ExportExcelAsync(ProductDetailReportPageQueryDto queryDto)
        {
            string fileName = string.Format("({0})产能报表", DateTime.Now.ToString("yyyyMMddHHmmss"));
            queryDto.PageIndex = 1;
            queryDto.PageSize = 1000000;

            var pageData = await GetPageInfoAsync(queryDto);

            List<ProductDetailExportDto> exportExcels = new List<ProductDetailExportDto>();
            ProductDetailExportDto exportExcel = new ProductDetailExportDto();
            foreach (var item in pageData.Data)
            {
                exportExcel.StartDate = item.StartDate;
                exportExcel.EndDate = item.EndDate;
                exportExcel.OutputQty = item.OutputQty;
                exportExcel.FeedingQty = item.FeedingQty;
                exportExcel.MaterialCode = item.MaterialCode;
                exportExcel.MaterialName = item.MaterialName;
                exportExcel.OrderCode = item.OrderCode;
                exportExcel.Type = item.Type;
                exportExcel.ProcedureName = item.ProcedureName;

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

        /// <summary>
        /// 获取下线工序产出汇总数
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        public async Task<decimal> GetOutputQtyAsync([FromQuery] ProductDetailReportQueryDto queryDto)
        {
            decimal outputQty = 0;

            var query = new ProductDetailReportQuery() {
                StartDate = queryDto.StartDate,
                EndDate = queryDto.EndDate,
            };

            if (queryDto.OrderCode != null)
            {
                var orderEntity = await _planWorkOrderRepository.GetByCodeAsync(new() { OrderCode = queryDto.OrderCode });
                if (orderEntity != null) query.OrderId = orderEntity.Id;
            }

            if (queryDto.EquipmentCode != null)
            {
                var equipmentEntity = await _equEquipmentRepository.GetByCodeAsync(new() { Code = queryDto.EquipmentCode });
                if (equipmentEntity != null) query.EquipmentId = equipmentEntity.Id;
            }

            if (queryDto.ResourceCode != null)
            {
                var resourceEntity = await _procResourceRepository.GetByCodeAsync(new() { Code = queryDto.ResourceCode });
                if (resourceEntity != null) query.ResourceId = resourceEntity.Id;
            }

            outputQty = await _productDetailRepository.GetOutputSumAsyc(query);

            return outputQty;
        }

        /// <summary>
        /// 获取所有工序
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<SelectOptionDto>> GetProcdureListAsync()
        {
            var procducesInfo = await _productDetailRepository.GetProcdureInfoAsync();

            return procducesInfo.Select(s => new SelectOptionDto
            {
                Key = $"{s.Id}",
                Label = $"【{s.Code}】 {s.Name}",
                Value = $"{s.Id}"
            });
        }

    }
}
