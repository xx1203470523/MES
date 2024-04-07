using FluentValidation;
using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Enums;
using Hymson.MES.CoreServices.Bos.Job;
using Hymson.MES.CoreServices.Bos.Manufacture;
using Hymson.MES.CoreServices.Bos.Manufacture.ManuCreateBarcode;
using Hymson.MES.CoreServices.Bos.Parameter;
using Hymson.MES.CoreServices.Services.Common;
using Hymson.MES.CoreServices.Services.Manufacture;
using Hymson.MES.CoreServices.Services.Manufacture.ManuCreateBarcode;
using Hymson.MES.Services.Dtos.Manufacture.ManuSfcOperate;
using Hymson.Web.Framework.WorkContext;

namespace Hymson.MES.Services.Services.Manufacture
{
    /// <summary>
    /// 生产服务
    /// </summary>
    public class ManuSfcOperateService : IManuSfcOperateService
    {
        /// <summary>
        /// 当前设备对象
        /// </summary>
        private readonly ICurrentEquipment _currentEquipment;

        /// <summary>
        /// 验证器
        /// </summary>
        private readonly AbstractValidator<InBoundDto> _validationInBoundDtoRules;
        private readonly AbstractValidator<InBoundMoreDto> _validationInBoundMoreDtoRules;
        private readonly AbstractValidator<OutBoundDto> _validationOutBoundDtoRules;
        private readonly AbstractValidator<OutBoundMoreDto> _validationOutBoundMoreDtoRules;

        /// <summary>
        /// 服务接口（生产通用）
        /// </summary>
        private readonly IManuCommonService _manuCommonService;

        /// <summary>
        /// 服务接口（过站）
        /// </summary>
        private readonly IManuPassStationService _manuPassStationService;

        /// <summary>
        /// 业务接口（创建条码服务）
        /// </summary>
        private readonly IManuCreateBarcodeService _manuCreateBarcodeService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="currentEquipment"></param>
        /// <param name="validationInBoundDtoRules"></param>
        /// <param name="validationInBoundMoreDtoRules"></param>
        /// <param name="validationOutBoundDtoRules"></param>
        /// <param name="validationOutBoundMoreDtoRules"></param>
        /// <param name="manuCommonService"></param>
        /// <param name="manuPassStationService"></param>
        /// <param name="manuCreateBarcodeService"></param>
        public ManuSfcOperateService(ICurrentEquipment currentEquipment,
            AbstractValidator<InBoundDto> validationInBoundDtoRules,
            AbstractValidator<InBoundMoreDto> validationInBoundMoreDtoRules,
            AbstractValidator<OutBoundDto> validationOutBoundDtoRules,
            AbstractValidator<OutBoundMoreDto> validationOutBoundMoreDtoRules,
            IManuCommonService manuCommonService,
            IManuPassStationService manuPassStationService,
            IManuCreateBarcodeService manuCreateBarcodeService)
        {
            _currentEquipment = currentEquipment;
            _validationInBoundDtoRules = validationInBoundDtoRules;
            _validationInBoundMoreDtoRules = validationInBoundMoreDtoRules;
            _validationOutBoundDtoRules = validationOutBoundDtoRules;
            _validationOutBoundMoreDtoRules = validationOutBoundMoreDtoRules;
            _manuCommonService = manuCommonService;
            _manuPassStationService = manuPassStationService;
            _manuCreateBarcodeService = manuCreateBarcodeService;
        }

        /// <summary>
        /// 创建条码（半成品）
        /// </summary>
        /// <param name="baseDto"></param>
        /// <returns></returns>
        public async Task<IEnumerable<string>> CreateBarcodeBySemiProductIdAsync(BaseDto baseDto)
        {
            var manuSFCEntities = await _manuCreateBarcodeService.CreateBarcodeBySemiProductIdAsync(new CreateBarcodeByResourceCode
            {
                SiteId = _currentEquipment.SiteId,
                UserName = _currentEquipment.Name,
                ResourceCode = baseDto.ResourceCode
            });

            if (manuSFCEntities == null || !manuSFCEntities.Any()) return Enumerable.Empty<string>();
            return manuSFCEntities.Select(s => s.SFC);
        }

        /// <summary>
        /// 创建条码（电芯）
        /// </summary>
        /// <param name="baseDto"></param>
        /// <returns></returns>
        public async Task<IEnumerable<string>> CreateCellBarCodeAsync(BaseDto baseDto)
        {
            var manuSFCEntities = await _manuCreateBarcodeService.CreateCellBarCodeAsync(new CreateBarcodeByResourceCode
            {
                SiteId = _currentEquipment.SiteId,
                UserName = _currentEquipment.Name,
                ResourceCode = baseDto.ResourceCode
            });

            if (manuSFCEntities == null || !manuSFCEntities.Any()) return Enumerable.Empty<string>();
            return manuSFCEntities.Select(s => s.SFC);
        }

