/*
 *creator: Karl
 *
 *describe: 环境检验单检验明细    服务接口 | 代码由框架生成
 *builder:  pengxin
 *build datetime: 2024-03-22 05:04:43
 */
using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.QualEnvOrderDetail;

namespace Hymson.MES.Services.Services.QualEnvOrderDetail
{
    /// <summary>
    /// 环境检验单检验明细 service接口
    /// </summary>
    public interface IQualEnvOrderDetailService
    {
        /// <summary>
        /// 获取分页List
        /// </summary>
        /// <param name="qualEnvOrderDetailPagedQueryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<QualEnvOrderDetailDto>> GetPagedListAsync(QualEnvOrderDetailPagedQueryDto qualEnvOrderDetailPagedQueryDto);

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="qualEnvOrderDetailCreateDto"></param>
        /// <returns></returns>
        Task CreateQualEnvOrderDetailAsync(QualEnvOrderDetailCreateDto qualEnvOrderDetailCreateDto);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="qualEnvOrderDetailModifyDto"></param>
        /// <returns></returns>
        Task ModifyQualEnvOrderDetailAsync(QualEnvOrderDetailModifyDto qualEnvOrderDetailModifyDto);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteQualEnvOrderDetailAsync(long id);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<int> DeletesQualEnvOrderDetailAsync(long[] ids);

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<QualEnvOrderDetailDto> QueryQualEnvOrderDetailByIdAsync(long id);
    }
}
