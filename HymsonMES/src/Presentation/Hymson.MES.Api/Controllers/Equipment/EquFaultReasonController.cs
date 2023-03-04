/*
 *creator: pengxin
 *
 *describe: 设备故障原因表    控制器 | 代码由框架生成  
 *builder:  pengxin
 *build datetime: 2023-02-28 15:15:20
 */
using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Equipment;
using Hymson.MES.Services.Services.Equipment;
using Hymson.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Extensions; 

namespace Hymson.MES.Api.Controllers.Equipment
{
    /// <summary>
    /// 控制器（设备故障原因表）
    /// @author pengxin
    /// @date 2023-02-28 15:15:20
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class EquFaultReasonController : ControllerBase
    {
        /// <summary>
        /// 接口（设备故障原因表）
        /// </summary>
        private readonly IEquFaultReasonService _EquFaultReasonService;
        private readonly ILogger<EquFaultReasonController> _logger;

        /// <summary>
        /// 构造函数（设备故障原因表）
        /// </summary>
        /// <param name="EquFaultReasonService"></param>
        public EquFaultReasonController(IEquFaultReasonService EquFaultReasonService, ILogger<EquFaultReasonController> logger)
        {
            _EquFaultReasonService = EquFaultReasonService;
            _logger = logger;
        }

        /// <summary>
        /// 分页查询列表（设备故障原因表）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("pagelist")]
        public async Task<PagedInfo<EquFaultReasonDto>> QueryPagedEquFaultReasonAsync(EquFaultReasonPagedQueryDto parm)
        {
            return await _EquFaultReasonService.GetPageListAsync(parm);
        }

        /// <summary>
        /// 查询详情（设备故障原因表）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<EquFaultReasonDto> QueryEquFaultReasonByIdAsync(long id)
        {
            return await _EquFaultReasonService.QueryEquFaultReasonByIdAsync(id);
        }

        /// <summary>
        /// 添加（设备故障原因表）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        public async Task<int> AddEquFaultReasonAsync([FromBody] EquFaultReasonCreateDto parm)
        {
            return await _EquFaultReasonService.CreateEquFaultReasonAsync(parm);
        }

        /// <summary>
        /// 更新（设备故障原因表）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("update")]
        public async Task UpdateEquFaultReasonAsync([FromBody] EquFaultReasonModifyDto parm)
        {
            await _EquFaultReasonService.ModifyEquFaultReasonAsync(parm);
        }

        /// <summary>
        /// 删除（设备故障原因表）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("delete")]
        public async Task<int> DeleteEquFaultReasonAsync([FromBody] long[] ids)
        {
            //long[] idsArr = StringExtension.SpitLongArrary(ids);
            return await _EquFaultReasonService.DeletesEquFaultReasonAsync(ids);
        }

    }
}