using Hymson.MES.Services.Dtos.Manufacture;

namespace Hymson.MES.Services.Services.Manufacture
{
    /// <summary>
    /// 服务接口（生产异常处理）
    /// </summary>
    public interface IManuProductExceptionHandlingService
    {
        /// <summary>
        /// 查询条码（离脱）
        /// </summary>
        /// <param name="barCode"></param>
        /// <returns></returns>
        Task<ManuBarCodeDto> GetBarCodeAsync(string barCode);

        /// <summary>
        /// 提交（离脱）
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        Task<int> SubmitDetachmentAsync(ManuDetachmentDto requestDto);

    }
}
