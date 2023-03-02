using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Equipment;

namespace Hymson.MES.Services.Services.Equipment.EquEquipmentUnit
{
    /// <summary>
    /// 
    /// </summary>
    public interface IEquEquipmentUnitService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="createDto"></param>
        /// <returns></returns>
        Task<int> CreateAsync(EquEquipmentUnitSaveDto createDto);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="modifyDto"></param>
        /// <returns></returns>
        Task<int> ModifyAsync(EquEquipmentUnitSaveDto modifyDto);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idsArr"></param>
        /// <returns></returns>
        Task<int> DeletesAsync(long[] idsArr);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<EquEquipmentUnitDto>> GetPagedListAsync(EquEquipmentUnitPagedQueryDto pagedQueryDto);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<EquEquipmentUnitDto> GetDetailAsync(long id);
    }
}
