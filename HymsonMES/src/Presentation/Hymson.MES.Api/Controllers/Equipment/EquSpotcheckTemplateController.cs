/*
 *creator: Karl
 *
 *describe: 设备点检模板    控制器 | 代码由框架生成  
 *builder:  pengxin
 *build datetime: 2024-05-13 03:06:41
 */
using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.EquSpotcheckTemplate;
using Hymson.MES.Services.Services.EquSpotcheckTemplate;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.EquSpotcheckTemplate
{
    /// <summary>
    /// 控制器（设备点检模板）
    /// @author pengxin
    /// @date 2024-05-13 03:06:41
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class EquSpotcheckTemplateController : ControllerBase
    {
        /// <summary>
        /// 接口（设备点检模板）
        /// </summary>
        private readonly IEquSpotcheckTemplateService _equSpotcheckTemplateService;
        private readonly ILogger<EquSpotcheckTemplateController> _logger;

        /// <summary>
        /// 构造函数（设备点检模板）
        /// </summary>
        /// <param name="equSpotcheckTemplateService"></param>
        public EquSpotcheckTemplateController(IEquSpotcheckTemplateService equSpotcheckTemplateService, ILogger<EquSpotcheckTemplateController> logger)
        {
            _equSpotcheckTemplateService = equSpotcheckTemplateService;
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
        public async Task<PagedInfo<EquSpotcheckTemplateDto>> QueryPagedEquSpotcheckTemplateAsync([FromQuery] EquSpotcheckTemplatePagedQueryDto parm)
        {
            return await _equSpotcheckTemplateService.GetPagedListAsync(parm);
        }

        /// <summary>
        /// 查询详情（设备点检模板）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<EquSpotcheckTemplateDto> QueryEquSpotcheckTemplateByIdAsync(long id)
        {
            return await _equSpotcheckTemplateService.QueryEquSpotcheckTemplateByIdAsync(id);
        }

        /// <summary>
        /// 添加（设备点检模板）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        public async Task AddEquSpotcheckTemplateAsync([FromBody] EquSpotcheckTemplateCreateDto parm)
        {
            await _equSpotcheckTemplateService.CreateEquSpotcheckTemplateAsync(parm);
        }

        /// <summary>
        /// 更新（设备点检模板）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("update")]
        public async Task UpdateEquSpotcheckTemplateAsync([FromBody] EquSpotcheckTemplateModifyDto parm)
        {
            await _equSpotcheckTemplateService.ModifyEquSpotcheckTemplateAsync(parm);
        }

        /// <summary>
        /// 删除（设备点检模板）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete")]
        public async Task DeleteEquSpotcheckTemplateAsync([FromBody] long[] ids)
        {
            await _equSpotcheckTemplateService.DeletesEquSpotcheckTemplateAsync(ids);
        }

        #endregion
    }
}