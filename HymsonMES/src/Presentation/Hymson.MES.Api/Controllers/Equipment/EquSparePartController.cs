using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Equipment;
using Hymson.MES.Services.Services.Equipment.EquSparePart;
using Hymson.Utils.Extensions;
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
        [Route("create")]
        public async Task AddEquSparePartAsync([FromBody] EquSparePartCreateDto createDto)
        {
            await _equSparePartService.CreateEquSparePartAsync(createDto);
        }

        /// <summary>
        /// 更新（备件注册）
        /// </summary>
        /// <param name="modifyDto"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("update")]
        public async Task UpdateEquSparePartAsync([FromBody] EquSparePartModifyDto modifyDto)
        {
            await _equSparePartService.ModifyEquSparePartAsync(modifyDto);
        }

        /// <summary>
        /// 删除（备件注册）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete")]
        public async Task DeleteEquSparePartAsync(string ids)
        {
            long[] idsArr = StringExtension.SpitLongArrary(ids);
            await _equSparePartService.DeletesEquSparePartAsync(idsArr);
        }
        
        /// <summary>
        /// 分页查询列表（备件注册）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("pagelist")]
        public async Task<PagedInfo<EquSparePartDto>> QueryPagedEquSparePartAsync([FromQuery] EquSparePartPagedQueryDto parm)
        {
            return await _equSparePartService.GetPageListAsync(parm);
        }

        /// <summary>
        /// 查询详情（备件注册）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<EquSparePartDto> QueryEquSparePartByIdAsync(long id)
        {
            return await _equSparePartService.QueryEquSparePartByIdAsync(id);
        }


    }
}