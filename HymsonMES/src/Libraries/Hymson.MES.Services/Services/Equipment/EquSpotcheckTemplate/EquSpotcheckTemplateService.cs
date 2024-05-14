/*
 *creator: Karl
 *
 *describe: 设备点检模板    服务 | 代码由框架生成
 *builder:  pengxin
 *build datetime: 2024-05-13 03:06:41
 */
using Elastic.Clients.Elasticsearch;
using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.EquSpotcheckTemplate;
using Hymson.MES.Core.Domain.EquSpotcheckTemplateEquipmentGroupRelation;
using Hymson.MES.Core.Domain.EquSpotcheckTemplateItemRelation;
using Hymson.MES.Core.Enums;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.EquSpotcheckTemplate;
using Hymson.MES.Data.Repositories.EquSpotcheckTemplateEquipmentGroupRelation;
using Hymson.MES.Data.Repositories.EquSpotcheckTemplateItemRelation;
using Hymson.MES.Services.Dtos.EquSpotcheckTemplate;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;
using System.Transactions;

namespace Hymson.MES.Services.Services.EquSpotcheckTemplate
{
    /// <summary>
    /// 设备点检模板 服务
    /// </summary>
    public class EquSpotcheckTemplateService : IEquSpotcheckTemplateService
    {
        private readonly ICurrentUser _currentUser;
        private readonly ICurrentSite _currentSite;

        /// <summary>
        /// 设备点检模板 仓储
        /// </summary>
        private readonly IEquSpotcheckTemplateRepository _equSpotcheckTemplateRepository;
        private readonly IEquSpotcheckTemplateItemRelationRepository _equSpotcheckTemplateItemRelationRepository;
        private readonly IEquSpotcheckTemplateEquipmentGroupRelationRepository _equSpotcheckTemplateEquipmentGroupRelationRepository;
        private readonly AbstractValidator<EquSpotcheckTemplateCreateDto> _validationCreateRules;
        private readonly AbstractValidator<EquSpotcheckTemplateModifyDto> _validationModifyRules;

        public EquSpotcheckTemplateService(ICurrentUser currentUser, ICurrentSite currentSite, IEquSpotcheckTemplateRepository equSpotcheckTemplateRepository, AbstractValidator<EquSpotcheckTemplateCreateDto> validationCreateRules, AbstractValidator<EquSpotcheckTemplateModifyDto> validationModifyRules)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _equSpotcheckTemplateRepository = equSpotcheckTemplateRepository;
            _validationCreateRules = validationCreateRules;
            _validationModifyRules = validationModifyRules;
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="equSpotcheckTemplateCreateDto"></param>
        /// <returns></returns>
        public async Task CreateEquSpotcheckTemplateAsync(EquSpotcheckTemplateCreateDto equSpotcheckTemplateCreateDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0)
            {
                throw new ValidationException(nameof(ErrorCode.MES10101));
            }

            //验证DTO
            await _validationCreateRules.ValidateAndThrowAsync(equSpotcheckTemplateCreateDto);

            //DTO转换实体
            var equSpotcheckTemplateEntity = equSpotcheckTemplateCreateDto.ToEntity<EquSpotcheckTemplateEntity>();
            equSpotcheckTemplateEntity.Id = IdGenProvider.Instance.CreateId();
            equSpotcheckTemplateEntity.CreatedBy = _currentUser.UserName;
            equSpotcheckTemplateEntity.UpdatedBy = _currentUser.UserName;
            equSpotcheckTemplateEntity.CreatedOn = HymsonClock.Now();
            equSpotcheckTemplateEntity.UpdatedOn = HymsonClock.Now();
            equSpotcheckTemplateEntity.SiteId = _currentSite.SiteId ?? 0;

            List<EquSpotcheckTemplateEquipmentGroupRelationEntity> groupRelationList = new();
            foreach (var item in equSpotcheckTemplateCreateDto.groupRelationDto)
            {
                var groupRelation = new EquSpotcheckTemplateEquipmentGroupRelationEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    EquipmentGroupId = item.EquipmentGroupId,
                    SpotCheckTemplateId = equSpotcheckTemplateEntity.Id,

                    IsDeleted = 0,
                    CreatedBy = _currentUser.UserName,
                    UpdatedBy = _currentUser.UserName,
                    CreatedOn = HymsonClock.Now(),
                    UpdatedOn = HymsonClock.Now(),
                };

                groupRelationList.Add(groupRelation);
            }

            List<EquSpotcheckTemplateItemRelationEntity> relationList = new();
            foreach (var item in equSpotcheckTemplateCreateDto.relationDto)
            {

                var relation = new EquSpotcheckTemplateItemRelationEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SpotCheckTemplateId = equSpotcheckTemplateEntity.Id,
                    Center = item.Center,
                    LowerLimit = item.LowerLimit,
                    SpotCheckItemId = item.SpotCheckItemId,
                    UpperLimit = item.UpperLimit,

                    IsDeleted = 0,
                    CreatedBy = _currentUser.UserName,
                    UpdatedBy = _currentUser.UserName,
                    CreatedOn = HymsonClock.Now(),
                    UpdatedOn = HymsonClock.Now(),
                };

