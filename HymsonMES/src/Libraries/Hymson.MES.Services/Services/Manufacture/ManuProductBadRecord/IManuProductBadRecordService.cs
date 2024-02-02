/*
 *creator: Karl
 *
 *describe: 产品不良录入    服务接口 | 代码由框架生成
 *builder:  zhaoqing
 *build datetime: 2023-03-27 03:49:17
 */
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Services.Dtos.Manufacture;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Services.Manufacture
{
    /// <summary>
    /// 产品不良录入 service接口
    /// </summary>
    public interface IManuProductBadRecordService
    {
        /// <summary>
        /// 获取分页List
        /// </summary>
        /// <param name="manuProductBadRecordPagedQueryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<ManuProductBadRecordDto>> GetPageListAsync(ManuProductBadRecordPagedQueryDto manuProductBadRecordPagedQueryDto);

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="manuProductBadRecordCreateDto"></param>
        /// <returns></returns>
        Task CreateManuProductBadRecordAsync(ManuProductBadRecordCreateDto manuProductBadRecordCreateDto);

        /// <summary>
        /// 新增 (有条码类型)
        /// 给面板 特殊业务使用：多个载具编码
        /// </summary>
        /// <param name="createDto"></param>
        /// <returns></returns>
        Task CreateManuProductBadRecordAboutCodeTypeAsync(FacePlateManuProductBadRecordCreateDto createDto);

        /// <summary>
        /// 查询条码的不合格代码信息
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuProductBadRecordViewDto>> GetBadRecordsBySfcAsync(ManuProductBadRecordQueryDto queryDto);

        /// <summary>
        /// 不良复判
        /// </summary>
        /// <param name="badReJudgmentDto"></param>
        /// <returns></returns>
        Task BadReJudgmentAsync(BadReJudgmentDto badReJudgmentDto);

        /// <summary>
        /// 取消标识
        /// </summary>
        /// <param name="cancelDto"></param>
        /// <returns></returns>
        Task CancelSfcIdentificationAsync(CancelSfcIdentificationDto cancelDto);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="manuProductBadRecordModifyDto"></param>
        /// <returns></returns>
        Task ModifyManuProductBadRecordAsync(ManuProductBadRecordModifyDto manuProductBadRecordModifyDto);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="idsArr"></param>
        /// <returns></returns>
        Task<int> DeletesManuProductBadRecordAsync(long[] idsArr);

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ManuProductBadRecordDto> QueryManuProductBadRecordByIdAsync(long id);

        #region 录入标识
        /// <summary>
        /// 录入标识
        /// </summary>
        /// <param name="productBadRecordMarkSaveDtos"></param>
        /// <returns></returns>
        /// <exception cref="CustomerValidationException"></exception>
        Task SaveBadRecordMarkEntryAsync(List<ManuProductBadRecordMarkSaveDto> productBadRecordMarkSaveDtos);

        /// <summary>
        /// 录入标识导出模板
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        Task DownloadImportTemplateAsync(Stream stream);

        /// <summary>
        /// 导入数据
        /// </summary>
        /// <param name="formFile"></param>
        /// <returns></returns>
        /// <exception cref="CustomerValidationException"></exception>
        Task ImportBadRecordMarkEntryAsync(IFormFile formFile);
        #endregion
    }
}
