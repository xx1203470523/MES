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
using Hymson.MES.Core.Domain.Equipment.EquMaintenance;
using Hymson.MES.Core.Enums;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.MES.Data.Repositories.Equipment;
using Hymson.MES.Data.Repositories.Equipment.EquEquipment;
using Hymson.MES.Data.Repositories.Equipment.EquEquipmentGroup;
using Hymson.MES.Data.Repositories.Equipment.EquMaintenance.EquMaintenanceItem;
using Hymson.MES.Data.Repositories.EquMaintenanceTemplate;
using Hymson.MES.Data.Repositories.EquMaintenanceTemplateEquipmentGroupRelation;
using Hymson.MES.Data.Repositories.EquMaintenanceTemplateItemRelation;
using Hymson.MES.Services.Dtos.EquMaintenanceTemplate;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;
using System.Collections.Generic;
using System.Security.Policy;
using System.Transactions;

namespace Hymson.MES.Services.Services.EquMaintenanceTemplate
{
    /// <summary>
    /// 设备点检模板 服务
    /// </summary>
    public class EquMaintenanceTemplateService : IEquMaintenanceTemplateService
    {
        private readonly ICurrentUser _currentUser;
        private readonly ICurrentSite _currentSite;

        /// <summary>
        /// 设备点检模板 仓储
        /// </summary>
        private readonly IEquMaintenanceTemplateRepository _EquMaintenanceTemplateRepository;
        private readonly IEquMaintenanceTemplateItemRelationRepository _EquMaintenanceTemplateItemRelationRepository;
        private readonly IEquMaintenanceTemplateEquipmentGroupRelationRepository _EquMaintenanceTemplateEquipmentGroupRelationRepository;
        private readonly IEquMaintenanceItemRepository _EquMaintenanceItemRepository;
        private readonly IEquEquipmentGroupRepository _equEquipmentGroupRepository;
        private readonly IEquEquipmentRepository _equEquipmentRepository;
        private readonly AbstractValidator<EquMaintenanceTemplateCreateDto> _validationCreateRules;
        private readonly AbstractValidator<EquMaintenanceTemplateModifyDto> _validationModifyRules;

        public EquMaintenanceTemplateService(ICurrentUser currentUser, ICurrentSite currentSite, IEquMaintenanceTemplateRepository EquMaintenanceTemplateRepository, AbstractValidator<EquMaintenanceTemplateCreateDto> validationCreateRules, AbstractValidator<EquMaintenanceTemplateModifyDto> validationModifyRules, IEquMaintenanceItemRepository EquMaintenanceItemRepository, IEquEquipmentGroupRepository equEquipmentGroupRepository, IEquMaintenanceTemplateItemRelationRepository EquMaintenanceTemplateItemRelationRepository, IEquMaintenanceTemplateEquipmentGroupRelationRepository EquMaintenanceTemplateEquipmentGroupRelationRepository, IEquEquipmentRepository equEquipmentRepository)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _EquMaintenanceTemplateRepository = EquMaintenanceTemplateRepository;
            _validationCreateRules = validationCreateRules;
            _validationModifyRules = validationModifyRules;
            _EquMaintenanceItemRepository = EquMaintenanceItemRepository;
            _equEquipmentGroupRepository = equEquipmentGroupRepository;
            _EquMaintenanceTemplateItemRelationRepository = EquMaintenanceTemplateItemRelationRepository;
            _EquMaintenanceTemplateEquipmentGroupRelationRepository = EquMaintenanceTemplateEquipmentGroupRelationRepository;
            _equEquipmentRepository = equEquipmentRepository;
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="EquMaintenanceTemplateCreateDto"></param>
        /// <returns></returns>
        public async Task CreateEquMaintenanceTemplateAsync(EquMaintenanceTemplateCreateDto EquMaintenanceTemplateCreateDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0)
            {
                throw new ValidationException(nameof(ErrorCode.MES10101));
            }

            //验证DTO
            await _validationCreateRules.ValidateAndThrowAsync(EquMaintenanceTemplateCreateDto);


            var EquMaintenanceTemplate = await _EquMaintenanceTemplateRepository.GetByCodeAsync(new EquMaintenanceTemplateQuery
            {
                Code = EquMaintenanceTemplateCreateDto.Code,
                Version = EquMaintenanceTemplateCreateDto.Version,
                SiteId = _currentSite.SiteId,
            });

