using Hymson.MES.Services.Dtos.Manufacture;

namespace Hymson.MES.Services.Services.Manufacture.ManuMarking
{
    /// <summary>
    /// Marking标识服务接口
    /// </summary>
    public interface IManuMarkingService
    {
        /// <summary>
        /// Marking录入校验
        /// </summary>
        /// <param name="checkDto"></param>
        /// <returns></returns>
        Task<MarkingEnterViewDto> CheckMarkingEnterAsync(ManuMarkingCheckDto checkDto);

        /// <summary>
        /// Marking关闭检索
        /// </summary>
        /// <param name="sfc"></param>
        /// <returns></returns>
        Task<IEnumerable<MarkingCloseViewDto>> GetBarcodePagedListBySFCAsync(string sfc);

        /// <summary>
        /// Marking关闭SFC校验
        /// </summary>
        /// <param name="sfc"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        Task CheckSFCMarkingCloseAsync(string sfc);

        /// <summary>
        /// Marking关闭确认提交
        /// </summary>
        /// <param name="markingCloseConfirmDto"></param>
        /// <returns></returns>
        Task SaveMarkingCloseAsync(MarkingCloseConfirmDto markingCloseConfirmDto);
    }
}
