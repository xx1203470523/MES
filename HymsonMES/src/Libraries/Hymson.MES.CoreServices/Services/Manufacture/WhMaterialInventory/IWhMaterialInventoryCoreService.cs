using Hymson.MES.CoreServices.Bos.Manufacture;

namespace Hymson.MES.CoreServices.Services.Manufacture.WhMaterialInventory
{
    /// <summary>
    /// 车间库存接收接口
    /// </summary>
    public interface IWhMaterialInventoryCoreService
    {
        /// <summary>
        /// 车间库存接收
        /// </summary>
        /// <param name="bo"></param>
        /// <returns></returns>
        Task<int> MaterialInventoryAsync(MaterialInventoryBo bo);
    }
}
