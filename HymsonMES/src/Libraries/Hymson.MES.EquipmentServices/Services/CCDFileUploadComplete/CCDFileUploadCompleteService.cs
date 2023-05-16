using FluentValidation;
using Hymson.MES.EquipmentServices.Request.CCDFileUploadComplete;
using Hymson.Web.Framework.WorkContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.EquipmentServices.Services.CCDFileUploadComplete
{
    /// <summary>
    /// CCD文件上传完成服务
    /// </summary>
    public class CCDFileUploadCompleteService : ICCDFileUploadCompleteService
    {
        private readonly ICurrentEquipment _currentEquipment;
        private readonly AbstractValidator<CCDFileUploadCompleteRequest> _validationCCDFileUploadCompleteRequestRules;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="validationCCDFileUploadCompleteRequestRules"></param>
        /// <param name="currentEquipment"></param>
        public CCDFileUploadCompleteService(AbstractValidator<CCDFileUploadCompleteRequest> validationCCDFileUploadCompleteRequestRules, ICurrentEquipment currentEquipment)
        {
            _validationCCDFileUploadCompleteRequestRules = validationCCDFileUploadCompleteRequestRules;
            _currentEquipment = currentEquipment;
        }

        /// <summary>
        /// CCD文件上传完成
        /// </summary>
        /// <param name="cCDFileUploadCompleteRequest"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task CCDFileUploadCompleteAsync(CCDFileUploadCompleteRequest cCDFileUploadCompleteRequest)
        {
            await _validationCCDFileUploadCompleteRequestRules.ValidateAndThrowAsync(cCDFileUploadCompleteRequest);
            throw new NotImplementedException();
        }
    }
}
