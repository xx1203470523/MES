using FluentValidation;
using FluentValidation.Results;
using Hymson.Infrastructure.Exceptions;
using Hymson.Localization.Services;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Domain.Plan;
using Hymson.MES.Core.Enums;
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
using static Mysqlx.Expect.Open.Types.Condition.Types;

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
        private readonly IManuSfcStepRepository _manuSfcStepRepository;
        

        public ManuMergeService(ILocalizationService localizationService
            , IManuSfcCirculationRepository manuSfcCirculationRepository
            , IManuSfcInfoRepository manuSfcInfoRepository
            , IManuSfcStepRepository manuSfcStepRepository
            , IManuSfcRepository manuSfcRepository
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
           _manuSfcStepRepository = manuSfcStepRepository;
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
            
            }
           

            if (validationFailures.Any())
            {
                throw new ValidationException(_localizationService.GetResource("SFCError"), validationFailures);
            }
            else
            {
                string targetSFC = string.Empty;
                var firstsfcproduce = manuSfcProduceEntities.First();
                var productId = firstsfcproduce.ProductId;
                var workorderId = firstsfcproduce.WorkOrderId;
                //生成GB码
                var  materialEntity = await _procMaterialRepository.GetByIdAsync(productId);
                var inteCodeRule = await _inteCodeRulesRepository.GetInteCodeRulesByProductIdAsync(new Data.Repositories.Integrated.InteCodeRule.Query.InteCodeRulesByProductQuery
                {
                    CodeType = Core.Enums.Integrated.CodeRuleCodeTypeEnum.ProcessControlSeqCode,
                    ProductId = productId,
                }) ?? throw new CustomerValidationException(nameof(ErrorCode.MES16501)).WithData("product", materialEntity.MaterialCode);

                var barcodes = await _manuGenerateBarcodeService.GenerateBarcodeListByIdAsync(new Bos.Manufacture.ManuGenerateBarcode.GenerateBarcodeBo
                {
                    SiteId = param.SiteId,
                    CodeRuleId = inteCodeRule.Id,
                    Count = 1,
                    Sfcs = param.Barcodes,
                    ProductId = productId,
                    WorkOrderId = workorderId,
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
                
              
                using var trans = TransactionHelper.GetTransactionScope();
                
                //更新 级组条码为完成状态
                await _manuSfcRepository.UpdateStatusAsync(new ManuSfcUpdateCommand
                {
                    SiteId = param.SiteId,
                    Sfcs = param.Barcodes.ToArray(),
                    Status = Core.Enums.SfcStatusEnum.Complete,
                    UpdatedOn =HymsonClock.Now(),
                    UserId = updateName
                });

                //国标码一套表
                var sfcEntity = new ManuSfcEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SiteId = param.SiteId,
                    SFC = targetSFC,
                    Qty = firstsfcproduce.Qty,
                    IsUsed = YesOrNoEnum.Yes,
                    Status = SfcStatusEnum.Activity,
                    CreatedBy = updateName,
                    Type = SfcTypeEnum.Produce,
                    UpdatedBy = updateName
                };
                var sfcInfoEntity  = new ManuSfcInfoEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SiteId = param.SiteId,
                    SfcId = sfcEntity.Id,
                    WorkOrderId = workorderId,
                    ProductId = productId,
                    IsUsed = true,
                    CreatedBy = updateName,
                    UpdatedBy = updateName
                };

                var sfcProduceEntity = new ManuSfcProduceEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SiteId = param.SiteId,
                    SFC = targetSFC,
                    SFCId = sfcEntity.Id,
                    ProductId = productId,
                    WorkOrderId = workorderId,
                    BarCodeInfoId = sfcInfoEntity.Id,
                    ResourceId = firstsfcproduce.ResourceId,
                    EquipmentId = firstsfcproduce.EquipmentId,
                    ProcessRouteId = firstsfcproduce.ProcessRouteId,
                    WorkCenterId = firstsfcproduce.WorkCenterId,
                    ProductBOMId = firstsfcproduce.ProductBOMId,
                    Qty = firstsfcproduce.Qty,
                    ProcedureId = firstsfcproduce.Id,
                    Status = SfcStatusEnum.Activity,
                    RepeatedCount = 0,
                    IsScrap = TrueOrFalseEnum.No,
                    CreatedBy = updateName,
                    UpdatedBy = updateName
                };
                //下达 step
                var sfcStepEntity_create = new ManuSfcStepEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SiteId = param.SiteId,
                    SFC = targetSFC,
                    ProductId = firstsfcproduce.ProductId,
                    WorkOrderId = firstsfcproduce.WorkOrderId,
                    ProductBOMId = firstsfcproduce.ProductBOMId,
                    WorkCenterId = firstsfcproduce.WorkCenterId,
                    EquipmentId = firstsfcproduce.EquipmentId,
                    ResourceId = firstsfcproduce.ResourceId,
                    Qty = firstsfcproduce.Qty,
                    ProcedureId = firstsfcproduce.ProcedureId,
                    Operatetype = ManuSfcStepTypeEnum.Create,
                    CurrentStatus = SfcStatusEnum.lineUp,
                    CreatedBy = updateName,
                    UpdatedBy = updateName
                };
                // 进站step
                //var sfcStepEntity_instation = new ManuSfcStepEntity
                //{
                //    Id = IdGenProvider.Instance.CreateId(),
                //    SiteId = param.SiteId,
                //    SFC = targetSFC,
                //    ProductId = firstsfcproduce.ProductId,
                //    WorkOrderId = firstsfcproduce.WorkOrderId,
                //    ProductBOMId = firstsfcproduce.ProductBOMId,
                //    WorkCenterId = firstsfcproduce.WorkCenterId,
                //    EquipmentId = firstsfcproduce.EquipmentId,
                //    ResourceId = firstsfcproduce.ResourceId,
                //    Qty = firstsfcproduce.Qty,
                //    ProcedureId = firstsfcproduce.ProcedureId,
                //    Operatetype = ManuSfcStepTypeEnum.InStock,
                //    CurrentStatus = SfcStatusEnum.lineUp,
                //    CreatedBy = updateName,
                //    UpdatedBy = updateName
                //};
                //流转记录
                var manuSfcCirculationEntitys = new List<ManuSfcCirculationEntity>();
                foreach (var item in param.Barcodes)
                {
                    var sfcCirculationEntities = await _manuSfcCirculationRepository.GetManuSfcCirculationEntitiesAsync(new ManuSfcCirculationQuery
                    {
                        Sfc = item,
                        SiteId = param.SiteId,
                        CirculationTypes = new List<SfcCirculationTypeEnum>() { SfcCirculationTypeEnum.Merge }.ToArray(),
                        ProcedureId = firstsfcproduce.ProcedureId,
                    });
                    //如果流转记录已经生成 跳过该条码的数据组装
                    if (sfcCirculationEntities != null && sfcCirculationEntities.Any())
                    {
                        continue;
                    }

                    manuSfcCirculationEntitys.Add(new ManuSfcCirculationEntity
                    {
                        Id = IdGenProvider.Instance.CreateId(),
                        SiteId = param.SiteId,
                        ProcedureId = firstsfcproduce.ProcedureId,
                        ResourceId = firstsfcproduce.ResourceId,
                        SFC = item,
                        WorkOrderId = sfcProduceEntity.WorkOrderId,
                        ProductId = sfcProduceEntity.ProductId,
                        CirculationBarCode = targetSFC,
                        CirculationProductId = sfcProduceEntity.ProductId,
                        CirculationMainProductId = sfcProduceEntity.ProductId,
                        CirculationQty = sfcProduceEntity.Qty,
                        CirculationType = SfcCirculationTypeEnum.Merge,
                        CreatedBy = updateName,
                        UpdatedBy = updateName
                    });
                }

                await _manuSfcRepository.InsertAsync(sfcEntity);
                await _manuSfcInfoRepository.InsertAsync(sfcInfoEntity);
                await _manuSfcProduceRepository.InsertAsync(sfcProduceEntity);
                await _manuSfcStepRepository.InsertAsync(sfcStepEntity_create);
                await _manuSfcStepRepository.InsertAsync(sfcStepEntity_instation);
                if (manuSfcCirculationEntitys.Any())
                    await _manuSfcCirculationRepository.InsertRangeAsync(manuSfcCirculationEntitys);
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
