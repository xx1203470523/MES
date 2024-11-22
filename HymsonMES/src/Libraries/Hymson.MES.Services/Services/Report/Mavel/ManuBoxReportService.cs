using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Excel.Abstractions;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.Localization.Services;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Manufacture.ManuRequistionOrder;
using Hymson.MES.Data.Repositories.Manufacture.Query;
using Hymson.MES.Services.Dtos.Manufacture;
using Hymson.MES.Services.Dtos.NioPushCollection;
using Hymson.Minio;
using Hymson.Snowflake;
using Hymson.Utils;

namespace Hymson.MES.Services.Services.Manufacture
{
    /// <summary>
    /// 服务（领料记录详情） 
    /// </summary>
    public class ManuBoxReportService : IManuBoxReportService
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
        /// 仓储接口（领料记录详情）
        /// </summary>
        private readonly IManuProductReceiptOrderDetailRepository _manuProductReceiptOrderDetailRepository;

        private readonly IExcelService _excelService;
        private readonly ILocalizationService _localizationService;
        private readonly IMinioService _minioService;

        /// <summary>
        /// 构造函数
        /// </summary>
        public ManuBoxReportService(ICurrentUser currentUser, 
            ICurrentSite currentSite,
            IManuProductReceiptOrderDetailRepository manuProductReceiptOrderDetailRepository,
            IExcelService excelService,
            ILocalizationService localizationService,
            IMinioService minioService
            )
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _manuProductReceiptOrderDetailRepository = manuProductReceiptOrderDetailRepository;
            _excelService = excelService;
            _localizationService = localizationService;
            _minioService = minioService;
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ReportBoxResultDto>> GetPagedListAsync(ReportBoxQueryDto pagedQueryDto)
        {
            //var pagedQuery = pagedQueryDto.ToQuery<ManuRequistionOrderDetailPagedQuery>();
            //pagedQuery.SiteId = _currentSite.SiteId ?? 0;
            var pagedInfo = await _manuProductReceiptOrderDetailRepository.GetReportPagedInfoAsync(pagedQueryDto);

            // 实体到DTO转换 装载数据
            var dtos = pagedInfo.Data;
            return new PagedInfo<ReportBoxResultDto>(dtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 根据查询条件导出参数数据
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<NioPushCollectionExportResultDto> ExprotAsync(ReportBoxQueryDto pagedQueryDto)
        {
            var pagedQuery = pagedQueryDto.ToQuery<ReportBoxQueryDto>();
            pagedQuery.PageSize = 1000;
            var pagedInfoList = await _manuProductReceiptOrderDetailRepository.GetReportPagedInfoAsync(pagedQueryDto);
            //var dtos = pagedInfoList.Data.Select(a => a.ToModel<ManuRequistionOrderDto>());
            var dtos = pagedInfoList.Data.Select(a => new ReportBoxResultDto
            {
                InWmsData = a.InWmsData,
                OrderCode = a.OrderCode,
                CompletionOrderCode = a.CompletionOrderCode ?? "",
                Sfc = a.Sfc ?? "",
                Specifications = a.Specifications,
                ContaineCode = a.ContaineCode,
                MaterialName = a.MaterialName,
                MaterialCode = a.MaterialCode,
                Unit = a.Unit,
                Qty = a.Qty,
                InWmsQty = a.InWmsQty,
                WorkPlanCode = a.WorkPlanCode,
                WarehouseCode = a.WarehouseCode,
                CreatedBy =a.CreatedBy,
                Status = a.Status
            });

            // 实体到DTO转换 装载数据
            //var dtos = pagedInfo.Data.Select(s => s.ToModel<NioPushCollectionDto>());

            var pagedInfo = new PagedInfo<ReportBoxResultDto>(dtos, pagedInfoList.PageIndex, pagedInfoList.PageSize, pagedInfoList.TotalCount);

            //实体到DTO转换 装载数据
            List<ManuBoxReportExportDto> listDto = new();

            if (pagedInfo.Data == null || !pagedInfo.Data.Any())
            {
                var filePathN = await _excelService.ExportAsync(listDto, _localizationService.GetResource("ManuBoxReport"), _localizationService.GetResource("ManuBoxReport"));
                //上传到文件服务器
                var uploadResultN = await _minioService.PutObjectAsync(filePathN);
                return new NioPushCollectionExportResultDto
                {
                    FileName = _localizationService.GetResource("ManuBoxReport"),
                    Path = uploadResultN.AbsoluteUrl,
                };
            }
            //对应的excel数值从这里开始
            foreach (var a in pagedInfo.Data)
            {
                listDto.Add(new ManuBoxReportExportDto()
                {
                    InWmsData = a.InWmsData,
                    OrderCode = a.OrderCode,
                    CompletionOrderCode = a.CompletionOrderCode ?? "",
                    Sfc = a.Sfc ?? "",
                    Specifications = a.Specifications,
                    ContaineCode = a.ContaineCode,
                    MaterialName = a.MaterialName,
                    MaterialCode = a.MaterialCode,
                    Unit = a.Unit,
                    Qty = a.Qty,
                    InWmsQty = a.InWmsQty,
                    WorkPlanCode = a.WorkPlanCode,
                    WarehouseCode = a.WarehouseCode,
                    CreatedBy = a.CreatedBy,
                    Status = a.Status
                });
            }

            var filePath = await _excelService.ExportAsync(listDto, _localizationService.GetResource("ManuBoxReport"), _localizationService.GetResource("ManuBoxReport"));
            //上传到文件服务器
            var uploadResult = await _minioService.PutObjectAsync(filePath);
            return new NioPushCollectionExportResultDto
            {
                FileName = _localizationService.GetResource("ManuBoxReport"),
                Path = uploadResult.AbsoluteUrl,
            };

        }


    }
}
