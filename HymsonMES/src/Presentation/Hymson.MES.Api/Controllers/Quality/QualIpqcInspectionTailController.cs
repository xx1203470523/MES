using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Quality;
using Hymson.MES.Services.Services.Quality;
using Hymson.Web.Framework.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Quality
{
    /// <summary>
    /// 控制器（尾检检验单）
    /// @author xiaofei
    /// @date 2023-08-24 10:52:02
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class QualIpqcInspectionTailController : ControllerBase
    {
        /// <summary>
        /// 日志
        /// </summary>
        private readonly ILogger<QualIpqcInspectionTailController> _logger;
        /// <summary>
        /// 服务接口（尾检检验单）
        /// </summary>
        private readonly IQualIpqcInspectionTailService _qualIpqcInspectionTailService;


        /// <summary>
        /// 构造函数（尾检检验单）
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="qualIpqcInspectionTailService"></param>
        public QualIpqcInspectionTailController(ILogger<QualIpqcInspectionTailController> logger, IQualIpqcInspectionTailService qualIpqcInspectionTailService)
        {
            _logger = logger;
            _qualIpqcInspectionTailService = qualIpqcInspectionTailService;
        }

        /// <summary>
        /// 添加（尾检检验单）
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        [PermissionDescription("quality:ipqcInspectionTail:insert")]
        public async Task AddAsync([FromBody] QualIpqcInspectionTailSaveDto saveDto)
        {
            await _qualIpqcInspectionTailService.CreateAsync(saveDto);
        }

        /// <summary>
        /// 删除（尾检检验单）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete")]
        [PermissionDescription("quality:ipqcInspectionTail:delete")]
        public async Task DeleteAsync([FromBody] long[] ids)
        {
            await _qualIpqcInspectionTailService.DeletesAsync(ids);
        }

        /// <summary>
        /// 查询详情（尾检检验单）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<QualIpqcInspectionTailDto?> QueryByIdAsync(long id)
        {
            return await _qualIpqcInspectionTailService.QueryByIdAsync(id);
        }

        /// <summary>
        /// 分页查询列表（尾检检验单）
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("pagelist")]
        public async Task<PagedInfo<QualIpqcInspectionTailDto>> QueryPagedListAsync([FromQuery] QualIpqcInspectionTailPagedQueryDto pagedQueryDto)
        {
            return await _qualIpqcInspectionTailService.GetPagedListAsync(pagedQueryDto);
        }

        /// <summary>
        /// 执行检验
        /// </summary>
        /// <param name="updateDto"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("execute")]
        [PermissionDescription("quality:ipqcInspectionTail:execute")]
        public async Task ExecuteAsync([FromBody] StatusChangeDto updateDto)
        {
            await _qualIpqcInspectionTailService.ExecuteAsync(updateDto);
        }

        /// <summary>
        /// 样品检验数据录入
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("sampleAdd")]
        [PermissionDescription("quality:ipqcInspectionTail:sampleAdd")]
        public async Task InsertSampleDataAsync([FromBody] List<QualIpqcInspectionTailSampleCreateDto> dto)
        {
            await _qualIpqcInspectionTailService.InsertSampleDataAsync(dto);
        }

        /// <summary>
        /// 样品检验数据修改
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("sampleUpdate")]
        [PermissionDescription("quality:ipqcInspectionTail:sampleUpdate")]
        public async Task UpdateSampleDataAsync([FromBody] QualIpqcInspectionTailSampleUpdateDto dto)
        {
            await _qualIpqcInspectionTailService.UpdateSampleDataAsync(dto);
        }

        /// <summary>
        /// 检验完成
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("complete")]
        [PermissionDescription("quality:ipqcInspectionTail:complete")]
        public async Task CompleteAsync([FromBody] StatusChangeDto dto)
        {
            await _qualIpqcInspectionTailService.CompleteAsync(dto);
        }

        /// <summary>
        /// 不合格处理
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("unqualifiedHandle")]
        [PermissionDescription("quality:ipqcInspectionTail:unqualifiedHandle")]
        public async Task UnqualifiedHandleAsync([FromBody] UnqualifiedHandleDto dto)
        {
            await _qualIpqcInspectionTailService.UnqualifiedHandleAsync(dto);
        }

        /// <summary>
        /// 附件上传
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("attachmentAdd")]
        [PermissionDescription("quality:ipqcInspectionTail:attachmentAdd")]
        public async Task AttachmentAddAsync([FromBody] AttachmentAddDto dto)
        {
            await _qualIpqcInspectionTailService.AttachmentAddAsync(dto);
        }

        /// <summary>
        /// 附件删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("attachmentDelete")]
        [PermissionDescription("quality:ipqcInspectionTail:attachmentDelete")]
        public async Task AttachmentDeleteAsync([FromBody] long[] ids)
        {
            await _qualIpqcInspectionTailService.AttachmentDeleteAsync(ids);
        }

    }
}