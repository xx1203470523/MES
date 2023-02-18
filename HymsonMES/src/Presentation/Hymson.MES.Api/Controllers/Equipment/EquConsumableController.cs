using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Equipment;
using Hymson.MES.Services.Services.Equipment.EquSparePart;
using Hymson.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Equipment
{
    /// <summary>
    /// 控制器（工装注册）
    /// @author 陈志谱
    /// @date 2023-02-18
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class EquConsumableController : ControllerBase
    {
        /// <summary>
        /// 接口（工装注册）
        /// </summary>
        private readonly IEquConsumableService _equConsumableService;
        private readonly ILogger<EquConsumableController> _logger;

        /// <summary>
        /// 构造函数（工装注册）
        /// </summary>
        /// <param name="equSparePartService"></param>
        public EquConsumableController(IEquConsumableService equSparePartService, ILogger<EquConsumableController> logger)
        {
            _equConsumableService = equSparePartService;
            _logger = logger;
        }


        /// <summary>
        /// 添加（工装注册）
        /// </summary>
        /// <param name="createDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        public async Task<int> CreateAsync([FromBody] EquConsumableCreateDto createDto)
        {
            return await _equConsumableService.CreateAsync(createDto);
        }

        /// <summary>
        /// 更新（工装注册）
        /// </summary>
        /// <param name="modifyDto"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("update")]
        public async Task<int> ModifyAsync([FromBody] EquConsumableModifyDto modifyDto)
        {
            return await _equConsumableService.ModifyAsync(modifyDto);
        }

        /// <summary>
        /// 删除（工装注册）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete")]
        public async Task<int> DeletesAsync(string ids)
        {
            long[] idsArr = StringExtension.SpitLongArrary(ids);
            return await _equConsumableService.DeletesAsync(idsArr);
        }
        
        /// <summary>
        /// 分页查询列表（工装注册）
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("pagelist")]
        public async Task<PagedInfo<EquConsumableDto>> GetPagedListAsync([FromQuery] EquConsumablePagedQueryDto pagedQueryDto)
        {
            return await _equConsumableService.GetPagedListAsync(pagedQueryDto);
        }

        /// <summary>
        /// 查询详情（工装注册）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<EquConsumableDto> GetDetailAsync(long id)
        {
            return await _equConsumableService.GetDetailAsync(id);
        }


    }
}