using FluentValidation;
using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Core.Constants;
using Hymson.MES.CoreServices.Bos.Job;
using Hymson.MES.CoreServices.Bos.Parameter;
using Hymson.MES.CoreServices.Services.Job.JobUtility.Execute;
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
        /// 业务接口（创建条码服务）
        /// </summary>
        private readonly IManuCreateBarcodeService _manuCreateBarcodeService;

        /// <summary>
        /// 服务接口
        /// </summary>
        private readonly IExecuteJobService<JobRequestBo> _executeJobService;

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
        /// <param name="manuCreateBarcodeService"></param>
        /// <param name="executeJobService"></param>
        public ManufactureService(ICurrentEquipment currentEquipment,
            AbstractValidator<InBoundDto> validationInBoundDtoRules,
            AbstractValidator<InBoundMoreDto> validationInBoundMoreDtoRules,
            AbstractValidator<OutBoundDto> validationOutBoundDtoRules,
            AbstractValidator<OutBoundMoreDto> validationOutBoundMoreDtoRules,
            IEquEquipmentRepository equEquipmentRepository,
            IProcResourceRepository procResourceRepository,
            IProcProcedureRepository procProcedureRepository,
            IManuCreateBarcodeService manuCreateBarcodeService,
            IExecuteJobService<JobRequestBo> executeJobService)
        {
            _currentEquipment = currentEquipment;
            _validationInBoundDtoRules = validationInBoundDtoRules;
            _validationInBoundMoreDtoRules = validationInBoundMoreDtoRules;
            _validationOutBoundDtoRules = validationOutBoundDtoRules;
            _validationOutBoundMoreDtoRules = validationOutBoundMoreDtoRules;
            _equEquipmentRepository = equEquipmentRepository;
            _procResourceRepository = procResourceRepository;
            _procProcedureRepository = procProcedureRepository;
            _manuCreateBarcodeService = manuCreateBarcodeService;
            _executeJobService = executeJobService;
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

            if (manuSFCEntities == null || manuSFCEntities.Any() == false) return Enumerable.Empty<string>();
            return manuSFCEntities.Select(s => s.SFC);
        }


        /// <summary>
        /// 进站
        /// </summary>
        /// <param name="inBoundDto"></param>
        /// <returns></returns>
        public async Task InBoundAsync(InBoundDto inBoundDto)
        {
            await _validationInBoundDtoRules.ValidateAndThrowAsync(inBoundDto);
            if (inBoundDto == null) throw new CustomerValidationException(nameof(ErrorCode.MES10100));

            var manuBo = await GetManufactureBoAsync(new ManufactureRequestBo
            {
                SiteId = _currentEquipment.SiteId,
                ResourceCode = inBoundDto.ResourceCode,
                EquipmentCode = inBoundDto.EquipmentCode
            });
            if (manuBo == null) return;

            var jobBos = new List<JobBo> { };
            jobBos.Add(new JobBo { Name = "InStationJobService" });

            _ = await _executeJobService.ExecuteAsync(jobBos, new JobRequestBo
            {
                SiteId = _currentEquipment.SiteId,
                UserName = _currentEquipment.Name,
                Time = inBoundDto.LocalTime,
                ProcedureId = manuBo.ProcedureId,
                ResourceId = manuBo.ResourceId,
                EquipmentId = manuBo.EquipmentId,
                InStationRequestBos = new InStationRequestBo[] { new InStationRequestBo { SFC = inBoundDto.SFC } }
            });
        }

        /// <summary>
        /// 进站（多个）
        /// </summary>
        /// <param name="inBoundMoreDto"></param>
        /// <returns></returns>
        public async Task InBoundMoreAsync(InBoundMoreDto inBoundMoreDto)
        {
            await _validationInBoundMoreDtoRules.ValidateAndThrowAsync(inBoundMoreDto);
            if (inBoundMoreDto == null) throw new CustomerValidationException(nameof(ErrorCode.MES10100));
            if (inBoundMoreDto.SFCs.Count() <= 0) throw new CustomerValidationException(nameof(ErrorCode.MES19101));

            var manuBo = await GetManufactureBoAsync(new ManufactureRequestBo
            {
                SiteId = _currentEquipment.SiteId,
                ResourceCode = inBoundMoreDto.ResourceCode,
                EquipmentCode = inBoundMoreDto.EquipmentCode
            });
            if (manuBo == null) return;

            var jobBos = new List<JobBo> { };
            jobBos.Add(new JobBo { Name = "InStationJobService" });

            _ = await _executeJobService.ExecuteAsync(jobBos, new JobRequestBo
            {
                SiteId = _currentEquipment.SiteId,
                UserName = _currentEquipment.Name,
                Time = inBoundMoreDto.LocalTime,
                ProcedureId = manuBo.ProcedureId,
                ResourceId = manuBo.ResourceId,
                EquipmentId = manuBo.EquipmentId,
                InStationRequestBos = inBoundMoreDto.SFCs.Select(s => new InStationRequestBo { SFC = s.SFC })
            });
        }

        /// <summary>
        /// 出站
        /// </summary>
        /// <param name="outBoundDto"></param>
        /// <returns></returns>
        public async Task OutBoundAsync(OutBoundDto outBoundDto)
        {
            await _validationOutBoundDtoRules.ValidateAndThrowAsync(outBoundDto);
            if (outBoundDto == null) throw new CustomerValidationException(nameof(ErrorCode.MES10100));

            var manuBo = await GetManufactureBoAsync(new ManufactureRequestBo
            {
                SiteId = _currentEquipment.SiteId,
                ResourceCode = outBoundDto.ResourceCode,
                EquipmentCode = outBoundDto.EquipmentCode
            });
            if (manuBo == null) return;

            var jobBos = new List<JobBo> { };
            if (outBoundDto.IsPassingStation)
            {
                jobBos.Add(new JobBo { Name = "InStationJobService" });
            }

            // 进出站参数
            var requestBo = new JobRequestBo
            {
                SiteId = _currentEquipment.SiteId,
                UserName = _currentEquipment.Name,
                Time = outBoundDto.LocalTime,
                ProcedureId = manuBo.ProcedureId,
                ResourceId = manuBo.ResourceId,
                EquipmentId = manuBo.EquipmentId
            };

            var outStationRequestBo = new OutStationRequestBo
            {
                SFC = outBoundDto.SFC,
                IsQualified = outBoundDto.Passed == 1,
            };

            // 消耗信息
            if (outBoundDto.BindFeedingCodes != null && outBoundDto.BindFeedingCodes.Any())
            {
                outStationRequestBo.ConsumeList = outBoundDto.BindFeedingCodes.Select(s => new OutStationConsumeBo { BarCode = s });
            }

            // 不合格代码信息
            if (outBoundDto.NG != null && outBoundDto.NG.Any())
            {
                outStationRequestBo.OutStationUnqualifiedList = outBoundDto.NG.Select(s => new OutStationUnqualifiedBo { UnqualifiedCode = s.NGCode });
            }

            /*
            // 出站参数信息
            if (outBoundDto.ParamList != null && outBoundDto.ParamList.Any())
            {
                outStationRequestBo.ParamList = outBoundDto.ParamList;
            }
            */

            jobBos.Add(new JobBo { Name = "OutStationJobService" });
            requestBo.OutStationRequestBos = new OutStationRequestBo[] { outStationRequestBo };
            _ = await _executeJobService.ExecuteAsync(jobBos, requestBo);
        }

        /// <summary>
        /// 出站（多个）
        /// </summary>
        /// <param name="outBoundMoreDto"></param>
        /// <returns></returns>
        public async Task OutBoundMoreAsync(OutBoundMoreDto outBoundMoreDto)
        {
            await _validationOutBoundMoreDtoRules.ValidateAndThrowAsync(outBoundMoreDto);
            if (outBoundMoreDto == null) throw new CustomerValidationException(nameof(ErrorCode.MES10100));
            if (outBoundMoreDto.SFCs.Length <= 0) throw new CustomerValidationException(nameof(ErrorCode.MES19101));

            var manuBo = await GetManufactureBoAsync(new ManufactureRequestBo
            {
                SiteId = _currentEquipment.SiteId,
                ResourceCode = outBoundMoreDto.ResourceCode,
                EquipmentCode = outBoundMoreDto.EquipmentCode
            });
            if (manuBo == null) return;

            var jobBos = new List<JobBo> { };

            // 进出站参数
            var requestBo = new JobRequestBo
            {
                SiteId = _currentEquipment.SiteId,
                UserName = _currentEquipment.Name,
                Time = outBoundMoreDto.LocalTime,
                ProcedureId = manuBo.ProcedureId,
                ResourceId = manuBo.ResourceId,
                EquipmentId = manuBo.EquipmentId
            };

            // 进站参数
            if (outBoundMoreDto.SFCs.Any(a => a.IsPassingStation))
            {
                jobBos.Add(new JobBo { Name = "InStationJobService" });
                requestBo.InStationRequestBos = outBoundMoreDto.SFCs.Where(w => w.IsPassingStation).Select(s => new InStationRequestBo { SFC = s.SFC });
            }

            // 出站参数
            var outStationRequestBos = new List<OutStationRequestBo>();
            foreach (var outBoundDto in outBoundMoreDto.SFCs)
            {
                var outStationRequestBo = new OutStationRequestBo
                {
                    SFC = outBoundDto.SFC,
                    IsQualified = outBoundDto.Passed == 1,
                };

                // 消耗信息
                if (outBoundDto.BindFeedingCodes != null && outBoundDto.BindFeedingCodes.Any())
                {
                    outStationRequestBo.ConsumeList = outBoundDto.BindFeedingCodes.Select(s => new OutStationConsumeBo { BarCode = s });
                }

                // 不合格代码信息
                if (outBoundDto.NG != null && outBoundDto.NG.Any())
                {
                    outStationRequestBo.OutStationUnqualifiedList = outBoundDto.NG.Select(s => new OutStationUnqualifiedBo { UnqualifiedCode = s.NGCode });
                }

                /*
                // 出站参数信息
                if (outBoundDto.ParamList != null && outBoundDto.ParamList.Any())
                {
                    outStationRequestBo.ParamList = outBoundDto.ParamList;
                }
                */

                outStationRequestBos.Add(outStationRequestBo);
            }

            jobBos.Add(new JobBo { Name = "OutStationJobService" });
            requestBo.OutStationRequestBos = outStationRequestBos;
            _ = await _executeJobService.ExecuteAsync(jobBos, requestBo);
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
            if (resourceBindEntities == null || resourceBindEntities.Any() == false)
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