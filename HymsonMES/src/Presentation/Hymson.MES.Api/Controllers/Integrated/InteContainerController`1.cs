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
    /// ������������ά����
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
        /// ���캯��������ά����
        /// </summary>
        /// <param name="inteContainerService"></param>
        public InteContainerController(IInteContainerService inteContainerService)
        {
            _inteContainerService = inteContainerService;
        }

        /// <summary>
        /// ��ӣ�����ά����
        /// </summary>
        /// <param name="createDto"></param>
        /// <returns></returns>
        [HttpPost]
        [LogDescription("�������", BusinessType.INSERT)]
        [PermissionDescription("inte:container:insert")]
        public async Task CreateAsync(InteContainerInfoDto createDto)
        {
            await _inteContainerService.CreateAsync(createDto);
        }

        /// <summary>
        /// ���£�����ά����
        /// </summary>
        /// <param name="modifyDto"></param>
        /// <returns></returns>
        [HttpPut]
        [LogDescription("��������", BusinessType.UPDATE)]
        [PermissionDescription("inte:container:update")]
        public async Task ModifyAsync(InteContainerInfoUpdateDto modifyDto)
        {
            await _inteContainerService.ModifyAsync(modifyDto);
        }

        /// <summary>
        /// ��ѯ������Ϣ
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("contaiinerInfo/{id}")]
        public async Task<InteContainerInfoOutputDto> GetInfoByIdAsync(long id)
        {
            return await _inteContainerService.GetInfoByIdAsync(id);
        }

        /// <summary>
        /// ��ѯ���������Ϣ
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("contaiinerSpecificationInfo/{id}")]
        public async Task<InteContainerSpecificationOutputDto> GetSpecificationByIdAsync(long id)
        {
            return await _inteContainerService.GetSpecificationByIdAsync(id);
        }

        /// <summary>
        /// ��ѯ���������б�
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("containerFreightInfo/{id}")]
        public async Task<IEnumerable<InteContainerFreightOutputDto>> GetEntitiesAsync(long id)
        {
            return await _inteContainerService.GetEntitiesAsync(id);
        }

        /// <summary>
        /// ɾ��������ά����
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        [LogDescription("ɾ������", BusinessType.DELETE)]
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
        public async Task<PagedInfo<InteContainerInfoOutputDto>> GetPagedListAsync([FromQuery] InteContainerInfoPagedQueryDto pagedQueryDto)
        {
            return await _inteContainerService.GetPagedListAsync(pagedQueryDto);
        }

        /// <summary>
        /// ��ѯ���飨����ά����
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<InteContainerReDto> GetDetailAsync(long id)
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