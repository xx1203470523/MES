using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Concurrent;
using System.Net;
using System.Reflection;

namespace Hymson.MES.Equipment.Api.Controllers
{
    /// <summary>
    /// 通用接口
    /// </summary>
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CommonController : ControllerBase
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly IMemoryCache _memoryCache;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="memoryCache"></param>
        public CommonController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
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
            return Ok(new { caches, ips, hostName, Assembly.GetEntryAssembly()?.GetName().Version });
        }

        /// <summary>
        /// 获取某个缓存值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 根据缓存key删除缓存
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [Route("remove-cache")]
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Remove(string key)
        {
            _memoryCache.Remove(key);
            return Ok(Assembly.GetEntryAssembly()?.GetName().Version);

        }

        /// <summary>
        /// 清理所有缓存
        /// </summary>
        /// <returns></returns>
        [Route("clear")]
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Clear()
        {
            _memoryCache.RemoveCacheRegex("&");
            return Ok(Assembly.GetEntryAssembly()?.GetName().Version);

        }

    }
}