        /// <summary>
        /// 进站
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task InBoundAsync(InBoundDto request)
        {
            await _validationInBoundDtoRules.ValidateAndThrowAsync(request);
            if (request == null) throw new CustomerValidationException(nameof(ErrorCode.MES10100));

            var manuBo = await _manuCommonService.GetManufactureBoAsync(new ManufactureRequestBo
            {
                SiteId = _currentEquipment.SiteId,
                ResourceCode = request.ResourceCode,
                EquipmentCode = _currentEquipment.Code
            });
            if (manuBo == null) return;

            _ = await _manuPassStationService.InStationRangeBySFCAsync(new SFCInStationBo
            {
                SiteId = _currentEquipment.SiteId,
                UserName = _currentEquipment.Name,
                ProcedureId = manuBo.ProcedureId,
                ResourceId = manuBo.ResourceId,
                EquipmentId = manuBo.EquipmentId,
                SFCs = new string[] { request.SFC }
            }, RequestSourceEnum.EquipmentApi);
        }

        /// <summary>
        /// 进站（多个）
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task InBoundMoreAsync(InBoundMoreDto request)
        {
            await _validationInBoundMoreDtoRules.ValidateAndThrowAsync(request);
            if (request == null) throw new CustomerValidationException(nameof(ErrorCode.MES10100));
            if (!request.SFCs.Any()) throw new CustomerValidationException(nameof(ErrorCode.MES19101));

            var manuBo = await _manuCommonService.GetManufactureBoAsync(new ManufactureRequestBo
            {
                SiteId = _currentEquipment.SiteId,
                ResourceCode = request.ResourceCode,
                EquipmentCode = _currentEquipment.Code
            });
            if (manuBo == null) return;

            _ = await _manuPassStationService.InStationRangeBySFCAsync(new SFCInStationBo
            {
                SiteId = _currentEquipment.SiteId,
                UserName = _currentEquipment.Name,
                ProcedureId = manuBo.ProcedureId,
                ResourceId = manuBo.ResourceId,
                EquipmentId = manuBo.EquipmentId,
                SFCs = request.SFCs.Select(s => s.SFC)
            }, RequestSourceEnum.EquipmentApi);
        }

        /// <summary>
        /// 出站
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task OutBoundAsync(OutBoundDto request)
        {
            await _validationOutBoundDtoRules.ValidateAndThrowAsync(request);
            if (request == null) throw new CustomerValidationException(nameof(ErrorCode.MES10100));

            var manuBo = await _manuCommonService.GetManufactureBoAsync(new ManufactureRequestBo
            {
                SiteId = _currentEquipment.SiteId,
                ResourceCode = request.ResourceCode,
                EquipmentCode = _currentEquipment.Code
            });
            if (manuBo == null) return;

            var outStationRequestBo = new OutStationRequestBo
            {
                SFC = request.SFC,
                IsQualified = request.IsQualified == 1
            };

            // 消耗条码
            if (request.ConsumeCodes != null && request.ConsumeCodes.Any())
            {
                outStationRequestBo.ConsumeList = request.ConsumeCodes.Select(s => new OutStationConsumeBo { BarCode = s });
            }

            // 不合格代码
            if (request.FailInfo != null && request.FailInfo.Any())
            {
                outStationRequestBo.OutStationUnqualifiedList = request.FailInfo.Select(s => new OutStationUnqualifiedBo { UnqualifiedCode = s.NCCode });
            }

            _ = await _manuPassStationService.OutStationRangeBySFCAsync(new SFCOutStationBo
            {
                SiteId = _currentEquipment.SiteId,
                UserName = _currentEquipment.Name,
                ProcedureId = manuBo.ProcedureId,
                ResourceId = manuBo.ResourceId,
                EquipmentId = manuBo.EquipmentId,
                OutStationRequestBos = new List<OutStationRequestBo> { outStationRequestBo }
            }, RequestSourceEnum.EquipmentApi);
        }

