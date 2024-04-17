using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.MES.Data.Repositories.Equipment.EquEquipment;
using Hymson.MES.Data.Repositories.Equipment.EquEquipment.Query;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.Process.ProcProcessEquipmentGroupRelation.Query;
using Hymson.MES.Data.Repositories.Process.Query;
using Hymson.MES.Services.Dtos.Process;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;
using System.Linq;


namespace Hymson.MES.Services.Services.Process
{
    /// <summary>
    /// 服务（工艺设备组） 
    /// </summary>
    public class ProcProcessEquipmentGroupService : IProcProcessEquipmentGroupService
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
        private readonly AbstractValidator<ProcProcessEquipmentGroupSaveDto> _validationSaveRules;

        /// <summary>
        /// 仓储接口（工艺设备组）
        /// </summary>
        private readonly IProcProcessEquipmentGroupRepository _procProcessEquipmentGroupRepository;

        /// <summary>
        /// 仓储（设备）
        /// </summary>
        private readonly IProcProcessEquipmentGroupRelationRepository _procProcessEquipmentGroupRelationRepository;

        private readonly IEquEquipmentRepository _equipmentRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="currentSite"></param>
        /// <param name="validationSaveRules"></param>
        /// <param name="procProcessEquipmentGroupRepository"></param>
        /// <param name="procProcessEquipmentGroupRelationRepository"></param>
        /// <param name="equipmentRepository"></param>
        public ProcProcessEquipmentGroupService(ICurrentUser currentUser, ICurrentSite currentSite, AbstractValidator<ProcProcessEquipmentGroupSaveDto> validationSaveRules,
            IProcProcessEquipmentGroupRepository procProcessEquipmentGroupRepository,
            IProcProcessEquipmentGroupRelationRepository procProcessEquipmentGroupRelationRepository,
            IEquEquipmentRepository equipmentRepository)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _validationSaveRules = validationSaveRules;
            _procProcessEquipmentGroupRepository = procProcessEquipmentGroupRepository;
            _procProcessEquipmentGroupRelationRepository = procProcessEquipmentGroupRelationRepository;
            _equipmentRepository = equipmentRepository;
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        public async Task<long> CreateProcProcessEquipmentGroupAsync(ProcProcessEquipmentGroupSaveDto saveDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0) throw new CustomerValidationException(nameof(ErrorCode.MES10101));

            // 验证DTO
            await _validationSaveRules.ValidateAndThrowAsync(saveDto);
            if (saveDto.Code.Contains(' ')) throw new CustomerValidationException(nameof(ErrorCode.MES18901));
            if (saveDto.Name.ToTrimSpace() == "") throw new CustomerValidationException(nameof(ErrorCode.MES18902));
            
            // 更新时间
            var updatedBy = _currentUser.UserName;
            var updatedOn = HymsonClock.Now();

            // DTO转换实体
            var entity = saveDto.ToEntity<ProcProcessEquipmentGroupEntity>();
            entity.Id = IdGenProvider.Instance.CreateId();
            entity.CreatedBy = updatedBy;
            entity.CreatedOn = updatedOn;
            entity.UpdatedBy = updatedBy;
            entity.UpdatedOn = updatedOn;
            entity.SiteId = _currentSite.SiteId ?? 0;

            // 编码唯一性验证
            var checkEntity = await _procProcessEquipmentGroupRepository.GetByCodeAsync(new EntityByCodeQuery { Site = entity.SiteId, Code = entity.Code });
            if (checkEntity != null) throw new CustomerValidationException(nameof(ErrorCode.MES18900)).WithData("Code", entity.Code);

