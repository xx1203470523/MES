using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Equipment;
using Hymson.MES.Services.Services.Equipment.EquSparePartType;
using Hymson.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Equipment
{
    /// <summary>
    /// 控制器（备件类型）
    /// @author 陈志谱
    /// @date 2023-02-11 04:10:42
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class EquSparePartTypeController : ControllerBase
    {
        /// <summary>
        /// 接口（备件类型）
        /// </summary>
        private readonly IEquSparePartTypeService _equSparePartTypeService;
        private readonly ILogger<EquSparePartTypeController> _logger;

        /// <summary>
        /// 构造函数（备件类型）
        /// </summary>
        /// <param name="equSparePartTypeService"></param>
        public EquSparePartTypeController(IEquSparePartTypeService equSparePartTypeService, ILogger<EquSparePartTypeController> logger)
        {
            _equSparePartTypeService = equSparePartTypeService;
            _logger = logger;
        }


        /// <summary>
        /// 添加（备件类型）
        /// </summary>
        /// <param name="createDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        public async Task<int> CreateAsync([FromBody] EquSparePartTypeCreateDto createDto)
        {
            return await _equSparePartTypeService.CreateAsync(createDto);
        }

        /// <summary>
        /// 更新（备件类型）
        /// </summary>
        /// <param name="modifyDto"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("update")]
        public async Task<int> ModifyAsync([FromBody] EquSparePartTypeModifyDto modifyDto)
        {
            return await _equSparePartTypeService.ModifyAsync(modifyDto);
        }

        /// <summary>
        /// 删除（备件类型）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete")]
        public async Task<int> DeletesAsync(string ids)
        {
            long[] idsArr = StringExtension.SpitLongArrary(ids);
            return await _equSparePartTypeService.DeletesAsync(idsArr);
        }

        /// <summary>
        /// 分页查询列表（备件类型）
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("pagelist")]
        public async Task<PagedInfo<EquSparePartTypeDto>> GetPagedListAsync([FromQuery] EquSparePartTypePagedQueryDto pagedQueryDto)
        {
            return await _equSparePartTypeService.GetPagedListAsync(pagedQueryDto);
        }

        /// <summary>
        /// 查询详情（备件类型）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<EquSparePartTypeDto> GetDetailAsync(long id)
        {
            return await _equSparePartTypeService.GetDetailAsync(id);
        }

    }
}