/*
 *creator: Karl
 *
 *describe: 降级规则    控制器 | 代码由框架生成  
 *builder:  Karl
 *build datetime: 2023-08-07 02:00:57
 */
using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Manufacture;
using Hymson.MES.Services.Services.Manufacture;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Manufacture
{
    /// <summary>
    /// 控制器（降级规则）
    /// @author Karl
    /// @date 2023-08-07 02:00:57
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ManuDowngradingRuleController : ControllerBase
    {
        /// <summary>
        /// 接口（降级规则）
        /// </summary>
        private readonly IManuDowngradingRuleService _manuDowngradingRuleService;
        private readonly ILogger<ManuDowngradingRuleController> _logger;

        /// <summary>
        /// 构造函数（降级规则）
        /// </summary>
        /// <param name="manuDowngradingRuleService"></param>
        public ManuDowngradingRuleController(IManuDowngradingRuleService manuDowngradingRuleService, ILogger<ManuDowngradingRuleController> logger)
        {
            _manuDowngradingRuleService = manuDowngradingRuleService;
            _logger = logger;
        }

        #region 框架生成方法

        /// <summary>
        /// 分页查询列表（降级规则）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("pagelist")]
        public async Task<PagedInfo<ManuDowngradingRuleDto>> QueryPagedManuDowngradingRuleAsync([FromQuery] ManuDowngradingRulePagedQueryDto parm)
        {
            return await _manuDowngradingRuleService.GetPagedListAsync(parm);
        }

        /// <summary>
        /// 查询详情（降级规则）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ManuDowngradingRuleDto> QueryManuDowngradingRuleByIdAsync(long id)
        {
            return await _manuDowngradingRuleService.QueryManuDowngradingRuleByIdAsync(id);
        }

        /// <summary>
        /// 添加（降级规则）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        public async Task AddManuDowngradingRuleAsync([FromBody] ManuDowngradingRuleCreateDto parm)
        {
             await _manuDowngradingRuleService.CreateManuDowngradingRuleAsync(parm);
        }

        /// <summary>
        /// 更新（降级规则）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("update")]
        public async Task UpdateManuDowngradingRuleAsync([FromBody] ManuDowngradingRuleModifyDto parm)
        {
             await _manuDowngradingRuleService.ModifyManuDowngradingRuleAsync(parm);
        }

        /// <summary>
        /// 删除（降级规则）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete")]
        public async Task DeleteManuDowngradingRuleAsync([FromBody] long[] ids)
        {
            await _manuDowngradingRuleService.DeletesManuDowngradingRuleAsync(ids);
        }

        #endregion
    }
}