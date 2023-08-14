using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Integrated;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Integrated;
using Hymson.MES.CoreServices.Bos.Integrated;
using Hymson.MES.CoreServices.Dtos.Common;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.MES.Data.Repositories.Integrated;
using Hymson.MES.Data.Repositories.Integrated.InteEvent.Command;
using Hymson.MES.Data.Repositories.Integrated.InteWorkCenter.Query;
using Hymson.MES.Data.Repositories.Integrated.Query;
using Hymson.MES.Services.Dtos.Integrated;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;
using System.Diagnostics;
using System.Diagnostics.Tracing;

namespace Hymson.MES.Services.Services.Integrated
{
    /// <summary>
    /// 服务（事件类型维护） 
    /// </summary>
    public class InteEventTypeService : IInteEventTypeService
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
        private readonly AbstractValidator<InteEventTypeSaveDto> _validationSaveRules;

        /// <summary>
        /// 仓储接口（消息组）
        /// </summary>
        private readonly IInteMessageGroupRepository _inteMessageGroupRepository;

        /// <summary>
        /// 仓储接口（消息组推送方式）
        /// </summary>
        private readonly IInteMessageGroupPushMethodRepository _inteMessageGroupPushMethodRepository;

        /// <summary>
        /// 仓储接口（事件维护）
        /// </summary>
        private readonly IInteEventRepository _inteEventRepository;

        /// <summary>
        /// 仓储接口（事件类型维护）
        /// </summary>
        private readonly IInteEventTypeRepository _inteEventTypeRepository;

        /// <summary>
        /// 仓储接口（事件类型关联群组）
        /// </summary>
        private readonly IInteEventTypeMessageGroupRelationRepository _inteEventTypeMessageGroupRelationRepository;

        /// <summary>
        /// 仓储接口（事件升级）
        /// </summary>
        private readonly IInteEventTypeUpgradeRepository _inteEventTypeUpgradeRepository;

        /// <summary>
        /// 仓储接口（事件升级消息组关联表）
        /// </summary>
        private readonly IInteEventTypeUpgradeMessageGroupRelationRepository _inteEventTypeUpgradeMessageGroupRelationRepository;

