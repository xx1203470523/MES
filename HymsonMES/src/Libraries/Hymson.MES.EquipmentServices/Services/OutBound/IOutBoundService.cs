using Hymson.MES.EquipmentServices.Dtos.OutBound;

namespace Hymson.MES.EquipmentServices.Services.OutBound
{
    /// <summary>
    /// 出站
    /// </summary>
    public interface IOutBoundService
    {
        /// <summary>
        /// 出站
        /// </summary>
        /// <param name="outBoundDto"></param>
        /// <returns></returns>
        Task OutBound(OutBoundDto outBoundDto);

        /// <summary>
        /// 进站（多个）
        /// </summary>
        /// <param name="outBoundMoreDto"></param>
        /// <returns></returns>
        Task OutBoundMore(OutBoundMoreDto outBoundMoreDto);
    }
}
