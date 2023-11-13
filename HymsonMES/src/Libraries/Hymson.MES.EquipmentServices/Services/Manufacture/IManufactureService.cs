using Hymson.MES.EquipmentServices.Dtos;

namespace Hymson.MES.EquipmentServices.Services.Manufacture
{
    /// <summary>
    /// 生产服务接口
    /// </summary>
    public interface IManufactureService
    {
        /// <summary>
        /// 创建条码
        /// </summary>
        /// <param name="baseDto"></param>
        /// <returns></returns>
        Task<IEnumerable<string>> CreateBarcodeBySemiProductIdAsync(BaseDto baseDto);

        /// <summary>
        /// 进站
        /// </summary>
        /// <param name="inBoundDto"></param>
        /// <returns></returns>
        Task InBoundAsync(InBoundDto inBoundDto);

        /// <summary>
        /// 进站（多个）
        /// </summary>
        /// <param name="inBoundMoreDto"></param>
        /// <returns></returns>
        Task InBoundMoreAsync(InBoundMoreDto inBoundMoreDto);

        /// <summary>
        /// 出站
        /// </summary>
        /// <param name="outBoundDto"></param>
        /// <returns></returns>
        Task OutBoundAsync(OutBoundDto outBoundDto);

        /// <summary>
        /// 出站（多个）
        /// </summary>
        /// <param name="outBoundMoreDto"></param>
        /// <returns></returns>
        Task OutBoundMoreAsync(OutBoundMoreDto outBoundMoreDto);

    }
}