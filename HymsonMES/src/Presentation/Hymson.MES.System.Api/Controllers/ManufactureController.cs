using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.System.Api.Controllers
{
    /// <summary>
    /// 生产
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ManufactureController : ControllerBase
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ManufactureController() { }

    }
}