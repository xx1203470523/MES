using Hymson.MES.Services.Dtos.Manufacture;

namespace Hymson.MES.Services.Services.Manufacture
{
    /// <summary>
    /// 服务接口（生产异常处理）
    /// </summary>
    public interface IManuProductExceptionHandlingService
    {
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

    }
}
