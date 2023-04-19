using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Common;
using Hymson.MES.Services.Dtos.Integrated;
using Hymson.MES.Services.Dtos.Process;
using Hymson.MES.Services.Services.Process.Resource;
using Hymson.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Hymson.MES.Api.Controllers
{
    /// <summary>
    /// ��Դά����Controller
    /// @tableName proc_resource
    /// @author zhaoqing
    /// @date 2023-02-08
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
        /// ��ѯ��Դ�����¹�������Դ
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
        public async Task<PagedInfo<ProcResourceDto>> GettPageListByProcedureIdAsync([FromQuery] ProcResourceProcedurePagedQueryDto query)
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
        /// �����Դ����
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task AddProcResourceAsync([FromBody] ProcResourceCreateDto parm)
        {
            await _procResourceService.AddProcResourceAsync(parm);
        }

        /// <summary>
        /// ������Դά����
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPut]
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
        public async Task DeleteProcResourceAsync(DeleteDto deleteDto)
        {
            await _procResourceService.DeleteProcResourceAsync(deleteDto.Ids);
        }
    }
}