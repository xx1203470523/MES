using Hymson.MES.Services.Dtos.Common;
using Hymson.MES.Services.Dtos.Quality;
using Microsoft.AspNetCore.Http;

namespace Hymson.MES.Services.Services.Quality
{
    /// <summary>
    /// 服务接口（AQL检验计划）
    /// </summary>
    public interface IQualAQLPlanService
    {
        /// <summary>
        /// 查询列表
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<QualAQLPlanExcelDto>> QueryListAsync();

        /// <summary>
        /// 下载导入模板
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        Task<string> DownloadImportTemplateAsync(Stream stream);

        /// <summary>
        /// 导入
        /// </summary>
        /// <returns></returns>
        Task ImportAsync(IFormFile formFile);

        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<ExportResponseDto> ExprotAsync(QualAQLPlanExprotRequestDto dto);

    }
}
