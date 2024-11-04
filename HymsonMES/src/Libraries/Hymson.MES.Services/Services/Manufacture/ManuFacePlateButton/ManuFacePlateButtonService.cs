/*
 *creator: Karl
 *
 *describe: 操作面板按钮    服务 | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-04-01 02:58:19
 */
using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Integrated;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.CoreServices.Bos.Job;
using Hymson.MES.CoreServices.Dtos.Common;
using Hymson.MES.CoreServices.Services.Job;
using Hymson.MES.CoreServices.Services.Job.JobUtility.Execute;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Common.Query;
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
        /// 服务接口（作业通用）
        /// </summary>
        private readonly IJobCommonService _jobCommonService;

        /// <summary>
        /// 操作面板按钮 仓储
        /// </summary>
        private readonly IManuFacePlateButtonRepository _manuFacePlateButtonRepository;

        /// <summary>
        /// 仓储接口（面板按钮作业关系）
        /// </summary>
        private readonly IManuFacePlateButtonJobRelationRepository _manuFacePlateButtonJobRelationRepository;

        /// <summary>
        /// 仓储接口（作业）
        /// </summary>
        private readonly IInteJobRepository _inteJobRepository;


        /// <summary>
        /// 仓储接口（作业）
        /// </summary>
        private readonly IExecuteJobService<JobBaseBo> _executeJobService;

        /// <summary>
        /// 
        /// </summary>
        private readonly AbstractValidator<ManuFacePlateButtonCreateDto> _validationCreateRules;
        private readonly AbstractValidator<ManuFacePlateButtonModifyDto> _validationModifyRules;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="currentSite"></param>
        /// <param name="jobCommonService"></param>
        /// <param name="manuFacePlateButtonRepository"></param>
        /// <param name="manuFacePlateButtonJobRelationRepository"></param>
        /// <param name="inteJobRepository"></param>
        /// <param name="validationCreateRules"></param>
        /// <param name="validationModifyRules"></param>
        public ManuFacePlateButtonService(ICurrentUser currentUser, ICurrentSite currentSite,
            IJobCommonService jobCommonService,
            IManuFacePlateButtonRepository manuFacePlateButtonRepository,
            IManuFacePlateButtonJobRelationRepository manuFacePlateButtonJobRelationRepository,
            IInteJobRepository inteJobRepository,
            AbstractValidator<ManuFacePlateButtonCreateDto> validationCreateRules,
            AbstractValidator<ManuFacePlateButtonModifyDto> validationModifyRules, IExecuteJobService<JobBaseBo> executeJobService)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _jobCommonService = jobCommonService;
            _manuFacePlateButtonRepository = manuFacePlateButtonRepository;
            _manuFacePlateButtonJobRelationRepository = manuFacePlateButtonJobRelationRepository;
            _inteJobRepository = inteJobRepository;
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
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0)
            {
                throw new ValidationException(nameof(ErrorCode.MES10101));
            }

            //验证DTO
            await _validationCreateRules.ValidateAndThrowAsync(manuFacePlateButtonCreateDto);

            //DTO转换实体
            var manuFacePlateButtonEntity = manuFacePlateButtonCreateDto.ToEntity<ManuFacePlateButtonEntity>();
            manuFacePlateButtonEntity.Id = IdGenProvider.Instance.CreateId();
            manuFacePlateButtonEntity.CreatedBy = _currentUser.UserName;
            manuFacePlateButtonEntity.UpdatedBy = _currentUser.UserName;
            manuFacePlateButtonEntity.CreatedOn = HymsonClock.Now();
            manuFacePlateButtonEntity.UpdatedOn = HymsonClock.Now();
            manuFacePlateButtonEntity.SiteId = _currentSite.SiteId ?? 123456;

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
            manuFacePlateButtonPagedQuery.SiteId = _currentSite.SiteId ?? 123456;
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
                var jobEntity = inteJobEntitys.Where(c => c.Id == manuFacePlateButtonJobRelationDto.JobId).FirstOrDefault();
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
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0)
            {
                throw new ValidationException(nameof(ErrorCode.MES10101));
            }

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
            return null;
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
                var jobEntity = inteJobEntitys.Where(c => c.Id == manuFacePlateButtonJobRelationDto.JobId).FirstOrDefault();
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
            if (buttonJobs.Any() == false) return result;

            // 根据 buttonJobs 读取对应的job对象
            var jobsOfNotSort = await _inteJobRepository.GetEntitiesAsync(new EntityBySiteIdQuery { SiteId = _currentSite.SiteId ?? 123456 });

            List<InteJobEntity> jobs = new();
            foreach (var item in buttonJobs)
            {
                var tempJob = jobsOfNotSort.FirstOrDefault(f => f.Id == item.JobId);
                if (tempJob == null) continue;

                jobs.Add(tempJob);
            }

            // 是否清除条码
            if (buttonJobs.Any(a => a.IsClear) == true) dto.Param?.Add("IsClear", "True");

            /*
            // 执行Job
            var jobResponses = await _executeJobService.ExecuteAsync(jobs.Select(s => new JobBo { Name = s.ClassProgram }), new JobRequestBo
            {
                SiteId = _currentSite.SiteId ?? 123456,
                UserName = _currentUser.UserName,
                ProcedureId = dto.Param["ProcedureId"].ParseToLong(),
                ResourceId = dto.Param["ResourceId"].ParseToLong(),
                SFCs = new string[] { dto.Param["SFC"] }
            });
            foreach (var item in jobResponses)
            {
                result.Add(item.Key, new JobResponseDto
                {
                    Rows = item.Value.Rows,
                    Content = item.Value.Content,
                    Message = item.Value.Message,
                    Time = item.Value.Time
                });
            }
            */

            result = await _jobCommonService.ExecuteJobAsync(jobs, dto.Param);
            return result;
        }


        /// <summary>
        ///  新按钮（点击）
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<Dictionary<string, JobResponseBo>> NewClickAsync(ButtonRequestDto dto, dynamic bo)
        {
            var result = new Dictionary<string, JobResponseBo> { }; // 返回结果

            // 先检查按钮是否存在
            var button = await _manuFacePlateButtonRepository.GetByIdAsync(dto.FacePlateButtonId)
                ?? throw new CustomerValidationException(nameof(ErrorCode.MES17208));

            // 根据面板ID和按钮ID找出绑定的作业job
            var buttonJobs = await _manuFacePlateButtonJobRelationRepository.GetByFacePlateButtonIdAsync(dto.FacePlateButtonId);
            if (buttonJobs.Any() == false) return result;

            // 根据 buttonJobs 读取对应的job对象
            var jobs = await _inteJobRepository.GetByIdsAsync(buttonJobs.Select(s => s.JobId).ToArray());

            // 是否清除条码
            if (buttonJobs.Any(a => a.IsClear) == true) dto.Param?.Add("IsClear", "True");

            var jobBos = new List<JobBo>();
            foreach (var job in jobs)
            {
                jobBos.Add(new JobBo { Name = job.ClassProgram });
            }
            result = await _executeJobService.ExecuteAsync(jobBos, bo);
            // 执行Job
            // result = await _jobCommonService.ExecuteJobAsync(jobs, dto.Param);
            return result;
        }

    }
}
