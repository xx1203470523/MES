using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.Localization.Services;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Quality;
using Hymson.MES.Core.Enums;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Quality;
using Hymson.MES.Services.Dtos.Common;
using Hymson.MES.Services.Dtos.Quality;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;

namespace Hymson.MES.Services.Services.Quality.QualUnqualifiedCode
{
    /// <summary>
    /// 不合格代码服务
    /// @author wangkeming
    /// @date 2023-02-11 04:45:25
    /// </summary>
    public class QualUnqualifiedCodeService : IQualUnqualifiedCodeService
    {
        private readonly IQualUnqualifiedCodeRepository _qualUnqualifiedCodeRepository;
        private readonly IQualUnqualifiedGroupRepository _qualUnqualifiedGroupRepository;
        private readonly AbstractValidator<QualUnqualifiedCodeCreateDto> _validationCreateRules;
        private readonly AbstractValidator<QualUnqualifiedCodeModifyDto> _validationModifyRules;
        private readonly ICurrentUser _currentUser;
        private readonly ICurrentSite _currentSite;

        private readonly ILocalizationService _localizationService;

        /// <summary>
        /// 不合格代码服务
        /// </summary>
        /// <param name="qualUnqualifiedCodeRepository"></param>
        /// <param name="qualUnqualifiedGroupRepository"></param>
        /// <param name="validationCreateRules"></param>
        /// <param name="validationModifyRules"></param>
        /// <param name="currentUser"></param>
        /// <param name="currentSite"></param>
        /// <param name="localizationService"></param>
        public QualUnqualifiedCodeService(IQualUnqualifiedCodeRepository qualUnqualifiedCodeRepository, IQualUnqualifiedGroupRepository qualUnqualifiedGroupRepository, AbstractValidator<QualUnqualifiedCodeCreateDto> validationCreateRules, AbstractValidator<QualUnqualifiedCodeModifyDto> validationModifyRules, ICurrentUser currentUser, ICurrentSite currentSite, ILocalizationService localizationService)
        {
            _qualUnqualifiedCodeRepository = qualUnqualifiedCodeRepository;
            _qualUnqualifiedGroupRepository = qualUnqualifiedGroupRepository;
            _validationCreateRules = validationCreateRules;
            _validationModifyRules = validationModifyRules;
            _currentUser = currentUser;
            _currentSite = currentSite;
            _localizationService = localizationService;
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="qualUnqualifiedCodePagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<QualUnqualifiedCodeDto>> GetPageListAsync(QualUnqualifiedCodePagedQueryDto qualUnqualifiedCodePagedQueryDto)
        {
            var qualUnqualifiedCodePagedQuery = qualUnqualifiedCodePagedQueryDto.ToQuery<QualUnqualifiedCodePagedQuery>();

            qualUnqualifiedCodePagedQuery.SiteId = _currentSite.SiteId.GetValueOrDefault();

            if (qualUnqualifiedCodePagedQueryDto.ProcedureId.HasValue)
            {
                var qualUnqualifiedGroupEntities = await _qualUnqualifiedGroupRepository.GetListByProcedureIdAsync(new QualUnqualifiedGroupQuery
                {
                    ProcedureId = qualUnqualifiedCodePagedQueryDto.ProcedureId.GetValueOrDefault(),
                    SiteId = _currentSite.SiteId.GetValueOrDefault()
                });
                var qualUnqualifiedGroupIds = qualUnqualifiedGroupEntities.Select(m => m.Id);

                var qualUnqualifiedCodeEntities = await _qualUnqualifiedCodeRepository.GetListByGroupIdAsync(new QualUnqualifiedCodeQuery
                {
                    UnqualifiedGroupIds = qualUnqualifiedGroupIds,
                    SiteId = _currentSite.SiteId.GetValueOrDefault()
                });
                qualUnqualifiedCodePagedQuery.Ids = qualUnqualifiedCodeEntities.Select(m => m.Id);
                if (!qualUnqualifiedCodePagedQuery.Ids.Any())
                {
                    qualUnqualifiedCodePagedQuery.Ids = new List<long>() { 0 };
                }
            }

            var pagedInfo = await _qualUnqualifiedCodeRepository.GetPagedInfoAsync(qualUnqualifiedCodePagedQuery);

            //实体到DTO转换 装载数据
            List<QualUnqualifiedCodeDto> qualUnqualifiedCodeDtos = PrepareQualUnqualifiedCodeDtos(pagedInfo);
            return new PagedInfo<QualUnqualifiedCodeDto>(qualUnqualifiedCodeDtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<QualUnqualifiedCodeDto> QueryQualUnqualifiedCodeByIdAsync(long id)
        {
            var qualUnqualifiedCodeEntity = await _qualUnqualifiedCodeRepository.GetByIdAsync(id);

            QualUnqualifiedCodeDto qualUnqualifiedCodeDto = new QualUnqualifiedCodeDto();
            if (qualUnqualifiedCodeEntity != null)
            {
                qualUnqualifiedCodeDto = qualUnqualifiedCodeEntity.ToModel<QualUnqualifiedCodeDto>();
            }
            return qualUnqualifiedCodeDto;
        }

        /// <summary>
        /// 获取不合格代码组
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<List<UnqualifiedCodeGroupRelationDto>> GetQualUnqualifiedCodeGroupRelationByIdAsync(long id)
        {
            var unqualifiedCodeGroupRelations = await _qualUnqualifiedCodeRepository.GetQualUnqualifiedCodeGroupRelationAsync(id);
            var nqualifiedCodeGroupRelationList = new List<UnqualifiedCodeGroupRelationDto>();
            if (unqualifiedCodeGroupRelations != null && unqualifiedCodeGroupRelations.Any())
            {

                foreach (var item in unqualifiedCodeGroupRelations)
                {
                    nqualifiedCodeGroupRelationList.Add(new UnqualifiedCodeGroupRelationDto()
                    {
                        Id = item.Id,
                        UnqualifiedGroupId = item.UnqualifiedGroupId,
                        UnqualifiedGroupName = item.UnqualifiedGroupName,
                        UnqualifiedGroup = item.UnqualifiedGroup,
                        CreatedBy = item.CreatedBy,
                        CreatedOn = item.CreatedOn,
                        UpdatedBy = item.UpdatedBy,
                        UpdatedOn = item.UpdatedOn
                    });
                }
            }
            return nqualifiedCodeGroupRelationList;
        }

        /// <summary>
        /// 根据不合格代码组id查询不合格代码列表
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<QualUnqualifiedCodeDto>> GetListByGroupIdAsync(long groupId)
        {
            var query = new QualUnqualifiedCodeQuery
            {
                SiteId = _currentSite.SiteId ?? 0,
                UnqualifiedGroupId = groupId,
                Status = SysDataStatusEnum.Enable
                // StatusArr = new SysDataStatusEnum[] { SysDataStatusEnum.Enable, SysDataStatusEnum.Retain }
            };
            var list = await _qualUnqualifiedCodeRepository.GetListByGroupIdAsync(query);

            //实体到DTO转换 装载数据
            var qualUnqualifiedCodes = new List<QualUnqualifiedCodeDto>();
            foreach (var entity in list)
            {
                var unqualifiedCodeDto = entity.ToModel<QualUnqualifiedCodeDto>();
                qualUnqualifiedCodes.Add(unqualifiedCodeDto);
            }
            return qualUnqualifiedCodes;
        }

        /// <summary>
        /// 根据工序id查询不合格代码列表
        /// </summary>
        /// <param name="procedureId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<QualUnqualifiedCodeDto>> GetListByProcedureIdAsync(long procedureId)
        {
            //实体到DTO转换 装载数据
            var qualUnqualifiedCodes = new List<QualUnqualifiedCodeDto>();
            var groupQuery = new QualUnqualifiedGroupQuery
            {
                SiteId = _currentSite.SiteId ?? 0,
                ProcedureId = procedureId
            };
            var list = await _qualUnqualifiedGroupRepository.GetListByProcedureIdAsync(groupQuery);
            if (list == null || !list.Any())
            {
                return qualUnqualifiedCodes;
            }

            var groupIds = list.Select(x => x.Id).ToArray();
            var query = new QualUnqualifiedCodeQuery
            {
                SiteId = _currentSite.SiteId ?? 0,
                UnqualifiedGroupIds = groupIds,
                Status = SysDataStatusEnum.Enable
            };
            var qualUnqualifieds = await _qualUnqualifiedCodeRepository.GetListByGroupIdAsync(query);
            var qualCodes = new List<string>();

            foreach (var entity in qualUnqualifieds)
            {
                if (qualCodes.Contains(entity.UnqualifiedCode))
                {
                    continue;
                }

                qualCodes.Add(entity.UnqualifiedCode);
                var unqualifiedCodeDto = entity.ToModel<QualUnqualifiedCodeDto>();
                qualUnqualifiedCodes.Add(unqualifiedCodeDto);
            }
            return qualUnqualifiedCodes;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="param">新增参数</param>
        /// <returns></returns>
        public async Task<long> CreateQualUnqualifiedCodeAsync(QualUnqualifiedCodeCreateDto param)
        {
            param.UnqualifiedCode = param.UnqualifiedCode.ToTrimSpace().ToUpperInvariant();
            param.UnqualifiedCodeName = param.UnqualifiedCodeName.Trim();

            //验证DTO
            await _validationCreateRules.ValidateAndThrowAsync(param);

            var qualUnqualifiedCodeEntity = await _qualUnqualifiedCodeRepository.GetByCodeAsync(new QualUnqualifiedCodeByCodeQuery { Code = param.UnqualifiedCode, Site = _currentSite.SiteId });
            if (qualUnqualifiedCodeEntity != null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES11108)).WithData("code", param.UnqualifiedCode);
            }
            var userId = _currentUser.UserName;
            //DTO转换实体
            qualUnqualifiedCodeEntity = param.ToEntity<QualUnqualifiedCodeEntity>();
            qualUnqualifiedCodeEntity.Id = IdGenProvider.Instance.CreateId();
            qualUnqualifiedCodeEntity.CreatedBy = userId;
            qualUnqualifiedCodeEntity.UpdatedBy = userId;
            qualUnqualifiedCodeEntity.SiteId = _currentSite.SiteId ?? 0;

            qualUnqualifiedCodeEntity.Status = SysDataStatusEnum.Build;

            List<QualUnqualifiedCodeGroupRelation> list = new List<QualUnqualifiedCodeGroupRelation>();
            if (param.UnqualifiedGroupIds != null && param.UnqualifiedGroupIds.Any())
            {
                foreach (var item in param.UnqualifiedGroupIds)
                {
                    list.Add(new QualUnqualifiedCodeGroupRelation
                    {
                        Id = IdGenProvider.Instance.CreateId(),
                        UnqualifiedGroupId = item,
                        UnqualifiedCodeId = qualUnqualifiedCodeEntity.Id,
                        SiteId = _currentSite.SiteId ?? 0,
                        CreatedBy = userId,
                        UpdatedBy = userId
                    });
                }
            }

            using var ts = TransactionHelper.GetTransactionScope();
            //插入不合格代码表
            await _qualUnqualifiedCodeRepository.InsertAsync(qualUnqualifiedCodeEntity);
            if (list != null && list.Any())
            {
                await _qualUnqualifiedGroupRepository.InsertQualUnqualifiedCodeGroupRelationRangAsync(list);
            }
            ts.Complete();
            return qualUnqualifiedCodeEntity.Id;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeletesQualUnqualifiedCodeAsync(long[] ids)
        {
            var qualUnqualifiedList = await _qualUnqualifiedCodeRepository.GetByIdsAsync(ids);
            if (qualUnqualifiedList != null && qualUnqualifiedList.Any(x => x.Status != SysDataStatusEnum.Build))
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10106));
            }
            var userId = _currentUser.UserName;
            return await _qualUnqualifiedCodeRepository.DeletesAsync(new DeleteCommand { Ids = ids, DeleteOn = HymsonClock.Now(), UserId = userId });
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task ModifyQualUnqualifiedCodeAsync(QualUnqualifiedCodeModifyDto param)
        {
            param.UnqualifiedCodeName = param.UnqualifiedCodeName.Trim();
            //验证DTO
            await _validationModifyRules.ValidateAndThrowAsync(param);
            var qualUnqualifiedEntity = await _qualUnqualifiedCodeRepository.GetByIdAsync(param.Id);
            //验证某些状态是不能编辑的
            var canEditStatusEnum = new SysDataStatusEnum[] { SysDataStatusEnum.Build, SysDataStatusEnum.Retain };
            if (!canEditStatusEnum.Any(x => x == qualUnqualifiedEntity.Status))
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10129));
            }

