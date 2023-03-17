using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Integrated;
using Hymson.MES.Services.Services.Integrated.IIntegratedService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Extensions;

namespace Hymson.MES.Api.Controllers.Integrated
{
    /// <summary>
    ///  工作中心控制器
    /// @author admin
    /// @date 2023-02-21
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class InteWorkCenterController : ControllerBase
    {
        private readonly IInteWorkCenterService _inteWorkCenterService;
        private readonly ILogger<InteWorkCenterController> _logger;

        /// <summary>
        /// 作业表控制器
        /// </summary>
        /// <param name="inteWorkCenterService"></param>
        /// <param name="logger"></param>
        public InteWorkCenterController(IInteWorkCenterService inteWorkCenterService, ILogger<InteWorkCenterController> logger)
        {
            _inteWorkCenterService = inteWorkCenterService;
            _logger = logger;
        }

        /// <summary>
        /// 分页查询列表
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("pagelist")]
        public async Task<PagedInfo<InteWorkCenterDto>> QueryPagedInteWorkCenterAsync([FromQuery] InteWorkCenterPagedQueryDto param)
        {
            return await _inteWorkCenterService.GetPageListAsync(param);
        }

        /// <summary>
        /// 查询详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<InteWorkCenterDto> QueryInteWorkCenterByIdAsync(long id)
        {
            return await _inteWorkCenterService.QueryInteWorkCenterByIdAsync(id);
        }

        /// <summary>
        /// 查询关联资源
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}/resource")]
        public async Task<List<InteWorkCenterResourceRelationDto>> QueryInteWorkCenterResourceRelatioByIdAsync(long id)
        {
            return await _inteWorkCenterService.GetInteWorkCenterResourceRelatioByIdAsync(id);
        }

        /// <summary>
        /// 查询关联工作中心
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}/workCenter")]
        public async Task<List<InteWorkCenterRelationDto>> QueryInteWorkCenterRelationByIdAsync(long id)
        {
            return await _inteWorkCenterService.GetInteWorkCenterRelationByIdAsync(id);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task AddInteWorkCenterAsync([FromBody] InteWorkCenterCreateDto param)
        {
            await _inteWorkCenterService.CreateInteWorkCenterAsync(param);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task UpdateInteWorkCenterAsync([FromBody] InteWorkCenterModifyDto param)
        {
            await _inteWorkCenterService.ModifyInteWorkCenterAsync(param);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task DeleteInteWorkCenterAsync(long[] ids)
        {
            await _inteWorkCenterService.DeleteRangInteWorkCenterAsync(ids);
        }
    }
}