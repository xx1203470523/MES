using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Core.Constants;
using Hymson.MES.CoreServices.Dtos.Parameter;
using Hymson.MES.CoreServices.Services.Parameter;
using Hymson.MES.Data.Repositories.Equipment.EquEquipment;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.Process.Parameter.Query;
using Hymson.MES.Data.Repositories.Process.Resource;
using Hymson.MES.EquipmentServices.Dtos.Parameter;
using Hymson.Utils;
using Hymson.Web.Framework.WorkContext;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.EquipmentServices.Services.Parameter.ProductProcessCollection
{
    /// <summary>
    /// 
    /// </summary>
    public class ProductProcessCollectionService : IProductProcessCollectionService
    {
        private readonly ICurrentEquipment _currentEquipment;

        /// <summary>
        /// 仓储接口（工序维护）
        /// </summary>
        private readonly IProcProcedureRepository _procProcedureRepository;

        /// <summary>
        /// 仓储接口（资源维护）
        /// </summary>
        private readonly IProcResourceRepository _procResourceRepository;

        /// <summary>
        /// 仓储接口（参数维护）
        /// </summary>
        private readonly IProcParameterRepository _procParameterRepository;

        /// <summary>
        /// 产品过程参数
        /// </summary>
        private readonly IManuProductParameterService _manuProductParameterService;

        /// <summary>
        /// 设备过程参数
        /// </summary>
        private readonly IManuEquipmentParameterService _manuEquipmentParameterService;

        /// <summary>
        /// 设备仓储
        /// </summary>
        private readonly IEquEquipmentRepository _equEquipmentRepository;

        /// <summary>
        /// 参数采集
        /// </summary>
        /// <param name="currentEquipment"></param>
        /// <param name="procProcedureRepository"></param>
        /// <param name="procResourceRepository"></param>
        /// <param name="procParameterRepository"></param>
        /// <param name="manuProductParameterService"></param>
        /// <param name="manuEquipmentParameterService"></param>
        /// <param name="equEquipmentRepository"></param>
        public ProductProcessCollectionService(ICurrentEquipment currentEquipment, IProcProcedureRepository
            procProcedureRepository, IProcResourceRepository procResourceRepository, IProcParameterRepository
            procParameterRepository, IManuProductParameterService manuProductParameterService,
            IManuEquipmentParameterService manuEquipmentParameterService, IEquEquipmentRepository equEquipmentRepository)
        {
            _currentEquipment = currentEquipment;
            _procProcedureRepository = procProcedureRepository;
            _procResourceRepository = procResourceRepository;
            _procParameterRepository = procParameterRepository;
            _manuProductParameterService = manuProductParameterService;
            _manuEquipmentParameterService = manuEquipmentParameterService;
            _equEquipmentRepository = equEquipmentRepository;
        }

        /// <summary>
        /// 参数采集
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task CollectionAsync(ProductProcessParameterDto param)
        {
            var resourceEntity = await _procResourceRepository.GetByResourceCodeAsync(new ProcResourceQuery
            {
                ResCode = param.ResourceCode,
                SiteId = _currentEquipment.SiteId
            });

            if (resourceEntity == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES19919)).WithData("ResCode", param.ResourceCode);
            }

            var produreEntity = await _procProcedureRepository.GetProcProdureByResourceIdAsync(new ProcProdureByResourceIdQuery
            {
                ResourceId = resourceEntity.Id,
                SiteId = _currentEquipment.SiteId
            });
            if (produreEntity == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES19101)).WithData("ResCode", param.ResourceCode);
            }

            var parameters = await _procParameterRepository.GetByCodesAsync(new ProcParametersByCodeQuery
            {
                SiteId = _currentEquipment.SiteId,
                Codes = param.Products.SelectMany(x => x.Parameters).Select(x => x.ParameterCode)
            });

            var list = new List<ParameterDto>();
            var errorParameter = new List<string>();
            foreach (var product in param.Products)
            {
                foreach (var parameter in product.Parameters)
                {
                    var parameterEntity = parameters.FirstOrDefault(x => x.ParameterCode == parameter.ParameterCode);
                    if (parameterEntity == null)
                    {
                        errorParameter.Add(parameter.ParameterCode);
                    }
                    else
                    {
                        list.Add(new ParameterDto
                        {
                            SiteId = _currentEquipment.SiteId,
                            SFC = product.SFC,
                            ProcedureId = produreEntity.Id,
                            ParameterId = parameterEntity.Id,
                            ParameterValue = parameter.ParameterValue,
                            CollectionTime = parameter.CollectionTime,
                            UserName = _currentEquipment.Name,
                            Date = HymsonClock.Now()
                        });
                    }
                }
            }
            if (errorParameter.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES19101)).WithData("ParameterCodes", string.Join(",", errorParameter));
            }

            await _manuProductParameterService.InsertRangeAsync(list);
        }

        /// <summary>
        /// 设备参数采集
        /// </summary>
        /// <param name="equipmentProductProcessParameters"></param>
        /// <returns></returns>
        public async Task EquipmentCollectionAsync(IEnumerable<EquipmentProductProcessParameterDto> equipmentProductProcessParameters)
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
                var parameterEntity = parameters.FirstOrDefault(x => x.ParameterCode == item.ParameterCode);
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

            await _manuEquipmentParameterService.CreateEquipmentParameterTable("equ_parameter_wwkkmm");

            if (errorParameter.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES19101)).WithData("ParameterCodes", string.Join(",", errorParameter));
            }

            await _manuEquipmentParameterService.InsertRangeAsync(list);
        }
    }
}
