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
        [LogDescription("尾检检验单", BusinessType.INSERT)]
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
        [LogDescription("尾检检验单", BusinessType.DELETE)]
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
        /// 获取检验单已检样本列表
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        [HttpGet("samplelist")]
        public async Task<PagedInfo<QualIpqcInspectionTailSampleDto>> GetPagedSampleListAsync([FromQuery]QualIpqcInspectionTailSamplePagedQueryDto pagedQueryDto)
        {
            return await _qualIpqcInspectionTailService.GetPagedSampleListAsync(pagedQueryDto);
        }

        /// <summary>
        /// 根据检验单ID获取检验单附件列表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("attachmentlist/{id}")]
        public async Task<IEnumerable<QualIpqcInspectionTailAnnexDto>?> GetAttachmentListAsync(long id)
        {
            return await _qualIpqcInspectionTailService.GetAttachmentListAsync(id);
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
        [LogDescription("执行检验", BusinessType.OTHER)]
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
        [LogDescription("样品检验数据录入", BusinessType.OTHER)]
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
        [LogDescription("样品检验数据修改", BusinessType.OTHER)]
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
        [LogDescription("检验完成", BusinessType.OTHER)]
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
        [LogDescription("不合格处理", BusinessType.OTHER)]
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
        [LogDescription("附件上传", BusinessType.OTHER)]
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
        [LogDescription("附件删除", BusinessType.OTHER)]
        [PermissionDescription("quality:ipqcInspectionTail:attachmentDelete")]
        public async Task AttachmentDeleteAsync([FromBody] long[] ids)
        {
            await _qualIpqcInspectionTailService.AttachmentDeleteAsync(ids);
        }

        /// <summary>
        /// 查询检验单样品应检参数并校验
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("shouldInspectItems")]
        public async Task<IEnumerable<SampleShouldInspectItemsDto>?> GetSampleShouldInspectItemsAsync([FromQuery] SampleShouldInspectItemsQueryDto query)
        {
            return await _qualIpqcInspectionTailService.GetSampleShouldInspectItemsAsync(query);
        }

    }
}