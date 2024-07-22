using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Services.Dtos.Warehouse;

namespace Hymson.MES.Services.Services.Warehouse
{
    /// <summary>
    /// 物料库存 service接口
    /// </summary>
    public interface IWhMaterialInventoryService
    {
        /// <summary>
        /// 获取分页List
        /// </summary>
        /// <param name="whMaterialInventoryPagedQueryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<WhMaterialInventoryPageListViewDto>> GetPageListAsync(WhMaterialInventoryPagedQueryDto whMaterialInventoryPagedQueryDto);

        /// <summary>
        /// 查询是否已存在物料条码
        /// </summary>
        /// <param name="materialBarCode"></param>
        /// <returns></returns>
        Task<bool> CheckMaterialBarCodeAnyAsync(string materialBarCode);

        /// <summary>
        /// 根据物料条码查询
        /// </summary>
        /// <param name="barCode"></param>
        /// <returns></returns>
        Task<WhMaterialInventoryDto?> QueryWhMaterialInventoryByBarCodeAsync(string barCode);

        /// <summary>
        /// 获取条码信息
        /// </summary>
        /// <param name="barCode"></param>
        /// <returns></returns>
        Task<WhMaterialInventoryPageListViewDto?> QueryWhMaterialBarCodeAsync(string barCode);

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="whMaterialInventoryCreateDto"></param>
        /// <returns></returns>
        Task CreateWhMaterialInventoryAsync(WhMaterialInventoryCreateDto whMaterialInventoryCreateDto);


        /// <summary>
        ///批量新增
        /// </summary>
        /// <param name="requestDtos"></param>
        /// <returns></returns>
        Task CreateWhMaterialInventoryListAsync(IEnumerable<WhMaterialInventoryListCreateDto> requestDtos);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="whMaterialInventoryModifyDto"></param>
        /// <returns></returns>
        Task ModifyWhMaterialInventoryAsync(WhMaterialInventoryModifyDto whMaterialInventoryModifyDto);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteWhMaterialInventoryAsync(long id);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<int> DeletesWhMaterialInventoryAsync(string ids);

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<WhMaterialInventoryDto> QueryWhMaterialInventoryByIdAsync(long id);

        /// <summary>
        /// 根据物料编码获取物料与供应商数据
        /// </summary>
        /// <param name="materialId"></param>
        /// <returns></returns>
        Task<ProcMaterialInfoViewDto> GetMaterialAndSupplierByMateialCodeIdAsync(long materialId);


        /// <summary>
        /// 根据查询条件获取分页数据 来源外部的
        /// </summary>
        /// <param name="whMaterialInventoryPagedQueryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<WhMaterialInventoryPageListViewDto>> GetOutsidePageListAsync(WhMaterialInventoryPagedQueryDto whMaterialInventoryPagedQueryDto);

        /// <summary>
        /// 根据物料条码查询 来源外部的数据
        /// </summary>
        /// <param name="barCode"></param>
        /// <returns></returns>
        Task<WhMaterialInventoryDetailDto?> QueryOutsideWhMaterialInventoryByBarCodeAsync(string barCode);

        /// <summary>
        /// 获取物料库存相关的信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="CustomerValidationException"></exception>
        Task<WhMaterialInventoryDetailDto> QueryWhMaterialInventoryDetailByIdAsync(long id);

        /// <summary>
        /// 修改外部来源库存
        /// </summary>
        /// <param name="modifyDto"></param>
        /// <returns></returns>
        Task UpdateOutsideWhMaterialInventoryAsync(OutsideWhMaterialInventoryModifyDto modifyDto);

        /// <summary>
        /// 条码拆分
        /// </summary>
        /// <param name="adjustDto"></param>
        /// <returns></returns>
        Task<string> BarcodeSplitAdjustAsync(MaterialBarCodeSplitAdjustDto adjustDto);

        /// <summary>
        /// 物料合并
        /// </summary>
        /// <param name="adjustDto"></param>
        /// <returns></returns>
        Task<string> BarcodeMergeAdjustAsync(MaterialBarCodeMergeAdjust adjustDto);

        /// <summary>
        /// 合并拆分条码的验证
        /// </summary>
        /// <param name="sfcs"></param>
        /// <returns></returns>
        Task<bool> MergeAdjustVerifySfcAsync(string[] sfcs);
        /// <summary>
        ///  领料申请
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task PickMaterialsRequestAsync(PickMaterialsRequest request);
        Task PickMaterialsRequestAsync(PickMaterialsRequestV2 request);
        /// <summary>
        /// 取消领料
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<bool> PickMaterialsCancelAsync(PickMaterialsCancel request);
        /// <summary>
        ///  领料申请
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task MaterialReturnRequestAsync(MaterialReturnRequest request);
        /// <summary>
        /// 取消领料
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<bool> MaterialReturnCancelAsync(MaterialReturnCancel request);

        /// <summary>
        /// 成品入库
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task ProductReceiptRequestAsync(ProductReceiptRequest request);

        /// <summary>
        /// 取消入库
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<bool> ProductReceiptCancelAsync(MaterialReturnCancel request);
    }
}
