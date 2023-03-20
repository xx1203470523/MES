/*
 *describe: 条码生产信息（物理删除）    控制器 | 代码由框架生成  
 *builder:  zhaoqing
 *build datetime: 2023-03-18 05:37:27
 */
using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Manufacture;
using Hymson.MES.Services.Services.Manufacture;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Manufacture
{
    /// <summary>
    /// 控制器（条码生产信息（物理删除））
    /// @author zhaoqing
    /// @date 2023-03-18 05:37:27
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ManuSfcProduceController : ControllerBase
    {
        /// <summary>
        /// 接口（条码生产信息（物理删除））
        /// </summary>
        private readonly IManuSfcProduceService _manuSfcProduceService;
        private readonly ILogger<ManuSfcProduceController> _logger;

        /// <summary>
        /// 构造函数（条码生产信息（物理删除））
        /// </summary>
        /// <param name="manuSfcProduceService"></param>
        /// <param name="logger"></param>
        public ManuSfcProduceController(IManuSfcProduceService manuSfcProduceService, ILogger<ManuSfcProduceController> logger)
        {
            _manuSfcProduceService = manuSfcProduceService;
            _logger = logger;
        }

        /// <summary>
        /// 分页查询列表（条码生产信息（物理删除））
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("pagelist")]
        public async Task<PagedInfo<ManuSfcProduceViewDto>> QueryPagedManuSfcProduceAsync([FromQuery] ManuSfcProducePagedQueryDto parm)
        {
            return await _manuSfcProduceService.GetPageListAsync(parm);
        }

        /// <summary>
        /// 查询详情（条码生产信息（物理删除））
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ManuSfcProduceDto> QueryManuSfcProduceByIdAsync(long id)
        {
            return await _manuSfcProduceService.QueryManuSfcProduceByIdAsync(id);
        }

        /// <summary>
        /// 添加（条码生产信息（物理删除））
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        public async Task AddManuSfcProduceAsync([FromBody] ManuSfcProduceCreateDto parm)
        {
             await _manuSfcProduceService.CreateManuSfcProduceAsync(parm);
        }

        /// <summary>
        /// 更新（条码生产信息（物理删除））
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("update")]
        public async Task UpdateManuSfcProduceAsync([FromBody] ManuSfcProduceModifyDto parm)
        {
            await _manuSfcProduceService.ModifyManuSfcProduceAsync(parm);
        }

        /// <summary>
        /// 删除（条码生产信息（物理删除））
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("delete")]
        public async Task DeleteManuSfcProduceAsync(string ids)
        {
            await _manuSfcProduceService.DeletesManuSfcProduceAsync(ids);
        }

    }
}