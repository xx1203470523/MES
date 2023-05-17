using FluentValidation;
using Hymson.MES.EquipmentServices.Dtos.CCDFileUploadComplete;
using Hymson.MES.Core.Domain.Integrated;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.Snowflake;
using Hymson.Utils;
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
        private readonly IManuCcdFileRepository _manuCcdFileRepository;

        private readonly AbstractValidator<CCDFileUploadCompleteDto> _validationCCDFileUploadCompleteDtoRules;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="validationCCDFileUploadCompleteDtoRules"></param>
        /// <param name="currentEquipment"></param>
        public CCDFileUploadCompleteService(IManuCcdFileRepository manuCcdFileRepository, AbstractValidator<CCDFileUploadCompleteDto> validationCCDFileUploadCompleteDtoRules, ICurrentEquipment currentEquipment)
        {
            _manuCcdFileRepository = manuCcdFileRepository;

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
            List<ManuCcdFileEntity> list = new List<ManuCcdFileEntity>();
            foreach (var item in cCDFileUploadCompleteRequest.SFCs)
            {
                list.Add(new ManuCcdFileEntity
                {
                    Passed = item.Passed,
                    SFC = item.SFC,
                    Timestamp = item.Timestamp,
                    URI = item.URI,
                    Id = IdGenProvider.Instance.CreateId(),
                    CreatedBy = _currentEquipment.Name,
                    UpdatedBy = _currentEquipment.Name,
                    CreatedOn = HymsonClock.Now(),
                    UpdatedOn = HymsonClock.Now(),
                    SiteId = _currentEquipment.SiteId
                });
            }
            await _manuCcdFileRepository.InsertsAsync(list);
        }
    }
}