            var procProcessEquipmentGroupRelationEntities = new List<ProcProcessEquipmentGroupRelationEntity>();
            //验证工序+设备唯一性
            var procProcessEquipmentGroupRelations = saveDto.ToEntity<ProcProcessEquipmentGroupRelations>();
            IEnumerable<string> equipmentIds = procProcessEquipmentGroupRelations.EquipmentIDs;
            if (equipmentIds != null)
            {
                Dictionary<long, List<long>> allProcProcessEquipmentGroupRelations = new();
                var processEquGroupRelationRelationEntities = await _procProcessEquipmentGroupRelationRepository.GetEntitiesAsync(entity.SiteId);
                foreach (var processEquipmentGroupRelationEntity in processEquGroupRelationRelationEntities)
                {
                    if (allProcProcessEquipmentGroupRelations.ContainsKey(processEquipmentGroupRelationEntity.EquipmentGroupId))
                    {
                        allProcProcessEquipmentGroupRelations[processEquipmentGroupRelationEntity.EquipmentGroupId].Add(processEquipmentGroupRelationEntity.EquipmentId);
                    }
                    else
                    {
                        allProcProcessEquipmentGroupRelations[processEquipmentGroupRelationEntity.EquipmentGroupId] = new List<long>() { processEquipmentGroupRelationEntity.EquipmentId };
                    }
                }
                List<long> EquipmentGroupIds = new();//设备与当前设备组相同的所有设备组Id
                foreach (var key in allProcProcessEquipmentGroupRelations.Keys
                    .Where(x=> allProcProcessEquipmentGroupRelations[x].SequenceEqual(equipmentIds.Select(s => Convert.ToInt64(s)).ToList())))
                {
                    EquipmentGroupIds.Add(key);
                }
               
                //Insert Relation
                foreach (var item in equipmentIds)
                {
                    ProcProcessEquipmentGroupRelationEntity procProcessEquipmentGroupRelationEntity = new()
                    {
                        EquipmentGroupId = entity.Id,
                        EquipmentId = long.Parse(item),
                        SiteId = _currentSite.SiteId ?? 0,
                        Id = IdGenProvider.Instance.CreateId(),
                        CreatedBy = updatedBy,
                        CreatedOn = updatedOn,
                        UpdatedBy = updatedBy,
                        UpdatedOn = updatedOn
                    };

                    //添加
                    procProcessEquipmentGroupRelationEntities.Add(procProcessEquipmentGroupRelationEntity);
                }
            }

            // 保存
            var rows = 0;
            using (var trans = TransactionHelper.GetTransactionScope())
            {
                rows += await _procProcessEquipmentGroupRepository.InsertAsync(entity);
                rows += await _procProcessEquipmentGroupRelationRepository.InsertRangeAsync(procProcessEquipmentGroupRelationEntities);
                trans.Complete();
            }
            return entity.Id;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        public async Task<int> ModifyProcProcessEquipmentGroupAsync(ProcProcessEquipmentGroupSaveDto saveDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0) throw new CustomerValidationException(nameof(ErrorCode.MES10101));

            // 验证DTO
            await _validationSaveRules.ValidateAndThrowAsync(saveDto);
            if (saveDto.Name.ToTrimSpace() == "") throw new CustomerValidationException(nameof(ErrorCode.MES18902));

            // DTO转换实体
            var entity = saveDto.ToEntity<ProcProcessEquipmentGroupEntity>();
            entity.UpdatedBy = _currentUser.UserName;
            entity.UpdatedOn = HymsonClock.Now();
            entity.SiteId = _currentSite.SiteId ?? 0;

            //Update Relation
            // 更新时间
            var updatedBy = _currentUser.UserName;
            var updatedOn = HymsonClock.Now();
            var procProcessEquipmentGroupRelationEntities = new List<ProcProcessEquipmentGroupRelationEntity>();

            //验证工序+设备唯一性
            var procProcessEquipmentGroupRelations = saveDto.ToEntity<ProcProcessEquipmentGroupRelations>();
            IEnumerable<string> equipmentIds = procProcessEquipmentGroupRelations.EquipmentIDs;
            if (equipmentIds != null)
            {
                Dictionary<long, List<long>> allProcProcessEquipmentGroupRelations = new();
                var processEquGroupRelationRelationEntities = await _procProcessEquipmentGroupRelationRepository.GetEntitiesAsync(entity.SiteId);
                foreach (var processEquipmentGroupRelationEntity in processEquGroupRelationRelationEntities)
                {
                    if (allProcProcessEquipmentGroupRelations.ContainsKey(processEquipmentGroupRelationEntity.EquipmentGroupId))
                    {
                        allProcProcessEquipmentGroupRelations[processEquipmentGroupRelationEntity.EquipmentGroupId].Add(processEquipmentGroupRelationEntity.EquipmentId);
                    }
                    else
                    {
                        allProcProcessEquipmentGroupRelations[processEquipmentGroupRelationEntity.EquipmentGroupId] = new List<long>() { processEquipmentGroupRelationEntity.EquipmentId };
                    }
                }
                if(saveDto.Id!=null)
                    allProcProcessEquipmentGroupRelations.Remove(saveDto.Id.ParseToLong());
                List<long> EquipmentGroupIds = new();//设备与当前设备组相同的所有设备组Id
                foreach (var key in allProcProcessEquipmentGroupRelations.Keys
                    .Where(x=> allProcProcessEquipmentGroupRelations[x].SequenceEqual(equipmentIds.Select(s => Convert.ToInt64(s)).ToList())))
                {
                    EquipmentGroupIds.Add(key);
                }

                foreach (var item in equipmentIds)
                {
                    ProcProcessEquipmentGroupRelationEntity procProcessEquipmentGroupRelationEntity = new()
                    {
                        EquipmentGroupId = entity.Id,
                        EquipmentId = long.Parse(item),
                        SiteId = _currentSite.SiteId ?? 0,
                        Id = IdGenProvider.Instance.CreateId(),
                        CreatedBy = updatedBy,
                        CreatedOn = updatedOn,
                        UpdatedBy = updatedBy,
                        UpdatedOn = updatedOn
                    };

                    //添加
                    procProcessEquipmentGroupRelationEntities.Add(procProcessEquipmentGroupRelationEntity);
                }
            }
                
