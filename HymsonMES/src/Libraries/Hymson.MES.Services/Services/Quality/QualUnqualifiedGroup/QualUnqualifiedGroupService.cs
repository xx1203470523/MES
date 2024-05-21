using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Quality;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Quality;
using Hymson.MES.Services.Dtos.Quality;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Services.Services.Quality.QualUnqualifiedGroup
{
    /// <summary>
    /// 不合格代码组服务
    /// @author wangkeming
    /// @date 2023-02-11 04:45:25
    /// </summary>
    public class QualUnqualifiedGroupService : IQualUnqualifiedGroupService
    {
        private readonly IQualUnqualifiedGroupRepository _qualUnqualifiedGroupRepository;
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
        /// /// <param name="currentUser"></param>
        /// /// <param name="currentSite"></param>
        public QualUnqualifiedGroupService(IQualUnqualifiedGroupRepository qualUnqualifiedGroupRepository, AbstractValidator<QualUnqualifiedGroupCreateDto> validationCreateRules, AbstractValidator<QualUnqualifiedGroupModifyDto> validationModifyRules, ICurrentUser currentUser, ICurrentSite currentSite)
        {
            _qualUnqualifiedGroupRepository = qualUnqualifiedGroupRepository;
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
            qualUnqualifiedGroupPagedQuery.SiteId = _currentSite.SiteId ?? 0;
            var pagedInfo = await _qualUnqualifiedGroupRepository.GetPagedInfoAsync(qualUnqualifiedGroupPagedQuery);

            //实体到DTO转换 装载数据
            List<QualUnqualifiedGroupDto> qualUnqualifiedGroupDtos = PrepareQualUnqualifiedGroupDtos(pagedInfo);
            return new PagedInfo<QualUnqualifiedGroupDto>(qualUnqualifiedGroupDtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 查询工序下的不合格组列表
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        public async Task<IEnumerable<QualUnqualifiedGroupDto>> GetListByProcedureIdAsync([FromQuery] QualUnqualifiedGroupQueryDto queryDto)
        {
            var query = new QualUnqualifiedGroupQuery
            {
                SiteId = _currentSite.SiteId ?? 0,
                ProcedureId = queryDto.ProcedureId
            };
            var list = await _qualUnqualifiedGroupRepository.GetListByProcedureIdAsync(query);

            //实体到DTO转换 装载数据
            var unqualifiedGroupDtos = new List<QualUnqualifiedGroupDto>();
            foreach (var entity in list)
            {
                var groupDto = entity.ToModel<QualUnqualifiedGroupDto>();
                unqualifiedGroupDtos.Add(groupDto);
            }
            return unqualifiedGroupDtos;
        }

        /// <summary>
        /// 查询物料组关联的不合格组列表
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        public async Task<IEnumerable<QualUnqualifiedGroupDto>> GetListByMaterialGroupIddAsync([FromQuery] QualUnqualifiedGroupQueryDto queryDto)
        {
            var query = new QualUnqualifiedGroupQuery
            {
                SiteId = _currentSite.SiteId ?? 0,
                MaterialGroupId = queryDto.MaterialGroupId
            };
            var list = await _qualUnqualifiedGroupRepository.GetListByMaterialGroupIddAsync(query);

            //实体到DTO转换 装载数据
            var unqualifiedGroupDtos = new List<QualUnqualifiedGroupDto>();
            foreach (var entity in list)
            {
                var groupDto = entity.ToModel<QualUnqualifiedGroupDto>();
                unqualifiedGroupDtos.Add(groupDto);
            }
            return unqualifiedGroupDtos;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<long> CreateQualUnqualifiedGroupAsync(QualUnqualifiedGroupCreateDto param)
        {
            param.UnqualifiedGroup = param.UnqualifiedGroup.ToTrimSpace().ToUpperInvariant();
            param.UnqualifiedGroupName = param.UnqualifiedGroupName.Trim();

            await _validationCreateRules.ValidateAndThrowAsync(param);

            var qualUnqualifiedGroupEntity = await _qualUnqualifiedGroupRepository.GetByCodeAsync(new QualUnqualifiedGroupByCodeQuery { Code = param.UnqualifiedGroup, Site = _currentSite.SiteId ?? 0 });
            if (qualUnqualifiedGroupEntity != null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES11206)).WithData("code", param.UnqualifiedGroup);
            }
            var userId = _currentUser.UserName;
            //DTO转换实体
            qualUnqualifiedGroupEntity = param.ToEntity<QualUnqualifiedGroupEntity>();
            qualUnqualifiedGroupEntity.Id = IdGenProvider.Instance.CreateId();
            qualUnqualifiedGroupEntity.CreatedBy = userId;
            qualUnqualifiedGroupEntity.UpdatedBy = userId;
            qualUnqualifiedGroupEntity.SiteId = _currentSite.SiteId ?? 0;

            List<QualUnqualifiedCodeGroupRelation> qualUnqualifiedCodeGroupRelationlist = new List<QualUnqualifiedCodeGroupRelation>();
            if (param.UnqualifiedCodeIds != null && param.UnqualifiedCodeIds.Any())
            {
                foreach (var item in param.UnqualifiedCodeIds)
                {
                    qualUnqualifiedCodeGroupRelationlist.Add(new QualUnqualifiedCodeGroupRelation
                    {
                        Id = IdGenProvider.Instance.CreateId(),
                        SiteId = _currentSite.SiteId ?? 0,
                        UnqualifiedGroupId = qualUnqualifiedGroupEntity.Id,
                        UnqualifiedCodeId = item,
                        CreatedBy = userId,
                        UpdatedBy = userId
                    });
                }
            }

            List<QualUnqualifiedGroupProcedureRelation> qualUnqualifiedGroupProcedureRelationList = new List<QualUnqualifiedGroupProcedureRelation>();
            if (param.ProcedureIds != null && param.ProcedureIds.Any())
            {
                foreach (var item in param.ProcedureIds)
                {
                    qualUnqualifiedGroupProcedureRelationList.Add(new QualUnqualifiedGroupProcedureRelation
                    {
                        Id = IdGenProvider.Instance.CreateId(),
                        SiteId = _currentSite.SiteId ?? 0,
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
            return qualUnqualifiedGroupEntity.Id;
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeletesQualUnqualifiedGroupAsync(long[] ids)
        {
            var userId = _currentUser.UserName;
            return await _qualUnqualifiedGroupRepository.DeleteRangAsync(new DeleteCommand { Ids = ids, DeleteOn = HymsonClock.Now(), UserId = userId });
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
            param.UnqualifiedGroupName = param.UnqualifiedGroupName.Trim();
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
                        Id = IdGenProvider.Instance.CreateId(),
                        SiteId = _currentSite.SiteId ?? 0,

                        UnqualifiedGroupId = qualUnqualifiedGroupEntity.Id,
                        UnqualifiedCodeId = item,
                        CreatedBy = userId,
                        UpdatedBy = userId
                    });
                }
            }

            List<QualUnqualifiedGroupProcedureRelation> qualUnqualifiedGroupProcedureRelationList = new List<QualUnqualifiedGroupProcedureRelation>();
            if (param.ProcedureIds != null && param.ProcedureIds.Any())
            {
                foreach (var item in param.ProcedureIds)
                {
                    qualUnqualifiedGroupProcedureRelationList.Add(new QualUnqualifiedGroupProcedureRelation
                    {
                        Id = IdGenProvider.Instance.CreateId(),
                        SiteId = _currentSite.SiteId ?? 0,

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
            var qualUnqualifiedGroupEntity = await _qualUnqualifiedGroupRepository.GetByIdAsync(id);

            if (qualUnqualifiedGroupEntity != null)
            {
                var qualUnqualifiedGroupDto = qualUnqualifiedGroupEntity.ToModel<QualUnqualifiedGroupDto>();
                return qualUnqualifiedGroupDto;
            }
            else
            {
                return new QualUnqualifiedGroupDto();
            }
        }

        /// <summary>
        /// 获取不合格组中不合格代码
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<List<QualUnqualifiedGroupCodeRelationDto>> GetQualUnqualifiedCodeGroupRelationByIdAsync(long id)
        {
            var qualUnqualifiedCodeGroupRelationList = await _qualUnqualifiedGroupRepository.GetQualUnqualifiedCodeGroupRelationAsync(id);
            var qualUnqualifiedGroupCodeRelationList = new List<QualUnqualifiedGroupCodeRelationDto>();
            if (qualUnqualifiedCodeGroupRelationList != null && qualUnqualifiedCodeGroupRelationList.Any())
            {

                foreach (var item in qualUnqualifiedCodeGroupRelationList)
                {
                    qualUnqualifiedGroupCodeRelationList.Add(new QualUnqualifiedGroupCodeRelationDto()
                    {
                        Id = item.Id,
                        UnqualifiedGroupId = item.UnqualifiedGroupId,
                        UnqualifiedId = item.UnqualifiedCodeId,
                        UnqualifiedCode = item.UnqualifiedCode,
                        UnqualifiedCodeName = item.UnqualifiedCodeName,
                        CreatedBy = item.CreatedBy,
                        CreatedOn = item.CreatedOn,
                        UpdatedBy = item.UpdatedBy,
                        UpdatedOn = item.UpdatedOn,
                    });
                }
            }
            return qualUnqualifiedGroupCodeRelationList;
        }

        /// <summary>
        /// 获取不合格组中工序
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<List<QualUnqualifiedGroupProcedureRelationDto>> GetQualUnqualifiedCodeProcedureRelationByIdAsync(long id)
        {
            var qualUnqualifiedCodeProcedureRelationList = await _qualUnqualifiedGroupRepository.GetQualUnqualifiedCodeProcedureRelationAsync(id);
            var qualUnqualifiedGroupProcedureRelationList = new List<QualUnqualifiedGroupProcedureRelationDto>();
            if (qualUnqualifiedCodeProcedureRelationList != null && qualUnqualifiedCodeProcedureRelationList.Any())
            {

                foreach (var item in qualUnqualifiedCodeProcedureRelationList)
                {
                    qualUnqualifiedGroupProcedureRelationList.Add(new QualUnqualifiedGroupProcedureRelationDto()
                    {
                        Id = item.Id,
                        UnqualifiedGroupId = item.UnqualifiedGroupId,
                        ProcedureId = item.ProcedureId,
                        ProcedureName = item.ProcedureName,
                        ProcedureCode = item.ProcedureCode,
                        CreatedBy = item.CreatedBy,
                        CreatedOn = item.CreatedOn,
                        UpdatedBy = item.UpdatedBy,
                        UpdatedOn = item.UpdatedOn,
                    });
                }
            }
            return qualUnqualifiedGroupProcedureRelationList;
        }
    }
}
