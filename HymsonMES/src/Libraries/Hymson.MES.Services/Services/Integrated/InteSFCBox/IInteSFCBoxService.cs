using Hymson.Infrastructure;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Services.Dtos.Integrated;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Services.Integrated.InteSFCBox
{
    public interface IInteSFCBoxService
    {
        /// <summary>
        /// 电芯批次导入
        /// </summary>
        /// <param name="uploadStockDetailDto"></param>
        /// <returns></returns>
        Task<int> ImportDataAsync(UploadSFCBoxDto uploadStockDetailDto);

        /// <summary>
        /// 电信批次导入（自动过滤重复的）
        /// </summary>
        /// <param name="uploadStockDetailDto"></param>
        /// <returns></returns>
        Task<int> ImportDataNoRepeatAsync(UploadSFCBoxDto uploadStockDetailDto);

        /// <summary>
        /// 导入模板下载
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        Task DownloadImportTemplateAsync(Stream stream);

        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<InteSFCBoxDto>> GetPagedListAsync(InteSFCBoxQueryDto pagedQueryDto);

        /// <summary>
        /// 工单新新查询弹出窗口
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<InteSFCBoxRView>> GetBoxCodeListAsync(InteSFCBoxQueryDto pagedQueryDto);

        /// <summary>
        /// 箱码工单验证
        /// </summary>
        /// <param name="validate"></param>
        /// <returns></returns>
        Task<InteSFCBoxValidateResponse> SFCValidate(InteSFCBoxValidateQuery validate);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="idsArr"></param>
        /// <returns></returns>
        Task<int> DeletesAsync(long[] idsArr);


    }
}
