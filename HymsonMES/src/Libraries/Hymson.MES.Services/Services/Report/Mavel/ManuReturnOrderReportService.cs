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
using Hymson.MES.Core.Enums.Warehouse;
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
    public class ManuReturnOrderReportService : IManuReturnOrderReportService
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
        private readonly IManuReturnOrderDetailRepository _manuReturnOrderDetailRepository;

        private readonly IExcelService _excelService;
        private readonly ILocalizationService _localizationService;
        private readonly IMinioService _minioService;

        /// <summary>
        /// 构造函数
        /// </summary>
        public ManuReturnOrderReportService(ICurrentUser currentUser, 
            ICurrentSite currentSite,
            IManuReturnOrderDetailRepository manuReturnOrderDetailRepository,
            IExcelService excelService,
            ILocalizationService localizationService,
            IMinioService minioService
            )
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _manuReturnOrderDetailRepository = manuReturnOrderDetailRepository;
            _excelService = excelService;
            _localizationService = localizationService;
            _minioService = minioService;
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary> 
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ReportReturnOrderResultDto>> GetPagedListAsync(ReportReturnOrderQueryDto pagedQueryDto)
        {
            //var pagedQuery = pagedQueryDto.ToQuery<ManuRequistionOrderDetailPagedQuery>();
            //pagedQuery.SiteId = _currentSite.SiteId ?? 0;
            var pagedInfo = await _manuReturnOrderDetailRepository.GetReportPagedInfoAsync(pagedQueryDto);

            // 实体到DTO转换 装载数据
            var dtos = pagedInfo.Data;
            return new PagedInfo<ReportReturnOrderResultDto>(dtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 根据查询条件导出参数数据
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<NioPushCollectionExportResultDto> ExprotAsync(ReportReturnOrderQueryDto pagedQueryDto)
        {
            var pagedQuery = pagedQueryDto.ToQuery<ReportReturnOrderQueryDto>();
            pagedQuery.PageSize = 100000;
            var pagedInfoList = await _manuReturnOrderDetailRepository.GetReportPagedInfoAsync(pagedQueryDto);
            //var dtos = pagedInfoList.Data.Select(a => a.ToModel<ManuRequistionOrderDto>());
            var dtos = pagedInfoList.Data.Select(a => new ManuRequistionOrderDto
            {
                ReturnDate = a.ReturnDate,
                ReturnOrderCode = a.ReturnOrderCode,
                OrderCode = a.OrderCode,
                MaterialCode = a.MaterialCode ?? "",
                MaterialName = a.MaterialName ?? "",
                Specifications = a.Specifications,
                Unit = a.Unit,
                OrderQty = a.OrderQty,
                ReqQty = a.ReqQty,
                WorkPlanCode = a.WorkPlanCode ?? "",
                Warehouse = a.Warehouse,
                CreatedBy = a.CreatedBy,
                Status = a.Status
            });

            var pagedInfo = new PagedInfo<ManuRequistionOrderDto>(dtos, pagedInfoList.PageIndex, pagedInfoList.PageSize, pagedInfoList.TotalCount);

            //实体到DTO转换 装载数据
            List<ManuReturnOrderExportDto> listDto = new();

            if (pagedInfo.Data == null || !pagedInfo.Data.Any())
            {
                var filePathN = await _excelService.ExportAsync(listDto, _localizationService.GetResource("ManuReturnOrder"), _localizationService.GetResource("ManuReturnOrder"));
                //上传到文件服务器
                var uploadResultN = await _minioService.PutObjectAsync(filePathN);
                return new NioPushCollectionExportResultDto
                {
                    FileName = _localizationService.GetResource("ManuReturnOrder"),
                    Path = uploadResultN.AbsoluteUrl,
                };
            }
            //对应的excel数值从这里开始
            foreach (var item in pagedInfo.Data)
            {
                listDto.Add(new ManuReturnOrderExportDto()
                {
                    ReturnDate = item.ReturnDate,
                    ReturnOrderCode = item.ReturnOrderCode,
                    OrderCode = item.OrderCode,
                    MaterialCode = item.MaterialCode ?? "",
                    MaterialName = item.MaterialName ?? "",
                    Specifications = item.Specifications,
                    Unit = item.Unit,
                    OrderQty = item.OrderQty,
                    ReqQty = item.ReqQty,
                    WorkPlanCode = item.WorkPlanCode ?? "",
                    Warehouse = item.Warehouse,
                    CreatedBy = item.CreatedBy,
                    Status = (WhWarehouseMaterialReturnStatusEnum)((int)item.Status)
                });
            }            
            var filePath = await _excelService.ExportAsync(listDto, _localizationService.GetResource("ManuReturnOrder"), _localizationService.GetResource("ManuReturnOrder"));
            var uploadResult = await _minioService.PutObjectAsync(filePath);
            
            
            //上传到文件服务器
            
            return new NioPushCollectionExportResultDto
            {
                FileName = _localizationService.GetResource("ManuReturnOrder"),
                Path = uploadResult.AbsoluteUrl,
            };

        }

    }
}
