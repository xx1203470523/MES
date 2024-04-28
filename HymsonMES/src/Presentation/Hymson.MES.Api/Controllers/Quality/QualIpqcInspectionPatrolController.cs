using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Quality;
using Hymson.MES.Services.Services.Quality;
using Hymson.Web.Framework.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Quality
{
    /// <summary>
    /// 控制器（巡检检验单）
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class QualIpqcInspectionPatrolController : ControllerBase
    {
        /// <summary>
        /// 日志
        /// </summary>
        private readonly ILogger<QualIpqcInspectionPatrolController> _logger;
        /// <summary>
        /// 服务接口（巡检检验单）
        /// </summary>
        private readonly IQualIpqcInspectionPatrolService _qualIpqcInspectionPatrolService;


        /// <summary>
        /// 构造函数（巡检检验单）
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="qualIpqcInspectionPatrolService"></param>
        public QualIpqcInspectionPatrolController(ILogger<QualIpqcInspectionPatrolController> logger, IQualIpqcInspectionPatrolService qualIpqcInspectionPatrolService)
        {
            _logger = logger;
            _qualIpqcInspectionPatrolService = qualIpqcInspectionPatrolService;
        }

        /// <summary>
        /// 添加（巡检检验单）
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        [LogDescription("巡检检验单", BusinessType.INSERT)]
        //[PermissionDescription("quality:ipqcInspectionPatrol:insert")]
        public async Task AddAsync([FromBody] QualIpqcInspectionPatrolSaveDto saveDto)
        {
            await _qualIpqcInspectionPatrolService.CreateAsync(saveDto);
        }

        /// <summary>
        /// 删除（巡检检验单）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        [LogDescription("巡检检验单", BusinessType.DELETE)]
        [PermissionDescription("quality:qualIpqcInspectionPatrol:delete")]
        public async Task DeleteAsync([FromBody] long[] ids)
        {
            await _qualIpqcInspectionPatrolService.DeletesAsync(ids);
        }

        /// <summary>
        /// 查询详情（巡检检验单）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<QualIpqcInspectionPatrolDto?> QueryByIdAsync(long id)
        {
            return await _qualIpqcInspectionPatrolService.QueryByIdAsync(id);
        }

        /// <summary>
        /// 获取检验单已检样本列表
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        [HttpGet("samplelist")]
        public async Task<PagedInfo<QualIpqcInspectionPatrolSampleDto>> GetPagedSampleListAsync([FromQuery]  QualIpqcInspectionPatrolSamplePagedQueryDto pagedQueryDto)
        {
            return await _qualIpqcInspectionPatrolService.GetPagedSampleListAsync(pagedQueryDto);
        }

        /// <summary>
        /// 根据检验单ID获取检验单附件列表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("attachmentlist/{id}")]
        public async Task<IEnumerable<QualIpqcInspectionPatrolAnnexDto>?> GetAttachmentListAsync(long id)
        {
            return await _qualIpqcInspectionPatrolService.GetAttachmentListAsync(id);
        }

        /// <summary>
        /// 分页查询列表（巡检检验单）
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("pagelist")]
        public async Task<PagedInfo<QualIpqcInspectionPatrolDto>> QueryPagedListAsync([FromQuery] QualIpqcInspectionPatrolPagedQueryDto pagedQueryDto)
        {
            return await _qualIpqcInspectionPatrolService.GetPagedListAsync(pagedQueryDto);
        }

        /// <summary>
        /// 执行检验
        /// </summary>
        /// <param name="updateDto"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("execute")]
        [LogDescription("执行检验", BusinessType.OTHER)]
        //[PermissionDescription("quality:ipqcInspectionPatrol:execute")]
        public async Task ExecuteAsync([FromBody] StatusChangeDto updateDto)
        {
            await _qualIpqcInspectionPatrolService.ExecuteAsync(updateDto);
        }

        /// <summary>
        /// 样品检验数据录入
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("sampleAdd")]
        [LogDescription("样品检验数据录入", BusinessType.OTHER)]
        //[PermissionDescription("quality:ipqcInspectionPatrol:sampleAdd")]
        public async Task InsertSampleDataAsync([FromBody] List<QualIpqcInspectionPatrolSampleCreateDto> dto)
        {
            await _qualIpqcInspectionPatrolService.InsertSampleDataAsync(dto);
        }

        /// <summary>
        /// 样品检验数据修改
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("sampleUpdate")]
        [LogDescription("样品检验数据修改", BusinessType.OTHER)]
        //[PermissionDescription("quality:ipqcInspectionPatrol:sampleUpdate")]
        public async Task UpdateSampleDataAsync([FromBody] QualIpqcInspectionPatrolSampleUpdateDto dto)
        {
            await _qualIpqcInspectionPatrolService.UpdateSampleDataAsync(dto);
        }

        /// <summary>
        /// 检验完成
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("complete")]
        [LogDescription("检验完成", BusinessType.OTHER)]
        //[PermissionDescription("quality:ipqcInspectionPatrol:complete")]
        public async Task CompleteAsync([FromBody] StatusChangeDto dto)
        {
            await _qualIpqcInspectionPatrolService.CompleteAsync(dto);
        }

        /// <summary>
        /// 不合格处理
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("unqualifiedHandle")]
        [LogDescription("不合格处理", BusinessType.OTHER)]
        //[PermissionDescription("quality:ipqcInspectionPatrol:unqualifiedHandle")]
        public async Task UnqualifiedHandleAsync([FromBody] UnqualifiedHandleDto dto)
        {
            await _qualIpqcInspectionPatrolService.UnqualifiedHandleAsync(dto);
        }

        /// <summary>
        /// 附件上传
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("attachmentAdd")]
        [LogDescription("附件上传", BusinessType.OTHER)]
        //[PermissionDescription("quality:ipqcInspectionPatrol:attachmentAdd")]
        public async Task AttachmentAddAsync([FromBody] AttachmentAddDto dto)
        {
            await _qualIpqcInspectionPatrolService.AttachmentAddAsync(dto);
        }

        /// <summary>
        /// 附件删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("attachmentDelete")]
        [LogDescription("附件删除", BusinessType.OTHER)]
        //[PermissionDescription("quality:ipqcInspectionPatrol:attachmentDelete")]
        public async Task AttachmentDeleteAsync([FromBody] long[] ids)
        {
            await _qualIpqcInspectionPatrolService.AttachmentDeleteAsync(ids);
        }

        /// <summary>
        /// 查询检验单样品应检参数并校验
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("shouldInspectItems")]
        public async Task<IEnumerable<SampleShouldInspectItemsDto>?> GetSampleShouldInspectItemsAsync([FromQuery] SampleShouldInspectItemsQueryDto query)
        {
            return await _qualIpqcInspectionPatrolService.GetSampleShouldInspectItemsAsync(query);
        }

    }
}