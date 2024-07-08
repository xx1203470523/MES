using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Integrated;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.CoreServices.Bos.Job;
using Hymson.MES.CoreServices.Services.Job;
using Hymson.MES.CoreServices.Services.Job.JobUtility.Execute;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.MES.Data.Repositories.Integrated;
using Hymson.MES.Data.Repositories.Integrated.IIntegratedRepository;
using Hymson.MES.Data.Repositories.Integrated.InteJob.Query;
using Hymson.MES.Data.Repositories.Integrated.Query;
using Hymson.MES.Services.Dtos.Integrated;
using Hymson.MES.Services.Dtos.Process;
using Hymson.MES.Services.Services.Integrated.IIntegratedService;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;
using Microsoft.Extensions.DependencyInjection;
using System.Transactions;

namespace Hymson.MES.Services.Services.Integrated
{
    /// <summary>
    /// 作业表服务
    /// @author admin
    /// @date 2023-02-21
    /// </summary>
    public class InteJobService : IInteJobService
    {
        private readonly IInteJobRepository _inteJobRepository;

        /// <summary>
        /// 工序配置作业表仓储
        /// </summary>
        private readonly IInteJobBusinessRelationRepository _jobBusinessRelationRepository;
        private readonly IInteJobConfigRepository _inteJobConfigRepository;
        /// <summary>
        /// 注入反射获取依赖对象
        /// </summary>
        private readonly IServiceProvider _serviceProvider;
        private readonly AbstractValidator<InteJobCreateDto> _validationCreateRules;
        private readonly AbstractValidator<InteJobModifyDto> _validationModifyRules;
        private readonly ICurrentUser _currentUser;
        private readonly ICurrentSite _currentSite;
        private readonly IExecuteJobService<JobBaseBo> _executeJobService;


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="inteJobRepository"></param>
        /// <param name="jobBusinessRelationRepository"></param>
        /// <param name="inteJobConfigRepository"></param>
        /// <param name="executeJobService"></param>
        /// <param name="validationCreateRules"></param>
        /// <param name="validationModifyRules"></param>
        /// <param name="currentUser"></param>
        /// <param name="currentSite"></param>
        /// <param name="serviceProvider"></param>
        public InteJobService(IInteJobRepository inteJobRepository,
            IInteJobBusinessRelationRepository jobBusinessRelationRepository,
            IInteJobConfigRepository inteJobConfigRepository,
            IExecuteJobService<JobBaseBo> executeJobService,
            AbstractValidator<InteJobCreateDto> validationCreateRules, AbstractValidator<InteJobModifyDto> validationModifyRules, ICurrentUser currentUser, ICurrentSite currentSite, IServiceProvider serviceProvider)
        {
            _inteJobRepository = inteJobRepository;
            _jobBusinessRelationRepository = jobBusinessRelationRepository;
            _inteJobConfigRepository = inteJobConfigRepository;
            _validationCreateRules = validationCreateRules;
            _validationModifyRules = validationModifyRules;
            _currentUser = currentUser;
            _currentSite = currentSite;
            _serviceProvider = serviceProvider;
            _executeJobService = executeJobService;
        }


        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="pram"></param>
        /// <returns></returns>
        public async Task<PagedInfo<InteJobDto>> GetPageListAsync(InteJobPagedQueryDto pram)
        {
            var inteJobPagedQuery = pram.ToQuery<InteJobPagedQuery>();
            inteJobPagedQuery.SiteId = _currentSite.SiteId;
            var pagedInfo = await _inteJobRepository.GetPagedInfoAsync(inteJobPagedQuery);

            //实体到DTO转换 装载数据
            List<InteJobDto> inteJobDtos = PrepareInteJobDtos(pagedInfo);
            return new PagedInfo<InteJobDto>(inteJobDtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<InteJobDto> QueryInteJobByIdAsync(long id)
        {
            var inteJobEntity = await _inteJobRepository.GetByIdAsync(id);
            if (inteJobEntity != null)
            {
                return inteJobEntity.ToModel<InteJobDto>();
            }
            return new InteJobDto();
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="param">新增参数</param>
        /// <returns></returns>
        public async Task<long> CreateInteJobAsync(InteJobCreateDto param)
        {
            //验证DTO
            await _validationCreateRules.ValidateAndThrowAsync(param);

            // 获取所有实现类
            var services = _serviceProvider.GetServices<IJobService>();
            if (!services.Any(it => it.GetType().Name == param.ClassProgram))
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES12011));
            }
            var inteJobEntity = await _inteJobRepository.GetByCodeAsync(new EntityByCodeQuery { Code = param.Code, Site = _currentSite.SiteId });
            if (inteJobEntity != null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES12001)).WithData("code", param.Code);
            }

