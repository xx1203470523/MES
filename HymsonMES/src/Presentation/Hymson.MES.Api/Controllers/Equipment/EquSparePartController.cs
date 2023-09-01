using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Equipment;
using Hymson.MES.Services.Services.Equipment.EquSparePart;
using Hymson.Web.Framework.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Equipment
{
    /// <summary>
    /// 控制器（备件注册）
    /// @author 陈志谱
    /// @date 2023-02-13 02:38:21
    /// </summary>
    
    [ApiController]
    [Route("api/v1/[controller]")]
    public class EquSparePartController : ControllerBase
    {
        /// <summary>
        /// 接口（备件注册）
        /// </summary>
        private readonly IEquSparePartService _equSparePartService;
        private readonly ILogger<EquSparePartController> _logger;

        /// <summary>
        /// 构造函数（备件注册）
        /// </summary>
        /// <param name="equSparePartService"></param>
        /// <param name="logger"></param>
        public EquSparePartController(IEquSparePartService equSparePartService, ILogger<EquSparePartController> logger)
        {
            _equSparePartService = equSparePartService;
            _logger = logger;
        }


        /// <summary>
        /// 添加（备件注册）
        /// </summary>
        /// <param name="createDto"></param>
        /// <returns></returns>
        [HttpPost]
        [LogDescription("备件注册", BusinessType.INSERT)]
        [PermissionDescription("equ:sparePart:insert")]
        public async Task CreateAsync(EquSparePartSaveDto createDto)
        {
            await _equSparePartService.CreateAsync(createDto);
        }

        /// <summary>
        /// 更新（备件注册）
        /// </summary>
        /// <param name="modifyDto"></param>
        /// <returns></returns>
        [HttpPut]
        [LogDescription("备件注册", BusinessType.UPDATE)]
        [PermissionDescription("equ:sparePart:update")]
        public async Task ModifyAsync(EquSparePartSaveDto modifyDto)
        {
            await _equSparePartService.ModifyAsync(modifyDto);
        }

        /// <summary>
        /// 删除（备件注册）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        [LogDescription("备件注册", BusinessType.DELETE)]
        [PermissionDescription("equ:sparePart:delete")]
        public async Task DeletesAsync(long[] ids)
        {
            await _equSparePartService.DeletesAsync(ids);
        }

        /// <summary>
        /// 分页查询列表（备件注册）
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("page")]
        //[PermissionDescription("equ:sparePart:list")]
        public async Task<PagedInfo<EquSparePartDto>> GetPagedListAsync([FromQuery] EquSparePartPagedQueryDto pagedQueryDto)
        {
            return await _equSparePartService.GetPagedListAsync(pagedQueryDto);
        }

        /// <summary>
        /// 查询详情（备件注册）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<EquSparePartDto> GetDetailAsync(long id)
        {
            return await _equSparePartService.GetDetailAsync(id);
        }


    }
}