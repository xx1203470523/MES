using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Equipment;
using Hymson.MES.Services.Dtos.Plan;
using Hymson.MES.Services.Dtos.Quality;
using Hymson.MES.Services.Services.Equipment;
using Hymson.Web.Framework.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Equipment
{
    /// <summary>
    /// 控制器（备件类型）
    /// @author kongaomeng
    /// @date 2023-12-15 10:56:56
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class EquSparePartsGroupController : ControllerBase
    {
        /// <summary>
        /// 日志
        /// </summary>
        private readonly ILogger<EquSparePartsGroupController> _logger;
        /// <summary>
        /// 服务接口（备件类型）
        /// </summary>
        private readonly IEquSparePartsGroupService _equSparePartsGroupService;


        /// <summary>
        /// 构造函数（备件类型）
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="equSparePartsGroupService"></param>
        public EquSparePartsGroupController(ILogger<EquSparePartsGroupController> logger, IEquSparePartsGroupService equSparePartsGroupService)
        {
            _logger = logger;
            _equSparePartsGroupService = equSparePartsGroupService;
        }

        /// <summary>
        /// 添加（备件类型）
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        [LogDescription("备件类型", BusinessType.INSERT)]
        public async Task AddEquSparePartsGroupAsync([FromBody] EquSparePartsGroupSaveDto saveDto)
        {
             await _equSparePartsGroupService.CreateEquSparePartsGroupAsync(saveDto);
        }

        /// <summary>
        /// 更新（备件类型）
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("update")]
        [LogDescription("备件类型", BusinessType.UPDATE)]
        public async Task UpdateEquSparePartsGroupAsync([FromBody] EquSparePartsGroupSaveDto saveDto)
        {
             await _equSparePartsGroupService.ModifyEquSparePartsGroupAsync(saveDto);
        }

        /// <summary>
        /// 删除（备件类型）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete")]
        [LogDescription("备件类型", BusinessType.DELETE)]
        public async Task DeleteEquSparePartsGroupAsync([FromBody] long[] ids)
        {
            await _equSparePartsGroupService.DeletesEquSparePartsGroupAsync(ids);
        }

        /// <summary>
        /// 查询详情（备件类型）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<EquSparePartsGroupDto?> QueryEquSparePartsGroupByIdAsync(long id)
        {
            return await _equSparePartsGroupService.QueryEquSparePartsGroupByIdAsync(id);
        }

        /// <summary>
        /// 查询容器关联的设备组
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}/equipmentGroupList")]
        public async Task<List<EquSparePartsGroupEquipmentGroupRelationSaveDto>> GetQualUnqualifiedCodeGroupRelationByIdAsync(long id)
        {
            return await _equSparePartsGroupService.GetSparePartsEquipmentGroupRelationByIdAsync(id);
        }

        /// <summary>
        /// 分页查询列表（备件类型）
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("pagelist")]
        public async Task<PagedInfo<EquSparePartsGroupDto>> QueryPagedEquSparePartsGroupAsync([FromQuery] EquSparePartsGroupPagedQueryDto pagedQueryDto)
        {
            return await _equSparePartsGroupService.GetPagedListAsync(pagedQueryDto);
        }
    }
}