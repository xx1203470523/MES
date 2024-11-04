/*
 *creator: Karl
 *
 *describe: 操作面板    服务 | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-04-01 01:56:57
 */
using FluentValidation;
using FluentValidation.Results;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.Localization.Services;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Integrated;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Enums;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.MES.Data.Repositories.Integrated.IIntegratedRepository;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Services.Dtos.Manufacture;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;
using Minio.DataModel;
using System.Linq;
using System.Transactions;

namespace Hymson.MES.Services.Services.Manufacture
{
    /// <summary>
    /// 操作面板 服务
    /// </summary>
    public class ManuFacePlateService : IManuFacePlateService
    {
        private readonly ICurrentUser _currentUser;
        private readonly ICurrentSite _currentSite;

        #region Repository
        /// <summary>
        /// 操作面板 仓储
        /// </summary>
        private readonly IManuFacePlateRepository _manuFacePlateRepository;
        private readonly IManuFacePlateProductionRepository _manuFacePlateProductionRepository;
        private readonly IManuFacePlateRepairRepository _manuFacePlateRepairRepository;
        private readonly IManuFacePlateContainerPackRepository _manuFacePlateContainerPackRepository;
        private readonly IManuFacePlateButtonRepository _manuFacePlateButtonRepository;
        private readonly IManuFacePlateButtonJobRelationRepository _manuFacePlateButtonJobRelationRepository;
        private readonly IProcProcedureRepository _procProcedureRepository;
        private readonly IProcResourceRepository _procResourceRepository;
        private readonly IInteJobRepository _inteJobRepository;
        private readonly ILocalizationService _localizationService;
        private readonly AbstractValidator<ManuFacePlateCreateDto> _validationCreateRules;
        private readonly AbstractValidator<ManuFacePlateModifyDto> _validationModifyRules;
        private readonly AbstractValidator<ManuFacePlateProductionCreateDto> _validationProductionCreateRules;
        private readonly AbstractValidator<ManuFacePlateProductionModifyDto> _validationProductionModifyRules;
        private readonly AbstractValidator<ManuFacePlateRepairCreateDto> _validationRepairCreateRules;
        private readonly AbstractValidator<ManuFacePlateRepairModifyDto> _validationRepairModifyRules;
        private readonly AbstractValidator<ManuFacePlateContainerPackCreateDto> _validationContainerPackCreateRules;
        private readonly AbstractValidator<ManuFacePlateContainerPackModifyDto> _validationContainerPackModifyRules;
        private readonly AbstractValidator<ManuFacePlateButtonModifyDto> _validationButtonModifyRules;
        private readonly AbstractValidator<ManuFacePlateButtonCreateDto> _validationButtonCreateRules;

        public ManuFacePlateService(ICurrentUser currentUser, ICurrentSite currentSite, IManuFacePlateRepository manuFacePlateRepository, IManuFacePlateProductionRepository manuFacePlateProductionRepository,
            IProcProcedureRepository procProcedureRepository, IProcResourceRepository procResourceRepository, IManuFacePlateRepairRepository manuFacePlateRepairRepository, IManuFacePlateContainerPackRepository manuFacePlateContainerPackRepository,
            AbstractValidator<ManuFacePlateCreateDto> validationCreateRules, AbstractValidator<ManuFacePlateModifyDto> validationModifyRules,
            AbstractValidator<ManuFacePlateProductionCreateDto> validationProductionCreateRules, AbstractValidator<ManuFacePlateProductionModifyDto> validationProductionModifyRules,
            AbstractValidator<ManuFacePlateRepairCreateDto> validationRepairCreateRules, AbstractValidator<ManuFacePlateRepairModifyDto> validationRepairModifyRules,
            AbstractValidator<ManuFacePlateContainerPackCreateDto> validationContainerPackCreateRules, AbstractValidator<ManuFacePlateContainerPackModifyDto> validationContainerPackModifyRules,
            IInteJobRepository inteJobRepository, AbstractValidator<ManuFacePlateButtonModifyDto> validationButtonModifyRules,
            AbstractValidator<ManuFacePlateButtonCreateDto> validationButtonCreateRules, IManuFacePlateButtonRepository manuFacePlateButtonRepository,
            IManuFacePlateButtonJobRelationRepository manuFacePlateButtonJobRelationRepository, ILocalizationService localizationService)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _manuFacePlateRepository = manuFacePlateRepository;
            _manuFacePlateProductionRepository = manuFacePlateProductionRepository;
            _procProcedureRepository = procProcedureRepository;
            _procResourceRepository = procResourceRepository;
            _manuFacePlateRepairRepository = manuFacePlateRepairRepository;
            _manuFacePlateContainerPackRepository = manuFacePlateContainerPackRepository;
            _validationCreateRules = validationCreateRules;
            _validationModifyRules = validationModifyRules;
            _validationProductionCreateRules = validationProductionCreateRules;
            _validationProductionModifyRules = validationProductionModifyRules;
            _validationRepairCreateRules = validationRepairCreateRules;
            _validationRepairModifyRules = validationRepairModifyRules;
            _validationContainerPackCreateRules = validationContainerPackCreateRules;
            _validationContainerPackModifyRules = validationContainerPackModifyRules;
            _inteJobRepository = inteJobRepository;
            _validationButtonModifyRules = validationButtonModifyRules;
            _validationButtonCreateRules = validationButtonCreateRules;
            _manuFacePlateButtonRepository = manuFacePlateButtonRepository;
            _manuFacePlateButtonJobRelationRepository = manuFacePlateButtonJobRelationRepository;
            _localizationService = localizationService;
        }
        #endregion

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="manuFacePlateCreateDto"></param>
        /// <returns></returns>
        public async Task CreateManuFacePlateAsync(ManuFacePlateCreateDto manuFacePlateCreateDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0)
            {
                throw new ValidationException(nameof(ErrorCode.MES10101));
            }

            //验证DTO
            await _validationCreateRules.ValidateAndThrowAsync(manuFacePlateCreateDto);

            //DTO转换实体
            var manuFacePlateEntity = manuFacePlateCreateDto.ToEntity<ManuFacePlateEntity>();
            manuFacePlateEntity.Id = IdGenProvider.Instance.CreateId();
            manuFacePlateEntity.CreatedBy = _currentUser.UserName;
            manuFacePlateEntity.UpdatedBy = _currentUser.UserName;
            manuFacePlateEntity.CreatedOn = HymsonClock.Now();
            manuFacePlateEntity.UpdatedOn = HymsonClock.Now();
            manuFacePlateEntity.SiteId = _currentSite.SiteId ?? 123456;

