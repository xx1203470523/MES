using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;
using Hymson.MES.Services.Dtos.Common;
using Hymson.MES.Services.Dtos.Integrated;
using Hymson.MES.Services.Services.Integrated.InteContainer;
using Hymson.Web.Framework.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Integrated
{
    /// <summary>
    /// ¿ØÖÆÆ÷£¨ÈÝÆ÷Î¬»¤£©
    /// </summary>
    
    [ApiController]
    [Route("api/v1/[controller]")]
    public class InteContainerController : ControllerBase
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly IInteContainerService _inteContainerService;
        private readonly ILogger<InteContainerController> _logger;

        /// <summary>
        /// ¹¹Ôìº¯Êý£¨ÈÝÆ÷Î¬»¤£©
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="inteContainerService"></param>
        public InteContainerController(ILogger<InteContainerController> logger, IInteContainerService inteContainerService)
        {
            _inteContainerService = inteContainerService;
            _logger = logger;
        }

        /// <summary>
        /// Ìí¼Ó£¨ÈÝÆ÷Î¬»¤£©
        /// </summary>
        /// <param name="createDto"></param>
        /// <returns></returns>
        [HttpPost]
        [LogDescription("ÈÝÆ÷Î¬»¤", BusinessType.INSERT)]
        [PermissionDescription("inte:container:insert")]
        public async Task CreateAsync(InteContainerSaveDto createDto)
        {
            await _inteContainerService.CreateAsync(createDto);
        }

        /// <summary>
        /// ¸üÐÂ£¨ÈÝÆ÷Î¬»¤£©
        /// </summary>
        /// <param name="modifyDto"></param>
        /// <returns></returns>
        [HttpPut]
        [LogDescription("ÈÝÆ÷Î¬»¤", BusinessType.UPDATE)]
        [PermissionDescription("inte:container:update")]
        public async Task ModifyAsync(InteContainerSaveDto modifyDto)
        {
            await _inteContainerService.ModifyAsync(modifyDto);
        }

        /// <summary>
        /// É¾³ý£¨ÈÝÆ÷Î¬»¤£©
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        [LogDescription("ÈÝÆ÷Î¬»¤", BusinessType.DELETE)]
        [PermissionDescription("inte:container:delete")]
        public async Task DeletesAsync(long[] ids)
        {
            await _inteContainerService.DeletesAsync(ids);
        }

        /// <summary>
        /// »ñÈ¡·ÖÒ³Êý¾Ý£¨ÈÝÆ÷Î¬»¤£©
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        [Route("page")]
        [HttpGet]
        //[PermissionDescription("inte:container:list")]
        public async Task<PagedInfo<InteContainerDto>> GetPagedListAsync([FromQuery] InteContainerPagedQueryDto pagedQueryDto)
        {
            return await _inteContainerService.GetPagedListAsync(pagedQueryDto);
        }

        /// <summary>
        /// ²éÑ¯ÏêÇé£¨ÈÝÆ÷Î¬»¤£©
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<InteContainerDto> GetDetailAsync(long id)
        {
            return await _inteContainerService.GetDetailAsync(id);
        }

        #region ×´Ì¬±ä¸ü
        /// <summary>
        /// ÆôÓÃ£¨ÈÝÆ÷Î¬»¤£©
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("updateStatusEnable")]
        [LogDescription("ÈÝÆ÷Î¬»¤", BusinessType.UPDATE)]
        [PermissionDescription("inte:container:updateStatusEnable")]
        public async Task UpdateStatusEnable([FromBody] long id)
        {
            await _inteContainerService.UpdateStatusAsync(new ChangeStatusDto { Id = id, Status = SysDataStatusEnum.Enable });
        }

        /// <summary>
        /// ±£Áô£¨ÈÝÆ÷Î¬»¤£©
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("updateStatusRetain")]
        [LogDescription("ÈÝÆ÷Î¬»¤", BusinessType.UPDATE)]
        [PermissionDescription("inte:container:updateStatusRetain")]
        public async Task UpdateStatusRetain([FromBody] long id)
        {
            await _inteContainerService.UpdateStatusAsync(new ChangeStatusDto { Id = id, Status = SysDataStatusEnum.Retain });
        }

        /// <summary>
        /// ·Ï³ý£¨ÈÝÆ÷Î¬»¤£©
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("updateStatusAbolish")]
        [LogDescription("ÈÝÆ÷Î¬»¤", BusinessType.UPDATE)]
        [PermissionDescription("inte:container:updateStatusAbolish")]
        public async Task UpdateStatusAbolish([FromBody] long id)
        {
            await _inteContainerService.UpdateStatusAsync(new ChangeStatusDto { Id = id, Status = SysDataStatusEnum.Abolish });
        }

        #endregion
    }
}