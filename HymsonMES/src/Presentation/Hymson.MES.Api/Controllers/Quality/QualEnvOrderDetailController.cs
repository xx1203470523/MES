/*
 *creator: Karl
 *
 *describe: 环境检验单检验明细    控制器 | 代码由框架生成  
 *builder:  pengxin
 *build datetime: 2024-03-22 05:04:43
 */
using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.QualEnvOrderDetail;
using Hymson.MES.Services.Services.QualEnvOrderDetail;
using Hymson.Web.Framework.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.QualEnvOrderDetail
{
    /// <summary>
    /// 控制器（环境检验单检验明细）
    /// @author pengxin
    /// @date 2024-03-22 05:04:43
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class QualEnvOrderDetailController : ControllerBase
    {
        /// <summary>
        /// 接口（环境检验单检验明细）
        /// </summary>
        private readonly IQualEnvOrderDetailService _qualEnvOrderDetailService;
        private readonly ILogger<QualEnvOrderDetailController> _logger;

        /// <summary>
        /// 构造函数（环境检验单检验明细）
        /// </summary>
        /// <param name="qualEnvOrderDetailService"></param>
        public QualEnvOrderDetailController(IQualEnvOrderDetailService qualEnvOrderDetailService, ILogger<QualEnvOrderDetailController> logger)
        {
            _qualEnvOrderDetailService = qualEnvOrderDetailService;
            _logger = logger;
        }

        #region 框架生成方法

        /// <summary>
        /// 分页查询列表（环境检验单检验明细）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("pagelist")]
        public async Task<PagedInfo<QualEnvOrderDetailDto>> QueryPagedQualEnvOrderDetailAsync([FromQuery] QualEnvOrderDetailPagedQueryDto parm)
        {
            return await _qualEnvOrderDetailService.GetPagedListAsync(parm);
        }

        /// <summary>
        /// 查询详情（环境检验单检验明细）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<QualEnvOrderDetailDto> QueryQualEnvOrderDetailByIdAsync(long id)
        {
            return await _qualEnvOrderDetailService.QueryQualEnvOrderDetailByIdAsync(id);
        }

        /// <summary>
        /// 添加（环境检验单检验明细）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        [LogDescription("环境检验单检验明细", BusinessType.INSERT)]
        public async Task AddQualEnvOrderDetailAsync([FromBody] QualEnvOrderDetailCreateDto parm)
        {
            await _qualEnvOrderDetailService.CreateQualEnvOrderDetailAsync(parm);
        }

        /// <summary>
        /// 更新（环境检验单检验明细）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("update")]
        [LogDescription("环境检验单检验明细", BusinessType.UPDATE)]
        public async Task UpdateQualEnvOrderDetailAsync([FromBody] QualEnvOrderDetailModifyDto parm)
        {
            await _qualEnvOrderDetailService.ModifyQualEnvOrderDetailAsync(parm);
        }

        /// <summary>
        /// 更新（环境检验单检验明细）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("updates")]
        [LogDescription("环境检验单检验明细", BusinessType.UPDATE)]
        public async Task ModifyQualEnvOrderDetailsAsync([FromBody] List<QualEnvOrderDetailModifyDto> parm)
        {
            await _qualEnvOrderDetailService.ModifyQualEnvOrderDetailsAsync(parm);
        }

        /// <summary>
        /// 删除（环境检验单检验明细）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete")]
        [LogDescription("环境检验单检验明细", BusinessType.DELETE)]
        public async Task DeleteQualEnvOrderDetailAsync([FromBody] long[] ids)
        {
            await _qualEnvOrderDetailService.DeletesQualEnvOrderDetailAsync(ids);
        }

        #endregion

        #region
        /// <summary>
        /// 根据检验单ID获取数据
        /// </summary>
        /// <param name="envOrderId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("getDetailByEnvOrderId/{envOrderId}")]
        public async Task<IEnumerable<QualEnvOrderDetailExtendDto>> GetQualEnvOrderDetailByEnvOrderIdAsync(long envOrderId)
        {
            return await _qualEnvOrderDetailService.GetQualEnvOrderDetailByEnvOrderIdAsync(envOrderId);
        }

        #endregion
    }
}