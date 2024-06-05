using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Plan;
using Hymson.MES.Services.Services.Plan;
using Hymson.Web.Framework.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Plan
{
    /// <summary>
    /// 控制器（条码打印）
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PlanSfcPrintController : ControllerBase
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly ILogger<PlanSfcPrintController> _logger;

        /// <summary>
        /// 接口（条码打印）
        /// </summary>
        private readonly IPlanSfcPrintService _planSfcPrintService;


        /// <summary>
        /// 构造函数（条码打印）
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="planSfcPrintService"></param>
        public PlanSfcPrintController(ILogger<PlanSfcPrintController> logger,
            IPlanSfcPrintService planSfcPrintService)
        {
            _logger = logger;
            _planSfcPrintService = planSfcPrintService;
        }


        /// <summary>
        /// 添加（条码打印）
        /// </summary>
        /// <param name="createDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        [LogDescription("条码打印", BusinessType.INSERT)]
        //[PermissionDescription("plan:sfcPrint:insert")]
        public async Task CreateAsync([FromBody] PlanSfcPrintCreateDto createDto)
        {
            await _planSfcPrintService.CreateAsync(createDto);
        }

        [HttpPost]
        [Route("print")]
        [LogDescription("条码打印", BusinessType.INSERT)]
        [PermissionDescription("plan:sfcPrint:print")]
        public async Task PrintAsync([FromBody] PlanSfcPrintCreatePrintDto createDto)
        {
            await _planSfcPrintService.CreatePrintAsync(createDto);
        }

        [HttpPost]
        [Route("materialPrint")]
        [LogDescription("物料库存条码打印", BusinessType.INSERT)]
        [PermissionDescription("plan:sfcPrint:materialPrint")]
        public async Task WhMaterialPrintAsync([FromBody] WhMaterialInventoryPrintCreatePrintDto createDto)
        {
            await _planSfcPrintService.WhMaterialPrintAsync(createDto);
        }

        
        /// <summary>
        /// 删除（条码打印）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete")]
        [LogDescription("条码打印", BusinessType.DELETE)]
        [PermissionDescription("plan:sfcPrint:delete")]
        public async Task DeletesAsync(long[] ids)
        {
            await _planSfcPrintService.DeletesAsync(ids);
        }

        /// <summary>
        /// 分页查询列表（条码打印）
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("page")]
        public async Task<PagedInfo<PlanSfcPrintDto>> GetPagedListAsync([FromQuery] PlanSfcPrintPagedQueryDto pagedQueryDto)
        {
            return await _planSfcPrintService.GetPagedListAsync(pagedQueryDto);
        }
    }
}