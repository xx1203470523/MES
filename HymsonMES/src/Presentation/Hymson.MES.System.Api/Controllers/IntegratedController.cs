using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.System.Api.Controllers
{
    /// <summary>
    /// 综合
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class IntegratedController : ControllerBase
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public IntegratedController() { }

    }
}