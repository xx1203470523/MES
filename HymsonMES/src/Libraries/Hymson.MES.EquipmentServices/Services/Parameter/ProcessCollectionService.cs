using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.CoreServices.Bos.Manufacture;
using Hymson.MES.CoreServices.Bos.Parameter;
using Hymson.MES.CoreServices.Dtos.Parameter;
using Hymson.MES.CoreServices.Services.Common;
using Hymson.MES.CoreServices.Services.Parameter;
using Hymson.MES.Data.Repositories.Equipment.EquEquipment;
using Hymson.MES.Data.Repositories.Parameter;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.Process.Query;
using Hymson.MES.EquipmentServices.Dtos.Parameter;
using Hymson.Utils;
using Hymson.Web.Framework.WorkContext;
using System.Net.NetworkInformation;

namespace Hymson.MES.EquipmentServices.Services.Parameter.ProcessCollection
{
    /// <summary>
    /// 
    /// </summary>
    public class ProcessCollectionService : IProcessCollectionService
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly ICurrentEquipment _currentEquipment;

        /// <summary>
        /// 服务接口（生产通用）
        /// </summary>
        private readonly IManuCommonService _manuCommonService;

        /// <summary>
        /// 接口（参数收集）
        /// </summary>
        private readonly IManuProductParameterService _manuProductParameterService;

        /// <summary>
        /// 仓储接口（参数维护）
        /// </summary>
        private readonly IProcParameterRepository _procParameterRepository;

        /// <summary>
        /// 设备过程参数
        /// </summary>
        private readonly IManuEquipmentParameterService _manuEquipmentParameterService;

        /// <summary>
        /// 设备仓储
        /// </summary>
        private readonly IEquEquipmentRepository _equEquipmentRepository;

        /// <summary>
        /// 参数仓储
        /// </summary>
        private readonly IManuEquipmentParameterRepository _manuEquipmentParameterRepository;

        /// <summary>
        /// 参数仓储
        /// </summary>
        private readonly IManuProductParameterRepository _manuProductParameterRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="currentEquipment"></param>
        /// <param name="manuCommonService"></param>
        /// <param name="manuProductParameterService"></param>
        /// <param name="procParameterRepository"></param>
        /// <param name="manuEquipmentParameterService"></param>
        /// <param name="equEquipmentRepository"></param>
        /// <param name="manuEquipmentParameterRepository"></param>
        /// <param name="manuProductParameterRepository"></param>
        public ProcessCollectionService(ICurrentEquipment currentEquipment,
            IManuCommonService manuCommonService,
            IManuProductParameterService manuProductParameterService,
            IProcParameterRepository procParameterRepository,
            IManuEquipmentParameterService manuEquipmentParameterService,
            IEquEquipmentRepository equEquipmentRepository,
            IManuEquipmentParameterRepository manuEquipmentParameterRepository,
            IManuProductParameterRepository manuProductParameterRepository)
        {
            _currentEquipment = currentEquipment;
            _manuCommonService = manuCommonService;
            _manuProductParameterService = manuProductParameterService;
            _procParameterRepository = procParameterRepository;
            _manuEquipmentParameterService = manuEquipmentParameterService;
            _equEquipmentRepository = equEquipmentRepository;
            _manuEquipmentParameterRepository = manuEquipmentParameterRepository;
            _manuProductParameterRepository = manuProductParameterRepository;
        }


        /// <summary>
        /// 参数采集（产品）
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task ProductCollectionAsync(ProductProcessParameterDto request)
        {
            if (request == null) throw new CustomerValidationException(nameof(ErrorCode.MES10100));

            // 根据载具代码获取载具里面的条码
            if (!request.SFCs.Any()) throw new CustomerValidationException(nameof(ErrorCode.MES16312));

            var manuBo = await _manuCommonService.GetManufactureBoAsync(new ManufactureRequestBo
            {
                SiteId = _currentEquipment.SiteId,
                ResourceCode = request.ResourceCode,
                EquipmentCode = _currentEquipment.Code
            });
            if (manuBo == null) return;

            List<string> SFCs = new();
            switch (request.Type)
            {
                case ManuFacePlateBarcodeTypeEnum.Product:
                    SFCs = request.SFCs.ToList();
                    break;
                case ManuFacePlateBarcodeTypeEnum.Vehicle:
                    var vehicleSFCs = await _manuCommonService.GetSFCsByVehicleCodesAsync(new VehicleSFCRequestBo
                    {
                        SiteId = _currentEquipment.SiteId,
                        VehicleCodes = request.SFCs
                    });
                    SFCs = vehicleSFCs.Select(s => s.SFC).ToList();
                    break;
                default:
                    break;
            }

            _ = await _manuProductParameterService.ProductProcessCollectAsync(new ProductProcessParameterBo
            {
                SiteId = _currentEquipment.SiteId,
                UserName = _currentEquipment.Name,
                Time = HymsonClock.Now(),
                ProcedureId = manuBo.ProcedureId,
                ResourceId = manuBo.ResourceId,
                SFCs = SFCs,
                Parameters = request.Parameters
            });
        }

        /// <summary>
        /// 参数采集（设备）
        /// </summary>
        /// <param name="equipmentProductProcessParameters"></param>
        /// <returns></returns>
        public async Task EquipmentCollectionAsync(IEnumerable<EquipmentProcessParameterDto> equipmentProductProcessParameters)
        {
            var equipmentEntity = await _equEquipmentRepository.GetByEquipmentCodeAsync(new Data.Repositories.Common.Query.EntityByCodeQuery
            {
                Site = _currentEquipment.SiteId,
                Code = _currentEquipment.Code,
            });

            if (equipmentEntity == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES19934)).WithData("EquipmentCode", _currentEquipment.Code);
            }
            var parameters = await _procParameterRepository.GetByCodesAsync(new ProcParametersByCodeQuery
            {
                SiteId = _currentEquipment.SiteId,
                Codes = equipmentProductProcessParameters.Select(x => x.ParameterCode)
            });

            var list = new List<EquipmentParameterDto>();
            var errorParameter = new List<string>();
            foreach (var item in equipmentProductProcessParameters)
            {
                var parameterEntity = parameters.FirstOrDefault(x => item.ParameterCode.Equals(x.ParameterCode, StringComparison.OrdinalIgnoreCase));
                if (parameterEntity == null)
                {
                    errorParameter.Add(item.ParameterCode);
                    continue;
                }
                list.Add(
                    new EquipmentParameterDto
                    {
                        SiteId = _currentEquipment.SiteId,
                        EquipmentId = equipmentEntity.Id,
                        ParameterId = parameterEntity.Id,
                        ParameterValue = item.ParameterValue,
                        CollectionTime = item.CollectionTime,
                        UserName = _currentEquipment.Name,
                        Date = HymsonClock.Now()
                    });
            }

            if (errorParameter.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES19606)).WithData("ParameterCodes", string.Join(",", errorParameter));
            }

            await _manuEquipmentParameterService.InsertRangeAsync(list);
        }

        /// <summary>
        /// 获取参数表名
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<List<string>> GetParamTableNameAsync(GetParamNameDto dto)
        {
            if(dto.SiteId == 0)
            {
                dto.SiteId = 42874561778253824;
            }
            List<string> resultList = new List<string>();
            var name = await _manuEquipmentParameterRepository.GetParamTableName(dto.SiteId, dto.EquipmentId);
            var nameList = _manuProductParameterRepository.GetParamTableName(dto.SiteId, dto.ProcedureId, dto.Sfc);
            resultList.AddRange(nameList);
            resultList.Add(name);
            return resultList;
        }
    }
}
