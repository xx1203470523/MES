using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Quality;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Integrated;
using Hymson.MES.Data.Repositories.Quality;
using Hymson.MES.Data.Repositories.Quality.QualUnqualifiedGroup.Query;
using Hymson.MES.Services.Dtos.Quality;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;

namespace Hymson.MES.Services.Services.Quality
{
    /// <summary>
    /// 不合格代码组服务
    /// @author wangkeming
    /// @date 2023-02-11 04:45:25
    /// </summary>
    public class QualUnqualifiedGroupService : IQualUnqualifiedGroupService
    {
        private readonly IQualUnqualifiedGroupRepository _qualUnqualifiedGroupRepository;
        private readonly IInteJobBusinessRelationRepository _inteJobBusinessRelationRepository;
        private readonly AbstractValidator<QualUnqualifiedGroupCreateDto> _validationCreateRules;
        private readonly AbstractValidator<QualUnqualifiedGroupModifyDto> _validationModifyRules;
        private readonly ICurrentUser _currentUser;
        private readonly ICurrentSite _currentSite;

        /// <summary>
        /// 不合格代码组服务
        /// </summary>
        /// <param name="qualUnqualifiedGroupRepository"></param>
        /// <param name="validationCreateRules"></param>
        /// <param name="validationModifyRules"></param>
        public QualUnqualifiedGroupService(IQualUnqualifiedGroupRepository qualUnqualifiedGroupRepository, AbstractValidator<QualUnqualifiedGroupCreateDto> validationCreateRules, AbstractValidator<QualUnqualifiedGroupModifyDto> validationModifyRules, IInteJobBusinessRelationRepository inteJobBusinessRelationRepository, ICurrentUser currentUser, ICurrentSite currentSite)
        {
            _qualUnqualifiedGroupRepository = qualUnqualifiedGroupRepository;
            _inteJobBusinessRelationRepository = inteJobBusinessRelationRepository;
            _validationCreateRules = validationCreateRules;
            _validationModifyRules = validationModifyRules;
            _currentUser = currentUser;
            _currentSite = currentSite;
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<PagedInfo<QualUnqualifiedGroupDto>> GetPageListAsync(QualUnqualifiedGroupPagedQueryDto param)
        {
            var qualUnqualifiedGroupPagedQuery = param.ToQuery<QualUnqualifiedGroupPagedQuery>();
            var pagedInfo = await _qualUnqualifiedGroupRepository.GetPagedInfoAsync(qualUnqualifiedGroupPagedQuery);

            //实体到DTO转换 装载数据
            List<QualUnqualifiedGroupDto> qualUnqualifiedGroupDtos = PrepareQualUnqualifiedGroupDtos(pagedInfo);
            return new PagedInfo<QualUnqualifiedGroupDto>(qualUnqualifiedGroupDtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task CreateQualUnqualifiedGroupAsync(QualUnqualifiedGroupCreateDto param)
        {
            if (param == null)
            {
                throw new ValidationException(ErrorCode.MES10100);
            }
            await _validationCreateRules.ValidateAndThrowAsync(param);

            var qualUnqualifiedGroupEntity = await _qualUnqualifiedGroupRepository.GetByCodeAsync(new QualUnqualifiedGroupByCodeQuery { Code = param.UnqualifiedGroup, Site = _currentSite.Name });
            if (qualUnqualifiedGroupEntity != null)
            {
                throw new BusinessException(ErrorCode.MES11206).WithData("code", param.UnqualifiedGroup);
            }
            var userId = _currentUser.UserName;
            //DTO转换实体
            qualUnqualifiedGroupEntity = param.ToEntity<QualUnqualifiedGroupEntity>();
            qualUnqualifiedGroupEntity.Id = IdGenProvider.Instance.CreateId();
            qualUnqualifiedGroupEntity.CreatedBy = userId;
            qualUnqualifiedGroupEntity.UpdatedBy = userId;
            qualUnqualifiedGroupEntity.SiteCode = _currentSite.Name;

            List<QualUnqualifiedCodeGroupRelation> qualUnqualifiedCodeGroupRelationlist = new List<QualUnqualifiedCodeGroupRelation>();
            if (param.UnqualifiedCodeIds != null && param.UnqualifiedCodeIds.Any())
            {
                foreach (var item in param.UnqualifiedCodeIds)
                {
                    qualUnqualifiedCodeGroupRelationlist.Add(new QualUnqualifiedCodeGroupRelation
                    {
                        SiteCode = _currentSite.Name,
                        UnqualifiedGroupId = qualUnqualifiedGroupEntity.Id,
                        UnqualifiedCodeId = item,
                        CreatedBy = userId,
                        UpdatedBy = userId
                    });
                }
            }

            List<QualUnqualifiedGroupProcedureRelation> qualUnqualifiedGroupProcedureRelationList = new List<QualUnqualifiedGroupProcedureRelation>();
            if (param.UnqualifiedCodeIds != null && param.UnqualifiedCodeIds.Any())
            {
                foreach (var item in param.UnqualifiedCodeIds)
                {
                    qualUnqualifiedGroupProcedureRelationList.Add(new QualUnqualifiedGroupProcedureRelation
                    {
                        SiteCode = _currentSite.Name,
                        UnqualifiedGroupId = qualUnqualifiedGroupEntity.Id,
                        ProcedureId = item,
                        CreatedBy = userId,
                        UpdatedBy = userId
                    });
                }
            }

            using var ts = TransactionHelper.GetTransactionScope();
            await _qualUnqualifiedGroupRepository.InsertAsync(qualUnqualifiedGroupEntity);
            //不合格组关联不合格代码
            if (qualUnqualifiedCodeGroupRelationlist != null && qualUnqualifiedCodeGroupRelationlist.Any())
            {
                await _qualUnqualifiedGroupRepository.InsertQualUnqualifiedCodeGroupRelationRangAsync(qualUnqualifiedCodeGroupRelationlist);
            }
            //不合格组关联工序
            if (qualUnqualifiedGroupProcedureRelationList != null && qualUnqualifiedGroupProcedureRelationList.Any())
            {
                await _qualUnqualifiedGroupRepository.InsertQualUnqualifiedGroupProcedureRelationRangAsync(qualUnqualifiedGroupProcedureRelationList);
            }
            ts.Complete();
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeletesQualUnqualifiedGroupAsync(string ids)
        {
            long[] idsArr = StringExtension.SpitLongArrary(ids);
            var userId = _currentUser.UserName;
            return await _qualUnqualifiedGroupRepository.DeleteRangAsync(new DeleteCommand { Ids = idsArr, DeleteOn = HymsonClock.Now(), UserId = userId });
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="pagedInfo"></param>
        /// <returns></returns>
        private static List<QualUnqualifiedGroupDto> PrepareQualUnqualifiedGroupDtos(PagedInfo<QualUnqualifiedGroupEntity> pagedInfo)
        {
            var qualUnqualifiedGroupDtos = new List<QualUnqualifiedGroupDto>();
            foreach (var qualUnqualifiedGroupEntity in pagedInfo.Data)
            {
                var qualUnqualifiedGroupDto = qualUnqualifiedGroupEntity.ToModel<QualUnqualifiedGroupDto>();
                qualUnqualifiedGroupDtos.Add(qualUnqualifiedGroupDto);
            }

            return qualUnqualifiedGroupDtos;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task ModifyQualUnqualifiedGroupAsync(QualUnqualifiedGroupModifyDto param)
        {
            if (param == null)
            {
                throw new ValidationException(ErrorCode.MES10100);
            }
            //验证DTO
            await _validationModifyRules.ValidateAndThrowAsync(param);
            var userId = _currentUser.UserName;
            //DTO转换实体
            var qualUnqualifiedGroupEntity = param.ToEntity<QualUnqualifiedGroupEntity>();
            qualUnqualifiedGroupEntity.UpdatedBy = userId;
            qualUnqualifiedGroupEntity.UpdatedOn = HymsonClock.Now();

            List<QualUnqualifiedCodeGroupRelation> qualUnqualifiedCodeGroupRelationlist = new List<QualUnqualifiedCodeGroupRelation>();
            if (param.UnqualifiedCodeIds != null && param.UnqualifiedCodeIds.Any())
            {
                foreach (var item in param.UnqualifiedCodeIds)
                {
                    qualUnqualifiedCodeGroupRelationlist.Add(new QualUnqualifiedCodeGroupRelation
                    {
                        SiteCode = _currentSite.Name,
                        UnqualifiedGroupId = qualUnqualifiedGroupEntity.Id,
                        UnqualifiedCodeId = item,
                        CreatedBy = userId,
                        UpdatedBy = userId
                    });
                }
            }

            List<QualUnqualifiedGroupProcedureRelation> qualUnqualifiedGroupProcedureRelationList = new List<QualUnqualifiedGroupProcedureRelation>();
            if (param.UnqualifiedCodeIds != null && param.UnqualifiedCodeIds.Any())
            {
                foreach (var item in param.UnqualifiedCodeIds)
                {
                    qualUnqualifiedGroupProcedureRelationList.Add(new QualUnqualifiedGroupProcedureRelation
                    {
                        SiteCode = _currentSite.Name,
                        UnqualifiedGroupId = qualUnqualifiedGroupEntity.Id,
                        ProcedureId = item,
                        CreatedBy = userId,
                        UpdatedBy = userId
                    });
                }
            }

            using var ts = TransactionHelper.GetTransactionScope();
            await _qualUnqualifiedGroupRepository.UpdateAsync(qualUnqualifiedGroupEntity);

            await _qualUnqualifiedGroupRepository.RealDelteQualUnqualifiedCodeGroupRelationAsync(param.Id);
            //不合格组关联不合格代码
            if (qualUnqualifiedCodeGroupRelationlist != null && qualUnqualifiedCodeGroupRelationlist.Any())
            {
                await _qualUnqualifiedGroupRepository.InsertQualUnqualifiedCodeGroupRelationRangAsync(qualUnqualifiedCodeGroupRelationlist);
            }

            await _qualUnqualifiedGroupRepository.RealDelteQualUnqualifiedGroupProcedureRelationAsync(param.Id);
            //不合格组关联工序
            if (qualUnqualifiedGroupProcedureRelationList != null && qualUnqualifiedGroupProcedureRelationList.Any())
            {
                await _qualUnqualifiedGroupRepository.InsertQualUnqualifiedGroupProcedureRelationRangAsync(qualUnqualifiedGroupProcedureRelationList);
            }
            ts.Complete();
        }

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<QualUnqualifiedGroupDto> QueryQualUnqualifiedGroupByIdAsync(long id)
        {
            var qualUnqualifiedGroupTask = _qualUnqualifiedGroupRepository.GetByIdAsync(id);
            var qalUnqualifiedCodeProcedureRelationTask = _qualUnqualifiedGroupRepository.GetQualUnqualifiedCodeProcedureRelationAsync(id);
            var qualUnqualifiedCodeGroupRelationTask = _qualUnqualifiedGroupRepository.GetQualUnqualifiedCodeGroupRelationAsync(id);
            var qualUnqualifiedGroupEntity = await qualUnqualifiedGroupTask;
            var qualUnqualifiedCodeProcedureRelationList = await qalUnqualifiedCodeProcedureRelationTask;
            var qualUnqualifiedCodeGroupRelationList = await qualUnqualifiedCodeGroupRelationTask;
            QualUnqualifiedGroupDto qualUnqualifiedGroupDto = new QualUnqualifiedGroupDto();

            if (qualUnqualifiedGroupEntity != null)
            {
                qualUnqualifiedGroupDto = qualUnqualifiedGroupEntity.ToModel<QualUnqualifiedGroupDto>();

                if (qualUnqualifiedCodeProcedureRelationList != null && qualUnqualifiedCodeProcedureRelationList.Any())
                {
                    qualUnqualifiedGroupDto.QualUnqualifiedGroupCodeRelationList = new List<QualUnqualifiedGroupCodeRelationDto>();
                    foreach (var item in qualUnqualifiedCodeGroupRelationList)
                    {
                        qualUnqualifiedGroupDto.QualUnqualifiedGroupCodeRelationList.Add(new QualUnqualifiedGroupCodeRelationDto()
                        {
                            Id = item.Id,
                            UnqualifiedGroupId = item.UnqualifiedGroupId,
                            UnqualifiedCode = item.UnqualifiedCode,
                            UnqualifiedCodeName = item.UnqualifiedCodeName,
                            CreatedBy = item.CreatedBy,
                            CreatedOn = item.CreatedOn,
                            UpdatedBy = item.UpdatedBy,
                            UpdatedOn = item.UpdatedOn,
                        });
                    }
                }
                if (qualUnqualifiedCodeProcedureRelationList != null && qualUnqualifiedCodeProcedureRelationList.Any())
                {
                    qualUnqualifiedGroupDto.QualUnqualifiedGroupProcedureRelationList = new List<QualUnqualifiedGroupProcedureRelationDto>();
                    foreach (var item in qualUnqualifiedCodeProcedureRelationList)
                    {
                        qualUnqualifiedGroupDto.QualUnqualifiedGroupProcedureRelationList.Add(new QualUnqualifiedGroupProcedureRelationDto()
                        {
                            Id = item.Id,
                            UnqualifiedGroupId = item.UnqualifiedGroupId,
                            ProcedureCode = item.ProcedureCode,
                            ProcedureName = item.ProcedureName,
                            CreatedBy = item.CreatedBy,
                            CreatedOn = item.CreatedOn,
                            UpdatedBy = item.UpdatedBy,
                            UpdatedOn = item.UpdatedOn,
                        });
                    }
                }
                return qualUnqualifiedGroupDto;
            }
            else
            {
                return null;
            }
        }
    }
}
