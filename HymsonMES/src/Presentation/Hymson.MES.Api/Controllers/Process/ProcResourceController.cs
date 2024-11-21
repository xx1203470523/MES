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
    /// ��Դά����
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ProcResourceController : ControllerBase
    {
        /// <summary>
        /// ��Դά����ӿ�
        /// </summary>
        private readonly IProcResourceService _procResourceService;
        private readonly ILogger<ProcResourceController> _logger;

        /// <summary>
        /// ���캯��
        /// </summary>
        /// <param name="procResourceService"></param>
        /// <param name="logger"></param>
        public ProcResourceController(IProcResourceService procResourceService, ILogger<ProcResourceController> logger)
        {
            _procResourceService = procResourceService;
            _logger = logger;
        }

        /// <summary>
        /// ��ȡ��ҳ����
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
        /// 查询资源维护表列表(关联资源类型，展示资源类型名称);FQC检验马威，点击资源，显示弹出框资源列表所调接口
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [Route("listByScw")]
        [HttpGet]
        public async Task<PagedInfo<ProcResourceViewDto>> QueryProcResourceByScwAsync([FromQuery] ProcResourcePagedQueryDto query)
        {
            return await _procResourceService.GetPageListByScwAsync(query);
        }

        /// <summary>
        /// ��ȡ��ҳ����
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
        /// ��ѯ��Դά��������
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ProcResourceDto> GetProcResourceAsync(long id)
        {
            return await _procResourceService.GetByIdAsync(id);
        }


        /// <summary>
        /// ��ѯ��Դά��������
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet("workCenterLineresAndResTypeRources")]
        public async Task<PagedInfo<ProcResourceDto>> GetPageListBylineIdAndProcProcedureId([FromQuery] ProcResourcePagedlineIdAndProcProcedureIdDto param)
        {
            return await _procResourceService.GetPageListBylineIdAndProcProcedureIdAsync(param);
        }

        /// <summary>
        /// ��ѯ��Դ�����¹�    ������Դ
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
        /// ��ȡ��ҳ����
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
        /// ��Դ������ӡ������
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
        /// ��ȡ��Դ��������
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
        /// ��ȡ��Դ�����豸����
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
        /// ��ȡ��Դ������ҵ����
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
        /// ��ȡ��Դ������������
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
        /// ��ȡ������֤����
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
        /// �����Դ����
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPost]
        [LogDescription("��Դά��", BusinessType.INSERT)]
        [PermissionDescription("proc:resource:insert")]
        public async Task<long> AddProcResourceAsync([FromBody] ProcResourceCreateDto parm)
        {
            return await _procResourceService.AddProcResourceAsync(parm);
        }

        /// <summary>
        /// ������Դά����
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPut]
        [LogDescription("��Դά��", BusinessType.UPDATE)]
        [PermissionDescription("proc:resource:update")]
        public async Task UpdateProcResourceAsync([FromBody] ProcResourceModifyDto parm)
        {
            await _procResourceService.UpdateProcResrouceAsync(parm);
        }

        /// <summary>
        /// ɾ����Դά����
        /// </summary>
        /// <param name="deleteDto"></param>
        /// <returns></returns>
        [HttpDelete]
        [LogDescription("��Դά��", BusinessType.DELETE)]
        [PermissionDescription("proc:resource:delete")]
        public async Task DeleteProcResourceAsync(DeleteDto deleteDto)
        {
            await _procResourceService.DeleteProcResourceAsync(deleteDto.Ids);
        }

        /// <summary>
        /// ��ѯ��Դ�󶨵��豸
        /// </summary>
        /// <param name="resourceId"></param>
        /// <returns></returns>
        [HttpGet("equipments/{resourceId}")]
        public async Task<IEnumerable<SelectOptionDto>> QueryEquipmentsByResourceIdAsync(long resourceId)
        {
            return await _procResourceService.QueryEquipmentsByResourceIdAsync(resourceId);
        }

        #region ״̬���
        /// <summary>
        /// ���ã���Դά����
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("updateStatusEnable")]
        [LogDescription("��Դά��", BusinessType.UPDATE)]
        [PermissionDescription("proc:resource:updateStatusEnable")]
        public async Task UpdateStatusEnable([FromBody] long id)
        {
            await _procResourceService.UpdateStatusAsync(new ChangeStatusDto { Id = id, Status = SysDataStatusEnum.Enable });
        }

        /// <summary>
        /// ��������Դά����
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("updateStatusRetain")]
        [LogDescription("��Դά��", BusinessType.UPDATE)]
        [PermissionDescription("proc:resource:updateStatusRetain")]
        public async Task UpdateStatusRetain([FromBody] long id)
        {
            await _procResourceService.UpdateStatusAsync(new ChangeStatusDto { Id = id, Status = SysDataStatusEnum.Retain });
        }

        /// <summary>
        /// �ϳ�����Դά����
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("updateStatusAbolish")]
        [LogDescription("��Դά��", BusinessType.UPDATE)]
        [PermissionDescription("proc:resource:updateStatusAbolish")]
        public async Task UpdateStatusAbolish([FromBody] long id)
        {
            await _procResourceService.UpdateStatusAsync(new ChangeStatusDto { Id = id, Status = SysDataStatusEnum.Abolish });
        }

        #endregion
    }
}