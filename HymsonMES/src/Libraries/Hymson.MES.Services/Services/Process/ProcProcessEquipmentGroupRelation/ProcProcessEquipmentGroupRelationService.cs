using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.Process.Query;
using Hymson.MES.Services.Dtos.Process;
using Hymson.Snowflake;
using Hymson.Utils;

namespace Hymson.MES.Services.Services.Process
{
    /// <summary>
    /// 服务（设备组关联设备表） 
    /// </summary>
    public class ProcProcessEquipmentGroupRelationService : IProcProcessEquipmentGroupRelationService
    {
        /// <summary>
        /// 当前用户
        /// </summary>
        private readonly ICurrentUser _currentUser;
        /// <summary>
        /// 当前站点
        /// </summary>
        private readonly ICurrentSite _currentSite;

        /// <summary>
        /// 参数验证器
        /// </summary>
        private readonly AbstractValidator<ProcProcessEquipmentGroupRelationSaveDto> _validationSaveRules;

        /// <summary>
        /// 仓储接口（设备组关联设备表）
        /// </summary>
        private readonly IProcProcessEquipmentGroupRelationRepository _procProcessEquipmentGroupRelationRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="currentSite"></param>
        /// <param name="validationSaveRules"></param>
        /// <param name="procProcessEquipmentGroupRelationRepository"></param>
        public ProcProcessEquipmentGroupRelationService(ICurrentUser currentUser, ICurrentSite currentSite, AbstractValidator<ProcProcessEquipmentGroupRelationSaveDto> validationSaveRules, 
            IProcProcessEquipmentGroupRelationRepository procProcessEquipmentGroupRelationRepository)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _validationSaveRules = validationSaveRules;
            _procProcessEquipmentGroupRelationRepository = procProcessEquipmentGroupRelationRepository;
        }


        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        public async Task<int> CreateProcProcessEquipmentGroupRelationAsync(ProcProcessEquipmentGroupRelationSaveDto saveDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0) throw new CustomerValidationException(nameof(ErrorCode.MES10101));

            // 验证DTO
            await _validationSaveRules.ValidateAndThrowAsync(saveDto);

            // 更新时间
            var updatedBy = _currentUser.UserName;
            var updatedOn = HymsonClock.Now();

            // DTO转换实体
            var entity = saveDto.ToEntity<ProcProcessEquipmentGroupRelationEntity>();
            entity.Id = IdGenProvider.Instance.CreateId();
            entity.CreatedBy = updatedBy;
            entity.CreatedOn = updatedOn;
            entity.UpdatedBy = updatedBy;
            entity.UpdatedOn = updatedOn;
            entity.SiteId = _currentSite.SiteId ?? 0;

            // 保存
            return await _procProcessEquipmentGroupRelationRepository.InsertAsync(entity);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        public async Task<int> ModifyProcProcessEquipmentGroupRelationAsync(ProcProcessEquipmentGroupRelationSaveDto saveDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0) throw new CustomerValidationException(nameof(ErrorCode.MES10101));

             // 验证DTO
            await _validationSaveRules.ValidateAndThrowAsync(saveDto);

            // DTO转换实体
            var entity = saveDto.ToEntity<ProcProcessEquipmentGroupRelationEntity>();
            entity.UpdatedBy = _currentUser.UserName;
            entity.UpdatedOn = HymsonClock.Now();

            return await _procProcessEquipmentGroupRelationRepository.UpdateAsync(entity);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> DeleteProcProcessEquipmentGroupRelationAsync(long id)
        {
            return await _procProcessEquipmentGroupRelationRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeletesProcProcessEquipmentGroupRelationAsync(long[] ids)
        {
            return await _procProcessEquipmentGroupRelationRepository.DeletesAsync(new DeleteCommand
            {
                Ids = ids,
                DeleteOn = HymsonClock.Now(),
                UserId = _currentUser.UserName
            });
        }

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ProcProcessEquipmentGroupRelationDto?> QueryProcProcessEquipmentGroupRelationByIdAsync(long id) 
        {
           var procProcessEquipmentGroupRelationEntity = await _procProcessEquipmentGroupRelationRepository.GetByIdAsync(id);
           if (procProcessEquipmentGroupRelationEntity == null) return null;
           
           return procProcessEquipmentGroupRelationEntity.ToModel<ProcProcessEquipmentGroupRelationDto>();
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ProcProcessEquipmentGroupRelationDto>> GetPagedListAsync(ProcProcessEquipmentGroupRelationPagedQueryDto pagedQueryDto)
        {
            var pagedQuery = pagedQueryDto.ToQuery<ProcProcessEquipmentGroupRelationPagedQuery>();
            var pagedInfo = await _procProcessEquipmentGroupRelationRepository.GetPagedInfoAsync(pagedQuery);

            // 实体到DTO转换 装载数据
            var dtos = pagedInfo.Data.Select(s => s.ToModel<ProcProcessEquipmentGroupRelationDto>());
            return new PagedInfo<ProcProcessEquipmentGroupRelationDto>(dtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

    }
}