            IEnumerable<long> Ids = new List<long>() { saveDto.Id ?? 0 };
            DeleteCommand deleteCommand = new()
            {
                Ids = Ids,
                DeleteOn = updatedOn,
                UserId = updatedBy
            };

            // 保存
            var rows = 0;
            using (var trans = TransactionHelper.GetTransactionScope())
            {
                rows += await _procProcessEquipmentGroupRepository.UpdateAsync(entity);
                //先删除
                rows += await _procProcessEquipmentGroupRelationRepository.DeletesAsync(deleteCommand);
                //后添加
                rows += await _procProcessEquipmentGroupRelationRepository.InsertRangeAsync(procProcessEquipmentGroupRelationEntities);
                trans.Complete();
            }
            return rows;

        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> DeleteProcProcessEquipmentGroupAsync(long id)
        {
            await _procProcessEquipmentGroupRelationRepository.DeleteByProcEquIdAsync(id);
            return await _procProcessEquipmentGroupRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeletesProcProcessEquipmentGroupAsync(long[] ids)
        {
            await _procProcessEquipmentGroupRelationRepository.DeletesAsync(new DeleteCommand
            {
                Ids = ids,
                DeleteOn = HymsonClock.Now(),
                UserId = _currentUser.UserName
            });
            return await _procProcessEquipmentGroupRepository.DeletesAsync(new DeleteCommand
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
        public async Task<ProcProcessEquipmentGroupDto?> QueryProcProcessEquipmentGroupByIdAsync(long id)
        {
            ProcProcessEquipmentGroupDto dto = new();
            IEnumerable<ProcProcessEquipmentGroupRelationEntity> processEquipmentGroupRelationEntities;

            if (id == 0)
            {
                processEquipmentGroupRelationEntities = await _procProcessEquipmentGroupRelationRepository.GetByGroupIdAsync(new ProcProcessEquipmentGroupIdQuery { SiteId = _currentSite.SiteId ?? 0, ProcessEquipmentGroupId = id });
            }
            else
            {
                var procProcessEquipmentGroupEntity = await _procProcessEquipmentGroupRepository.GetByIdAsync(id);
                dto.Info = procProcessEquipmentGroupEntity.ToModel<ProcProcessEquipmentGroupListDto>();

                processEquipmentGroupRelationEntities = await _procProcessEquipmentGroupRelationRepository.GetByGroupIdAsync(new ProcProcessEquipmentGroupIdQuery { SiteId = _currentSite.SiteId ?? 0, ProcessEquipmentGroupId = id });
            }

            var equipmentEntities = await _equipmentRepository.GetEntitiesAsync(new EquEquipmentQuery
            {
                SiteId = _currentSite.SiteId ?? 0,
            });

            List<ProcProcessEquipmentBaseDto> euipments = new();
            foreach (var item in equipmentEntities)
            {
                var equipment = processEquipmentGroupRelationEntities.FirstOrDefault(f => f.EquipmentId == item.Id);
                euipments.Add(new ProcProcessEquipmentBaseDto
                {
                    Id = item.Id,
                    EquipmentGroupId = equipment == null ? 0 : equipment.EquipmentGroupId,
                    Code = item.EquipmentCode,
                    Name = item.EquipmentName
                });
            }

            dto.Equipments = euipments;
            return dto;
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ProcProcessEquipmentGroupListDto>> GetPagedListAsync(ProcProcessEquipmentGroupPagedQueryDto pagedQueryDto)
        {
            var pagedQuery = pagedQueryDto.ToQuery<ProcProcessEquipmentGroupPagedQuery>();
            pagedQuery.SiteId = _currentSite.SiteId;
            var pagedInfo = await _procProcessEquipmentGroupRepository.GetPagedInfoAsync(pagedQuery);

            // 实体到DTO转换 装载数据
            var dtos = pagedInfo.Data.Select(s => s.ToModel<ProcProcessEquipmentGroupListDto>());
            return new PagedInfo<ProcProcessEquipmentGroupListDto>(dtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

    }
}
