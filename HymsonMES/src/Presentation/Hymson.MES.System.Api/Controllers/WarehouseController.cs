using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.System.Api.Controllers
{
    /// <summary>
    /// 库存
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class WarehouseController : ControllerBase
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public WarehouseController() { }


    }
}