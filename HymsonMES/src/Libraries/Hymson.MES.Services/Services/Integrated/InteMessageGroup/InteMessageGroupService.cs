using FluentValidation;
using FluentValidation.Results;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Integrated;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.MES.Data.Repositories.Integrated;
using Hymson.MES.Data.Repositories.Integrated.Query;
using Hymson.MES.Services.Dtos.Integrated;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;

namespace Hymson.MES.Services.Services.Integrated
{
    /// <summary>
    /// 服务（消息组） 
    /// </summary>
    public class InteMessageGroupService : IInteMessageGroupService
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
        private readonly AbstractValidator<InteMessageGroupSaveDto> _validationSaveRules;

        /// <summary>
        /// 仓储接口（消息组）
        /// </summary>
        private readonly IInteMessageGroupRepository _inteMessageGroupRepository;

        /// <summary>
        /// 仓储接口（消息组推送方式）
        /// </summary>
        private readonly IInteMessageGroupPushMethodRepository _inteMessageGroupPushMethodRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="currentSite"></param>
        /// <param name="validationSaveRules"></param>
        /// <param name="inteMessageGroupRepository"></param>
        /// <param name="inteMessageGroupPushMethodRepository"></param>
        public InteMessageGroupService(ICurrentUser currentUser, ICurrentSite currentSite, AbstractValidator<InteMessageGroupSaveDto> validationSaveRules,
            IInteMessageGroupRepository inteMessageGroupRepository,
            IInteMessageGroupPushMethodRepository inteMessageGroupPushMethodRepository)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _validationSaveRules = validationSaveRules;
            _inteMessageGroupRepository = inteMessageGroupRepository;
            _inteMessageGroupPushMethodRepository = inteMessageGroupPushMethodRepository;
        }


        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        public async Task<int> CreateAsync(InteMessageGroupSaveDto saveDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0) throw new ValidationException(nameof(ErrorCode.MES10101));

            // 验证DTO
            await _validationSaveRules.ValidateAndThrowAsync(saveDto);

            // 更新时间
            var updatedBy = _currentUser.UserName;
            var updatedOn = HymsonClock.Now();

            // DTO转换实体
            var entity = saveDto.ToEntity<InteMessageGroupEntity>();
            entity.Id = IdGenProvider.Instance.CreateId();
            entity.CreatedBy = updatedBy;
            entity.CreatedOn = updatedOn;
            entity.UpdatedBy = updatedBy;
            entity.UpdatedOn = updatedOn;
            entity.SiteId = _currentSite.SiteId ?? 0;

            // 编码唯一性验证
            var checkEntity = await _inteMessageGroupRepository.GetByCodeAsync(new EntityByCodeQuery
            {
                Site = entity.SiteId,
                Code = entity.Code
            });
            if (checkEntity != null) throw new CustomerValidationException(nameof(ErrorCode.MES10521)).WithData("Code", entity.Code);

            // 判断规格上限和规格下限（数据类型为数值）
            List<ValidationFailure> validationFailures = new();
            foreach (var item in saveDto.Details)
            {
                // TODO
            }

            // 是否存在错误
            if (validationFailures.Any())
            {
                //throw new ValidationException(_localizationService.GetResource("SFCError"), validationFailures);
                throw new ValidationException("", validationFailures);
            }

            var details = saveDto.Details.Select(s =>
            {
                var detailEntity = s.ToEntity<InteMessageGroupPushMethodEntity>();
                detailEntity.Id = IdGenProvider.Instance.CreateId();
                detailEntity.MessageGroupId = entity.Id;
                detailEntity.CreatedBy = updatedBy;
                detailEntity.CreatedOn = updatedOn;
                detailEntity.UpdatedBy = updatedBy;
                detailEntity.UpdatedOn = updatedOn;
                detailEntity.SiteId = entity.SiteId;

                return detailEntity;
            });

            var rows = 0;
            using (var trans = TransactionHelper.GetTransactionScope())
            {
                rows += await _inteMessageGroupRepository.InsertAsync(entity);
                rows += await _inteMessageGroupPushMethodRepository.InsertRangeAsync(details);
                trans.Complete();
            }
            return rows;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        public async Task<int> ModifyAsync(InteMessageGroupSaveDto saveDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0) throw new ValidationException(nameof(ErrorCode.MES10101));

            // 验证DTO
            await _validationSaveRules.ValidateAndThrowAsync(saveDto);

            // 更新时间
            var updatedBy = _currentUser.UserName;
            var updatedOn = HymsonClock.Now();

            // DTO转换实体
            var entity = saveDto.ToEntity<InteMessageGroupEntity>();
            entity.UpdatedBy = updatedBy;
            entity.UpdatedOn = updatedOn;
            entity.SiteId = _currentSite.SiteId ?? 0;

            // 编码唯一性验证
            var checkEntity = await _inteMessageGroupRepository.GetByCodeAsync(new EntityByCodeQuery
            {
                Site = entity.SiteId,
                Code = entity.Code
            });
            if (checkEntity != null && checkEntity.Id != entity.Id)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10521)).WithData("Code", entity.Code);
            }

            // 判断规格上限和规格下限（数据类型为数值）
            List<ValidationFailure> validationFailures = new();
            foreach (var item in saveDto.Details)
            {
                // TODO
            }

            // 是否存在错误
            if (validationFailures.Any())
            {
                //throw new ValidationException(_localizationService.GetResource("SFCError"), validationFailures);
                throw new ValidationException("", validationFailures);
            }

            var details = saveDto.Details.Select(s =>
            {
                var detailEntity = s.ToEntity<InteMessageGroupPushMethodEntity>();
                detailEntity.Id = IdGenProvider.Instance.CreateId();
                detailEntity.MessageGroupId = entity.Id;
                detailEntity.CreatedBy = updatedBy;
                detailEntity.CreatedOn = updatedOn;
                detailEntity.UpdatedBy = updatedBy;
                detailEntity.UpdatedOn = updatedOn;
                detailEntity.SiteId = entity.SiteId;

                return detailEntity;
            });

            var command = new DeleteByParentIdCommand
            {
                ParentId = entity.Id,
                UpdatedBy = updatedBy,
                UpdatedOn = updatedOn
            };

            var rows = 0;
            using (var trans = TransactionHelper.GetTransactionScope())
            {
                rows += await _inteMessageGroupRepository.UpdateAsync(entity);
                rows += await _inteMessageGroupPushMethodRepository.DeleteByParentIdAsync(command);
                rows += await _inteMessageGroupPushMethodRepository.InsertRangeAsync(details);
                trans.Complete();
            }
            return rows;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(long id)
        {
            return await _inteMessageGroupRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeletesAsync(long[] ids)
        {
            return await _inteMessageGroupRepository.DeletesAsync(new DeleteCommand
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
        public async Task<InteMessageGroupDto?> QueryByIdAsync(long id)
        {
            var inteMessageGroupEntity = await _inteMessageGroupRepository.GetByIdAsync(id);
            if (inteMessageGroupEntity == null) return null;

            return inteMessageGroupEntity.ToModel<InteMessageGroupDto>();
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<InteMessageGroupDto>> GetPagedListAsync(InteMessageGroupPagedQueryDto pagedQueryDto)
        {
            var pagedQuery = pagedQueryDto.ToQuery<InteMessageGroupPagedQuery>();
            pagedQuery.SiteId = _currentSite.SiteId ?? 0;
            var pagedInfo = await _inteMessageGroupRepository.GetPagedListAsync(pagedQuery);

            // 实体到DTO转换 装载数据
            var dtos = pagedInfo.Data.Select(s => s.ToModel<InteMessageGroupDto>());
            return new PagedInfo<InteMessageGroupDto>(dtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

    }
}
