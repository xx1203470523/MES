using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.ManuFillingDataRecord;
using Hymson.MES.Services.Services.ManuFillingDataRecord;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.ManuFillingDataRecord
{
    /// <summary>
    /// 控制器（补液数据上传记录）
    /// @author Yxx
    /// @date 2024-03-19 07:42:52
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ManuFillingDataRecordController : ControllerBase
    {
        /// <summary>
        /// 日志
        /// </summary>
        private readonly ILogger<ManuFillingDataRecordController> _logger;
        /// <summary>
        /// 服务接口（补液数据上传记录）
        /// </summary>
        private readonly IManuFillingDataRecordService _manuFillingDataRecordService;


        /// <summary>
        /// 构造函数（补液数据上传记录）
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="manuFillingDataRecordService"></param>
        public ManuFillingDataRecordController(ILogger<ManuFillingDataRecordController> logger, IManuFillingDataRecordService manuFillingDataRecordService)
        {
            _logger = logger;
            _manuFillingDataRecordService = manuFillingDataRecordService;
        }

        /// <summary>
        /// 添加（补液数据上传记录）
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        public async Task AddAsync([FromBody] ManuFillingDataRecordSaveDto saveDto)
        {
             await _manuFillingDataRecordService.CreateAsync(saveDto);
        }

        /// <summary>
        /// 更新（补液数据上传记录）
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("update")]
        public async Task UpdateAsync([FromBody] ManuFillingDataRecordSaveDto saveDto)
        {
             await _manuFillingDataRecordService.ModifyAsync(saveDto);
        }

        /// <summary>
        /// 删除（补液数据上传记录）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete")]
        public async Task DeleteAsync([FromBody] long[] ids)
        {
            await _manuFillingDataRecordService.DeletesAsync(ids);
        }

        /// <summary>
        /// 查询详情（补液数据上传记录）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ManuFillingDataRecordDto?> QueryByIdAsync(long id)
        {
            return await _manuFillingDataRecordService.QueryByIdAsync(id);
        }

        /// <summary>
        /// 分页查询列表（补液数据上传记录）
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("pagelist")]
        public async Task<PagedInfo<ManuFillingDataRecordDto>> QueryPagedListAsync([FromQuery] ManuFillingDataRecordPagedQueryDto pagedQueryDto)
        {
            return await _manuFillingDataRecordService.GetPagedListAsync(pagedQueryDto);
        }

    }
}