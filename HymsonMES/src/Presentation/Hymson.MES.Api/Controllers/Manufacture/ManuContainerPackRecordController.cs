using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Manufacture;
using Hymson.MES.Services.Services.Manufacture;
using Hymson.Web.Framework.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Manufacture
{
    /// <summary>
    /// 控制器（容器装载记录）
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ManuContainerPackRecordController : ControllerBase
    {
        /// <summary>
        /// 接口（容器装载记录）
        /// </summary>
        private readonly IManuContainerPackRecordService _manuContainerPackRecordService;
        private readonly ILogger<ManuContainerPackRecordController> _logger;

        /// <summary>
        /// 构造函数（容器装载记录）
        /// </summary>
        /// <param name="manuContainerPackRecordService"></param>
        /// <param name="logger"></param>
        public ManuContainerPackRecordController(IManuContainerPackRecordService manuContainerPackRecordService, ILogger<ManuContainerPackRecordController> logger)
        {
            _manuContainerPackRecordService = manuContainerPackRecordService;
            _logger = logger;
        }

        #region 框架生成方法

        /// <summary>
        /// 分页查询列表（容器装载记录）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("pagelist")]
        public async Task<PagedInfo<ManuContainerPackRecordDto>> QueryPagedManuContainerPackRecordAsync([FromQuery] ManuContainerPackRecordPagedQueryDto parm)
        {
            return await _manuContainerPackRecordService.GetPagedListAsync(parm);
        }

        /// <summary>
        /// 查询详情（容器装载记录）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ManuContainerPackRecordDto> QueryManuContainerPackRecordByIdAsync(long id)
        {
            return await _manuContainerPackRecordService.QueryManuContainerPackRecordByIdAsync(id);
        }

        /// <summary>
        /// 添加（容器装载记录）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        [LogDescription("容器装载记录", BusinessType.INSERT)]
        public async Task AddManuContainerPackRecordAsync([FromBody] ManuContainerPackRecordCreateDto parm)
        {
             await _manuContainerPackRecordService.CreateManuContainerPackRecordAsync(parm);
        }

        /// <summary>
        /// 更新（容器装载记录）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("update")]
        [LogDescription("容器装载记录", BusinessType.UPDATE)]
        public async Task UpdateManuContainerPackRecordAsync([FromBody] ManuContainerPackRecordModifyDto parm)
        {
             await _manuContainerPackRecordService.ModifyManuContainerPackRecordAsync(parm);
        }

        /// <summary>
        /// 删除（容器装载记录）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete")]
        [LogDescription("容器装载记录", BusinessType.DELETE)]
        public async Task DeleteManuContainerPackRecordAsync([FromBody] long[] ids)
        {
            await _manuContainerPackRecordService.DeletesManuContainerPackRecordAsync(ids);
        }

        #endregion
    }
}