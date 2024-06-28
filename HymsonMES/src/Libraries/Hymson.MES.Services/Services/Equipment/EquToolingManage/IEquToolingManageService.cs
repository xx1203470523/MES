using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Common;
using Hymson.MES.Services.Dtos.Equipment;
using Hymson.MES.Services.Dtos.Integrated;
using Hymson.MES.Services.Dtos.Process;

namespace Hymson.MES.Services.Services.Equipment.EquToolingManage
{
    /// <summary>
    /// 工具管理表 service接口
    /// </summary>
    public interface IEquToolingManageService
    {
        /// <summary>
        /// 获取分页List
        /// </summary>
        /// <param name="equToolingManagePagedQueryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<EquToolingManageViewDto>> GetPageListAsync(EquToolingManagePagedQueryDto equToolingManagePagedQueryDto);


        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<EquToolingManageViewDto> QueryEquToolingManageByIdAsync(long id);

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="procEquToolingManageCreateDto"></param>
        /// <returns></returns>
        Task<long> AddEquToolingManageAsync(AddEquToolingManageDto equToolingManageCreateDto);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<int> DeleteEquToolingManageAsync(long[] ids);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="procEquToolingManageModifyDto"></param>
        /// <returns></returns>
        Task ModifyEquToolingManageAsync(EquToolingManageModifyDto equToolingManageModifyDto);
    }
}