                relationList.Add(relation);
            }

            using var trans = TransactionHelper.GetTransactionScope();

            await _equSpotcheckTemplateItemRelationRepository.InsertsAsync(relationList);

            await _equSpotcheckTemplateEquipmentGroupRelationRepository.InsertsAsync(groupRelationList);

            await _equSpotcheckTemplateRepository.InsertAsync(equSpotcheckTemplateEntity);

            trans.Complete();
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteEquSpotcheckTemplateAsync(long id)
        {
            await _equSpotcheckTemplateRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeletesEquSpotcheckTemplateAsync(long[] ids)
        {
            var templateList = await _equSpotcheckTemplateRepository.GetByIdsAsync(ids);

            var codeEnabiles = "";
            foreach (var item in templateList)
            {
                if (item.Status == SysDataStatusEnum.Enable)
                {
                    codeEnabiles += item.Code + ",";
                }
            }

            if (!string.IsNullOrWhiteSpace(codeEnabiles))
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES12201)).WithData("Code", codeEnabiles);
            }

            int row = 0;
            var trans = TransactionHelper.GetTransactionScope();
            await _equSpotcheckTemplateItemRelationRepository.DeletesByIdsAsync(ids);
            await _equSpotcheckTemplateEquipmentGroupRelationRepository.DeletesByIdsAsync(ids);

            row += await _equSpotcheckTemplateRepository.DeletesAsync(new DeleteCommand { Ids = ids, DeleteOn = HymsonClock.Now(), UserId = _currentUser.UserName });
            trans.Complete();
            return row;
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="equSpotcheckTemplatePagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<EquSpotcheckTemplateDto>> GetPagedListAsync(EquSpotcheckTemplatePagedQueryDto equSpotcheckTemplatePagedQueryDto)
        {
            var equSpotcheckTemplatePagedQuery = equSpotcheckTemplatePagedQueryDto.ToQuery<EquSpotcheckTemplatePagedQuery>();
            equSpotcheckTemplatePagedQuery.SiteId = _currentSite.SiteId;
            var pagedInfo = await _equSpotcheckTemplateRepository.GetPagedInfoAsync(equSpotcheckTemplatePagedQuery);

            //实体到DTO转换 装载数据
            List<EquSpotcheckTemplateDto> equSpotcheckTemplateDtos = PrepareEquSpotcheckTemplateDtos(pagedInfo);
            return new PagedInfo<EquSpotcheckTemplateDto>(equSpotcheckTemplateDtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pagedInfo"></param>
        /// <returns></returns>
        private static List<EquSpotcheckTemplateDto> PrepareEquSpotcheckTemplateDtos(PagedInfo<EquSpotcheckTemplateEntity> pagedInfo)
        {
            var equSpotcheckTemplateDtos = new List<EquSpotcheckTemplateDto>();
            foreach (var equSpotcheckTemplateEntity in pagedInfo.Data)
            {
                var equSpotcheckTemplateDto = equSpotcheckTemplateEntity.ToModel<EquSpotcheckTemplateDto>();
                equSpotcheckTemplateDtos.Add(equSpotcheckTemplateDto);
            }

            return equSpotcheckTemplateDtos;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="equSpotcheckTemplateModifyDto"></param>
        /// <returns></returns>
        public async Task ModifyEquSpotcheckTemplateAsync(EquSpotcheckTemplateModifyDto equSpotcheckTemplateModifyDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0)
            {
                throw new ValidationException(nameof(ErrorCode.MES10101));
            }

            //验证DTO
            await _validationModifyRules.ValidateAndThrowAsync(equSpotcheckTemplateModifyDto);

            //DTO转换实体
            var equSpotcheckTemplateEntity = equSpotcheckTemplateModifyDto.ToEntity<EquSpotcheckTemplateEntity>();
            equSpotcheckTemplateEntity.UpdatedBy = _currentUser.UserName;
            equSpotcheckTemplateEntity.UpdatedOn = HymsonClock.Now();

            List<EquSpotcheckTemplateItemRelationEntity> addRelation = new();
            List<EquSpotcheckTemplateItemRelationEntity> editRelation = new();
            long[] deleteRelation = Array.Empty<long>();
            if (equSpotcheckTemplateModifyDto.relationDto != null && equSpotcheckTemplateModifyDto.relationDto.Any())
            {
                var relationIds = equSpotcheckTemplateModifyDto.relationDto.Select(it => it.Id).ToArray();
                var relationList = await _equSpotcheckTemplateItemRelationRepository.GetByIdsAsync(relationIds);

                if (relationList != null && relationList.Any())
                {
                    deleteRelation = relationList.Select(it => it.Id).Where(it => !it.Equals(relationIds)).ToArray();
                }

                foreach (var item in equSpotcheckTemplateModifyDto.relationDto)
                {
                    var relation = relationList?.Where(it => it.Id == item.Id).FirstOrDefault();
                    if (relation != null)
                    {
                        relation.UpperLimit = item.UpperLimit;
                        relation.Center = item.Center;
                        relation.LowerLimit = item.LowerLimit;
                        relation.UpdatedBy = _currentUser.UserName;
                        relation.UpdatedOn = HymsonClock.Now();
                        editRelation.Add(relation);
                    }
                    else
                    {
                        addRelation.Add(new EquSpotcheckTemplateItemRelationEntity
                        {
                            Id = IdGenProvider.Instance.CreateId(),
                            SpotCheckTemplateId = equSpotcheckTemplateEntity.Id,
                            Center = item.Center,
                            LowerLimit = item.LowerLimit,
                            SpotCheckItemId = item.SpotCheckItemId,
                            UpperLimit = item.UpperLimit,

                            IsDeleted = 0,
                            CreatedBy = _currentUser.UserName,
                            UpdatedBy = _currentUser.UserName,
                            CreatedOn = HymsonClock.Now(),
                            UpdatedOn = HymsonClock.Now()
                        });
                    }
                }
            }



            List<EquSpotcheckTemplateEquipmentGroupRelationEntity> addGroupRelation = new();
            List<EquSpotcheckTemplateEquipmentGroupRelationEntity> editGroupRelation = new();
            long[] deleteGroupRelation = Array.Empty<long>();
            if (equSpotcheckTemplateModifyDto.groupRelationDto != null && equSpotcheckTemplateModifyDto.groupRelationDto.Any())
            {
                var groupRelationIds = equSpotcheckTemplateModifyDto.groupRelationDto.Select(it => it.Id).ToArray();
                var groupRelationList = await _equSpotcheckTemplateEquipmentGroupRelationRepository.GetByIdsAsync(groupRelationIds);

                if (groupRelationList != null && groupRelationList.Any())
                {
                    deleteGroupRelation = groupRelationList.Select(it => it.Id).Where(it => !it.Equals(groupRelationIds)).ToArray();
                }

                foreach (var item in equSpotcheckTemplateModifyDto.groupRelationDto)
                {
                    var groupRelation = groupRelationList?.Where(it => it.Id == item.Id).FirstOrDefault();
                    if (groupRelation != null)
                    {
                        groupRelation.EquipmentGroupId = item.EquipmentGroupId;
                        groupRelation.UpdatedBy = _currentUser.UserName;
                        groupRelation.UpdatedOn = HymsonClock.Now();
                        editGroupRelation.Add(groupRelation);
                    }
                    else
                    {
                        addGroupRelation.Add(new EquSpotcheckTemplateEquipmentGroupRelationEntity
                        {
                            Id = IdGenProvider.Instance.CreateId(),
                            EquipmentGroupId = item.EquipmentGroupId,
                            SpotCheckTemplateId = equSpotcheckTemplateEntity.Id,

                            IsDeleted = 0,
                            CreatedBy = _currentUser.UserName,
                            UpdatedBy = _currentUser.UserName,
                            CreatedOn = HymsonClock.Now(),
                            UpdatedOn = HymsonClock.Now(),
                        });
                    }
                }
            }


            var trans = TransactionHelper.GetTransactionScope();
            await _equSpotcheckTemplateRepository.UpdateAsync(equSpotcheckTemplateEntity);

            await _equSpotcheckTemplateItemRelationRepository.UpdatesAsync(editRelation);
            await _equSpotcheckTemplateItemRelationRepository.InsertsAsync(addRelation);
            await _equSpotcheckTemplateItemRelationRepository.DeletesByIdsAsync(deleteRelation);

            await _equSpotcheckTemplateEquipmentGroupRelationRepository.UpdatesAsync(editGroupRelation);
            await _equSpotcheckTemplateEquipmentGroupRelationRepository.InsertsAsync(addGroupRelation);
            await _equSpotcheckTemplateEquipmentGroupRelationRepository.DeletesByIdsAsync(deleteGroupRelation);

            trans.Complete();
        }

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<EquSpotcheckTemplateDto> QueryEquSpotcheckTemplateByIdAsync(long id)
        {
            var equSpotcheckTemplateEntity = await _equSpotcheckTemplateRepository.GetByIdAsync(id);
            if (equSpotcheckTemplateEntity != null)
            {
                return equSpotcheckTemplateEntity.ToModel<EquSpotcheckTemplateDto>();
            }
            return null;
        }
    }
}
