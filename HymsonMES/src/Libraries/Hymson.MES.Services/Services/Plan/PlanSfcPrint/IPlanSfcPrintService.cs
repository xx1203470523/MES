using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.CoreServices.Bos.Manufacture.ManuCreateBarcode;
using Hymson.MES.Services.Dtos.Manufacture.ManuMainstreamProcessDto.ManuCreateBarcodeDto;
using Hymson.MES.Services.Dtos.Plan;

namespace Hymson.MES.Services.Services.Plan
{
    /// <summary>
    /// 条码打印 service接口
    /// </summary>
    public interface IPlanSfcPrintService
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="createDto"></param>
        /// <returns></returns>
        Task CreateAsync(PlanSfcPrintCreateDto createDto);
        /// <summary>
        /// 打印
        /// </summary>
        /// <param name="createDto"></param>
        /// <returns></returns>
        Task CreatePrintAsync(PlanSfcPrintCreatePrintDto createDto);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="idsArr"></param>
        /// <returns></returns>
        Task<int> DeletesAsync(IEnumerable<long> idsArr);

        /// <summary>
        /// 分页查询列表（条码打印）
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<PlanSfcPrintDto>> GetPagedListAsync(PlanSfcPrintPagedQueryDto pagedQueryDto);

        /// <summary>
        /// 生成条码
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        Task<List<CreateBarcodeByWorkOrderOutputBo>> CreateBarcodeByWorkOrderIdAsync(CreateBarcodeByWorkOrderDto parm);

        /// <summary>
        /// 生成条码并打印
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        Task CreateBarcodeByWorkOrderIdAndPrintAsync(CreateBarcodeByWorkOrderAndPrintDto parm);

        /// <summary>
        /// 在条码下达时获取最新条码状态
        /// </summary>
        /// <param name="planSfcPrintQueryDto"></param>
        /// <returns></returns>
        Task<IEnumerable<CreateBarcodeByWorkOrderOutputBo>> GetNewBarCodeOnBarCodeCreatedAsync(PlanSfcPrintQueryDto planSfcPrintQueryDto);

        /// <summary>
        /// 物料库存打印
        /// </summary>
        /// <param name="createDto"></param>
        /// <returns></returns>
        Task WhMaterialPrintAsync(WhMaterialInventoryPrintCreatePrintDto createDto);
    }
}
