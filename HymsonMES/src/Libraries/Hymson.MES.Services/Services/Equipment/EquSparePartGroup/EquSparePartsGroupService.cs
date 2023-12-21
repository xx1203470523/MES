using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Equipment;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.MES.Data.Repositories.Equipment;
using Hymson.MES.Data.Repositories.Equipment.Query;
using Hymson.MES.Services.Dtos.Equipment;
using Hymson.MES.Services.Dtos.Quality;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;
using System.Diagnostics.Tracing;

namespace Hymson.MES.Services.Services.Equipment
{
    /// <summary>
    /// 服务（备件类型） 
    /// </summary>
    public class EquSparePartsGroupService : IEquSparePartsGroupService
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
        private readonly AbstractValidator<EquSparePartsGroupSaveDto> _validationSaveRules;

        /// <summary>
        /// 仓储接口（备件类型）
        /// </summary>
        private readonly IEquSparePartsGroupRepository _equSparePartsGroupRepository;
        private readonly IEquSparePartsRepository _equSparePartsRepository;
        private readonly IEquSparePartsGroupEquipmentGroupRelationRepository _equSparePartsGroupEquipmentGroupRelationRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="currentSite"></param>
        /// <param name="validationSaveRules"></param>
        /// <param name="equSparePartsGroupRepository"></param>
        /// <param name="equSparePartsGroupEquipmentGroupRelationRepository"></param>
        /// <param name="equSparePartsRepository"></param>
        public EquSparePartsGroupService(ICurrentUser currentUser, ICurrentSite currentSite, AbstractValidator<EquSparePartsGroupSaveDto> validationSaveRules, 
            IEquSparePartsGroupRepository equSparePartsGroupRepository,
            IEquSparePartsGroupEquipmentGroupRelationRepository equSparePartsGroupEquipmentGroupRelationRepository, IEquSparePartsRepository equSparePartsRepository)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _validationSaveRules = validationSaveRules;
            _equSparePartsGroupRepository = equSparePartsGroupRepository;
            _equSparePartsGroupEquipmentGroupRelationRepository = equSparePartsGroupEquipmentGroupRelationRepository;
            _equSparePartsRepository = equSparePartsRepository;
        }


        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        public async Task<int> CreateEquSparePartsGroupAsync(EquSparePartsGroupSaveDto saveDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0) throw new ValidationException(nameof(ErrorCode.MES10101));

            // 验证DTO
            await _validationSaveRules.ValidateAndThrowAsync(saveDto);

            // 更新时间
            var updatedBy = _currentUser.UserName;
            var updatedOn = HymsonClock.Now();

            // DTO转换实体
            var entity = saveDto.ToEntity<EquSparePartsGroupEntity>();
            entity.Id = IdGenProvider.Instance.CreateId();
            entity.CreatedBy = updatedBy;
            entity.CreatedOn = updatedOn;
            entity.UpdatedBy = updatedBy;
            entity.UpdatedOn = updatedOn;
            entity.SiteId = _currentSite.SiteId ?? 0;
            // 编码唯一性验证
            var checkEntity = await _equSparePartsGroupRepository.GetByCodeAsync(new EntityByCodeQuery
            {
                Site = entity.SiteId,
                Code = entity.Code
            });
            if (checkEntity != null) throw new CustomerValidationException(nameof(ErrorCode.MES10521)).WithData("Code", entity.Code);

            //关联的设备组
            List<EquSparePartsGroupEquipmentGroupRelationEntity> list = new();

            if (saveDto.EquipmentGroupIds != null)
                foreach (var item in saveDto.EquipmentGroupIds)
                {
                    var equipmentGroupEntity = new EquSparePartsGroupEquipmentGroupRelationEntity
                    {
                        Id =IdGenProvider.Instance.CreateId(),
                        SiteId = _currentSite.SiteId ?? 0,
                        SparePartsGroupId = entity.Id,
                        EquipmentGroupId = item,
                        CreatedBy = updatedBy,
                        CreatedOn = updatedOn,
                        UpdatedBy = updatedBy,
                        UpdatedOn = updatedOn
                    };
                    list.Add(equipmentGroupEntity);
                }
                //关联备件
                var updatedTypeEntity = new UpdateSparePartsTypeEntity
                {
                    Id = entity.Id,
                    SparePartIds = saveDto.SparePartIds,
                    UpdatedBy= updatedBy,
                    UpdatedOn= updatedOn
                };
            //保存
            var rows = 0;
            using (var trans = TransactionHelper.GetTransactionScope())
            {
                rows = await _equSparePartsGroupRepository.InsertAsync(entity);
                if (rows <= 0)
                {
                    trans.Dispose();
                }
                else
                {
                    var rowArray = await Task.WhenAll(new List<Task<int>>() {
                        _equSparePartsGroupEquipmentGroupRelationRepository.InsertRangeAsync(list),
                        _equSparePartsRepository.UpdateTypeAsync(updatedTypeEntity),

                    });
                    rows += rowArray.Sum();
                    trans.Complete();
                }
            }
            return rows;

        }

        /// <summary>
        /// 获取设备组
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<List<EquSparePartsGroupEquipmentGroupRelationSaveDto>> GetSparePartsEquipmentGroupRelationByIdAsync(long id)
        {
            var unqualifiedCodeGroupRelations = await _equSparePartsGroupEquipmentGroupRelationRepository.GetSparePartsEquipmentGroupRelationAsync(id);
            var nqualifiedCodeGroupRelationList = new List<EquSparePartsGroupEquipmentGroupRelationSaveDto>();
            if (unqualifiedCodeGroupRelations != null && unqualifiedCodeGroupRelations.Any())
            {

                foreach (var item in unqualifiedCodeGroupRelations)
                {
                    nqualifiedCodeGroupRelationList.Add(new EquSparePartsGroupEquipmentGroupRelationSaveDto()
                    {
                        Id = item.Id,
                        EquipmentGroupId= item.EquipmentGroupId,
                        SparePartsGroupId= item.SparePartsGroupId,
                        EquipmentGroupCode=item.EquipmentGroupCode,
                        EquipmentGroupName=item.EquipmentGroupName,
                        CreatedBy = item.CreatedBy,
                        CreatedOn = item.CreatedOn
                    });
                }
            }
            return nqualifiedCodeGroupRelationList;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        public async Task<int> ModifyEquSparePartsGroupAsync(EquSparePartsGroupSaveDto saveDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0) throw new ValidationException(nameof(ErrorCode.MES10101));

             // 验证DTO
            await _validationSaveRules.ValidateAndThrowAsync(saveDto);

            // DTO转换实体
            var entity = saveDto.ToEntity<EquSparePartsGroupEntity>();
            entity.UpdatedBy = _currentUser.UserName;
            entity.UpdatedOn = HymsonClock.Now();

            //获取关联设备
            List<EquSparePartsGroupEquipmentGroupRelationEntity> list = new();

            if (saveDto.EquipmentGroupIds != null)
                foreach (var item in saveDto.EquipmentGroupIds)
                {
                    var equipmentGroupEntity = new EquSparePartsGroupEquipmentGroupRelationEntity
                    {
                        Id = IdGenProvider.Instance.CreateId(),
                        SiteId = _currentSite.SiteId ?? 0,
                        SparePartsGroupId = entity.Id,
                        EquipmentGroupId = item,
                        CreatedBy = _currentUser.UserName,
                        CreatedOn = HymsonClock.Now()
                };
                    list.Add(equipmentGroupEntity);
                }

            var command = new DeleteByParentIdCommand
            {
                ParentId = entity.Id,
                UpdatedBy = _currentUser.UserName,
                UpdatedOn = HymsonClock.Now()
            };

            //关联备件
            var updatedTypeEntity = new UpdateSparePartsTypeEntity
            {
                Id = entity.Id,
                SparePartIds = saveDto.SparePartIds,
                UpdatedBy = _currentUser.UserName,
                UpdatedOn = HymsonClock.Now()
        };

            //保存
            var rows = 0;
            using (var trans = TransactionHelper.GetTransactionScope())
            {
                rows = await _equSparePartsGroupEquipmentGroupRelationRepository.DeleteByParentIdAsync(command);
                var a = await _equSparePartsRepository.CleanTypeAsync(updatedTypeEntity);


                if (rows <= 0)
                {
                    trans.Dispose();
                }
                else
                {
                    var rowArray = await Task.WhenAll(new List<Task<int>>() {
                    _equSparePartsGroupRepository.UpdateAsync(entity),
                    _equSparePartsGroupEquipmentGroupRelationRepository.InsertRangeAsync(list),
                    _equSparePartsRepository.UpdateTypeAsync(updatedTypeEntity),

                    });
                    rows += rowArray.Sum();
                    trans.Complete();
                }
            }
            return rows;


        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> DeleteEquSparePartsGroupAsync(long id)
        {
            return await _equSparePartsGroupRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeletesEquSparePartsGroupAsync(long[] ids)
        {
            return await _equSparePartsGroupRepository.DeletesAsync(new DeleteCommand
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
        public async Task<EquSparePartsGroupDto?> QueryEquSparePartsGroupByIdAsync(long id) 
        {
           var equSparePartsGroupEntity = await _equSparePartsGroupRepository.GetByIdAsync(id);
           if (equSparePartsGroupEntity == null) return null;
           
           return equSparePartsGroupEntity.ToModel<EquSparePartsGroupDto>();
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<EquSparePartsGroupDto>> GetPagedListAsync(EquSparePartsGroupPagedQueryDto pagedQueryDto)
        {
            var pagedQuery = pagedQueryDto.ToQuery<EquSparePartsGroupPagedQuery>();
            pagedQuery.SiteId = _currentSite.SiteId ?? 0;
            var pagedInfo = await _equSparePartsGroupRepository.GetPagedInfoAsync(pagedQuery);

            // 实体到DTO转换 装载数据
            var dtos = pagedInfo.Data.Select(s => s.ToModel<EquSparePartsGroupDto>());
            return new PagedInfo<EquSparePartsGroupDto>(dtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

    }
}
