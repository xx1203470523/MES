using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Process;
using Hymson.MES.Services.Services.Process.MaskCode;
using Hymson.Web.Framework.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Process
{
    /// <summary>
    /// ������������ά����
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ProcMaskCodeController : ControllerBase
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly IProcMaskCodeService _procMaskCodeService;
        private readonly ILogger<ProcMaskCodeController> _logger;

        /// <summary>
        /// ���캯��������ά����
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="procMaskCodeService"></param>
        public ProcMaskCodeController(ILogger<ProcMaskCodeController> logger, IProcMaskCodeService procMaskCodeService)
        {
            _procMaskCodeService = procMaskCodeService;
            _logger = logger;
        }

        /// <summary>
        /// ��ӣ�����ά����
        /// </summary>
        /// <param name="createDto"></param>
        /// <returns></returns>
        [HttpPost]
        [LogDescription("����ά��", BusinessType.INSERT)]
        [PermissionDescription("proc:maskCode:insert")]
        public async Task CreateAsync(ProcMaskCodeSaveDto createDto)
        {
            await _procMaskCodeService.CreateAsync(createDto);
        }

        /// <summary>
        /// ���£�����ά����
        /// </summary>
        /// <param name="modifyDto"></param>
        /// <returns></returns>
        [HttpPut]
        [LogDescription("����ά��", BusinessType.UPDATE)]
        [PermissionDescription("proc:maskCode:update")]
        public async Task ModifyAsync(ProcMaskCodeSaveDto modifyDto)
        {
            await _procMaskCodeService.ModifyAsync(modifyDto);
        }

        /// <summary>
        /// ɾ��������ά����
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        [LogDescription("����ά��", BusinessType.DELETE)]
        [PermissionDescription("proc:maskCode:delete")]
        public async Task DeletesAsync(long[] ids)
        {
            await _procMaskCodeService.DeletesAsync(ids);
        }

        /// <summary>
        /// ��ȡ��ҳ���ݣ�����ά����
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        [Route("page")]
        [HttpGet]
        public async Task<PagedInfo<ProcMaskCodeDto>> GetPagedListAsync([FromQuery] ProcMaskCodePagedQueryDto pagedQueryDto)
        {
            return await _procMaskCodeService.GetPagedListAsync(pagedQueryDto);
        }

        /// <summary>
        /// ��ѯ���飨����ά����
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ProcMaskCodeDto> GetDetailAsync(long id)
        {
            return await _procMaskCodeService.GetDetailAsync(id);
        }
    }
}