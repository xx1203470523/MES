using Hymson.MES.EquipmentServices.Request.UnBindSFC;

namespace Hymson.MES.EquipmentServices.Services.BindSFC
{
    /// <summary>
    /// 条码解绑服务
    /// </summary>
    public interface IUnBindSFCService
    {
        /// <summary>
        /// 解绑
        /// </summary>
        /// <param name="unBindSFCRequest"></param>
        /// <returns></returns>
        Task UnBindSFCAsync(UnBindSFCRequest unBindSFCRequest);
    }
}
