using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Integrated;
using Hymson.MES.Services.Services.Integrated;
using Hymson.Web.Framework.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Integrated
{
    /// <summary>
    /// 控制器（编码规则）
    /// </summary>
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
        /// <param name="logger"></param>
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
        //[PermissionDescription("inte:codeRules:list")]
        public async Task<PagedInfo<InteCodeRulesPageViewDto>> QueryPagedInteCodeRulesAsync([FromQuery] InteCodeRulesPagedQueryDto parm)
        {
            return await _inteCodeRulesService.GetPageListAsync(parm);
        }

        /// <summary>
        /// 查询详情（编码规则）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<InteCodeRulesDetailViewDto> QueryInteCodeRulesByIdAsync(long id)
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
        [LogDescription("编码规则", BusinessType.INSERT)]
        [PermissionDescription("inte:codeRules:insert")]
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
        [LogDescription("编码规则", BusinessType.UPDATE)]
        [PermissionDescription("inte:codeRules:update")]
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
        [LogDescription("编码规则", BusinessType.DELETE)]
        [PermissionDescription("inte:codeRules:delete")]
        public async Task DeleteInteCodeRulesAsync([FromBody] long[] ids)
        {
            await _inteCodeRulesService.DeletesInteCodeRulesAsync(ids);
        }

    }
}