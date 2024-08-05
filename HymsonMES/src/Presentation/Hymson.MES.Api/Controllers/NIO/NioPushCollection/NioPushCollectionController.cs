using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.NioPushCollection;
using Hymson.MES.Services.Services.NioPushCollection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.NioPushCollection
{
    /// <summary>
    /// 控制器（NIO推送参数）
    /// @author Yxx
    /// @date 2024-08-05 04:09:48
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class NioPushCollectionController : ControllerBase
    {
        /// <summary>
        /// 日志
        /// </summary>
        private readonly ILogger<NioPushCollectionController> _logger;
        /// <summary>
        /// 服务接口（NIO推送参数）
        /// </summary>
        private readonly INioPushCollectionService _nioPushCollectionService;


        /// <summary>
        /// 构造函数（NIO推送参数）
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="nioPushCollectionService"></param>
        public NioPushCollectionController(ILogger<NioPushCollectionController> logger, INioPushCollectionService nioPushCollectionService)
        {
            _logger = logger;
            _nioPushCollectionService = nioPushCollectionService;
        }

        /// <summary>
        /// 添加（NIO推送参数）
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        public async Task AddAsync([FromBody] NioPushCollectionSaveDto saveDto)
        {
             await _nioPushCollectionService.CreateAsync(saveDto);
        }

        /// <summary>
        /// 更新（NIO推送参数）
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("update")]
        public async Task UpdateAsync([FromBody] NioPushCollectionSaveDto saveDto)
        {
             await _nioPushCollectionService.ModifyAsync(saveDto);
        }

        /// <summary>
        /// 删除（NIO推送参数）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete")]
        public async Task DeleteAsync([FromBody] long[] ids)
        {
            await _nioPushCollectionService.DeletesAsync(ids);
        }

        /// <summary>
        /// 查询详情（NIO推送参数）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<NioPushCollectionDto?> QueryByIdAsync(long id)
        {
            return await _nioPushCollectionService.QueryByIdAsync(id);
        }

        /// <summary>
        /// 分页查询列表（NIO推送参数）
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("pagelist")]
        public async Task<PagedInfo<NioPushCollectionDto>> QueryPagedListAsync([FromQuery] NioPushCollectionPagedQueryDto pagedQueryDto)
        {
            return await _nioPushCollectionService.GetPagedListAsync(pagedQueryDto);
        }

    }
}