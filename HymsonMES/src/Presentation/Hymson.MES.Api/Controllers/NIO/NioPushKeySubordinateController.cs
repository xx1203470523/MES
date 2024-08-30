using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.NIO;
using Hymson.MES.Services.Services.NIO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.NIO
{
    /// <summary>
    /// 控制器（物料及其关键下级件信息表）
    /// @author User
    /// @date 2024-08-30 03:48:41
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class NioPushKeySubordinateController : ControllerBase
    {
        /// <summary>
        /// 日志
        /// </summary>
        private readonly ILogger<NioPushKeySubordinateController> _logger;
        /// <summary>
        /// 服务接口（物料及其关键下级件信息表）
        /// </summary>
        private readonly INioPushKeySubordinateService _nioPushKeySubordinateService;


        /// <summary>
        /// 构造函数（物料及其关键下级件信息表）
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="nioPushKeySubordinateService"></param>
        public NioPushKeySubordinateController(ILogger<NioPushKeySubordinateController> logger, INioPushKeySubordinateService nioPushKeySubordinateService)
        {
            _logger = logger;
            _nioPushKeySubordinateService = nioPushKeySubordinateService;
        }

        /// <summary>
        /// 添加（物料及其关键下级件信息表）
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        public async Task AddAsync([FromBody] NioPushKeySubordinateSaveDto saveDto)
        {
             await _nioPushKeySubordinateService.CreateAsync(saveDto);
        }

        /// <summary>
        /// 更新（物料及其关键下级件信息表）
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("update")]
        public async Task UpdateAsync([FromBody] NioPushKeySubordinateSaveDto saveDto)
        {
             await _nioPushKeySubordinateService.ModifyAsync(saveDto);
        }

        /// <summary>
        /// 删除（物料及其关键下级件信息表）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete")]
        public async Task DeleteAsync([FromBody] long[] ids)
        {
            await _nioPushKeySubordinateService.DeletesAsync(ids);
        }

        /// <summary>
        /// 查询详情（物料及其关键下级件信息表）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<NioPushKeySubordinateDto?> QueryByIdAsync(long id)
        {
            return await _nioPushKeySubordinateService.QueryByIdAsync(id);
        }

        /// <summary>
        /// 分页查询列表（物料及其关键下级件信息表）
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("pagelist")]
        public async Task<PagedInfo<NioPushKeySubordinateDto>> QueryPagedListAsync([FromQuery] NioPushKeySubordinatePagedQueryDto pagedQueryDto)
        {
            return await _nioPushKeySubordinateService.GetPagedListAsync(pagedQueryDto);
        }

    }
}