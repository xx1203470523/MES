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
    /// 控制器（工具类型）
    /// @author kongaomeng
    /// @date 2023-12-15 10:56:56
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class EquToolingTypeController : ControllerBase
    {
        /// <summary>
        /// 日志
        /// </summary>
        private readonly ILogger<EquToolingTypeController> _logger;
        /// <summary>
        /// 服务接口（工具类型）
        /// </summary>
        private readonly IEquToolingTypeService _equToolingTypeService;


        /// <summary>
        /// 构造函数（工具类型）
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="equToolingTypeService"></param>
        public EquToolingTypeController(ILogger<EquToolingTypeController> logger, IEquToolingTypeService equToolingTypeService)
        {
            _logger = logger;
            _equToolingTypeService = equToolingTypeService;
        }

        /// <summary>
        /// 添加（工具类型）
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        [LogDescription("工具类型", BusinessType.INSERT)]
        public async Task AddEquSparePartsGroupAsync([FromBody] EquToolingTypeSaveDto saveDto)
        {
             await _equToolingTypeService.CreateEquSparePartsGroupAsync(saveDto);
        }

        /// <summary>
        /// 更新（工具类型）
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("update")]
        [LogDescription("工具类型", BusinessType.UPDATE)]
        public async Task UpdateEquSparePartsGroupAsync([FromBody] EquToolingTypeSaveDto saveDto)
        {
             await _equToolingTypeService.ModifyEquSparePartsGroupAsync(saveDto);
        }

        /// <summary>
        /// 删除（工具类型）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete")]
        [LogDescription("工具类型", BusinessType.DELETE)]
        public async Task DeleteEquSparePartsGroupAsync([FromBody] long[] ids)
        {
            await _equToolingTypeService.DeletesEquSparePartsGroupAsync(ids);
        }

        /// <summary>
        /// 查询详情（工具类型）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<EquToolingTypeDto?> QueryEquSparePartsGroupByIdAsync(long id)
        {
            return await _equToolingTypeService.QueryEquSparePartsGroupByIdAsync(id);
        }

        /// <summary>
        /// 查询容器关联的设备组
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}/equipmentGroupList")]
        public async Task<List<EquToolingTypeGroupEquipmentGroupRelationSaveDto>> GetQualUnqualifiedCodeGroupRelationByIdAsync(long id)
        {
            return await _equToolingTypeService.GetSparePartsEquipmentGroupRelationByIdAsync(id);
        }

        ///// <summary>
        ///// 查询容器关联的物料
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //[HttpGet("{id}/MaterialList")]
        //public async Task<List<EquSparePartsGroupEquipmentGroupRelationSaveDto>> GetQualUnqualifiedCodeMaterialRelationByIdAsync(long id)
        //{
        //    return await _equToolingTypeService.GetSparePartsEquipmentGroupRelationByIdAsync(id);
        //}

        /// <summary>
        /// 分页查询列表（工具类型）
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("pagelist")]
        public async Task<PagedInfo<EquToolingTypeDto>> QueryPagedEquSparePartsGroupAsync([FromQuery] EquToolingTypeQueryDto pagedQueryDto)
        {
            return await _equToolingTypeService.GetPagedListAsync(pagedQueryDto);
        }
    }
}