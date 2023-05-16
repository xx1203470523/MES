using Hymson.MES.EquipmentServices.Request.BindSFC;

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
        /// <param name="bindSFCRequest"></param>
        /// <returns></returns>
        Task BindSFCAsync(BindSFCRequest bindSFCRequest);

        /// <summary>
        /// 解绑
        /// </summary>
        /// <param name="unBindSFCRequest"></param>
        /// <returns></returns>
        Task UnBindSFCAsync(UnBindSFCRequest unBindSFCRequest);
    }
}
