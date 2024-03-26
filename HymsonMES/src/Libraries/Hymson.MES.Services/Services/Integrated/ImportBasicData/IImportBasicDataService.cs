using Microsoft.AspNetCore.Http;

namespace Hymson.MES.Services.Services.Integrated
{
    /// <summary>
    /// 服务接口（基础数据导入）
    /// </summary>
    public interface IImportBasicDataService
    {
        /// <summary>
        /// 设备数据导入
        /// </summary>
        /// <param name="formFile"></param>
        /// <returns></returns>
        Task ImportEquDataAsync(IFormFile formFile);

        /// <summary>
        /// 资源数据导入
        /// </summary>
        /// <returns></returns>
        Task ImportResourceDataAsync(IFormFile formFile);

        /// <summary>
        /// 资源类型数据导入
        /// </summary>
        /// <returns></returns>
        Task ImportResourceTypeDataAsync(IFormFile formFile);

        /// <summary>
        /// 工序数据导入
        /// </summary>
        /// <returns></returns>
        Task ImportProcedureDataAsync(IFormFile formFile);

        /// <summary>
        /// 产线数据导入
        /// </summary>
        /// <returns></returns>
        Task ImportWorkLineDataAsync(IFormFile formFile);

        /// <summary>
        /// 车间数据导入
        /// </summary>
        /// <returns></returns>
        Task ImportWorkShopDataAsync(IFormFile formFile);

        /// <summary>
        /// 物料组数据导入
        /// </summary>
        /// <returns></returns>
        Task ImportMaterialGroupDataAsync(IFormFile formFile);
    }
}