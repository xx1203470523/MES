using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Excel;
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
using Hymson.MES.Data.Repositories.NIO.NioPushCollection.View;
using Hymson.MES.Data.Repositories.NioPushCollection.Query;
using Hymson.MES.Data.Repositories.Process.Query;
using Hymson.MES.Services.Dtos.Manufacture;
using Hymson.MES.Services.Dtos.NioPushCollection;
using Hymson.Minio;
using Hymson.Snowflake;
using Hymson.Utils;
using Minio.DataModel;
using System.Linq;

namespace Hymson.MES.Services.Services.Manufacture
{
    /// <summary>
    /// 服务（领料记录详情） 
    /// </summary>
    public class ManuRequistionOrderReportService : IManuRequistionOrderReportService
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
        private readonly IManuRequistionOrderDetailRepository _manuRequistionOrderDetailRepository;

        private readonly IExcelService _excelService;
        private readonly ILocalizationService _localizationService;
        private readonly IMinioService _minioService;

        /// <summary>
        /// 构造函数
        /// </summary>
        public ManuRequistionOrderReportService(
            ICurrentUser currentUser, 
            ICurrentSite currentSite, 
            IManuRequistionOrderDetailRepository manuRequistionOrderDetailRepository,
            IExcelService excelService,
            ILocalizationService localizationService,
            IMinioService minioService
            )
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _manuRequistionOrderDetailRepository = manuRequistionOrderDetailRepository;
            _excelService = excelService;
            _localizationService = localizationService;
            _minioService = minioService;
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ReportRequistionOrderResultDto>> GetPagedListAsync(ReportRequistionOrderQueryDto pagedQueryDto)
        {
            //var pagedQuery = pagedQueryDto.ToQuery<ManuRequistionOrderDetailPagedQuery>();
            //pagedQuery.SiteId = _currentSite.SiteId ?? 0;
            var pagedInfo = await _manuRequistionOrderDetailRepository.GetReportPagedInfoAsync(pagedQueryDto);

            // 实体到DTO转换 装载数据
            var dtos = pagedInfo.Data;
            return new PagedInfo<ReportRequistionOrderResultDto>(dtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 查询仓库地址分组
        /// </summary>
        /// <returns></returns>
        public async Task<List<ManuRequistionOrderGroupDto>> GetWarehouseListAsync()
        {
            var pagedInfo = await _manuRequistionOrderDetailRepository.GetManuRequistionOrderGroupListAsync();
            return pagedInfo;
        }

        /// <summary>
        /// 根据查询条件导出参数数据
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<NioPushCollectionExportResultDto> ExprotAsync(ReportRequistionOrderQueryDto pagedQueryDto)
        {
            var pagedQuery = pagedQueryDto.ToQuery<ReportRequistionOrderQueryDto>();
            pagedQuery.PageSize = 1000;
            var pagedInfoList = await _manuRequistionOrderDetailRepository.GetReportPagedInfoAsync(pagedQueryDto);
            //var dtos = pagedInfoList.Data.Select(a => a.ToModel<ManuRequistionOrderDto>());
            var dtos = pagedInfoList.Data.Select(a => new ManuRequistionOrderDto
            {
                ReqDate = a.ReqDate,
                OutWmsDate = a.OutWmsDate,
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

            // 实体到DTO转换 装载数据
            //var dtos = pagedInfo.Data.Select(s => s.ToModel<NioPushCollectionDto>());

            var pagedInfo = new PagedInfo<ManuRequistionOrderDto>(dtos, pagedInfoList.PageIndex, pagedInfoList.PageSize, pagedInfoList.TotalCount);

            //实体到DTO转换 装载数据
            List<ManuRequistionOrderExportDto> listDto = new();

            if (pagedInfo.Data == null || !pagedInfo.Data.Any())
            {
                var filePathN = await _excelService.ExportAsync(listDto, _localizationService.GetResource("ManuRequistionOrder"), _localizationService.GetResource("ManuRequistionOrder"));
                //上传到文件服务器
                var uploadResultN = await _minioService.PutObjectAsync(filePathN);
                return new NioPushCollectionExportResultDto
                {
                    FileName = _localizationService.GetResource("ManuRequistionOrder"),
                    Path = uploadResultN.AbsoluteUrl,
                };
            }
            //对应的excel数值从这里开始
            foreach (var item in pagedInfo.Data)
            {
                listDto.Add(new ManuRequistionOrderExportDto()
                {
                    ReqDate = item.ReqDate,
                    OutWmsDate = item.OutWmsDate,
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
                    Status = item.Status
                });
            }

            var filePath = await _excelService.ExportAsync(listDto, _localizationService.GetResource("ManuRequistionOrder"), _localizationService.GetResource("ManuRequistionOrder"));
            //上传到文件服务器
            var uploadResult = await _minioService.PutObjectAsync(filePath);
            return new NioPushCollectionExportResultDto
            {
                FileName = _localizationService.GetResource("ManuRequistionOrder"),
                Path = uploadResult.AbsoluteUrl,
            };

        }

    }
}
