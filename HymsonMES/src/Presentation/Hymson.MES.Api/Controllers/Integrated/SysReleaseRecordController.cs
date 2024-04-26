/*
 *creator: Karl
 *
 *describe: 发布记录表    控制器 | 代码由框架生成  
 *builder:  pengxin
 *build datetime: 2023-12-19 10:03:09
 */
using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Integrated;
using Hymson.MES.Services.Services.Integrated;
using Hymson.Web.Framework.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Integrated
{
    /// <summary>
    /// 控制器（发布记录表）
    /// @author pengxin
    /// @date 2023-12-19 10:03:09
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class SysReleaseRecordController : ControllerBase
    {
        /// <summary>
        /// 接口（发布记录表）
        /// </summary>
        private readonly ISysReleaseRecordService _sysReleaseRecordService;
        private readonly ILogger<SysReleaseRecordController> _logger;

        /// <summary>
        /// 构造函数（发布记录表）
        /// </summary>
        /// <param name="sysReleaseRecordService"></param>
        public SysReleaseRecordController(ISysReleaseRecordService sysReleaseRecordService, ILogger<SysReleaseRecordController> logger)
        {
            _sysReleaseRecordService = sysReleaseRecordService;
            _logger = logger;
        }

        #region 框架生成方法

        /// <summary>
        /// 分页查询列表（发布记录表）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("pagelist")]
        public async Task<PagedInfo<SysReleaseRecordDto>> QueryPagedSysReleaseRecordAsync([FromQuery] SysReleaseRecordPagedQueryDto parm)
        {
            return await _sysReleaseRecordService.GetPagedListAsync(parm);
        }

        /// <summary>
        /// 查询详情（发布记录表）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<SysReleaseRecordDto> QuerySysReleaseRecordByIdAsync(long id)
        {
            return await _sysReleaseRecordService.QuerySysReleaseRecordByIdAsync(id);
        }

        /// <summary>
        /// 添加（发布记录表）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        [LogDescription("发布记录表", BusinessType.INSERT)]
        public async Task AddSysReleaseRecordAsync([FromBody] SysReleaseRecordCreateDto parm)
        {
            await _sysReleaseRecordService.CreateSysReleaseRecordAsync(parm);
        }

        /// <summary>
        /// 更新（发布记录表）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("update")]
        [LogDescription("发布记录表", BusinessType.UPDATE)]
        public async Task UpdateSysReleaseRecordAsync([FromBody] SysReleaseRecordModifyDto parm)
        {
            await _sysReleaseRecordService.ModifySysReleaseRecordAsync(parm);
        }

        /// <summary>
        /// 更新（发布记录表）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("updateStatus")]
        [LogDescription("发布记录表", BusinessType.UPDATE)]
        public async Task UpdateSysReleaseRecordStatusAsync([FromBody] SysReleaseRecordModifyDto parm)
        {
            await _sysReleaseRecordService.ModifySysReleaseRecordStatusAsync(parm);
        }

        /// <summary>
        /// 删除（发布记录表）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete")]
        [LogDescription("发布记录表", BusinessType.DELETE)]
        public async Task DeleteSysReleaseRecordAsync([FromBody] long[] ids)
        {
            await _sysReleaseRecordService.DeletesSysReleaseRecordAsync(ids);
        }

        #endregion
    }
}