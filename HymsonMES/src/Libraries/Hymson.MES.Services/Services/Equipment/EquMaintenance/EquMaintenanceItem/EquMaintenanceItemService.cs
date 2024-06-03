using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Equipment;
using Hymson.MES.Core.Domain.Equipment.EquSpotcheck;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Equipment.EquMaintenance.EquMaintenanceItem;
using Hymson.MES.Data.Repositories.Equipment.EquMaintenance.EquMaintenanceItem.Query;
using Hymson.MES.Data.Repositories.Equipment.Query;
using Hymson.MES.Data.Repositories.EquMaintenanceTemplate;
using Hymson.MES.Data.Repositories.EquMaintenanceTemplateItemRelation;
using Hymson.MES.Data.Repositories.Integrated;
using Hymson.MES.Services;
using Hymson.MES.Services.Dtos.Equipment;
using Hymson.Snowflake;
using Hymson.Utils;

namespace Hymson.MES.Services.Services.Equipment.EquMaintenance.EquMaintenanceItem
{
    /// <summary>
    /// 服务（设备保养项目） 
    /// </summary>
    public class EquMaintenanceItemService : IEquMaintenanceItemService
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
        private readonly AbstractValidator<EquMaintenanceItemSaveDto> _validationSaveRules;

        /// <summary>
        /// 仓储接口（设备保养项目）
        /// </summary>
        private readonly IEquMaintenanceItemRepository _equMaintenanceItemRepository;

        /// <summary>
        /// 单位
        /// </summary>
        private readonly IInteUnitRepository _inteUnitRepository;

        private readonly IEquMaintenanceTemplateItemRelationRepository _EquMaintenanceTemplateItemRelationRepository;
        private readonly IEquMaintenanceTemplateRepository _equMaintenanceTemplateRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="currentSite"></param>
        /// <param name="validationSaveRules"></param>
        /// <param name="equMaintenanceItemRepository"></param>
        public EquMaintenanceItemService(ICurrentUser currentUser, ICurrentSite currentSite, AbstractValidator<EquMaintenanceItemSaveDto> validationSaveRules,
            IEquMaintenanceItemRepository equMaintenanceItemRepository,
            IInteUnitRepository inteUnitRepository, 
            IEquMaintenanceTemplateItemRelationRepository equMaintenanceTemplateItemRelationRepository,
            IEquMaintenanceTemplateRepository equMaintenanceTemplateRepository)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _validationSaveRules = validationSaveRules;
            _equMaintenanceItemRepository = equMaintenanceItemRepository;
            _inteUnitRepository = inteUnitRepository;
            _EquMaintenanceTemplateItemRelationRepository = equMaintenanceTemplateItemRelationRepository;
            _equMaintenanceTemplateRepository= equMaintenanceTemplateRepository;
        }


        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        public async Task<int> CreateAsync(EquMaintenanceItemSaveDto saveDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0) throw new CustomerValidationException(nameof(ErrorCode.MES10101));

            // 验证DTO
            await _validationSaveRules.ValidateAndThrowAsync(saveDto);

            var Entitys = await _equMaintenanceItemRepository.GetEntitiesAsync(new EquMaintenanceItemQuery
            {
                Code = saveDto.Code,
                SiteId = _currentSite.SiteId ?? 0
            });
            if (Entitys.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10405)).WithData("Code", saveDto.Code);
            }

            // 更新时间
            var updatedBy = _currentUser.UserName;
            var updatedOn = HymsonClock.Now();

            // DTO转换实体
            var entity = saveDto.ToEntity<EquMaintenanceItemEntity>();
            entity.Id = IdGenProvider.Instance.CreateId();
            entity.CreatedBy = updatedBy;
            entity.CreatedOn = updatedOn;
            entity.UpdatedBy = updatedBy;
            entity.UpdatedOn = updatedOn;
            entity.SiteId = _currentSite.SiteId ?? 0;

            // 保存
            return await _equMaintenanceItemRepository.InsertAsync(entity);

        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        public async Task<int> ModifyAsync(EquMaintenanceItemUpdateDto saveDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0) throw new CustomerValidationException(nameof(ErrorCode.MES10101));

            // 验证DTO
            await _validationSaveRules.ValidateAndThrowAsync(saveDto);

            // DTO转换实体
            var entity = saveDto.ToEntity<EquMaintenanceItemEntity>();
            entity.UpdatedBy = _currentUser.UserName;
            entity.UpdatedOn = HymsonClock.Now();
            entity.SiteId = _currentSite.SiteId ?? 0;

            return await _equMaintenanceItemRepository.UpdateAsync(entity);

        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(long id)
        {
            return await _equMaintenanceItemRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeletesAsync(long[] ids)
        {
            //var equMaintenanceItemEntity = await _equMaintenanceItemRepository.GetByIdsAsync(ids);             

            var equMaintenanceTemplateItemRelations = await _EquMaintenanceTemplateItemRelationRepository.GetEquMaintenanceTemplateItemRelationEntitiesAsync(new EquMaintenanceTemplateItemRelationQuery
            {
                MaintenanceItemIds = ids,
                SiteId = _currentSite.SiteId
            });

            if(equMaintenanceTemplateItemRelations != null&& equMaintenanceTemplateItemRelations.Any())
            {
                var equMaintenanceTemplateEntitys = await _equMaintenanceTemplateRepository.GetByIdsAsync(equMaintenanceTemplateItemRelations.Select(x=>x.MaintenanceTemplateId).ToArray());
           
                var templateCodes = equMaintenanceTemplateEntitys.Select(s=>s.Code).ToList();
 
                if (templateCodes.Any())
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES15920)).WithData("Code", string.Join(',', templateCodes));
                }
            }

            return await _equMaintenanceItemRepository.DeletesAsync(new DeleteCommand
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
        public async Task<EquMaintenanceItemDto?> QueryByIdAsync(long id)
        {
            var equSpotcheckItemEntity = await _equMaintenanceItemRepository.GetByIdAsync(id);
            if (equSpotcheckItemEntity == null) return null;
            var inteUnitEntity = await _inteUnitRepository.GetByIdAsync(equSpotcheckItemEntity.UnitId.GetValueOrDefault());

            var dto = equSpotcheckItemEntity.ToModel<EquMaintenanceItemDto>();
            dto.Unit = inteUnitEntity?.Code ?? string.Empty;
            return dto;
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<EquMaintenanceItemDto>> GetPagedListAsync(EquMaintenanceItemPagedQueryDto pagedQueryDto)
        {
            var pagedQuery = pagedQueryDto.ToQuery<EquMaintenanceItemPagedQuery>();
            pagedQuery.SiteId = _currentSite.SiteId ?? 0;
            var pagedInfo = await _equMaintenanceItemRepository.GetPagedListAsync(pagedQuery);

            // 实体到DTO转换 装载数据
            var dtos = pagedInfo.Data.Select(s => s.ToModel<EquMaintenanceItemDto>());

            var result = new PagedInfo<EquMaintenanceItemDto>(dtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);

            if (result.Data.Any())
            {
                var unitIds = result.Data.Select(m => m.UnitId.GetValueOrDefault());
                if (unitIds.Any())
                {
                    var unitEntitys = await _inteUnitRepository.GetByIdsAsync(unitIds.Distinct()!);

                    result.Data = result.Data.Select(s =>
                    {
                        var unit = unitEntitys.FirstOrDefault(f => f.Id == s.UnitId);
                        if (unit != null)
                        {
                            s.Unit = unit.Code;
                        }
                        return s;
                    });
                }

            }

            return result;
        }

    }
}
