using Dapper;
using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Integrated;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Enums;
using Hymson.MES.CoreServices.Bos.Job;
using Hymson.MES.CoreServices.Bos.Manufacture;
using Hymson.MES.CoreServices.Bos.Parameter;
using Hymson.MES.CoreServices.Dtos.Common;
using Hymson.MES.CoreServices.Services.Job.JobUtility.Execute;
using Hymson.MES.CoreServices.Services.Manufacture;
using Hymson.MES.CoreServices.Services.Parameter;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.MES.Data.Repositories.Integrated;
using Hymson.MES.Data.Repositories.Integrated.IIntegratedRepository;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Services.Dtos.Manufacture;
using Hymson.Snowflake;
using Hymson.Utils;

namespace Hymson.MES.Services.Services.Manufacture
{
    /// <summary>
    /// 操作面板按钮 服务
    /// </summary>
    public class ManuFacePlateButtonService : IManuFacePlateButtonService
    {
        private readonly ICurrentUser _currentUser;
        private readonly ICurrentSite _currentSite;

        /// <summary>
        /// 操作面板按钮 仓储
        /// </summary>
        private readonly IManuFacePlateButtonRepository _manuFacePlateButtonRepository;

        /// <summary>
        /// 仓储接口（面板按钮作业关系）
        /// </summary>
        private readonly IManuFacePlateButtonJobRelationRepository _manuFacePlateButtonJobRelationRepository;

        /// <summary>
        /// 服务接口（过站）
        /// </summary>
        private readonly IManuPassStationService _manuPassStationService;

        /// <summary>
        /// 接口（参数收集）
        /// </summary>
        private readonly IManuProductParameterService _manuProductParameterService;

        /// <summary>
        /// 仓储接口（作业）
        /// </summary>
        private readonly IInteJobRepository _inteJobRepository;

        /// <summary>
        /// 仓储接口（载具注册）
        /// </summary>
        private readonly IInteVehicleRepository _inteVehicleRepository;

        /// <summary>
        /// 仓储接口（二维载具条码明细）
        /// </summary>
        private readonly IInteVehiceFreightStackRepository _inteVehiceFreightStackRepository;

        /// <summary>
        /// 仓储接口（作业）
        /// </summary>
        private readonly IExecuteJobService<JobBaseBo> _executeJobService;

