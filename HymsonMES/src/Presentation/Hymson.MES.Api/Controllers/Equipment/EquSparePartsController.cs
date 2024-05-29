using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Equipment;
using Hymson.MES.Services.Services.Equipment;
using Hymson.Web.Framework.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Equipment
{
    /// <summary>
    /// 控制器（设备-备件注册表）
    /// @author Kongaomeng
    /// @date 2023-12-18 02:37:31
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class EquSparePartsController : ControllerBase
    {
        /// <summary>
        /// 日志
        /// </summary>
        private readonly ILogger<EquSparePartsController> _logger;
        /// <summary>
        /// 服务接口（备件注册表）
        /// </summary>
        private readonly IEquSparePartsService _equSparePartsService;


        /// <summary>
        /// 构造函数（备件注册表）
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="equSparePartsService"></param>
        public EquSparePartsController(ILogger<EquSparePartsController> logger, IEquSparePartsService equSparePartsService)
        {
            _logger = logger;
            _equSparePartsService = equSparePartsService;
        }

        /// <summary>
        /// 添加（备件注册表）
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        [LogDescription("备件注册表", BusinessType.INSERT)]
        public async Task AddEquSparePartsAsync([FromBody] EquSparePartsSaveDto saveDto)
        {
             await _equSparePartsService.CreateEquSparePartsAsync(saveDto);
        }

        /// <summary>
        /// 更新（备件注册表）
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("update")]
        [LogDescription("备件注册表", BusinessType.UPDATE)]
        public async Task UpdateEquSparePartsAsync([FromBody] EquSparePartsSaveDto saveDto)
        {
             await _equSparePartsService.ModifyEquSparePartsAsync(saveDto);
        }

        /// <summary>
        /// 删除（备件注册表）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete")]
        [LogDescription("备件注册表", BusinessType.DELETE)]
        public async Task DeleteEquSparePartsAsync([FromBody] long[] ids)
        {
            await _equSparePartsService.DeletesEquSparePartsAsync(ids);
        }

        /// <summary>
        /// 查询详情（备件注册表）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<EquSparePartsDto?> QueryEquSparePartsByIdAsync(long id)
        {
            return await _equSparePartsService.QueryEquSparePartsByIdAsync(id);
        }

        /// <summary>
        /// 分页查询列表（备件注册表）
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("pagelist")]
        public async Task<PagedInfo<EquSparePartsDto>> QueryPagedEquSparePartsAsync([FromQuery] EquSparePartsPagedQueryDto pagedQueryDto)
        {
            return await _equSparePartsService.GetPagedListAsync(pagedQueryDto);
        }

        /// <summary>
        /// 分页查询列表（过滤已经有类型的备件）//
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("page")]
        public async Task<PagedInfo<EquSparePartsDto>> QueryGetPagedAsync([FromQuery] EquSparePartsPagedQueryDto pagedQueryDto)
        {
            return await _equSparePartsService.GetPagedAsync(pagedQueryDto);
        }

        /// <summary>
        /// 查询备件类型关联的备件
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}/sparePartsGroupList")]
        public async Task<List<EquSparePartsDto>> GetSparePartsGroupRelationByIdAsync(long id)
        {
            return await _equSparePartsService.GetSparePartsGroupRelationByIdAsync(id);
        }

    }
}