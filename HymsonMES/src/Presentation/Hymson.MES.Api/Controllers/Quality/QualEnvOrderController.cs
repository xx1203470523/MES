/*
 *creator: Karl
 *
 *describe: 环境检验单    控制器 | 代码由框架生成  
 *builder:  pengxin
 *build datetime: 2024-03-22 05:04:53
 */
using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.QualEnvOrder;
using Hymson.MES.Services.Services.QualEnvOrder;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.QualEnvOrder
{
    /// <summary>
    /// 控制器（环境检验单）
    /// @author pengxin
    /// @date 2024-03-22 05:04:53
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class QualEnvOrderController : ControllerBase
    {
        /// <summary>
        /// 接口（环境检验单）
        /// </summary>
        private readonly IQualEnvOrderService _qualEnvOrderService;
        private readonly ILogger<QualEnvOrderController> _logger;

        /// <summary>
        /// 构造函数（环境检验单）
        /// </summary>
        /// <param name="qualEnvOrderService"></param>
        public QualEnvOrderController(IQualEnvOrderService qualEnvOrderService, ILogger<QualEnvOrderController> logger)
        {
            _qualEnvOrderService = qualEnvOrderService;
            _logger = logger;
        }

        #region 框架生成方法

        /// <summary>
        /// 分页查询列表（环境检验单）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("pagelist")]
        public async Task<PagedInfo<QualEnvOrderDto>> QueryPagedQualEnvOrderAsync([FromQuery] QualEnvOrderPagedQueryDto parm)
        {
            return await _qualEnvOrderService.GetPagedListAsync(parm);
        }

        /// <summary>
        /// 查询详情（环境检验单）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<QualEnvOrderDto> QueryQualEnvOrderByIdAsync(long id)
        {
            return await _qualEnvOrderService.QueryQualEnvOrderByIdAsync(id);
        }

        /// <summary>
        /// 根据ID查询关联信息（环境检验单）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("relatesInfo/{id}")]
        public async Task<QualEnvOrderDto> QueryQualEnvOrderRelatesInfoByIdAsync(long id)
        {
            return await _qualEnvOrderService.QueryQualEnvOrderRelatesInfoByIdAsync(id);
        }

        /// <summary>
        /// 添加（环境检验单）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        public async Task AddQualEnvOrderAsync([FromBody] QualEnvOrderCreateConvertDto parm)
        {
            await _qualEnvOrderService.QualEnvOrderCreateConvert(parm);
        }

        /// <summary>
        /// 更新（环境检验单）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("update")]
        public async Task UpdateQualEnvOrderAsync([FromBody] QualEnvOrderModifyDto parm)
        {
            await _qualEnvOrderService.ModifyQualEnvOrderAsync(parm);
        }

        /// <summary>
        /// 删除（环境检验单）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete")]
        public async Task DeleteQualEnvOrderAsync([FromBody] long[] ids)
        {
            await _qualEnvOrderService.DeletesQualEnvOrderAsync(ids);
        }

        #endregion
    }
}