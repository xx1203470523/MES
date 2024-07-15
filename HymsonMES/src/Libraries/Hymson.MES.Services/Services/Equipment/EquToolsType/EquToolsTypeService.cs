using Elastic.Clients.Elasticsearch;
using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Equipment;
using Hymson.MES.Core.Enums;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.MES.Data.Repositories.Equipment;
using Hymson.MES.Data.Repositories.Equipment.EquEquipmentGroup;
using Hymson.MES.Data.Repositories.Equipment.Query;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Services.Dtos.Equipment;
using Hymson.MES.Services.Dtos.Process;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;

namespace Hymson.MES.Services.Services.Equipment
{
    /// <summary>
    /// 服务（工具类型管理） 
    /// </summary>
    public class EquToolsTypeService : IEquToolsTypeService
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
        /// 仓储接口（工具类型管理）
        /// </summary>
        private readonly IEquToolsTypeRepository _equToolsTypeRepository;

        /// <summary>
        /// 工具管理表 仓储
        /// </summary>
        private readonly IEquToolingManageRepository _equToolingManageRepository;

        private readonly IEquToolsTypeEquipmentGroupRelationRepository _groupRelationRepository;
        private readonly IEquToolsTypeMaterialRelationRepository _materialRelationRepository;
        /// <summary>
        /// 仓储（设备组）
        /// </summary>
        private readonly IEquEquipmentGroupRepository _equEquipmentGroupRepository;
        private readonly IProcMaterialRepository _procMaterialRepository;

        /// <summary>
        /// 参数验证器
        /// </summary>
        private readonly AbstractValidator<EquToolsTypeSaveDto> _validationSaveRules;

        /// <summary>
        /// 构造函数
        /// </summary>
        public EquToolsTypeService(ICurrentUser currentUser, ICurrentSite currentSite,
            IEquToolsTypeRepository equToolsTypeRepository,
            IEquToolingManageRepository equToolingManageRepository,
            IEquToolsTypeEquipmentGroupRelationRepository groupRelationRepository,
            IEquToolsTypeMaterialRelationRepository materialRelationRepository,
            IEquEquipmentGroupRepository equEquipmentGroupRepository,
            IProcMaterialRepository procMaterialRepository,
            AbstractValidator<EquToolsTypeSaveDto> validationSaveRules)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _equToolsTypeRepository = equToolsTypeRepository;
            _equToolingManageRepository = equToolingManageRepository;
            _groupRelationRepository = groupRelationRepository;
            _materialRelationRepository = materialRelationRepository;
            _equEquipmentGroupRepository = equEquipmentGroupRepository;
            _procMaterialRepository = procMaterialRepository;
            _validationSaveRules = validationSaveRules;
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        public async Task<int> CreateAsync(EquToolsTypeSaveDto saveDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0) throw new CustomerValidationException(nameof(ErrorCode.MES10101));

            // 验证DTO
            await _validationSaveRules.ValidateAndThrowAsync(saveDto);

            // 更新时间
            var updatedBy = _currentUser.UserName;
            var updatedOn = HymsonClock.Now();
            var siteId = _currentSite.SiteId ?? 0;

