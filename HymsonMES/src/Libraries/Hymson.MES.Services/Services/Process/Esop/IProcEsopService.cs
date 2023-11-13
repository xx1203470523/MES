/*
 *creator: Karl
 *
 *describe: ESOP    服务接口 | 代码由框架生成
 *builder:  zhaoqing
 *build datetime: 2023-11-02 02:39:53
 */
using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Integrated;
using Hymson.MES.Services.Dtos.Process;
using Hymson.MES.Services.Dtos.Quality;

namespace Hymson.MES.Services.Services.Process
{
    /// <summary>
    /// ESOP service接口
    /// </summary>
    public interface IProcEsopService
    {
        /// <summary>
        /// 获取分页List
        /// </summary>
        /// <param name="procEsopPagedQueryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<ProcEsopDto>> GetPagedListAsync(ProcEsopPagedQueryDto procEsopPagedQueryDto);

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="procEsopCreateDto"></param>
        /// <returns></returns>
        Task CreateProcEsopAsync(ProcEsopCreateDto procEsopCreateDto);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="procEsopModifyDto"></param>
        /// <returns></returns>
        Task ModifyProcEsopAsync(ProcEsopModifyDto procEsopModifyDto);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteProcEsopAsync(long id);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<int> DeletesProcEsopAsync(long[] ids);

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ProcEsopDto> QueryProcEsopByIdAsync(long id);

        /// <summary>
        /// 根据Esop ID获取esop附件列表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<IEnumerable<InteAttachmentDto>?> GetAttachmentListAsync(long id);

        /// <summary>
        /// 附件上传
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<int> AttachmentAddAsync(AttachmentAddDto dto);

        /// <summary>
        /// 附件删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<int> AttachmentDeleteAsync(long[] ids);
    }
}
