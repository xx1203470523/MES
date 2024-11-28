using Hymson.Localization.Services;
using Hymson.MES.Core.Domain.SysSetting;
using Hymson.MES.Services.Dtos.Common;
using Hymson.MES.Services.Services.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Concurrent;
using System.Net;

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
        private readonly IMemoryCache _memoryCache;
        private readonly ICommonService _commonService;


        public CommonController(IEnumService enumService, IMemoryCache memoryCache, ICommonService commonService)
        {
            _enumService = enumService;
            _memoryCache = memoryCache;
            _commonService = commonService;
        }

        /// <summary>
        /// 获取枚举
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
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
        /// <summary>
        /// 打印所有缓存key
        /// </summary>
        /// <returns></returns>
        [Route("cache-keys")]
        [HttpGet]
        [AllowAnonymous]
        public IActionResult GetCacheKeys()
        {
            var caches = _memoryCache.GetCacheKeys();
            IPHostEntry ipEntry = Dns.GetHostEntry(Dns.GetHostName());
            ConcurrentBag<string> ips = new ConcurrentBag<string>();
            foreach (var ip in ipEntry.AddressList)
            {
                ips.Add("IP Address: " + ip.ToString());
            }
            var hostName = Dns.GetHostName();
            return Ok(new { caches, ips, hostName });
        }

        [Route("cache")]
        [HttpGet]
        [AllowAnonymous]
        public IActionResult GetCache(string key)
        {
            var result = _memoryCache.TryGetValue(key, out var cache);
            if (result)
            { return Ok(cache); }
            else
            { return NotFound(); }

        }

        [Route("remove-cache")]
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Remove(string key)
        {
             _memoryCache.Remove(key);
           return Ok();

        }

        [Route("clear")]
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Clear()
        {
            _memoryCache.RemoveCacheRegex("&");
            return Ok();

        }

        /// <summary>
        /// 获取系统全局配置列表
        /// </summary>
        /// <returns></returns>
        [Route("settings")]
        [HttpGet]
        public async Task<IEnumerable<SysSettingEntity>> GetSettingsAsync()
        {
            return await _commonService.GetSettingsAsync();
        }

        /// <summary>
        /// 保存系统配置
        /// </summary>
        /// <param name="settings"></param>
        /// <returns></returns>
        [Route("settings/save")]
        [HttpPost]
        public async Task SaveSettingsAsync(List<SysSettingDto> settings)
        {
            await _commonService.SaveSettingsAsync(settings);
        }
    }
}