            var userId = _currentUser.UserName;
            //DTO转换实体
            var qualUnqualifiedCodeEntity = param.ToEntity<QualUnqualifiedCodeEntity>();
            qualUnqualifiedCodeEntity.UpdatedBy = userId;
            qualUnqualifiedCodeEntity.UpdatedOn = HymsonClock.Now();

            List<QualUnqualifiedCodeGroupRelation> list = new List<QualUnqualifiedCodeGroupRelation>();
            if (param.UnqualifiedGroupIds != null && param.UnqualifiedGroupIds.Any())
            {
                foreach (var item in param.UnqualifiedGroupIds)
                {
                    list.Add(new QualUnqualifiedCodeGroupRelation
                    {
                        Id = IdGenProvider.Instance.CreateId(),
                        UnqualifiedGroupId = item,
                        UnqualifiedCodeId = qualUnqualifiedCodeEntity.Id,
                        SiteId = _currentSite.SiteId ?? 0,
                        CreatedBy = userId,
                        UpdatedBy = userId
                    });
                }
            }

            using var ts = TransactionHelper.GetTransactionScope();
            //更新不合格代码表
            await _qualUnqualifiedCodeRepository.UpdateAsync(qualUnqualifiedCodeEntity);
            //TODO 清理关联表
            await _qualUnqualifiedGroupRepository.RealDelteQualUnqualifiedCodeGroupRelationByUnqualifiedIdAsync(param.Id);

