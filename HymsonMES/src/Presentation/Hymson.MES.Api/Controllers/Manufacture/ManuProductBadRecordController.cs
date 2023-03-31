/*
 *describe: 产品不良录入    控制器 | 代码由框架生成  
 *builder:  zhaoqing
 *build datetime: 2023-03-27 03:49:17
 */
using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Common;
using Hymson.MES.Services.Dtos.Manufacture;
using Hymson.MES.Services.Services.Manufacture;
using Hymson.MES.Services.Services.Manufacture.ManuSfcProduce;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Manufacture
{
    /// <summary>
    /// 控制器（产品不良录入）
    /// @author zhaoqing
    /// @date 2023-03-27 03:49:17
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ManuProductBadRecordController : ControllerBase
    {
        /// <summary>
        /// 接口（产品不良录入）
        /// </summary>
        private readonly IManuProductBadRecordService _manuProductBadRecordService;
        private readonly ILogger<ManuProductBadRecordController> _logger;

        /// <summary>
        /// 构造函数（产品不良录入）
        /// </summary>
        /// <param name="manuProductBadRecordService"></param>
        public ManuProductBadRecordController(IManuProductBadRecordService manuProductBadRecordService, ILogger<ManuProductBadRecordController> logger)
        {
            _manuProductBadRecordService = manuProductBadRecordService;
            _logger = logger;
        }

        /// <summary>
        /// 分页查询列表（产品不良录入）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("pagelist")]
        public async Task<PagedInfo<ManuProductBadRecordDto>> QueryPagedManuProductBadRecordAsync([FromQuery] ManuProductBadRecordPagedQueryDto parm)
        {
            return await _manuProductBadRecordService.GetPageListAsync(parm);
        }

        /// <summary>
        /// 查询详情（产品不良录入）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ManuProductBadRecordDto> QueryManuProductBadRecordByIdAsync(long id)
        {
            return await _manuProductBadRecordService.QueryManuProductBadRecordByIdAsync(id);
        }

        /// <summary>
        /// 添加（产品不良录入）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task AddManuProductBadRecordAsync([FromBody] ManuProductBadRecordCreateDto parm)
        {
            await _manuProductBadRecordService.CreateManuProductBadRecordAsync(parm);
        }

        /// <summary>
        /// 查询条码的不合格代码信息
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("badRecords")]
        public async Task<IEnumerable<ManuProductBadRecordViewDto>> GetBadRecordsBySfcAsync([FromQuery] ManuProductBadRecordQueryDto parm)
        {
            return await _manuProductBadRecordService.GetBadRecordsBySfcAsync(parm);
        }

        /// <summary>
        /// 条码报废
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("cancelIdentify")]
        public async Task CancelSfcIdentification(CancelSfcIdentificationDto parm)
        {
            await _manuProductBadRecordService.CancelSfcIdentificationAsync(parm);
        }

        /// <summary>
        /// 更新（产品不良录入）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task UpdateManuProductBadRecordAsync([FromBody] ManuProductBadRecordModifyDto parm)
        {
            await _manuProductBadRecordService.ModifyManuProductBadRecordAsync(parm);
        }

        /// <summary>
        /// 删除（产品不良录入）
        /// </summary>
        /// <param name="deleteDto"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task DeleteManuProductBadRecordAsync(DeleteDto deleteDto)
        {
            await _manuProductBadRecordService.DeletesManuProductBadRecordAsync(deleteDto.Ids);
        }

    }
}