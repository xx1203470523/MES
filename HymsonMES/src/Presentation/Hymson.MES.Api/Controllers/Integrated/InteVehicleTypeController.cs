using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Integrated;
using Hymson.MES.Services.Services.Integrated;
using Hymson.Web.Framework.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Integrated
{
    /// <summary>
    /// 控制器（载具类型维护）
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class InteVehicleTypeController : ControllerBase
    {
        /// <summary>
        /// 接口（载具类型维护）
        /// </summary>
        private readonly IInteVehicleTypeService _inteVehicleTypeService;
        private readonly ILogger<InteVehicleTypeController> _logger;

        /// <summary>
        /// 构造函数（载具类型维护）
        /// </summary>
        /// <param name="inteVehicleTypeService"></param>
        /// <param name="logger"></param>
        public InteVehicleTypeController(IInteVehicleTypeService inteVehicleTypeService, ILogger<InteVehicleTypeController> logger)
        {
            _inteVehicleTypeService = inteVehicleTypeService;
            _logger = logger;
        }

        #region 框架生成方法

        /// <summary>
        /// 分页查询列表（载具类型维护）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("pagelist")]
        public async Task<PagedInfo<InteVehicleTypeDto>> QueryPagedInteVehicleTypeAsync([FromQuery] InteVehicleTypePagedQueryDto parm)
        {
            return await _inteVehicleTypeService.GetPagedListAsync(parm);
        }

        /// <summary>
        /// 查询详情（载具类型维护）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<InteVehicleTypeDto> QueryInteVehicleTypeByIdAsync(long id)
        {
            return await _inteVehicleTypeService.QueryInteVehicleTypeByIdAsync(id);
        }

        /// <summary>
        /// 添加（载具类型维护）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        [LogDescription("载具类型", BusinessType.INSERT)]
        [PermissionDescription("inte:inteVehicleType:insert")]
        public async Task AddInteVehicleTypeAsync([FromBody] InteVehicleTypeCreateDto parm)
        {
             await _inteVehicleTypeService.CreateInteVehicleTypeAsync(parm);
        }

        /// <summary>
        /// 更新（载具类型维护）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("update")]
        [LogDescription("载具类型", BusinessType.UPDATE)]
        [PermissionDescription("inte:inteVehicleType:update")]
        public async Task UpdateInteVehicleTypeAsync([FromBody] InteVehicleTypeModifyDto parm)
        {
             await _inteVehicleTypeService.ModifyInteVehicleTypeAsync(parm);
        }

        /// <summary>
        /// 删除（载具类型维护）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete")]
        [LogDescription("载具类型", BusinessType.DELETE)]
        [PermissionDescription("inte:inteVehicleType:delete")]
        public async Task DeleteInteVehicleTypeAsync([FromBody] long[] ids)
        {
            await _inteVehicleTypeService.DeletesInteVehicleTypeAsync(ids);
        }

        #endregion

        /// <summary>
        /// 查询详情（载具类型维护）
        /// </summary>
        /// <param name="vehicleTypeId"></param>
        /// <returns></returns>
        [HttpGet("getVehicleTypeVerify/{vehicleTypeId}")]
        public async Task<IEnumerable<InteVehicleTypeVerifyDto>> QueryInteVehicleTypeVerifyByVehicleTypeIdAsync(long vehicleTypeId)
        {
            return await _inteVehicleTypeService.QueryInteVehicleTypeVerifyByVehicleTypeIdAsync(vehicleTypeId);
        }
    }
}