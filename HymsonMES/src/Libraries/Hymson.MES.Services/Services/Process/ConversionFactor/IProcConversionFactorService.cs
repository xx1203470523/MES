using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Common;
using Hymson.MES.Services.Dtos.Integrated;
using Hymson.MES.Services.Dtos.Process;

namespace Hymson.MES.Services.Services.Process.Procedure
{
    /// <summary>
    /// 转换系数表 service接口
    /// </summary>
    public interface IProcConversionFactorService
    {


        /// <summary>
        /// 获取分页List
        /// </summary>
        /// <param name="procConversionFactorPagedQueryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<ProcConversionFactorViewDto>> GetPageListAsync(ProcConversionFactorPagedQueryDto procConversionFactorPagedQueryDto);


        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ProcConversionFactorDto> QueryProcConversionFactorByIdAsync(long id);

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="procConversionFactorCreateDto"></param>
        /// <returns></returns>
        Task<long> AddProcConversionFactorAsync(AddConversionFactorDto procConversionFactorCreateDto);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<int> DeleteProcConversionFactorAsync(long[] ids);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="procConversionFactorModifyDto"></param>
        /// <returns></returns>
        Task ModifyProcConversionFactorAsync(ProcConversionFactorModifyDto procConversionFactorModifyDto);


    }
}
