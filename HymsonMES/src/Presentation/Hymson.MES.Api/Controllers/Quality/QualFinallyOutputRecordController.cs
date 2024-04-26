using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Quality;
using Hymson.MES.Services.Services.Quality;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Quality
{
    /// <summary>
    /// 控制器（成品条码产出记录(FQC生成使用)）
    /// @author Jam
    /// @date 2024-04-01 07:19:29
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class QualFinallyOutputRecordController : ControllerBase
    {
        /// <summary>
        /// 日志
        /// </summary>
        private readonly ILogger<QualFinallyOutputRecordController> _logger;
        /// <summary>
        /// 服务接口（成品条码产出记录(FQC生成使用)）
        /// </summary>
        private readonly IQualFinallyOutputRecordService _qualFinallyOutputRecordService;


        /// <summary>
        /// 构造函数（成品条码产出记录(FQC生成使用)）
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="qualFinallyOutputRecordService"></param>
        public QualFinallyOutputRecordController(ILogger<QualFinallyOutputRecordController> logger, IQualFinallyOutputRecordService qualFinallyOutputRecordService)
        {
            _logger = logger;
            _qualFinallyOutputRecordService = qualFinallyOutputRecordService;
        }

        /// <summary>
        /// 添加（成品条码产出记录(FQC生成使用)）
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        public async Task AddAsync([FromBody] QualFinallyOutputRecordSaveDto saveDto)
        {
            await _qualFinallyOutputRecordService.CreateAsync(saveDto);
        }

        /// <summary>
        /// 更新（成品条码产出记录(FQC生成使用)）
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("update")]
        public async Task UpdateAsync([FromBody] QualFinallyOutputRecordSaveDto saveDto)
        {
            await _qualFinallyOutputRecordService.ModifyAsync(saveDto);
        }

        /// <summary>
        /// 删除（成品条码产出记录(FQC生成使用)）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete")]
        public async Task DeleteAsync([FromBody] long[] ids)
        {
            await _qualFinallyOutputRecordService.DeletesAsync(ids);
        }

        /// <summary>
        /// 查询详情（成品条码产出记录(FQC生成使用)）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<QualFinallyOutputRecordDto?> QueryByIdAsync(long id)
        {
            return await _qualFinallyOutputRecordService.QueryByIdAsync(id);
        }

        /// <summary>
        /// 查询详情（成品条码产出记录(FQC生成使用)）
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("finallyRecord")]
        public async Task<PagedInfo<QualFinallyOutputRecordView>?> QueryBySFCAsync([FromQuery] FQCInspectionSFCQueryDto query)
        {
            return await _qualFinallyOutputRecordService.QueryBySFCAsync(query);
        }

        /// <summary>
        /// 查询详情（成品条码产出记录(FQC生成使用)）
        /// </summary>
        /// <param name="sfc"></param>
        /// <returns></returns>
        [HttpGet("finallyRecordBarCode/{sfc}")]
        public async Task<QualFinallyOutputRecordView?> FinallyRecordBarCodeAsync(string sfc)
        {
            return await _qualFinallyOutputRecordService.QueryBySFCFirstAsync(sfc);
        }

        /// <summary>
        /// 分页查询列表（成品条码产出记录(FQC生成使用)）
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("pagelist")]
        public async Task<PagedInfo<QualFinallyOutputRecordDto>> QueryPagedListAsync([FromQuery] QualFinallyOutputRecordPagedQueryDto pagedQueryDto)
        {
            return await _qualFinallyOutputRecordService.GetPagedListAsync(pagedQueryDto);
        }

    }
}