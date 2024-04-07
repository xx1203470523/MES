using FluentValidation;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Authentication;
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
using Hymson.Infrastructure;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Plan;
using Hymson.MES.Services.Dtos.Plan;
using Hymson.MES.Services.Dtos.Manufacture.ManuSfcOperateDto;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Data.Repositories.Process;

namespace Hymson.MES.Services.Services.Manufacture
{
    /// <summary>
    /// 生产服务
    /// </summary>
    public class ManuSfcOperateService : IManuSfcOperateService
    {
        private readonly ICurrentUser _currentUser;
        private readonly ICurrentSite _currentSite;

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
        private readonly IManuSfcProduceRepository _manuSfcProduceRepository;
        private readonly IProcMaterialRepository _procMaterialRepository;
        private readonly IPlanWorkOrderRepository _planWorkOrderRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="currentSite"></param>
        /// <param name="validationInBoundDtoRules"></param>
        /// <param name="validationInBoundMoreDtoRules"></param>
        /// <param name="validationOutBoundDtoRules"></param>
        /// <param name="validationOutBoundMoreDtoRules"></param>
        /// <param name="manuCommonService"></param>
        /// <param name="manuPassStationService"></param>
        /// <param name="manuCreateBarcodeService"></param>
        /// <param name="manuSfcProduceRepository"></param>
        /// <param name="procMaterialRepository"></param>
        /// <param name="planWorkOrderRepository"></param>
        public ManuSfcOperateService(
            AbstractValidator<InBoundDto> validationInBoundDtoRules,
            AbstractValidator<InBoundMoreDto> validationInBoundMoreDtoRules,
            AbstractValidator<OutBoundDto> validationOutBoundDtoRules,
            AbstractValidator<OutBoundMoreDto> validationOutBoundMoreDtoRules,
            IManuCommonService manuCommonService,
            IManuPassStationService manuPassStationService,
            IManuCreateBarcodeService manuCreateBarcodeService,
            ICurrentUser currentUser,
            ICurrentSite currentSite,
            IManuSfcProduceRepository manuSfcProduceRepository,
            IProcMaterialRepository procMaterialRepository,
            IPlanWorkOrderRepository planWorkOrderRepository)
        {
            _validationInBoundDtoRules = validationInBoundDtoRules;
            _validationInBoundMoreDtoRules = validationInBoundMoreDtoRules;
            _validationOutBoundDtoRules = validationOutBoundDtoRules;
            _validationOutBoundMoreDtoRules = validationOutBoundMoreDtoRules;
            _manuCommonService = manuCommonService;
            _manuPassStationService = manuPassStationService;
            _manuCreateBarcodeService = manuCreateBarcodeService;
            _currentUser = currentUser;
            _currentSite = currentSite;
            _manuSfcProduceRepository = manuSfcProduceRepository;
            _procMaterialRepository = procMaterialRepository;
            _planWorkOrderRepository = planWorkOrderRepository;
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
                SiteId = _currentSite.SiteId??0,
                UserName = _currentUser.UserName,
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
                SiteId =  _currentSite.SiteId??0,
                UserName = _currentUser.UserName,
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
                SiteId =  _currentSite.SiteId??0,
                ResourceCode = request.ResourceCode,
                EquipmentCode = request.EquipmentCode
            });
            if (manuBo == null) return;

            _ = await _manuPassStationService.InStationRangeBySFCAsync(new SFCInStationBo
            {
                SiteId =  _currentSite.SiteId??0,
                UserName = _currentUser.UserName,
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
                SiteId =  _currentSite.SiteId??0,
                ResourceCode = request.ResourceCode,
                EquipmentCode = request.EquipmentCode
            });
            if (manuBo == null) return;

            _ = await _manuPassStationService.InStationRangeBySFCAsync(new SFCInStationBo
            {
                SiteId =  _currentSite.SiteId??0,
                UserName = _currentUser.UserName,
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
                SiteId =  _currentSite.SiteId??0,
                ResourceCode = request.ResourceCode,
                EquipmentCode = request.EquipmentCode
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
                SiteId =  _currentSite.SiteId??0,
                UserName = _currentUser.UserName,
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
                SiteId =  _currentSite.SiteId??0,
                ResourceCode = request.ResourceCode,
                EquipmentCode = request.EquipmentCode
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
                SiteId =  _currentSite.SiteId??0,
                UserName = _currentUser.UserName,
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
                SiteId =  _currentSite.SiteId??0,
                ResourceCode = request.ResourceCode,
                EquipmentCode = request.EquipmentCode
            });
            if (manuBo == null) return;

            _ = await _manuPassStationService.InStationRangeByVehicleAsync(new VehicleInStationBo
            {
                SiteId =  _currentSite.SiteId??0,
                UserName = _currentUser.UserName,
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
                SiteId =  _currentSite.SiteId??0,
                ResourceCode = request.ResourceCode,
                EquipmentCode = request.EquipmentCode
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
                SiteId =  _currentSite.SiteId??0,
                UserName = _currentUser.UserName,
                ProcedureId = manuBo.ProcedureId,
                ResourceId = manuBo.ResourceId,
                EquipmentId = manuBo.EquipmentId,
                OutStationRequestBos = new List<OutStationRequestBo> { outStationRequestBo }
            }, RequestSourceEnum.EquipmentApi);
        }


        /// <summary>
        /// 分页查询列表（PDA条码出站）
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ManuSfcInstationPagedQueryOutputDto>> GetPagedListAsync(ManuSfcInstationPagedQueryDto pagedQueryDto)
        {
            var defaultResult = new PagedInfo<ManuSfcInstationPagedQueryOutputDto>(Enumerable.Empty<ManuSfcInstationPagedQueryOutputDto>(), 0, 0, 0);

            var queryData = pagedQueryDto.ToQuery<ManuSfcProduceVehiclePagedQuery>();
            queryData.SiteId= _currentSite.SiteId??0;
            queryData.Status = SfcStatusEnum.lineUp;
            var pageInfo = await _manuSfcProduceRepository.GetManuSfcPageListAsync(queryData);
            if (pageInfo.Data == null || !pageInfo.Data.Any()) { 
                return defaultResult;
            }

            //获取物料信息
            var materialIds = pageInfo.Data.Select(a => a.ProductId).Distinct();
            var materialEntities = await _procMaterialRepository.GetByIdsAsync(materialIds);

            //获取工单信息
            var workOrderIds = pageInfo.Data.Select(a => a.WorkOrderId).Distinct();
            var planWorkOrderEntities = await _planWorkOrderRepository.GetByIdsAsync(workOrderIds);

            var result = new List<ManuSfcInstationPagedQueryOutputDto>();

            foreach (var item in pageInfo.Data) {
                var model = new ManuSfcInstationPagedQueryOutputDto();
                model.SFC = item.Sfc;
                model.Status = item.Status;
                model.Qty = item.Qty;

                var materialEntity = materialEntities.FirstOrDefault(a => a.Id == item.ProcedureId);
                model.MaterialCode = materialEntity?.MaterialCode??"";

                var planWorkOrderEntity= planWorkOrderEntities.FirstOrDefault(a=>a.Id== item.WorkOrderId);
                model.OrderCode = planWorkOrderEntity?.OrderCode ?? "";

                result.Add(model);
            }

            return new PagedInfo<ManuSfcInstationPagedQueryOutputDto>(result, pageInfo.PageIndex, pageInfo.PageSize, pageInfo.TotalCount);
        }
    }
}