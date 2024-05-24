/*
 *creator: Karl
 *
 *describe: 设备点检计划    控制器 | 代码由框架生成  
 *builder:  pengxin
 *build datetime: 2024-05-16 02:14:30
 */
using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.EquMaintenancePlan;
using Hymson.MES.Services.Services.EquMaintenancePlan;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.EquMaintenancePlan
{
    /// <summary>
    /// 控制器（设备点检计划）
    /// @author pengxin
    /// @date 2024-05-16 02:14:30
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class EquMaintenancePlanController : ControllerBase
    {
        /// <summary>
        /// 接口（设备点检计划）
        /// </summary>
        private readonly IEquMaintenancePlanService _EquMaintenancePlanService;
        private readonly ILogger<EquMaintenancePlanController> _logger;

        /// <summary>
        /// 构造函数（设备点检计划）
        /// </summary>
        /// <param name="EquMaintenancePlanService"></param>
        public EquMaintenancePlanController(IEquMaintenancePlanService EquMaintenancePlanService, ILogger<EquMaintenancePlanController> logger)
        {
            _EquMaintenancePlanService = EquMaintenancePlanService;
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
        public async Task<PagedInfo<EquMaintenancePlanDto>> QueryPagedEquMaintenancePlanAsync([FromQuery] EquMaintenancePlanPagedQueryDto parm)
        {
            return await _EquMaintenancePlanService.GetPagedListAsync(parm);
        }

        /// <summary>
        /// 查询详情（设备点检计划）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<EquMaintenancePlanDto> QueryEquMaintenancePlanByIdAsync(long id)
        {
            return await _EquMaintenancePlanService.QueryEquMaintenancePlanByIdAsync(id);
        }

        /// <summary>
        /// 查询详情（设备点检计划）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("equRelation/{id}")]
        public async Task<List<QueryEquRelationListDto>> QueryEquRelationListAsync(long id)
        {
            return await _EquMaintenancePlanService.QueryEquRelationListAsync(id);
        }

        /// <summary>
        /// 添加（设备点检计划）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        public async Task AddEquMaintenancePlanAsync([FromBody] EquMaintenancePlanCreateDto parm)
        {
            await _EquMaintenancePlanService.CreateEquMaintenancePlanAsync(parm);
        }

        /// <summary>
        /// 更新（设备点检计划）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("update")]
        public async Task UpdateEquMaintenancePlanAsync([FromBody] EquMaintenancePlanModifyDto parm)
        {
            await _EquMaintenancePlanService.ModifyEquMaintenancePlanAsync(parm);
        }

        /// <summary>
        /// 删除（设备点检计划）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete")]
        public async Task DeleteEquMaintenancePlanAsync([FromBody] DeletesDto param) 
        {
            await _EquMaintenancePlanService.DeletesEquMaintenancePlanAsync(param);
        }

        /// <summary>
        /// 添加（生成点检任务）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("generate")]
        public async Task GenerateEquMaintenanceTaskAsync([FromBody] GenerateDto parm)
        {
            await _EquMaintenancePlanService.GenerateEquMaintenanceTaskCoreAsync(parm); 
        }
        #endregion
    }
}