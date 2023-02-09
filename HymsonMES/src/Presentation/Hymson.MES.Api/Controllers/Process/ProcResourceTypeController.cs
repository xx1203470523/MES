using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.OnStock;
using Hymson.MES.Services.Dtos.Process;
using Hymson.MES.Services.Services.OnStock;
using Hymson.MES.Services.Services.Process.IProcessService;
using Hymson.Utils.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers
{
    /// <summary>
    /// 资源类型维护表Controller
    /// @tableName proc_resource_type
    /// @author zhaoqing
    /// @date 2023-02-06
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ProcResourceTypeController : ControllerBase
    {
        /// <summary>
        /// 资源类型维护表接口
        /// </summary>
        private readonly IProcResourceTypeService _procResourceTypeService;
        private readonly ILogger<ProcResourceTypeController> _logger;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="procResourceTypeService"></param>
        /// <param name="logger"></param>
        public ProcResourceTypeController(IProcResourceTypeService procResourceTypeService, ILogger<ProcResourceTypeController> logger)
        {
            _procResourceTypeService = procResourceTypeService;
            _logger = logger;
        }

        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [Route("list")]
        [HttpGet]
        public async Task<PagedInfo<ProcResourceTypeViewDto>> QueryProcResourceType([FromQuery] ProcResourceTypePagedQueryDto query)
        {
            return await _procResourceTypeService.GetPageListAsync(query);
        }

        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [Route("querylist")]
        [HttpGet]
        public async Task<PagedInfo<ProcResourceTypeDto>> GetProcResourceTypeList([FromQuery] ProcResourceTypePagedQueryDto query)
        {
            return await _procResourceTypeService.GetListAsync(query);
        }

        /// <summary>
        /// 查询资源类型维护表详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        //[ActionPermissionFilter(Permission = "business:procResourceType:query")]
        public async Task<ProcResourceTypeDto> GetProcResourceType(long id)
        {
            return await _procResourceTypeService.GetListAsync(id);
        }

        /// <summary>
        /// 添加资源类型数据
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task AddProcResourceType([FromBody] ProcResourceTypeAddCommandDto parm)
        {
            await _procResourceTypeService.AddProcResourceTypeAsync(parm);
        }

        /// <summary>
        /// 更新资源类型维护表
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task UpdateProcResourceType([FromBody] ProcResourceTypeUpdateCommandDto parm)
        {
            await _procResourceTypeService.UpdateProcResrouceTypeAsync(parm);
        }

        /// <summary>
        /// 更新资源类型维护表
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete("{ids}")]
        public async Task DeleteProcResourceType(string ids)
        {
            await _procResourceTypeService.DeleteProcResourceTypeAsync(ids);
        }
    }
}