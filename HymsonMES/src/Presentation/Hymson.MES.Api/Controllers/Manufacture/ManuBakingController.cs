/*
 *creator: Karl
 *
 *describe: 烘烤工序    控制器 | 代码由框架生成  
 *builder:  wxk
 *build datetime: 2023-07-28 05:41:13
 */
using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Manufacture;
using Hymson.MES.Services.Services.Manufacture;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Manufacture
{
    /// <summary>
    /// 控制器（烘烤工序）
    /// @author wxk
    /// @date 2023-07-28 05:41:13
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ManuBakingController : ControllerBase
    {
        /// <summary>
        /// 接口（烘烤工序）
        /// </summary>
        private readonly IManuBakingService _manuBakingService;
        private readonly ILogger<ManuBakingController> _logger;

        /// <summary>
        /// 构造函数（烘烤工序）
        /// </summary>
        /// <param name="manuBakingService"></param>
        public ManuBakingController(IManuBakingService manuBakingService, ILogger<ManuBakingController> logger)
        {
            _manuBakingService = manuBakingService;
            _logger = logger;
        }

        #region 框架生成方法

        /// <summary>
        /// 分页查询列表（烘烤工序）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("pagelist")]
        public async Task<PagedInfo<ManuBakingDto>> QueryPagedManuBakingAsync([FromQuery] ManuBakingPagedQueryDto parm)
        {
            return await _manuBakingService.GetPagedListAsync(parm);
        }

        /// <summary>
        /// 查询详情（烘烤工序）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ManuBakingDto> QueryManuBakingByIdAsync(long id)
        {
            return await _manuBakingService.QueryManuBakingByIdAsync(id);
        }

        /// <summary>
        /// 添加（烘烤工序）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        public async Task AddManuBakingAsync([FromBody] ManuBakingCreateDto parm)
        {
             await _manuBakingService.CreateManuBakingAsync(parm);
        }

        /// <summary>
        /// 更新（烘烤工序）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("update")]
        public async Task UpdateManuBakingAsync([FromBody] ManuBakingModifyDto parm)
        {
             await _manuBakingService.ModifyManuBakingAsync(parm);
        }

        /// <summary>
        /// 删除（烘烤工序）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete")]
        public async Task DeleteManuBakingAsync([FromBody] long[] ids)
        {
            await _manuBakingService.DeletesManuBakingAsync(ids);
        }

        #endregion
    }
}