using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Integrated;

namespace Hymson.MES.Services.Services.InteClass
{
    /// <summary>
    /// 生产班次 service接口
    /// </summary>
    public interface IInteClassService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        Task<int> AddInteClassAsync(AddInteClassDto parm);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        Task<int> UpdateInteClassAsync(UpdateInteClassDto parm);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idsArr"></param>
        /// <returns></returns>
        Task<int> DeleteInteClassAsync(long[] idsArr);

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
        Task<InteClassWithDetailDto> GetInteClassAsync(long id);

    }
}
