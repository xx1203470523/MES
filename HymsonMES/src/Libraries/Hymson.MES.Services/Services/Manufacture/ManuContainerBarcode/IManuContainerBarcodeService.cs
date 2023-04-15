/*
 *creator: Karl
 *
 *describe: 容器条码表    服务接口 | 代码由框架生成
 *builder:  wxk
 *build datetime: 2023-04-12 02:29:23
 */
using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Manufacture;

namespace Hymson.MES.Services.Services.Manufacture
{
    /// <summary>
    /// 容器条码表 service接口
    /// </summary>
    public interface IManuContainerBarcodeService
    {
        /// <summary>
        /// 获取分页List
        /// </summary>
        /// <param name="manuContainerBarcodePagedQueryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<ManuContainerBarcodeDto>> GetPagedListAsync(ManuContainerBarcodePagedQueryDto manuContainerBarcodePagedQueryDto);

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="manuContainerBarcodeCreateDto"></param>
        /// <returns></returns>
        Task<ManuContainerBarcodeView> CreateManuContainerBarcodeAsync(ManuContainerBarcodeCreateDto manuContainerBarcodeCreateDto);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="manuContainerBarcodeModifyDto"></param>
        /// <returns></returns>
        Task ModifyManuContainerBarcodeAsync(ManuContainerBarcodeModifyDto manuContainerBarcodeModifyDto);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteManuContainerBarcodeAsync(long id);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<int> DeletesManuContainerBarcodeAsync(long[] ids);

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ManuContainerBarcodeDto> QueryManuContainerBarcodeByIdAsync(long id);
    }
}
