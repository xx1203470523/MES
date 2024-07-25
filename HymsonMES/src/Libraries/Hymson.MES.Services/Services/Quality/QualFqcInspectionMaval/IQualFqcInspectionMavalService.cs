/*
 *creator: Karl
 *
 *describe: 马威FQC检验    服务接口 | 代码由框架生成
 *builder:  pengxin
 *build datetime: 2024-07-24 03:09:40
 */
using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.EquRepairOrder;
using Hymson.MES.Services.Dtos.Integrated;
using Hymson.MES.Services.Dtos.QualFqcInspectionMaval;
using Hymson.Web.Framework.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Services.Services.QualFqcInspectionMaval
{
    /// <summary>
    /// 马威FQC检验 service接口
    /// </summary>
    public interface IQualFqcInspectionMavalService
    {
        /// <summary>
        /// 获取分页List
        /// </summary>
        /// <param name="qualFqcInspectionMavalPagedQueryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<QualFqcInspectionMavalDto>> GetPagedListAsync(QualFqcInspectionMavalPagedQueryDto qualFqcInspectionMavalPagedQueryDto);

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="qualFqcInspectionMavalCreateDto"></param>
        /// <returns></returns>
        Task CreateQualFqcInspectionMavalAsync(QualFqcInspectionMavalCreateDto qualFqcInspectionMavalCreateDto);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="qualFqcInspectionMavalModifyDto"></param>
        /// <returns></returns>
        Task ModifyQualFqcInspectionMavalAsync(QualFqcInspectionMavalModifyDto qualFqcInspectionMavalModifyDto);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteQualFqcInspectionMavalAsync(long id);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<int> DeletesQualFqcInspectionMavalAsync(long[] ids);

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<QualFqcInspectionMavalDto> QueryQualFqcInspectionMavalByIdAsync(long id);



        /// <summary>
        /// 上传单据附件
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<int> SaveAttachmentAsync(QualFqcInspectionMavalSaveAttachmentDto dto);

        /// <summary>
        /// 删除单据附件
        /// </summary>
        /// <param name="orderAttachmentId"></param>
        /// <returns></returns>
        Task<int> DeleteAttachmentByIdAsync(long orderAttachmentId);

        /// <summary>
        /// 查询单据附件
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns> 
        Task<IEnumerable<InteAttachmentBaseDto>> QueryOrderAttachmentListByIdAsync(QualFqcInspectionMavalAttachmentDto dto);


        /// <summary>
        /// 获取ID
        /// </summary>
        /// <returns></returns>
        long GetNewId();
    }
}
