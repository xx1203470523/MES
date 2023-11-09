using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Common;
using Hymson.MES.Services.Dtos.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Services.Report
{
    public interface IProductDetailService
    {
        /// <summary>
        /// 产能报表-分页查询
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        public Task<PagedInfo<ProductDetailReportOutputDto>> GetPageInfoAsync(ProductDetailReportQueryDto queryDto);

        /// <summary>
        /// 产能报表-导出Excel
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        public Task<ExportResultDto> ExportExcelAsync(ProductDetailReportQueryDto queryDto);
    }
}
