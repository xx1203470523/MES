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
using Hymson.MES.Core.Domain.Equipment.EquSpotcheck;
using Hymson.MES.Core.Enums;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.MES.Data.Repositories.Equipment;
using Hymson.MES.Data.Repositories.Equipment.EquEquipment;
using Hymson.MES.Data.Repositories.Equipment.EquEquipmentGroup;
using Hymson.MES.Data.Repositories.EquSpotcheckTemplate;
using Hymson.MES.Data.Repositories.EquSpotcheckTemplateEquipmentGroupRelation;
using Hymson.MES.Data.Repositories.EquSpotcheckTemplateItemRelation;
using Hymson.MES.Services.Dtos.EquSpotcheckTemplate;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;
using System.Collections.Generic;
using System.Security.Policy;
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
        private readonly IEquSpotcheckItemRepository _equSpotcheckItemRepository;
        private readonly IEquEquipmentGroupRepository _equEquipmentGroupRepository;
        private readonly IEquEquipmentRepository _equEquipmentRepository;
        private readonly AbstractValidator<EquSpotcheckTemplateCreateDto> _validationCreateRules;
        private readonly AbstractValidator<EquSpotcheckTemplateModifyDto> _validationModifyRules;

        public EquSpotcheckTemplateService(ICurrentUser currentUser, ICurrentSite currentSite, IEquSpotcheckTemplateRepository equSpotcheckTemplateRepository, AbstractValidator<EquSpotcheckTemplateCreateDto> validationCreateRules, AbstractValidator<EquSpotcheckTemplateModifyDto> validationModifyRules, IEquSpotcheckItemRepository equSpotcheckItemRepository, IEquEquipmentGroupRepository equEquipmentGroupRepository, IEquSpotcheckTemplateItemRelationRepository equSpotcheckTemplateItemRelationRepository, IEquSpotcheckTemplateEquipmentGroupRelationRepository equSpotcheckTemplateEquipmentGroupRelationRepository, IEquEquipmentRepository equEquipmentRepository)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _equSpotcheckTemplateRepository = equSpotcheckTemplateRepository;
            _validationCreateRules = validationCreateRules;
            _validationModifyRules = validationModifyRules;
            _equSpotcheckItemRepository = equSpotcheckItemRepository;
            _equEquipmentGroupRepository = equEquipmentGroupRepository;
            _equSpotcheckTemplateItemRelationRepository = equSpotcheckTemplateItemRelationRepository;
            _equSpotcheckTemplateEquipmentGroupRelationRepository = equSpotcheckTemplateEquipmentGroupRelationRepository;
            _equEquipmentRepository = equEquipmentRepository;
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


            var equSpotcheckTemplate = await _equSpotcheckTemplateRepository.GetByCodeAsync(new EquSpotcheckTemplateQuery
            {
                Code = equSpotcheckTemplateCreateDto.Code,
                Version = equSpotcheckTemplateCreateDto.Version,
                SiteId = _currentSite.SiteId,
            });

            if (equSpotcheckTemplate != null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES12202)).WithData("Code", equSpotcheckTemplateCreateDto.Code).WithData("Version", equSpotcheckTemplateCreateDto.Version);
            }

            //DTO转换实体
            var equSpotcheckTemplateEntity = equSpotcheckTemplateCreateDto.ToEntity<EquSpotcheckTemplateEntity>();
            equSpotcheckTemplateEntity.Id = IdGenProvider.Instance.CreateId();
            equSpotcheckTemplateEntity.CreatedBy = _currentUser.UserName;
            equSpotcheckTemplateEntity.UpdatedBy = _currentUser.UserName;
            equSpotcheckTemplateEntity.CreatedOn = HymsonClock.Now();
            equSpotcheckTemplateEntity.UpdatedOn = HymsonClock.Now();
            equSpotcheckTemplateEntity.SiteId = _currentSite.SiteId ?? 0;


            List<EquSpotcheckTemplateEquipmentGroupRelationEntity> groupRelationList = new();
            var eGroupIds = equSpotcheckTemplateCreateDto.groupRelationDto.Select(it => it.Id).ToArray();
            var equSpotcheckTemplateEquipmentGroupRelations = await _equSpotcheckTemplateEquipmentGroupRelationRepository.GetByGroupIdAsync(eGroupIds);
            if (equSpotcheckTemplateEquipmentGroupRelations != null && equSpotcheckTemplateEquipmentGroupRelations.Any())
            {
                var groupRelationIds = equSpotcheckTemplateEquipmentGroupRelations.Select(it => it.EquipmentGroupId).ToArray();
                var equipmentGroups = await _equEquipmentGroupRepository.GetByIdsAsync(groupRelationIds);
                var equipmentGroupCodes = string.Join(",", equipmentGroups.Select(it => it.EquipmentGroupCode));
                throw new CustomerValidationException(nameof(ErrorCode.MES12203)).WithData("Code", equipmentGroupCodes);
            }
            foreach (var item in equSpotcheckTemplateCreateDto.groupRelationDto)
            {
                var groupRelation = new EquSpotcheckTemplateEquipmentGroupRelationEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    EquipmentGroupId = item.Id,
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
                    SpotCheckItemId = item.Id,
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
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<int> DeletesEquSpotcheckTemplateAsync(EquSpotcheckTemplateDeleteDto param)
        {
            var ids = param.Ids;
            var templateList = await _equSpotcheckTemplateRepository.GetByIdsAsync(ids.ToArray());

            var codeEnabiles = "";
            foreach (var item in templateList)
            {
                if (item.Status == DisableOrEnableEnum.Enable)
                {
                    codeEnabiles += item.Code + ",";
                }
            }

            if (!string.IsNullOrWhiteSpace(codeEnabiles))
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES12201)).WithData("Code", codeEnabiles);
            }

            int row = 0;
            using var trans = TransactionHelper.GetTransactionScope();
            await _equSpotcheckTemplateItemRelationRepository.DeleteBySpotCheckTemplateIdsAsync(ids);
            await _equSpotcheckTemplateEquipmentGroupRelationRepository.DeletesBySpotCheckTemplateIdsAsync(ids);

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

            if (equSpotcheckTemplatePagedQueryDto.EquipmentId.HasValue)
            {
                var equEquipment = await _equEquipmentRepository.GetByIdAsync(equSpotcheckTemplatePagedQueryDto.EquipmentId ?? 0);
                if (equEquipment == null)
                {
                    return null;
                }
                List<long> equGroupEquipmentIds = new()
                    {
                        equEquipment.EquipmentGroupId
                    };
                var equSpotcheckTemplateEquipmentGroups = await _equSpotcheckTemplateEquipmentGroupRelationRepository.GetByGroupIdAsync(equGroupEquipmentIds);
                if (equSpotcheckTemplateEquipmentGroups == null || !equSpotcheckTemplateEquipmentGroups.Any())
                {
                    return null;
                }
                equSpotcheckTemplatePagedQuery.SpotCheckTemplateIds = equSpotcheckTemplateEquipmentGroups.Select(it => it.SpotCheckTemplateId).ToList();

            }
            if (!string.IsNullOrWhiteSpace(equSpotcheckTemplatePagedQueryDto.EquipmentGroupCode))
            {
                var equGroupEquipment = await _equEquipmentGroupRepository.GetByCodeAsync(new EntityByCodeQuery { Code = equSpotcheckTemplatePagedQueryDto.EquipmentGroupCode, Site = _currentSite.SiteId });
                if (equGroupEquipment != null)
                {
                    List<long> equGroupEquipmentIds = new()
                    {
                        equGroupEquipment.Id
                    };
                    var equSpotcheckTemplateEquipmentGroups = await _equSpotcheckTemplateEquipmentGroupRelationRepository.GetByGroupIdAsync(equGroupEquipmentIds);
                    if (equSpotcheckTemplateEquipmentGroups != null && equSpotcheckTemplateEquipmentGroups.Any())
                    {
                        equSpotcheckTemplatePagedQuery.SpotCheckTemplateIds = equSpotcheckTemplateEquipmentGroups.Select(it => it.SpotCheckTemplateId).ToList();
                    }
                }

            }
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
            equSpotcheckTemplateEntity.SiteId = _currentSite.SiteId ?? 0;
            equSpotcheckTemplateEntity.CreatedBy = _currentUser.UserName;
            equSpotcheckTemplateEntity.CreatedOn = HymsonClock.Now();

            equSpotcheckTemplateEntity.UpdatedBy = _currentUser.UserName;
            equSpotcheckTemplateEntity.UpdatedOn = HymsonClock.Now();

            List<long> SpotCheckItemIds = new()
            {
                equSpotcheckTemplateEntity.Id
            };

            List<EquSpotcheckTemplateItemRelationEntity> addRelation = new();

            if (equSpotcheckTemplateModifyDto.relationDto != null && equSpotcheckTemplateModifyDto.relationDto.Any())
            {
                foreach (var item in equSpotcheckTemplateModifyDto.relationDto)
                {
                    addRelation.Add(new EquSpotcheckTemplateItemRelationEntity
                    {
                        Id = IdGenProvider.Instance.CreateId(),
                        SpotCheckTemplateId = equSpotcheckTemplateEntity.Id,
                        Center = item.Center,
                        LowerLimit = item.LowerLimit,
                        SpotCheckItemId = item.SpotCheckItemId == 0 ? item.Id : item.SpotCheckItemId,
                        UpperLimit = item.UpperLimit,

                        IsDeleted = 0,
                        CreatedBy = _currentUser.UserName,
                        UpdatedBy = _currentUser.UserName,
                        CreatedOn = HymsonClock.Now(),
                        UpdatedOn = HymsonClock.Now()
                    });
                }
            }


            List<EquSpotcheckTemplateEquipmentGroupRelationEntity> addGroupRelation = new();

            var eGroupIds = equSpotcheckTemplateModifyDto.groupRelationDto.Select(it => it.Id).ToArray();
            var equSpotcheckTemplateEquipmentGroupRelations = await _equSpotcheckTemplateEquipmentGroupRelationRepository.GetByGroupIdAsync(eGroupIds);
            if (equSpotcheckTemplateEquipmentGroupRelations != null && equSpotcheckTemplateEquipmentGroupRelations.Any())
            {
                var groupRelationIds = equSpotcheckTemplateEquipmentGroupRelations.Select(it => it.EquipmentGroupId).ToArray();
                var equipmentGroups = await _equEquipmentGroupRepository.GetByIdsAsync(groupRelationIds);
                var equipmentGroupCodes = string.Join(",", equipmentGroups.Select(it => it.EquipmentGroupCode));
                throw new CustomerValidationException(nameof(ErrorCode.MES12203)).WithData("Code", equipmentGroupCodes);
            }
            foreach (var item in equSpotcheckTemplateModifyDto.groupRelationDto)
            {
                addGroupRelation.Add(new EquSpotcheckTemplateEquipmentGroupRelationEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    EquipmentGroupId = item.EquipmentGroupId == 0 ? item.Id : item.EquipmentGroupId,
                    SpotCheckTemplateId = equSpotcheckTemplateEntity.Id,

                    IsDeleted = 0,
                    CreatedBy = _currentUser.UserName,
                    UpdatedBy = _currentUser.UserName,
                    CreatedOn = HymsonClock.Now(),
                    UpdatedOn = HymsonClock.Now(),
                });
            }

            using var trans = TransactionHelper.GetTransactionScope();
            await _equSpotcheckTemplateRepository.UpdateAsync(equSpotcheckTemplateEntity);

            await _equSpotcheckTemplateItemRelationRepository.DeleteBySpotCheckTemplateIdsAsync(SpotCheckItemIds);
            await _equSpotcheckTemplateItemRelationRepository.InsertsAsync(addRelation);

            await _equSpotcheckTemplateEquipmentGroupRelationRepository.DeletesBySpotCheckTemplateIdsAsync(SpotCheckItemIds);
            await _equSpotcheckTemplateEquipmentGroupRelationRepository.InsertsAsync(addGroupRelation);

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



        #region 关联信息
        /// <summary>
        /// 获取模板关联信息（项目）
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<List<GetSpotcheckItemRelationListDto>> QueryItemRelationListAsync(GetEquSpotcheckTemplateItemRelationDto param)
        {
            var equSpotcheckTemplateItemRelations = await _equSpotcheckTemplateItemRelationRepository.GetEquSpotcheckTemplateItemRelationEntitiesAsync(new EquSpotcheckTemplateItemRelationQuery
            {
                SpotCheckTemplateIds = param.SpotCheckTemplateIds,
                SiteId = _currentSite.SiteId
            });

            List<GetSpotcheckItemRelationListDto> list = new();
            if (equSpotcheckTemplateItemRelations != null && equSpotcheckTemplateItemRelations.Any())
            {
                var spotCheckItemIds = equSpotcheckTemplateItemRelations.Select(it => it.SpotCheckItemId).ToArray();
                var equSpotcheckItems = await _equSpotcheckItemRepository.GetByIdsAsync(spotCheckItemIds);

                foreach (var item in equSpotcheckTemplateItemRelations)
                {
                    var equSpotcheckItem = equSpotcheckItems.FirstOrDefault(it => it.Id == item.SpotCheckItemId);
                    GetSpotcheckItemRelationListDto itemRelation = new()
                    {
                        Id = item.Id,
                        SpotCheckItemId = item.SpotCheckItemId,
                        SpotCheckTemplateId = item.SpotCheckTemplateId,
                        Center = item.Center,
                        LowerLimit = item.LowerLimit,
                        UpperLimit = item.UpperLimit,
                        CheckMethod = equSpotcheckItem?.CheckMethod,
                        CheckType = equSpotcheckItem?.CheckType,
                        Code = equSpotcheckItem?.Code,
                        Name = equSpotcheckItem?.Name,
                        Components = equSpotcheckItem?.Components,
                        DataType = equSpotcheckItem?.DataType
                    };

                    list.Add(itemRelation);
                }

            }

            return list;
        }

        /// <summary>
        /// 获取模板关联信息（设备组）
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<List<QuerySpotcheckEquipmentGroupRelationListDto>> QueryEquipmentGroupRelationListAsync(GetEquSpotcheckTemplateItemRelationDto param)
        {
            var equSpotcheckTemplateEquipmentGroupRelations = await _equSpotcheckTemplateEquipmentGroupRelationRepository.GetEquSpotcheckTemplateEquipmentGroupRelationEntitiesAsync(new EquSpotcheckTemplateEquipmentGroupRelationQuery
            {
                SpotCheckTemplateIds = param.SpotCheckTemplateIds,
                SiteId = _currentSite.SiteId
            });

            List<QuerySpotcheckEquipmentGroupRelationListDto> list = new();
            if (equSpotcheckTemplateEquipmentGroupRelations != null && equSpotcheckTemplateEquipmentGroupRelations.Any())
            {
                var equipmentGroupIds = equSpotcheckTemplateEquipmentGroupRelations.Select(it => it.EquipmentGroupId).ToArray();
                var equSpotcheckItems = await _equEquipmentGroupRepository.GetByIdsAsync(equipmentGroupIds);

                foreach (var item in equSpotcheckTemplateEquipmentGroupRelations)
                {
                    var equSpotcheckItem = equSpotcheckItems.FirstOrDefault(it => it.Id == item.EquipmentGroupId);
                    QuerySpotcheckEquipmentGroupRelationListDto groupRelation = new()
                    {
                        Id = item.Id,
                        EquipmentGroupId = item.EquipmentGroupId,
                        SpotCheckTemplateId = item.SpotCheckTemplateId,
                        EquipmentGroupCode = equSpotcheckItem?.EquipmentGroupCode,
                        EquipmentGroupName = equSpotcheckItem?.EquipmentGroupName,
                        Remark = equSpotcheckItem?.Remark,
                    };

                    list.Add(groupRelation);
                }
            }

            return list;
        }
        #endregion
    }
}
