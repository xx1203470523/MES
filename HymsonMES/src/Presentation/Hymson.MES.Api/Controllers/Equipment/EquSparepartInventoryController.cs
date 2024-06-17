/*
 *creator: Karl
 *
 *describe: 备件库存    控制器 | 代码由框架生成  
 *builder:  pengxin
 *build datetime: 2024-06-12 10:15:26
 */
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Services.Dtos.EquSparepartInventory;
using Hymson.MES.Services.Services.EquSparepartInventory;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.EquSparepartInventory
{
    /// <summary>
    /// 控制器（备件库存）
    /// @author pengxin
    /// @date 2024-06-12 10:15:26
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class EquSparepartInventoryController : ControllerBase
    {
        /// <summary>
        /// 接口（备件库存）
        /// </summary>
        private readonly IEquSparepartInventoryService _equSparepartInventoryService;
        private readonly ILogger<EquSparepartInventoryController> _logger;

        /// <summary>
        /// 构造函数（备件库存）
        /// </summary>
        /// <param name="equSparepartInventoryService"></param>
        public EquSparepartInventoryController(IEquSparepartInventoryService equSparepartInventoryService, ILogger<EquSparepartInventoryController> logger)
        {
            _equSparepartInventoryService = equSparepartInventoryService;
            _logger = logger;
        }

        #region 框架生成方法

        /// <summary>
        /// 获取备件信息
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("getEquSpareParts")]
        public async Task<GetEquSparePartsDto> GetEquSpareParts([FromQuery] GetEquSparePartsParamDto parm)
        {
            return await _equSparepartInventoryService.GetEquSparePartsAsync(parm);
        }


        /// <summary>
        /// 获取出库选择备件信息
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("getOutboundChoose")]
        public async Task<IEnumerable<GetOutboundChooseEquSparePartsDto>> GetOutboundChooseEquSparePartsAsync([FromQuery] GetOutboundChooseEquSparePartsParamDto parm)
        {
            return await _equSparepartInventoryService.GetOutboundChooseEquSparePartsAsync(parm);
        }

        /// <summary>
        /// 分页查询列表（备件库存）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("pagelist")]
        public async Task<PagedInfo<EquSparepartInventoryPageDto>> QueryPagedEquSparepartInventoryAsync([FromQuery] EquSparepartInventoryPagedQueryDto parm)
        {
            return await _equSparepartInventoryService.GetPagedListAsync(parm);
        }

        /// <summary>
        /// 添加（备件库存）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("operation")]
        public async Task OperationEquSparepartInventoryAsync([FromBody] OperationEquSparepartInventoryDto parm)
        {
            await _equSparepartInventoryService.OperationEquSparepartInventoryAsync(parm);
        }

        /// <summary>
        /// 查询详情（备件库存）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<EquSparepartInventoryDto> QueryEquSparepartInventoryByIdAsync(long id)
        {
            return await _equSparepartInventoryService.QueryEquSparepartInventoryByIdAsync(id);
        }



        /// <summary>
        /// 更新（备件库存）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("update")]
        public async Task UpdateEquSparepartInventoryAsync([FromBody] EquSparepartInventoryModifyDto parm)
        {
            await _equSparepartInventoryService.ModifyEquSparepartInventoryAsync(parm);
        }

        /// <summary>
        /// 删除（备件库存）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete")]
        public async Task DeleteEquSparepartInventoryAsync([FromBody] long[] ids)
        {
            await _equSparepartInventoryService.DeletesEquSparepartInventoryAsync(ids);
        }

        #endregion
    }
}