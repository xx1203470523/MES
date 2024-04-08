using Hymson.MES.Services.Dtos.Manufacture;
using Microsoft.AspNetCore.Http;

namespace Hymson.MES.Services.Services.Manufacture
{
    /// <summary>
    /// 服务接口（生产异常处理）
    /// </summary>
    public interface IManuProductExceptionHandlingService
    {
        #region 设备误判
        /// <summary>
        /// 根据条码查询信息（设备误判）
        /// </summary>
        /// <param name="barCode"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuMisjudgmentBarCodeDto>> GetMisjudgmentByBarCodeAsync(string barCode);

        /// <summary>
        /// 提交（设备误判）
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        Task<int> SubmitMisjudgmentAsync(ManuMisjudgmentDto requestDto);
        #endregion


        #region 返工
        /// <summary>
        /// 根据条码查询信息（返工）
        /// </summary>
        /// <param name="barCode"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuReworkBarCodeDto>> GetReworkByBarCodeAsync(string barCode);

        /// <summary>
        /// 根据托盘码条码查询信息（返工）
        /// </summary>
        /// <param name="palletCode"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuReworkBarCodeDto>> GetReworkByPalletCodeAsync(string palletCode);

        /// <summary>
        /// 提交（返工）
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        Task<int> SubmitReworkAsync(ManuReworkDto requestDto);

        /// <summary>
        /// 下载导入模板（返工）
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        Task<string> DownloadImportTemplateAsync(Stream stream);

        /// <summary>
        /// 导入（返工）
        /// </summary>
        /// <returns></returns>
        Task ImportAsync(IFormFile formFile);
        #endregion


        #region 离脱
        /// <summary>
        /// 根据条码查询信息（离脱）
        /// </summary>
        /// <param name="barCode"></param>
        /// <returns></returns>
        Task<ManuDetachmentBarCodeDto> GetDetachmentByBarCodeAsync(string barCode);

        /// <summary>
        /// 提交（离脱）
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        Task<int> SubmitDetachmentAsync(ManuDetachmentDto requestDto);
        #endregion

    }
}
