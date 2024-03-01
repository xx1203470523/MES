using Microsoft.AspNetCore.Http;

namespace Hymson.MES.Services.Services.Integrated
{
    /// <summary>
    /// 服务接口（基础数据导入）
    /// </summary>
    public interface IImportBasicDataService
    {
        /// <summary>
        /// 基础数据导入
        /// </summary>
        /// <param name="formFile"></param>
        /// <returns></returns>
        Task ImportDataAsync(IFormFile formFile);
    }
}