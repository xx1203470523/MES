using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.OnStock;
using Hymson.MES.Services.Dtos.Process;
using Hymson.MES.Services.Services.OnStock;
using Hymson.MES.Services.Services.Process;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers
{
    /// <summary>
    /// ��Դ����ά����Controller
    /// @tableName proc_resource_type
    /// @author zhaoqing
    /// @date 2023-02-06
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ProcResourceTypeController : ControllerBase
    {
        /// <summary>
        /// ��Դ����ά����ӿ�
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
        /// ��ȡ��ҳ����
        /// </summary>
        /// <param name="procResourceTypePagedQueryDto"></param>
        /// <returns></returns>
        [HttpGet("list")]
        public async Task<PagedInfo<ProcResourceTypeViewDto>> QueryProcResourceType([FromQuery] ProcResourceTypePagedQueryDto procResourceTypePagedQueryDto)
        {
            return await _procResourceTypeService.GetPageListAsync(procResourceTypePagedQueryDto);
        }

        /// <summary>
        /// ��ȡ��ҳ����
        /// </summary>
        /// <param name="procResourceTypePagedQueryDto"></param>
        /// <returns></returns>
        [HttpGet("querylist")]
        public async Task<PagedInfo<ProcResourceTypeDto>> GetProcResourceTypeList([FromQuery] ProcResourceTypePagedQueryDto procResourceTypePagedQueryDto)
        {
            return await _procResourceTypeService.GetListAsync(procResourceTypePagedQueryDto);
        }

        /// <summary>
        /// ��ѯ��Դ����ά��������
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
        ///// ������¼
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