using Hymson.Authentication.JwtBearer.Security;
using Hymson.MES.Services.Dtos.Report;
using Hymson.MES.Services.Services.Report.OrderProcedureNum;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Report
{
    /// <summary>
    /// 工单工序数量
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class OrderProcedureNumController : ControllerBase
    {
        /// <summary>
        /// 工单工序数量
        /// </summary>
        private readonly IOrderProcedureNumService _orderProcedureNumService;

        /// <summary>
        /// 构造函数
        /// </summary>
        public OrderProcedureNumController(IOrderProcedureNumService orderProcedureNumService)
        {
            _orderProcedureNumService = orderProcedureNumService;
        }

        /// <summary>
        /// 获取工单工序数量
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAll")]
        public async Task<OrderProcedureNumAllResultDto> GetOrderProcedureNumListAsync([FromQuery] OrderProcedureNumDto param)
        {
            return await _orderProcedureNumService.GetOrderProcedureNumListAsync(param);
        }
    }
}
