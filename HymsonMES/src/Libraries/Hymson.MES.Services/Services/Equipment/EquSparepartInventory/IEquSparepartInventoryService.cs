/*
 *creator: Karl
 *
 *describe: 备件库存    服务接口 | 代码由框架生成
 *builder:  pengxin
 *build datetime: 2024-06-12 10:15:26
 */
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Services.Dtos.EquSparepartInventory;

namespace Hymson.MES.Services.Services.EquSparepartInventory
{
    /// <summary>
    /// 备件库存 service接口
    /// </summary>
    public interface IEquSparepartInventoryService
    {

        /// <summary>
        /// 获取备件信息
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        /// <exception cref="CustomerValidationException"></exception>
        Task<GetEquSparePartsDto> GetEquSparePartsAsync(GetEquSparePartsParamDto param);


        /// <summary>
        /// 获取出库选择备件信息
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        /// <exception cref="CustomerValidationException"></exception>
        Task<IEnumerable<GetOutboundChooseEquSparePartsDto>> GetOutboundChooseEquSparePartsAsync(GetOutboundChooseEquSparePartsParamDto param);


        /// <summary>
        /// 获取分页List
        /// </summary>
        /// <param name="equSparepartInventoryPagedQueryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<EquSparepartInventoryPageDto>> GetPagedListAsync(EquSparepartInventoryPagedQueryDto equSparepartInventoryPagedQueryDto);

        /// <summary>
        /// 备件库存操作（出入库） 
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        Task OperationEquSparepartInventoryAsync(OperationEquSparepartInventoryDto parm);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="equSparepartInventoryModifyDto"></param>
        /// <returns></returns>
        Task ModifyEquSparepartInventoryAsync(EquSparepartInventoryModifyDto equSparepartInventoryModifyDto);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteEquSparepartInventoryAsync(long id);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<int> DeletesEquSparepartInventoryAsync(long[] ids);

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<EquSparepartInventoryDto> QueryEquSparepartInventoryByIdAsync(long id);
    }
}