        /// <summary>
        /// 验证器
        /// </summary>
        private readonly AbstractValidator<ManuFacePlateButtonCreateDto> _validationCreateRules;
        private readonly AbstractValidator<ManuFacePlateButtonModifyDto> _validationModifyRules;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="currentSite"></param>
        /// <param name="manuFacePlateButtonRepository"></param>
        /// <param name="manuFacePlateButtonJobRelationRepository"></param>
        /// <param name="manuPassStationService"></param>
        /// <param name="manuProductParameterService"></param>
        /// <param name="inteJobRepository"></param>
        /// <param name="inteVehicleRepository"></param>
        /// <param name="inteVehiceFreightStackRepository"></param>
        /// <param name="validationCreateRules"></param>
        /// <param name="validationModifyRules"></param>
        /// <param name="executeJobService"></param>
        public ManuFacePlateButtonService(ICurrentUser currentUser, ICurrentSite currentSite,
            IManuFacePlateButtonRepository manuFacePlateButtonRepository,
            IManuFacePlateButtonJobRelationRepository manuFacePlateButtonJobRelationRepository,
            IManuPassStationService manuPassStationService,
            IManuProductParameterService manuProductParameterService,
            IInteJobRepository inteJobRepository,
            IInteVehicleRepository inteVehicleRepository,
            IInteVehiceFreightStackRepository inteVehiceFreightStackRepository,
            AbstractValidator<ManuFacePlateButtonCreateDto> validationCreateRules,
            AbstractValidator<ManuFacePlateButtonModifyDto> validationModifyRules,
            IExecuteJobService<JobBaseBo> executeJobService)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _manuFacePlateButtonRepository = manuFacePlateButtonRepository;
            _manuFacePlateButtonJobRelationRepository = manuFacePlateButtonJobRelationRepository;
            _manuPassStationService = manuPassStationService;
            _manuProductParameterService = manuProductParameterService;
            _inteJobRepository = inteJobRepository;
            _inteVehicleRepository = inteVehicleRepository;
            _inteVehiceFreightStackRepository = inteVehiceFreightStackRepository;
            _validationCreateRules = validationCreateRules;
            _validationModifyRules = validationModifyRules;
            _executeJobService = executeJobService;
        }


        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="manuFacePlateButtonCreateDto"></param>
        /// <returns></returns>
        public async Task CreateManuFacePlateButtonAsync(ManuFacePlateButtonCreateDto manuFacePlateButtonCreateDto)
        {
            //验证DTO
            await _validationCreateRules.ValidateAndThrowAsync(manuFacePlateButtonCreateDto);

            //DTO转换实体
            var manuFacePlateButtonEntity = manuFacePlateButtonCreateDto.ToEntity<ManuFacePlateButtonEntity>();
            manuFacePlateButtonEntity.Id = IdGenProvider.Instance.CreateId();
            manuFacePlateButtonEntity.CreatedBy = _currentUser.UserName;
            manuFacePlateButtonEntity.UpdatedBy = _currentUser.UserName;
            manuFacePlateButtonEntity.CreatedOn = HymsonClock.Now();
            manuFacePlateButtonEntity.UpdatedOn = HymsonClock.Now();
            manuFacePlateButtonEntity.SiteId = _currentSite.SiteId ?? 0;

            //入库
            await _manuFacePlateButtonRepository.InsertAsync(manuFacePlateButtonEntity);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteManuFacePlateButtonAsync(long id)
        {
            await _manuFacePlateButtonRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeletesManuFacePlateButtonAsync(long[] ids)
        {
            return await _manuFacePlateButtonRepository.DeletesAsync(new DeleteCommand { Ids = ids, DeleteOn = HymsonClock.Now(), UserId = _currentUser.UserName });
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="manuFacePlateButtonPagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ManuFacePlateButtonDto>> GetPagedListAsync(ManuFacePlateButtonPagedQueryDto manuFacePlateButtonPagedQueryDto)
        {
            var manuFacePlateButtonPagedQuery = manuFacePlateButtonPagedQueryDto.ToQuery<ManuFacePlateButtonPagedQuery>();
            manuFacePlateButtonPagedQuery.SiteId = _currentSite.SiteId ?? 0;
            var pagedInfo = await _manuFacePlateButtonRepository.GetPagedInfoAsync(manuFacePlateButtonPagedQuery);

            //实体到DTO转换 装载数据
            IList<ManuFacePlateButtonDto> manuFacePlateButtonDtos = await PrepareManuFacePlateButtonDtos(pagedInfo.Data);
            return new PagedInfo<ManuFacePlateButtonDto>(manuFacePlateButtonDtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 按钮信息组装
        /// </summary>
        /// <param name="manuFacePlateButtons"></param>
        /// <returns></returns>
        private async Task<IList<ManuFacePlateButtonDto>> PrepareManuFacePlateButtonDtos(IEnumerable<ManuFacePlateButtonEntity> manuFacePlateButtons)
        {
            var manuFacePlateButtonDtos = new List<ManuFacePlateButtonDto>();
            if (!manuFacePlateButtons.Any())
            {
                return manuFacePlateButtonDtos;
            }
            //关联的ButtonId
            var facePalateButtonIds = manuFacePlateButtons.Select(c => c.Id).ToArray();
            //查询关联的JOB信息
            var manuFacePlateButtonJobRelationEntitys = await _manuFacePlateButtonJobRelationRepository.GetByFacePlateButtonIdsAsync(facePalateButtonIds.ToArray());
            var facePalateButtonJobIds = manuFacePlateButtonJobRelationEntitys.Select(c => c.JobId).ToArray();
            //job信息
            List<InteJobEntity> inteJobEntitys = new List<InteJobEntity>();
            if (facePalateButtonJobIds.Any())
            {
                var inteJobs = await _inteJobRepository.GetByIdsAsync(facePalateButtonJobIds);
                inteJobEntitys = inteJobs.ToList();
            }
            //按钮关联JOb信息
            List<ManuFacePlateButtonJobRelationDto> manuFacePlateButtonJobRelationDtos = new();
            foreach (var manuFacePlateButtonJobRelationEntity in manuFacePlateButtonJobRelationEntitys)
            {
                var manuFacePlateButtonJobRelationDto = manuFacePlateButtonJobRelationEntity.ToModel<ManuFacePlateButtonJobRelationDto>();
                //填充JOB信息
                var jobEntity = inteJobEntitys.FirstOrDefault(c => c.Id == manuFacePlateButtonJobRelationDto.JobId);
                if (jobEntity != null)
                {
                    manuFacePlateButtonJobRelationDto.JobCode = jobEntity.Code;
                    manuFacePlateButtonJobRelationDto.JobName = jobEntity.Name;
                }
                manuFacePlateButtonJobRelationDtos.Add(manuFacePlateButtonJobRelationDto);
            }
            foreach (var manuFacePlateButtonEntity in manuFacePlateButtons)
            {
                var manuFacePlateButtonDto = manuFacePlateButtonEntity.ToModel<ManuFacePlateButtonDto>();
                //筛选对应ButtonId的Relation
                var buttonJobRealtions = manuFacePlateButtonJobRelationDtos.Where(c => c.FacePlateButtonId == manuFacePlateButtonDto.Id);
                manuFacePlateButtonDto.ManuFacePlateButtonJobRelations = buttonJobRealtions.ToArray();
                manuFacePlateButtonDtos.Add(manuFacePlateButtonDto);
            }

            return manuFacePlateButtonDtos;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="manuFacePlateButtonModifyDto"></param>
        /// <returns></returns>
        public async Task ModifyManuFacePlateButtonAsync(ManuFacePlateButtonModifyDto manuFacePlateButtonModifyDto)
        {
            //验证DTO
            await _validationModifyRules.ValidateAndThrowAsync(manuFacePlateButtonModifyDto);

            //DTO转换实体
            var manuFacePlateButtonEntity = manuFacePlateButtonModifyDto.ToEntity<ManuFacePlateButtonEntity>();
            manuFacePlateButtonEntity.UpdatedBy = _currentUser.UserName;
            manuFacePlateButtonEntity.UpdatedOn = HymsonClock.Now();

            await _manuFacePlateButtonRepository.UpdateAsync(manuFacePlateButtonEntity);
        }

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ManuFacePlateButtonDto> QueryManuFacePlateButtonByIdAsync(long id)
        {
            var manuFacePlateButtonEntity = await _manuFacePlateButtonRepository.GetByIdAsync(id);
            if (manuFacePlateButtonEntity != null)
            {
                return manuFacePlateButtonEntity.ToModel<ManuFacePlateButtonDto>();
            }
            return new ManuFacePlateButtonDto();
        }

        /// <summary>
        /// 根据facePlateId查询
        /// </summary>
        /// <param name="facePlateId"></param>
        /// <returns></returns>
        public async Task<IList<ManuFacePlateButtonDto>> QueryManuFacePlateButtonByFacePlateIdAsync(long facePlateId)
        {
            var manuFacePlateButtonListEntity = await _manuFacePlateButtonRepository.GetByFacePlateIdAsync(facePlateId);
            var manuFacePlateButtonDtoList = await PrepareManuFacePlateButtonDtos(manuFacePlateButtonListEntity);
            return manuFacePlateButtonDtoList;
        }

        /// <summary>
        /// 根据buttionid查询按钮信息
        /// </summary>
        /// <param name="buttionId"></param>
        /// <returns></returns>
        public async Task<ManuFacePlateButtonDto> QueryManuFacePlateButtonByButtonIdAsync(long buttionId)
        {
            ManuFacePlateButtonDto manuFacePlateButtonDto = new ManuFacePlateButtonDto();
            var manuFacePlateButtonEntity = await _manuFacePlateButtonRepository.GetByIdAsync(buttionId);
            if (manuFacePlateButtonEntity == null)
            {
                return manuFacePlateButtonDto;
            }
            //关联的ButtonId
            var facePalateButtonIds = manuFacePlateButtonEntity.Id;
            //查询关联的JOB信息
            var manuFacePlateButtonJobRelationEntitys = await _manuFacePlateButtonJobRelationRepository.GetByFacePlateButtonIdAsync(facePalateButtonIds);
            var facePalateButtonJobIds = manuFacePlateButtonJobRelationEntitys.Select(c => c.JobId).ToArray();
            //job信息
            List<InteJobEntity> inteJobEntitys = new List<InteJobEntity>();
            if (facePalateButtonJobIds.Any())
            {
                var inteJobs = await _inteJobRepository.GetByIdsAsync(facePalateButtonJobIds);
                inteJobEntitys = inteJobs.ToList();
            }
            //按钮关联JOb信息
            List<ManuFacePlateButtonJobRelationDto> manuFacePlateButtonJobRelationDtos = new();
            foreach (var manuFacePlateButtonJobRelationEntity in manuFacePlateButtonJobRelationEntitys)
            {
                var manuFacePlateButtonJobRelationDto = manuFacePlateButtonJobRelationEntity.ToModel<ManuFacePlateButtonJobRelationDto>();
                //填充JOB信息
                var jobEntity = inteJobEntitys.FirstOrDefault(c => c.Id == manuFacePlateButtonJobRelationDto.JobId);
                if (jobEntity != null)
                {
                    manuFacePlateButtonJobRelationDto.JobCode = jobEntity.Code;
                    manuFacePlateButtonJobRelationDto.JobName = jobEntity.Name;
                }
                manuFacePlateButtonJobRelationDtos.Add(manuFacePlateButtonJobRelationDto);
            }
            manuFacePlateButtonDto = manuFacePlateButtonEntity.ToModel<ManuFacePlateButtonDto>();
            manuFacePlateButtonDto.ManuFacePlateButtonJobRelations = manuFacePlateButtonJobRelationDtos.ToArray();
            return manuFacePlateButtonDto;
        }

        /// <summary>
        /// 按钮（点击）
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<Dictionary<string, JobResponseDto>> ClickAsync(ButtonRequestDto dto)
        {
            var result = new Dictionary<string, JobResponseDto> { }; // 返回结果

            // 先检查按钮是否存在
            var button = await _manuFacePlateButtonRepository.GetByIdAsync(dto.FacePlateButtonId)
                ?? throw new CustomerValidationException(nameof(ErrorCode.MES17208));

            // 根据面板ID和按钮ID找出绑定的作业job
            var buttonJobs = await _manuFacePlateButtonJobRelationRepository.GetByFacePlateButtonIdAsync(dto.FacePlateButtonId);
            if (!buttonJobs.Any()) return result;

            // 根据 buttonJobs 读取对应的job对象
            var jobsOfNotSort = await _inteJobRepository.GetEntitiesAsync(new EntityBySiteIdQuery { SiteId = _currentSite.SiteId ?? 0 });

            List<InteJobEntity> jobs = new();
            foreach (var item in buttonJobs)
            {
                var tempJob = jobsOfNotSort.FirstOrDefault(f => f.Id == item.JobId);
                if (tempJob == null) continue;

                jobs.Add(tempJob);
            }

            // 是否清除条码
            if (buttonJobs.Any(a => a.IsClear)) dto.Param?.Add("IsClear", "True");

            // 是否清除条码
            var isClear = false;
            if (buttonJobs.Any(a => a.IsClear)) isClear = true;

            // 如果没有读取到有效作业，就提示错误
            if (!jobs.Any()) throw new CustomerValidationException(nameof(ErrorCode.MES17255));

            if (!dto.Param!.ContainsKey("Type")) throw new CustomerValidationException(nameof(ErrorCode.MES17256)).WithData("Param", "Type");
            if (!dto.Param!.ContainsKey("SFCs")) throw new CustomerValidationException(nameof(ErrorCode.MES17256)).WithData("Param", "SFCs");

            // 条码类型
            var codeType = CodeTypeEnum.SFC;
            if (!dto.Param!.ContainsKey("Type"))
            {
                codeType = CodeTypeEnum.SFC;
            }
            else
            {
                if (!Enum.TryParse(dto.Param["Type"], out codeType)) codeType = CodeTypeEnum.SFC;
            }

            // 作业请求参数
            var requestBo = new JobRequestBo
            {
                SiteId = _currentSite.SiteId ?? 0,
                UserName = _currentUser.UserName,
                Type = codeType,
                ProcedureId = dto.Param!["ProcedureId"].ParseToLong(),
                ResourceId = dto.Param["ResourceId"].ParseToLong()
            };

            List<string> SFCs = new();
            List<PanelRequestBo> panelRequestBos = new();
            List<InStationRequestBo> inStationRequestBos = new();
            List<OutStationRequestBo> outStationRequestBos = new();
            switch (requestBo.Type)
            {
                case CodeTypeEnum.Vehicle:
                    var vehicleCodes = dto.Param!["SFCs"].ToDeserialize<List<string>>()
                        ?? throw new CustomerValidationException(nameof(ErrorCode.MES18623)).WithData("Code", dto.Param!["SFCs"]);

                    // 读取载具关联的条码
                    var vehicleEntities = await _inteVehicleRepository.GetByCodesAsync(new EntityByCodesQuery
                    {
                        SiteId = requestBo.SiteId,
                        Codes = vehicleCodes
                    });

                    // 不在系统中的载具代码
                    var notInSystem = vehicleCodes.Except(vehicleEntities.Select(s => s.Code));
                    if (notInSystem.Any())
                    {
                        throw new CustomerValidationException(nameof(ErrorCode.MES18624))
                            .WithData("Code", string.Join(',', notInSystem));
                    }

                    // 查询载具关联的条码明细
                    var vehicleFreightStackEntities = await _inteVehiceFreightStackRepository.GetEntitiesAsync(new EntityByParentIdsQuery
                    {
                        SiteId = requestBo.SiteId,
                        ParentIds = vehicleEntities.Select(s => s.Id)
                    });
                    var vehicleFreightStackDic = vehicleFreightStackEntities.ToLookup(w => w.VehicleId).ToDictionary(d => d.Key, d => d);

                    SFCs = vehicleFreightStackEntities.Select(s => s.BarCode).AsList();
                    foreach (var item in vehicleFreightStackDic)
                    {
                        var vehicleEntity = vehicleEntities.FirstOrDefault(f => f.Id == item.Key);
                        if (vehicleEntity == null) continue;

                        panelRequestBos.AddRange(item.Value.Select(s => new PanelRequestBo { SFC = s.BarCode, VehicleCode = vehicleEntity.Code }));
                        inStationRequestBos.AddRange(item.Value.Select(s => new InStationRequestBo { SFC = s.BarCode, VehicleCode = vehicleEntity.Code }));
                        outStationRequestBos.AddRange(item.Value.Select(s => new OutStationRequestBo { SFC = s.BarCode, VehicleCode = vehicleEntity.Code }));
                    }
                    break;
                case CodeTypeEnum.SFC:
                default:
                    var sfcCodes = dto.Param!["SFCs"].ToDeserialize<List<string>>()
                        ?? throw new CustomerValidationException(nameof(ErrorCode.MES17415)).WithData("SFC", dto.Param!["SFCs"]);

                    SFCs = sfcCodes;
                    panelRequestBos.AddRange(SFCs.Select(s => new PanelRequestBo { SFC = s }));
                    inStationRequestBos.AddRange(SFCs.Select(s => new InStationRequestBo { SFC = s }));
                    outStationRequestBos.AddRange(SFCs.Select(s => new OutStationRequestBo { SFC = s }));
                    break;
            }

            requestBo.SFCs = SFCs;  // 这句后面要改
            requestBo.PanelRequestBos = panelRequestBos;
            requestBo.InStationRequestBos = inStationRequestBos;
            requestBo.OutStationRequestBos = outStationRequestBos;

            // 执行Job
            var jobResponses = await _executeJobService.ExecuteAsync(jobs.Select(s => new JobBo { Name = s.ClassProgram }), requestBo);
            foreach (var item in jobResponses)
            {
                if (isClear) item.Value.Content.Add("IsClear", "True");

                result.Add(item.Key, new JobResponseDto
                {
                    Rows = item.Value.Rows,
                    Content = item.Value.Content,
                    Message = item.Value.Message,
                    Time = item.Value.Time
                });
            }
            return result;
        }

        /// <summary>
        /// 进站（接口）
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<Dictionary<string, JobResponseDto>> InStationAsync(InStationRequestDto dto)
        {
            var result = new Dictionary<string, JobResponseDto> { }; // 返回结果

            var boResult = new Dictionary<string, JobResponseBo> { };
            switch (dto.Type)
            {
                case CodeTypeEnum.SFC:
                    var sfcCodes = dto.Params;
                    if (sfcCodes == null || !sfcCodes.Any())
                    {
                        throw new CustomerValidationException(nameof(ErrorCode.MES17415)).WithData("SFC", "");
                    }

                    boResult = await _manuPassStationService.InStationRangeBySFCAsync(new SFCInStationBo
                    {
                        SiteId = _currentSite.SiteId ?? 0,
                        UserName = _currentUser.UserName,
                        ProcedureId = dto.ProcedureId,
                        ResourceId = dto.ResourceId,
                        SFCs = sfcCodes
                    });
                    break;
                case CodeTypeEnum.Vehicle:
                    var vehicleCodes = dto.Params;
                    if (vehicleCodes == null || !vehicleCodes.Any())
                    {
                        throw new CustomerValidationException(nameof(ErrorCode.MES18623)).WithData("Code", "");
                    }

                    boResult = await _manuPassStationService.InStationRangeByVehicleAsync(new VehicleInStationBo
                    {
                        SiteId = _currentSite.SiteId ?? 0,
                        UserName = _currentUser.UserName,
                        ProcedureId = dto.ProcedureId,
                        ResourceId = dto.ResourceId,
                        VehicleCodes = vehicleCodes
                    });
                    break;
                default:
                    break;
            }

            foreach (var item in boResult)
            {
                result.Add(item.Key, new JobResponseDto
                {
                    Rows = item.Value.Rows,
                    Content = item.Value.Content,
                    Message = item.Value.Message,
                    Time = item.Value.Time
                });
            }

            return result;
        }

        /// <summary>
        /// 出站（接口）
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<Dictionary<string, JobResponseDto>> OutStationAsync(OutStationRequestDto dto)
        {
            var result = new Dictionary<string, JobResponseDto> { }; // 返回结果

            var boResult = new Dictionary<string, JobResponseBo> { };
            switch (dto.Type)
            {
                case CodeTypeEnum.SFC:
                    var sfcCodes = dto.Params;
                    if (sfcCodes == null || !sfcCodes.Any())
                    {
                        throw new CustomerValidationException(nameof(ErrorCode.MES17415)).WithData("SFC", "");
                    }

                    boResult = await _manuPassStationService.OutStationRangeBySFCAsync(new SFCOutStationBo
                    {
                        SiteId = _currentSite.SiteId ?? 0,
                        UserName = _currentUser.UserName,
                        ProcedureId = dto.ProcedureId,
                        ResourceId = dto.ResourceId,
                        SFCs = sfcCodes
                    });
                    break;
                case CodeTypeEnum.Vehicle:
                    var vehicleCodes = dto.Params;
                    if (vehicleCodes == null || !vehicleCodes.Any())
                    {
                        throw new CustomerValidationException(nameof(ErrorCode.MES18623)).WithData("Code", "");
                    }

                    boResult = await _manuPassStationService.OutStationRangeByVehicleAsync(new VehicleOutStationBo
                    {
                        SiteId = _currentSite.SiteId ?? 0,
                        UserName = _currentUser.UserName,
                        ProcedureId = dto.ProcedureId,
                        ResourceId = dto.ResourceId,
                        VehicleCodes = vehicleCodes
                    });
                    break;
                default:
                    break;
            }

            foreach (var item in boResult)
            {
                result.Add(item.Key, new JobResponseDto
                {
                    Rows = item.Value.Rows,
                    Content = item.Value.Content,
                    Message = item.Value.Message,
                    Time = item.Value.Time
                });
            }

            return result;
        }

        /// <summary>
        /// 参数收集（点击）
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<int> ProductParameterCollectAsync(ProductProcessParameterDto dto)
        {
            return await _manuProductParameterService.ProductParameterCollectAsync(new ProductProcessParameterBo
            {
                SiteId = _currentSite.SiteId ?? 0,
                UserName = _currentUser.UserName,
                Time = HymsonClock.Now(),
                ProcedureId = dto.ProcedureId,
                ResourceId = dto.ResourceId,
                SFC = dto.SFC,
                Parameters = dto.Parameters
            });
        }

        /// <summary>
        ///  新按钮（点击）
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="bo"></param>
        /// <returns></returns>
        public async Task<Dictionary<string, JobResponseBo>> NewClickAsync(ButtonRequestDto dto, dynamic bo)
        {
            var result = new Dictionary<string, JobResponseBo> { }; // 返回结果

            // 先检查按钮是否存在
            var button = await _manuFacePlateButtonRepository.GetByIdAsync(dto.FacePlateButtonId)
                ?? throw new CustomerValidationException(nameof(ErrorCode.MES17208));

            // 根据面板ID和按钮ID找出绑定的作业job
            var buttonJobs = await _manuFacePlateButtonJobRelationRepository.GetByFacePlateButtonIdAsync(dto.FacePlateButtonId);
            if (!buttonJobs.Any()) return result;

            // 根据 buttonJobs 读取对应的job对象
            var jobs = await _inteJobRepository.GetByIdsAsync(buttonJobs.Select(s => s.JobId).ToArray());

            // 是否清除条码
            if (buttonJobs.Any(a => a.IsClear)) dto.Param?.Add("IsClear", "True");

            var jobBos = new List<JobBo>();
            foreach (var job in jobs)
            {
                jobBos.Add(new JobBo { Name = job.ClassProgram });
            }
            result = await _executeJobService.ExecuteAsync(jobBos, bo);

            return result;
        }


    }
}
