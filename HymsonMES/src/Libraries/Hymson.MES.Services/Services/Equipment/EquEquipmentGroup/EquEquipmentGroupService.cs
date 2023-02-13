using FluentValidation;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Domain.Equipment;
using Hymson.MES.Data.Repositories.Equipment.EquEquipment;
using Hymson.MES.Data.Repositories.Equipment.EquEquipmentGroup;
using Hymson.MES.Data.Repositories.Equipment.EquEquipmentGroup.Query;
using Hymson.MES.Services.Dtos.Equipment;
using Hymson.Utils;
using System.Reflection.Emit;

namespace Hymson.MES.Services.Services.EquEquipmentGroup
{
    /// <summary>
    /// 设备组 服务
    /// </summary>
    public class EquEquipmentGroupService : IEquEquipmentGroupService
    {
        /// <summary>
        /// 设备组（仓储）
        /// </summary>
        private readonly IEquEquipmentRepository _equEquipmentRepository;
        private readonly IEquEquipmentGroupRepository _equEquipmentGroupRepository;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="equEquipmentRepository"></param>
        /// <param name="equEquipmentGroupRepository"></param>
        /// <param name="validationCreateRules"></param>
        /// <param name="validationModifyRules"></param>
        public EquEquipmentGroupService(IEquEquipmentRepository equEquipmentRepository,
            IEquEquipmentGroupRepository equEquipmentGroupRepository)
        {
            _equEquipmentRepository = equEquipmentRepository;
            _equEquipmentGroupRepository = equEquipmentGroupRepository;
        }


        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="createDto"></param>
        /// <returns></returns>
        public async Task CreateEquEquipmentGroupAsync(EquEquipmentGroupCreateDto createDto)
        {
            //验证DTO
            //await _validationCreateRules.ValidateAndThrowAsync(createDto);

            //DTO转换实体
            var equEquipmentGroupEntity = createDto.ToEntity<EquEquipmentGroupEntity>();
            equEquipmentGroupEntity.CreatedBy = "TODO";
            equEquipmentGroupEntity.UpdatedBy = "TODO";

            //入库
            await _equEquipmentGroupRepository.InsertAsync(equEquipmentGroupEntity);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="modifyDto"></param>
        /// <returns></returns>
        public async Task ModifyEquEquipmentGroupAsync(EquEquipmentGroupModifyDto modifyDto)
        {
            //验证DTO
            //await _validationModifyRules.ValidateAndThrowAsync(modifyDto);

            //DTO转换实体
            var equEquipmentGroupEntity = modifyDto.ToEntity<EquEquipmentGroupEntity>();
            equEquipmentGroupEntity.UpdatedBy = "TODO";

            await _equEquipmentGroupRepository.UpdateAsync(equEquipmentGroupEntity);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteEquEquipmentGroupAsync(long id)
        {
            await _equEquipmentGroupRepository.SoftDeleteAsync(id);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="idsArr"></param>
        /// <returns></returns>
        public async Task<int> DeletesEquEquipmentGroupAsync(long[] idsArr)
        {
            return await _equEquipmentGroupRepository.SoftDeleteAsync(idsArr);
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<EquEquipmentGroupListDto>> GetPageListAsync(EquEquipmentGroupPagedQueryDto pagedQueryDto)
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
        public async Task<EquEquipmentGroupDto> GetEquEquipmentGroupWithEquipmentsAsync(EquEquipmentGroupQueryDto query)
        {
            EquEquipmentGroupDto dto = new();
            IEnumerable<EquEquipmentEntity> equipmentEntitys;

            switch (query.OperateType.ToLower())
            {
                case "add":
                    equipmentEntitys = await _equEquipmentRepository.GetByGroupIdAsync(0);
                    break;
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