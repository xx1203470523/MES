/*
 *creator: Karl
 *
 *describe: 标准参数关联类型表    控制器 | 代码由框架生成  
 *builder:  Karl
 *build datetime: 2023-02-15 03:53:38
 */
using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Process;
using Hymson.MES.Services.Services.Process;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Extensions;

namespace Hymson.MES.Api.Controllers.Process
{
    /// <summary>
    /// 控制器（标准参数关联类型表）
    /// @author Karl
    /// @date 2023-02-15 03:53:38
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ProcParameterLinkTypeController : ControllerBase
    {
        /// <summary>
        /// 接口（标准参数关联类型表）
        /// </summary>
        private readonly IProcParameterLinkTypeService _procParameterLinkTypeService;
        private readonly ILogger<ProcParameterLinkTypeController> _logger;

        /// <summary>
        /// 构造函数（标准参数关联类型表）
        /// </summary>
        /// <param name="procParameterLinkTypeService"></param>
        public ProcParameterLinkTypeController(IProcParameterLinkTypeService procParameterLinkTypeService, ILogger<ProcParameterLinkTypeController> logger)
        {
            _procParameterLinkTypeService = procParameterLinkTypeService;
            _logger = logger;
        }

        /// <summary>
        /// 分页查询列表（标准参数关联类型表）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("pagelist")]
        public async Task<PagedInfo<ProcParameterLinkTypeViewDto>> QueryPagedProcParameterLinkTypeAsync(ProcParameterLinkTypePagedQueryDto parm)
        {
            return await _procParameterLinkTypeService.GetPageListAsync(parm);
        }

        /// <summary>
        /// 分页查询详情（设备/产品参数）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("detail")]
        public async Task<PagedInfo<ProcParameterLinkTypeViewDto>> QueryPagedProcParameterLinkTypeByTypeAsync(ProcParameterDetailPagerQueryDto parm)
        {
            return await _procParameterLinkTypeService.QueryPagedProcParameterLinkTypeByTypeAsync(parm);
        }

        ///// <summary>
        ///// 查询详情（标准参数关联类型表）
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //[HttpGet("{id}")]
        //public async Task<ProcParameterLinkTypeDto> QueryProcParameterLinkTypeByIdAsync(long id)
        //{
        //    return await _procParameterLinkTypeService.QueryProcParameterLinkTypeByIdAsync(id);
        //}

        /// <summary>
        /// 添加（标准参数关联类型表）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        public async Task AddProcParameterLinkTypeAsync([FromBody] ProcParameterLinkTypeCreateDto parm)
        {
             await _procParameterLinkTypeService.CreateProcParameterLinkTypeAsync(parm);
        }

        /// <summary>
        /// 更新（标准参数关联类型表）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("update")]
        public async Task UpdateProcParameterLinkTypeAsync([FromBody] ProcParameterLinkTypeModifyDto parm)
        {
             await _procParameterLinkTypeService.ModifyProcParameterLinkTypeAsync(parm);
        }

        /// <summary>
        /// 删除（标准参数关联类型表）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("delete")]
        public async Task<int> DeleteProcParameterLinkTypeAsync(long[] ids)
        {
            //long[] idsArr = StringExtension.SpitLongArrary(ids);
            return await _procParameterLinkTypeService.DeletesProcParameterLinkTypeAsync(ids);
        }

    }
}