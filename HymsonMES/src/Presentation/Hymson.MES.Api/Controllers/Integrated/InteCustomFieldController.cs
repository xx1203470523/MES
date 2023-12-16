using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Integrated;
using Hymson.MES.Services.Services.Integrated;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Integrated
{
    /// <summary>
    /// 控制器（自定义字段）
    /// @author Karl
    /// @date 2023-12-15 04:30:52
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class InteCustomFieldController : ControllerBase
    {
        /// <summary>
        /// 日志
        /// </summary>
        private readonly ILogger<InteCustomFieldController> _logger;
        /// <summary>
        /// 服务接口（自定义字段）
        /// </summary>
        private readonly IInteCustomFieldService _inteCustomFieldService;


        /// <summary>
        /// 构造函数（自定义字段）
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="inteCustomFieldService"></param>
        public InteCustomFieldController(ILogger<InteCustomFieldController> logger, IInteCustomFieldService inteCustomFieldService)
        {
            _logger = logger;
            _inteCustomFieldService = inteCustomFieldService;
        }

        /// <summary>
        /// 添加或更新（自定义字段）
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("addOrUpdate")]
        public async Task AddOrUpdateAsync([FromBody] InteCustomFieldSaveDto saveDto)
        {
             await _inteCustomFieldService.AddOrUpdateAsync(saveDto);
        }
    }
}