            if (list.Any())
            {
                //插入不合格代码
                await _qualUnqualifiedGroupRepository.InsertQualUnqualifiedCodeGroupRelationRangAsync(list);
            }
            ts.Complete();
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="pagedInfo"></param>
        /// <returns></returns>
        private static List<QualUnqualifiedCodeDto> PrepareQualUnqualifiedCodeDtos(PagedInfo<QualUnqualifiedCodeEntity> pagedInfo)
        {
            var qualUnqualifiedCodeDtos = new List<QualUnqualifiedCodeDto>();
            foreach (var qualUnqualifiedCodeEntity in pagedInfo.Data)
            {
                var qualUnqualifiedCodeDto = qualUnqualifiedCodeEntity.ToModel<QualUnqualifiedCodeDto>();
                qualUnqualifiedCodeDtos.Add(qualUnqualifiedCodeDto);
            }
            return qualUnqualifiedCodeDtos;
        }

        #region 状态变更
        /// <summary>
        /// 状态变更
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task UpdateStatusAsync(ChangeStatusDto param)
        {
            #region 参数校验
            if (param.Id == 0)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10125));
            }
            if (!Enum.IsDefined(typeof(SysDataStatusEnum), param.Status))
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10126));
            }
            if (param.Status == SysDataStatusEnum.Build)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10128));
            }

            #endregion

            var changeStatusCommand = new ChangeStatusCommand()
            {
                Id = param.Id,
                Status = param.Status,

                UpdatedBy = _currentUser.UserName,
                UpdatedOn = HymsonClock.Now()
            };

            #region 校验数据
            var entity = await _qualUnqualifiedCodeRepository.GetByIdAsync(changeStatusCommand.Id);
            if (entity == null || entity.IsDeleted != 0)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES11115));
            }
            if (entity.Status == changeStatusCommand.Status)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10127)).WithData("status", _localizationService.GetResource($"{typeof(SysDataStatusEnum).FullName}.{Enum.GetName(typeof(SysDataStatusEnum), entity.Status)}"));
            }
            #endregion

            #region 操作数据库
            await _qualUnqualifiedCodeRepository.UpdateStatusAsync(changeStatusCommand);
            #endregion
        }

        #endregion
    }
}
