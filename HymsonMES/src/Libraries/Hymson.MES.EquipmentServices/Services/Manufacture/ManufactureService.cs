using FluentValidation;
using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Core.Constants;
using Hymson.MES.CoreServices.Bos.Manufacture;
using Hymson.MES.CoreServices.Services.Manufacture;
using Hymson.MES.CoreServices.Services.Manufacture.ManuCreateBarcode;
using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.MES.Data.Repositories.Equipment.EquEquipment;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.Process.Resource;
using Hymson.MES.EquipmentServices.Dtos;
using Hymson.Web.Framework.WorkContext;

namespace Hymson.MES.EquipmentServices.Services.Manufacture
{
    /// <summary>
    /// 生产服务
    /// </summary>
    public class ManufactureService : IManufactureService
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
        /// 仓储接口（设备注册）
        /// </summary>
        private readonly IEquEquipmentRepository _equEquipmentRepository;

        /// <summary>
        /// 仓储接口（资源维护）
        /// </summary>
        private readonly IProcResourceRepository _procResourceRepository;

        /// <summary>
        /// 仓储接口（工序维护）
        /// </summary>
        private readonly IProcProcedureRepository _procProcedureRepository;

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
        /// <param name="equEquipmentRepository"></param>
        /// <param name="procResourceRepository"></param>
        /// <param name="procProcedureRepository"></param>
        /// <param name="manuPassStationService"></param>
        /// <param name="manuCreateBarcodeService"></param>
        public ManufactureService(ICurrentEquipment currentEquipment,
            AbstractValidator<InBoundDto> validationInBoundDtoRules,
            AbstractValidator<InBoundMoreDto> validationInBoundMoreDtoRules,
            AbstractValidator<OutBoundDto> validationOutBoundDtoRules,
            AbstractValidator<OutBoundMoreDto> validationOutBoundMoreDtoRules,
            IEquEquipmentRepository equEquipmentRepository,
            IProcResourceRepository procResourceRepository,
            IProcProcedureRepository procProcedureRepository,
            IManuPassStationService manuPassStationService,
            IManuCreateBarcodeService manuCreateBarcodeService)
        {
            _currentEquipment = currentEquipment;
            _validationInBoundDtoRules = validationInBoundDtoRules;
            _validationInBoundMoreDtoRules = validationInBoundMoreDtoRules;
            _validationOutBoundDtoRules = validationOutBoundDtoRules;
            _validationOutBoundMoreDtoRules = validationOutBoundMoreDtoRules;
            _equEquipmentRepository = equEquipmentRepository;
            _procResourceRepository = procResourceRepository;
            _procProcedureRepository = procProcedureRepository;
            _manuPassStationService = manuPassStationService;
            _manuCreateBarcodeService = manuCreateBarcodeService;
        }


