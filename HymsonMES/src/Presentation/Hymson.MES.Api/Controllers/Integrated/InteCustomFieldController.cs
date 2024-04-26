using Hymson.Infrastructure;
using Hymson.MES.Core.Enums.Integrated;
using Hymson.MES.Services.Dtos.Integrated;
using Hymson.MES.Services.Services.Integrated;
using Hymson.Web.Framework.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Integrated
{
    /// <summary>
    /// 控制器（自定义字段）
    /// @author Karl
    /// @date 2023-12-15 04:30:52
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class InteCustomFieldController : ControllerBase
    {
        /// <summary>
        /// 日志
        /// </summary>
        private readonly ILogger<InteCustomFieldController> _logger;
        /// <summary>
        /// 服务接口（自定义字段）
        /// </summary>
        private readonly IInteCustomFieldService _inteCustomFieldService;


        /// <summary>
        /// 构造函数（自定义字段）
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="inteCustomFieldService"></param>
        public InteCustomFieldController(ILogger<InteCustomFieldController> logger, IInteCustomFieldService inteCustomFieldService)
        {
            _logger = logger;
            _inteCustomFieldService = inteCustomFieldService;
        }

        /// <summary>
        /// 添加或更新（自定义字段）
        /// </summary>
        /// <param name="saveDtos"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("addOrUpdate")]
        [LogDescription("自定义字段", BusinessType.UPDATE)]
        public async Task AddOrUpdateAsync([FromBody] IEnumerable<InteCustomFieldSaveDto> saveDtos)
        {
             await _inteCustomFieldService.AddOrUpdateAsync(saveDtos);
        }

        /// <summary>
        /// 获取业务类型下的字段信息
        /// </summary>
        /// <param name="businessType"></param>
        /// <returns></returns>
        [HttpGet("getDataByBusinessType")]
        public async Task<IEnumerable<InteCustomFieldDto>> GetDataByBusinessTypeAsync([FromQuery] InteCustomFieldBusinessTypeEnum businessType) 
        {
            return await _inteCustomFieldService.GetDataByBusinessTypeAsync(businessType);
        }

        /// <summary>
        /// 保存各个业务ID的自定义字段数据
        /// </summary>
        /// <param name="saveBusinessDtos"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("saveBusinessData")]
        [LogDescription("自定义字段", BusinessType.OTHER)]
        public async Task SaveBusinessDataAsync(IEnumerable<InteCustomFieldBusinessEffectuateDto> saveBusinessDtos) 
        {
            await _inteCustomFieldService.SaveBusinessDataAsync(saveBusinessDtos);
        }

        /// <summary>
        /// 根据业务ID获取业务数据
        /// </summary>
        /// <param name="businessId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("getBusinessEffectuates")]
        public async Task<IEnumerable<InteCustomFieldBusinessEffectuateDto>> GetBusinessEffectuatesAsync(long businessId) 
        {
            return await _inteCustomFieldService.GetBusinessEffectuatesAsync(businessId);
        }

        /// <summary>
        /// 根据业务ID获取业务数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delBusinessEffectuates")]
        [LogDescription("自定义字段", BusinessType.DELETE)]
        public async Task<int> DelBusinessEffectuatesAsync(long[] ids)
        {
            return await _inteCustomFieldService.DelBusinessEffectuatesAsync(ids);
        }
    }
}