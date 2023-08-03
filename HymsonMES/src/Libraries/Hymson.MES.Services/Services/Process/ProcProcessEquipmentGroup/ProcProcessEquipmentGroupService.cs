using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Equipment.EquEquipment;
using Hymson.MES.Data.Repositories.Equipment.EquEquipment.Query;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.Process.ProcProcessEquipmentGroupRelation.Query;
using Hymson.MES.Data.Repositories.Process.Query;
using Hymson.MES.Services.Dtos.Process;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;

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
        public async Task<int> CreateProcProcessEquipmentGroupAsync(ProcProcessEquipmentGroupSaveDto saveDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0) throw new ValidationException(nameof(ErrorCode.MES10101));

            // 验证DTO
            await _validationSaveRules.ValidateAndThrowAsync(saveDto);

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

            //Insert Relation
            var procProcessEquipmentGroupRelations = saveDto.ToEntity<ProcProcessEquipmentGroupRelations>();
            IEnumerable<string> equipmentIds = procProcessEquipmentGroupRelations.EquipmentIDs;
            var procProcessEquipmentGroupRelationEntities = new List<ProcProcessEquipmentGroupRelationEntity>();
            foreach (var item in equipmentIds)
            {
                ProcProcessEquipmentGroupRelationEntity procProcessEquipmentGroupRelationEntity = new ProcProcessEquipmentGroupRelationEntity();
                procProcessEquipmentGroupRelationEntity.EquipmentGroupId = entity.Id;
                procProcessEquipmentGroupRelationEntity.EquipmentId = long.Parse(item);
                procProcessEquipmentGroupRelationEntity.SiteId = _currentSite.SiteId ?? 0;
                procProcessEquipmentGroupRelationEntity.Id = IdGenProvider.Instance.CreateId();
                procProcessEquipmentGroupRelationEntity.CreatedBy = updatedBy;
                procProcessEquipmentGroupRelationEntity.CreatedOn = updatedOn;
                procProcessEquipmentGroupRelationEntity.UpdatedBy = updatedBy;
                procProcessEquipmentGroupRelationEntity.UpdatedOn = updatedOn;

                //添加
                procProcessEquipmentGroupRelationEntities.Add(procProcessEquipmentGroupRelationEntity);
            }

            // 保存
            var rows = 0;
            using (var trans = TransactionHelper.GetTransactionScope())
            {
                rows += await _procProcessEquipmentGroupRepository.InsertAsync(entity);
                rows += await _procProcessEquipmentGroupRelationRepository.InsertRangeAsync(procProcessEquipmentGroupRelationEntities);
                trans.Complete();
            }
            return rows;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        public async Task<int> ModifyProcProcessEquipmentGroupAsync(ProcProcessEquipmentGroupSaveDto saveDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0) throw new ValidationException(nameof(ErrorCode.MES10101));

            // 验证DTO
            await _validationSaveRules.ValidateAndThrowAsync(saveDto);

            // DTO转换实体
            var entity = saveDto.ToEntity<ProcProcessEquipmentGroupEntity>();
            entity.UpdatedBy = _currentUser.UserName;
            entity.UpdatedOn = HymsonClock.Now();
            entity.SiteId = _currentSite.SiteId ?? 0;

            //Update Relation
            // 更新时间
            var updatedBy = _currentUser.UserName;
            var updatedOn = HymsonClock.Now();
            var procProcessEquipmentGroupRelations = saveDto.ToEntity<ProcProcessEquipmentGroupRelations>();
            var procProcessEquipmentGroupRelationEntities = new List<ProcProcessEquipmentGroupRelationEntity>();
            IEnumerable<string> equipmentIds = procProcessEquipmentGroupRelations.EquipmentIDs;
            foreach (var item in equipmentIds)
            {
                ProcProcessEquipmentGroupRelationEntity procProcessEquipmentGroupRelationEntity = new ProcProcessEquipmentGroupRelationEntity();
                procProcessEquipmentGroupRelationEntity.EquipmentGroupId = entity.Id;
                procProcessEquipmentGroupRelationEntity.EquipmentId = long.Parse(item);
                procProcessEquipmentGroupRelationEntity.SiteId = _currentSite.SiteId ?? 0;
                procProcessEquipmentGroupRelationEntity.Id = IdGenProvider.Instance.CreateId();
                procProcessEquipmentGroupRelationEntity.CreatedBy = updatedBy;
                procProcessEquipmentGroupRelationEntity.CreatedOn = updatedOn;
                procProcessEquipmentGroupRelationEntity.UpdatedBy = updatedBy;
                procProcessEquipmentGroupRelationEntity.UpdatedOn = updatedOn;

                //添加
                procProcessEquipmentGroupRelationEntities.Add(procProcessEquipmentGroupRelationEntity);
            }
            IEnumerable<long> Ids=new List<long>() { saveDto.Id??0 };
            DeleteCommand deleteCommand = new DeleteCommand()
            {
                Ids=Ids,
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
            IEnumerable<ProcProcessEquipmentGroupRelationEntity> processEquipmentEntitys;

            if (id == 0)
            {
                processEquipmentEntitys = await _procProcessEquipmentGroupRelationRepository.GetByGroupIdAsync(new ProcProcessEquipmentGroupIdQuery { SiteId = _currentSite.SiteId ?? 0, ProcessEquipmentGroupId = id });
            }
            else
            {
                dto.Info = (await _procProcessEquipmentGroupRepository.GetByIdAsync(id)).ToModel<ProcProcessEquipmentGroupListDto>();
                processEquipmentEntitys = await _procProcessEquipmentGroupRelationRepository.GetByGroupIdAsync(new ProcProcessEquipmentGroupIdQuery { SiteId = _currentSite.SiteId ?? 0, ProcessEquipmentGroupId = id });

            }

            var equipmentIds = processEquipmentEntitys.Select(s => s.EquipmentId);
            var equipmentEntities = await _equipmentRepository.GetEntitiesAsync(new EquEquipmentQuery
            {
                SiteId = _currentSite.SiteId ?? 0,
            });

            List<ProcProcessEquipmentBaseDto> euipments = new();
            foreach (var item in processEquipmentEntitys)
            {
                var equipment = equipmentEntities.FirstOrDefault(f => f.Id == item.EquipmentId);
                if (equipment == null) continue;

                euipments.Add(new ProcProcessEquipmentBaseDto
                {
                    Id = item.EquipmentId,
                    EquipmentGroupId = item.EquipmentGroupId,
                    Code = equipment.EquipmentCode,
                    Name = equipment.EquipmentName
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
