using Hymson.Infrastructure;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Services.Dtos.Manufacture;

namespace Hymson.MES.Services.Services.Manufacture
{
    /// <summary>
    /// 在制品拆解服务类 service接口
    /// </summary>
    public interface IInProductDismantleService
    {
        /// <summary>
        /// 根据ID查询Bom 详情
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        Task<List<InProductDismantleDto>> GetProcBomDetailAsync(InProductDismantleQueryDto queryDto);

        /// <summary>
        /// 在制品移除
        /// </summary>
        /// <param name="removeDto"></param>
        /// <returns></returns>
        Task RemoveModuleAsync(InProductDismantleRemoveDto removeDto);

        /// <summary>
        /// 在制品拆解添加组件
        /// </summary>
        /// <param name="addDto"></param>
        /// <returns></returns>
        Task AddModuleAsync(InProductDismantleAddDto addDto);

        /// <summary>
        /// 在制品拆解换件
        /// </summary>
        /// <param name="replaceDto"></param>
        /// <returns></returns>
        Task ReplaceModuleAsync(InProductDismantleReplaceDto replaceDto);
    }
}
