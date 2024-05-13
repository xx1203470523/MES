using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Equipment;
using Hymson.MES.Services.Services.Equipment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Equipment
{
    /// <summary>
    /// 控制器（设备点检项目）
    /// @author User
    /// @date 2024-05-13 02:57:55
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class EquSpotcheckItemController : ControllerBase
    {
        /// <summary>
        /// 日志
        /// </summary>
        private readonly ILogger<EquSpotcheckItemController> _logger;
        /// <summary>
        /// 服务接口（设备点检项目）
        /// </summary>
        private readonly IEquSpotcheckItemService _equSpotcheckItemService;


        /// <summary>
        /// 构造函数（设备点检项目）
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="equSpotcheckItemService"></param>
        public EquSpotcheckItemController(ILogger<EquSpotcheckItemController> logger, IEquSpotcheckItemService equSpotcheckItemService)
        {
            _logger = logger;
            _equSpotcheckItemService = equSpotcheckItemService;
        }

        /// <summary>
        /// 添加（设备点检项目）
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        public async Task AddAsync([FromBody] EquSpotcheckItemSaveDto saveDto)
        {
             await _equSpotcheckItemService.CreateAsync(saveDto);
        }

        /// <summary>
        /// 更新（设备点检项目）
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("update")]
        public async Task UpdateAsync([FromBody] EquSpotcheckItemSaveDto saveDto)
        {
             await _equSpotcheckItemService.ModifyAsync(saveDto);
        }

        /// <summary>
        /// 删除（设备点检项目）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete")]
        public async Task DeleteAsync([FromBody] long[] ids)
        {
            await _equSpotcheckItemService.DeletesAsync(ids);
        }

        /// <summary>
        /// 查询详情（设备点检项目）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<EquSpotcheckItemDto?> QueryByIdAsync(long id)
        {
            return await _equSpotcheckItemService.QueryByIdAsync(id);
        }

        /// <summary>
        /// 分页查询列表（设备点检项目）
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("pagelist")]
        public async Task<PagedInfo<EquSpotcheckItemDto>> QueryPagedListAsync([FromQuery] EquSpotcheckItemPagedQueryDto pagedQueryDto)
        {
            return await _equSpotcheckItemService.GetPagedListAsync(pagedQueryDto);
        }

    }
}