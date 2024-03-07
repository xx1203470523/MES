using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.ManuEuqipmentNewestInfo;
using Hymson.MES.Services.Services.ManuEuqipmentNewestInfo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.ManuEuqipmentNewestInfo
{
    /// <summary>
    /// 控制器（设备最新信息）
    /// @author Yxx
    /// @date 2024-03-07 09:00:41
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ManuEuqipmentNewestInfoController : ControllerBase
    {
        /// <summary>
        /// 日志
        /// </summary>
        private readonly ILogger<ManuEuqipmentNewestInfoController> _logger;
        /// <summary>
        /// 服务接口（设备最新信息）
        /// </summary>
        private readonly IManuEuqipmentNewestInfoService _manuEuqipmentNewestInfoService;


        /// <summary>
        /// 构造函数（设备最新信息）
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="manuEuqipmentNewestInfoService"></param>
        public ManuEuqipmentNewestInfoController(ILogger<ManuEuqipmentNewestInfoController> logger, IManuEuqipmentNewestInfoService manuEuqipmentNewestInfoService)
        {
            _logger = logger;
            _manuEuqipmentNewestInfoService = manuEuqipmentNewestInfoService;
        }

        /// <summary>
        /// 添加（设备最新信息）
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        public async Task AddAsync([FromBody] ManuEuqipmentNewestInfoSaveDto saveDto)
        {
             await _manuEuqipmentNewestInfoService.CreateAsync(saveDto);
        }

        /// <summary>
        /// 更新（设备最新信息）
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("update")]
        public async Task UpdateAsync([FromBody] ManuEuqipmentNewestInfoSaveDto saveDto)
        {
             await _manuEuqipmentNewestInfoService.ModifyAsync(saveDto);
        }

        /// <summary>
        /// 删除（设备最新信息）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete")]
        public async Task DeleteAsync([FromBody] long[] ids)
        {
            await _manuEuqipmentNewestInfoService.DeletesAsync(ids);
        }

        /// <summary>
        /// 查询详情（设备最新信息）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ManuEuqipmentNewestInfoDto?> QueryByIdAsync(long id)
        {
            return await _manuEuqipmentNewestInfoService.QueryByIdAsync(id);
        }

        /// <summary>
        /// 分页查询列表（设备最新信息）
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("pagelist")]
        public async Task<PagedInfo<ManuEuqipmentNewestInfoDto>> QueryPagedListAsync([FromQuery] ManuEuqipmentNewestInfoPagedQueryDto pagedQueryDto)
        {
            return await _manuEuqipmentNewestInfoService.GetPagedListAsync(pagedQueryDto);
        }

    }
}