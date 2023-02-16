using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Process;
using Hymson.MES.Services.Services.Process.IProcessService;
using Hymson.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Hymson.MES.Api.Controllers
{
    /// <summary>
    /// 资源维护表Controller
    /// @tableName proc_resource
    /// @author zhaoqing
    /// @date 2023-02-08
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ProcResourceController : ControllerBase
    {
        /// <summary>
        /// 资源维护表接口
        /// </summary>
        private readonly IProcResourceService _procResourceService;
        private readonly ILogger<ProcResourceController> _logger;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="procResourceService"></param>
        /// <param name="logger"></param>
        public ProcResourceController(IProcResourceService procResourceService, ILogger<ProcResourceController> logger)
        {
            _procResourceService = procResourceService;
            _logger = logger;
        }

        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [Route("list")]
        [HttpGet]
        public async Task<PagedInfo<ProcResourceViewDto>> QueryProcResource([FromQuery] ProcResourcePagedQueryDto query)
        {
            return await _procResourceService.GetPageListAsync(query);
        }

        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [Route("querylist")]
        [HttpGet]
        public async Task<PagedInfo<ProcResourceDto>> GetProcResourceList([FromQuery] ProcResourcePagedQueryDto query)
        {
            return await _procResourceService.GetListAsync(query);
        }

        /// <summary>
        /// 查询资源维护表详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ProcResourceDto> GetProcResource(long id)
        {
            return await _procResourceService.GetByIdAsync(id);
        }

        /// <summary>
        /// 查询资源类型下关联的资源
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [Route("listforgroup")]
        [HttpGet]
        public async Task<PagedInfo<ProcResourceDto>> GetListForGroup([FromQuery] ProcResourcePagedQueryDto query)
        {
            return await _procResourceService.GetListForGroupAsync(query);
        }

        /// <summary>
        /// 资源关联打印机数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [Route("print/list")]
        [HttpGet]
        public async Task<PagedInfo<ProcResourceConfigPrintViewDto>> GetResourceConfigPrint([FromQuery] ProcResourceConfigPrintPagedQueryDto query)
        {
            return await _procResourceService.GetcResourceConfigPrintAsync(query);
        }

        /// <summary>
        /// 获取资源设置数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [Route("res/list")]
        [HttpGet]
        public async Task<PagedInfo<ProcResourceConfigResDto>> GetResourceConfigPrint([FromQuery] ProcResourceConfigResPagedQueryDto query)
        {
            return await _procResourceService.GetcResourceConfigResAsync(query);
        }

        /// <summary>
        /// 获取资源关联设备数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [Route("equ/list")]
        [HttpGet]
        public async Task<PagedInfo<ProcResourceEquipmentBindViewDto>> GetResourceConfigEquAsync([FromQuery] ProcResourceEquipmentBindPagedQueryDto query)
        {
            return await _procResourceService.GetcResourceConfigEquAsync(query);
        }

        /// <summary>
        /// 获取资源关联作业数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [Route("job/list")]
        [HttpGet]
        public async Task<PagedInfo<ProcResourceConfigJobViewDto>> GetcResourceConfigJoAsync([FromQuery] ProcResourceConfigJobPagedQueryDto query)
        {
            return await _procResourceService.GetcResourceConfigJoAsync(query);
        }

        /// <summary>
        /// 添加资源数据
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task AddProcResource([FromBody] ProcResourceCreateDto parm)
        {
            await _procResourceService.AddProcResourceAsync(parm);
        }

        /// <summary>
        /// 更新资源维护表
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task UpdateProcResource([FromBody] ProcResourceModifyDto parm)
        {
            await _procResourceService.UpdateProcResrouceAsync(parm);
        }

        /// <summary>
        /// 删除资源维护表
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete("{ids}")]
        public async Task DeleteProcResource(string ids)
        {
            await _procResourceService.DeleteProcResourceAsync(ids);
        }
    }
}