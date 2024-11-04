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
using Hymson.MES.Data.Repositories.Quality;
using Hymson.MES.Data.Repositories.Quality.IQualityRepository;
using Hymson.MES.Data.Repositories.Quality.QualUnqualifiedCode.Query;
using Hymson.MES.Data.Repositories.Quality.QualUnqualifiedGroup.Query;
using Hymson.MES.Services.Dtos.Quality;
using Hymson.MES.Services.Services.Quality.IQualityService;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Services.Services.Quality
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

        /// <summary>
        /// 不合格代码服务
        /// </summary>
        /// <param name="qualUnqualifiedCodeRepository"></param>
        /// <param name="qualUnqualifiedGroupRepository"></param>
        /// <param name="validationCreateRules"></param>
        /// <param name="validationModifyRules"></param>
        /// <param name="currentUser"></param>
        /// <param name="currentSite"></param>
        public QualUnqualifiedCodeService(IQualUnqualifiedCodeRepository qualUnqualifiedCodeRepository, IQualUnqualifiedGroupRepository qualUnqualifiedGroupRepository, AbstractValidator<QualUnqualifiedCodeCreateDto> validationCreateRules, AbstractValidator<QualUnqualifiedCodeModifyDto> validationModifyRules, ICurrentUser currentUser, ICurrentSite currentSite)
        {
            _qualUnqualifiedCodeRepository = qualUnqualifiedCodeRepository;
            _qualUnqualifiedGroupRepository = qualUnqualifiedGroupRepository;
            _validationCreateRules = validationCreateRules;
            _validationModifyRules = validationModifyRules;
            _currentUser = currentUser;
            _currentSite = currentSite;
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="pram"></param>
        /// <returns></returns>
        public async Task<PagedInfo<QualUnqualifiedCodeDto>> GetPageListAsync(QualUnqualifiedCodePagedQueryDto pram)
        {
            var qualUnqualifiedCodePagedQuery = pram.ToQuery<QualUnqualifiedCodePagedQuery>();
            qualUnqualifiedCodePagedQuery.SiteId = _currentSite.SiteId ?? 123456; // TODO _currentSite.Name;
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
                SiteId = _currentSite.SiteId ?? 123456,
                UnqualifiedGroupId = groupId,
                StatusArr = new SysDataStatusEnum[] { SysDataStatusEnum.Enable, SysDataStatusEnum.Retain }
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
        /// 新增
        /// </summary>
        /// <param name="param">新增参数</param>
        /// <returns></returns>
        /// <exception cref="ValidationException">参数为空</exception>
        /// <exception cref="BusinessException">编码复用</exception>
        public async Task CreateQualUnqualifiedCodeAsync(QualUnqualifiedCodeCreateDto param)
        {
            if (param == null)
            {
                throw new ValidationException(nameof(ErrorCode.MES10100));
            }

            param.UnqualifiedCode = param.UnqualifiedCode.ToTrimSpace().ToUpperInvariant();
            param.UnqualifiedCodeName = param.UnqualifiedCodeName.Trim();

            //验证DTO
            await _validationCreateRules.ValidateAndThrowAsync(param);

            var qualUnqualifiedCodeEntity = await _qualUnqualifiedCodeRepository.GetByCodeAsync(new QualUnqualifiedCodeByCodeQuery { Code = param.UnqualifiedCode, Site = _currentSite.SiteId });
            if (qualUnqualifiedCodeEntity != null)
            {
                throw new BusinessException(nameof(ErrorCode.MES11108)).WithData("code", param.UnqualifiedCode);
            }
            var userId = _currentUser.UserName;
            //DTO转换实体
            qualUnqualifiedCodeEntity = param.ToEntity<QualUnqualifiedCodeEntity>();
            qualUnqualifiedCodeEntity.Id = IdGenProvider.Instance.CreateId();
            qualUnqualifiedCodeEntity.CreatedBy = userId;
            qualUnqualifiedCodeEntity.UpdatedBy = userId;
            qualUnqualifiedCodeEntity.SiteId = _currentSite.SiteId ?? 123456;
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
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeletesQualUnqualifiedCodeAsync(long[] ids)
        {
            var qualUnqualifiedList= await _qualUnqualifiedCodeRepository.GetByIdsAsync(ids);
            if (qualUnqualifiedList != null&& qualUnqualifiedList.Any(x=>x.Status!= SysDataStatusEnum.Build))
            {
                throw new BusinessException(nameof(ErrorCode.MES10106));
            }
            var userId = _currentUser.UserName;
            return await _qualUnqualifiedCodeRepository.DeletesAsync(new DeleteCommand { Ids = ids, DeleteOn = HymsonClock.Now(), UserId = userId });
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        /// <exception cref="ValidationException">参数为空</exception>
        public async Task ModifyQualUnqualifiedCodeAsync(QualUnqualifiedCodeModifyDto param)
        {
            if (param == null)
            {
                throw new ValidationException(nameof(ErrorCode.MES10100));
            }
            param.UnqualifiedCodeName = param.UnqualifiedCodeName.Trim();
            //验证DTO
            await _validationModifyRules.ValidateAndThrowAsync(param);
            var qualUnqualifiedEntity = await _qualUnqualifiedCodeRepository.GetByIdAsync(param.Id);
            if (qualUnqualifiedEntity != null && qualUnqualifiedEntity.Status != SysDataStatusEnum.Build && param.Status == SysDataStatusEnum.Build)
            {
                throw new BusinessException(nameof(ErrorCode.MES11111));
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
                        UnqualifiedGroupId = qualUnqualifiedCodeEntity.Id,
                        UnqualifiedCodeId = item,
                        CreatedBy = userId,
                        UpdatedBy = userId
                    });
                }
            }

            using var ts = TransactionHelper.GetTransactionScope();
            //更新不合格代码表
            await _qualUnqualifiedCodeRepository.UpdateAsync(qualUnqualifiedCodeEntity);
            //TODO 清理关联表
            await _qualUnqualifiedGroupRepository.RealDelteQualUnqualifiedCodeGroupRelationAsync(param.Id);

            if (list != null && list.Any())
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
    }
}
