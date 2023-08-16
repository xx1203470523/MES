using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Quality;
using Hymson.MES.Core.Enums;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.Quality;
using Hymson.MES.Data.Repositories.Quality.Query;
using Hymson.MES.Services.Dtos.Quality;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;

namespace Hymson.MES.Services.Services.Quality
{
    /// <summary>
    /// 服务（IPQC检验项目） 
    /// </summary>
    public class QualIpqcInspectionService : IQualIpqcInspectionService
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
        private readonly AbstractValidator<QualIpqcInspectionSaveDto> _validationSaveRules;

        /// <summary>
        /// 仓储接口（IPQC检验项目）
        /// </summary>
        private readonly IQualIpqcInspectionRepository _qualIpqcInspectionRepository;
        private readonly IQualIpqcInspectionParameterRepository _qualIpqcInspectionParameterRepository;
        private readonly IQualIpqcInspectionRuleRepository _qualIpqcInspectionRuleRepository;
        private readonly IQualIpqcInspectionRuleResourceRelationRepository _qualIpqcInspectionRuleResourceRelationRepository;
        private readonly IProcParameterRepository _procParameterRepository;
        private readonly IQualInspectionParameterGroupRepository _procParameterGroupRepository;
        private readonly IProcMaterialRepository _procMaterialRepository;
        private readonly IProcProcedureRepository _procProcedureRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        public QualIpqcInspectionService(ICurrentUser currentUser,
            ICurrentSite currentSite,
            AbstractValidator<QualIpqcInspectionSaveDto> validationSaveRules,
            IQualIpqcInspectionRepository qualIpqcInspectionRepository,
            IQualIpqcInspectionParameterRepository qualIpqcInspectionParameterRepository,
            IQualIpqcInspectionRuleRepository qualIpqcInspectionRuleRepository,
            IQualIpqcInspectionRuleResourceRelationRepository qualIpqcInspectionRuleResourceRelationRepository,
            IProcParameterRepository procParameterRepository,
            IQualInspectionParameterGroupRepository qualInspectionParameterGroupRepository,
            IProcMaterialRepository procMaterialRepository,
            IProcProcedureRepository procProcedureRepository)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _validationSaveRules = validationSaveRules;
            _qualIpqcInspectionRepository = qualIpqcInspectionRepository;
            _qualIpqcInspectionParameterRepository = qualIpqcInspectionParameterRepository;
            _qualIpqcInspectionRuleRepository = qualIpqcInspectionRuleRepository;
            _qualIpqcInspectionRuleResourceRelationRepository = qualIpqcInspectionRuleResourceRelationRepository;
            _procParameterRepository = procParameterRepository;
            _procParameterGroupRepository = qualInspectionParameterGroupRepository;
            _procMaterialRepository = procMaterialRepository;
            _procProcedureRepository = procProcedureRepository;
        }


        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        public async Task<int> CreateAsync(QualIpqcInspectionSaveDto saveDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0) throw new ValidationException(nameof(ErrorCode.MES10101));

            // 验证DTO
            await _validationSaveRules.ValidateAndThrowAsync(saveDto);

            // 更新时间
            var updatedBy = _currentUser.UserName;
            var updatedOn = HymsonClock.Now();

            // 唯一性校验
            var isExist = await _qualIpqcInspectionRepository.IsExistAsync(new QualIpqcInspectionQuery
            {
                SiteId = _currentSite.SiteId ?? 0,
                InspectionParameterGroupId = saveDto.InspectionParameterGroupId,
                Type = saveDto.Type,
                Version = saveDto.Version
            });
            if (isExist)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES13101)).WithData("Code", saveDto.ParameterGroupCode);
            }

            // DTO转换实体
            var entity = saveDto.ToEntity<QualIpqcInspectionEntity>();
            entity.Id = IdGenProvider.Instance.CreateId();
            entity.CreatedBy = updatedBy;
            entity.CreatedOn = updatedOn;
            entity.UpdatedBy = updatedBy;
            entity.UpdatedOn = updatedOn;
            entity.SiteId = _currentSite.SiteId ?? 0;

            //参数项目
            IEnumerable<QualIpqcInspectionParameterEntity>? details = null;
            if (saveDto.Details != null && saveDto.Details.Any())
            {
                details = saveDto.Details.Select(s =>
                {
                    var detailEntity = s.ToEntity<QualIpqcInspectionParameterEntity>();
                    detailEntity.Id = IdGenProvider.Instance.CreateId();
                    detailEntity.IpqcInspectionId = entity.Id;
                    detailEntity.CreatedBy = updatedBy;
                    detailEntity.CreatedOn = updatedOn;
                    detailEntity.UpdatedBy = updatedBy;
                    detailEntity.UpdatedOn = updatedOn;
                    return detailEntity;
                });
            }

            //检验规则&资源
            List<QualIpqcInspectionRuleEntity>? rules = null;
            IEnumerable<QualIpqcInspectionRuleResourceRelationEntity>? ruleResources = null;
            if (saveDto.Rules != null && saveDto.Rules.Any())
            {
                rules = new List<QualIpqcInspectionRuleEntity>();
                foreach (var item in saveDto.Rules)
                {
                    var ruleEntity = item.ToEntity<QualIpqcInspectionRuleEntity>();
                    ruleEntity.Id = IdGenProvider.Instance.CreateId();
                    ruleEntity.SiteId = entity.SiteId;
                    ruleEntity.CreatedBy = updatedBy;
                    ruleEntity.CreatedOn = updatedOn;
                    ruleEntity.UpdatedBy = updatedBy;
                    ruleEntity.UpdatedOn = updatedOn;

                    //关联资源
                    if (item.Resources != null && item.Resources.Any())
                    {
                        ruleResources = item.Resources.Select(s =>
                        {
                            var ruleResourceEntity = s.ToEntity<QualIpqcInspectionRuleResourceRelationEntity>();
                            ruleResourceEntity.Id = IdGenProvider.Instance.CreateId();
                            ruleResourceEntity.SiteId = entity.SiteId;
                            ruleResourceEntity.IpqcInspectionRuleId = ruleResourceEntity.Id;
                            ruleResourceEntity.CreatedBy = updatedBy;
                            ruleResourceEntity.CreatedOn = updatedOn;
                            ruleResourceEntity.UpdatedBy = updatedBy;
                            ruleResourceEntity.UpdatedOn = updatedOn;

                            return ruleResourceEntity;
                        });
                    }

                    rules.Add(ruleEntity);
                }
            }

            var rows = 0;
            using (var trans = TransactionHelper.GetTransactionScope())
            {
                rows += await _qualIpqcInspectionRepository.InsertAsync(entity);
                if (details != null && details.Any())
                {
                    rows += await _qualIpqcInspectionParameterRepository.InsertRangeAsync(details);
                }
                if (rules != null && rules.Any())
                {
                    rows += await _qualIpqcInspectionRuleRepository.InsertRangeAsync(rules);
                }
                if (ruleResources != null && ruleResources.Any())
                {
                    rows += await _qualIpqcInspectionRuleResourceRelationRepository.InsertRangeAsync(ruleResources);
                }
                trans.Complete();
            }
            return rows;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        public async Task<int> ModifyAsync(QualIpqcInspectionSaveDto saveDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0) throw new ValidationException(nameof(ErrorCode.MES10101));

            // 验证DTO
            await _validationSaveRules.ValidateAndThrowAsync(saveDto);

            // 更新时间
            var updatedBy = _currentUser.UserName;
            var updatedOn = HymsonClock.Now();

            var dbEntity = await _qualIpqcInspectionRepository.GetByIdAsync(saveDto.Id);

            #region 校验

            if (dbEntity == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10104));
            }

            #endregion

            // DTO转换实体
            var entity = saveDto.ToEntity<QualIpqcInspectionEntity>();
            entity.CreatedBy = dbEntity.CreatedBy;
            entity.CreatedOn = dbEntity.CreatedOn;
            entity.UpdatedBy = updatedBy;
            entity.UpdatedOn = updatedOn;

            //参数项目
            IEnumerable<QualIpqcInspectionParameterEntity>? details = null;
            if (saveDto.Details != null && saveDto.Details.Any())
            {
                details = saveDto.Details.Select(s =>
                {
                    var detailEntity = s.ToEntity<QualIpqcInspectionParameterEntity>();
                    detailEntity.Id = IdGenProvider.Instance.CreateId();
                    detailEntity.IpqcInspectionId = entity.Id;
                    detailEntity.CreatedBy = updatedBy;
                    detailEntity.CreatedOn = updatedOn;
                    detailEntity.UpdatedBy = updatedBy;
                    detailEntity.UpdatedOn = updatedOn;
                    return detailEntity;
                });
            }

            //检验规则&资源
            List<QualIpqcInspectionRuleEntity>? rules = null;
            IEnumerable<QualIpqcInspectionRuleResourceRelationEntity>? ruleResources = null;
            if (saveDto.Rules != null && saveDto.Rules.Any())
            {
                rules = new List<QualIpqcInspectionRuleEntity>();
                foreach (var item in saveDto.Rules)
                {
                    var ruleEntity = item.ToEntity<QualIpqcInspectionRuleEntity>();
                    ruleEntity.Id = IdGenProvider.Instance.CreateId();
                    ruleEntity.SiteId = entity.SiteId;
                    ruleEntity.CreatedBy = updatedBy;
                    ruleEntity.CreatedOn = updatedOn;
                    ruleEntity.UpdatedBy = updatedBy;
                    ruleEntity.UpdatedOn = updatedOn;

                    //关联资源
                    if (item.Resources != null && item.Resources.Any())
                    {
                        ruleResources = item.Resources.Select(s =>
                        {
                            var ruleResourceEntity = s.ToEntity<QualIpqcInspectionRuleResourceRelationEntity>();
                            ruleResourceEntity.Id = IdGenProvider.Instance.CreateId();
                            ruleResourceEntity.SiteId = entity.SiteId;
                            ruleResourceEntity.IpqcInspectionRuleId = ruleResourceEntity.Id;
                            ruleResourceEntity.CreatedBy = updatedBy;
                            ruleResourceEntity.CreatedOn = updatedOn;
                            ruleResourceEntity.UpdatedBy = updatedBy;
                            ruleResourceEntity.UpdatedOn = updatedOn;

                            return ruleResourceEntity;
                        });
                    }

                    rules.Add(ruleEntity);
                }
            }

            var rows = 0;
            using (var trans = TransactionHelper.GetTransactionScope())
            {
                rows += await _qualIpqcInspectionRepository.UpdateAsync(entity);
                rows += await _qualIpqcInspectionParameterRepository.DeleteReallyByMainIdAsync(entity.Id);
                rows += await _qualIpqcInspectionRuleRepository.DeleteReallyByMainIdAsync(entity.Id);
                rows += await _qualIpqcInspectionRuleResourceRelationRepository.DeleteReallyByMainIdAsync(entity.Id);
                if (details != null && details.Any())
                {
                    rows += await _qualIpqcInspectionParameterRepository.InsertRangeAsync(details);
                }
                if (rules != null && rules.Any())
                {
                    rows += await _qualIpqcInspectionRuleRepository.InsertRangeAsync(rules);
                }
                if (ruleResources != null && ruleResources.Any())
                {
                    rows += await _qualIpqcInspectionRuleResourceRelationRepository.InsertRangeAsync(ruleResources);
                }
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
            return await _qualIpqcInspectionRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeletesAsync(long[] ids)
        {
            var list = await _qualIpqcInspectionRepository.GetByIdsAsync(ids);
            if (list != null && list.Any(x => x.Status != SysDataStatusEnum.Build))
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10106));
            }

            var command = new DeleteCommand
            {
                Ids = ids,
                DeleteOn = HymsonClock.Now(),
                UserId = _currentUser.UserName
            };

            var rows = 0;
            using (var trans = TransactionHelper.GetTransactionScope())
            {
                rows += await _qualIpqcInspectionRepository.DeletesAsync(command);
                foreach (var id in ids)
                {
                    var delCommand = new DeleteByParentIdCommand
                    {
                        ParentId = id,
                        UpdatedBy = _currentUser.UserName,
                        UpdatedOn = command.DeleteOn
                    };
                    rows += await _qualIpqcInspectionParameterRepository.DeleteByMainIdAsync(delCommand);
                    rows += await _qualIpqcInspectionRuleRepository.DeleteByMainIdAsync(delCommand);
                    rows += await _qualIpqcInspectionRuleResourceRelationRepository.DeleteByMainIdAsync(delCommand);
                }

                trans.Complete();
            }
            return rows;
        }

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<QualIpqcInspectionDto?> QueryByIdAsync(long id)
        {
            var qualIpqcInspectionEntity = await _qualIpqcInspectionRepository.GetByIdAsync(id);
            if (qualIpqcInspectionEntity == null) return null;

            var dto = qualIpqcInspectionEntity.ToModel<QualIpqcInspectionDto>();
            if (dto == null) return null;

            var parameterGroupTask = _procParameterGroupRepository.GetByIdAsync(dto.InspectionParameterGroupId);
            var materialTask = _procMaterialRepository.GetByIdAsync(dto.MaterialId);
            var procedureTask = _procProcedureRepository.GetByIdAsync(dto.ProcedureId);

            var parameterGroupEntity = await parameterGroupTask;
            var materialEntity = await materialTask;
            var procedureEntity = await procedureTask;

            if (parameterGroupEntity != null)
            {
                dto.ParameterGroupCode = parameterGroupEntity.Code;
                dto.ParameterGroupName = parameterGroupEntity.Name;
                dto.ParameterGroupVersion = parameterGroupEntity.Version;
            }
            if (materialEntity != null)
            {
                dto.MaterialCode = materialEntity.MaterialCode;
                dto.MaterialName = materialEntity.MaterialName;
            }
            if (procedureEntity != null)
            {
                dto.ProcedureCode = procedureEntity.Code;
                dto.ProcedureName = procedureEntity.Name;
            }

            return dto;
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<QualIpqcInspectionViewDto>> GetPagedListAsync(QualIpqcInspectionPagedQueryDto pagedQueryDto)
        {
            var pagedQuery = pagedQueryDto.ToQuery<QualIpqcInspectionPagedQuery>();
            pagedQuery.SiteId = _currentSite.SiteId ?? 0;
            var pagedInfo = await _qualIpqcInspectionRepository.GetPagedListAsync(pagedQuery);

            // 实体到DTO转换 装载数据
            var dtos = pagedInfo.Data.Select(s => s.ToModel<QualIpqcInspectionViewDto>());
            return new PagedInfo<QualIpqcInspectionViewDto>(dtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 根据ID获取关联明细列表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IEnumerable<QualIpqcInspectionParameterDto>?> QueryDetailsByMainIdAsync(long id)
        {
            var ipqcParameters = await _qualIpqcInspectionParameterRepository.GetEntitiesAsync(new QualIpqcInspectionParameterQuery { IpqcInspectionId = id });

            // 查询已经缓存的参数实体
            var parameterEntities = await _procParameterRepository.GetProcParameterEntitiesAsync(new ProcParameterQuery
            {
                SiteId = _currentSite.SiteId ?? 0
            });

            List<QualIpqcInspectionParameterDto> dtos = new();
            foreach (var item in ipqcParameters)
            {
                var dto = item.ToModel<QualIpqcInspectionParameterDto>();
                var parameterEntity = parameterEntities.FirstOrDefault(f => f.Id == item.ParameterId);
                if (parameterEntity != null)
                {
                    dto.ParameterCode = parameterEntity.ParameterCode;
                    dto.ParameterName = parameterEntity.ParameterName;
                    dto.ParameterUnit = parameterEntity.ParameterUnit;
                    dto.DataType = parameterEntity.DataType;
                }

                dtos.Add(dto);
            }

            return dtos;
        }

    }
}
