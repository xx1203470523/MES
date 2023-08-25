using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Quality;
using Hymson.MES.Services.Services.Quality;
using Hymson.Web.Framework.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Quality
{
    /// <summary>
    /// 控制器（首检检验单）
    /// @author xiaofei
    /// @date 2023-08-21 06:10:55
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class QualIpqcInspectionHeadController : ControllerBase
    {
        /// <summary>
        /// 日志
        /// </summary>
        private readonly ILogger<QualIpqcInspectionHeadController> _logger;
        /// <summary>
        /// 服务接口（首检检验单）
        /// </summary>
        private readonly IQualIpqcInspectionHeadService _qualIpqcInspectionHeadService;


        /// <summary>
        /// 构造函数（首检检验单）
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="qualIpqcInspectionHeadService"></param>
        public QualIpqcInspectionHeadController(ILogger<QualIpqcInspectionHeadController> logger, IQualIpqcInspectionHeadService qualIpqcInspectionHeadService)
        {
            _logger = logger;
            _qualIpqcInspectionHeadService = qualIpqcInspectionHeadService;
        }

        /// <summary>
        /// 添加（首检检验单）
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        [PermissionDescription("quality:ipqcInspectionHead:insert")]
        public async Task AddAsync([FromBody] QualIpqcInspectionHeadSaveDto saveDto)
        {
            await _qualIpqcInspectionHeadService.CreateAsync(saveDto);
        }

        /// <summary>
        /// 删除（首检检验单）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete")]
        [PermissionDescription("quality:ipqcInspectionHead:delete")]
        public async Task DeleteAsync([FromBody] long[] ids)
        {
            await _qualIpqcInspectionHeadService.DeletesAsync(ids);
        }

        /// <summary>
        /// 查询详情（首检检验单）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<QualIpqcInspectionHeadDto?> QueryByIdAsync(long id)
        {
            return await _qualIpqcInspectionHeadService.QueryByIdAsync(id);
        }

        /// <summary>
        /// 分页查询列表（首检检验单）
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("pagelist")]
        public async Task<PagedInfo<QualIpqcInspectionHeadDto>> QueryPagedListAsync([FromQuery] QualIpqcInspectionHeadPagedQueryDto pagedQueryDto)
        {
            return await _qualIpqcInspectionHeadService.GetPagedListAsync(pagedQueryDto);
        }

        /// <summary>
        /// 执行检验
        /// </summary>
        /// <param name="updateDto"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("execute")]
        [PermissionDescription("quality:ipqcInspectionHead:execute")]
        public async Task ExecuteAsync([FromBody] StatusChangeDto updateDto)
        {
            await _qualIpqcInspectionHeadService.ExecuteAsync(updateDto);
        }

        /// <summary>
        /// 样品检验数据录入
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("sampleAdd")]
        [PermissionDescription("quality:ipqcInspectionHead:sampleAdd")]
        public async Task InsertSampleDataAsync([FromBody] List<QualIpqcInspectionHeadSampleCreateDto> dto)
        {
            await _qualIpqcInspectionHeadService.InsertSampleDataAsync(dto);
        }

        /// <summary>
        /// 样品检验数据修改
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("sampleUpdate")]
        [PermissionDescription("quality:ipqcInspectionHead:sampleUpdate")]
        public async Task UpdateSampleDataAsync([FromBody] QualIpqcInspectionHeadSampleUpdateDto dto)
        {
            await _qualIpqcInspectionHeadService.UpdateSampleDataAsync(dto);
        }

        /// <summary>
        /// 检验完成
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("complete")]
        [PermissionDescription("quality:ipqcInspectionHead:complete")]
        public async Task CompleteAsync([FromBody] StatusChangeDto dto)
        {
            await _qualIpqcInspectionHeadService.CompleteAsync(dto);
        }

        /// <summary>
        /// 不合格处理
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("unqualifiedHandle")]
        [PermissionDescription("quality:ipqcInspectionHead:unqualifiedHandle")]
        public async Task UnqualifiedHandleAsync([FromBody] UnqualifiedHandleDto dto)
        {
            await _qualIpqcInspectionHeadService.UnqualifiedHandleAsync(dto);
        }

        /// <summary>
        /// 附件上传
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("attachmentAdd")]
        [PermissionDescription("quality:ipqcInspectionHead:attachmentAdd")]
        public async Task AttachmentAddAsync([FromBody] AttachmentAddDto dto)
        {
            await _qualIpqcInspectionHeadService.AttachmentAddAsync(dto);
        }

        /// <summary>
        /// 附件删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("attachmentDelete")]
        [PermissionDescription("quality:ipqcInspectionHead:attachmentDelete")]
        public async Task AttachmentDeleteAsync([FromBody] long[] ids)
        {
            await _qualIpqcInspectionHeadService.AttachmentDeleteAsync(ids);
        }

    }
}