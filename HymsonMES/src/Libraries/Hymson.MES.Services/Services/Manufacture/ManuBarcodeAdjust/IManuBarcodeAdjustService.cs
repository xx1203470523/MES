using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Manufacture;

namespace Hymson.MES.Services.Services.Manufacture
{
    /// <summary>
    /// 烘烤工序 service接口
    /// </summary>
    public interface IManuBarcodeAdjustService
    {
        /// <summary>
        /// 分页获取到条码
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<ManuSfcAboutInfoViewDto>> GetBarcodePagedListAsync(ManuSfcAboutInfoPagedQueryDto queryDto);

        /// <summary>
        /// 调整条码数量的验证
        /// </summary>
        /// <param name="sfcs"></param>
        /// <returns></returns>
        Task<bool> QtyAdjustVerifySfcAsync(string[] sfcs);

        /// <summary>
        /// 合并条码的验证
        /// </summary>
        /// <param name="sfcs"></param>
        /// <returns></returns>
        Task<bool> MergeAdjustVerifySfcAsync(string[] sfcs);

        /// <summary>
        /// 合并的获取条码数据
        /// </summary>
        /// <param name="sfc"></param>
        /// <returns></returns>
        Task<ManuSfcAboutInfoViewDto?> GetSfcAboutInfoBySfcInMergeAsync(string sfc);

        /// <summary>
        /// 数量调整的获取条码数据
        /// </summary>
        /// <param name="sfc"></param>
        /// <returns></returns>
        Task<ManuSfcAboutInfoViewDto?> GetSfcAboutInfoBySfcInQtyAsync(string sfc);

        /// <summary>
        /// 获取条码数据(Marking拦截)
        /// </summary>
        /// <param name="sfc"></param>
        /// <returns></returns>
        Task<ManuSfcAboutInfoViewDto?> GetSfcAboutInfoByMarkingSfcAsync(string sfc);

        /// <summary>
        /// 条码拆分
        /// </summary>
        /// <param name="splitDto"></param>
        /// <returns></returns>
        Task<string> BarCodeSplitAsync(ManuBarCodeSplitRequestDto splitDto);

        /// <summary>
        /// 合并条码
        /// </summary>
        /// <param name="mergeDto"></param>
        /// <returns></returns>
        Task<string> BarCodeMergeAsync(ManuBarCodeMergeRequestDto mergeDto);

        /// <summary>
        /// 更改条码数量
        /// </summary>
        /// <param name="adjustDto"></param>
        /// <returns></returns>
        Task BarcodeQtyAdjustAsync(ManuBarcodeQtyAdjustDto adjustDto);

    }
}
