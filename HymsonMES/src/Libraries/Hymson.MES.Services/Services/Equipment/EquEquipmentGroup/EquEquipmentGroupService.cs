using Hymson.Authentication;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Domain.Equipment;
using Hymson.MES.Core.Enums;
using Hymson.MES.Data.Repositories.Equipment.EquEquipment;
using Hymson.MES.Data.Repositories.Equipment.EquEquipmentGroup;
using Hymson.MES.Data.Repositories.Equipment.EquEquipmentGroup.Query;
using Hymson.MES.Services.Dtos.Equipment;
using Hymson.Snowflake;
using static Dapper.SqlMapper;

namespace Hymson.MES.Services.Services.EquEquipmentGroup
{
    /// <summary>
    /// 设备组 服务
    /// </summary>
    public class EquEquipmentGroupService : IEquEquipmentGroupService
    {
        /// <summary>
        /// 当前登录用户对象
        /// </summary>
        private readonly ICurrentUser _currentUser;

        /// <summary>
        /// 仓储（设备组）
        /// </summary>
        private readonly IEquEquipmentGroupRepository _equEquipmentGroupRepository;

        /// <summary>
        /// 仓储（设备）
        /// </summary>
        private readonly IEquEquipmentRepository _equEquipmentRepository;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="equEquipmentGroupRepository"></param>
        /// <param name="equEquipmentRepository"></param>
        public EquEquipmentGroupService(ICurrentUser currentUser,
            IEquEquipmentGroupRepository equEquipmentGroupRepository,
            IEquEquipmentRepository equEquipmentRepository)
        {
            _currentUser = currentUser;
            _equEquipmentGroupRepository = equEquipmentGroupRepository;
            _equEquipmentRepository = equEquipmentRepository;
        }


        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="createDto"></param>
        /// <returns></returns>
        public async Task<int> CreateAsync(EquEquipmentGroupCreateDto createDto)
        {
            // 验证DTO
            //await _validationCreateRules.ValidateAndThrowAsync(createDto);

            // DTO转换实体
            var entity = createDto.ToEntity<EquEquipmentGroupEntity>();
            entity.Id = IdGenProvider.Instance.CreateId();
            entity.CreatedBy = _currentUser.UserName;
            entity.UpdatedBy = _currentUser.UserName;

            // TODO 事务处理
            var rows = 0;
            rows += await _equEquipmentGroupRepository.InsertAsync(entity);
            rows += await _equEquipmentRepository.UpdateEquipmentGroupIdAsync(entity.Id, createDto.EquipmentIDs);
            return rows;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="modifyDto"></param>
        /// <returns></returns>
        public async Task<int> ModifyAsync(EquEquipmentGroupModifyDto modifyDto)
        {
            // 验证DTO
            //await _validationModifyRules.ValidateAndThrowAsync(modifyDto);

            // DTO转换实体
            var entity = modifyDto.ToEntity<EquEquipmentGroupEntity>();
            entity.UpdatedBy = _currentUser.UserName;

            // TODO 事务处理
            var rows = 0;
            rows += await _equEquipmentGroupRepository.UpdateAsync(entity);
            rows += await _equEquipmentRepository.ClearEquipmentGroupIdAsync(entity.Id);
            rows += await _equEquipmentRepository.UpdateEquipmentGroupIdAsync(entity.Id, modifyDto.EquipmentIDs);
            return rows;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(long id)
        {
            return await _equEquipmentGroupRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="idsArr"></param>
        /// <returns></returns>
        public async Task<int> DeletesAsync(long[] idsArr)
        {
            return await _equEquipmentGroupRepository.DeletesAsync(idsArr);
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<EquEquipmentGroupListDto>> GetPagedListAsync(EquEquipmentGroupPagedQueryDto pagedQueryDto)
        {
            var pagedQuery = pagedQueryDto.ToQuery<EquEquipmentGroupPagedQuery>();
            var pagedInfo = await _equEquipmentGroupRepository.GetPagedListAsync(pagedQuery);

            // 实体到DTO转换 装载数据
            var dtos = pagedInfo.Data.Select(s => s.ToModel<EquEquipmentGroupListDto>());
            return new PagedInfo<EquEquipmentGroupListDto>(dtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<EquEquipmentGroupDto> GetDetailAsync(EquEquipmentGroupQueryDto query)
        {
            EquEquipmentGroupDto dto = new();
            IEnumerable<EquEquipmentEntity> equipmentEntitys;

            switch (query.OperateType)
            {
                case OperateTypeEnum.Add:
                    equipmentEntitys = await _equEquipmentRepository.GetByGroupIdAsync(0);
                    break;
                case OperateTypeEnum.Edit:
                case OperateTypeEnum.View:
                default:
                    dto.Info = (await _equEquipmentGroupRepository.GetByIdAsync(query.Id)).ToModel<EquEquipmentGroupListDto>();
                    equipmentEntitys = await _equEquipmentRepository.GetByGroupIdAsync(query.Id);
                    break;
            }

            dto.Equipments = equipmentEntitys.Select(s => s.ToModel<EquEquipmentBaseDto>());
            return dto;
        }
    }
}