            saveDto.Code = saveDto.Code.Trim().ToUpperInvariant();
            // 编码唯一性验证
            var checkEntity = await _equToolsTypeRepository.GetByCodeAsync(new EntityByCodeQuery
            {
                Site = siteId,
                Code = saveDto.Code
            });
            if (checkEntity != null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10521)).WithData("Code", saveDto.Code);
            }

            // DTO转换实体
            var entity = saveDto.ToEntity<EquToolsTypeEntity>();
            entity.Id = IdGenProvider.Instance.CreateId();
            entity.CreatedBy = updatedBy;
            entity.CreatedOn = updatedOn;
            entity.UpdatedBy = updatedBy;
            entity.UpdatedOn = updatedOn;
            entity.SiteId = _currentSite.SiteId ?? 0;

            //关联的设备组
            List<EquToolsTypeEquipmentGroupRelationEntity> equToolsTypeEquipments = new();

            if (saveDto.EquipmentGroupIds != null && saveDto.EquipmentGroupIds.Any())
            {
                foreach (var item in saveDto.EquipmentGroupIds)
                {
                    var equipmentGroupEntity = new EquToolsTypeEquipmentGroupRelationEntity
                    {
                        Id = IdGenProvider.Instance.CreateId(),
                        ToolTypeId = entity.Id,
                        EquipmentGroupId = item
                    };
                    equToolsTypeEquipments.Add(equipmentGroupEntity);
                }
            }

            //工具类型关联物料
            List<EquToolsTypeMaterialRelationEntity> equToolsTypeMaterials = new();
            if (saveDto.MaterialIdIds != null && saveDto.MaterialIdIds.Any())
            {
                foreach (var item in saveDto.MaterialIdIds)
                {
                    var materialIdEntity = new EquToolsTypeMaterialRelationEntity
                    {
                        Id = IdGenProvider.Instance.CreateId(),
                        ToolTypeId = entity.Id,
                        MaterialId = item
                    };
                    equToolsTypeMaterials.Add(materialIdEntity);
                }
            }

            // 保存
            var rows = 0;
            using (var trans = TransactionHelper.GetTransactionScope())
            {
                rows = await _equToolsTypeRepository.InsertAsync(entity);
                if (rows <= 0)
                {
                    trans.Dispose();
                }
                if (equToolsTypeEquipments.Any())
                {
                    await _groupRelationRepository.InsertRangeAsync(equToolsTypeEquipments);
                }
                if (equToolsTypeMaterials.Any())
                {
                    await _materialRelationRepository.InsertRangeAsync(equToolsTypeMaterials);
                }
                trans.Complete();
            }
            return rows;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        public async Task<int> ModifyAsync(EquToolsTypeSaveDto saveDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0) throw new CustomerValidationException(nameof(ErrorCode.MES10101));

            // 验证DTO
            await _validationSaveRules.ValidateAndThrowAsync(saveDto);

            var entity = await _equToolsTypeRepository.GetByIdAsync(saveDto.Id);
            if (entity == null)
            {
                throw new BusinessException(nameof(ErrorCode.MES10388));
            }

            // DTO转换实体
            entity.Name = saveDto.Name;
            entity.Status = saveDto.Status;
            entity.RatedLife = saveDto.RatedLife;
            entity.Remark = saveDto.Remark;
            entity.IsCalibrated = saveDto.IsCalibrated;
            entity.CalibrationCycle = saveDto.CalibrationCycle;
            entity.CalibrationCycleUnit = saveDto.CalibrationCycleUnit;
            entity.IsAllEquipmentUsed = saveDto.IsAllEquipmentUsed;
            entity.IsAllMaterialUsed = saveDto.IsAllMaterialUsed;
            entity.UpdatedBy = _currentUser.UserName;
            entity.UpdatedOn = HymsonClock.Now();

            //关联的设备组
            List<EquToolsTypeEquipmentGroupRelationEntity> equToolsTypeEquipments = new();

            if (saveDto.EquipmentGroupIds != null && saveDto.EquipmentGroupIds.Any())
            {
                foreach (var item in saveDto.EquipmentGroupIds)
                {
                    var equipmentGroupEntity = new EquToolsTypeEquipmentGroupRelationEntity
                    {
                        Id = IdGenProvider.Instance.CreateId(),
                        ToolTypeId = entity.Id,
                        EquipmentGroupId = item
                    };
                    equToolsTypeEquipments.Add(equipmentGroupEntity);
                }
            }

            //工具类型关联物料
            List<EquToolsTypeMaterialRelationEntity> equToolsTypeMaterials = new();
            if (saveDto.MaterialIdIds != null && saveDto.MaterialIdIds.Any())
            {
                foreach (var item in saveDto.MaterialIdIds)
                {
                    var materialIdEntity = new EquToolsTypeMaterialRelationEntity
                    {
                        Id = IdGenProvider.Instance.CreateId(),
                        ToolTypeId = entity.Id,
                        MaterialId = item
                    };
                    equToolsTypeMaterials.Add(materialIdEntity);
                }
            }

            // 保存
            var rows = 0;
            using (var trans = TransactionHelper.GetTransactionScope())
            {
                rows = await _equToolsTypeRepository.UpdateAsync(entity);
                if (rows <= 0)
                {
                    trans.Dispose();
                }

                //删除
                rows += await _groupRelationRepository.DeleteByToolTypeIdsAsync(new[] { entity.Id });
                if (equToolsTypeEquipments.Any())
                {
                    await _groupRelationRepository.InsertRangeAsync(equToolsTypeEquipments);
                }

                //删除
                rows += await _materialRelationRepository.DeleteByToolTypeIdsAsync(new[] { entity.Id });
                if (equToolsTypeMaterials.Any())
                {
                    await _materialRelationRepository.InsertRangeAsync(equToolsTypeMaterials);
                }
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
            return await _equToolsTypeRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeletesAsync(long[] ids)
        {
            if (!ids.Any()) throw new CustomerValidationException(nameof(ErrorCode.MES10213));

            var entities = await _equToolsTypeRepository.GetByIdsAsync(ids);
            if (entities != null && entities.Any(a => a.Status == DisableOrEnableEnum.Enable))
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10135));
            }

            //工具类型被工具引用不能删除
            var equToolingTypes = await _equToolingManageRepository.GetEntitiesAsync(new EquToolingManageQuery
            {
                SiteId = _currentSite.SiteId ?? 0,
                ToolTypeIds = ids
            });
            if (equToolingTypes != null && equToolingTypes.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES13516));
            }

            var rows = 0;
            var nowTime = HymsonClock.Now();
            using (var trans = TransactionHelper.GetTransactionScope())
            {
                //删主数据
                rows += await _equToolsTypeRepository.DeletesAsync(new DeleteCommand
                {
                    Ids = ids,
                    DeleteOn = nowTime,
                    UserId = _currentUser.UserName,
                });

                rows += await _groupRelationRepository.DeleteByToolTypeIdsAsync(ids);
                rows += await _materialRelationRepository.DeleteByToolTypeIdsAsync(ids);
                trans.Complete();
            }
            return rows;
        }

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<EquToolsTypeDto?> QueryByIdAsync(long id)
        {
            var equToolsTypeEntity = await _equToolsTypeRepository.GetByIdAsync(id);
            if (equToolsTypeEntity == null) return null;

            return equToolsTypeEntity.ToModel<EquToolsTypeDto>();
        }

        /// <summary>
        /// 获取关联设备组
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<EquToolsTypeCofigEquipmentGroupDto> GetEquipmentRelationAsync(long id)
        {
            var toolsTypeEntity = await _equToolsTypeRepository.GetByIdAsync(id);
            var equToolsTypeCofig = new EquToolsTypeCofigEquipmentGroupDto { IsAllEquipmentUsed = toolsTypeEntity?.IsAllEquipmentUsed ?? false };
            //查数据
            var equToolsTypes = await _groupRelationRepository.GetEntitiesAsync(new EquToolsTypeEquipmentGroupRelationQuery
            {
                ToolTypeId = id
            });

            var groupRelationDtos = new List<EquToolsTypeEquipmentGroupRelationDto>();
            if (equToolsTypes != null && equToolsTypes.Any())
            {

                foreach (var item in equToolsTypes)
                {
                    groupRelationDtos.Add(new EquToolsTypeEquipmentGroupRelationDto()
                    {
                        ToolTypeId = item.ToolTypeId,
                        EquipmentGroupId = item.EquipmentGroupId,
                    });
                }
            }
            equToolsTypeCofig.ToolsTypes = groupRelationDtos;
            return equToolsTypeCofig;
        }

        /// <summary>
        /// 获取物料
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<EquToolsTypeCofigMaterialDto> GetMaterialRelationAsync(long id)
        {
            var toolsTypeEntity = await _equToolsTypeRepository.GetByIdAsync(id);
            var equToolsTypeCofig = new EquToolsTypeCofigMaterialDto { IsAllMaterialUsed = toolsTypeEntity?.IsAllMaterialUsed ?? false };

            //查数据
            var equToolsTypes = await _materialRelationRepository.GetEntitiesAsync(new EquToolsTypeMaterialRelationQuery
            {
                ToolTypeId = id
            });

            var groupRelationDtos = new List<EquToolsTypeMaterialRelationDto>();
            if (equToolsTypes != null && equToolsTypes.Any())
            {

                foreach (var item in equToolsTypes)
                {
                    groupRelationDtos.Add(new EquToolsTypeMaterialRelationDto()
                    {
                        ToolTypeId = item.ToolTypeId,
                        MaterialId = item.MaterialId,
                    });
                }
            }

            equToolsTypeCofig.ToolsTypes = groupRelationDtos;
            return equToolsTypeCofig;
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<EquToolsTypeDto>> GetPagedListAsync(EquToolsTypePagedQueryDto pagedQueryDto)
        {
            var pagedQuery = pagedQueryDto.ToQuery<EquToolsTypePagedQuery>();
            pagedQuery.SiteId = _currentSite.SiteId ?? 0;
            var pagedInfo = await _equToolsTypeRepository.GetPagedListAsync(pagedQuery);

            // 实体到DTO转换 装载数据
            var dtos = pagedInfo.Data.Select(s => s.ToModel<EquToolsTypeDto>());
            return new PagedInfo<EquToolsTypeDto>(dtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 获取设备组
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IEnumerable<EquEquipmentGroupListDto>> GetEquipmentsAsync(long id)
        {
            var siteId = _currentSite.SiteId ?? 0;
            //查数据
            var equToolsTypes = await _groupRelationRepository.GetEntitiesAsync(new EquToolsTypeEquipmentGroupRelationQuery());
            var groupEntities = await _equEquipmentGroupRepository.GetEntitiesAsync(new Data.Repositories.Equipment.EquEquipmentGroup.Query.EquEquipmentGroupQuery
            {
                SiteId = siteId
            });
            if (id == 0)
            {
                var groupIds = equToolsTypes.Select(x => x.EquipmentGroupId).Distinct().ToArray();
                groupEntities = groupEntities.Where(x => !groupIds.Contains(x.Id));
            }
            else
            {
                var groupIds = equToolsTypes.Where(x => x.ToolTypeId != id).Select(x => x.EquipmentGroupId).Distinct().ToArray();
                groupEntities = groupEntities.Where(x => !groupIds.Contains(x.Id));
            }

            var groupListDtos = groupEntities.Select(s => s.ToModel<EquEquipmentGroupListDto>());
            return groupListDtos;
        }

        /// <summary>
        /// 获取物料列表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcMaterialDto>> GetMaterialsAsync(long id)
        {
            var siteId = _currentSite.SiteId ?? 0;
            //查数据
            var equToolsTypes = await _materialRelationRepository.GetEntitiesAsync(new EquToolsTypeMaterialRelationQuery());
            var materialEntities = await _procMaterialRepository.GetProcMaterialEntitiesAsync(new ProcMaterialQuery{SiteId = siteId});
            if (id == 0)
            {
                var materialIds = equToolsTypes.Select(x => x.MaterialId).Distinct().ToArray();
                materialEntities = materialEntities.Where(x => !materialIds.Contains(x.Id));
            }
            else
            {
                var materialIds = equToolsTypes.Where(x => x.ToolTypeId != id).Select(x => x.MaterialId).Distinct().ToArray();
                materialEntities = materialEntities.Where(x => !materialIds.Contains(x.Id));
            }

            var materialDtos = materialEntities.Select(s => s.ToModel<ProcMaterialDto>());
            return materialDtos;
        }

    }
}
