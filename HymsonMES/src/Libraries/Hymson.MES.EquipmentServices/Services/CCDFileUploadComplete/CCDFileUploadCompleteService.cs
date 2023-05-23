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
using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Core.Constants;
using System.Security.Policy;
using System.Reflection.Metadata;
using Hymson.Utils.Tools;

namespace Hymson.MES.EquipmentServices.Services.CCDFileUploadComplete
{
    /// <summary>
    /// CCD文件上传完成服务
    /// </summary>
    public class CCDFileUploadCompleteService : ICCDFileUploadCompleteService
    {
        private readonly ICurrentEquipment _currentEquipment;
        private readonly IManuCcdFileRepository _manuCcdFileRepository;
        private readonly IManuSfcRepository _manuSfcRepository;

        private readonly AbstractValidator<CCDFileUploadCompleteDto> _validationCCDFileUploadCompleteDtoRules;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="manuCcdFileRepository"></param>
        /// <param name="manuSfcRepository"></param>
        /// <param name="validationCCDFileUploadCompleteDtoRules"></param>
        /// <param name="currentEquipment"></param>
        public CCDFileUploadCompleteService(IManuCcdFileRepository manuCcdFileRepository, IManuSfcRepository manuSfcRepository, AbstractValidator<CCDFileUploadCompleteDto> validationCCDFileUploadCompleteDtoRules, ICurrentEquipment currentEquipment)
        {
            _manuCcdFileRepository = manuCcdFileRepository;
            _manuSfcRepository = manuSfcRepository;

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
            var sfcs = cCDFileUploadCompleteDto.SFCs.Select(it => it.SFC).ToArray();
            var manuSfcList = await _manuSfcRepository.GetBySFCsAsync(sfcs);
            if (manuSfcList == null || !manuSfcList.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES19117)).WithData("SFC", string.Join(',', sfcs));
            }
            //验证条码
            var notSfc = sfcs.Where(it => !manuSfcList.Where(sfc => sfc.SFC == it).Any());
            if (notSfc != null && notSfc.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES19117)).WithData("SFC", string.Join(',', notSfc));
            }

            var manuCcdFile = await _manuCcdFileRepository.GetManuCcdFileEntitiesAsync(new ManuCcdFileQuery { SiteId = _currentEquipment.SiteId, Sfcs = sfcs });
            List<ManuCcdFileEntity> addList = new();
            List<ManuCcdFileEntity> updateList = new();
            foreach (var item in cCDFileUploadCompleteDto.SFCs)
            {
                var manuCcdFileEntit = new ManuCcdFileEntity();
                if (manuCcdFile != null && manuCcdFile.Any())
                {
                    manuCcdFileEntit = manuCcdFile.Where(it => it.SFC == item.SFC).FirstOrDefault();
                }
                //添加与更改
                if (manuCcdFileEntit != null && manuCcdFileEntit.Id > 0)
                {
                    manuCcdFileEntit.Passed = item.Passed;
                    manuCcdFileEntit.SFC = item.SFC;
                    manuCcdFileEntit.Timestamp = item.Timestamp;
                    manuCcdFileEntit.URL = item.URL;
                    manuCcdFileEntit.UpdatedBy = _currentEquipment.Name;
                    manuCcdFileEntit.UpdatedOn = HymsonClock.Now();
                    updateList.Add(manuCcdFileEntit);
                }
                else
                {
                    addList.Add(new ManuCcdFileEntity
                    {
                        Passed = item.Passed,
                        SFC = item.SFC,
                        Timestamp = item.Timestamp,
                        URL = item.URL,
                        Id = IdGenProvider.Instance.CreateId(),
                        CreatedBy = _currentEquipment.Name,
                        UpdatedBy = _currentEquipment.Name,
                        CreatedOn = HymsonClock.Now(),
                        UpdatedOn = HymsonClock.Now(),
                        SiteId = _currentEquipment.SiteId
                    });
                }

            }
            //入库
            var trans = TransactionHelper.GetTransactionScope();
            if (updateList != null && updateList.Any())
            {
                var res = await _manuCcdFileRepository.UpdatesAsync(updateList);
            }
            if (addList != null && addList.Any())
            {
                var res = await _manuCcdFileRepository.InsertsAsync(addList);
            }
            trans.Complete();
        }
    }
}
