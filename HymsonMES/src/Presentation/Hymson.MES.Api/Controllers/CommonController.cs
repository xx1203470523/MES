using Hymson.Localization.Services;
using Hymson.MES.Services.Dtos.Common;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CommonController : ControllerBase
    {
        private readonly IEnumService _enumService;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="enumService"></param>
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

        /// <summary>
        /// 获取枚举
        /// </summary>
        /// <returns></returns>
        [Route("enumsList")]
        [HttpGet]
        public Dictionary<string, IEnumerable<EnumDto>> GetEnumTypesList()
        {
            Dictionary<string, IEnumerable<EnumDto>> keyValuePairs = new();

            var dicts = _enumService.GetEnumTypes();
            foreach (var dict in dicts)
            {
                keyValuePairs.Add(dict.Key, dict.Value.Select(s => new EnumDto { Value = s.Key, Label = s.Value }));
            }

            return keyValuePairs;
        }

    }
}
