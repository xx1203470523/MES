using Hymson.Infrastructure;
using Hymson.MES.CoreServices.Bos.Job;
using Hymson.MES.CoreServices.Dtos.Common;
using Hymson.MES.Services.Dtos.Manufacture;

namespace Hymson.MES.Services.Services.Manufacture
{
    /// <summary>
    /// 操作面板按钮 service接口
    /// </summary>
    public interface IManuFacePlateButtonService
    {
        /// <summary>
        /// 获取分页List
        /// </summary>
        /// <param name="manuFacePlateButtonPagedQueryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<ManuFacePlateButtonDto>> GetPagedListAsync(ManuFacePlateButtonPagedQueryDto manuFacePlateButtonPagedQueryDto);

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="manuFacePlateButtonCreateDto"></param>
        /// <returns></returns>
        Task CreateManuFacePlateButtonAsync(ManuFacePlateButtonCreateDto manuFacePlateButtonCreateDto);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="manuFacePlateButtonModifyDto"></param>
        /// <returns></returns>
        Task ModifyManuFacePlateButtonAsync(ManuFacePlateButtonModifyDto manuFacePlateButtonModifyDto);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteManuFacePlateButtonAsync(long id);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<int> DeletesManuFacePlateButtonAsync(long[] ids);

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ManuFacePlateButtonDto> QueryManuFacePlateButtonByIdAsync(long id);

        /// <summary>
        /// 根据buttonId查询
        /// </summary>
        /// <param name="buttionId"></param>
        /// <returns></returns>
        Task<ManuFacePlateButtonDto> QueryManuFacePlateButtonByButtonIdAsync(long buttionId);

        /// <summary>
        /// 按钮（回车）
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<Dictionary<string, JobResponseDto>> EnterAsync(EnterRequestDto dto);

        /// <summary>
        /// 按钮（点击）
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<Dictionary<string, JobResponseDto>> ClickAsync(ButtonRequestDto dto);

        /// <summary>
        /// 参数收集（点击）
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<int> ProductParameterCollectAsync(ProductProcessParameterDto dto);


        /// <summary>
        ///  新按钮（点击）
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="bo"></param>
        /// <returns></returns>
        Task<Dictionary<string, JobResponseBo>> NewClickAsync(ButtonRequestDto dto, dynamic bo);

    }
}
