using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Manufacture;
using Hymson.MES.Services.Services.Manufacture;
using Hymson.Web.Framework.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Manufacture
{
    /// <summary>
    /// 控制器（Marking信息表）
    /// @author xiaofei
    /// @date 2024-07-24 08:40:23
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ManuSfcMarkingController : ControllerBase
    {
        /// <summary>
        /// 日志
        /// </summary>
        private readonly ILogger<ManuSfcMarkingController> _logger;
        /// <summary>
        /// 服务接口（Marking信息表）
        /// </summary>
        private readonly IManuSfcMarkingService _manuSfcMarkingService;


        /// <summary>
        /// 构造函数（Marking信息表）
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="manuSfcMarkingService"></param>
        public ManuSfcMarkingController(ILogger<ManuSfcMarkingController> logger, IManuSfcMarkingService manuSfcMarkingService)
        {
            _logger = logger;
            _manuSfcMarkingService = manuSfcMarkingService;
        }

        /// <summary>
        /// 添加（Marking信息表）
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        [LogDescription("Marking录入", BusinessType.INSERT)]
        public async Task AddAsync([FromBody] List<ManuSfcMarkingSaveDto> saveDto)
        {
            await _manuSfcMarkingService.CreateAsync(saveDto);
        }

        /// <summary>
        /// 批量导入
        /// </summary>
        /// <param name="formFile"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("import")]
        [LogDescription("批量导入Marking", BusinessType.INSERT)]
        public async Task ImportAsync([FromForm(Name = "file")] IFormFile formFile)
        {
            await _manuSfcMarkingService.ImportAsync(formFile);
        }

        /// <summary>
        /// 获取打开状态Marking信息
        /// </summary>
        /// <param name="sfc"></param>
        /// <returns></returns>
        [HttpGet("getOpenMarkingList")]
        public async Task<IEnumerable<MarkingInfoDto>> GetOpenMarkingListBySFCAsync(string sfc)
        {
            return await _manuSfcMarkingService.GetOpenMarkingListBySFCAsync(sfc);
        }

        /// <summary>
        /// 查询详情（Marking信息表）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ManuSfcMarkingDto?> QueryByIdAsync(long id)
        {
            return await _manuSfcMarkingService.QueryByIdAsync(id);
        }

        /// <summary>
        /// 分页查询列表（Marking信息表）
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("pagelist")]
        public async Task<PagedInfo<ManuSfcMarkingDto>> QueryPagedListAsync([FromQuery] ManuSfcMarkingPagedQueryDto pagedQueryDto)
        {
            return await _manuSfcMarkingService.GetPagedListAsync(pagedQueryDto);
        }

    }
}