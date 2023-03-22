/*
 *creator: Karl
 *
 *describe: 条码信息表    控制器 | 代码由框架生成  
 *builder:  pengxin
 *build datetime: 2023-03-21 04:00:29
 */
using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Manufacture;
using Hymson.MES.Services.Services.Manufacture;
//using Hymson.Utils.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Extensions;

namespace Hymson.MES.Api.Controllers.Manufacture
{
    /// <summary>
    /// 控制器（条码信息表）
    /// @author pengxin
    /// @date 2023-03-21 04:00:29
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ManuSfcInfoController : ControllerBase
    {
        /// <summary>
        /// 接口（条码信息表）
        /// </summary>
        private readonly IManuSfcInfoService _manuSfcInfoService;
        private readonly ILogger<ManuSfcInfoController> _logger;

        /// <summary>
        /// 构造函数（条码信息表）
        /// </summary>
        /// <param name="manuSfcInfoService"></param>
        public ManuSfcInfoController(IManuSfcInfoService manuSfcInfoService, ILogger<ManuSfcInfoController> logger)
        {
            _manuSfcInfoService = manuSfcInfoService;
            _logger = logger;
        }

        /// <summary>
        /// 分页查询列表（条码信息表）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("pagelist")]
        public async Task<PagedInfo<ManuSfcInfoDto>> QueryPagedManuSfcInfoAsync([FromQuery] ManuSfcInfoPagedQueryDto parm)
        {
            return await _manuSfcInfoService.GetPageListAsync(parm);
        }

        /// <summary>
        /// 查询详情（条码信息表）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ManuSfcInfoDto> QueryManuSfcInfoByIdAsync(long id)
        {
            return await _manuSfcInfoService.QueryManuSfcInfoByIdAsync(id);
        }

        /// <summary>
        /// 添加（条码信息表）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        public async Task AddManuSfcInfoAsync([FromBody] ManuSfcInfoCreateDto parm)
        {
            await _manuSfcInfoService.CreateManuSfcInfoAsync(parm);
        }

        /// <summary>
        /// 更新（条码信息表）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("update")]
        public async Task UpdateManuSfcInfoAsync([FromBody] ManuSfcInfoModifyDto parm)
        {
            await _manuSfcInfoService.ModifyManuSfcInfoAsync(parm);
        }

        /// <summary>
        /// 删除（条码信息表）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete")]
        public async Task DeleteManuSfcInfoAsync([FromBody] long[] ids)
        {
            await _manuSfcInfoService.DeletesManuSfcInfoAsync(ids);
        }

    }
}