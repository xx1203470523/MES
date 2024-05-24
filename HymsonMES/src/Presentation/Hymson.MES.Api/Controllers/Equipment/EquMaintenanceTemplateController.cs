/*
 *creator: Karl
 *
 *describe: 设备点检模板    控制器 | 代码由框架生成  
 *builder:  pengxin
 *build datetime: 2024-05-13 03:06:41
 */
using Elastic.Clients.Elasticsearch;
using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.EquMaintenanceTemplate;
using Hymson.MES.Services.Services.EquMaintenanceTemplate;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.EquMaintenanceTemplate
{
    /// <summary>
    /// 控制器（设备点检模板）
    /// @author pengxin
    /// @date 2024-05-13 03:06:41
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class EquMaintenanceTemplateController : ControllerBase
    {
        /// <summary>
        /// 接口（设备点检模板）
        /// </summary>
        private readonly IEquMaintenanceTemplateService _EquMaintenanceTemplateService;
        private readonly ILogger<EquMaintenanceTemplateController> _logger;

        /// <summary>
        /// 构造函数（设备点检模板）
        /// </summary>
        /// <param name="EquMaintenanceTemplateService"></param>
        public EquMaintenanceTemplateController(IEquMaintenanceTemplateService EquMaintenanceTemplateService, ILogger<EquMaintenanceTemplateController> logger)
        {
            _EquMaintenanceTemplateService = EquMaintenanceTemplateService;
            _logger = logger;
        }

        #region 框架生成方法

        /// <summary>
        /// 分页查询列表（设备点检模板）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("pagelist")]
        public async Task<PagedInfo<EquMaintenanceTemplateDto>> QueryPagedEquMaintenanceTemplateAsync([FromQuery] EquMaintenanceTemplatePagedQueryDto parm)
        {
            return await _EquMaintenanceTemplateService.GetPagedListAsync(parm);
        }

        /// <summary>
        /// 查询详情（设备点检模板）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<EquMaintenanceTemplateDto> QueryEquMaintenanceTemplateByIdAsync(long id)
        {
            return await _EquMaintenanceTemplateService.QueryEquMaintenanceTemplateByIdAsync(id);
        }

        /// <summary>
        /// 添加（设备点检模板）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        public async Task AddEquMaintenanceTemplateAsync([FromBody] EquMaintenanceTemplateCreateDto parm)
        {
            await _EquMaintenanceTemplateService.CreateEquMaintenanceTemplateAsync(parm);
        }

        /// <summary>
        /// 更新（设备点检模板）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("update")]
        public async Task UpdateEquMaintenanceTemplateAsync([FromBody] EquMaintenanceTemplateModifyDto parm)
        {
            await _EquMaintenanceTemplateService.ModifyEquMaintenanceTemplateAsync(parm);
        }

        /// <summary>
        /// 删除（设备点检模板）
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete")]
        public async Task DeleteEquMaintenanceTemplateAsync([FromBody] EquMaintenanceTemplateDeleteDto param)
        {
            await _EquMaintenanceTemplateService.DeletesEquMaintenanceTemplateAsync(param);
        }


        /// <summary>
        /// 获取模板关联信息（项目）
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("getitem")]
        public async Task<List<GetItemRelationListDto>> QueryItemRelationListAsync([FromQuery] GetEquMaintenanceTemplateItemRelationDto param)
        {
            return await _EquMaintenanceTemplateService.QueryItemRelationListAsync(param);
        }

        /// <summary>
        /// 获取模板关联信息（设备组）
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("getgroup")]
        public async Task<List<QueryEquipmentGroupRelationListDto>> QueryEquipmentGroupRelationListAsync([FromQuery] GetEquMaintenanceTemplateItemRelationDto param)
        {
            return await _EquMaintenanceTemplateService.QueryEquipmentGroupRelationListAsync(param);
        }
        #endregion
    }
}