        /// <summary>
        /// 创建条码
        /// </summary>
        /// <param name="baseDto"></param>
        /// <returns></returns>
        public async Task<IEnumerable<string>> CreateBarcodeBySemiProductIdAsync(BaseDto baseDto)
        {
            var manuSFCEntities = await _manuCreateBarcodeService.CreateBarcodeBySemiProductIdAsync(new CoreServices.Bos.Manufacture.ManuCreateBarcode.CreateBarcodeBySemiProductId
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

            var manuBo = await GetManufactureBoAsync(new ManufactureRequestBo
            {
                SiteId = _currentEquipment.SiteId,
                ResourceCode = request.ResourceCode,
                EquipmentCode = request.EquipmentCode
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
            });
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

            var manuBo = await GetManufactureBoAsync(new ManufactureRequestBo
            {
                SiteId = _currentEquipment.SiteId,
                ResourceCode = request.ResourceCode,
                EquipmentCode = request.EquipmentCode
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
            });
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

            var manuBo = await GetManufactureBoAsync(new ManufactureRequestBo
            {
                SiteId = _currentEquipment.SiteId,
                ResourceCode = request.ResourceCode,
                EquipmentCode = request.EquipmentCode
            });
            if (manuBo == null) return;

            _ = await _manuPassStationService.OutStationRangeBySFCAsync(new SFCOutStationBo
            {
                SiteId = _currentEquipment.SiteId,
                UserName = _currentEquipment.Name,
                ProcedureId = manuBo.ProcedureId,
                ResourceId = manuBo.ResourceId,
                EquipmentId = manuBo.EquipmentId,
                SFCs = new string[] { request.SFC }
            });
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

            var manuBo = await GetManufactureBoAsync(new ManufactureRequestBo
            {
                SiteId = _currentEquipment.SiteId,
                ResourceCode = request.ResourceCode,
                EquipmentCode = request.EquipmentCode
            });
            if (manuBo == null) return;

            _ = await _manuPassStationService.OutStationRangeBySFCAsync(new SFCOutStationBo
            {
                SiteId = _currentEquipment.SiteId,
                UserName = _currentEquipment.Name,
                ProcedureId = manuBo.ProcedureId,
                ResourceId = manuBo.ResourceId,
                EquipmentId = manuBo.EquipmentId,
                SFCs = request.SFCs.Select(s => s.SFC)
            });
        }

        /// <summary>
        /// 载具进站
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task InBoundVehicleAsync(InBoundVehicleDto request)
        {
            //await _validationInBoundDtoRules.ValidateAndThrowAsync(request);
            if (request == null) throw new CustomerValidationException(nameof(ErrorCode.MES10100));

            var manuBo = await GetManufactureBoAsync(new ManufactureRequestBo
            {
                SiteId = _currentEquipment.SiteId,
                ResourceCode = request.ResourceCode,
                EquipmentCode = request.EquipmentCode
            });
            if (manuBo == null) return;

            _ = await _manuPassStationService.InStationRangeByVehicleAsync(new VehicleInStationBo
            {
                SiteId = _currentEquipment.SiteId,
                UserName = _currentEquipment.Name,
                ProcedureId = manuBo.ProcedureId,
                ResourceId = manuBo.ResourceId,
                EquipmentId = manuBo.EquipmentId,
                VehicleCodes = new string[] { request.VehicleCode }
            });
        }

        /// <summary>
        /// 载具出站
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task OutBoundVehicleAsync(OutBoundCarrierDto request)
        {
            //await _validationOutBoundDtoRules.ValidateAndThrowAsync(request);
            if (request == null) throw new CustomerValidationException(nameof(ErrorCode.MES10100));

            var manuBo = await GetManufactureBoAsync(new ManufactureRequestBo
            {
                SiteId = _currentEquipment.SiteId,
                ResourceCode = request.ResourceCode,
                EquipmentCode = request.EquipmentCode
            });
            if (manuBo == null) return;

            _ = await _manuPassStationService.OutStationRangeByVehicleAsync(new VehicleOutStationBo
            {
                SiteId = _currentEquipment.SiteId,
                UserName = _currentEquipment.Name,
                ProcedureId = manuBo.ProcedureId,
                ResourceId = manuBo.ResourceId,
                EquipmentId = manuBo.EquipmentId,
                VehicleCodes = new string[] { request.CarrierNo }
            });
        }



        #region 内部方法
        /// <summary>
        /// 获取当前生产对象
        /// </summary>
        /// <param name="requestBo"></param>
        /// <returns></returns>
        private async Task<ManufactureResponseBo> GetManufactureBoAsync(ManufactureRequestBo requestBo)
        {
            // 查询资源
            var resourceEntity = await _procResourceRepository.GetByCodeAsync(new EntityByCodeQuery
            {
                Site = requestBo.SiteId,
                Code = requestBo.ResourceCode
            }) ?? throw new CustomerValidationException(nameof(ErrorCode.MES19919)).WithData("ResCode", requestBo.ResourceCode);

            // 根据设备
            var equipmentEntity = await _equEquipmentRepository.GetByCodeAsync(new EntityByCodeQuery
            {
                Site = requestBo.SiteId,
                Code = requestBo.EquipmentCode
            }) ?? throw new CustomerValidationException(nameof(ErrorCode.MES19005)).WithData("Code", requestBo.EquipmentCode);

            // 读取设备绑定的资源
            var resourceBindEntities = await _procResourceRepository.GetByEquipmentCodeAsync(new ProcResourceQuery
            {
                SiteId = requestBo.SiteId,
                EquipmentCode = requestBo.EquipmentCode
            });
            if (resourceBindEntities == null || !resourceBindEntities.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES19131))
                    .WithData("ResCode", requestBo.ResourceCode)
                    .WithData("EquCode", requestBo.EquipmentCode);
            }

            // 读取资源对应的工序
            var procProcedureEntity = await _procProcedureRepository.GetProcProdureByResourceIdAsync(new ProcProdureByResourceIdQuery
            {
                SiteId = _currentEquipment.SiteId,
                ResourceId = resourceEntity.Id
            }) ?? throw new CustomerValidationException(nameof(ErrorCode.MES19913)).WithData("ResCode", requestBo.ResourceCode);

            return new ManufactureResponseBo
            {
                ResourceId = resourceEntity.Id,
                ProcedureId = procProcedureEntity.Id,
                EquipmentId = equipmentEntity.Id
            };
        }

        #endregion
    }

    /// <summary>
    /// 当前生成对象
    /// </summary>
    public class ManufactureRequestBo
    {
        /// <summary>
        /// 站点ID
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 资源编码
        /// </summary>
        public string ResourceCode { get; set; } = "";

        /// <summary>
        /// 设备编码
        /// </summary>
        public string EquipmentCode { get; set; } = "";
    }

    /// <summary>
    /// 当前生成对象
    /// </summary>
    public class ManufactureResponseBo
    {
        /// <summary>
        /// 资源ID
        /// </summary>
        public long ResourceId { get; set; }

        /// <summary>
        /// 设备ID
        /// </summary>
        public long EquipmentId { get; set; }

        /// <summary>
        /// 工序ID
        /// </summary>
        public long ProcedureId { get; set; }

    }
}