using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Warehouse;
using Hymson.MES.Services.Services.Warehouse;
using Hymson.Web.Framework.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Warehouse
{
    /// <summary>
    /// 控制器（物料台账）
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class WhMaterialStandingbookController : ControllerBase
    {
        /// <summary>
        /// 接口（物料台账）
        /// </summary>
        private readonly IWhMaterialStandingbookService _whMaterialStandingbookService;
        private readonly ILogger<WhMaterialStandingbookController> _logger;

        /// <summary>
        /// 构造函数（物料台账）
        /// </summary>
        /// <param name="whMaterialStandingbookService"></param>
        /// <param name="logger"></param>
        public WhMaterialStandingbookController(IWhMaterialStandingbookService whMaterialStandingbookService, ILogger<WhMaterialStandingbookController> logger)
        {
            _whMaterialStandingbookService = whMaterialStandingbookService;
            _logger = logger;
        }

        /// <summary>
        /// 分页查询列表（物料台账）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("pagelist")]
        public async Task<PagedInfo<WhMaterialStandingbookDto>> QueryPagedWhMaterialStandingbookAsync([FromQuery] WhMaterialStandingbookPagedQueryDto parm)
        {
            return await _whMaterialStandingbookService.GetPageListAsync(parm);
        }

        /// <summary>
        /// 查询详情（物料台账）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<WhMaterialStandingbookDto> QueryWhMaterialStandingbookByIdAsync(long id)
        {
            return await _whMaterialStandingbookService.QueryWhMaterialStandingbookByIdAsync(id);
        }

        /// <summary>
        /// 添加（物料台账）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        [LogDescription("物料台账", BusinessType.INSERT)]
        [PermissionDescription("wh:materialStandingbook:insert")]
        public async Task AddWhMaterialStandingbookAsync([FromBody] WhMaterialStandingbookCreateDto parm)
        {
            await _whMaterialStandingbookService.CreateWhMaterialStandingbookAsync(parm);
        }

        /// <summary>
        /// 更新（物料台账）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("update")]
        [LogDescription("物料台账", BusinessType.UPDATE)]
        [PermissionDescription("wh:materialStandingbook:update")]
        public async Task UpdateWhMaterialStandingbookAsync([FromBody] WhMaterialStandingbookModifyDto parm)
        {
            await _whMaterialStandingbookService.ModifyWhMaterialStandingbookAsync(parm);
        }

        /// <summary>
        /// 删除（物料台账）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("delete")]
        [LogDescription("物料台账", BusinessType.DELETE)]
        [PermissionDescription("wh:materialStandingbook:delete")]
        public async Task DeleteWhMaterialStandingbookAsync(string ids)
        {
            await _whMaterialStandingbookService.DeletesWhMaterialStandingbookAsync(ids);
        }

        /// <summary>
        /// 查询详情（关系）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}/relation")]
        public async Task<IEnumerable<WhMaterialStandingBookRelationDto>> GetWhMaterialStandingBookRelationByIdAsync(long id)
        {
            return await _whMaterialStandingbookService.GetWhMaterialStandingBookRelationByIdAsync(id);
        }

        /// <summary>
        /// 查询详情（上料）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}/feeding")]
        public async Task<WhMaterialStandingBookFeedingDto> GetWhMaterialStandingBookFeedingByIdAsync(long id)
        {
            return await _whMaterialStandingbookService.GetWhMaterialStandingBookFeedingByIdAsync(id);
        }

    }
}