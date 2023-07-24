using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Quality;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.MES.Data.Repositories.Quality;
using Hymson.MES.Data.Repositories.Quality.Query;
using Hymson.MES.Services.Dtos.Quality;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;

namespace Hymson.MES.Services.Services.Quality
{
    /// <summary>
    /// 服务（环境检验参数表） 
    /// </summary>
    public class QualEnvParameterGroupService : IQualEnvParameterGroupService
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
        private readonly AbstractValidator<QualEnvParameterGroupSaveDto> _validationSaveRules;

        /// <summary>
        /// 仓储接口（环境检验参数表）
        /// </summary>
        private readonly IQualEnvParameterGroupRepository _qualEnvParameterGroupRepository;

        /// <summary>
        /// 仓储接口（环境检验参数项目表）
        /// </summary>
        private readonly IQualEnvParameterGroupDetailRepository _qualEnvParameterGroupDetailRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="currentSite"></param>
        /// <param name="validationSaveRules"></param>
        /// <param name="qualEnvParameterGroupRepository"></param>
        /// <param name="qualEnvParameterGroupDetailRepository"></param>
        public QualEnvParameterGroupService(ICurrentUser currentUser, ICurrentSite currentSite, AbstractValidator<QualEnvParameterGroupSaveDto> validationSaveRules,
            IQualEnvParameterGroupRepository qualEnvParameterGroupRepository,
            IQualEnvParameterGroupDetailRepository qualEnvParameterGroupDetailRepository)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _validationSaveRules = validationSaveRules;
            _qualEnvParameterGroupRepository = qualEnvParameterGroupRepository;
            _qualEnvParameterGroupDetailRepository = qualEnvParameterGroupDetailRepository;
        }


        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        public async Task<int> CreateQualEnvParameterGroupAsync(QualEnvParameterGroupSaveDto saveDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0) throw new ValidationException(nameof(ErrorCode.MES10101));

            // 验证DTO
            await _validationSaveRules.ValidateAndThrowAsync(saveDto);

            // 更新时间
            var updatedBy = _currentUser.UserName;
            var updatedOn = HymsonClock.Now();

            // DTO转换实体
            var entity = saveDto.ToEntity<QualEnvParameterGroupEntity>();
            entity.Id = IdGenProvider.Instance.CreateId();
            entity.CreatedBy = updatedBy;
            entity.CreatedOn = updatedOn;
            entity.UpdatedBy = updatedBy;
            entity.UpdatedOn = updatedOn;
            entity.SiteId = _currentSite.SiteId ?? 0;

            // 编码唯一性验证
            var checkEntity = await _qualEnvParameterGroupRepository.GetByCodeAsync(new EntityByCodeQuery { Site = entity.SiteId, Code = entity.Code });
            if (checkEntity != null) throw new CustomerValidationException(nameof(ErrorCode.MES12600)).WithData("Code", entity.Code);

            var details = saveDto.Details.Select(s =>
            {
                var detailEntity = s.ToEntity<QualEnvParameterGroupDetailEntity>();
                detailEntity.Id = IdGenProvider.Instance.CreateId();
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
                rows += await _qualEnvParameterGroupRepository.InsertAsync(entity);
                rows += await _qualEnvParameterGroupDetailRepository.InsertRangeAsync(details);
                trans.Complete();
            }
            return rows;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        public async Task<int> ModifyQualEnvParameterGroupAsync(QualEnvParameterGroupSaveDto saveDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0) throw new ValidationException(nameof(ErrorCode.MES10101));

            // 验证DTO
            await _validationSaveRules.ValidateAndThrowAsync(saveDto);

            // 更新时间
            var updatedBy = _currentUser.UserName;
            var updatedOn = HymsonClock.Now();

            // DTO转换实体
            var entity = saveDto.ToEntity<QualEnvParameterGroupEntity>();
            entity.UpdatedBy = updatedBy;
            entity.UpdatedOn = updatedOn;

            var details = saveDto.Details.Select(s =>
            {
                var detailEntity = s.ToEntity<QualEnvParameterGroupDetailEntity>();
                detailEntity.Id = IdGenProvider.Instance.CreateId();
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
                rows += await _qualEnvParameterGroupRepository.UpdateAsync(entity);
                rows += await _qualEnvParameterGroupDetailRepository.DeleteByParentIdAsync(command);
                rows += await _qualEnvParameterGroupDetailRepository.InsertRangeAsync(details);
                trans.Complete();
            }
            return rows;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> DeleteQualEnvParameterGroupAsync(long id)
        {
            return await _qualEnvParameterGroupRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeletesQualEnvParameterGroupAsync(long[] ids)
        {
            return await _qualEnvParameterGroupRepository.DeletesAsync(new DeleteCommand
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
        public async Task<QualEnvParameterGroupInfoDto?> QueryQualEnvParameterGroupByIdAsync(long id)
        {
            var qualEnvParameterGroupEntity = await _qualEnvParameterGroupRepository.GetByIdAsync(id);
            if (qualEnvParameterGroupEntity == null) return null;

            return qualEnvParameterGroupEntity.ToModel<QualEnvParameterGroupInfoDto>();
        }

        /// <summary>
        /// 根据ID获取项目明细列表
        /// </summary>
        /// <param name="parameterVerifyEnvId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<QualEnvParameterGroupDetailDto>> QueryDetailsByParameterVerifyEnvIdAsync(long parameterVerifyEnvId)
        {
            var qualEnvParameterGroupDetailEntities = await _qualEnvParameterGroupDetailRepository.GetEntitiesAsync(new QualEnvParameterGroupDetailQuery
            {
                ParameterVerifyEnvId = parameterVerifyEnvId
            });

            return qualEnvParameterGroupDetailEntities.Select(s => s.ToModel<QualEnvParameterGroupDetailDto>());
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<QualEnvParameterGroupDto>> GetPagedListAsync(QualEnvParameterGroupPagedQueryDto pagedQueryDto)
        {
            var pagedQuery = pagedQueryDto.ToQuery<QualEnvParameterGroupPagedQuery>();
            pagedQuery.SiteId = _currentSite.SiteId ?? 0;
            var pagedInfo = await _qualEnvParameterGroupRepository.GetPagedInfoAsync(pagedQuery);

            // 实体到DTO转换 装载数据
            var dtos = pagedInfo.Data.Select(s => s.ToModel<QualEnvParameterGroupDto>());
            return new PagedInfo<QualEnvParameterGroupDto>(dtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

    }
}