        /// <summary>
        /// 出站（多个）
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task OutBoundMoreAsync(OutBoundMoreDto request)
        {
            await _validationOutBoundMoreDtoRules.ValidateAndThrowAsync(request);
            if (request == null) throw new CustomerValidationException(nameof(ErrorCode.MES10100));
            if (request.SFCs.Length <= 0) throw new CustomerValidationException(nameof(ErrorCode.MES19101));

            var manuBo = await _manuCommonService.GetManufactureBoAsync(new ManufactureRequestBo
            {
                SiteId = _currentEquipment.SiteId,
                ResourceCode = request.ResourceCode,
                EquipmentCode = _currentEquipment.Code
            });
            if (manuBo == null) return;

            List<OutStationRequestBo> outStationRequestBos = new();
            foreach (var item in request.SFCs)
            {
                var outStationRequestBo = new OutStationRequestBo
                {
                    SFC = item.SFC,
                    IsQualified = item.IsQualified == 1
                };

                // 消耗条码
                if (item.ConsumeCodes != null && item.ConsumeCodes.Any())
                {
                    outStationRequestBo.ConsumeList = item.ConsumeCodes.Select(s => new OutStationConsumeBo { BarCode = s });
                }

                // 不合格代码
                if (item.FailInfo != null && item.FailInfo.Any())
                {
                    outStationRequestBo.OutStationUnqualifiedList = item.FailInfo.Select(s => new OutStationUnqualifiedBo { UnqualifiedCode = s.NCCode });
                }

                outStationRequestBos.Add(outStationRequestBo);
            }

            _ = await _manuPassStationService.OutStationRangeBySFCAsync(new SFCOutStationBo
            {
                SiteId = _currentEquipment.SiteId,
                UserName = _currentEquipment.Name,
                ProcedureId = manuBo.ProcedureId,
                ResourceId = manuBo.ResourceId,
                EquipmentId = manuBo.EquipmentId,
                OutStationRequestBos = outStationRequestBos
            }, RequestSourceEnum.EquipmentApi);
        }

        /// <summary>
        /// 载具进站
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task InBoundCarrierAsync(InBoundCarrierDto request)
        {
            //await _validationInBoundDtoRules.ValidateAndThrowAsync(request);
            if (request == null) throw new CustomerValidationException(nameof(ErrorCode.MES10100));

            var manuBo = await _manuCommonService.GetManufactureBoAsync(new ManufactureRequestBo
            {
                SiteId = _currentEquipment.SiteId,
                ResourceCode = request.ResourceCode,
                EquipmentCode = _currentEquipment.Code
            });
            if (manuBo == null) return;

            _ = await _manuPassStationService.InStationRangeByVehicleAsync(new VehicleInStationBo
            {
                SiteId = _currentEquipment.SiteId,
                UserName = _currentEquipment.Name,
                ProcedureId = manuBo.ProcedureId,
                ResourceId = manuBo.ResourceId,
                EquipmentId = manuBo.EquipmentId,
                VehicleCodes = new string[] { request.CarrierNo }
            }, RequestSourceEnum.EquipmentApi);
        }

        /// <summary>
        /// 载具出站
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task OutBoundCarrierAsync(OutBoundCarrierDto request)
        {
            //await _validationOutBoundDtoRules.ValidateAndThrowAsync(request);
            if (request == null) throw new CustomerValidationException(nameof(ErrorCode.MES10100));

            var manuBo = await _manuCommonService.GetManufactureBoAsync(new ManufactureRequestBo
            {
                SiteId = _currentEquipment.SiteId,
                ResourceCode = request.ResourceCode,
                EquipmentCode = _currentEquipment.Code
            });
            if (manuBo == null) return;

            var outStationRequestBo = new OutStationRequestBo
            {
                VehicleCode = request.CarrierNo,
                IsQualified = request.IsQualified == 1
            };

            // 消耗条码
            if (request.ConsumeCodes != null && request.ConsumeCodes.Any())
            {
                outStationRequestBo.ConsumeList = request.ConsumeCodes.Select(s => new OutStationConsumeBo { BarCode = s });
            }

            // 不合格代码
            if (request.FailInfo != null && request.FailInfo.Any())
            {
                outStationRequestBo.OutStationUnqualifiedList = request.FailInfo.Select(s => new OutStationUnqualifiedBo { UnqualifiedCode = s.NCCode });
            }

            _ = await _manuPassStationService.OutStationRangeByVehicleAsync(new VehicleOutStationBo
            {
                SiteId = _currentEquipment.SiteId,
                UserName = _currentEquipment.Name,
                ProcedureId = manuBo.ProcedureId,
                ResourceId = manuBo.ResourceId,
                EquipmentId = manuBo.EquipmentId,
                OutStationRequestBos = new List<OutStationRequestBo> { outStationRequestBo }
            }, RequestSourceEnum.EquipmentApi);
        }

    }
}