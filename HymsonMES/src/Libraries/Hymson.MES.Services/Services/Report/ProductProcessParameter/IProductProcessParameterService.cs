using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Services.Report.ProductProcessParameter
{
    public interface IProductProcessParameterService
    {
        /// <summary>
        /// 产品过程参数报表分页查询
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<ProductProcessParameterReportDto>> GetPageListAsync(ProductProcessParameterReportPagedQueryDto pagedQueryDto);

        /// <summary>
        /// 根据查询条件导出数据
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        Task<ProductProcessParameterExportResultDto> ExprotProductProcessParameterListAsync(ProductProcessParameterReportPagedQueryDto pagedQueryDto);
    }
}
