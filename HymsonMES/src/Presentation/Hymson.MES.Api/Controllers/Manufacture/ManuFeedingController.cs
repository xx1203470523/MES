using Hymson.MES.Services.Dtos.Manufacture;
using Hymson.MES.Services.Services.Manufacture.ManuFeeding;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Manufacture
{
    /// <summary>
    /// �����������ϼ��أ�
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ManuFeedingController : ControllerBase
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly IManuFeedingService _manuFeedingService;
        private readonly ILogger<ManuFeedingController> _logger;

        /// <summary>
        /// ���캯�������ϼ��أ�
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="manuFeedingService"></param>
        public ManuFeedingController(ILogger<ManuFeedingController> logger, IManuFeedingService manuFeedingService)
        {
            _logger = logger;
            _manuFeedingService = manuFeedingService;
        }


        /// <summary>
        /// ��ѯ��Դ�����ϼ��أ�
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        [HttpGet("resource")]
        public async Task<IEnumerable<ManuFeedingResourceDto>> GetFeedingResourceListAsync([FromQuery] ManuFeedingResourceQueryDto queryDto)
        {
            return await _manuFeedingService.GetFeedingResourceListAsync(queryDto);
        }

        /// <summary>
        /// ��ѯ���ϣ����ϼ��أ�
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        [HttpGet("material")]
        public async Task<IEnumerable<ManuFeedingMaterialDto>> GetFeedingMaterialListAsync([FromQuery] ManuFeedingMaterialQueryDto queryDto)
        {
            return await _manuFeedingService.GetFeedingMaterialListAsync(queryDto);
        }

        /*
        /// <summary>
        /// ��ӣ����ϼ��أ�
        /// </summary>
        /// <param name="createDto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task CreateAsync(InteContainerSaveDto createDto)
        {
            await _manuFeedingService.CreateAsync(createDto);
        }
        */

        /*
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
        */
    }
}