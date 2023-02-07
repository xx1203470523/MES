using Hymson.Infrastructure;
using Hymson.MES.Services.Services.Process;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Process
{
    /// <summary>
    /// ������������ά����
    /// @tableName proc_material
    /// @author Czhipu
    /// @date 2022-08-29
    /// </summary>
    [ApiController]
    [Route("process/procMaterial")]
    public class ProcMaterialController : ControllerBase
    {
        //// <summary>
        /// �ӿڣ�����ά����
        /// </summary>
        private readonly IProcMaterialService _procMaterialService;
        private readonly ILogger<ProcMaterialController> _logger;

        /// <summary>
        /// ���캯��������ά����
        /// </summary>
        /// <param name="procMaterialService"></param>
        public ProcMaterialController(IProcMaterialService procMaterialService, ILogger<ProcMaterialController> logger)
        {
            _procMaterialService = procMaterialService;
            _logger = logger;
        }

        ///// <summary>
        ///// ��ȡ��ҳ����
        ///// </summary>
        ///// <param name="whStockChangeRecordPagedQueryDto"></param>
        ///// <returns></returns>
        //[Route("pagelist")]
        //[HttpGet]
        //public async Task<PagedInfo<WhStockChangeRecordDto>> GetList([FromQuery]WhStockChangeRecordPagedQueryDto whStockChangeRecordPagedQueryDto)
        //{
        //    return await _whStockChangeRecordService.GetListAsync(whStockChangeRecordPagedQueryDto);
        //}

        ///// <summary>
        ///// ������¼
        ///// </summary>
        ///// <param name="whStockChangeRecordDto"></param>
        ///// <returns></returns>
        //[HttpPost]
        //[Route("create")]
        //public async Task Create(WhStockChangeRecordDto whStockChangeRecordDto)
        //{
        //    await _whStockChangeRecordService.CreateWhStockChangeRecordAsync(whStockChangeRecordDto);
        //}
    }
}