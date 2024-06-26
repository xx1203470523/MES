using Hymson.MES.SystemServices.Dtos;
using Hymson.MES.SystemServices.Services.Integrated;
using Hymson.Web.Framework.Attributes;
using Hymson.Web.Framework.Filters.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.System.Api.Controllers
{
    /// <summary>
    /// 综合
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class IntegratedController : ControllerBase
    {
        /// <summary>
        /// 日志对象
        /// </summary>
        private readonly ILogger<IntegratedController> _logger;

        /// <summary>
        /// 接口（客户）
        /// </summary>
        private readonly IInteCustomerService _inteCustomerService;

        /// <summary>
        /// 接口（供应商）
        /// </summary>
        private readonly IInteSupplierService _inteSupplierService;

        /// <summary>
        /// 接口（单位）
        /// </summary>
        private readonly IInteUnitService _inteUnitService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="inteCustomerService"></param>
        /// <param name="inteSupplierService"></param>
        /// <param name="inteUnitService"></param>
        public IntegratedController(ILogger<IntegratedController> logger,
            IInteCustomerService inteCustomerService,
            IInteSupplierService inteSupplierService,
            IInteUnitService inteUnitService)
        {
            _logger = logger;
            _inteCustomerService = inteCustomerService;
            _inteSupplierService = inteSupplierService;
            _inteUnitService = inteUnitService;
        }

        /// <summary>
        /// 客户信息（同步）
        /// </summary>
        /// <returns></returns>
        [HttpPost("Customer/sync")]
        [ProducesResponseType(typeof(ResultDto), 200)]
        [LogDescription("客户信息（同步）", BusinessType.INSERT)]
        public async Task SyncCustomerAsync(IEnumerable<InteCustomerDto> requestDtos)
        {
            _ = await _inteCustomerService.SyncCustomerAsync(requestDtos);
        }

        /// <summary>
        /// 供应商信息（同步）
        /// </summary>
        /// <returns></returns>
        [HttpPost("Supplier/sync")]
        [ProducesResponseType(typeof(ResultDto), 200)]
        [LogDescription("供应商信息（同步）", BusinessType.INSERT)]
        public async Task SyncSupplierAsync(IEnumerable<InteSupplierDto> requestDtos)
        {
            _ = await _inteSupplierService.SyncSupplierAsync(requestDtos);
        }

        /// <summary>
        /// 单位信息（同步）
        /// </summary>
        /// <returns></returns>
        [HttpPost("Unit/sync")]
        [ProducesResponseType(typeof(ResultDto), 200)]
        [LogDescription("单位信息（同步）", BusinessType.INSERT)]
        public async Task SyncUnitAsync(IEnumerable<InteUnitDto> requestDtos)
        {
            _ = await _inteUnitService.SyncUnitAsync(requestDtos);
        }

    }
}