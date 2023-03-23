using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Integrated;
using Hymson.MES.Services.Services.Integrated.InteContainer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Integrated
{
    /// <summary>
    /// ������������ά����
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class InteContainerController : ControllerBase
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly IInteContainerService _inteContainerService;
        private readonly ILogger<InteContainerController> _logger;

        /// <summary>
        /// ���캯��������ά����
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="inteContainerService"></param>
        public InteContainerController(ILogger<InteContainerController> logger, IInteContainerService inteContainerService)
        {
            _inteContainerService = inteContainerService;
            _logger = logger;
        }

        /// <summary>
        /// ��ӣ�����ά����
        /// </summary>
        /// <param name="createDto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task CreateAsync(InteContainerSaveDto createDto)
        {
            await _inteContainerService.CreateAsync(createDto);
        }

        /// <summary>
        /// ���£�����ά����
        /// </summary>
        /// <param name="modifyDto"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task ModifyAsync(InteContainerSaveDto modifyDto)
        {
            await _inteContainerService.ModifyAsync(modifyDto);
        }

        /// <summary>
        /// ɾ��������ά����
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task DeletesAsync(long[] ids)
        {
            await _inteContainerService.DeletesAsync(ids);
        }

        /// <summary>
        /// ��ȡ��ҳ���ݣ�����ά����
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        [Route("page")]
        [HttpGet]
        public async Task<PagedInfo<InteContainerDto>> GetPagedListAsync([FromQuery] InteContainerPagedQueryDto pagedQueryDto)
        {
            return await _inteContainerService.GetPagedListAsync(pagedQueryDto);
        }

        /// <summary>
        /// ��ѯ���飨����ά����
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<InteContainerDto> GetDetailAsync(long id)
        {
            return await _inteContainerService.GetDetailAsync(id);
        }
    }
}