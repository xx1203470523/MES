using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Manufacture;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Services.Services.Manufacture
{
    /// <summary>
    /// 服务接口（Marking信息表）
    /// </summary>
    public interface IManuSfcMarkingService
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        Task CreateAsync(IEnumerable<ManuSfcMarkingSaveDto> saveDto);

        /// <summary>
        /// 批量导入
        /// </summary>
        /// <param name="formFile"></param>
        /// <returns></returns>
        Task ImportAsync([FromForm(Name = "file")] IFormFile formFile);

        /// <summary>
        /// 获取打开状态Marking信息
        /// </summary>
        /// <param name="sfc"></param>
        /// <returns></returns>
        Task<IEnumerable<MarkingInfoDto>> GetOpenMarkingListBySFCAsync(string sfc);

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ManuSfcMarkingDto?> QueryByIdAsync(long id);

        /// <summary>
        /// 获取分页List
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<ManuSfcMarkingDto>> GetPagedListAsync(ManuSfcMarkingPagedQueryDto pagedQueryDto);

    }
}