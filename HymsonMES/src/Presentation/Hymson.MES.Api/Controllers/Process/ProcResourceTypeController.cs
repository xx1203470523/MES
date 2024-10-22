using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Common;
using Hymson.MES.Services.Dtos.Process;
using Hymson.MES.Services.Services.Process.ResourceType;
using Hymson.Web.Framework.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers
{
    /// <summary>
    /// 资源类型维护表
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
        public async Task<PagedInfo<ProcResourceTypeViewDto>> QueryProcResourceTypeAsync([FromQuery] ProcResourceTypePagedQueryDto query)
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
        public async Task<PagedInfo<ProcResourceTypeDto>> GetProcResourceTypeListAsync([FromQuery] ProcResourceTypePagedQueryDto query)
        {
            return await _procResourceTypeService.GetListAsync(query);
        }

        /// <summary>
        /// 查询资源类型维护表详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ProcResourceTypeDto> GetProcResourceTypeAsync(long id)
        {
            return await _procResourceTypeService.GetListAsync(id);
        }

        /// <summary>
        /// 添加资源类型数据
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPost]
        [LogDescription("资源类型维护", BusinessType.INSERT)]
        [PermissionDescription("proc:resourceType:insert")]
        public async Task<long> AddProcResourceTypeAsync([FromBody] ProcResourceTypeAddDto parm)
        {
            return await _procResourceTypeService.AddProcResourceTypeAsync(parm);
        }

        /// <summary>
        /// 更新资源类型维护表
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPut]
        [LogDescription("资源类型维护", BusinessType.UPDATE)]
        [PermissionDescription("proc:resourceType:update")]
        public async Task UpdateProcResourceTypeAsync([FromBody] ProcResourceTypeUpdateDto parm)
        {
            await _procResourceTypeService.UpdateProcResrouceTypeAsync(parm);
        }

        /// <summary>
        /// 删除资源类型维护表
        /// </summary>
        /// <param name="deleteDto"></param>
        /// <returns></returns>
        [HttpDelete]
        [LogDescription("资源类型维护", BusinessType.DELETE)]
        [PermissionDescription("proc:resourceType:delete")]
        public async Task DeleteProcResourceTypeAsync(DeleteDto deleteDto)
        {
            await _procResourceTypeService.DeleteProcResourceTypeAsync(deleteDto.Ids);
        }
    }
}