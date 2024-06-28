using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Integrated;
using Hymson.MES.Core.Domain.Quality;
using Hymson.MES.Core.Enums;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.MES.Data.Repositories.Integrated;
using Hymson.MES.Data.Repositories.Integrated.Query;
using Hymson.MES.Services.Dtos.Integrated;
using Hymson.MES.Services.Dtos.Quality;
using Hymson.Snowflake;
using Hymson.Utils;

namespace Hymson.MES.Services.Services.Integrated
{
    /// <summary>
    /// 服务（事件维护） 
    /// </summary>
    public class InteEventService : IInteEventService
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
        private readonly AbstractValidator<InteEventSaveDto> _validationSaveRules;

        /// <summary>
        /// 仓储接口（事件维护）
        /// </summary>
        private readonly IInteEventRepository _inteEventRepository;

        /// <summary>
        /// 仓储接口（事件类型维护）
        /// </summary>
        private readonly IInteEventTypeRepository _inteEventTypeRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="currentSite"></param>
        /// <param name="validationSaveRules"></param>
        /// <param name="inteEventRepository"></param>
        /// <param name="inteEventTypeRepository"></param>
        public InteEventService(ICurrentUser currentUser, ICurrentSite currentSite, AbstractValidator<InteEventSaveDto> validationSaveRules,
            IInteEventRepository inteEventRepository,
            IInteEventTypeRepository inteEventTypeRepository)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _validationSaveRules = validationSaveRules;
            _inteEventRepository = inteEventRepository;
            _inteEventTypeRepository = inteEventTypeRepository;
        }


        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        public async Task<long> CreateAsync(InteEventSaveDto saveDto)
        {
            // 验证DTO
            await _validationSaveRules.ValidateAndThrowAsync(saveDto);

            // 更新时间
            var updatedBy = _currentUser.UserName;
            var updatedOn = HymsonClock.Now();

            // DTO转换实体
            var entity = saveDto.ToEntity<InteEventEntity>();
            entity.Id = IdGenProvider.Instance.CreateId();
            entity.SiteId = _currentSite.SiteId ?? 0;
            entity.CreatedBy = updatedBy;
            entity.CreatedOn = updatedOn;
            entity.UpdatedBy = updatedBy;
            entity.UpdatedOn = updatedOn;

            if (saveDto.EventTypeCode != null)
            {
                var eventTypeEntity = await _inteEventTypeRepository.GetByCodeAsync(new EntityByCodeQuery
                {
                    Site = entity.SiteId,
                    Code = saveDto.EventTypeCode ?? string.Empty
                });

                if (eventTypeEntity == null)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES10910)).WithData("Code", saveDto.EventTypeCode);
                }
            }
            else { entity.EventTypeId = default; }

            // 编码唯一性验证
            var checkEntity = await _inteEventRepository.GetByCodeAsync(new EntityByCodeQuery
            {
                Site = entity.SiteId,
                Code = entity.Code
            });
            if (checkEntity != null) throw new CustomerValidationException(nameof(ErrorCode.MES10521)).WithData("Code", entity.Code);

            // 保存
            await _inteEventRepository.InsertAsync(entity);
            return entity.Id;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        public async Task<int> ModifyAsync(InteEventSaveDto saveDto)
        {
            // 验证DTO
            await _validationSaveRules.ValidateAndThrowAsync(saveDto);

            // DTO转换实体
            var entity = saveDto.ToEntity<InteEventEntity>();
            entity.SiteId = _currentSite.SiteId ?? 0;
            entity.UpdatedBy = _currentUser.UserName;
            entity.UpdatedOn = HymsonClock.Now();

            if (saveDto.EventTypeCode != null)
            {
                var eventTypeEntity = await _inteEventTypeRepository.GetByCodeAsync(new EntityByCodeQuery
                {
                    Site = entity.SiteId,
                    Code = saveDto.EventTypeCode ?? string.Empty
                });

                if (eventTypeEntity == null)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES10910)).WithData("Code", saveDto.EventTypeCode);
                }
            }
            else { entity.EventTypeId = default; }

            // 编码唯一性验证
            var checkEntity = await _inteEventRepository.GetByCodeAsync(new EntityByCodeQuery
            {
                Site = entity.SiteId,
                Code = entity.Code
            });
            if (checkEntity != null && checkEntity.Id != entity.Id)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10521)).WithData("Code", entity.Code);
            }

            return await _inteEventRepository.UpdateAsync(entity);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(long id)
        {
            return await _inteEventRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeletesAsync(long[] ids)
        {
            var getInfo = await _inteEventRepository.GetByIdsAsync(ids);
            var isStatues = getInfo.Where(x => x.Status == DisableOrEnableEnum.Enable);
            if (isStatues.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10909));
            }
            return await _inteEventRepository.DeletesAsync(new DeleteCommand
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
        public async Task<InteEventInfoDto?> QueryByIdAsync(long id)
        {
            var inteEventEntity = await _inteEventRepository.GetByIdAsync(id);
            if (inteEventEntity == null) return null;

            var dto = inteEventEntity.ToModel<InteEventInfoDto>();
            if (dto == null) return dto;

            var inteEventTypeEntity = await _inteEventTypeRepository.GetByIdAsync(dto.EventTypeId);
            if (inteEventTypeEntity != null)
            {
                dto.EventTypeCode = inteEventTypeEntity.Code;
                dto.EventTypeName = inteEventTypeEntity.Name;
            }

            return dto;
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<InteEventDto>> GetPagedListAsync(InteEventPagedQueryDto pagedQueryDto)
        {
            var pagedQuery = pagedQueryDto.ToQuery<InteEventPagedQuery>();
            pagedQuery.SiteId = _currentSite.SiteId ?? 0;
            var pagedInfo = await _inteEventRepository.GetPagedListAsync(pagedQuery);

            // 实体到DTO转换 装载数据
            var dtos = pagedInfo.Data.Select(s => s.ToModel<InteEventDto>());
            return new PagedInfo<InteEventDto>(dtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

    }
}
