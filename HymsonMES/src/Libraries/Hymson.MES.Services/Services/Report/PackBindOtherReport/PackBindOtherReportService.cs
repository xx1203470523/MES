using Hymson.Excel.Abstractions;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Services.Dtos.Common;
using Hymson.MES.Services.Dtos.Report;
using Hymson.Minio;
using Hymson.Utils;
using Microsoft.AspNetCore.Components.Web;
using MySqlX.XDevAPI.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Services.Report.PackBindOtherReport
{
    public class PackBindOtherReportService : IPackBindOtherReportService
    {
        private IMinioService _minioService;
        private IExcelService _excelService;

        private IManuSfcCirculationRepository _manuSfcCirculationRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="minioService"></param>
        /// <param name="excelService"></param>
        /// <param name="manuSfcCirculationRepository"></param>
        public PackBindOtherReportService(
            IMinioService minioService,
            IExcelService excelService,
            IManuSfcCirculationRepository manuSfcCirculationRepository)
        {
            _excelService = excelService;
            _minioService = minioService;   
            _manuSfcCirculationRepository = manuSfcCirculationRepository;
        }

        /// <summary>
        /// 分页查询外箱码
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<PagedInfo<PackBindOtherReportViewDto>> GetPagedInfoAsync(PackBindOtherPageQueryPagedDto query)
        {
            //只查询绑定类型为外箱码绑定的条码
            var manuSfcCirculationEntities = await _manuSfcCirculationRepository.GetPagedInfoAsync(new()
            {
                SFC = query.Sfc,
                CirculationBarCode = query.BindSfc,
                CirculationTypes = new SfcCirculationTypeEnum[] {  SfcCirculationTypeEnum.BindPack1,SfcCirculationTypeEnum.BindPack2, SfcCirculationTypeEnum.BindPack3, SfcCirculationTypeEnum.BindPack4 },
                PageIndex = query.PageIndex,
                PageSize = query.PageSize,
            });

            var pageInfo = new List<PackBindOtherReportViewDto>();
            foreach (var item in manuSfcCirculationEntities.Data)
            {
                PackBindOtherReportViewDto newItem = new();
                newItem.SFC = item.SFC;
                newItem.BindSfc = item.CirculationBarCode;
                newItem.CreatedBy = item.CreatedBy;
                newItem.CreatedOn = item.CreatedOn;
                newItem.CirculationType = item.CirculationType;

                pageInfo.Add(newItem);
            }

            return new PagedInfo<PackBindOtherReportViewDto>(pageInfo, query.PageIndex, query.PageSize);
        }

        public async Task<ExportResultDto> ExportExcelAsync(PackBindOtherQueryDto query)
        {
            var fileName = "门包箱条码绑定报表";
            var result = new ExportResultDto();
            //只查询绑定类型为外箱码绑定的条码
            var manuSfcCirculationEntities = await _manuSfcCirculationRepository.GetListAsync(new()
            {
                SFC = query.Sfc,
                CirculationBarCode = query.BindSfc,
                CirculationTypes = new SfcCirculationTypeEnum[] { SfcCirculationTypeEnum.BindPack1, SfcCirculationTypeEnum.BindPack2, SfcCirculationTypeEnum.BindPack3, SfcCirculationTypeEnum.BindPack4 }
            });

            var resultData = new List<PackBindOtherReportExcelDto>();
            foreach (var item in manuSfcCirculationEntities)
            {
                PackBindOtherReportExcelDto newItem = item.ToExcelModel<PackBindOtherReportExcelDto>();
                newItem.BindSfc = item.CirculationBarCode;
                newItem.CirculationTypeName = item.CirculationType.GetDescription();
                resultData.Add(newItem);
            }

            if (resultData?.Any() == true)
            {
                var filePath = await _excelService.ExportAsync(resultData, fileName, fileName);
                //上传到文件服务器  
                var uploadResult = await _minioService.PutObjectAsync(filePath);
                result.FileName = fileName;
                result.Path = uploadResult.AbsoluteUrl;
                result.RelativePath = uploadResult.RelativeUrl;
            }

            return result;
        }
    }
}
