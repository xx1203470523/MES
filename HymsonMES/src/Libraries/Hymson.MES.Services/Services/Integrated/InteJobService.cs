using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Integrated;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.MES.Data.Repositories.Integrated.IIntegratedRepository;
using Hymson.MES.Data.Repositories.Integrated.InteJob.Query;
using Hymson.MES.Services.Dtos.Integrated;
using Hymson.MES.Services.Services.Integrated.IIntegratedService;
using Hymson.Snowflake;
using Hymson.Utils;

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
        private readonly AbstractValidator<InteJobCreateDto> _validationCreateRules;
        private readonly AbstractValidator<InteJobModifyDto> _validationModifyRules;
        private readonly ICurrentUser _currentUser;
        private readonly ICurrentSite _currentSite;

        /// <summary>
        /// 作业表服务
        /// </summary>
        /// <param name="inteJobRepository"></param>
        /// <param name="validationCreateRules"></param>
        /// <param name="validationModifyRules"></param>
        /// <param name="currentUser"></param>
        /// <param name="currentSite"></param>
        public InteJobService(IInteJobRepository inteJobRepository, AbstractValidator<InteJobCreateDto> validationCreateRules, AbstractValidator<InteJobModifyDto> validationModifyRules, ICurrentUser currentUser, ICurrentSite currentSite)
        {
            _inteJobRepository = inteJobRepository;
            _validationCreateRules = validationCreateRules;
            _validationModifyRules = validationModifyRules;
            _currentUser = currentUser;
            _currentSite = currentSite;
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="pram"></param>
        /// <returns></returns>
        public async Task<PagedInfo<InteJobDto>> GetPageListAsync(InteJobPagedQueryDto pram)
        {
            var inteJobPagedQuery = pram.ToQuery<InteJobPagedQuery>();
            inteJobPagedQuery.SiteId = _currentSite.SiteId ;
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
            return null;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="param">新增参数</param>
        /// <returns></returns>
        /// <exception cref="ValidationException">参数为空</exception>
        /// <exception cref="BusinessException">编码复用</exception>
        public async Task CreateInteJobAsync(InteJobCreateDto param)
        {
            if (param == null)
            {
                throw new ValidationException(nameof(ErrorCode.MES10100));
            }
            //验证DTO
            await _validationCreateRules.ValidateAndThrowAsync(param);

            var inteJobEntity = await _inteJobRepository.GetByCodeAsync(new EntityByCodeQuery { Code = param.Code, Site = _currentSite.SiteId });
            if (inteJobEntity != null)
            {
                throw new BusinessException(nameof(ErrorCode.MES12001)).WithData("code", param.Code);
            }
            var userId = _currentUser.UserName;
            //DTO转换实体
            inteJobEntity = param.ToEntity<InteJobEntity>();
            inteJobEntity.Id = IdGenProvider.Instance.CreateId();
            inteJobEntity.CreatedBy = userId;
            inteJobEntity.UpdatedBy = userId;
            inteJobEntity.SiteId = _currentSite.SiteId ?? 0;

            await _inteJobRepository.InsertAsync(inteJobEntity);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeleteRangInteJobAsync(long[] ids)
        {
            var userId = _currentUser.UserName;
            return await _inteJobRepository.DeleteRangAsync(new DeleteCommand { Ids = ids, DeleteOn = HymsonClock.Now(), UserId = userId });
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        /// <exception cref="ValidationException">参数为空</exception>
        public async Task ModifyInteJobAsync(InteJobModifyDto param)
        {
            if (param == null)
            {
                throw new ValidationException(nameof(ErrorCode.MES10100));
            }
            //验证DTO
            await _validationModifyRules.ValidateAndThrowAsync(param);
            var userId = _currentUser.UserName;
            //DTO转换实体
            var inteJobEntity = param.ToEntity<InteJobEntity>();
            inteJobEntity.UpdatedBy = userId;
            inteJobEntity.UpdatedOn = HymsonClock.Now();

            await _inteJobRepository.UpdateAsync(inteJobEntity); ;
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