            //入库
            await _manuFacePlateRepository.InsertAsync(manuFacePlateEntity);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteManuFacePlateAsync(long id)
        {
            await _manuFacePlateRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeletesManuFacePlateAsync(long[] ids)
        {
            var manuFacePlates = await _manuFacePlateRepository.GetByIdsAsync(ids);
            //if (manuFacePlates != null && manuFacePlates.Any())
            //{
            //    var isAnyEnableOrRetain = manuFacePlates.Where(c => c.Status == SysDataStatusEnum.Enable || c.Status == SysDataStatusEnum.Retain).Any();
            //    if (isAnyEnableOrRetain)
            //        throw new CustomerValidationException(nameof(ErrorCode.MES16913));
            //}
            if (manuFacePlates != null && manuFacePlates.Any(a => a.Status != SysDataStatusEnum.Build))
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10106));
            }

            return await _manuFacePlateRepository.DeletesAsync(new DeleteCommand { Ids = ids, DeleteOn = HymsonClock.Now(), UserId = _currentUser.UserName });
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="manuFacePlatePagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ManuFacePlateDto>> GetPagedListAsync(ManuFacePlatePagedQueryDto manuFacePlatePagedQueryDto)
        {
            var manuFacePlatePagedQuery = manuFacePlatePagedQueryDto.ToQuery<ManuFacePlatePagedQuery>();
            manuFacePlatePagedQuery.SiteId = _currentSite.SiteId ?? 123456;
            var pagedInfo = await _manuFacePlateRepository.GetPagedInfoAsync(manuFacePlatePagedQuery);

            //实体到DTO转换 装载数据
            List<ManuFacePlateDto> manuFacePlateDtos = PrepareManuFacePlateDtos(pagedInfo);
            return new PagedInfo<ManuFacePlateDto>(manuFacePlateDtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pagedInfo"></param>
        /// <returns></returns>
        private static List<ManuFacePlateDto> PrepareManuFacePlateDtos(PagedInfo<ManuFacePlateEntity> pagedInfo)
        {
            var manuFacePlateDtos = new List<ManuFacePlateDto>();
            foreach (var manuFacePlateEntity in pagedInfo.Data)
            {
                var manuFacePlateDto = manuFacePlateEntity.ToModel<ManuFacePlateDto>();
                manuFacePlateDtos.Add(manuFacePlateDto);
            }

            return manuFacePlateDtos;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="manuFacePlateModifyDto"></param>
        /// <returns></returns>
        public async Task ModifyManuFacePlateAsync(ManuFacePlateModifyDto manuFacePlateModifyDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0)
            {
                throw new ValidationException(nameof(ErrorCode.MES10101));
            }

            //验证DTO
            await _validationModifyRules.ValidateAndThrowAsync(manuFacePlateModifyDto);

            //DTO转换实体
            var manuFacePlateEntity = manuFacePlateModifyDto.ToEntity<ManuFacePlateEntity>();
            manuFacePlateEntity.UpdatedBy = _currentUser.UserName;
            manuFacePlateEntity.UpdatedOn = HymsonClock.Now();

            var entity = await _manuFacePlateRepository.GetByIdAsync(manuFacePlateModifyDto.Id)
                ?? throw new BusinessException(nameof(ErrorCode.MES17209));

            if (entity.Status != SysDataStatusEnum.Build && manuFacePlateModifyDto.Status == SysDataStatusEnum.Build)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10108));
            }

            await _manuFacePlateRepository.UpdateAsync(manuFacePlateEntity);
        }

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ManuFacePlateQueryDto> QueryManuFacePlateByIdAsync(long id)
        {
            ManuFacePlateQueryDto facePlateQueryDto = new ManuFacePlateQueryDto();
            var manuFacePlateEntity = await _manuFacePlateRepository.GetByIdAsync(id);
            if (manuFacePlateEntity != null)
            {
                long resourceId = 0;
                long procedureId = 0;
                facePlateQueryDto.FacePlate = manuFacePlateEntity.ToModel<ManuFacePlateDto>();
                #region 生产过站
                if (manuFacePlateEntity.Type == Core.Enums.Manufacture.ManuFacePlateTypeEnum.ProducePassingStation)
                {
                    var manuFacePlateProductionEntity = await _manuFacePlateProductionRepository.GetByFacePlateIdAsync(manuFacePlateEntity.Id);
                    if (manuFacePlateProductionEntity != null)
                    {
                        resourceId = manuFacePlateProductionEntity.ResourceId;
                        procedureId = manuFacePlateProductionEntity.ProcedureId;

                        facePlateQueryDto.FacePlateProduction = manuFacePlateProductionEntity.ToModel<ManuFacePlateProductionDto>();
                        //颜色不为空就显示
                        facePlateQueryDto.FacePlateProduction.IsShowQualifiedColour = !string.IsNullOrEmpty(facePlateQueryDto.FacePlateProduction.QualifiedColour);
                        facePlateQueryDto.FacePlateProduction.IsShowUnqualifiedColour = !string.IsNullOrEmpty(facePlateQueryDto.FacePlateProduction.UnqualifiedColour);
                        //填充Job数据
                        facePlateQueryDto.FacePlateProduction.ScanJobCode = await QueryInteJobCodes(manuFacePlateProductionEntity.ScanJobId);
                    }
                }
                #endregion

                #region 在制品维修
                //在制品维修
                if (manuFacePlateEntity.Type == Core.Enums.Manufacture.ManuFacePlateTypeEnum.ProductionRepair)
                {
                    var manuFacePlateRepairEntity = await _manuFacePlateRepairRepository.GetByFacePlateIdAsync(manuFacePlateEntity.Id);
                    if (manuFacePlateRepairEntity != null)
                    {
                        resourceId = manuFacePlateRepairEntity.ResourceId;
                        procedureId = manuFacePlateRepairEntity.ProcedureId;
                        facePlateQueryDto.FacePlateRepair = manuFacePlateRepairEntity.ToModel<ManuFacePlateRepairDto>();
                    }
                }
                #endregion

                #region 容器包装
                //容器包装
                if (manuFacePlateEntity.Type == Core.Enums.Manufacture.ManuFacePlateTypeEnum.ContainerPack)
                {
                    var manuFacePlateContainerPackEntity = await _manuFacePlateContainerPackRepository.GetByFacePlateIdAsync(manuFacePlateEntity.Id);
                    if (manuFacePlateContainerPackEntity != null)
                    {
                        resourceId = manuFacePlateContainerPackEntity.ResourceId;
                        procedureId = manuFacePlateContainerPackEntity.ProcedureId;
                        facePlateQueryDto.FacePlateContainerPack = manuFacePlateContainerPackEntity.ToModel<ManuFacePlateContainerPackDto>();
                        //颜色不为空就显示
                        facePlateQueryDto.FacePlateContainerPack.IsShowQualifiedColour = !string.IsNullOrEmpty(facePlateQueryDto.FacePlateContainerPack.QualifiedColour);
                        facePlateQueryDto.FacePlateContainerPack.IsShowErrorsColour = !string.IsNullOrEmpty(facePlateQueryDto.FacePlateContainerPack.ErrorsColour);
                        //填充Job数据
                        facePlateQueryDto.FacePlateContainerPack.ScanJobCode = await QueryInteJobCodes(manuFacePlateContainerPackEntity.ScanJobId);
                    }
                }
                #endregion

                #region 按钮信息
                var manuFacePlateButtonEntityList = await _manuFacePlateButtonRepository.GetByFacePlateIdAsync(manuFacePlateEntity.Id);
                if (manuFacePlateButtonEntityList != null && manuFacePlateButtonEntityList.Any())
                {
                    //关联的ButtonId
                    var facePalateButtonIds = manuFacePlateButtonEntityList.Select(c => c.Id).ToArray();
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
                        var jobEntity = inteJobEntitys.Where(c => c.Id == manuFacePlateButtonJobRelationDto.JobId).FirstOrDefault();
                        if (jobEntity != null)
                        {
                            manuFacePlateButtonJobRelationDto.JobCode = jobEntity.Code;
                            manuFacePlateButtonJobRelationDto.JobName = jobEntity.Name;
                        }
                        manuFacePlateButtonJobRelationDtos.Add(manuFacePlateButtonJobRelationDto);
                    }

                    //按钮信息
                    List<ManuFacePlateButtonDto> manuFacePlateButtons = new();
                    foreach (var manuFacePlateButtonEntity in manuFacePlateButtonEntityList)
                    {
                        var manuFacePlateButtonDto = manuFacePlateButtonEntity.ToModel<ManuFacePlateButtonDto>();
                        //筛选对应ButtonId的Relation
                        var buttonJobRealtions = manuFacePlateButtonJobRelationDtos.Where(c => c.FacePlateButtonId == manuFacePlateButtonDto.Id);
                        manuFacePlateButtonDto.ManuFacePlateButtonJobRelations = buttonJobRealtions.ToArray();
                        manuFacePlateButtons.Add(manuFacePlateButtonDto);
                    }
                    facePlateQueryDto.ManuFacePlateButtons = manuFacePlateButtons.ToArray();
                }
                #endregion

                #region 填充关联表数据
                //资源
                if (resourceId > 0)
                {
                    var procResourceEntity = await _procResourceRepository.GetResByIdAsync(resourceId);
                    if (procResourceEntity != null)
                    {   //在制品维修
                        if (manuFacePlateEntity.Type == Core.Enums.Manufacture.ManuFacePlateTypeEnum.ProductionRepair)
                        {
                            facePlateQueryDto.FacePlateRepair.ResourceName = procResourceEntity.ResName;
                            facePlateQueryDto.FacePlateRepair.ResourceCode = procResourceEntity.ResCode;
                        }
                        //生产过站
                        if (manuFacePlateEntity.Type == Core.Enums.Manufacture.ManuFacePlateTypeEnum.ProducePassingStation)
                        {
                            facePlateQueryDto.FacePlateProduction.ResourceName = procResourceEntity.ResName;
                            facePlateQueryDto.FacePlateProduction.ResourceCode = procResourceEntity.ResCode;
                        }
                        //容器包装
                        if (manuFacePlateEntity.Type == Core.Enums.Manufacture.ManuFacePlateTypeEnum.ContainerPack)
                        {
                            facePlateQueryDto.FacePlateContainerPack.ResourceName = procResourceEntity.ResName;
                            facePlateQueryDto.FacePlateContainerPack.ResourceCode = procResourceEntity.ResCode;
                        }
                    }
                }
                //工序
                if (procedureId > 0)
                {
                    var procProcedureEntity = await _procProcedureRepository.GetByIdAsync(procedureId);
                    if (procProcedureEntity != null)
                    {
                        //在制品维修
                        if (manuFacePlateEntity.Type == Core.Enums.Manufacture.ManuFacePlateTypeEnum.ProductionRepair)
                        {
                            facePlateQueryDto.FacePlateRepair.ProcedureName = procProcedureEntity.Name;
                            facePlateQueryDto.FacePlateRepair.ProcedureCode = procProcedureEntity.Code;
                        }
                        //生产过站
                        if (manuFacePlateEntity.Type == Core.Enums.Manufacture.ManuFacePlateTypeEnum.ProducePassingStation)
                        {
                            facePlateQueryDto.FacePlateProduction.ProcedureName = procProcedureEntity.Name;
                            facePlateQueryDto.FacePlateProduction.ProcedureCode = procProcedureEntity.Code;
                        }
                        //容器包装
                        if (manuFacePlateEntity.Type == Core.Enums.Manufacture.ManuFacePlateTypeEnum.ContainerPack)
                        {
                            facePlateQueryDto.FacePlateContainerPack.ProcedureName = procProcedureEntity.Name;
                            facePlateQueryDto.FacePlateContainerPack.ProcedureCode = procProcedureEntity.Code;
                        }
                    }
                }
                #endregion
            }
            return facePlateQueryDto;
        }

        /// <summary>
        /// 批量查询Job编码
        /// </summary>
        /// <param name="scanJobIdStr"></param>
        /// <returns>返回使用,逗号分割的Code</returns>
        private async Task<string> QueryInteJobCodes(string scanJobIdStr)
        {
            string jobCodeStr = string.Empty;
            if (!string.IsNullOrEmpty(scanJobIdStr))
            {
                var scanJobIdStrArry = scanJobIdStr.Split(',');
                List<long> longs = new List<long>();
                foreach (var jobidStr in scanJobIdStrArry)
                {
                    //忽略转换失败的Id
                    if (long.TryParse(jobidStr, out long jobid))
                    {
                        longs.Add(jobid);
                    }
                }
                var inteJobEntity = await _inteJobRepository.GetByIdsAsync(longs.ToArray());
                if (inteJobEntity != null)
                {
                    jobCodeStr = string.Join(",", inteJobEntity.Select(c => c.Code));
                }
            }
            return jobCodeStr;
        }

        /// <summary>
        /// 根据Code查询
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public async Task<ManuFacePlateQueryDto> QueryManuFacePlateByCodeAsync(string code)
        {
            ManuFacePlateQueryDto facePlateQueryDto = new ManuFacePlateQueryDto();
            var manuFacePlateEntity = await _manuFacePlateRepository.GetByCodeAsync(new EntityByCodeQuery
            {
                Code = code,
                Site = _currentSite.SiteId
            });
            if (manuFacePlateEntity != null)
            {
                long resourceId = 0;
                long procedureId = 0;
                facePlateQueryDto.FacePlate = manuFacePlateEntity.ToModel<ManuFacePlateDto>();
                #region 生产过站
                if (manuFacePlateEntity.Type == Core.Enums.Manufacture.ManuFacePlateTypeEnum.ProducePassingStation)
                {
                    var manuFacePlateProductionEntity = await _manuFacePlateProductionRepository.GetByFacePlateIdAsync(manuFacePlateEntity.Id);
                    if (manuFacePlateProductionEntity != null)
                    {
                        resourceId = manuFacePlateProductionEntity.ResourceId;
                        procedureId = manuFacePlateProductionEntity.ProcedureId;

                        facePlateQueryDto.FacePlateProduction = manuFacePlateProductionEntity.ToModel<ManuFacePlateProductionDto>();
                        //查询时Production主键使用主表的Id返回
                        facePlateQueryDto.FacePlateProduction.Id = manuFacePlateEntity.Id;
                        //颜色不为空就显示
                        facePlateQueryDto.FacePlateProduction.IsShowQualifiedColour = !string.IsNullOrEmpty(facePlateQueryDto.FacePlateProduction.QualifiedColour);
                        facePlateQueryDto.FacePlateProduction.IsShowUnqualifiedColour = !string.IsNullOrEmpty(facePlateQueryDto.FacePlateProduction.UnqualifiedColour);
                        //填充Job数据
                        facePlateQueryDto.FacePlateProduction.ScanJobCode = await QueryInteJobCodes(manuFacePlateProductionEntity.ScanJobId);
                    }
                }
                #endregion

                #region 在制品维修
                //在制品维修
                if (manuFacePlateEntity.Type == Core.Enums.Manufacture.ManuFacePlateTypeEnum.ProductionRepair)
                {
                    var manuFacePlateRepairEntity = await _manuFacePlateRepairRepository.GetByFacePlateIdAsync(manuFacePlateEntity.Id);
                    if (manuFacePlateRepairEntity != null)
                    {
                        resourceId = manuFacePlateRepairEntity.ResourceId;
                        procedureId = manuFacePlateRepairEntity.ProcedureId;
                        facePlateQueryDto.FacePlateRepair = manuFacePlateRepairEntity.ToModel<ManuFacePlateRepairDto>();
                    }
                }
                #endregion
                #region 容器包装
                //容器包装
                if (manuFacePlateEntity.Type == Core.Enums.Manufacture.ManuFacePlateTypeEnum.ContainerPack)
                {
                    var manuFacePlateRepairEntity = await _manuFacePlateContainerPackRepository.GetByFacePlateIdAsync(manuFacePlateEntity.Id);
                    if (manuFacePlateRepairEntity != null)
                    {
                        resourceId = manuFacePlateRepairEntity.ResourceId;
                        procedureId = manuFacePlateRepairEntity.ProcedureId;
                        facePlateQueryDto.FacePlateContainerPack = manuFacePlateRepairEntity.ToModel<ManuFacePlateContainerPackDto>();
                        facePlateQueryDto.FacePlateContainerPack.IsShowErrorsColour = !string.IsNullOrEmpty(manuFacePlateRepairEntity.ErrorsColour);
                        facePlateQueryDto.FacePlateContainerPack.IsShowQualifiedColour = !string.IsNullOrEmpty(manuFacePlateRepairEntity.QualifiedColour);
                    }
                }
                #endregion

                #region 按钮信息
                var manuFacePlateButtonEntityList = (await _manuFacePlateButtonRepository.GetByFacePlateIdAsync(manuFacePlateEntity.Id)).OrderBy(x => x.Seq).ToList();
                //按钮信息
                List<ManuFacePlateButtonDto> manuFacePlateButtons = new();
                foreach (var manuFacePlateButtonEntity in manuFacePlateButtonEntityList)
                {
                    var manuFacePlateButtonDto = manuFacePlateButtonEntity.ToModel<ManuFacePlateButtonDto>();
                    manuFacePlateButtons.Add(manuFacePlateButtonDto);
                }
                facePlateQueryDto.ManuFacePlateButtons = manuFacePlateButtons.ToArray();
                #endregion

                #region 填充关联表数据
                //资源
                if (resourceId > 0)
                {
                    var procResourceEntity = await _procResourceRepository.GetResByIdAsync(resourceId);
                    if (procResourceEntity != null)
                    {   //在制品维修
                        if (manuFacePlateEntity.Type == Core.Enums.Manufacture.ManuFacePlateTypeEnum.ProductionRepair)
                        {
                            facePlateQueryDto.FacePlateRepair.ResourceName = procResourceEntity.ResName;
                            facePlateQueryDto.FacePlateRepair.ResourceCode = procResourceEntity.ResCode;
                        }
                        //在制品维修
                        if (manuFacePlateEntity.Type == Core.Enums.Manufacture.ManuFacePlateTypeEnum.ProducePassingStation)
                        {
                            facePlateQueryDto.FacePlateProduction.ResourceName = procResourceEntity.ResName;
                            facePlateQueryDto.FacePlateProduction.ResourceCode = procResourceEntity.ResCode;
                        }
                        //容器包装
                        if (manuFacePlateEntity.Type == Core.Enums.Manufacture.ManuFacePlateTypeEnum.ContainerPack)
                        {
                            facePlateQueryDto.FacePlateContainerPack.ResourceName = procResourceEntity.ResName;
                            facePlateQueryDto.FacePlateContainerPack.ResourceCode = procResourceEntity.ResCode;
                        }
                    }
                }
                //工序
                if (procedureId > 0)
                {
                    var procProcedureEntity = await _procProcedureRepository.GetByIdAsync(procedureId);
                    if (procProcedureEntity != null)
                    {
                        //在制品维修
                        if (manuFacePlateEntity.Type == Core.Enums.Manufacture.ManuFacePlateTypeEnum.ProductionRepair)
                        {
                            facePlateQueryDto.FacePlateRepair.ProcedureName = procProcedureEntity.Name;
                            facePlateQueryDto.FacePlateRepair.ProcedureCode = procProcedureEntity.Code;
                        }
                        //在制品维修
                        if (manuFacePlateEntity.Type == Core.Enums.Manufacture.ManuFacePlateTypeEnum.ProducePassingStation)
                        {
                            facePlateQueryDto.FacePlateProduction.ProcedureName = procProcedureEntity.Name;
                            facePlateQueryDto.FacePlateProduction.ProcedureCode = procProcedureEntity.Code;
                        }
                        if (manuFacePlateEntity.Type == Core.Enums.Manufacture.ManuFacePlateTypeEnum.ContainerPack)
                        {
                            facePlateQueryDto.FacePlateContainerPack.ProcedureName = procProcedureEntity.Name;
                            facePlateQueryDto.FacePlateContainerPack.ProcedureCode = procProcedureEntity.Code;
                            facePlateQueryDto.FacePlateContainerPack.PackingLevel = procProcedureEntity.PackingLevel;
                        }
                    }
                }
                #endregion
            }
            else
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16705));
            }
            return facePlateQueryDto;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="addManuFacePlateDto"></param>
        /// <returns></returns>
        /// <exception cref="ValidationException"></exception>
        public async Task AddManuFacePlateAsync(AddManuFacePlateDto addManuFacePlateDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0)
            {
                throw new ValidationException(nameof(ErrorCode.MES10101));
            }

            //验证FacePlate DTO
            await _validationCreateRules.ValidateAndThrowAsync(addManuFacePlateDto.FacePlate);
            //DTO转换实体
            var manuFacePlateEntity = addManuFacePlateDto.FacePlate.ToEntity<ManuFacePlateEntity>();
            manuFacePlateEntity.Id = IdGenProvider.Instance.CreateId();
            manuFacePlateEntity.CreatedBy = _currentUser.UserName;
            manuFacePlateEntity.UpdatedBy = _currentUser.UserName;
            manuFacePlateEntity.CreatedOn = HymsonClock.Now();
            manuFacePlateEntity.UpdatedOn = HymsonClock.Now();
            manuFacePlateEntity.SiteId = _currentSite.SiteId ?? 123456;

            #region 生产过站
            ManuFacePlateProductionEntity manuFacePlateProductionEntity = new ManuFacePlateProductionEntity();
            if (manuFacePlateEntity.Type == Core.Enums.Manufacture.ManuFacePlateTypeEnum.ProducePassingStation)
            {
                //验证Production DTO
                await _validationProductionCreateRules.ValidateAndThrowAsync(addManuFacePlateDto.FacePlateProduction);
                // manuFacePlateProduction表
                manuFacePlateProductionEntity = addManuFacePlateDto.FacePlateProduction.ToEntity<ManuFacePlateProductionEntity>();
                manuFacePlateProductionEntity.FacePlateId = manuFacePlateEntity.Id;
                manuFacePlateProductionEntity.Id = IdGenProvider.Instance.CreateId();
                manuFacePlateProductionEntity.CreatedBy = _currentUser.UserName;
                manuFacePlateProductionEntity.UpdatedBy = _currentUser.UserName;
                manuFacePlateProductionEntity.CreatedOn = HymsonClock.Now();
                manuFacePlateProductionEntity.UpdatedOn = HymsonClock.Now();
                manuFacePlateProductionEntity.SiteId = _currentSite.SiteId ?? 123456;
            }
            #endregion

            #region 在制品维修
            ManuFacePlateRepairEntity manuFacePlateRepairEntity = new ManuFacePlateRepairEntity();
            if (manuFacePlateEntity.Type == Core.Enums.Manufacture.ManuFacePlateTypeEnum.ProductionRepair)
            {
                //验证在制品维修
                await _validationRepairCreateRules.ValidateAndThrowAsync(addManuFacePlateDto.FacePlateRepair);
                // manuFacePlateRepairEntity表
                manuFacePlateRepairEntity = addManuFacePlateDto.FacePlateRepair.ToEntity<ManuFacePlateRepairEntity>();
                manuFacePlateRepairEntity.FacePlateId = manuFacePlateEntity.Id;
                manuFacePlateRepairEntity.Id = IdGenProvider.Instance.CreateId();
                manuFacePlateRepairEntity.CreatedBy = _currentUser.UserName;
                manuFacePlateRepairEntity.UpdatedBy = _currentUser.UserName;
                manuFacePlateRepairEntity.CreatedOn = HymsonClock.Now();
                manuFacePlateRepairEntity.UpdatedOn = HymsonClock.Now();
                manuFacePlateRepairEntity.SiteId = _currentSite.SiteId ?? 123456;
            }
            #endregion

            #region 容器包装
            ManuFacePlateContainerPackEntity manuFacePlateContainerPackEntity = new ManuFacePlateContainerPackEntity();
            if (manuFacePlateEntity.Type == Core.Enums.Manufacture.ManuFacePlateTypeEnum.ContainerPack)
            {
                //验证容器包装
                await _validationContainerPackCreateRules.ValidateAndThrowAsync(addManuFacePlateDto.FacePlateContainerPack);
                // manuFacePlateContainerPackEntity表
                manuFacePlateContainerPackEntity = addManuFacePlateDto.FacePlateContainerPack.ToEntity<ManuFacePlateContainerPackEntity>();
                manuFacePlateContainerPackEntity.FacePlateId = manuFacePlateEntity.Id;
                manuFacePlateContainerPackEntity.Id = IdGenProvider.Instance.CreateId();
                manuFacePlateContainerPackEntity.CreatedBy = _currentUser.UserName;
                manuFacePlateContainerPackEntity.UpdatedBy = _currentUser.UserName;
                manuFacePlateContainerPackEntity.CreatedOn = HymsonClock.Now();
                manuFacePlateContainerPackEntity.UpdatedOn = HymsonClock.Now();
                manuFacePlateContainerPackEntity.SiteId = _currentSite.SiteId ?? 123456;
            }
            #endregion


            #region 按钮列表
            var validationFailures = new List<ValidationFailure>();
            List<ManuFacePlateButtonEntity> manuFacePlateButtonEntityList = new List<ManuFacePlateButtonEntity>();
            //按钮关联JOB
            List<ManuFacePlateButtonJobRelationEntity> manuFacePlateButtonJobRelationEntityList = new List<ManuFacePlateButtonJobRelationEntity>();
            if (addManuFacePlateDto?.FacePlateButtonList?.Count > 0)
            {
                int x = 0;
                foreach (var manuFacePlateButtonCreateDto in addManuFacePlateDto.FacePlateButtonList)
                {
                    x++;
                    if (string.IsNullOrWhiteSpace(manuFacePlateButtonCreateDto.Name.Trim()) || manuFacePlateButtonCreateDto.Name.Length > 50)
                    {
                        var validationFailure = new ValidationFailure();
                        if (validationFailure.FormattedMessagePlaceholderValues == null || !validationFailure.FormattedMessagePlaceholderValues.Any())
                        {
                            validationFailure.FormattedMessagePlaceholderValues = new Dictionary<string, object> { { "CollectionIndex", x } };
                        }
                        else
                        {
                            validationFailure.FormattedMessagePlaceholderValues.Add("CollectionIndex", x);
                        }
                        validationFailure.ErrorCode = nameof(ErrorCode.MES17251);
                        validationFailures.Add(validationFailure);
                        continue;
                    }
                    if (manuFacePlateButtonCreateDto.Seq <= 0)
                    {
                        var validationFailure = new ValidationFailure();
                        if (validationFailure.FormattedMessagePlaceholderValues == null || !validationFailure.FormattedMessagePlaceholderValues.Any())
                        {
                            validationFailure.FormattedMessagePlaceholderValues = new Dictionary<string, object> { { "CollectionIndex", x } };
                        }
                        else
                        {
                            validationFailure.FormattedMessagePlaceholderValues.Add("CollectionIndex", x);
                        }
                        validationFailure.ErrorCode = nameof(ErrorCode.MES17252);
                        validationFailures.Add(validationFailure);
                        continue;
                    }
                    //验证按钮DTO
                    await _validationButtonCreateRules.ValidateAndThrowAsync(manuFacePlateButtonCreateDto);
                    var manuFacePlateButtonEntity = manuFacePlateButtonCreateDto.ToEntity<ManuFacePlateButtonEntity>();
                    manuFacePlateButtonEntity.Id = IdGenProvider.Instance.CreateId();
                    manuFacePlateButtonEntity.CreatedBy = _currentUser.UserName;
                    manuFacePlateButtonEntity.CreatedOn = HymsonClock.Now();
                    manuFacePlateButtonEntity.UpdatedBy = _currentUser.UserName;
                    manuFacePlateButtonEntity.UpdatedOn = HymsonClock.Now();
                    manuFacePlateButtonEntity.FacePlateId = manuFacePlateEntity.Id;
                    manuFacePlateButtonEntity.SiteId = _currentSite.SiteId ?? 123456;
                    manuFacePlateButtonEntityList.Add(manuFacePlateButtonEntity);
                    int i = 0;
                    //按钮关联JOb关系表
                    foreach (var manuFacePlateButtonJobRelations in manuFacePlateButtonCreateDto.ManuFacePlateButtonJobRelations)
                    {
                        i++;
                        if (manuFacePlateButtonJobRelations.Seq <= 0)
                        {
                            var validationFailure = new ValidationFailure();
                            if (validationFailure.FormattedMessagePlaceholderValues == null || !validationFailure.FormattedMessagePlaceholderValues.Any())
                            {
                                validationFailure.FormattedMessagePlaceholderValues = new Dictionary<string, object> { { "CollectionIndex", i } };
                            }
                            else
                            {
                                validationFailure.FormattedMessagePlaceholderValues.Add("CollectionIndex", x);
                            }
                            var MES17253 = string.Format(ErrorCode.MES17253, i);
                            validationFailure.ErrorCode = nameof(MES17253);
                            validationFailures.Add(validationFailure);
                            continue;
                        }
                        if (manuFacePlateButtonJobRelations.JobId <= 0)
                        {
                            var validationFailure = new ValidationFailure();
                            if (validationFailure.FormattedMessagePlaceholderValues == null || !validationFailure.FormattedMessagePlaceholderValues.Any())
                            {
                                validationFailure.FormattedMessagePlaceholderValues = new Dictionary<string, object> { { "CollectionIndex", i } };
                            }
                            else
                            {
                                validationFailure.FormattedMessagePlaceholderValues.Add("CollectionIndex", x);
                            }
                            var MES17254 = string.Format(ErrorCode.MES17254, i);
                            validationFailure.ErrorCode = nameof(MES17254);
                            validationFailures.Add(validationFailure);
                            continue;
                        }
                        var manuFacePlateButtonJobRelationEntity = manuFacePlateButtonJobRelations.ToEntity<ManuFacePlateButtonJobRelationEntity>();
                        manuFacePlateButtonJobRelationEntity.Id = IdGenProvider.Instance.CreateId();
                        manuFacePlateButtonJobRelationEntity.CreatedBy = _currentUser.UserName;
                        manuFacePlateButtonJobRelationEntity.CreatedOn = HymsonClock.Now();
                        manuFacePlateButtonJobRelationEntity.UpdatedBy = _currentUser.UserName;
                        manuFacePlateButtonJobRelationEntity.UpdatedOn = HymsonClock.Now();
                        manuFacePlateButtonJobRelationEntity.FacePlateButtonId = manuFacePlateButtonEntity.Id;
                        manuFacePlateButtonJobRelationEntity.SiteId = _currentSite.SiteId ?? 123456;
                        manuFacePlateButtonJobRelationEntityList.Add(manuFacePlateButtonJobRelationEntity);
                    }
                }
            }
            if (validationFailures.Any())
            {
                throw new ValidationException(_localizationService.GetResource("MES10107"), validationFailures);
            }
            #endregion

            #region 提交
            using (TransactionScope ts = TransactionHelper.GetTransactionScope())
            {
                //入库
                await _manuFacePlateRepository.InsertAsync(manuFacePlateEntity);
                //生产过站
                if (manuFacePlateEntity.Type == Core.Enums.Manufacture.ManuFacePlateTypeEnum.ProducePassingStation)
                {
                    await _manuFacePlateProductionRepository.InsertAsync(manuFacePlateProductionEntity);
                }
                //在制品维修
                if (manuFacePlateEntity.Type == Core.Enums.Manufacture.ManuFacePlateTypeEnum.ProductionRepair)
                {
                    await _manuFacePlateRepairRepository.InsertAsync(manuFacePlateRepairEntity);
                }
                //容器包装
                if (manuFacePlateEntity.Type == Core.Enums.Manufacture.ManuFacePlateTypeEnum.ContainerPack)
                {
                    await _manuFacePlateContainerPackRepository.InsertAsync(manuFacePlateContainerPackEntity);
                }
                //按钮列表
                if (manuFacePlateButtonEntityList.Count > 0)
                {
                    await _manuFacePlateButtonRepository.InsertsAsync(manuFacePlateButtonEntityList);
                }
                //按钮列表关联JOB
                if (manuFacePlateButtonJobRelationEntityList.Count > 0)
                {
                    await _manuFacePlateButtonJobRelationRepository.InsertsAsync(manuFacePlateButtonJobRelationEntityList);
                }
                //提交
                ts.Complete();
            }
            #endregion
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="updateManuFacePlateDto"></param>
        /// <returns></returns>
        /// <exception cref="ValidationException"></exception>
        public async Task UpdateManuFacePlateAsync(UpdateManuFacePlateDto updateManuFacePlateDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0)
            {
                throw new ValidationException(nameof(ErrorCode.MES10101));
            }

            //验证DTO
            await _validationModifyRules.ValidateAndThrowAsync(updateManuFacePlateDto.FacePlate);

            var entity = await _manuFacePlateRepository.GetByIdAsync(updateManuFacePlateDto.FacePlate.Id)
                ?? throw new BusinessException(nameof(ErrorCode.MES17209));

            if (entity.Status != SysDataStatusEnum.Build && updateManuFacePlateDto.FacePlate.Status == SysDataStatusEnum.Build)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10108));
            }

            //DTO转换实体
            var manuFacePlateEntity = updateManuFacePlateDto.FacePlate.ToEntity<ManuFacePlateEntity>();
            manuFacePlateEntity.UpdatedBy = _currentUser.UserName;
            manuFacePlateEntity.UpdatedOn = HymsonClock.Now();
            #region 生产过站
            ManuFacePlateProductionEntity manuFacePlateProductionEntity = new ManuFacePlateProductionEntity();
            if (manuFacePlateEntity.Type == Core.Enums.Manufacture.ManuFacePlateTypeEnum.ProducePassingStation)
            {
                await _validationProductionModifyRules.ValidateAndThrowAsync(updateManuFacePlateDto.FacePlateProduction);
                manuFacePlateProductionEntity = updateManuFacePlateDto.FacePlateProduction.ToEntity<ManuFacePlateProductionEntity>();
                manuFacePlateProductionEntity.UpdatedBy = _currentUser.UserName;
                manuFacePlateProductionEntity.UpdatedOn = HymsonClock.Now();
                manuFacePlateProductionEntity.FacePlateId = manuFacePlateEntity.Id;
            }
            #endregion

            #region 在制品维修
            ManuFacePlateRepairEntity manuFacePlateRepairEntity = new ManuFacePlateRepairEntity();
            if (manuFacePlateEntity.Type == Core.Enums.Manufacture.ManuFacePlateTypeEnum.ProductionRepair)
            {
                await _validationRepairModifyRules.ValidateAndThrowAsync(updateManuFacePlateDto.FacePlateRepair);
                manuFacePlateRepairEntity = updateManuFacePlateDto.FacePlateRepair.ToEntity<ManuFacePlateRepairEntity>();
                manuFacePlateRepairEntity.UpdatedBy = _currentUser.UserName;
                manuFacePlateRepairEntity.UpdatedOn = HymsonClock.Now();
                manuFacePlateRepairEntity.FacePlateId = manuFacePlateEntity.Id;
            }
            #endregion

            #region  容器装箱
            ManuFacePlateContainerPackEntity manuFacePlateContainerPackEntity = new ManuFacePlateContainerPackEntity();
            if (manuFacePlateEntity.Type == Core.Enums.Manufacture.ManuFacePlateTypeEnum.ContainerPack)
            {
                await _validationContainerPackModifyRules.ValidateAndThrowAsync(updateManuFacePlateDto.FacePlateContainerPack);
                manuFacePlateContainerPackEntity = updateManuFacePlateDto.FacePlateContainerPack.ToEntity<ManuFacePlateContainerPackEntity>();
                manuFacePlateContainerPackEntity.UpdatedBy = _currentUser.UserName;
                manuFacePlateContainerPackEntity.UpdatedOn = HymsonClock.Now();
                manuFacePlateContainerPackEntity.FacePlateId = manuFacePlateEntity.Id;
            }
            #endregion

            #region 按钮列表
            var validationFailures = new List<ValidationFailure>();

            //保存id用于删除原有关联关系
            List<long> facePlateButtonIds = new List<long>();
            List<ManuFacePlateButtonEntity> manuFacePlateButtonEntityList = new List<ManuFacePlateButtonEntity>();
            //按钮关联JOB
            List<ManuFacePlateButtonJobRelationEntity> manuFacePlateButtonJobRelationEntityList = new List<ManuFacePlateButtonJobRelationEntity>();
            List<long> FacePlateButtonIdList = new List<long>();
            if (updateManuFacePlateDto?.FacePlateButtonList?.Count > 0)
            {
                //查询原有面板对应的所有按钮
                var manuFacePlateButtonList = await _manuFacePlateButtonRepository.GetByFacePlateIdAsync(manuFacePlateEntity.Id);
                facePlateButtonIds = manuFacePlateButtonList.Select(c => c.Id).ToList();
                int x = 0;
                //处理按钮列表
                foreach (var manuFacePlateButtonModifyDto in updateManuFacePlateDto.FacePlateButtonList)
                {
                    //验证按钮DTO
                    await _validationButtonModifyRules.ValidateAndThrowAsync(manuFacePlateButtonModifyDto);
                    x++;
                    if (string.IsNullOrWhiteSpace(manuFacePlateButtonModifyDto.Name.Trim()) || manuFacePlateButtonModifyDto.Name.Length > 50)
                    {
                        var validationFailure = new ValidationFailure();
                        if (validationFailure.FormattedMessagePlaceholderValues == null || !validationFailure.FormattedMessagePlaceholderValues.Any())
                        {
                            validationFailure.FormattedMessagePlaceholderValues = new Dictionary<string, object> { { "CollectionIndex", x } };
                        }
                        else
                        {
                            validationFailure.FormattedMessagePlaceholderValues.Add("CollectionIndex", x);
                        }
                        validationFailure.ErrorCode = nameof(ErrorCode.MES17251);
                        validationFailures.Add(validationFailure);
                        continue;
                    }
                    if (manuFacePlateButtonModifyDto.Seq <= 0)
                    {
                        var validationFailure = new ValidationFailure();
                        if (validationFailure.FormattedMessagePlaceholderValues == null || !validationFailure.FormattedMessagePlaceholderValues.Any())
                        {
                            validationFailure.FormattedMessagePlaceholderValues = new Dictionary<string, object> { { "CollectionIndex", x } };
                        }
                        else
                        {
                            validationFailure.FormattedMessagePlaceholderValues.Add("CollectionIndex", x);
                        }
                        validationFailure.ErrorCode = nameof(ErrorCode.MES17252);
                        validationFailures.Add(validationFailure);
                        continue;
                    }


                    var manuFacePlateButtonEntity = manuFacePlateButtonModifyDto.ToEntity<ManuFacePlateButtonEntity>();
                    //重新生成ID
                    manuFacePlateButtonEntity.Id = IdGenProvider.Instance.CreateId();
                    manuFacePlateButtonEntity.UpdatedBy = _currentUser.UserName;
                    manuFacePlateButtonEntity.UpdatedOn = HymsonClock.Now();
                    manuFacePlateButtonEntity.FacePlateId = manuFacePlateEntity.Id;
                    manuFacePlateButtonEntity.SiteId = _currentSite.SiteId??0;
                    manuFacePlateButtonEntityList.Add(manuFacePlateButtonEntity);
                    int i = 0;
                    //按钮关联JOb关系表
                    foreach (var manuFacePlateButtonJobRelations in manuFacePlateButtonModifyDto.ManuFacePlateButtonJobRelations)
                    {
                        i++;
                        if (manuFacePlateButtonJobRelations.Seq <= 0)
                        {
                            var validationFailure = new ValidationFailure();
                            if (validationFailure.FormattedMessagePlaceholderValues == null || !validationFailure.FormattedMessagePlaceholderValues.Any())
                            {
                                validationFailure.FormattedMessagePlaceholderValues = new Dictionary<string, object> { { "CollectionIndex", i } };
                            }
                            else
                            {
                                validationFailure.FormattedMessagePlaceholderValues.Add("CollectionIndex", x);
                            }
                            validationFailure.ErrorCode = nameof(ErrorCode.MES17253);
                            validationFailures.Add(validationFailure);
                            continue;
                        }
                        if (manuFacePlateButtonJobRelations.JobId <= 0)
                        {
                            var validationFailure = new ValidationFailure();
                            if (validationFailure.FormattedMessagePlaceholderValues == null || !validationFailure.FormattedMessagePlaceholderValues.Any())
                            {
                                validationFailure.FormattedMessagePlaceholderValues = new Dictionary<string, object> { { "CollectionIndex", i } };
                            }
                            else
                            {
                                validationFailure.FormattedMessagePlaceholderValues.Add("CollectionIndex", x);
                            }
                            validationFailure.ErrorCode = nameof(ErrorCode.MES17254);
                            validationFailures.Add(validationFailure);
                            continue;
                        }
                        var manuFacePlateButtonJobRelationEntity = manuFacePlateButtonJobRelations.ToEntity<ManuFacePlateButtonJobRelationEntity>();
                        manuFacePlateButtonJobRelationEntity.Id = IdGenProvider.Instance.CreateId();
                        manuFacePlateButtonJobRelationEntity.UpdatedBy = _currentUser.UserName;
                        manuFacePlateButtonJobRelationEntity.UpdatedOn = HymsonClock.Now();
                        manuFacePlateButtonJobRelationEntity.FacePlateButtonId = manuFacePlateButtonEntity.Id;
                        manuFacePlateButtonJobRelationEntityList.Add(manuFacePlateButtonJobRelationEntity);
                    }
                }
                if (validationFailures.Any())
                {
                    throw new ValidationException(_localizationService.GetResource("MES10107"), validationFailures);
                }
            }
            #endregion

            #region 提交
            using (TransactionScope ts = TransactionHelper.GetTransactionScope())
            {
                await _manuFacePlateRepository.UpdateAsync(manuFacePlateEntity);
                //生产过站
                if (manuFacePlateEntity.Type == Core.Enums.Manufacture.ManuFacePlateTypeEnum.ProducePassingStation)
                {
                    await _manuFacePlateProductionRepository.UpdateByFacePlateIdAsync(manuFacePlateProductionEntity);
                }
                //在制品维修
                if (manuFacePlateEntity.Type == Core.Enums.Manufacture.ManuFacePlateTypeEnum.ProductionRepair)
                {
                    await _manuFacePlateRepairRepository.UpdateByFacePlateRepairIdAsync(manuFacePlateRepairEntity);
                }
                //容器包装
                if (manuFacePlateEntity.Type == Core.Enums.Manufacture.ManuFacePlateTypeEnum.ContainerPack)
                {
                    await _manuFacePlateContainerPackRepository.UpdateByFacePlateIdAsync(manuFacePlateContainerPackEntity);
                }
                //按钮列表先删除再添加
                await _manuFacePlateButtonRepository.DeleteTrueAsync(manuFacePlateEntity.Id);
                if (manuFacePlateButtonEntityList.Count > 0)
                {
                    await _manuFacePlateButtonRepository.InsertsAsync(manuFacePlateButtonEntityList);
                }
                //按钮列表关联JOB先删除再添加
                if (facePlateButtonIds.Any())
                {
                    await _manuFacePlateButtonJobRelationRepository.DeletesTrueAsync(facePlateButtonIds.ToArray());
                }
                if (manuFacePlateButtonJobRelationEntityList.Count > 0)
                {
                    await _manuFacePlateButtonJobRelationRepository.InsertsAsync(manuFacePlateButtonJobRelationEntityList);
                }

                //提交
                ts.Complete();
            }
            #endregion
        }
    }
}
