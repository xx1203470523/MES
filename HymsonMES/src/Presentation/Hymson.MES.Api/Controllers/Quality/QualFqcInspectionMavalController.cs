/*
 *creator: Karl
 *
 *describe: 马威FQC检验    控制器 | 代码由框架生成  
 *builder:  pengxin
 *build datetime: 2024-07-24 03:09:40
 */
using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.QualFqcInspectionMaval;
using Hymson.MES.Services.Dtos.Integrated;
using Hymson.MES.Services.Dtos.QualFqcInspectionMaval;
using Hymson.MES.Services.Services.QualFqcInspectionMaval;
using Hymson.Web.Framework.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.QualFqcInspectionMaval
{
    /// <summary>
    /// 控制器（马威FQC检验）
    /// @author pengxin
    /// @date 2024-07-24 03:09:40
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class QualFqcInspectionMavalController : ControllerBase
    {
        /// <summary>
        /// 接口（马威FQC检验）
        /// </summary>
        private readonly IQualFqcInspectionMavalService _qualFqcInspectionMavalService;
        private readonly ILogger<QualFqcInspectionMavalController> _logger;

        /// <summary>
        /// 构造函数（马威FQC检验）
        /// </summary>
        /// <param name="qualFqcInspectionMavalService"></param>
        public QualFqcInspectionMavalController(IQualFqcInspectionMavalService qualFqcInspectionMavalService, ILogger<QualFqcInspectionMavalController> logger)
        {
            _qualFqcInspectionMavalService = qualFqcInspectionMavalService;
            _logger = logger;
        }

        #region 框架生成方法

        /// <summary>
        /// 分页查询列表（马威FQC检验）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("pagelist")]
        public async Task<PagedInfo<QualFqcInspectionMavalDto>> QueryPagedQualFqcInspectionMavalAsync([FromQuery] QualFqcInspectionMavalPagedQueryDto parm)
        {
            return await _qualFqcInspectionMavalService.GetPagedListAsync(parm);
        }

        /// <summary>
        /// 查询详情（马威FQC检验）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<QualFqcInspectionMavalDto> QueryQualFqcInspectionMavalByIdAsync(long id)
        {
            return await _qualFqcInspectionMavalService.QueryQualFqcInspectionMavalByIdAsync(id);
        }

        /// <summary>
        /// 添加（马威FQC检验）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        public async Task AddQualFqcInspectionMavalAsync([FromBody] QualFqcInspectionMavalCreateDto parm)
        {
            await _qualFqcInspectionMavalService.CreateQualFqcInspectionMavalAsync(parm);
        }

        /// <summary>
        /// 更新（马威FQC检验）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("update")]
        public async Task UpdateQualFqcInspectionMavalAsync([FromBody] QualFqcInspectionMavalModifyDto parm)
        {
            await _qualFqcInspectionMavalService.ModifyQualFqcInspectionMavalAsync(parm);
        }

        /// <summary>
        /// 删除（马威FQC检验）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete")]
        public async Task DeleteQualFqcInspectionMavalAsync([FromBody] long[] ids)
        {
            await _qualFqcInspectionMavalService.DeletesQualFqcInspectionMavalAsync(ids);
        }

        #endregion

        #region 

        /// <summary>
        /// 上传单据附件
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost("attachment/save")]
        [LogDescription("上传单据附件", BusinessType.EXPORT)]
        public async Task SaveAttachmentAsync([FromBody] QualFqcInspectionMavalSaveAttachmentDto dto)
        {
            await _qualFqcInspectionMavalService.SaveAttachmentAsync(dto);
        }

        /// <summary>
        /// 删除单据附件
        /// </summary>
        /// <param name="orderAnnexId"></param>
        /// <returns></returns>
        [HttpDelete("attachment/delete/{orderAnnexId}")]
        [LogDescription("删除单据附件", BusinessType.OTHER)]
        public async Task DeleteAttachmentByIdAsync(long orderAnnexId)
        {
            await _qualFqcInspectionMavalService.DeleteAttachmentByIdAsync(orderAnnexId);
        }

        /// <summary>
        /// 查询单据附件
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpGet("attachment/attachmentList")]
        public async Task<IEnumerable<InteAttachmentBaseDto>> QueryAttachmentListByIdAsync([FromQuery] QualFqcInspectionMavalAttachmentDto dto)
        {
            return await _qualFqcInspectionMavalService.QueryOrderAttachmentListByIdAsync(dto);
        }

  

        /// <summary>
        /// 获取Id
        /// </summary>
        /// <returns></returns>
        [HttpGet("newId")]
        public long GetNewId()
        {
            return _qualFqcInspectionMavalService.GetNewId();
        }
        #endregion
    }
}