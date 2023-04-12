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
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Integrated;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Integrated.IIntegratedRepository;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Services.Dtos.Common;
using Hymson.MES.Services.Dtos.Manufacture;
using Hymson.MES.Services.Services.Job.Common;
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
            AbstractValidator<ManuFacePlateButtonModifyDto> validationModifyRules)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _jobCommonService = jobCommonService;
            _manuFacePlateButtonRepository = manuFacePlateButtonRepository;
            _manuFacePlateButtonJobRelationRepository = manuFacePlateButtonJobRelationRepository;
            _inteJobRepository = inteJobRepository;
            _validationCreateRules = validationCreateRules;
            _validationModifyRules = validationModifyRules;
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
            var pagedInfo = await _manuFacePlateButtonRepository.GetPagedInfoAsync(manuFacePlateButtonPagedQuery);

            //实体到DTO转换 装载数据
            List<ManuFacePlateButtonDto> manuFacePlateButtonDtos = PrepareManuFacePlateButtonDtos(pagedInfo);
            return new PagedInfo<ManuFacePlateButtonDto>(manuFacePlateButtonDtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pagedInfo"></param>
        /// <returns></returns>
        private static List<ManuFacePlateButtonDto> PrepareManuFacePlateButtonDtos(PagedInfo<ManuFacePlateButtonEntity> pagedInfo)
        {
            var manuFacePlateButtonDtos = new List<ManuFacePlateButtonDto>();
            foreach (var manuFacePlateButtonEntity in pagedInfo.Data)
            {
                var manuFacePlateButtonDto = manuFacePlateButtonEntity.ToModel<ManuFacePlateButtonDto>();
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
        /// 按钮（点击）
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<Dictionary<string, int>> ClickAsync(JobDto dto)
        {
            var result = new Dictionary<string, int>(); // 返回结果

            // 根据面板ID和按钮ID找出绑定的作业job
            var buttonJobs = await _manuFacePlateButtonJobRelationRepository.GetByFacePlateButtonIdAsync(dto.FacePlateButtonId);
            if (buttonJobs.Any() == false) return result;

            // 根据 buttonJobs 读取对应的job对象
            var jobs = await _inteJobRepository.GetByIdsAsync(buttonJobs.Select(s => s.JobId).ToArray());

            // 执行Job
            //result = await _jobCommonService.ExecuteJobAsync(new List<InteJobEntity> { }.Select(s => s.Code), dto);
            result = await _jobCommonService.ExecuteJobAsync(jobs.Select(s => s.Code), dto);
            return result;
        }
    }
}
