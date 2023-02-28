using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Equipment;
using Hymson.MES.Services.Services.Equipment.EquSparePartType;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Equipment
{
    /// <summary>
    /// 控制器（工装类型）
    /// @author 陈志谱
    /// @date 2023-02-18
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class EquConsumableTypeController : ControllerBase
    {
        /// <summary>
        /// 接口（工装类型）
        /// </summary>
        private readonly IEquConsumableTypeService _equConsumableTypeService;
        private readonly ILogger<EquConsumableTypeController> _logger;

        /// <summary>
        /// 构造函数（工装类型）
        /// </summary>
        /// <param name="equSparePartTypeService"></param>
        public EquConsumableTypeController(IEquConsumableTypeService equSparePartTypeService, ILogger<EquConsumableTypeController> logger)
        {
            _equConsumableTypeService = equSparePartTypeService;
            _logger = logger;
        }


        /// <summary>
        /// 添加（工装类型）
        /// </summary>
        /// <param name="createDto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task CreateAsync(EquConsumableTypeCreateDto createDto)
        {
            await _equConsumableTypeService.CreateAsync(createDto);
        }

        /// <summary>
        /// 更新（工装类型）
        /// </summary>
        /// <param name="modifyDto"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task ModifyAsync(EquConsumableTypeModifyDto modifyDto)
        {
            await _equConsumableTypeService.ModifyAsync(modifyDto);
        }

        /// <summary>
        /// 删除（工装类型）
        /// </summary>
        /// <param name="deleteDto"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task DeletesAsync(EquConsumableTypeDeleteDto deleteDto)
        {
            await _equConsumableTypeService.DeletesAsync(deleteDto.Ids);
        }

        /// <summary>
        /// 分页查询列表（工装类型）
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("page")]
        public async Task<PagedInfo<EquConsumableTypeDto>> GetPagedListAsync([FromQuery] EquConsumableTypePagedQueryDto pagedQueryDto)
        {
            return await _equConsumableTypeService.GetPagedListAsync(pagedQueryDto);
        }

        /// <summary>
        /// 查询详情（工装类型）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<EquConsumableTypeDto> GetDetailAsync(long id)
        {
            return await _equConsumableTypeService.GetDetailAsync(id);
        }

    }
}