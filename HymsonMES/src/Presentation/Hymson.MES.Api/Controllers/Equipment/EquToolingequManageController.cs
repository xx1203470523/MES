using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;
using Hymson.MES.Services.Dtos.Common;
using Hymson.MES.Services.Dtos.Equipment;
using Hymson.MES.Services.Dtos.Integrated;
using Hymson.MES.Services.Dtos.Process;
using Hymson.MES.Services.Services.Equipment.EquToolingManage;
using Hymson.MES.Services.Services.Process.Procedure;
using Hymson.Web.Framework.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers
{
    /// <summary>
    /// 控制器（工具管理表）
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class EquToolingequManageController : ControllerBase
    {
        /// <summary>
        /// 接口（工具管理表）
        /// </summary>
        private readonly IEquToolingManageService _equToolingManageService;
        private readonly ILogger<EquToolingequManageController> _logger;

        /// <summary>
        /// 构造函数（工具管理表）
        /// </summary>
        /// <param name="equToolingManageService"></param>
        /// <param name="logger"></param>
        public EquToolingequManageController(IEquToolingManageService equToolingManageService, ILogger<EquToolingequManageController> logger)
        {
            _equToolingManageService = equToolingManageService;
            _logger = logger;
        }

        /// <summary>
        /// 分页查询列表（工具管理表）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("list")]
        public async Task<PagedInfo<EquToolingManageViewDto>> QueryPagedProcProcedure([FromQuery] EquToolingManagePagedQueryDto parm)
        {
            return await _equToolingManageService.GetPageListAsync(parm);
        }

        /// <summary>
        /// 查询详情（工具管理表）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<EquToolingManageViewDto> QueryEquToolingManageByIdAsync(long id)
        {
            return await _equToolingManageService.QueryEquToolingManageByIdAsync(id);
        }

        /// <summary>
        /// 新增（工具管理表）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        [LogDescription("工具管理", BusinessType.INSERT)]
        public async Task<long> AddEquToolingManageAsync([FromBody] AddEquToolingManageDto parm)
        {
            return await _equToolingManageService.AddEquToolingManageAsync(parm);
        }


        /// <summary>
        /// 删除（工具管理表）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete")]
        [LogDescription("工具管理", BusinessType.DELETE)]
        public async Task DeleteEquToolingManageAsync([FromBody] long[] ids)
        {
            await _equToolingManageService.DeleteEquToolingManageAsync(ids);
        }

        /// <summary>
        /// 更新（工具管理表）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("update")]
        [LogDescription("工具管理", BusinessType.UPDATE)]
        public async Task ModifyEquToolingManageAsync([FromBody] EquToolingManageModifyDto parm)
        {
            await _equToolingManageService.ModifyEquToolingManageAsync(parm);
        }
    }
}