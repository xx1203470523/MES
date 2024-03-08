using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.CcdFileUploadCompleteRecord;
using Hymson.MES.Services.Services.CcdFileUploadCompleteRecord;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.CcdFileUploadCompleteRecord
{
    /// <summary>
    /// 控制器（CCD文件上传完成）
    /// @author Yxx
    /// @date 2024-03-08 10:30:50
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CcdFileUploadCompleteRecordController : ControllerBase
    {
        /// <summary>
        /// 日志
        /// </summary>
        private readonly ILogger<CcdFileUploadCompleteRecordController> _logger;
        /// <summary>
        /// 服务接口（CCD文件上传完成）
        /// </summary>
        private readonly ICcdFileUploadCompleteRecordService _ccdFileUploadCompleteRecordService;


        /// <summary>
        /// 构造函数（CCD文件上传完成）
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="ccdFileUploadCompleteRecordService"></param>
        public CcdFileUploadCompleteRecordController(ILogger<CcdFileUploadCompleteRecordController> logger, ICcdFileUploadCompleteRecordService ccdFileUploadCompleteRecordService)
        {
            _logger = logger;
            _ccdFileUploadCompleteRecordService = ccdFileUploadCompleteRecordService;
        }

        /// <summary>
        /// 添加（CCD文件上传完成）
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        public async Task AddAsync([FromBody] CcdFileUploadCompleteRecordSaveDto saveDto)
        {
             await _ccdFileUploadCompleteRecordService.CreateAsync(saveDto);
        }

        /// <summary>
        /// 更新（CCD文件上传完成）
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("update")]
        public async Task UpdateAsync([FromBody] CcdFileUploadCompleteRecordSaveDto saveDto)
        {
             await _ccdFileUploadCompleteRecordService.ModifyAsync(saveDto);
        }

        /// <summary>
        /// 删除（CCD文件上传完成）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete")]
        public async Task DeleteAsync([FromBody] long[] ids)
        {
            await _ccdFileUploadCompleteRecordService.DeletesAsync(ids);
        }

        /// <summary>
        /// 查询详情（CCD文件上传完成）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<CcdFileUploadCompleteRecordDto?> QueryByIdAsync(long id)
        {
            return await _ccdFileUploadCompleteRecordService.QueryByIdAsync(id);
        }

        /// <summary>
        /// 分页查询列表（CCD文件上传完成）
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("pagelist")]
        public async Task<PagedInfo<CcdFileUploadCompleteRecordDto>> QueryPagedListAsync([FromQuery] CcdFileUploadCompleteRecordPagedQueryDto pagedQueryDto)
        {
            return await _ccdFileUploadCompleteRecordService.GetPagedListAsync(pagedQueryDto);
        }

    }
}