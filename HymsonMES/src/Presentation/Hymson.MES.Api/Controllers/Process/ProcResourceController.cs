using Elastic.Clients.Elasticsearch.QueryDsl;
using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;
using Hymson.MES.CoreServices.Dtos.Common;
using Hymson.MES.Services.Dtos.Common;
using Hymson.MES.Services.Dtos.Integrated;
using Hymson.MES.Services.Dtos.Process;
using Hymson.MES.Services.Services.Process.Resource;
using Hymson.Web.Framework.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers
{
    /// <summary>
    /// 资源维护表
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ProcResourceController : ControllerBase
    {
        /// <summary>
        /// 资源维护表接口
        /// </summary>
        private readonly IProcResourceService _procResourceService;
        private readonly ILogger<ProcResourceController> _logger;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="procResourceService"></param>
        /// <param name="logger"></param>
        public ProcResourceController(IProcResourceService procResourceService, ILogger<ProcResourceController> logger)
        {
            _procResourceService = procResourceService;
            _logger = logger;
        }

        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [Route("list")]
        [HttpGet]
        public async Task<PagedInfo<ProcResourceViewDto>> QueryProcResourceAsync([FromQuery] ProcResourcePagedQueryDto query)
        {
            return await _procResourceService.GetPageListAsync(query);
        }

        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [Route("querylist")]
        [HttpGet]
        public async Task<PagedInfo<ProcResourceDto>> GetProcResourceListAsync([FromQuery] ProcResourcePagedQueryDto query)
        {
            return await _procResourceService.GetListAsync(query);
        }

        /// <summary>
        /// 查询资源维护表详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ProcResourceDto> GetProcResourceAsync(long id)
        {
            return await _procResourceService.GetByIdAsync(id);
        }


        /// <summary>
        /// 查询资源维护表详情
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet("workCenterLineresAndResTypeRources")]
        public async Task<PagedInfo<ProcResourceDto>> GetPageListBylineIdAndProcProcedureId([FromQuery] ProcResourcePagedlineIdAndProcProcedureIdDto param)
        {
            return await _procResourceService.GetPageListBylineIdAndProcProcedureIdAsync(param);
        }

        /// <summary>
        /// 查询资源类型下关    联的资源
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [Route("listforgroup")]
        [HttpGet]
        public async Task<List<ProcResourceDto>> GetListForGroupAsync([FromQuery] ProcResourcePagedQueryDto query)
        {
            return await _procResourceService.GetListForGroupAsync(query);
        }

        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [Route("listByProcedure")]
        [HttpGet]
        public async Task<PagedInfo<ProcResourceViewDto>> GettPageListByProcedureIdAsync([FromQuery] ProcResourceProcedurePagedQueryDto query)
        {
            return await _procResourceService.GettPageListByProcedureIdAsync(query);
        }

        /// <summary>
        /// 资源关联打印机数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [Route("print/list")]
        [HttpGet]
        public async Task<PagedInfo<ProcResourceConfigPrintViewDto>> GetResourceConfigPrintAsync([FromQuery] ProcResourceConfigPrintPagedQueryDto query)
        {
            return await _procResourceService.GetcResourceConfigPrintAsync(query);
        }

        /// <summary>
        /// 获取资源设置数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [Route("res/list")]
        [HttpGet]
        public async Task<PagedInfo<ProcResourceConfigResDto>> GetResourceConfigPrintAsync([FromQuery] ProcResourceConfigResPagedQueryDto query)
        {
            return await _procResourceService.GetcResourceConfigResAsync(query);
        }

        /// <summary>
        /// 获取资源关联设备数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [Route("equ/list")]
        [HttpGet]
        public async Task<PagedInfo<ProcResourceEquipmentBindViewDto>> GetResourceConfigEquAsync([FromQuery] ProcResourceEquipmentBindPagedQueryDto query)
        {
            return await _procResourceService.GetcResourceConfigEquAsync(query);
        }

        /// <summary>
        /// 获取资源关联作业数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [Route("job/list")]
        [HttpGet]
        public async Task<PagedInfo<ProcedureJobReleationDto>> GetProcedureConfigJobLisAsynct([FromQuery] InteJobBusinessRelationPagedQueryDto query)
        {
            return await _procResourceService.GetProcedureConfigJobListAsync(query);
        }

        /// <summary>
        /// 获取资源关联产出数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [Route("product/list")]
        [HttpGet]
        public async Task<IEnumerable<ProcProductSetDto>> GetResourceProductSetListAsync(ProcProductSetQueryDto query)
        {
            return await _procResourceService.GetResourceProductSetListAsync(query);
        }

        /// <summary>
        /// 获取资质认证设置
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("auth/{id}")]
        [HttpGet]
        public async Task<IEnumerable<ProcQualificationAuthenticationDto>> GetResourceAuthSetListAsync(long id)
        {
            return await _procResourceService.GetResourceAuthSetListAsync(id);
        }

        /// <summary>
        /// 添加资源数据
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPost]
        [LogDescription("资源维护", BusinessType.INSERT)]
        [PermissionDescription("proc:resource:insert")]
        public async Task<long> AddProcResourceAsync([FromBody] ProcResourceCreateDto parm)
        {
            return await _procResourceService.AddProcResourceAsync(parm);
        }

        /// <summary>
        /// 更新资源维护表
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPut]
        [LogDescription("资源维护", BusinessType.UPDATE)]
        [PermissionDescription("proc:resource:update")]
        public async Task UpdateProcResourceAsync([FromBody] ProcResourceModifyDto parm)
        {
            await _procResourceService.UpdateProcResrouceAsync(parm);
        }

        /// <summary>
        /// 删除资源维护表
        /// </summary>
        /// <param name="deleteDto"></param>
        /// <returns></returns>
        [HttpDelete]
        [LogDescription("资源维护", BusinessType.DELETE)]
        [PermissionDescription("proc:resource:delete")]
        public async Task DeleteProcResourceAsync(DeleteDto deleteDto)
        {
            await _procResourceService.DeleteProcResourceAsync(deleteDto.Ids);
        }

        /// <summary>
        /// 查询资源绑定的设备
        /// </summary>
        /// <param name="resourceId"></param>
        /// <returns></returns>
        [HttpGet("equipments/{resourceId}")]
        public async Task<IEnumerable<SelectOptionDto>> QueryEquipmentsByResourceIdAsync(long resourceId)
        {
            return await _procResourceService.QueryEquipmentsByResourceIdAsync(resourceId);
        }

        #region 状态变更
        /// <summary>
        /// 启用（资源维护）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("updateStatusEnable")]
        [LogDescription("资源维护", BusinessType.UPDATE)]
        [PermissionDescription("proc:resource:updateStatusEnable")]
        public async Task UpdateStatusEnable([FromBody] long id)
        {
            await _procResourceService.UpdateStatusAsync(new ChangeStatusDto { Id = id, Status = SysDataStatusEnum.Enable });
        }

        /// <summary>
        /// 保留（资源维护）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("updateStatusRetain")]
        [LogDescription("资源维护", BusinessType.UPDATE)]
        [PermissionDescription("proc:resource:updateStatusRetain")]
        public async Task UpdateStatusRetain([FromBody] long id)
        {
            await _procResourceService.UpdateStatusAsync(new ChangeStatusDto { Id = id, Status = SysDataStatusEnum.Retain });
        }

        /// <summary>
        /// 废除（资源维护）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("updateStatusAbolish")]
        [LogDescription("资源维护", BusinessType.UPDATE)]
        [PermissionDescription("proc:resource:updateStatusAbolish")]
        public async Task UpdateStatusAbolish([FromBody] long id)
        {
            await _procResourceService.UpdateStatusAsync(new ChangeStatusDto { Id = id, Status = SysDataStatusEnum.Abolish });
        }

        #endregion
    }
}