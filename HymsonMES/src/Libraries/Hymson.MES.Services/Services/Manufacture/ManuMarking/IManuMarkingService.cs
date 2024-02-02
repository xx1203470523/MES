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
    }
}