        /// <summary>
        /// 仓储接口（事件类型推送规则）
        /// </summary>
        private readonly IInteEventTypePushRuleRepository _inteEventTypePushRuleRepository;


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="currentSite"></param>
        /// <param name="validationSaveRules"></param>
        /// <param name="inteMessageGroupRepository"></param>
        /// <param name="inteMessageGroupPushMethodRepository"></param>
        /// <param name="inteEventRepository"></param>
        /// <param name="inteEventTypeRepository"></param>
        /// <param name="inteEventTypeMessageGroupRelationRepository"></param>
        /// <param name="inteEventTypeUpgradeRepository"></param>
        /// <param name="inteEventTypeUpgradeMessageGroupRelationRepository"></param>
        /// <param name="inteEventTypePushRuleRepository"></param>
        public InteEventTypeService(ICurrentUser currentUser, ICurrentSite currentSite, AbstractValidator<InteEventTypeSaveDto> validationSaveRules,
            IInteMessageGroupRepository inteMessageGroupRepository,
            IInteMessageGroupPushMethodRepository inteMessageGroupPushMethodRepository,
            IInteEventRepository inteEventRepository,
            IInteEventTypeRepository inteEventTypeRepository,
            IInteEventTypeMessageGroupRelationRepository inteEventTypeMessageGroupRelationRepository,
            IInteEventTypeUpgradeRepository inteEventTypeUpgradeRepository,
            IInteEventTypeUpgradeMessageGroupRelationRepository inteEventTypeUpgradeMessageGroupRelationRepository,
            IInteEventTypePushRuleRepository inteEventTypePushRuleRepository)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _validationSaveRules = validationSaveRules;
            _inteMessageGroupRepository = inteMessageGroupRepository;
            _inteMessageGroupPushMethodRepository = inteMessageGroupPushMethodRepository;
            _inteEventRepository = inteEventRepository;
            _inteEventTypeRepository = inteEventTypeRepository;
            _inteEventTypeMessageGroupRelationRepository = inteEventTypeMessageGroupRelationRepository;
            _inteEventTypeUpgradeRepository = inteEventTypeUpgradeRepository;
            _inteEventTypeUpgradeMessageGroupRelationRepository = inteEventTypeUpgradeMessageGroupRelationRepository;
            _inteEventTypePushRuleRepository = inteEventTypePushRuleRepository;
        }


        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        public async Task<int> CreateAsync(InteEventTypeSaveDto saveDto)
        {
            // 验证DTO
            await _validationSaveRules.ValidateAndThrowAsync(saveDto);

            // 更新时间
            var updatedBy = _currentUser.UserName;
            var updatedOn = HymsonClock.Now();

            // DTO转换实体
            var entity = saveDto.ToEntity<InteEventTypeEntity>();
            entity.Id = IdGenProvider.Instance.CreateId();
            entity.SiteId = _currentSite.SiteId ?? 0;
            entity.CreatedBy = updatedBy;
            entity.CreatedOn = updatedOn;
            entity.UpdatedBy = updatedBy;
            entity.UpdatedOn = updatedOn;

            // 编码唯一性验证
            var checkEntity = await _inteEventTypeRepository.GetByCodeAsync(new EntityByCodeQuery
            {
                Site = entity.SiteId,
                Code = entity.Code
            });
            if (checkEntity != null) throw new CustomerValidationException(nameof(ErrorCode.MES10521)).WithData("Code", entity.Code);

            // 关联事件
            saveDto.EventIds ??= new List<long>();
            var eventCommand = new UpdateEventTypeIdCommand
            {
                UpdatedBy = updatedBy,
                UpdatedOn = updatedOn,
                EventTypeId = entity.Id,
                Ids = saveDto.EventIds
            };

            // 关联消息组
            saveDto.MessageGroups ??= new List<InteEventTypeMessageGroupRelationDto>();
            var messageGroupEntities = saveDto.MessageGroups.Select(s =>
            {
                var detailEntity = s.ToEntity<InteEventTypeMessageGroupRelationEntity>();
                detailEntity.PushTypes = s.PushTypeArray.ToSerialize();
                detailEntity.Id = IdGenProvider.Instance.CreateId();
                detailEntity.EventTypeId = entity.Id;
                detailEntity.SiteId = entity.SiteId;
                detailEntity.CreatedBy = updatedBy;
                detailEntity.CreatedOn = updatedOn;
                detailEntity.UpdatedBy = updatedBy;
                detailEntity.UpdatedOn = updatedOn;
                return detailEntity;
            });

            // 升级
            //List<ValidationFailure> validationFailures = new();
            List<InteEventTypeUpgradeEntity> upgrades = new();
            List<InteEventTypeUpgradeMessageGroupRelationEntity> upgradeMessageGroupRelations = new();

            // 接收升级
            saveDto.ReceiveUpgrades ??= new List<InteEventTypeUpgradeDto>();
            foreach (var item in saveDto.ReceiveUpgrades)
            {
                var detailEntity = item.ToEntity<InteEventTypeUpgradeEntity>();
                detailEntity.Id = IdGenProvider.Instance.CreateId();
                detailEntity.PushScene = PushSceneEnum.ReceiveUpgrade;
                detailEntity.EventTypeId = entity.Id;
                detailEntity.SiteId = entity.SiteId;
                detailEntity.CreatedBy = updatedBy;
                detailEntity.CreatedOn = updatedOn;
                detailEntity.UpdatedBy = updatedBy;
                detailEntity.UpdatedOn = updatedOn;
                upgrades.Add(detailEntity);

                item.MessageGroups ??= new List<InteEventTypeMessageGroupRelationDto>();
                upgradeMessageGroupRelations.AddRange(item.MessageGroups.Select(s =>
                {
                    var relationEntity = s.ToEntity<InteEventTypeUpgradeMessageGroupRelationEntity>();
                    relationEntity.PushTypes = s.PushTypeArray.ToSerialize();
                    relationEntity.Id = IdGenProvider.Instance.CreateId();
                    relationEntity.EventTypeId = detailEntity.EventTypeId;
                    relationEntity.PushScene = detailEntity.PushScene;
                    relationEntity.EventTypeUpgradeId = detailEntity.Id;
                    relationEntity.SiteId = entity.SiteId;
                    relationEntity.CreatedBy = updatedBy;
                    relationEntity.CreatedOn = updatedOn;
                    relationEntity.UpdatedBy = updatedBy;
                    relationEntity.UpdatedOn = updatedOn;
                    return relationEntity;
                }));
            }

            // 处理升级
            saveDto.HandleUpgrades ??= new List<InteEventTypeUpgradeDto>();
            foreach (var item in saveDto.HandleUpgrades)
            {
                var detailEntity = item.ToEntity<InteEventTypeUpgradeEntity>();
                detailEntity.Id = IdGenProvider.Instance.CreateId();
                detailEntity.PushScene = PushSceneEnum.HandleUpgrade;
                detailEntity.EventTypeId = entity.Id;
                detailEntity.SiteId = entity.SiteId;
                detailEntity.CreatedBy = updatedBy;
                detailEntity.CreatedOn = updatedOn;
                detailEntity.UpdatedBy = updatedBy;
                detailEntity.UpdatedOn = updatedOn;
                upgrades.Add(detailEntity);

                item.MessageGroups ??= new List<InteEventTypeMessageGroupRelationDto>();
                upgradeMessageGroupRelations.AddRange(item.MessageGroups.Select(s =>
                {
                    var relationEntity = s.ToEntity<InteEventTypeUpgradeMessageGroupRelationEntity>();
                    relationEntity.PushTypes = s.PushTypeArray.ToSerialize();
                    relationEntity.Id = IdGenProvider.Instance.CreateId();
                    relationEntity.EventTypeId = detailEntity.EventTypeId;
                    relationEntity.PushScene = detailEntity.PushScene;
                    relationEntity.EventTypeUpgradeId = detailEntity.Id;
                    relationEntity.SiteId = entity.SiteId;
                    relationEntity.CreatedBy = updatedBy;
                    relationEntity.CreatedOn = updatedOn;
                    relationEntity.UpdatedBy = updatedBy;
                    relationEntity.UpdatedOn = updatedOn;
                    return relationEntity;
                }));
            }

            // 推送规则
            var ruleEntities = saveDto.Rules.Select(s =>
            {
                var detailEntity = s.ToEntity<InteEventTypePushRuleEntity>();
                detailEntity.Id = IdGenProvider.Instance.CreateId();
                detailEntity.EventTypeId = entity.Id;
                detailEntity.SiteId = entity.SiteId;
                detailEntity.CreatedBy = updatedBy;
                detailEntity.CreatedOn = updatedOn;
                detailEntity.UpdatedBy = updatedBy;
                detailEntity.UpdatedOn = updatedOn;
                return detailEntity;
            });

            var rows = 0;
            using (var trans = TransactionHelper.GetTransactionScope())
            {
                var rowArray = await Task.WhenAll(new List<Task<int>>()
                {
                    _inteEventRepository.UpdateEventTypeIdAsync(eventCommand),
                    _inteEventTypeRepository.InsertAsync(entity),
                    _inteEventTypeMessageGroupRelationRepository.InsertRangeAsync(messageGroupEntities),
                    _inteEventTypeUpgradeRepository.InsertRangeAsync(upgrades),
                    _inteEventTypeUpgradeMessageGroupRelationRepository.InsertRangeAsync(upgradeMessageGroupRelations),
                    _inteEventTypePushRuleRepository.InsertRangeAsync(ruleEntities)
                });
                rows += rowArray.Sum();
                trans.Complete();
            }
            return rows;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        public async Task<int> ModifyAsync(InteEventTypeSaveDto saveDto)
        {
            // 验证DTO
            await _validationSaveRules.ValidateAndThrowAsync(saveDto);

            // 更新时间
            var updatedBy = _currentUser.UserName;
            var updatedOn = HymsonClock.Now();

            // DTO转换实体
            var entity = saveDto.ToEntity<InteEventTypeEntity>();
            entity.SiteId = _currentSite.SiteId ?? 0;
            entity.UpdatedBy = updatedBy;
            entity.UpdatedOn = updatedOn;

            // 编码唯一性验证
            var checkEntity = await _inteEventTypeRepository.GetByCodeAsync(new EntityByCodeQuery
            {
                Site = entity.SiteId,
                Code = entity.Code
            });
            if (checkEntity != null && checkEntity.Id != entity.Id)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10521)).WithData("Code", entity.Code);
            }

            // 关联事件
            saveDto.EventIds ??= new List<long>();
            var eventCommand = new UpdateEventTypeIdCommand
            {
                UpdatedBy = updatedBy,
                UpdatedOn = updatedOn,
                EventTypeId = entity.Id,
                Ids = saveDto.EventIds
            };

            // 关联消息组
            saveDto.MessageGroups ??= new List<InteEventTypeMessageGroupRelationDto>();
            var messageGroupEntities = saveDto.MessageGroups.Select(s =>
            {
                var detailEntity = s.ToEntity<InteEventTypeMessageGroupRelationEntity>();
                detailEntity.PushTypes = s.PushTypeArray.ToSerialize();
                detailEntity.Id = IdGenProvider.Instance.CreateId();
                detailEntity.EventTypeId = entity.Id;
                detailEntity.SiteId = entity.SiteId;
                detailEntity.CreatedBy = updatedBy;
                detailEntity.CreatedOn = updatedOn;
                detailEntity.UpdatedBy = updatedBy;
                detailEntity.UpdatedOn = updatedOn;
                return detailEntity;
            });

            // 升级
            //List<ValidationFailure> validationFailures = new();
            List<InteEventTypeUpgradeEntity> upgrades = new();
            List<InteEventTypeUpgradeMessageGroupRelationEntity> upgradeMessageGroupRelations = new();

            // 接收升级
            saveDto.ReceiveUpgrades ??= new List<InteEventTypeUpgradeDto>();
            foreach (var item in saveDto.ReceiveUpgrades)
            {
                var detailEntity = item.ToEntity<InteEventTypeUpgradeEntity>();
                detailEntity.Id = IdGenProvider.Instance.CreateId();
                detailEntity.PushScene = PushSceneEnum.ReceiveUpgrade;
                detailEntity.EventTypeId = entity.Id;
                detailEntity.SiteId = entity.SiteId;
                detailEntity.CreatedBy = updatedBy;
                detailEntity.CreatedOn = updatedOn;
                detailEntity.UpdatedBy = updatedBy;
                detailEntity.UpdatedOn = updatedOn;
                upgrades.Add(detailEntity);

                item.MessageGroups ??= new List<InteEventTypeMessageGroupRelationDto>();
                upgradeMessageGroupRelations.AddRange(item.MessageGroups.Select(s =>
                {
                    var relationEntity = s.ToEntity<InteEventTypeUpgradeMessageGroupRelationEntity>();
                    relationEntity.PushTypes = s.PushTypeArray.ToSerialize();
                    relationEntity.Id = IdGenProvider.Instance.CreateId();
                    relationEntity.EventTypeId = detailEntity.EventTypeId;
                    relationEntity.PushScene = detailEntity.PushScene;
                    relationEntity.EventTypeUpgradeId = detailEntity.Id;
                    relationEntity.SiteId = entity.SiteId;
                    relationEntity.CreatedBy = updatedBy;
                    relationEntity.CreatedOn = updatedOn;
                    relationEntity.UpdatedBy = updatedBy;
                    relationEntity.UpdatedOn = updatedOn;
                    return relationEntity;
                }));
            }

            // 处理升级
            saveDto.HandleUpgrades ??= new List<InteEventTypeUpgradeDto>();
            foreach (var item in saveDto.HandleUpgrades)
            {
                var detailEntity = item.ToEntity<InteEventTypeUpgradeEntity>();
                detailEntity.Id = IdGenProvider.Instance.CreateId();
                detailEntity.PushScene = PushSceneEnum.HandleUpgrade;
                detailEntity.EventTypeId = entity.Id;
                detailEntity.SiteId = entity.SiteId;
                detailEntity.CreatedBy = updatedBy;
                detailEntity.CreatedOn = updatedOn;
                detailEntity.UpdatedBy = updatedBy;
                detailEntity.UpdatedOn = updatedOn;
                upgrades.Add(detailEntity);

                item.MessageGroups ??= new List<InteEventTypeMessageGroupRelationDto>();
                upgradeMessageGroupRelations.AddRange(item.MessageGroups.Select(s =>
                {
                    var relationEntity = s.ToEntity<InteEventTypeUpgradeMessageGroupRelationEntity>();
                    relationEntity.PushTypes = s.PushTypeArray.ToSerialize();
                    relationEntity.Id = IdGenProvider.Instance.CreateId();
                    relationEntity.EventTypeId = detailEntity.EventTypeId;
                    relationEntity.PushScene = detailEntity.PushScene;
                    relationEntity.EventTypeUpgradeId = detailEntity.Id;
                    relationEntity.SiteId = entity.SiteId;
                    relationEntity.CreatedBy = updatedBy;
                    relationEntity.CreatedOn = updatedOn;
                    relationEntity.UpdatedBy = updatedBy;
                    relationEntity.UpdatedOn = updatedOn;
                    return relationEntity;
                }));
            }

            // 推送规则
            var ruleEntities = saveDto.Rules.Select(s =>
            {
                var detailEntity = s.ToEntity<InteEventTypePushRuleEntity>();
                detailEntity.Id = IdGenProvider.Instance.CreateId();
                detailEntity.EventTypeId = entity.Id;
                detailEntity.SiteId = entity.SiteId;
                detailEntity.CreatedBy = updatedBy;
                detailEntity.CreatedOn = updatedOn;
                detailEntity.UpdatedBy = updatedBy;
                detailEntity.UpdatedOn = updatedOn;
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
                await _inteEventRepository.ClearEventTypeIdAsync(entity.Id);
                await _inteEventTypeMessageGroupRelationRepository.DeleteByParentIdAsync(command);
                await _inteEventTypeUpgradeRepository.DeleteByParentIdAsync(command);
                await _inteEventTypeUpgradeMessageGroupRelationRepository.DeleteByParentIdAsync(command);
                await _inteEventTypePushRuleRepository.DeleteByParentIdAsync(command);

                var rowArray = await Task.WhenAll(new List<Task<int>>()
                {
                    _inteEventRepository.UpdateEventTypeIdAsync(eventCommand),
                    _inteEventTypeRepository.UpdateAsync(entity),
                    _inteEventTypeMessageGroupRelationRepository.InsertRangeAsync(messageGroupEntities),
                    _inteEventTypeUpgradeRepository.InsertRangeAsync(upgrades),
                    _inteEventTypeUpgradeMessageGroupRelationRepository.InsertRangeAsync(upgradeMessageGroupRelations),
                    _inteEventTypePushRuleRepository.InsertRangeAsync(ruleEntities)
                });
                rows += rowArray.Sum();
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
            return await _inteEventTypeRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeletesAsync(long[] ids)
        {
            var rows = 0;
            using (var trans = TransactionHelper.GetTransactionScope())
            {
                var rowArray = await Task.WhenAll(new List<Task<int>>()
                {
                    _inteEventRepository.ClearEventTypeIdsAsync(ids),
                    _inteEventTypeRepository.DeletesAsync(new DeleteCommand
                    {
                        Ids = ids,
                        DeleteOn = HymsonClock.Now(),
                        UserId = _currentUser.UserName
                    })
                });
                rows += rowArray.Sum();
                trans.Complete();
            }
            return rows;
        }

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<InteEventTypeDto?> QueryByIdAsync(long id)
        {
            var inteEventTypeEntity = await _inteEventTypeRepository.GetByIdAsync(id);
            if (inteEventTypeEntity == null) return null;

            return inteEventTypeEntity.ToModel<InteEventTypeDto>();
        }

        /// <summary>
        /// 查询事件类型
        /// </summary>
        /// <param name="workShopId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<SelectOptionDto>> QueryEventTypesByWorkShopIdAsync(long workShopId)
        {
            var inteEventTypeEntities = await _inteEventTypeRepository.GetByWorkShopIdSqlAsync(workShopId);
            return inteEventTypeEntities.Select(s => new SelectOptionDto
            {
                Key = $"{s.Id}",
                Label = $"{s.Name}",
                Value = $"{s.Id}"
            });
        }

        /// <summary>
        /// 根据ID获取关联事件
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IEnumerable<InteEventBaseDto>> QueryEventsByMainIdAsync(long id)
        {
            var eventEntities = await _inteEventRepository.GetEntitiesAsync(new EntityBySiteIdQuery { SiteId = _currentSite.SiteId ?? 0 });

            return eventEntities.Where(w => w.EventTypeId == id || w.EventTypeId == 0)
                .Select(s => s.ToModel<InteEventBaseDto>());
        }

        /// <summary>
        /// 根据ID获取关联群组
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IEnumerable<InteEventTypeMessageGroupRelationDto>> QueryMessageGroupsByMainIdAsync(long id)
        {
            var messageGroupRelationEntities = await _inteEventTypeMessageGroupRelationRepository.GetEntitiesAsync(new EntityByParentIdQuery
            {
                ParentId = id
            });

            // 消息组基础信息（已缓存）
            var messageGroupEntities = await _inteMessageGroupRepository.GetEntitiesAsync(new EntityBySiteIdQuery { SiteId = _currentSite.SiteId ?? 0 });

            // 消息组关联推送方式（已缓存）
            var messageGroupPushMethodEntities = await _inteMessageGroupPushMethodRepository.GetEntitiesAsync(new EntityBySiteIdQuery { SiteId = _currentSite.SiteId ?? 0 });

            return GetMessageGroupRelations(messageGroupRelationEntities.Select(s => s.ToModel<MessageGroupBo>()), messageGroupEntities, messageGroupPushMethodEntities);
        }

        /// <summary>
        /// 根据ID获取升级数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<PagedInfo<InteEventTypeUpgradeDto>> GetUpgradeByMainIdAsync(InteEventTypeUpgradePagedQueryDto query)
        {
            // 升级信息
            var entities = await _inteEventTypeUpgradeRepository.GetEntitiesAsync(new InteEventTypeUpgradeQuery
            {
                EventTypeId = query.EventTypeId,
                PushScene = query.PushScene
            });

            // 升级消息组关联信息
            var messageGroupRelationEntities = await _inteEventTypeUpgradeMessageGroupRelationRepository.GetEntitiesAsync(new InteEventTypeUpgradeMessageGroupRelationQuery
            {
                EventTypeId = query.EventTypeId,
                PushScene = query.PushScene
            });
            var messageGroupRelationDic = messageGroupRelationEntities.ToLookup(w => w.EventTypeUpgradeId).ToDictionary(d => d.Key, d => d);

            // 消息组基础信息（已缓存）
            var messageGroupEntities = await _inteMessageGroupRepository.GetEntitiesAsync(new EntityBySiteIdQuery { SiteId = _currentSite.SiteId ?? 0 });

            // 消息组关联推送方式（已缓存）
            var messageGroupPushMethodEntities = await _inteMessageGroupPushMethodRepository.GetEntitiesAsync(new EntityBySiteIdQuery { SiteId = _currentSite.SiteId ?? 0 });

            // 组装数据
            List<InteEventTypeUpgradeDto> dtos = new();
            foreach (var entity in entities)
            {
                var dto = entity.ToModel<InteEventTypeUpgradeDto>();
                if (messageGroupRelationDic.TryGetValue(entity.Id, out var messageGroupRelations) == false) continue;

                dto.MessageGroups = GetMessageGroupRelations(messageGroupRelations.Select(s => s.ToModel<MessageGroupBo>()), messageGroupEntities, messageGroupPushMethodEntities);
                dtos.Add(dto);
            }

            return new PagedInfo<InteEventTypeUpgradeDto>(dtos, 1, 9999);
        }

        /// <summary>
        /// 根据ID获取推送规则
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IEnumerable<InteEventTypePushRuleDto>> QueryRulesByMainIdAsync(long id)
        {
            var entities = await _inteEventTypePushRuleRepository.GetEntitiesAsync(new EntityByParentIdQuery
            {
                ParentId = id
            });

            return entities.Select(s => s.ToModel<InteEventTypePushRuleDto>());
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<InteEventTypeDto>> GetPagedListAsync(InteEventTypePagedQueryDto pagedQueryDto)
        {
            var pagedQuery = pagedQueryDto.ToQuery<InteEventTypePagedQuery>();
            pagedQuery.SiteId = _currentSite.SiteId ?? 0;
            var pagedInfo = await _inteEventTypeRepository.GetPagedListAsync(pagedQuery);

            // 实体到DTO转换 装载数据
            var dtos = pagedInfo.Data.Select(s => s.ToModel<InteEventTypeDto>());
            return new PagedInfo<InteEventTypeDto>(dtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }


        #region 内部方法
        /// <summary>
        /// 获取消息组关联信息
        /// </summary>
        /// <param name="messageGroupBos"></param>
        /// <param name="messageGroupEntities"></param>
        /// <param name="messageGroupPushMethodEntities"></param>
        /// <returns></returns>
        private static IEnumerable<InteEventTypeMessageGroupRelationDto> GetMessageGroupRelations(IEnumerable<MessageGroupBo> messageGroupBos,
            IEnumerable<InteMessageGroupEntity> messageGroupEntities,
            IEnumerable<InteMessageGroupPushMethodEntity> messageGroupPushMethodEntities)
        {
            var messageGroupPushMethodDic = messageGroupPushMethodEntities.ToLookup(w => w.MessageGroupId).ToDictionary(d => d.Key, d => d);

            List<InteEventTypeMessageGroupRelationDto> dtos = new();
            foreach (var item in messageGroupBos)
            {
                var pushTypeArray = item.PushTypes.ToDeserialize<IEnumerable<PushTypeEnum>>();
                if (pushTypeArray == null) continue;

                // 消息组基础信息
                var messageGroupEntity = messageGroupEntities.FirstOrDefault(f => f.Id == item.MessageGroupId);
                if (messageGroupEntity == null) continue;

                // 消息组推送方式
                if (messageGroupPushMethodDic.TryGetValue(item.MessageGroupId, out var messageGroupPushMethods) == false) continue;

                dtos.Add(new InteEventTypeMessageGroupRelationDto
                {
                    Id = item.Id,
                    MessageGroupId = item.MessageGroupId,
                    PushTypeArray = pushTypeArray ?? new List<PushTypeEnum>(),
                    EnabledPushType = messageGroupPushMethods.Select(s => s.Type).Distinct(),
                    Code = messageGroupEntity.Code,
                    Name = messageGroupEntity.Name
                });
            }
            return dtos;
        }
        #endregion

    }
}
