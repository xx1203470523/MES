using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.OnStock;
using Hymson.MES.Services.Dtos.Process;
using Hymson.MES.Services.Services.OnStock;
using Hymson.MES.Services.Services.Process;
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
        /// 
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
        /// <param name="procResourceTypePagedQueryDto"></param>
        /// <returns></returns>
        [HttpGet("list")]
        public async Task<PagedInfo<ProcResourceTypeViewDto>> QueryProcResourceType([FromQuery] ProcResourceTypePagedQueryDto procResourceTypePagedQueryDto)
        {
            return await _procResourceTypeService.GetPageListAsync(procResourceTypePagedQueryDto);
        }

        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <param name="procResourceTypePagedQueryDto"></param>
        /// <returns></returns>
        [HttpGet("querylist")]
        public async Task<PagedInfo<ProcResourceTypeDto>> GetProcResourceTypeList([FromQuery] ProcResourceTypePagedQueryDto procResourceTypePagedQueryDto)
        {
            return await _procResourceTypeService.GetListAsync(procResourceTypePagedQueryDto);
        }

        /// <summary>
        /// 查询资源类型维护表详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{Id}")]
        //[ActionPermissionFilter(Permission = "business:procResourceType:query")]
        public async Task<ProcResourceTypeDto> GetProcResourceType(long id)
        {
            return await _procResourceTypeService.GetListAsync(id);
        }

        ///// <summary>
        ///// 创建记录
        ///// </summary>
        ///// <param name="whStockChangeRecordDto"></param>
        ///// <returns></returns>
        //[HttpPost]
        //[Route("create")]
        //public async Task Create(WhStockChangeRecordDto whStockChangeRecordDto)
        //{
        //    await _whStockChangeRecordService.CreateWhStockChangeRecordAsync(whStockChangeRecordDto);
        //}
    }
}