using Hymson.Localization.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers
{
    [Route("api/common")]
    [ApiController]
    public class CommonController : ControllerBase
    {
        private readonly IEnumService _enumService;

        public CommonController(IEnumService enumService)
        {
            _enumService = enumService;
        }
        /// <summary>
        /// 获取枚举
        /// </summary>
        /// <returns></returns>
        [Route("enums")]
        [HttpGet]
        public Dictionary<string, Dictionary<object, string>> GetEnumTypes()
        {
            return _enumService.GetEnumTypes();
        }
    }
}
