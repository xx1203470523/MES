/*
 *creator: Karl
 *
 *describe: 设备点检计划    控制器 | 代码由框架生成  
 *builder:  pengxin
 *build datetime: 2024-05-16 02:14:30
 */
using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.EquSpotcheckPlan;
using Hymson.MES.Services.Services.EquSpotcheckPlan;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.EquSpotcheckPlan
{
    /// <summary>
    /// 控制器（设备点检计划）
    /// @author pengxin
    /// @date 2024-05-16 02:14:30
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class EquSpotcheckPlanController : ControllerBase
    {
        /// <summary>
        /// 接口（设备点检计划）
        /// </summary>
        private readonly IEquSpotcheckPlanService _equSpotcheckPlanService;
        private readonly ILogger<EquSpotcheckPlanController> _logger;

        /// <summary>
        /// 构造函数（设备点检计划）
        /// </summary>
        /// <param name="equSpotcheckPlanService"></param>
        public EquSpotcheckPlanController(IEquSpotcheckPlanService equSpotcheckPlanService, ILogger<EquSpotcheckPlanController> logger)
        {
            _equSpotcheckPlanService = equSpotcheckPlanService;
            _logger = logger;
        }

        #region 框架生成方法

        /// <summary>
        /// 分页查询列表（设备点检计划）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("pagelist")]
        public async Task<PagedInfo<EquSpotcheckPlanDto>> QueryPagedEquSpotcheckPlanAsync([FromQuery] EquSpotcheckPlanPagedQueryDto parm)
        {
            return await _equSpotcheckPlanService.GetPagedListAsync(parm);
        }

        /// <summary>
        /// 查询详情（设备点检计划）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<EquSpotcheckPlanDto> QueryEquSpotcheckPlanByIdAsync(long id)
        {
            return await _equSpotcheckPlanService.QueryEquSpotcheckPlanByIdAsync(id);
        }

        /// <summary>
        /// 查询详情（设备点检计划）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("equRelation/{id}")] 
        public async Task<List<QueryEquRelationListDto>> QueryEquRelationListAsync(long id)
        {
            return await _equSpotcheckPlanService.QueryEquRelationListAsync(id); 
        }

        /// <summary>
        /// 添加（设备点检计划）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        public async Task AddEquSpotcheckPlanAsync([FromBody] EquSpotcheckPlanCreateDto parm)
        {
             await _equSpotcheckPlanService.CreateEquSpotcheckPlanAsync(parm);
        }

        /// <summary>
        /// 更新（设备点检计划）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("update")]
        public async Task UpdateEquSpotcheckPlanAsync([FromBody] EquSpotcheckPlanModifyDto parm)
        {
             await _equSpotcheckPlanService.ModifyEquSpotcheckPlanAsync(parm);
        }

        /// <summary>
        /// 删除（设备点检计划）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete")]
        public async Task DeleteEquSpotcheckPlanAsync([FromBody] long[] ids)
        {
            await _equSpotcheckPlanService.DeletesEquSpotcheckPlanAsync(ids);
        }

        #endregion
    }
}