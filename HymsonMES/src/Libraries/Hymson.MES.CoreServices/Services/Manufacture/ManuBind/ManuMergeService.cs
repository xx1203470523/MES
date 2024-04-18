using FluentValidation;
using FluentValidation.Results;
using Hymson.Infrastructure.Exceptions;
using Hymson.Localization.Services;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.CoreServices.Bos.Common;
using Hymson.MES.CoreServices.Dtos.Manufacture;
using Hymson.MES.CoreServices.Dtos.Manufacture.ManuBind;
using Hymson.MES.CoreServices.Services.Common;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Warehouse;
using Hymson.Utils.Tools;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.CoreServices.Services.Manufacture.ManuBind
{
    /// <summary>
    /// 条码合并服务
    /// </summary>
    public class ManuMergeService : IManuMergeService
    {
        private readonly ILocalizationService _localizationService;
        private readonly IManuCommonService _manuCommonService;
        private readonly IManuSfcProduceRepository _manuSfcProduceRepository;
        private readonly IWhMaterialInventoryRepository _whMaterialInventoryRepository;
        private readonly IManuSfcRepository _manuSfcRepository;
        private readonly IManuSfcInfoRepository _manuSfcInfoRepository;
        private readonly IManuSfcCirculationRepository _manuSfcCirculationRepository;
        private readonly IManuSfcStepRepository _manuSfcStepRepository;
        public ManuMergeService(ILocalizationService localizationService
            , IManuSfcCirculationRepository manuSfcCirculationRepository
            , IManuSfcInfoRepository manuSfcInfoRepository
            ,IManuSfcStepRepository manuSfcStepRepository
            ,IManuSfcRepository manuSfcRepository
            ,IManuSfcProduceRepository manuSfcProduceRepository) {
            _localizationService = localizationService;
            _manuSfcCirculationRepository = manuSfcCirculationRepository;
            _manuSfcInfoRepository = manuSfcInfoRepository;
            _manuSfcProduceRepository = manuSfcProduceRepository;
            _manuSfcRepository = manuSfcRepository;
            _manuSfcStepRepository = manuSfcStepRepository;
        }
        
        /// <summary>
        /// 内部条码合并
        /// </summary>
        /// <param name="sourcesfcs"></param>
        /// <param name="targetsfc"></param>
        /// <param name="localizationService"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task MergeAsync(ManuMergeDto param, ILocalizationService localizationService)
        {
            var validationFailures = new List<ValidationFailure>();
            string sourcekey = string.Empty;
            var manuSfcProduceEntities = await _manuSfcProduceRepository.GetListBySfcsAsync(new ManuSfcProduceBySfcsQuery
            {
                Sfcs = param.Barcodes,
                SiteId = param.SiteId
            });
            if (manuSfcProduceEntities == null || !manuSfcProduceEntities.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES17415)).WithData("SFC", string.Join(',', param.Barcodes));
            }

            // 是否有不属于在制品表的条码
            var notIncludeSFCs = param.Barcodes.Except(manuSfcProduceEntities.Select(s => s.SFC));
            if (notIncludeSFCs.Any())
            {
                var validationFailure = new ValidationFailure();
                validationFailure.FormattedMessagePlaceholderValues = new Dictionary<string, object>();
                validationFailure.FormattedMessagePlaceholderValues.Add("SFC", string.Join(',', param.Barcodes));
                validationFailure.ErrorCode = nameof(ErrorCode.MES17415);
                validationFailures.Add(validationFailure);
              //  throw new CustomerValidationException(nameof(ErrorCode.MES17415)).WithData("SFC", string.Join(',', notIncludeSFCs));
            }
          
            var sfcCirculationEntities = await _manuSfcCirculationRepository.GetSfcMoudulesAsync(new ManuSfcCirculationBySfcsQuery
            {
                Sfc = param.Barcodes,
                SiteId = param.SiteId,
                CirculationTypes = new List<SfcCirculationTypeEnum>() { SfcCirculationTypeEnum.Merge }.ToArray(),
                ProcedureId = param.ProcedureId,
            });
           
            if(sfcCirculationEntities == null || !sfcCirculationEntities.Any())
            {
                var validationFailure = new ValidationFailure();
                validationFailure.FormattedMessagePlaceholderValues = new Dictionary<string, object>();
                validationFailure.FormattedMessagePlaceholderValues.Add("sfc", string.Join(',', param.Barcodes));
                validationFailure.ErrorCode = nameof(ErrorCode.MES12840);
                validationFailures.Add(validationFailure);
               // throw new CustomerValidationException(nameof(ErrorCode.MES12840)).WithData("sfc", string.Join(',', param.Barcodes));
            }
            else
            {
                //notIncludeSFCs = param.Barcodes.Except(sfcCirculationEntities.Select(s => s.SFC));
                //if (notIncludeSFCs.Any())
                //{
                //    throw new CustomerValidationException(nameof(ErrorCode.MES12817));//.WithData("SFC", string.Join(',', notIncludeSFCs));
                //}
                var groupCirulationEntities = sfcCirculationEntities.GroupBy(g => g.CirculationBarCode);
                if (groupCirulationEntities.Count() >= 1)
                {
                    var validationFailure = new ValidationFailure();
                    validationFailure.FormattedMessagePlaceholderValues = new Dictionary<string, object>();
                    validationFailure.FormattedMessagePlaceholderValues.Add("sfc1", string.Join(',', string.Join(',', groupCirulationEntities.ToList()[0].Select(s=>s.SFC))));
                    validationFailure.FormattedMessagePlaceholderValues.Add("sfc2", string.Join(',', string.Join(',', groupCirulationEntities.ToList()[1].Select(s => s.SFC))));
                    validationFailure.ErrorCode = nameof(ErrorCode.MES12841);
                    validationFailures.Add(validationFailure);
                  
                }
                else
                {
                    sourcekey = groupCirulationEntities.First().Key;
                }
            }
            if (validationFailures.Any())
            {
                throw new ValidationException(localizationService.GetResource("SFCError"), validationFailures);
            }
            else
            {
                var lst = sfcCirculationEntities.ToList();
                lst.ForEach(i => i.CirculationBarCode = param.TargetSFC);
                var manusfc = await _manuSfcRepository.GetSingleAsync(new ManuSfcQuery
                {
                    SFC = sourcekey,
                    SiteId = param.SiteId,
                });
                manusfc.SFC = param.TargetSFC;
                var sfcproduce = await _manuSfcProduceRepository.GetBySFCIdAsync(manusfc.Id);
                sfcproduce.SFC = param.TargetSFC;
                var sfcStepEntity = await _manuSfcStepRepository.GetSfcMergeOrSplitAddStepAsync(new SfcMergeOrSplitAddStepQuery
                {
                    Sfc = sourcekey,
                    SiteId = param.SiteId,
                });
                sfcStepEntity.SFC = param.TargetSFC;
                using var trans = TransactionHelper.GetTransactionScope();

               
                await _manuSfcStepRepository.InsertAsync(sfcStepEntity);
                

                await _manuSfcRepository.UpdateAsync(manusfc);

                
                await _manuSfcCirculationRepository.InsertRangeAsync(lst);
                await _manuSfcProduceRepository.UpdateAsync(sfcproduce);
                trans.Complete();

            }






        }

        public async Task UnBindAsync(ManuUnBindDto param, ILocalizationService localizationService)
        {
            throw new NotImplementedException();
        }
    }
}
