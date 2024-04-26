using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;
using Hymson.MES.Services.Dtos.Common;
using Hymson.MES.Services.Dtos.Process;
using Hymson.MES.Services.Services.Process.ProcessRoute;
using Hymson.Web.Framework.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Process
{
    /// <summary>
    /// 控制器（工艺路线表）
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ProcProcessRouteController : ControllerBase
    {
        /// <summary>
        /// 接口（工艺路线表）
        /// </summary>
        private readonly IProcProcessRouteService _procProcessRouteService;
        private readonly ILogger<ProcProcessRouteController> _logger;

        /// <summary>
        /// 构造函数（工艺路线表）
        /// </summary>
        /// <param name="procProcessRouteService"></param>
        /// <param name="logger"></param>
        public ProcProcessRouteController(IProcProcessRouteService procProcessRouteService, ILogger<ProcProcessRouteController> logger)
        {
            _procProcessRouteService = procProcessRouteService;
            _logger = logger;
        }


        /// <summary>
        /// 分页查询列表（工艺路线表）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("list")]
        public async Task<PagedInfo<ProcProcessRouteDto>> QueryPagedProcProcessRouteAsync([FromQuery] ProcProcessRoutePagedQueryDto parm)
        {
            return await _procProcessRouteService.GetPageListAsync(parm);
        }

        /// <summary>
        /// 查询详情（工艺路线表）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<CustomProcessRouteDto> GetCustomProcProcessRouteAsync(long id)
        {
            return await _procProcessRouteService.GetCustomProcProcessRouteAsync(id);
        }

        /// <summary>
        /// 根据id查询不合格工艺路线列表
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("listByIds")]
        [LogDescription("根据id查询不合格工艺路线列表", BusinessType.INSERT)]
        public async Task<IEnumerable<ProcProcessRouteDto>> GetListByIdsAsync(ProcProcessRouteQueryDto queryDto)
        {
            return await _procProcessRouteService.GetListByIdsAsync(queryDto.Ids);
        }

        /// <summary>
        /// 根据ID查询工艺路线工序列表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("nodeList/{id}")]
        public async Task<IEnumerable<ProcProcessRouteDetailNodeViewDto>> GetNodesByRouteId(long id)
        {
            return await _procProcessRouteService.GetNodesByRouteIdAsync(id);
        }

        /// <summary>
        /// 分页查询工艺路线的工序列表
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("procedureList")]    
        public async Task<PagedInfo<ProcProcedureDto>> GetProcedureListByRouteId([FromQuery] ProcessRouteProcedureQueryDto parm)
        {
            return await _procProcessRouteService.GetPagedInfoByProcessRouteIdAsync(parm);
        }

        /// <summary>
        /// 添加（工艺路线表）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPost]
        [LogDescription("工艺路线", BusinessType.INSERT)]
        [PermissionDescription("proc:processRoute:insert")]
        public async Task<long> AddProcProcessRouteAsync([FromBody] ProcProcessRouteCreateDto parm)
        {
           return  await _procProcessRouteService.AddProcProcessRouteAsync(parm);
        }

        /// <summary>
        /// 更新（工艺路线表）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPut]
        [LogDescription("工艺路线", BusinessType.UPDATE)]
        [PermissionDescription("proc:processRoute:update")]
        public async Task UpdateProcProcessRouteAsync([FromBody] ProcProcessRouteModifyDto parm)
        {
            await _procProcessRouteService.UpdateProcProcessRouteAsync(parm);
        }

        /// <summary>
        /// 删除（工艺路线表）
        /// </summary>
        /// <param name="deleteDto"></param>
        /// <returns></returns>
        [HttpDelete]
        [LogDescription("工艺路线", BusinessType.DELETE)]
        [PermissionDescription("proc:processRoute:delete")]
        public async Task DeleteProcProcessRouteAsync(DeleteDto deleteDto)
        {
            await _procProcessRouteService.DeleteProcProcessRouteAsync(deleteDto.Ids);
        }

        #region 状态变更
        /// <summary>
        /// 启用（工艺路线）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("updateStatusEnable")]
        [LogDescription("工艺路线", BusinessType.UPDATE)]
        [PermissionDescription("proc:processRoute:updateStatusEnable")]
        public async Task UpdateStatusEnable([FromBody] long id)
        {
            await _procProcessRouteService.UpdateStatusAsync(new ChangeStatusDto { Id = id, Status = SysDataStatusEnum.Enable });
        }

        /// <summary>
        /// 保留（工艺路线）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("updateStatusRetain")]
        [LogDescription("工艺路线", BusinessType.UPDATE)]
        [PermissionDescription("proc:processRoute:updateStatusRetain")]
        public async Task UpdateStatusRetain([FromBody] long id)
        {
            await _procProcessRouteService.UpdateStatusAsync(new ChangeStatusDto { Id = id, Status = SysDataStatusEnum.Retain });
        }

        /// <summary>
        /// 废除（工艺路线）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("updateStatusAbolish")]
        [LogDescription("工艺路线", BusinessType.UPDATE)]
        [PermissionDescription("proc:processRoute:updateStatusAbolish")]
        public async Task UpdateStatusAbolish([FromBody] long id)
        {
            await _procProcessRouteService.UpdateStatusAsync(new ChangeStatusDto { Id = id, Status = SysDataStatusEnum.Abolish });
        }

        #endregion
    }
}