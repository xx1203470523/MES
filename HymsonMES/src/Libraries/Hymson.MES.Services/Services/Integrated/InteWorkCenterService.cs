using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Integrated;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Integrated;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.MES.Data.Repositories.Integrated.IIntegratedRepository;
using Hymson.MES.Data.Repositories.Integrated.InteWorkCenter.Query;
using Hymson.MES.Services.Dtos.Integrated;
using Hymson.MES.Services.Services.Integrated.IIntegratedService;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;

namespace Hymson.MES.Services.Services.Integrated
{
    /// <summary>
    /// 工作中心表服务
    /// @author admin
    /// @date 2023-02-22
    /// </summary>
    public class InteWorkCenterService : IInteWorkCenterService
    {
        private readonly IInteWorkCenterRepository _inteWorkCenterRepository;
        private readonly AbstractValidator<InteWorkCenterCreateDto> _validationCreateRules;
        private readonly AbstractValidator<InteWorkCenterModifyDto> _validationModifyRules;
        private readonly ICurrentUser _currentUser;
        private readonly ICurrentSite _currentSite;

        /// <summary>
        /// 工作中心表服务
        /// </summary>
        /// <param name="inteWorkCenterRepository"></param>
        /// <param name="validationCreateRules"></param>
        /// <param name="validationModifyRules"></param>
        /// <param name="currentUser"></param>
        /// <param name="currentSite"></param>
        public InteWorkCenterService(IInteWorkCenterRepository inteWorkCenterRepository, AbstractValidator<InteWorkCenterCreateDto> validationCreateRules, AbstractValidator<InteWorkCenterModifyDto> validationModifyRules, ICurrentUser currentUser, ICurrentSite currentSite)
        {
            _inteWorkCenterRepository = inteWorkCenterRepository;
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
        public async Task<PagedInfo<InteWorkCenterDto>> GetPageListAsync(InteWorkCenterPagedQueryDto pram)
        {
            var inteWorkCenterPagedQuery = pram.ToQuery<InteWorkCenterPagedQuery>();
            inteWorkCenterPagedQuery.SiteId = _currentSite.SiteId ?? 0;
            var pagedInfo = await _inteWorkCenterRepository.GetPagedInfoAsync(inteWorkCenterPagedQuery);

            //实体到DTO转换 装载数据
            List<InteWorkCenterDto> inteWorkCenterDtos = PrepareInteWorkCenterDtos(pagedInfo);
            return new PagedInfo<InteWorkCenterDto>(inteWorkCenterDtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<InteWorkCenterDto> QueryInteWorkCenterByIdAsync(long id)
        {
            InteWorkCenterDto inteWorkCenterDto = new InteWorkCenterDto();
            var inteWorkCenterEntity = await _inteWorkCenterRepository.GetByIdAsync(id);
            if (inteWorkCenterEntity != null)
            {
                inteWorkCenterDto = inteWorkCenterEntity.ToModel<InteWorkCenterDto>();
            }
            return inteWorkCenterDto;
        }

        /// <summary>
        /// 获取关联资源
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<List<InteWorkCenterResourceRelationDto>> GetInteWorkCenterResourceRelatioByIdAsync(long id)
        {
            var inteWorkCenterRelationList = await _inteWorkCenterRepository.GetInteWorkCenterResourceRelatioAsync(id);

            var workCenterResourceRelationList = new List<InteWorkCenterResourceRelationDto>();
            foreach (var inteWorkCenterResourceRelation in inteWorkCenterRelationList)
            {
                workCenterResourceRelationList.Add(new InteWorkCenterResourceRelationDto
                {
                    Id = inteWorkCenterResourceRelation.Id,
                    WorkCenterId = inteWorkCenterResourceRelation.WorkCenterId,
                    ResourceCode = inteWorkCenterResourceRelation.ResourceCode,
                    ResourceName = inteWorkCenterResourceRelation.ResourceName,
                    ResourceId = inteWorkCenterResourceRelation.ResourceId,
                    CreatedBy = inteWorkCenterResourceRelation.CreatedBy,
                    CreatedOn = inteWorkCenterResourceRelation.CreatedOn,
                    UpdatedBy = inteWorkCenterResourceRelation.UpdatedBy,
                    UpdatedOn = inteWorkCenterResourceRelation.UpdatedOn,
                });
            }
            return workCenterResourceRelationList;
        }


        /// <summary>
        /// 获取关联工作中心
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<List<InteWorkCenterRelationDto>> GetInteWorkCenterRelationByIdAsync(long id)
        {
            var inteWorkCenterRelationList = await _inteWorkCenterRepository.GetInteWorkCenterRelationAsync(id);

            var workCenterRelationList = new List<InteWorkCenterRelationDto>();
            foreach (var inteWorkCenterRelation in inteWorkCenterRelationList)
            {
                workCenterRelationList.Add(new InteWorkCenterRelationDto
                {
                    Id = inteWorkCenterRelation.Id,
                    WorkCenterId = inteWorkCenterRelation.WorkCenterId,
                    SubWorkCenterId = inteWorkCenterRelation.SubWorkCenterId,
                    WorkCenterCode = inteWorkCenterRelation.WorkCenterCode,
                    WorkCenterName = inteWorkCenterRelation.WorkCenterName,
                    CreatedBy = inteWorkCenterRelation.CreatedBy,
                    CreatedOn = inteWorkCenterRelation.CreatedOn,
                    UpdatedBy = inteWorkCenterRelation.UpdatedBy,
                    UpdatedOn = inteWorkCenterRelation.UpdatedOn,
                });
            }
            return workCenterRelationList;
        }



        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="param">新增参数</param>
        /// <returns></returns>
        /// <exception cref="ValidationException">参数为空</exception>
        /// <exception cref="BusinessException">编码复用</exception>
        public async Task CreateInteWorkCenterAsync(InteWorkCenterCreateDto param)
        {
            if (param == null)
            {
                throw new ValidationException(nameof(ErrorCode.MES10100));
            }
            //验证DTO
            await _validationCreateRules.ValidateAndThrowAsync(param);

            var inteWorkCenterEntity = await _inteWorkCenterRepository.GetByCodeAsync(new EntityByCodeQuery { Code = param.Code, Site = _currentSite.SiteId });
            if (inteWorkCenterEntity != null)
            {
                throw new BusinessException(nameof(ErrorCode.MES12101)).WithData("code", param.Code);
            }
            var userId = _currentUser.UserName;
            //DTO转换实体
            inteWorkCenterEntity = param.ToEntity<InteWorkCenterEntity>();
            inteWorkCenterEntity.Id = IdGenProvider.Instance.CreateId();
            inteWorkCenterEntity.CreatedBy = userId;
            inteWorkCenterEntity.UpdatedBy = userId;
            inteWorkCenterEntity.Status = SysDataStatusEnum.Build;
            inteWorkCenterEntity.SiteId = _currentSite.SiteId ?? 0;
            List<InteWorkCenterRelation> inteWorkCenterRelations = new List<InteWorkCenterRelation>();
            List<InteWorkCenterResourceRelation> inteWorkCenterResourceRelations = new List<InteWorkCenterResourceRelation>();
            if (param.Type == WorkCenterTypeEnum.Factory || param.Type == WorkCenterTypeEnum.Farm)
            {

                if (param.WorkCenterIds != null && param.WorkCenterIds.Any())
                {
                    foreach (var id in param.WorkCenterIds)
                    {
                        inteWorkCenterRelations.Add(new InteWorkCenterRelation
                        {
                            Id = IdGenProvider.Instance.CreateId(),
                            WorkCenterId = inteWorkCenterEntity.Id,
                            SubWorkCenterId = id,
                            CreatedBy = userId,
                            UpdatedBy = userId,
                        }); ;
                    }
                }
            }
            else
            {
                if (param.WorkCenterIds != null && param.WorkCenterIds.Any())
                {
                    foreach (var id in param.WorkCenterIds)
                    {
                        inteWorkCenterResourceRelations.Add(new InteWorkCenterResourceRelation
                        {
                            Id = IdGenProvider.Instance.CreateId(),
                            WorkCenterId = inteWorkCenterEntity.Id,
                            ResourceId = id,
                            CreatedBy = userId,
                            UpdatedBy = userId,
                        });
                    }
                }
            }
            using var ts = TransactionHelper.GetTransactionScope();
            await _inteWorkCenterRepository.InsertAsync(inteWorkCenterEntity);
            await _inteWorkCenterRepository.InsertInteWorkCenterRelationRangAsync(inteWorkCenterRelations);
            await _inteWorkCenterRepository.InsertInteWorkCenterResourceRelationRangAsync(inteWorkCenterResourceRelations);
            ts.Complete();
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeleteRangInteWorkCenterAsync(long[] ids)
        {
            var userId = _currentUser.UserName;

            // 启用状态或保留状态不可删除
            var workCenters = await _inteWorkCenterRepository.GetByIdsAsync(ids);
            if (workCenters.Any(a => a.Status == SysDataStatusEnum.Enable || a.Status == SysDataStatusEnum.Retain))
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES12113));
            }

            return await _inteWorkCenterRepository.DeleteRangAsync(new DeleteCommand { Ids = ids, DeleteOn = HymsonClock.Now(), UserId = userId });
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        /// <exception cref="ValidationException">参数为空</exception>
        public async Task ModifyInteWorkCenterAsync(InteWorkCenterModifyDto param)
        {
            if (param == null)
            {
                throw new ValidationException(nameof(ErrorCode.MES10100));
            }
            //验证DTO
            await _validationModifyRules.ValidateAndThrowAsync(param);
            var entity = await _inteWorkCenterRepository.GetByIdAsync(param.Id);
            if (entity == null)
            {
                throw new BusinessException(nameof(ErrorCode.MES12111));

            }
            else
            {
                if (entity.Type != param.Type)
                {
                    var getInteWorkCenterRelationTask = _inteWorkCenterRepository.GetInteWorkCenterRelationAsync(param.Id);
                    var getInteWorkCenterResourceRelationTask = _inteWorkCenterRepository.GetInteWorkCenterResourceRelatioAsync(param.Id);
                    var inteWorkCenterRelationList = await getInteWorkCenterRelationTask;
                    var inteWorkCenterResourceRelationList = await getInteWorkCenterResourceRelationTask;

                    if (inteWorkCenterRelationList != null && inteWorkCenterRelationList.Any() || inteWorkCenterResourceRelationList != null && inteWorkCenterResourceRelationList.Any())
                    {
                        throw new BusinessException(nameof(ErrorCode.MES12111));
                    }
                }
            }

            var userId = _currentUser.UserName;
            //DTO转换实体
            var inteWorkCenterEntity = param.ToEntity<InteWorkCenterEntity>();
            inteWorkCenterEntity.UpdatedBy = userId;
            inteWorkCenterEntity.UpdatedOn = HymsonClock.Now();

            List<InteWorkCenterRelation> inteWorkCenterRelations = new List<InteWorkCenterRelation>();
            List<InteWorkCenterResourceRelation> inteWorkCenterResourceRelations = new List<InteWorkCenterResourceRelation>();
            if (param.Type == WorkCenterTypeEnum.Factory || param.Type == WorkCenterTypeEnum.Farm)
            {

                if (param.WorkCenterIds != null && param.WorkCenterIds.Any())
                {
                    foreach (var id in param.WorkCenterIds)
                    {
                        inteWorkCenterRelations.Add(new InteWorkCenterRelation
                        {
                            Id = IdGenProvider.Instance.CreateId(),
                            WorkCenterId = inteWorkCenterEntity.Id,
                            SubWorkCenterId = id,
                            CreatedBy = userId,
                            UpdatedBy = userId,
                        }); ;
                    }
                }
            }
            else
            {
                if (param.ResourceIds != null && param.ResourceIds.Any())
                {
                    foreach (var id in param.ResourceIds)
                    {
                        inteWorkCenterResourceRelations.Add(new InteWorkCenterResourceRelation
                        {
                            Id = IdGenProvider.Instance.CreateId(),
                            WorkCenterId = inteWorkCenterEntity.Id,
                            ResourceId = id,
                            CreatedBy = userId,
                            UpdatedBy = userId,
                        });
                    }
                }
            }

            using var ts = TransactionHelper.GetTransactionScope();
            await _inteWorkCenterRepository.UpdateAsync(inteWorkCenterEntity);

            await _inteWorkCenterRepository.RealDelteInteWorkCenterRelationRangAsync(param.Id);
            await _inteWorkCenterRepository.InsertInteWorkCenterRelationRangAsync(inteWorkCenterRelations);

            await _inteWorkCenterRepository.RealDelteInteWorkCenterResourceRelationRangAsync(param.Id);
            await _inteWorkCenterRepository.InsertInteWorkCenterResourceRelationRangAsync(inteWorkCenterResourceRelations);
            ts.Complete();
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="pagedInfo"></param>
        /// <returns></returns>
        private static List<InteWorkCenterDto> PrepareInteWorkCenterDtos(PagedInfo<InteWorkCenterEntity> pagedInfo)
        {
            var inteWorkCenterDtos = new List<InteWorkCenterDto>();
            foreach (var inteWorkCenterEntity in pagedInfo.Data)
            {
                var inteWorkCenterDto = inteWorkCenterEntity.ToModel<InteWorkCenterDto>();
                inteWorkCenterDtos.Add(inteWorkCenterDto);
            }
            return inteWorkCenterDtos;
        }
    }
}