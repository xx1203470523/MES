using Hymson.MES.EquipmentServices.Request.CCDFileUploadComplete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.EquipmentServices.Services.CCDFileUploadComplete
{
    /// <summary>
    /// CCD文件上传完成
    /// </summary>
    public interface ICCDFileUploadCompleteService
    {
        /// <summary>
        /// CCD文件上传完成
        /// </summary>
        /// <param name="cCDFileUploadCompleteRequest"></param>
        /// <returns></returns> 
        Task CCDFileUploadCompleteAsync(CCDFileUploadCompleteRequest cCDFileUploadCompleteRequest);
    }
}
