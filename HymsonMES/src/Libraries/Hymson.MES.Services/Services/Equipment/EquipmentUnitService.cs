using FluentValidation;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Domain.Equipment;
using Hymson.MES.Data.Repositories.Equipment.IEquipment;
using Hymson.MES.Data.Repositories.Equipment.Query;
using Hymson.MES.Services.Dtos.Equipment;

namespace Hymson.MES.Services.Services.Equipment
{
    /// <summary>
    /// 
    /// </summary>
    public class EquipmentUnitService : IEquipmentUnitService
    {
        private readonly IEquipmentUnitRepository _equipmentUnitRepository;
        private readonly AbstractValidator<EquipmentUnitDto> _validationRules;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="whStockChangeRecordRepository"></param>
        /// <param name="validationRules"></param>
        public EquipmentUnitService(IEquipmentUnitRepository whStockChangeRecordRepository, AbstractValidator<EquipmentUnitDto> validationRules)
        {
            _equipmentUnitRepository = whStockChangeRecordRepository;
            _validationRules = validationRules;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="equipmentUnitDto"></param>
        /// <returns></returns>
        public async Task CreateEquipmentUnitAsync(EquipmentUnitDto equipmentUnitDto)
        {
            // 验证DTO
            await _validationRules.ValidateAndThrowAsync(equipmentUnitDto);
            // DTO转换实体
            var whStockChangeRecordEntity = equipmentUnitDto.ToEntity<EquipmentUnitEntity>();
            whStockChangeRecordEntity.CreateBy = "jinyi";
            whStockChangeRecordEntity.UpdateBy = "jinyi";
            // 入库
            await _equipmentUnitRepository.InsertAsync(whStockChangeRecordEntity);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="equipmentUnitDto"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task ModifyEquipmentUnitAsync(EquipmentUnitDto equipmentUnitDto)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task DeleteEquipmentUnitAsync(long id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="equipmentUnitPagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<EquipmentUnitDto>> GetListAsync(EquipmentUnitPagedQueryDto equipmentUnitPagedQueryDto)
        {
            var whStockChangeRecordPagedQuery = equipmentUnitPagedQueryDto.ToQuery<EquipmentUnitPagedQuery>();
            var pagedInfo = await _equipmentUnitRepository.GetPagedInfoAsync(whStockChangeRecordPagedQuery);

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
            var whStockChangeRecordDtos = new List<EquipmentUnitDto>();
            foreach (var whStockChangeRecordEntity in pagedInfo.Data)
            {
                var whStockChangeRecordDto = whStockChangeRecordEntity.ToModel<EquipmentUnitDto>();
                whStockChangeRecordDtos.Add(whStockChangeRecordDto);
            }

            return whStockChangeRecordDtos;
        }
    }
}
