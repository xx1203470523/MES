using FluentValidation;
using Hymson.MES.Core.Domain.Integrated;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.EquipmentServices.Request.CCDFileUploadComplete;
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
        private readonly AbstractValidator<CCDFileUploadCompleteRequest> _validationCCDFileUploadCompleteRequestRules;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="manuCcdFileRepository"></param>
        /// <param name="validationCCDFileUploadCompleteRequestRules"></param>
        /// <param name="currentEquipment"></param> 
        public CCDFileUploadCompleteService(IManuCcdFileRepository manuCcdFileRepository, AbstractValidator<CCDFileUploadCompleteRequest> validationCCDFileUploadCompleteRequestRules, ICurrentEquipment currentEquipment)
        {
            _manuCcdFileRepository = manuCcdFileRepository;
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
