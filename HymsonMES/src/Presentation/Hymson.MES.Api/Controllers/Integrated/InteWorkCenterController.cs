using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Common;
using Hymson.MES.Services.Dtos.Integrated;
using Hymson.MES.Services.Services.Integrated.IIntegratedService;
using Hymson.Web.Framework.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Integrated
{
    /// <summary>
    ///  工作中心控制器
    /// @author admin
    /// @date 2023-02-21
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class InteWorkCenterController : ControllerBase
    {
        private readonly IInteWorkCenterService _inteWorkCenterService;
        private readonly ILogger<InteWorkCenterController> _logger;

        /// <summary>
        /// 作业表控制器
        /// </summary>
        /// <param name="inteWorkCenterService"></param>
        /// <param name="logger"></param>
        public InteWorkCenterController(IInteWorkCenterService inteWorkCenterService, ILogger<InteWorkCenterController> logger)
        {
            _inteWorkCenterService = inteWorkCenterService;
            _logger = logger;
        }


        /// <summary>
        /// 分页查询列表
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("pagelist")]
        //[PermissionDescription("inte:workCenter:list")]
        public async Task<PagedInfo<InteWorkCenterDto>> QueryPagedInteWorkCenterAsync([FromQuery] InteWorkCenterPagedQueryDto param)
        {
            return await _inteWorkCenterService.GetPageListAsync(param);
        }

        /// <summary>
        /// 查询详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<InteWorkCenterDto> QueryInteWorkCenterByIdAsync(long id)
        {
            return await _inteWorkCenterService.QueryInteWorkCenterByIdAsync(id);
        }

        /// <summary>
        /// 查询关联资源
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}/resource")]
        public async Task<List<InteWorkCenterResourceRelationDto>> QueryInteWorkCenterResourceRelatioByIdAsync(long id)
        {
            return await _inteWorkCenterService.GetInteWorkCenterResourceRelatioByIdAsync(id);
        }

        /// <summary>
        /// 查询关联工作中心
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}/workCenter")]
        public async Task<List<InteWorkCenterRelationDto>> QueryInteWorkCenterRelationByIdAsync(long id)
        {
            return await _inteWorkCenterService.GetInteWorkCenterRelationByIdAsync(id);
        }

        /// <summary>
        /// 根据类型查询列表（工作中心）
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        [HttpGet("listByType")]
        public async Task<IEnumerable<SelectOptionDto>> GetListByTypeAndIdAsync([FromQuery] QueryInteWorkCenterByTypeAndParentIdDto queryDto)
        {
            return await _inteWorkCenterService.QueryListByTypeAndParentIdAsync(queryDto);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        [LogDescription("工作中心", BusinessType.INSERT)]
        [PermissionDescription("inte:workCenter:insert")]
        public async Task AddInteWorkCenterAsync([FromBody] InteWorkCenterCreateDto param)
        {
            await _inteWorkCenterService.CreateInteWorkCenterAsync(param);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPut]
        [LogDescription("工作中心", BusinessType.UPDATE)]
        [PermissionDescription("inte:workCenter:update")]
        public async Task UpdateInteWorkCenterAsync([FromBody] InteWorkCenterModifyDto param)
        {
            await _inteWorkCenterService.ModifyInteWorkCenterAsync(param);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        [LogDescription("工作中心", BusinessType.DELETE)]
        [PermissionDescription("inte:workCenter:delete")]
        public async Task DeleteInteWorkCenterAsync(long[] ids)
        {
            await _inteWorkCenterService.DeleteRangInteWorkCenterAsync(ids);
        }
    }
}