            var userId = _currentUser.UserName;
            //DTO转换实体
            inteJobEntity = param.ToEntity<InteJobEntity>();
            inteJobEntity.Id = IdGenProvider.Instance.CreateId();
            inteJobEntity.CreatedBy = userId;
            inteJobEntity.UpdatedBy = userId;
            inteJobEntity.SiteId = _currentSite.SiteId ?? 0;

            //作业规则数据
            var jobConfigEntities = new List<InteJobConfigEntity>();
            if (param.ConfigList != null && param.ConfigList.Count > 0)
            {
                foreach (var item in param.ConfigList)
                {
                    jobConfigEntities.Add(new InteJobConfigEntity
                    {
                        Id = IdGenProvider.Instance.CreateId(),
                        SiteId = _currentSite.SiteId ??0,
                        JobId = inteJobEntity.Id,
                        SetValue = item.SetValue,
                        RuleName = item.RuleName
                    });
                }
            }

            using (TransactionScope ts = TransactionHelper.GetTransactionScope(TransactionScopeOption.Required, IsolationLevel.ReadCommitted))
            {
                await _inteJobRepository.InsertAsync(inteJobEntity);
                if (jobConfigEntities.Any())
                {
                    await _inteJobConfigRepository.InsertRangeAsync(jobConfigEntities);
                }
                //提交
                ts.Complete();
            }
            return inteJobEntity.Id;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeleteRangInteJobAsync(long[] ids)
        {
            var userId = _currentUser.UserName;

            var list = await _jobBusinessRelationRepository.GetByJobIdsAsync(ids);
            if (list != null && list.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES12009));
            }
            var row = 0;
            using (var trans = TransactionHelper.GetTransactionScope())
            {
                row = await _inteJobRepository.DeleteRangAsync(new DeleteCommand { Ids = ids, DeleteOn = HymsonClock.Now(), UserId = userId });
                if (row != ids.Length)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES12010));
                }
                trans.Complete();
            }
            return row;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task ModifyInteJobAsync(InteJobModifyDto param)
        {
            //验证DTO
            await _validationModifyRules.ValidateAndThrowAsync(param);
            // 获取所有实现类
            var services = _serviceProvider.GetServices<IJobService>();
            if (!services.Any(it => it.GetType().Name == param.ClassProgram))
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES12011));
            }
            var userId = _currentUser.UserName;
            //DTO转换实体
            var inteJobEntity = param.ToEntity<InteJobEntity>();
            inteJobEntity.UpdatedBy = userId;
            inteJobEntity.UpdatedOn = HymsonClock.Now();

            //作业规则数据
            var jobConfigEntities = new List<InteJobConfigEntity>();
            if (param.ConfigList != null && param.ConfigList.Count > 0)
            {
                foreach (var item in param.ConfigList)
                {
                    jobConfigEntities.Add(new InteJobConfigEntity
                    {
                        Id = IdGenProvider.Instance.CreateId(),
                        SiteId = _currentSite.SiteId ?? 0,
                        JobId = inteJobEntity.Id,
                        SetValue = item.SetValue,
                        RuleName = item.RuleName
                    });
                }
            }

            using (TransactionScope ts = TransactionHelper.GetTransactionScope(TransactionScopeOption.Required, IsolationLevel.ReadCommitted))
            {
                await _inteJobRepository.UpdateAsync(inteJobEntity);

                //先删除再加
                await _inteJobConfigRepository.DeleteByJobIdAsync(inteJobEntity.Id);
                if (jobConfigEntities.Any())
                {
                    await _inteJobConfigRepository.InsertRangeAsync(jobConfigEntities);
                }
                //提交
                ts.Complete();
            }
        }

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="jobId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<InteJobConfigDto>> GetConfigByJobIdAsync(long jobId)
        {
            var jobConfigEntities = await _inteJobConfigRepository.GetEntitiesAsync(new InteJobConfigQuery
            {
                SiteId = _currentSite.SiteId ?? 0,
                JobId = jobId
            });

            var jobConfigDtos=new  List<InteJobConfigDto>();
            foreach (var item in jobConfigEntities)
            {
                jobConfigDtos.Add(new InteJobConfigDto
                {
                    SetValue = item.SetValue,
                    RuleName = item.RuleName
                });
            }
            return jobConfigDtos;
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="pagedInfo"></param>
        /// <returns></returns>
        private static List<InteJobDto> PrepareInteJobDtos(PagedInfo<InteJobEntity> pagedInfo)
        {
            var inteJobDtos = new List<InteJobDto>();
            foreach (var inteJobEntity in pagedInfo.Data)
            {
                var inteJobDto = inteJobEntity.ToModel<InteJobDto>();
                inteJobDtos.Add(inteJobDto);
            }
            return inteJobDtos;
        }

    }
}