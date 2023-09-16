using Hymson.Infrastructure;
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
        Task<PagedInfo<InteSFCBoxRView>> GetBoxCodeListAsync(InteSFCBoxQueryDto pagedQueryDto);
    }
}
