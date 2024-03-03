using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;
using Hymson.MES.Services.Dtos.Common;
using Hymson.MES.Services.Dtos.Integrated;
using Hymson.MES.Services.Services.Integrated.InteContainer.V1;
using Hymson.Web.Framework.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Api.Controllers.Integrated.V1
{
    /// <summary>
    /// ������������ά����
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
        /// ���캯��������ά����
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="inteContainerService"></param>
        public InteContainerController(ILogger<InteContainerController> logger, IInteContainerService inteContainerService)
        {
            _inteContainerService = inteContainerService;
            _logger = logger;
        }

        /// <summary>
        /// ���ӣ�����ά����
        /// </summary>
        /// <param name="createDto"></param>
        /// <returns></returns>
        [HttpPost]
        [LogDescription("����ά��", BusinessType.INSERT)]
        [PermissionDescription("inte:container:insert")]
        public async Task CreateAsync(InteContainerSaveDto createDto)
        {
            await _inteContainerService.CreateAsync(createDto);
        }

        /// <summary>
        /// ���£�����ά����
        /// </summary>
        /// <param name="modifyDto"></param>
        /// <returns></returns>
        [HttpPut]
        [LogDescription("����ά��", BusinessType.UPDATE)]
        [PermissionDescription("inte:container:update")]
        public async Task ModifyAsync(InteContainerSaveDto modifyDto)
        {
            await _inteContainerService.ModifyAsync(modifyDto);
        }

        /// <summary>
        /// ɾ��������ά����
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        [LogDescription("����ά��", BusinessType.DELETE)]
        [PermissionDescription("inte:container:delete")]
        public async Task DeletesAsync(long[] ids)
        {
            await _inteContainerService.DeletesAsync(ids);
        }

        /// <summary>
        /// ��ȡ��ҳ���ݣ�����ά����
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
        /// ��ѯ���飨����ά����
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<InteContainerDto> GetDetailAsync(long id)
        {
            return await _inteContainerService.GetDetailAsync(id);
        }

        #region ״̬���
        /// <summary>
        /// ���ã�����ά����
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("updateStatusEnable")]
        [LogDescription("����ά��", BusinessType.UPDATE)]
        [PermissionDescription("inte:container:updateStatusEnable")]
        public async Task UpdateStatusEnable([FromBody] long id)
        {
            await _inteContainerService.UpdateStatusAsync(new ChangeStatusDto { Id = id, Status = SysDataStatusEnum.Enable });
        }

        /// <summary>
        /// ����������ά����
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("updateStatusRetain")]
        [LogDescription("����ά��", BusinessType.UPDATE)]
        [PermissionDescription("inte:container:updateStatusRetain")]
        public async Task UpdateStatusRetain([FromBody] long id)
        {
            await _inteContainerService.UpdateStatusAsync(new ChangeStatusDto { Id = id, Status = SysDataStatusEnum.Retain });
        }

        /// <summary>
        /// �ϳ�������ά����
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("updateStatusAbolish")]
        [LogDescription("����ά��", BusinessType.UPDATE)]
        [PermissionDescription("inte:container:updateStatusAbolish")]
        public async Task UpdateStatusAbolish([FromBody] long id)
        {
            await _inteContainerService.UpdateStatusAsync(new ChangeStatusDto { Id = id, Status = SysDataStatusEnum.Abolish });
        }

        #endregion
    }
}