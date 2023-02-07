using FluentValidation;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Domain.Equipment;
using Hymson.MES.Data.Repositories.Equipment.EquipmentUnit;
using Hymson.MES.Data.Repositories.Equipment.EquipmentUnit.Query;
using Hymson.MES.Services.Dtos.Equipment.EquipmentUnit;
using Hymson.Snowflake;

namespace Hymson.MES.Services.Services.Equipment.EquipmentUnit
{
    /// <summary>
    /// 
    /// </summary>
    public class EquipmentUnitService : IEquipmentUnitService
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly IEquipmentUnitRepository _equipmentUnitRepository;
        private readonly AbstractValidator<EquipmentUnitCreateDto> _validationCreateRules;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="equipmentUnitRepository"></param>
        /// <param name="validationRules"></param>
        public EquipmentUnitService(IEquipmentUnitRepository equipmentUnitRepository, AbstractValidator<EquipmentUnitCreateDto> validationRules)
        {
            _equipmentUnitRepository = equipmentUnitRepository;
            _validationCreateRules = validationRules;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="createDto"></param>
        /// <returns></returns>
        public async Task<int> CreateEquipmentUnitAsync(EquipmentUnitCreateDto createDto)
        {
            // 验证DTO
            await _validationCreateRules.ValidateAndThrowAsync(createDto);

            // DTO转换实体
            var entity = createDto.ToEntity<EquipmentUnitEntity>();
            entity.Id = IdGenProvider.Instance.CreateId();
            entity.CreatedBy = "TODO";
            entity.UpdatedBy = "TODO";

            // 保存实体
            return await _equipmentUnitRepository.InsertAsync(entity);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="modifyDto"></param>
        /// <returns></returns>
        public async Task<int> ModifyEquipmentUnitAsync(EquipmentUnitModifyDto modifyDto)
        {
            // DTO转换实体
            var entity = modifyDto.ToEntity<EquipmentUnitEntity>();
            entity.UpdatedBy = "TODO";

            // 保存实体
            return await _equipmentUnitRepository.UpdateAsync(entity);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idsArr"></param>
        /// <returns></returns>
        public async Task<int> DeleteEquipmentUnitAsync(long[] idsArr)
        {
            return await _equipmentUnitRepository.DeleteAsync(idsArr);
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<EquipmentUnitDto>> GetListAsync(EquipmentUnitPagedQueryDto pagedQueryDto)
        {
            var pagedQuery = pagedQueryDto.ToQuery<EquipmentUnitPagedQuery>();
            var pagedInfo = await _equipmentUnitRepository.GetPagedInfoAsync(pagedQuery);

            // 实体到DTO转换 装载数据
            List<EquipmentUnitDto> equipmentUnitDtos = PrepareEquipmentUnitDtos(pagedInfo);
            return new PagedInfo<EquipmentUnitDto>(equipmentUnitDtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pagedInfo"></param>
        /// <returns></returns>
        private static List<EquipmentUnitDto> PrepareEquipmentUnitDtos(PagedInfo<EquipmentUnitEntity> pagedInfo)
        {
            var dtos = new List<EquipmentUnitDto>();
            foreach (var entity in pagedInfo.Data)
            {
                var dto = entity.ToModel<EquipmentUnitDto>();
                dtos.Add(dto);
            }

            return dtos;
        }
    }
}
