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
using Hymson.MES.CoreServices.Services.Job;
using Hymson.MES.CoreServices.Services.Manufacture.ManuGenerateBarcode;
using Hymson.MES.CoreServices.Services.Quality.QualFqcOrder;
using Hymson.MES.Data.Repositories.Integrated;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.Warehouse;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;
using Microsoft.Extensions.Logging;
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
       
        private readonly IManuSfcProduceRepository _manuSfcProduceRepository;
       
        private readonly IManuSfcRepository _manuSfcRepository;
        private readonly IManuSfcInfoRepository _manuSfcInfoRepository;
        private readonly IManuSfcCirculationRepository _manuSfcCirculationRepository;
       
        private readonly IManuGenerateBarcodeService _manuGenerateBarcodeService;
        private readonly IProcMaterialRepository _procMaterialRepository;
       
        private readonly IInteCodeRulesRepository _inteCodeRulesRepository;
        

        public ManuMergeService(ILocalizationService localizationService
            , IManuSfcCirculationRepository manuSfcCirculationRepository
            , IManuSfcInfoRepository manuSfcInfoRepository
       
            ,IManuSfcRepository manuSfcRepository
            , IProcMaterialRepository procMaterialRepository
          
            , IManuGenerateBarcodeService manuGenerateBarcodeService
            , IInteCodeRulesRepository inteCodeRulesRepository
            , IManuSfcProduceRepository manuSfcProduceRepository) {
            _localizationService = localizationService;
            _manuSfcCirculationRepository = manuSfcCirculationRepository;
            _manuSfcInfoRepository = manuSfcInfoRepository;
            _manuSfcProduceRepository = manuSfcProduceRepository;
            _manuSfcRepository = manuSfcRepository;
           
            _manuGenerateBarcodeService = manuGenerateBarcodeService;
            _procMaterialRepository = procMaterialRepository;
           
            _inteCodeRulesRepository = inteCodeRulesRepository;
        }
        
        /// <summary>
        /// 内部条码合并
        /// </summary>
        /// <param name="sourcesfcs"></param>
        /// <param name="targetsfc"></param>
        /// <param name="localizationService"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<string> MergeAsync(ManuMergeRequestDto param,string updateName)
        {
            var validationFailures = new List<ValidationFailure>();
            string sourcekey = string.Empty;  //隐式条码 ，需要被GB替换
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
                //ProcedureId = param.ProcedureId,
            });
           
            if(sfcCirculationEntities == null || !sfcCirculationEntities.Any())
            {
                var validationFailure = new ValidationFailure();
                validationFailure.FormattedMessagePlaceholderValues = new Dictionary<string, object>();
                validationFailure.FormattedMessagePlaceholderValues.Add("sfc", string.Join(',', param.Barcodes));
                validationFailure.ErrorCode = nameof(ErrorCode.MES12840);
                validationFailures.Add(validationFailure);
               
            }
            else
            {
                
                var groupCirulationEntities = sfcCirculationEntities.GroupBy(g => g.CirculationBarCode);
                if (groupCirulationEntities.Count() > 1)
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
                throw new ValidationException(_localizationService.GetResource("SFCError"), validationFailures);
            }
            else
            {
                string targetSFC = string.Empty;
                var manusfc = await _manuSfcRepository.GetSingleAsync(new ManuSfcQuery
                {
                    SFC = sourcekey,
                    SiteId = param.SiteId,
                });
                var sfcInfoEntity = await _manuSfcInfoRepository.GetBySFCIdAsync(manusfc.Id);
               
                var sfcproduce = await _manuSfcProduceRepository.GetBySFCIdAsync(manusfc.Id);

                //生成GB码
                var  materialEntity = await _procMaterialRepository.GetByIdAsync(sfcInfoEntity.ProductId);
                var inteCodeRule = await _inteCodeRulesRepository.GetInteCodeRulesByProductIdAsync(new Data.Repositories.Integrated.InteCodeRule.Query.InteCodeRulesByProductQuery
                {
                    CodeType = Core.Enums.Integrated.CodeRuleCodeTypeEnum.ProcessControlSeqCode,
                    ProductId = sfcInfoEntity.ProductId,
                }) ?? throw new CustomerValidationException(nameof(ErrorCode.MES16501)).WithData("product", materialEntity.MaterialCode);

                var barcodes = await _manuGenerateBarcodeService.GenerateBarcodeListByIdAsync(new Bos.Manufacture.ManuGenerateBarcode.GenerateBarcodeBo
                {
                    SiteId = param.SiteId,
                    CodeRuleId = inteCodeRule.Id,
                    Count = 1,
                    Sfcs = param.Barcodes,
                    ProductId = sfcInfoEntity.ProductId,
                    WorkOrderId = sfcInfoEntity.WorkOrderId,
                    UserName = updateName,

                });
                if(barcodes==null||!barcodes.Any())
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES16200));
                }
                else
                {
                    targetSFC = barcodes.First();
                }
                manusfc.SFC = targetSFC; //替换条码为国标码
                var lst = sfcCirculationEntities.ToList();
                lst.ForEach(i => i.CirculationBarCode = targetSFC); //更新流转记录的流转后条码值
               
                using var trans = TransactionHelper.GetTransactionScope();
                
                await _manuSfcRepository.UpdateAsync(manusfc); //更新分组条码为国标码
                //更新条码转换表 ，分组条码变更为国标码
                await _manuSfcCirculationRepository.UpdateRangeAsync(lst);
                //更新 级组条码为完成状态
                await _manuSfcRepository.UpdateStatusAsync(new ManuSfcUpdateCommand
                {
                    SiteId = param.SiteId,
                    Sfcs = param.Barcodes.ToArray(),
                    Status = Core.Enums.SfcStatusEnum.Complete,
                    UpdatedOn =HymsonClock.Now(),
                    UserId = updateName
                });
                //在制表更新分组条码为国标码
                await _manuSfcProduceRepository.UpdateSFCByIdAsync(new UpdateManuSfcProduceSFCByIdCommand
                {
                    SFC = targetSFC,
                    Id = sfcproduce.Id,
                    UpdatedBy = updateName,
                    UpdatedOn = HymsonClock.Now(),
                });
                trans.Complete();
                return targetSFC;

            }
            

        }

        public async Task UnBindAsync(ManuUnBindDto param, ILocalizationService localizationService)
        {
            throw new NotImplementedException();
        }
       
    }
}
