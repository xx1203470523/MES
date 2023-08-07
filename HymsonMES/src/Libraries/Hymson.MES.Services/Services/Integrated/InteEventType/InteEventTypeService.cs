using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Integrated;
using Hymson.MES.Core.Enums;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.MES.Data.Repositories.Integrated;
using Hymson.MES.Data.Repositories.Integrated.Query;
using Hymson.MES.Services.Dtos.Integrated;
using Hymson.Snowflake;
using Hymson.Utils;

namespace Hymson.MES.Services.Services.Integrated
{
    /// <summary>
    /// 服务（事件维护） 
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
        /// 仓储接口（事件维护）
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
        /// <param name="inteEventTypeRepository"></param>
        /// <param name="inteEventTypeMessageGroupRelationRepository"></param>
        /// <param name="inteEventTypeUpgradeRepository"></param>
        /// <param name="inteEventTypeUpgradeMessageGroupRelationRepository"></param>
        /// <param name="inteEventTypePushRuleRepository"></param>
        public InteEventTypeService(ICurrentUser currentUser, ICurrentSite currentSite, AbstractValidator<InteEventTypeSaveDto> validationSaveRules,
            IInteMessageGroupRepository inteMessageGroupRepository,
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
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0) throw new ValidationException(nameof(ErrorCode.MES10101));

            // 验证DTO
            await _validationSaveRules.ValidateAndThrowAsync(saveDto);

            // 更新时间
            var updatedBy = _currentUser.UserName;
            var updatedOn = HymsonClock.Now();

            // DTO转换实体
            var entity = saveDto.ToEntity<InteEventTypeEntity>();
            entity.Id = IdGenProvider.Instance.CreateId();
            entity.CreatedBy = updatedBy;
            entity.CreatedOn = updatedOn;
            entity.UpdatedBy = updatedBy;
            entity.UpdatedOn = updatedOn;
            entity.SiteId = _currentSite.SiteId ?? 0;

            // 保存
            return await _inteEventTypeRepository.InsertAsync(entity);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        public async Task<int> ModifyAsync(InteEventTypeSaveDto saveDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0) throw new ValidationException(nameof(ErrorCode.MES10101));

            // 验证DTO
            await _validationSaveRules.ValidateAndThrowAsync(saveDto);

            // DTO转换实体
            var entity = saveDto.ToEntity<InteEventTypeEntity>();
            entity.UpdatedBy = _currentUser.UserName;
            entity.UpdatedOn = HymsonClock.Now();

            return await _inteEventTypeRepository.UpdateAsync(entity);
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
            return await _inteEventTypeRepository.DeletesAsync(new DeleteCommand
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
        public async Task<InteEventTypeDto?> QueryByIdAsync(long id)
        {
            var inteEventTypeEntity = await _inteEventTypeRepository.GetByIdAsync(id);
            if (inteEventTypeEntity == null) return null;

            return inteEventTypeEntity.ToModel<InteEventTypeDto>();
        }

        /// <summary>
        /// 根据ID获取关联群组
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IEnumerable<InteEventTypeMessageGroupRelationDto>> QueryMessageGroupsByMainIdAsync(long id)
        {
            var entities = await _inteEventTypeMessageGroupRelationRepository.GetEntitiesAsync(new EntityByParentIdQuery
            {
                ParentId = id
            });

            return entities.Select(s => s.ToModel<InteEventTypeMessageGroupRelationDto>());
        }

        /// <summary>
        /// 根据ID获取升级数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IEnumerable<InteEventTypeUpgradeDto>> QueryReceivesByMainIdAsync(long id)
        {
            return await GetUpgradeByMainIdAsync(id, PushSceneEnum.ReceiveUpgrade);
        }

        /// <summary>
        /// 根据ID获取处理升级数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IEnumerable<InteEventTypeUpgradeDto>> QueryHandlesByMainIdAsync(long id)
        {
            return await GetUpgradeByMainIdAsync(id, PushSceneEnum.HandleUpgrade);
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
        /// 根据ID获取升级数据
        /// </summary>
        /// <param name="id"></param>
        /// <param name="pushScene"></param>
        /// <returns></returns>
        private async Task<IEnumerable<InteEventTypeUpgradeDto>> GetUpgradeByMainIdAsync(long id, PushSceneEnum pushScene)
        {
            // 升级信息
            var entities = await _inteEventTypeUpgradeRepository.GetEntitiesAsync(new InteEventTypeUpgradeQuery
            {
                EventTypeId = id,
                PushScene = pushScene
            });

            // 升级消息组关联信息
            var messageGroupRelationEntities = await _inteEventTypeUpgradeMessageGroupRelationRepository.GetEntitiesAsync(new InteEventTypeUpgradeMessageGroupRelationQuery
            {
                EventTypeId = id,
                PushScene = pushScene
            });
            var messageGroupRelationDic = messageGroupRelationEntities.ToLookup(w => w.EventTypeUpgradeId).ToDictionary(d => d.Key, d => d);

            // 消息组基础信息（已缓存）
            var messageGroupEntities = await _inteMessageGroupRepository.GetEntitiesAsync(new EntityBySiteIdQuery { SiteId = _currentSite.SiteId ?? 0 });

            // 组装数据
            List<InteEventTypeUpgradeDto> dtos = new();
            foreach (var entity in entities)
            {
                var dto = entity.ToModel<InteEventTypeUpgradeDto>();
                if (messageGroupRelationDic.TryGetValue(entity.Id, out var messageGroupRelations) == false) continue;

                List<InteEventTypeUpgradeMessageGroupRelationDto> messageGroups = new();
                foreach (var item in messageGroupRelations)
                {
                    var messageGroupEntity = messageGroupEntities.FirstOrDefault(f => f.Id == item.MessageGroupId);
                    if (messageGroupEntity == null) continue;

                    messageGroups.Add(new InteEventTypeUpgradeMessageGroupRelationDto
                    {
                        Id = item.Id,
                        MessageGroupId = item.MessageGroupId,
                        PushTypes = item.PushTypes,
                        Code = messageGroupEntity.Code,
                        Name = messageGroupEntity.Name
                    });
                }
                dto.MessageGroups = messageGroups;
                dtos.Add(dto);
            }

            return dtos;
        }
        #endregion

    }
}
