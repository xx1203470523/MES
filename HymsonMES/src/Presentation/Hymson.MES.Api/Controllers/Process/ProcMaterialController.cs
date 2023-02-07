/*
 *creator: Karl
 *
 *describe: 物料维护    控制器 | 代码由框架生成  
 *builder:  Karl
 *build datetime: 2023-02-07 11:16:51
 */
using Hymson.Infrastructure;
using Hymson.MES.Services.Services.Process;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Process
{
    /// <summary>
    /// 控制器（物料维护）
    /// @author Karl
    /// @date 2023-02-07 11:16:51
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ProcMaterialController : ControllerBase
    {
        //// <summary>
        /// 接口（物料维护）
        /// </summary>
        private readonly IProcMaterialService _procMaterialService;
        private readonly ILogger<ProcMaterialController> _logger;

        /// <summary>
        /// 构造函数（物料维护）
        /// </summary>
        /// <param name="procMaterialService"></param>
        public ProcMaterialController(IProcMaterialService procMaterialService, ILogger<ProcMaterialController> logger)
        {
            _procMaterialService = procMaterialService;
            _logger = logger;
        }

    }
}