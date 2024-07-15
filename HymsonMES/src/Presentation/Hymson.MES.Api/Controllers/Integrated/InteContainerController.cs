using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;
using Hymson.MES.Services.Dtos.Common;
using Hymson.MES.Services.Dtos.Inte;
using Hymson.MES.Services.Dtos.Integrated;
using Hymson.MES.Services.Services.Integrated.InteContainer;
using Hymson.Web.Framework.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Integrated
{
    /// <summary>
    /// 控制器（容器维护）
    /// </summary>
    [ApiController]
    [Route("api/v2/[controller]")]
    public class InteContainerController : ControllerBase
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly IInteContainerService _inteContainerService;

        /// <summary>
        /// 构造函数（容器维护）
        /// </summary>
        /// <param name="inteContainerService"></param>
        public InteContainerController(IInteContainerService inteContainerService)
        {
            _inteContainerService = inteContainerService;
        }

        /// <summary>
        /// 添加（容器维护）
        /// </summary>
        /// <param name="createDto"></param>
        /// <returns></returns>
        [HttpPost]
        [LogDescription("添加容器", BusinessType.INSERT)]
        [PermissionDescription("inte:container:insert")]
        public async Task CreateAsync(InteContainerInfoDto createDto)
        {
            await _inteContainerService.CreateAsync(createDto);
        }

        /// <summary>
        /// 更新（容器维护）
        /// </summary>
        /// <param name="modifyDto"></param>
        /// <returns></returns>
        [HttpPut]
        [LogDescription("更新容器", BusinessType.UPDATE)]
        [PermissionDescription("inte:container:update")]
        public async Task ModifyAsync(InteContainerInfoUpdateDto modifyDto)
        {
            await _inteContainerService.ModifyAsync(modifyDto);
        }

        /// <summary>
        /// 查询容器信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("contaiinerInfo/{id}")]
        public async Task<InteContainerInfoOutputDto> GetInfoByIdAsync(long id)
        {
            return await _inteContainerService.GetInfoByIdAsync(id);
        }

        /// <summary>
        /// 查询容器规格信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("contaiinerSpecificationInfo/{id}")]
        public async Task<InteContainerSpecificationOutputDto> GetSpecificationByIdAsync(long id)
        {
            return await _inteContainerService.GetSpecificationByIdAsync(id);
        }

        /// <summary>
        /// 查询容器装载列表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("containerFreightInfo/{id}")]
        public async Task<IEnumerable<InteContainerFreightOutputDto>> GetContainerFreightInfoByIdAsync(long id)
        {
            return await _inteContainerService.GetContainerFreightInfoByIdAsync(id);
        }

        /// <summary>
        /// 删除（容器维护）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        [LogDescription("删除容器", BusinessType.DELETE)]
        [PermissionDescription("inte:container:delete")]
        public async Task DeletesAsync(IEnumerable<long> ids)
        {
            await _inteContainerService.DeletesAsync(ids);
        }

        /// <summary>
        /// 获取分页数据（容器维护）
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        [Route("page")]
        [HttpGet]
        //[PermissionDescription("inte:container:list")]
        public async Task<PagedInfo<InteContainerInfoOutputDto>> GetPagedListAsync([FromQuery] InteContainerInfoPagedQueryDto pagedQueryDto)
        {
            return await _inteContainerService.GetPagedListAsync(pagedQueryDto);
        }

        /// <summary>
        /// 查询详情（容器维护）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<InteContainerReDto> GetDetailAsync(long id)
        {
            return await _inteContainerService.GetDetailAsync(id);
        }

        #region 状态变更
        /// <summary>
        /// 启用（容器维护）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("updateStatusEnable")]
        [LogDescription("容器维护", BusinessType.UPDATE)]
        [PermissionDescription("inte:container:updateStatusEnable")]
        public async Task UpdateStatusEnable([FromBody] long id)
        {
            await _inteContainerService.UpdateStatusAsync(new ChangeStatusDto { Id = id, Status = SysDataStatusEnum.Enable });
        }

        /// <summary>
        /// 保留（容器维护）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("updateStatusRetain")]
        [LogDescription("容器维护", BusinessType.UPDATE)]
        [PermissionDescription("inte:container:updateStatusRetain")]
        public async Task UpdateStatusRetain([FromBody] long id)
        {
            await _inteContainerService.UpdateStatusAsync(new ChangeStatusDto { Id = id, Status = SysDataStatusEnum.Retain });
        }

        /// <summary>
        /// 废除（容器维护）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("updateStatusAbolish")]
        [LogDescription("容器维护", BusinessType.UPDATE)]
        [PermissionDescription("inte:container:updateStatusAbolish")]
        public async Task UpdateStatusAbolish([FromBody] long id)
        {
            await _inteContainerService.UpdateStatusAsync(new ChangeStatusDto { Id = id, Status = SysDataStatusEnum.Abolish });
        }

        #endregion
    }
}