using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Integrated;
using Hymson.MES.Services.Services.Integrated.IIntegratedService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Extensions;

namespace Hymson.MES.Api.Controllers.Integrated
{
    /// <summary>
    ///  作业表控制器
    /// @author admin
    /// @date 2023-02-21
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class InteJobController : ControllerBase
    {
        private readonly IInteJobService _inteJobService;
        private readonly ILogger<InteJobController> _logger;

        /// <summary>
        /// 作业表控制器
        /// </summary>
        /// <param name="inteJobService"></param>
        /// <param name="logger"></param>
        public InteJobController(IInteJobService inteJobService, ILogger<InteJobController> logger)
        {
            _inteJobService = inteJobService;
            _logger = logger;
        }

        /// <summary>
        /// 分页查询列表
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("pagelist")]
        public async Task<PagedInfo<InteJobDto>> QueryPagedInteJobAsync([FromQuery] InteJobPagedQueryDto parm)
        {
            return await _inteJobService.GetPageListAsync(parm);
        }

        /// <summary>
        /// 查询详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<InteJobDto> QueryInteJobByIdAsync(long id)
        {
            return await _inteJobService.QueryInteJobByIdAsync(id);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        public async Task AddInteJobAsync([FromBody] InteJobCreateDto parm)
        {
            await _inteJobService.CreateInteJobAsync(parm);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("update")]
        public async Task UpdateInteJobAsync([FromBody] InteJobModifyDto parm)
        {
            await _inteJobService.ModifyInteJobAsync(parm);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("delete")]
        public async Task DeleteInteJobAsync(string ids)
        {
            await _inteJobService.DeleteRangInteJobAsync(ids);
        }
    }
}