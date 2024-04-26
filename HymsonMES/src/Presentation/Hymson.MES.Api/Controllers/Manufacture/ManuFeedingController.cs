using Hymson.MES.CoreServices.Dtos.Common;
using Hymson.MES.Services.Dtos.Manufacture;
using Hymson.MES.Services.Services.Manufacture.ManuFeeding;
using Hymson.Web.Framework.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Manufacture
{
    /// <summary>
    /// �����������ϼ��أ�
    /// </summary>
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
        public async Task<IEnumerable<SelectOptionDto>> GetFeedingResourceListAsync([FromQuery] ManuFeedingResourceQueryDto queryDto)
        {
            return await _manuFeedingService.GetFeedingResourceListAsync(queryDto);
        }

        /// <summary>
        /// �鹤�������ϼ��أ�
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        [HttpGet("loadPoint")]
        public async Task<IEnumerable<SelectOptionDto>> GetFeedingLoadPointListAsync([FromQuery] ManuFeedingLoadPointQueryDto queryDto)
        {
            return await _manuFeedingService.GetFeedingLoadPointListAsync(queryDto);
        }

        /// <summary>
        /// �鹤�������ϼ��أ�
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        [HttpGet("workOder")]
        public async Task<IEnumerable<SelectOptionDto>> GetFeedingWorkOrderListAsync([FromQuery] ManuFeedingWorkOrderQueryDto queryDto)
        {
            return await _manuFeedingService.GetFeedingWorkOrderListAsync(queryDto);
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

        /// <summary>
        /// ��ӣ����ϼ��أ�
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        [HttpPost]
        [LogDescription("���ϼ���", BusinessType.INSERT)]
        public async Task<ManuFeedingMaterialResponseDto> CreateAsync(ManuFeedingMaterialSaveDto saveDto)
        {
            return await _manuFeedingService.CreateAsync(saveDto);
        }

        /// <summary>
        /// ɾ�������ϼ��أ�
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        [LogDescription("���ϼ���", BusinessType.DELETE)]
        public async Task DeletesAsync(long[] ids)
        {
            await _manuFeedingService.DeletesAsync(ids);
        }

    }
}