/*
 *creator: Karl
 *
 *describe: 客户维护    控制器 | 代码由框架生成  
 *builder:  Karl
 *build datetime: 2023-07-11 09:33:26
 */
using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Integrated;
using Hymson.MES.Services.Services.Integrated;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Integrated
{
    /// <summary>
    /// 控制器（客户维护）
    /// @author Karl
    /// @date 2023-07-11 09:33:26
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class InteCustomController : ControllerBase
    {
        /// <summary>
        /// 接口（客户维护）
        /// </summary>
        private readonly IInteCustomService _inteCustomService;
        private readonly ILogger<InteCustomController> _logger;

        /// <summary>
        /// 构造函数（客户维护）
        /// </summary>
        /// <param name="inteCustomService"></param>
        public InteCustomController(IInteCustomService inteCustomService, ILogger<InteCustomController> logger)
        {
            _inteCustomService = inteCustomService;
            _logger = logger;
        }

        #region 框架生成方法

        /// <summary>
        /// 分页查询列表（客户维护）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("pagelist")]
        public async Task<PagedInfo<InteCustomDto>> QueryPagedInteCustomAsync([FromQuery] InteCustomPagedQueryDto parm)
        {
            return await _inteCustomService.GetPagedListAsync(parm);
        }

        /// <summary>
        /// 查询详情（客户维护）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<InteCustomDto> QueryInteCustomByIdAsync(long id)
        {
            return await _inteCustomService.QueryInteCustomByIdAsync(id);
        }

        /// <summary>
        /// 添加（客户维护）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        public async Task AddInteCustomAsync([FromBody] InteCustomCreateDto parm)
        {
             await _inteCustomService.CreateInteCustomAsync(parm);
        }

        /// <summary>
        /// 更新（客户维护）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("update")]
        public async Task UpdateInteCustomAsync([FromBody] InteCustomModifyDto parm)
        {
             await _inteCustomService.ModifyInteCustomAsync(parm);
        }

        /// <summary>
        /// 删除（客户维护）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete")]
        public async Task DeleteInteCustomAsync([FromBody] long[] ids)
        {
            await _inteCustomService.DeletesInteCustomAsync(ids);
        }

        #endregion
    }
}