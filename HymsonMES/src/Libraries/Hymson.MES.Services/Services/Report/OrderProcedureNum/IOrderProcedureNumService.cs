using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Common;
using Hymson.MES.Services.Dtos.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Services.Report.OrderProcedureNum
{
    /// <summary>
    /// 工单工序数量
    /// </summary>
    public interface IOrderProcedureNumService
    {
        /// <summary>
        /// 获取工单工序数量
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<PagedInfo<OrderProcedureNumResultDto>> GetOrderProcedureNumListAsync(OrderProcedureNumDto param);

        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<ExportResponseDto> ExportOrderProcedureNumListAsync(OrderProcedureNumDto param);
    }
}
