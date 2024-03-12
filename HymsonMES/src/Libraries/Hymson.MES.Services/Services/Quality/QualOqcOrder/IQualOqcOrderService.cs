using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Quality;

namespace Hymson.MES.Services.Services.Quality
{
    /// <summary>
    /// 服务接口（OQC检验单）
    /// </summary>
    public interface IQualOqcOrderService
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        Task<int> CreateAsync(QualOqcOrderSaveDto saveDto);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        Task<int> ModifyAsync(QualOqcOrderSaveDto saveDto);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<int> DeleteAsync(long id);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<int> DeletesAsync(long[] ids);

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<QualOqcOrderDto?> QueryByIdAsync(long id);

        /// <summary>
        /// 获取分页List
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<QualOqcOrderDto>> GetPagedListAsync(QualOqcOrderPagedQueryDto pagedQueryDto);

        /// <summary>
        /// 获取OQC检验单检验类型
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<IEnumerable<QualOqcOrderTypeOutDto>> GetOqcOrderTypeAsync(long id);

        /// <summary>
        /// 校验样品条码
        /// </summary>
        /// <param name="checkBarCodeQuqryDto"></param>
        /// <returns></returns>
        Task<CheckBarCodeOutDto> CheckBarCodeAsync(CheckBarCodeQuqryDto checkBarCodeQuqryDto);

        /// <summary>
        /// 保存样品数据
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        Task SaveOrderAsync(QualOqcOrderExecSaveDto requestDto);
    }
}