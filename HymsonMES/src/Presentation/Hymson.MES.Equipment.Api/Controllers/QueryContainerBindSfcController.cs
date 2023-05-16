using Hymson.MES.EquipmentServices.Request.QueryContainerBindSfc;
using Hymson.MES.EquipmentServices.Services.QueryContainerBindSfc;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Equipment.Api.Controllers
{
    /// <summary>
    ///容器内条码查询
    /// </summary>
    [Route("api/v1/[controller]")]
    [ApiController]
    public class QueryContainerBindSfcController : ControllerBase
    {
        private readonly IQueryContainerBindSfcService _QueryContainerBindSfcService;
        private readonly ILogger<QueryContainerBindSfcController> _logger;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="QueryContainerBindSfcService"></param>
        /// <param name="logger"></param>
        public QueryContainerBindSfcController(IQueryContainerBindSfcService QueryContainerBindSfcService, ILogger<QueryContainerBindSfcController> logger)
        {
            _QueryContainerBindSfcService = QueryContainerBindSfcService;
            _logger = logger;
        }

        /// <summary>
        ///容器内条码查询
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("QueryContainerBindSfc")]
        public async Task QueryContainerBindSfcAsync(QueryContainerBindSfcRequest request)
        {
            await _QueryContainerBindSfcService.QueryContainerBindSfcAsync(request);
        }
    }
}