            if (EquMaintenanceTemplate != null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES12202)).WithData("Code", EquMaintenanceTemplateCreateDto.Code).WithData("Version", EquMaintenanceTemplateCreateDto.Version);
            }

            //DTO转换实体
            var EquMaintenanceTemplateEntity = EquMaintenanceTemplateCreateDto.ToEntity<EquMaintenanceTemplateEntity>();
            EquMaintenanceTemplateEntity.Id = IdGenProvider.Instance.CreateId();
            EquMaintenanceTemplateEntity.CreatedBy = _currentUser.UserName;
            EquMaintenanceTemplateEntity.UpdatedBy = _currentUser.UserName;
            EquMaintenanceTemplateEntity.CreatedOn = HymsonClock.Now();
            EquMaintenanceTemplateEntity.UpdatedOn = HymsonClock.Now();
            EquMaintenanceTemplateEntity.SiteId = _currentSite.SiteId ?? 0;


            List<EquMaintenanceTemplateEquipmentGroupRelationEntity> groupRelationList = new();
            var eGroupIds = EquMaintenanceTemplateCreateDto.groupRelationDto.Select(it => it.Id).ToArray();
            var EquMaintenanceTemplateEquipmentGroupRelations = await _EquMaintenanceTemplateEquipmentGroupRelationRepository.GetByGroupIdAsync(eGroupIds);
            if (EquMaintenanceTemplateEquipmentGroupRelations != null && EquMaintenanceTemplateEquipmentGroupRelations.Any())
            {
                var groupRelationIds = EquMaintenanceTemplateEquipmentGroupRelations.Select(it => it.EquipmentGroupId).ToArray();
                var equipmentGroups = await _equEquipmentGroupRepository.GetByIdsAsync(groupRelationIds);
                var equipmentGroupCodes = string.Join(",", equipmentGroups.Select(it => it.EquipmentGroupCode));
                throw new CustomerValidationException(nameof(ErrorCode.MES12203)).WithData("Code", equipmentGroupCodes);
            }
            foreach (var item in EquMaintenanceTemplateCreateDto.groupRelationDto)
            {
                var groupRelation = new EquMaintenanceTemplateEquipmentGroupRelationEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    EquipmentGroupId = item.Id,
                    MaintenanceTemplateId = EquMaintenanceTemplateEntity.Id,

                    IsDeleted = 0,
                    CreatedBy = _currentUser.UserName,
                    UpdatedBy = _currentUser.UserName,
                    CreatedOn = HymsonClock.Now(),
                    UpdatedOn = HymsonClock.Now(),
                };

                groupRelationList.Add(groupRelation);
            }

            List<EquMaintenanceTemplateItemRelationEntity> relationList = new();
            foreach (var item in EquMaintenanceTemplateCreateDto.relationDto)
            {

                var relation = new EquMaintenanceTemplateItemRelationEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    MaintenanceTemplateId = EquMaintenanceTemplateEntity.Id,
                    Center = item.Center,
                    LowerLimit = item.LowerLimit,
                    MaintenanceItemId = item.Id,
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

            await _EquMaintenanceTemplateItemRelationRepository.InsertsAsync(relationList);

            await _EquMaintenanceTemplateEquipmentGroupRelationRepository.InsertsAsync(groupRelationList);

            await _EquMaintenanceTemplateRepository.InsertAsync(EquMaintenanceTemplateEntity);

            trans.Complete();

        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteEquMaintenanceTemplateAsync(long id)
        {
            await _EquMaintenanceTemplateRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<int> DeletesEquMaintenanceTemplateAsync(EquMaintenanceTemplateDeleteDto param)
        {
            var ids = param.Ids;
            var templateList = await _EquMaintenanceTemplateRepository.GetByIdsAsync(ids.ToArray());

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
            await _EquMaintenanceTemplateItemRelationRepository.DeleteByMaintenanceTemplateIdsAsync(ids);
            await _EquMaintenanceTemplateEquipmentGroupRelationRepository.DeletesByMaintenanceTemplateIdsAsync(ids);

            row += await _EquMaintenanceTemplateRepository.DeletesAsync(new DeleteCommand { Ids = ids, DeleteOn = HymsonClock.Now(), UserId = _currentUser.UserName });
            trans.Complete();
            return row;
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="EquMaintenanceTemplatePagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<EquMaintenanceTemplateDto>> GetPagedListAsync(EquMaintenanceTemplatePagedQueryDto EquMaintenanceTemplatePagedQueryDto)
        {
            var EquMaintenanceTemplatePagedQuery = EquMaintenanceTemplatePagedQueryDto.ToQuery<EquMaintenanceTemplatePagedQuery>();
            EquMaintenanceTemplatePagedQuery.SiteId = _currentSite.SiteId;

            if (EquMaintenanceTemplatePagedQueryDto.EquipmentId.HasValue)
            {
                var equEquipment = await _equEquipmentRepository.GetByIdAsync(EquMaintenanceTemplatePagedQueryDto.EquipmentId ?? 0);
                if (equEquipment == null)
                {
                    return null;
                }

                List<long> equGroupEquipmentIds = new()
                    {
                        equEquipment.EquipmentGroupId
                    };
                var EquMaintenanceTemplateEquipmentGroups = await _EquMaintenanceTemplateEquipmentGroupRelationRepository.GetByGroupIdAsync(equGroupEquipmentIds);
                if (EquMaintenanceTemplateEquipmentGroups == null || !EquMaintenanceTemplateEquipmentGroups.Any())
                {
                    return null;
                }
                EquMaintenanceTemplatePagedQuery.MaintenanceTemplateIds = EquMaintenanceTemplateEquipmentGroups.Select(it => it.MaintenanceTemplateId).ToList();

            }
            if (!string.IsNullOrWhiteSpace(EquMaintenanceTemplatePagedQueryDto.EquipmentGroupCode))
            {
                var equGroupEquipment = await _equEquipmentGroupRepository.GetByCodeAsync(new EntityByCodeQuery { Code = EquMaintenanceTemplatePagedQueryDto.EquipmentGroupCode, Site = _currentSite.SiteId });
                if (equGroupEquipment != null)
                {
                    List<long> equGroupEquipmentIds = new()
                    {
                        equGroupEquipment.Id
                    };
                    var EquMaintenanceTemplateEquipmentGroups = await _EquMaintenanceTemplateEquipmentGroupRelationRepository.GetByGroupIdAsync(equGroupEquipmentIds);
                    if (EquMaintenanceTemplateEquipmentGroups != null && EquMaintenanceTemplateEquipmentGroups.Any())
                    {
                        EquMaintenanceTemplatePagedQuery.MaintenanceTemplateIds = EquMaintenanceTemplateEquipmentGroups.Select(it => it.MaintenanceTemplateId).ToList();
                    }
                }

            }
            var pagedInfo = await _EquMaintenanceTemplateRepository.GetPagedInfoAsync(EquMaintenanceTemplatePagedQuery);

            //实体到DTO转换 装载数据
            List<EquMaintenanceTemplateDto> EquMaintenanceTemplateDtos = PrepareEquMaintenanceTemplateDtos(pagedInfo);
            return new PagedInfo<EquMaintenanceTemplateDto>(EquMaintenanceTemplateDtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="pagedInfo"></param>
        /// <returns></returns>
        private static List<EquMaintenanceTemplateDto> PrepareEquMaintenanceTemplateDtos(PagedInfo<EquMaintenanceTemplateEntity> pagedInfo)
        {
            var EquMaintenanceTemplateDtos = new List<EquMaintenanceTemplateDto>();
            foreach (var EquMaintenanceTemplateEntity in pagedInfo.Data)
            {
                var EquMaintenanceTemplateDto = EquMaintenanceTemplateEntity.ToModel<EquMaintenanceTemplateDto>();
                EquMaintenanceTemplateDtos.Add(EquMaintenanceTemplateDto);
            }

            return EquMaintenanceTemplateDtos;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="EquMaintenanceTemplateModifyDto"></param>
        /// <returns></returns>
        public async Task ModifyEquMaintenanceTemplateAsync(EquMaintenanceTemplateModifyDto EquMaintenanceTemplateModifyDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0)
            {
                throw new ValidationException(nameof(ErrorCode.MES10101));
            }

            //验证DTO
            await _validationModifyRules.ValidateAndThrowAsync(EquMaintenanceTemplateModifyDto);

            //DTO转换实体
            var EquMaintenanceTemplateEntity = EquMaintenanceTemplateModifyDto.ToEntity<EquMaintenanceTemplateEntity>();
            EquMaintenanceTemplateEntity.SiteId = _currentSite.SiteId ?? 0;
            EquMaintenanceTemplateEntity.CreatedBy = _currentUser.UserName;
            EquMaintenanceTemplateEntity.CreatedOn = HymsonClock.Now();

            EquMaintenanceTemplateEntity.UpdatedBy = _currentUser.UserName;
            EquMaintenanceTemplateEntity.UpdatedOn = HymsonClock.Now();

            List<long> MaintenanceItemIds = new()
            {
                EquMaintenanceTemplateEntity.Id
            };

            List<EquMaintenanceTemplateItemRelationEntity> addRelation = new();

            if (EquMaintenanceTemplateModifyDto.relationDto != null && EquMaintenanceTemplateModifyDto.relationDto.Any())
            {
                foreach (var item in EquMaintenanceTemplateModifyDto.relationDto)
                {
                    addRelation.Add(new EquMaintenanceTemplateItemRelationEntity
                    {
                        Id = IdGenProvider.Instance.CreateId(),
                        MaintenanceTemplateId = EquMaintenanceTemplateEntity.Id,
                        Center = item.Center,
                        LowerLimit = item.LowerLimit,
                        MaintenanceItemId = item.MaintenanceItemId == 0 ? item.Id : item.MaintenanceItemId,
                        UpperLimit = item.UpperLimit,

                        IsDeleted = 0,
                        CreatedBy = _currentUser.UserName,
                        UpdatedBy = _currentUser.UserName,
                        CreatedOn = HymsonClock.Now(),
                        UpdatedOn = HymsonClock.Now()
                    });
                }
            }


            List<EquMaintenanceTemplateEquipmentGroupRelationEntity> addGroupRelation = new();

            var eGroupIds = EquMaintenanceTemplateModifyDto.groupRelationDto.Select(it => it.Id).ToArray();
            var EquMaintenanceTemplateEquipmentGroupRelations = await _EquMaintenanceTemplateEquipmentGroupRelationRepository.GetByGroupIdAsync(eGroupIds);
            if (EquMaintenanceTemplateEquipmentGroupRelations != null && EquMaintenanceTemplateEquipmentGroupRelations.Any())
            {
                var groupRelationIds = EquMaintenanceTemplateEquipmentGroupRelations.Select(it => it.EquipmentGroupId).ToArray();
                var equipmentGroups = await _equEquipmentGroupRepository.GetByIdsAsync(groupRelationIds);
                var equipmentGroupCodes = string.Join(",", equipmentGroups.Select(it => it.EquipmentGroupCode));
                throw new CustomerValidationException(nameof(ErrorCode.MES12203)).WithData("Code", equipmentGroupCodes);
            }
            foreach (var item in EquMaintenanceTemplateModifyDto.groupRelationDto)
            {
                addGroupRelation.Add(new EquMaintenanceTemplateEquipmentGroupRelationEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    EquipmentGroupId = item.EquipmentGroupId == 0 ? item.Id : item.EquipmentGroupId,
                    MaintenanceTemplateId = EquMaintenanceTemplateEntity.Id,

                    IsDeleted = 0,
                    CreatedBy = _currentUser.UserName,
                    UpdatedBy = _currentUser.UserName,
                    CreatedOn = HymsonClock.Now(),
                    UpdatedOn = HymsonClock.Now(),
                });
            }

            using var trans = TransactionHelper.GetTransactionScope();
            await _EquMaintenanceTemplateRepository.UpdateAsync(EquMaintenanceTemplateEntity);

            await _EquMaintenanceTemplateItemRelationRepository.DeleteByMaintenanceTemplateIdsAsync(MaintenanceItemIds);
            await _EquMaintenanceTemplateItemRelationRepository.InsertsAsync(addRelation);

            await _EquMaintenanceTemplateEquipmentGroupRelationRepository.DeletesByMaintenanceTemplateIdsAsync(MaintenanceItemIds);
            await _EquMaintenanceTemplateEquipmentGroupRelationRepository.InsertsAsync(addGroupRelation);

            trans.Complete();
        }

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<EquMaintenanceTemplateDto> QueryEquMaintenanceTemplateByIdAsync(long id)
        {
            var EquMaintenanceTemplateEntity = await _EquMaintenanceTemplateRepository.GetByIdAsync(id);
            if (EquMaintenanceTemplateEntity != null)
            {
                return EquMaintenanceTemplateEntity.ToModel<EquMaintenanceTemplateDto>();
            }
            return null;
        }



        #region 关联信息
        /// <summary>
        /// 获取模板关联信息（项目）
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<List<GetItemRelationListDto>> QueryItemRelationListAsync(GetEquMaintenanceTemplateItemRelationDto param)
        {
            var EquMaintenanceTemplateItemRelations = await _EquMaintenanceTemplateItemRelationRepository.GetEquMaintenanceTemplateItemRelationEntitiesAsync(new EquMaintenanceTemplateItemRelationQuery
            {
                MaintenanceTemplateIds = param.MaintenanceTemplateIds,
                SiteId = _currentSite.SiteId
            });

            List<GetItemRelationListDto> list = new();
            if (EquMaintenanceTemplateItemRelations != null && EquMaintenanceTemplateItemRelations.Any())
            {
                var MaintenanceItemIds = EquMaintenanceTemplateItemRelations.Select(it => it.MaintenanceItemId).ToArray();
                var EquMaintenanceItems = await _EquMaintenanceItemRepository.GetByIdsAsync(MaintenanceItemIds);

                foreach (var item in EquMaintenanceTemplateItemRelations)
                {
                    var EquMaintenanceItem = EquMaintenanceItems.FirstOrDefault(it => it.Id == item.MaintenanceItemId);
                    GetItemRelationListDto itemRelation = new()
                    {
                        Id = item.Id,
                        MaintenanceItemId = item.MaintenanceItemId,
                        MaintenanceTemplateId = item.MaintenanceTemplateId,
                        Center = item.Center,
                        LowerLimit = item.LowerLimit,
                        UpperLimit = item.UpperLimit,
                        CheckMethod = EquMaintenanceItem?.CheckMethod,
                        CheckType = EquMaintenanceItem?.CheckType,
                        Code = EquMaintenanceItem?.Code,
                        Name = EquMaintenanceItem?.Name,
                        Components = EquMaintenanceItem?.Components,
                        DataType = EquMaintenanceItem?.DataType
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
        public async Task<List<QueryEquipmentGroupRelationListDto>> QueryEquipmentGroupRelationListAsync(GetEquMaintenanceTemplateItemRelationDto param)
        {
            var EquMaintenanceTemplateEquipmentGroupRelations = await _EquMaintenanceTemplateEquipmentGroupRelationRepository.GetEquMaintenanceTemplateEquipmentGroupRelationEntitiesAsync(new EquMaintenanceTemplateEquipmentGroupRelationQuery
            {
                MaintenanceTemplateIds = param.MaintenanceTemplateIds,
                SiteId = _currentSite.SiteId
            });

            List<QueryEquipmentGroupRelationListDto> list = new();
            if (EquMaintenanceTemplateEquipmentGroupRelations != null && EquMaintenanceTemplateEquipmentGroupRelations.Any())
            {
                var equipmentGroupIds = EquMaintenanceTemplateEquipmentGroupRelations.Select(it => it.EquipmentGroupId).ToArray();
                var EquMaintenanceItems = await _equEquipmentGroupRepository.GetByIdsAsync(equipmentGroupIds);

                foreach (var item in EquMaintenanceTemplateEquipmentGroupRelations)
                {
                    var EquMaintenanceItem = EquMaintenanceItems.FirstOrDefault(it => it.Id == item.EquipmentGroupId);
                    QueryEquipmentGroupRelationListDto groupRelation = new()
                    {
                        Id = item.Id,
                        EquipmentGroupId = item.EquipmentGroupId,
                        MaintenanceTemplateId = item.MaintenanceTemplateId,
                        EquipmentGroupCode = EquMaintenanceItem?.EquipmentGroupCode,
                        EquipmentGroupName = EquMaintenanceItem?.EquipmentGroupName,
                        Remark = EquMaintenanceItem?.Remark,
                    };

                    list.Add(groupRelation);
                }
            }

            return list;
        }
        #endregion
    }
}
