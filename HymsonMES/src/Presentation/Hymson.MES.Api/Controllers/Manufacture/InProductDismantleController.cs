using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Services.Dtos.Manufacture;
using Hymson.MES.Services.Services.Manufacture;
using Hymson.MES.Services.Services.Manufacture.ManuFeeding;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Manufacture
{
    /// <summary>
    /// 在制品拆解
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class InProductDismantleController : ControllerBase
    {
        private readonly IInProductDismantleService _inProductDismantleService;
        private readonly ILogger<InProductDismantleController> _logger;

        /// <summary>
        /// 构造函数（物料加载）
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="inProductDismantleService"></param>
        public InProductDismantleController(ILogger<InProductDismantleController> logger, IInProductDismantleService inProductDismantleService)
        {
            _logger = logger;
            _inProductDismantleService = inProductDismantleService;
        }

        /// <summary>
        /// 查询Bom维护表详情
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("mainmaterials")]
        public async Task<List<InProductDismantleDto>> GetProcBomDetailAsync([FromQuery] InProductDismantleQueryDto queryDto)
        {
            return await _inProductDismantleService.GetProcBomDetailAsync(queryDto);
        }
    }
}
