/*
 *creator: Karl
 *
 *describe: 环境检验单    服务接口 | 代码由框架生成
 *builder:  pengxin
 *build datetime: 2024-03-22 05:04:53
 */
using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.QualEnvOrder;

namespace Hymson.MES.Services.Services.QualEnvOrder
{
    /// <summary>
    /// 环境检验单 service接口
    /// </summary>
    public interface IQualEnvOrderService
    {
        /// <summary>
        /// 获取分页List
        /// </summary>
        /// <param name="qualEnvOrderPagedQueryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<QualEnvOrderDto>> GetPagedListAsync(QualEnvOrderPagedQueryDto qualEnvOrderPagedQueryDto);

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="qualEnvOrderCreateDto"></param>
        /// <returns></returns>
        Task CreateQualEnvOrderAsync(QualEnvOrderCreateDto qualEnvOrderCreateDto);

        /// <summary>
        /// 创建环境检验中转
        /// </summary>
        /// <param name="createConvertDto"></param>
        /// <returns></returns>
        Task QualEnvOrderCreateConvert(QualEnvOrderCreateConvertDto createConvertDto);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="qualEnvOrderModifyDto"></param>
        /// <returns></returns>
        Task ModifyQualEnvOrderAsync(QualEnvOrderModifyDto qualEnvOrderModifyDto);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteQualEnvOrderAsync(long id);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<int> DeletesQualEnvOrderAsync(long[] ids);

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<QualEnvOrderDto> QueryQualEnvOrderByIdAsync(long id);

        /// <summary>
        /// 根据ID查询关联信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<QualEnvOrderDto> QueryQualEnvOrderRelatesInfoByIdAsync(long id);
    }
}
