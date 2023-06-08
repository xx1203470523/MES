using FluentValidation;
using Hymson.Infrastructure;
using Hymson.MES.CoreServices.Dtos.Common;
using Hymson.MES.Services.Dtos.Integrated;

namespace Hymson.MES.Services.Services.Integrated.IIntegratedService
{
    /// <summary>
    /// 工作中心表服务接口
    /// @author admin
    /// @date 2023-02-22
    /// </summary>
    public interface IInteWorkCenterService
    {
        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="pram"></param>
        /// <returns></returns>
        Task<PagedInfo<InteWorkCenterDto>> GetPageListAsync(InteWorkCenterPagedQueryDto pram);

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<InteWorkCenterDto> QueryInteWorkCenterByIdAsync(long id);

        /// <summary>
        /// 获取关联资源
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<List<InteWorkCenterResourceRelationDto>> GetInteWorkCenterResourceRelatioByIdAsync(long id);

        /// <summary>
        /// 获取关联工作中心
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<List<InteWorkCenterRelationDto>> GetInteWorkCenterRelationByIdAsync(long id);

        /// <summary>
        /// 根据类型查询列表（工作中心）
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        Task<IEnumerable<SelectOptionDto>> QueryListByTypeAndParentIdAsync(QueryInteWorkCenterByTypeAndParentIdDto queryDto);

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="param">新增参数</param>
        /// <returns></returns>
        Task CreateInteWorkCenterAsync(InteWorkCenterCreateDto param);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<int> DeleteRangInteWorkCenterAsync(long[] ids);


        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        /// <exception cref="ValidationException">参数为空</exception>
        Task ModifyInteWorkCenterAsync(InteWorkCenterModifyDto param);
    }
}
