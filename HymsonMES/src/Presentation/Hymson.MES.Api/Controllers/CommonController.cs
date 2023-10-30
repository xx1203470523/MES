﻿using Hymson.Localization.Services;
using Hymson.MES.Services.Dtos.Common;
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

       
        /// <summary>
        /// 
        /// </summary>
        /// <param name="enumService"></param>
        /// <param name="memoryCache"></param>
        public CommonController(IEnumService enumService,IMemoryCache memoryCache)
        {
            _enumService = enumService;
            _memoryCache = memoryCache;
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
            return Ok();

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
            return Ok();

        }

    }
}
