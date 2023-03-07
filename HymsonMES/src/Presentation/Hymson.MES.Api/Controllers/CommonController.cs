using Hymson.Localization.Services;
using Hymson.MES.Services.Dtos.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Hymson.MES.Api.Controllers
{
    [Route("api/v1/[controller]")]
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


        /// <summary>
        /// 获取枚举
        /// </summary>
        /// <returns></returns>
        [Route("enumsList")]
        [HttpGet]
        public Dictionary<string, List<EnumDto>> GetEnumTypes2()
        {
            var dicts = _enumService.GetEnumTypes();

            Dictionary<string, List<EnumDto>> keyValuePairs1 = new Dictionary<string, List<EnumDto>>();
            foreach (var dict in dicts)
            {
                var list = new List<EnumDto>();
                var key = dict.Key;
                var items = dict.Value;
                foreach (var item in items)
                {
                    list.Add(new EnumDto
                    {
                        Value = item.Key,
                        Label = item.Value
                    });
                }
                keyValuePairs1.Add(key, list);
            }

         return keyValuePairs1;
        }
    }
}
