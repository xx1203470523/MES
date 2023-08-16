using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Integrated;

namespace Hymson.MES.Services.Services.Integrated
{
    /// <summary>
    /// 服务接口（消息管理）
    /// </summary>
    public interface IInteMessageManageService
    {
        /// <summary>
        /// 触发
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<int> TriggerAsync(InteMessageManageTriggerDto dto);

        /// <summary>
        /// 接收
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<int> ReceiveAsync(InteMessageManageReceiveDto dto);

        /// <summary>
        /// 处理
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<int> HandleAsync(InteMessageManageHandleDto dto);

        /// <summary>
        /// 关闭
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<int> CloseAsync(InteMessageManageCloseDto dto);

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
        Task<InteMessageManageDto?> QueryByIdAsync(long id);

        /// <summary>
        /// 获取分页List
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<InteMessageManageDto>> GetPagedListAsync(InteMessageManagePagedQueryDto pagedQueryDto);

        /// <summary>
        /// 获取消息编号
        /// </summary>
        /// <returns></returns>
        Task<string> GetCodeAsync();

    }
}