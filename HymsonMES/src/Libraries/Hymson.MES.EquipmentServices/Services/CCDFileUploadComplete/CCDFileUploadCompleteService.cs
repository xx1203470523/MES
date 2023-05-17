using FluentValidation;
using Hymson.MES.EquipmentServices.Dtos.CCDFileUploadComplete;
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
        private readonly AbstractValidator<CCDFileUploadCompleteDto> _validationCCDFileUploadCompleteDtoRules;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="validationCCDFileUploadCompleteDtoRules"></param>
        /// <param name="currentEquipment"></param>
        public CCDFileUploadCompleteService(AbstractValidator<CCDFileUploadCompleteDto> validationCCDFileUploadCompleteDtoRules, ICurrentEquipment currentEquipment)
        {
            _validationCCDFileUploadCompleteDtoRules = validationCCDFileUploadCompleteDtoRules;
            _currentEquipment = currentEquipment;
        }

        /// <summary>
        /// CCD文件上传完成
        /// </summary>
        /// <param name="cCDFileUploadCompleteDto"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task CCDFileUploadCompleteAsync(CCDFileUploadCompleteDto cCDFileUploadCompleteDto)
        {
            await _validationCCDFileUploadCompleteDtoRules.ValidateAndThrowAsync(cCDFileUploadCompleteDto);
            throw new NotImplementedException();
        }
    }
}
