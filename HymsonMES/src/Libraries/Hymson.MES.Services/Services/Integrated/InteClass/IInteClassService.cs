using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Integrated;

namespace Hymson.MES.Services.Services.Integrated.InteClass
{
    /// <summary>
    /// 班制维护 service接口
    /// </summary>
    public interface IInteClassService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        Task<int> CreateAsync(InteClassCreateDto parm);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        Task<int> ModifyAsync(InteClassModifyDto parm);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idsArr"></param>
        /// <returns></returns>
        Task<int> DeletesAsync(long[] idsArr);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<InteClassDto>> GetPagedListAsync(InteClassPagedQueryDto pagedQueryDto);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<InteClassWithDetailDto> GetDetailAsync(long id);

    }
}
