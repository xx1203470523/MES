using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Common;
using Hymson.MES.Services.Dtos.Process;
using Hymson.MES.Services.Services.Process.ResourceType;
using Hymson.Web.Framework.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers
{
    /// <summary>
    /// ��Դ����ά����
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ProcResourceTypeController : ControllerBase
    {
        /// <summary>
        /// ��Դ����ά����ӿ�
        /// </summary>
        private readonly IProcResourceTypeService _procResourceTypeService;
        private readonly ILogger<ProcResourceTypeController> _logger;

        /// <summary>
        /// ���캯��
        /// </summary>
        /// <param name="procResourceTypeService"></param>
        /// <param name="logger"></param>
        public ProcResourceTypeController(IProcResourceTypeService procResourceTypeService, ILogger<ProcResourceTypeController> logger)
        {
            _procResourceTypeService = procResourceTypeService;
            _logger = logger;
        }

        /// <summary>
        /// ��ȡ��ҳ����
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [Route("list")]
        [HttpGet]
        public async Task<PagedInfo<ProcResourceTypeViewDto>> QueryProcResourceTypeAsync([FromQuery] ProcResourceTypePagedQueryDto query)
        {
            return await _procResourceTypeService.GetPageListAsync(query);
        }

        /// <summary>
        /// ��ȡ��ҳ����
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [Route("querylist")]
        [HttpGet]
        public async Task<PagedInfo<ProcResourceTypeDto>> GetProcResourceTypeListAsync([FromQuery] ProcResourceTypePagedQueryDto query)
        {
            return await _procResourceTypeService.GetListAsync(query);
        }

        /// <summary>
        /// ��ѯ��Դ����ά��������
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ProcResourceTypeDto> GetProcResourceTypeAsync(long id)
        {
            return await _procResourceTypeService.GetListAsync(id);
        }

        /// <summary>
        /// �����Դ��������
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPost]
        [LogDescription("��Դ����ά��", BusinessType.INSERT)]
        [PermissionDescription("proc:resourceType:insert")]
        public async Task<long> AddProcResourceTypeAsync([FromBody] ProcResourceTypeAddDto parm)
        {
            return await _procResourceTypeService.AddProcResourceTypeAsync(parm);
        }

        /// <summary>
        /// ������Դ����ά����
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [HttpPut]
        [LogDescription("��Դ����ά��", BusinessType.UPDATE)]
        [PermissionDescription("proc:resourceType:update")]
        public async Task UpdateProcResourceTypeAsync([FromBody] ProcResourceTypeUpdateDto parm)
        {
            await _procResourceTypeService.UpdateProcResrouceTypeAsync(parm);
        }

        /// <summary>
        /// ɾ����Դ����ά����
        /// </summary>
        /// <param name="deleteDto"></param>
        /// <returns></returns>
        [HttpDelete]
        [LogDescription("��Դ����ά��", BusinessType.DELETE)]
        [PermissionDescription("proc:resourceType:delete")]
        public async Task DeleteProcResourceTypeAsync(DeleteDto deleteDto)
        {
            await _procResourceTypeService.DeleteProcResourceTypeAsync(deleteDto.Ids);
        }
    }
}