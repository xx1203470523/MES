/*
 *creator: Karl
 *
 *describe: 编码规则    控制器 | 代码由框架生成  
 *builder:  Karl
 *build datetime: 2023-03-17 05:02:26
 */
using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Integrated;
using Hymson.MES.Services.Services.Integrated;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Extensions;

namespace Hymson.MES.Api.Controllers.Integrated
{
    /// <summary>
    /// 控制器（编码规则）
    /// @author Karl
    /// @date 2023-03-17 05:02:26
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class InteCodeRulesController : ControllerBase
    {
        /// <summary>
        /// 接口（编码规则）
        /// </summary>
        private readonly IInteCodeRulesService _inteCodeRulesService;
        private readonly ILogger<InteCodeRulesController> _logger;

        /// <summary>
        /// 构造函数（编码规则）
        /// </summary>
        /// <param name="inteCodeRulesService"></param>
        public InteCodeRulesController(IInteCodeRulesService inteCodeRulesService, ILogger<InteCodeRulesController> logger)
        {
            _inteCodeRulesService = inteCodeRulesService;
            _logger = logger;
        }

        /// <summary>
        /// 分页查询列表（编码规则）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("pagelist")]
        public async Task<PagedInfo<InteCodeRulesDto>> QueryPagedInteCodeRulesAsync([FromQuery] InteCodeRulesPagedQueryDto parm)
        {
            return await _inteCodeRulesService.GetPageListAsync(parm);
        }

        /// <summary>
        /// 查询详情（编码规则）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<InteCodeRulesDto> QueryInteCodeRulesByIdAsync(long id)
        {
            return await _inteCodeRulesService.QueryInteCodeRulesByIdAsync(id);
        }

        /// <summary>
        /// 添加（编码规则）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        public async Task AddInteCodeRulesAsync([FromBody] InteCodeRulesCreateDto parm)
        {
             await _inteCodeRulesService.CreateInteCodeRulesAsync(parm);
        }

        /// <summary>
        /// 更新（编码规则）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("update")]
        public async Task UpdateInteCodeRulesAsync([FromBody] InteCodeRulesModifyDto parm)
        {
             await _inteCodeRulesService.ModifyInteCodeRulesAsync(parm);
        }

        /// <summary>
        /// 删除（编码规则）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete")]
        public async Task DeleteInteCodeRulesAsync([FromBody] long[] ids)
        {
            //long[] idsArr = StringExtension.SpitLongArrary(ids);
            await _inteCodeRulesService.DeletesInteCodeRulesAsync(ids);
        }

    }
}