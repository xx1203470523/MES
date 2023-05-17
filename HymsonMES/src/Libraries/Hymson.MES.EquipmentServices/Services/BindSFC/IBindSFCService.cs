using Hymson.MES.EquipmentServices.Dtos.BindSFC;

namespace Hymson.MES.EquipmentServices.Services.BindSFC
{
    /// <summary>
    /// 条码绑定服务
    /// </summary>
    public interface IBindSFCService
    {
        /// <summary>
        /// 绑定
        /// </summary>
        /// <param name="bindSFCDto"></param>
        /// <returns></returns>
        Task BindSFCAsync(BindSFCDto bindSFCDto);

        /// <summary>
        /// 解绑
        /// </summary>
        /// <param name="unBindSFCDto"></param>
        /// <returns></returns>
        Task UnBindSFCAsync(UnBindSFCDto unBindSFCDto);
    }
}
