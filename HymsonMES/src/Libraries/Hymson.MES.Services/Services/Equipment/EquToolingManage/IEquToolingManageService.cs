using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Common;
using Hymson.MES.Services.Dtos.Equipment;
using Hymson.MES.Services.Dtos.Integrated;
using Hymson.MES.Services.Dtos.Process;
using Microsoft.AspNetCore.Http;

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
        /// <param name="equToolingManageCreateDto"></param>
        /// <returns></returns>
        Task<long> AddEquToolingManageAsync(AddEquToolingManageDto equToolingManageCreateDto);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<int> DeleteEquToolingManageAsync(IEnumerable<long>  ids);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="equToolingManageModifyDto"></param>
        /// <returns></returns>
        Task ModifyEquToolingManageAsync(EquToolingManageModifyDto equToolingManageModifyDto);


        /// <summary>
        /// 下载导入模板
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
       Task<string> DownloadImportTemplateAsync(Stream stream);

        /// <summary>
        /// 导入
        /// </summary>
        /// <param name="formFile"></param>
        /// <returns></returns>
        Task ImportAsync(IFormFile formFile);

        /// <summary>
        /// 校准
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task CalibrationAsync(long id);
    }
}
