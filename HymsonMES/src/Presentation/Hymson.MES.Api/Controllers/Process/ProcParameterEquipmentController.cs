using Hymson.MES.Services.Dtos.Process;
using Hymson.MES.Services.Services.Process;
using Hymson.Web.Framework.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Process
{
    /// <summary>
    /// 控制器（设备参数）
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ProcParameterEquipmentController : ControllerBase
    {
        /// <summary>
        /// 接口（标准参数关联类型表）
        /// </summary>
        private readonly IProcParameterLinkTypeService _procParameterLinkTypeService;
        private readonly ILogger<ProcParameterLinkTypeController> _logger;

        /// <summary>
        /// 构造函数（设备参数）
        /// </summary>
        /// <param name="procParameterLinkTypeService"></param>
        /// <param name="logger"></param>
        public ProcParameterEquipmentController(IProcParameterLinkTypeService procParameterLinkTypeService, ILogger<ProcParameterLinkTypeController> logger)
        {
            _procParameterLinkTypeService = procParameterLinkTypeService;
            _logger = logger;
        }


        /// <summary>
        /// 添加（设备参数）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        [LogDescription("设备参数", BusinessType.INSERT)]
        [PermissionDescription("proc:parameterEqu:insert")]
        public async Task AddProcParameterLinkTypeAsync([FromBody] ProcParameterLinkTypeCreateDto parm)
        {
             await _procParameterLinkTypeService.CreateProcParameterLinkTypeAsync(parm);
        }

        /// <summary>
        /// 更新（设备参数）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("update")]
        [LogDescription("设备参数", BusinessType.UPDATE)]
        [PermissionDescription("proc:parameterEqu:update")]
        public async Task UpdateProcParameterLinkTypeAsync([FromBody] ProcParameterLinkTypeModifyDto parm)
        {
             await _procParameterLinkTypeService.ModifyProcParameterLinkTypeAsync(parm);
        }

        /// <summary>
        /// 删除（设备参数）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete")]
        [LogDescription("设备参数", BusinessType.DELETE)]
        [PermissionDescription("proc:parameterEqu:delete")]
        public async Task<int> DeleteProcParameterLinkTypeAsync(long[] ids)
        {
            return await _procParameterLinkTypeService.DeletesProcParameterLinkTypeAsync(ids);
        }

    }
}