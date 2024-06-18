using Hymson.Infrastructure;
using Hymson.MES.CoreServices.Dtos.Common;
using Hymson.MES.Services.Dtos.Common;
using Hymson.MES.Services.Dtos.Report;
using Microsoft.AspNetCore.Mvc;
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
        public Task<PagedInfo<ProductDetailReportOutputDto>> GetPageInfoAsync(ProductDetailReportPageQueryDto queryDto);

        /// <summary>
        /// 产能报表-导出Excel
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        public Task<ExportResultDto> ExportExcelAsync(ProductDetailReportPageQueryDto queryDto);

        /// <summary>
        /// 获取下线工序产出汇总数
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        public Task<decimal> GetOutputQtyAsync([FromQuery] ProductDetailReportQueryDto queryDto);

        /// <summary>
        /// 获取所有工序
        /// </summary>
        /// <returns></returns>
        public Task<IEnumerable<SelectOptionDto>> GetProcdureListAsync();
    }
}
