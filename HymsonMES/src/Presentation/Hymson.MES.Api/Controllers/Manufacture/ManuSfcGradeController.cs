using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Manufacture;
using Hymson.MES.Services.Services.Manufacture;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Manufacture
{
    /// <summary>
    /// 控制器（条码档位表）
    /// @author Karl
    /// @date 2023-11-14 02:20:31
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ManuSfcGradeController : ControllerBase
    {
        /// <summary>
        /// 日志
        /// </summary>
        private readonly ILogger<ManuSfcGradeController> _logger;
        /// <summary>
        /// 服务接口（条码档位表）
        /// </summary>
        private readonly IManuSfcGradeService _manuSfcGradeService;


        /// <summary>
        /// 构造函数（条码档位表）
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="manuSfcGradeService"></param>
        public ManuSfcGradeController(ILogger<ManuSfcGradeController> logger, IManuSfcGradeService manuSfcGradeService)
        {
            _logger = logger;
            _manuSfcGradeService = manuSfcGradeService;
        }

        
        /// <summary>
        /// 查询详情（条码档位表）
        /// </summary>
        /// <param name="sfc"></param>
        /// <returns></returns>
        [HttpGet("{sfc}")]
        public async Task<ManuSfcGradeViewDto?> GetBySFCAsync(string sfc)
        {
            return await _manuSfcGradeService.GetBySFCAsync(sfc);
        }
    }
}