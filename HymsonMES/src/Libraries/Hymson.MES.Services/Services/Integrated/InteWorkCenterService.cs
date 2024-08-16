using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.Localization.Services;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Integrated;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Integrated;
using Hymson.MES.CoreServices.Dtos.Common;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.MES.Data.Repositories.Integrated.IIntegratedRepository;
using Hymson.MES.Data.Repositories.Integrated.InteWorkCenter.Query;
using Hymson.MES.Data.Repositories.Plan;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Services.Dtos.Common;
using Hymson.MES.Services.Dtos.Integrated;
using Hymson.MES.Services.Services.Integrated.IIntegratedService;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Services.Services.Integrated
{
    /// <summary>
    /// 工作中心表服务
    /// @author admin
    /// @date 2023-02-22
    /// </summary>
    public class InteWorkCenterService : IInteWorkCenterService
    {
        private readonly ICurrentUser _currentUser;
        private readonly ICurrentSite _currentSite;
        private readonly AbstractValidator<InteWorkCenterCreateDto> _validationCreateRules;
        private readonly AbstractValidator<InteWorkCenterModifyDto> _validationModifyRules;
        private readonly IInteWorkCenterRepository _inteWorkCenterRepository;
        private readonly IProcResourceRepository _procResourceRepository;
        private readonly IPlanWorkOrderActivationRepository _planWorkOrderActivationRepository;

        private readonly ILocalizationService _localizationService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="currentSite"></param>
        /// <param name="validationCreateRules"></param>
        /// <param name="validationModifyRules"></param>
        /// <param name="inteWorkCenterRepository"></param>
        /// <param name="procResourceRepository"></param>
        /// <param name="planWorkOrderActivationRepository"></param>
        /// <param name="localizationService"></param>
        public InteWorkCenterService(ICurrentUser currentUser, ICurrentSite currentSite,
            AbstractValidator<InteWorkCenterCreateDto> validationCreateRules,
            AbstractValidator<InteWorkCenterModifyDto> validationModifyRules,
            IInteWorkCenterRepository inteWorkCenterRepository,
            IProcResourceRepository procResourceRepository,
            IPlanWorkOrderActivationRepository planWorkOrderActivationRepository, ILocalizationService localizationService)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _validationCreateRules = validationCreateRules;
            _validationModifyRules = validationModifyRules;
            _inteWorkCenterRepository = inteWorkCenterRepository;
            _procResourceRepository = procResourceRepository;
            _planWorkOrderActivationRepository = planWorkOrderActivationRepository;
            _localizationService = localizationService;
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

            // 实体到DTO转换 装载数据
            var dtos = pagedInfo.Data.Select(s => s.ToModel<InteWorkCenterDto>());
            return new PagedInfo<InteWorkCenterDto>(dtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<InteWorkCenterDto> QueryInteWorkCenterByIdAsync(long id)
        {
            InteWorkCenterDto inteWorkCenterDto = new();
            var inteWorkCenterEntity = await _inteWorkCenterRepository.GetByIdAsync(id);
            if (inteWorkCenterEntity == null) return inteWorkCenterDto;

            return inteWorkCenterEntity.ToModel<InteWorkCenterDto>();
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
        /// 根据类型查询列表（工作中心）
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        public async Task<IEnumerable<SelectOptionDto>> QueryListByTypeAndParentIdAsync(QueryInteWorkCenterByTypeAndParentIdDto queryDto)
        {
            var workCenters = await _inteWorkCenterRepository.GetByTypeAndParentIdAsync(new InteWorkCenterByTypeQuery
            {
                SiteId = _currentSite.SiteId,
                Type = queryDto.Type,
                ParentId = queryDto.ParentId
            });

            return workCenters.Select(s => new SelectOptionDto
            {
                Key = $"{s.Id}",
                Label = $"【{s.Code}】 {s.Name}",
                Value = $"{s.Id}"
            });
        }

        /// <summary>
        /// 获取当前站点下面的所有车间
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<SelectOptionDto>> GetWorkShopListAsync()
        {
            var workShops = await _inteWorkCenterRepository.GetWorkCenterListByTypeAsync(new EntityByTypeQuery
            {
                SiteId = _currentSite.SiteId ?? 0,
                Type = WorkCenterTypeEnum.Farm,
                Status = SysDataStatusEnum.Enable
            });

            return workShops.Select(s => new SelectOptionDto
            {
                Key = $"{s.Id}",
                Label = $"【{s.Code}】 {s.Name}",
                Value = $"{s.Id}"
            });
        }

        /// <summary>
        /// 获取当前站点下面的所有产线
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<SelectOptionDto>> GetProductionLineListAsync()
        {
            var workShops = await _inteWorkCenterRepository.GetWorkCenterListByTypeAsync(new EntityByTypeQuery
            {
                SiteId = _currentSite.SiteId ?? 0,
                Type = WorkCenterTypeEnum.Line,
                Status = SysDataStatusEnum.Enable
            });

            return workShops.Select(s => new SelectOptionDto
            {
                Key = $"{s.Id}",
                Label = $"【{s.Code}】 {s.Name}",
                Value = $"{s.Id}"
            });
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="param">新增参数</param>
        /// <returns></returns>
        public async Task<long> CreateInteWorkCenterAsync(InteWorkCenterCreateDto param)
        {
            // 验证DTO
            await _validationCreateRules.ValidateAndThrowAsync(param);

            var entity = await _inteWorkCenterRepository.GetByCodeAsync(new EntityByCodeQuery { Code = param.Code, Site = _currentSite.SiteId });
            if (entity != null) throw new CustomerValidationException(nameof(ErrorCode.MES12101)).WithData("code", param.Code);

            // DTO转换实体
            entity = param.ToEntity<InteWorkCenterEntity>();
            entity.Id = IdGenProvider.Instance.CreateId();
            entity.CreatedBy = _currentUser.UserName;
            entity.UpdatedBy = entity.CreatedBy;
            entity.Source = WorkCenterSourceEnum.MES;
            entity.SiteId = _currentSite.SiteId ?? 0;

            entity.Status = SysDataStatusEnum.Build;

            List<InteWorkCenterRelation> inteWorkCenterRelations = new();
            List<InteWorkCenterResourceRelation> inteWorkCenterResourceRelations = new();

            switch (param.Type)
            {
                //case WorkCenterTypeEnum.Factory:
                case WorkCenterTypeEnum.Farm:
                    param.WorkCenterIds ??= new List<long>();
                    inteWorkCenterRelations.AddRange(param.WorkCenterIds.Select(s => new InteWorkCenterRelation
                    {
                        Id = IdGenProvider.Instance.CreateId(),
                        WorkCenterId = entity.Id,
                        SubWorkCenterId = s,
                        CreatedBy = _currentUser.UserName,
                        UpdatedBy = _currentUser.UserName
                    }));

                    // 是否存在相同车间/产线
                    if (inteWorkCenterRelations.GroupBy(g => g.SubWorkCenterId).Count()
                        < inteWorkCenterRelations.Count)
                    {
                        throw new CustomerValidationException(nameof(ErrorCode.MES12119));
                    }

                    //验证产线是否已经绑定过车间，一个产线只能绑定一个车间
                    //根据产线获取信息
                    var inteWorkCenterByLineEntities = await _inteWorkCenterRepository.GetInteWorkCenterRelationEntityAsync(new InteWorkCenterRelationQuery { SubWorkCenterIds = param.WorkCenterIds });
                    if (inteWorkCenterByLineEntities != null && inteWorkCenterByLineEntities.Any())
                    {
                        throw new CustomerValidationException(nameof(ErrorCode.MES12126));
                    }
                    // 判断资源的状态是否存在新建和废除状态
                    var resourcesFarm = await _inteWorkCenterRepository.GetByIdsAsync(param.WorkCenterIds.ToArray());
                    if (resourcesFarm != null && resourcesFarm.Any(a => a.Status == SysDataStatusEnum.Build))
                    {
                        throw new CustomerValidationException(nameof(ErrorCode.MES12127)).WithData("code", param.Code);
                    }

                    break;
                case WorkCenterTypeEnum.Line:
                    param.ResourceIds ??= new List<long>();
                    if (!param.ResourceIds.Any()) throw new CustomerValidationException(nameof(ErrorCode.MES12116));

                    inteWorkCenterResourceRelations.AddRange(param.ResourceIds.Select(s => new InteWorkCenterResourceRelation
                    {
                        Id = IdGenProvider.Instance.CreateId(),
                        WorkCenterId = entity.Id,
                        SiteId = _currentSite.SiteId ?? 0,
                        ResourceId = s,
                        CreatedBy = _currentUser.UserName,
                        UpdatedBy = _currentUser.UserName
                    }));

                    // 是否存在相同资源
                    if (inteWorkCenterResourceRelations.GroupBy(g => g.ResourceId).Count()
                        < inteWorkCenterResourceRelations.Count)
                    {
                        throw new CustomerValidationException(nameof(ErrorCode.MES12120));
                    }

                    // 判断资源的状态是否存在新建和废除状态
                    var resources = await _procResourceRepository.GetListByIdsAsync(param.ResourceIds.ToArray());
                    if (resources != null && resources.Any(a => a.Status == (int)SysDataStatusEnum.Build || a.Status == (int)SysDataStatusEnum.Abolish))
                    {
                        // TODO 上面的资源类型要改为枚举
                        throw new CustomerValidationException(nameof(ErrorCode.MES12121));
                    }

                    // 判断资源是否被重复绑定
                    var workCenterIds = await _inteWorkCenterRepository.GetWorkCenterIdByResourceIdAsync(param.ResourceIds);
                    if (workCenterIds != null && workCenterIds.Any()) throw new CustomerValidationException(nameof(ErrorCode.MES12117));
                    break;
                default:
                    break;
            }

            // 保存
            using var ts = TransactionHelper.GetTransactionScope();
            await _inteWorkCenterRepository.InsertAsync(entity);
            await _inteWorkCenterRepository.InsertInteWorkCenterRelationRangAsync(inteWorkCenterRelations);
            await _inteWorkCenterRepository.InsertInteWorkCenterResourceRelationRangAsync(inteWorkCenterResourceRelations);
            ts.Complete();
            return entity.Id;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task ModifyInteWorkCenterAsync(InteWorkCenterModifyDto param)
        {
            // 验证DTO
            await _validationModifyRules.ValidateAndThrowAsync(param);

            var entity = await _inteWorkCenterRepository.GetByIdAsync(param.Id)
               ?? throw new CustomerValidationException(nameof(ErrorCode.MES12111));

            //验证某些状态是不能编辑的
            var canEditStatusEnum = new SysDataStatusEnum[] { SysDataStatusEnum.Build, SysDataStatusEnum.Retain };
            if (!canEditStatusEnum.Any(x => x == entity.Status))
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10129));
            }

            // 如果有修改混线类型（从允许混线修改为不允许混线）
            if (entity.Type == WorkCenterTypeEnum.Line && entity.IsMixLine != param.IsMixLine && param.IsMixLine == false)
            {
                // 判断激活的工单数量
                var maxCount = 1;
                var planWorkOrderActivationEntities = await _planWorkOrderActivationRepository.GetByWorkCenterIdAsync(entity.Id);
                if (planWorkOrderActivationEntities != null && planWorkOrderActivationEntities.Count() > maxCount)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES12118)).WithData("maxCount", maxCount);
                }
            }

            // DTO转换实体
            var inteWorkCenterEntity = param.ToEntity<InteWorkCenterEntity>();
            inteWorkCenterEntity.UpdatedBy = _currentUser.UserName;
            inteWorkCenterEntity.UpdatedOn = HymsonClock.Now();

            List<InteWorkCenterRelation> inteWorkCenterRelations = new();
            List<InteWorkCenterResourceRelation> inteWorkCenterResourceRelations = new();

            switch (param.Type)
            {
                //case WorkCenterTypeEnum.Factory:
                case WorkCenterTypeEnum.Farm:
                    param.WorkCenterIds ??= new List<long>();
                    inteWorkCenterRelations.AddRange(param.WorkCenterIds.Select(s => new InteWorkCenterRelation
                    {
                        Id = IdGenProvider.Instance.CreateId(),
                        WorkCenterId = entity.Id,
                        SubWorkCenterId = s,
                        CreatedBy = _currentUser.UserName,
                        UpdatedBy = _currentUser.UserName
                    }));

                    // 是否存在相同车间/产线
                    if (inteWorkCenterRelations.GroupBy(g => g.SubWorkCenterId).Count()
                        < inteWorkCenterRelations.Count)
                    {
                        throw new CustomerValidationException(nameof(ErrorCode.MES12119));
                    }

                    //验证产线是否已经绑定过车间，一个产线只能绑定一个车间
                    //根据产线获取信息
                    var inteWorkCenterByLineEntities = await _inteWorkCenterRepository.GetInteWorkCenterRelationEntityAsync(new InteWorkCenterRelationQuery { SubWorkCenterIds = param.WorkCenterIds });
                    if (inteWorkCenterByLineEntities != null && inteWorkCenterByLineEntities.Any())
                    {
                        if (inteWorkCenterByLineEntities.First().WorkCenterId != param.Id)
                        {
                            throw new CustomerValidationException(nameof(ErrorCode.MES12126));
                        }
                    }

                    break;
                case WorkCenterTypeEnum.Line:
                    param.ResourceIds ??= new List<long>();

                    inteWorkCenterResourceRelations.AddRange(param.ResourceIds.Select(s => new InteWorkCenterResourceRelation
                    {
                        Id = IdGenProvider.Instance.CreateId(),
                        WorkCenterId = entity.Id,
                        ResourceId = s,
                        SiteId = _currentSite.SiteId ?? 0,
                        CreatedBy = _currentUser.UserName,
                        UpdatedBy = _currentUser.UserName
                    }));

                    // 是否存在相同资源
                    if (inteWorkCenterResourceRelations.GroupBy(g => g.ResourceId).Count()
                        < inteWorkCenterResourceRelations.Count)
                    {
                        throw new CustomerValidationException(nameof(ErrorCode.MES12120));
                    }

                    // 判断资源的状态是否存在新建和废除状态
                    var resources = await _procResourceRepository.GetListByIdsAsync(param.ResourceIds.ToArray());
                    if (resources != null && resources.Any(a => a.Status == (int)SysDataStatusEnum.Build || a.Status == (int)SysDataStatusEnum.Abolish))
                    {
                        // TODO 上面的资源类型要改为枚举
                        throw new CustomerValidationException(nameof(ErrorCode.MES12121));
                    }

                    // 判断资源是否被重复绑定
                    var workCenters = await _inteWorkCenterRepository.GetWorkCenterResourceRelationAsync(param.ResourceIds, param.Id);
                    if (workCenters.Any(x => x.Id != entity.Id))
                    {
                        var procResourceEnties = await _procResourceRepository.GetResByIdsAsync(workCenters.Where(x => x.Id != entity.Id).Select(x => x.ResourceId).Distinct());
                        throw new CustomerValidationException(nameof(ErrorCode.MES12128)).WithData("ResourceCodes",string.Join( ",", procResourceEnties.Select(x => x.ResCode)));
                    }
                    break;
                default:
                    break;
            }

            // 如果有切换工作中心类型
            if (entity.Type != param.Type)
            {
                var getInteWorkCenterRelationTask = _inteWorkCenterRepository.GetInteWorkCenterRelationAsync(param.Id);
                var getInteWorkCenterResourceRelationTask = _inteWorkCenterRepository.GetInteWorkCenterResourceRelatioAsync(param.Id);
                var inteWorkCenterRelationList = await getInteWorkCenterRelationTask;
                var inteWorkCenterResourceRelationList = await getInteWorkCenterResourceRelationTask;

                if ((inteWorkCenterRelationList != null && inteWorkCenterRelationList.Any())
                    || (inteWorkCenterResourceRelationList != null && inteWorkCenterResourceRelationList.Any()))
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES12112));
                }
            }

            // 保存
            using var ts = TransactionHelper.GetTransactionScope();
            await _inteWorkCenterRepository.UpdateAsync(inteWorkCenterEntity);

            await _inteWorkCenterRepository.RealDelteInteWorkCenterRelationRangAsync(param.Id);
            await _inteWorkCenterRepository.InsertInteWorkCenterRelationRangAsync(inteWorkCenterRelations);

            await _inteWorkCenterRepository.RealDelteInteWorkCenterResourceRelationRangAsync(param.Id);
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

            // 只可删除新建状态
            var workCenters = await _inteWorkCenterRepository.GetByIdsAsync(ids);

            if (workCenters != null && workCenters.Any(a => a.Status != SysDataStatusEnum.Build))
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10106));
            }

            // 禅道有坑货提出可以删除 4146
            /*
            // 检查产线是否有下级资源
            var inteWorkCenterRelations = await _inteWorkCenterRepository.GetResourceIdsByWorkCenterIdAsync(ids);
            if (inteWorkCenterRelations != null && inteWorkCenterRelations.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES12114));
            }
            */

            var command = new DeleteCommand { Ids = ids, DeleteOn = HymsonClock.Now(), UserId = userId };

            // 删除关联关系
            var rows = 0;
            using var ts = TransactionHelper.GetTransactionScope();
            rows += await _inteWorkCenterRepository.DeleteWorkCenterRelationByParentIdsAsync(command);
            rows += await _inteWorkCenterRepository.DeleteResourceRelationByParentIdsAsync(command);
            rows += await _inteWorkCenterRepository.DeleteRangAsync(command);
            ts.Complete();
            return rows;
        }

        #region 状态变更
        /// <summary>
        /// 状态变更
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task UpdateStatusAsync(ChangeStatusDto param)
        {
            #region 参数校验
            if (param.Id == 0)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10125));
            }
            if (!Enum.IsDefined(typeof(SysDataStatusEnum), param.Status))
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10126));
            }
            if (param.Status == SysDataStatusEnum.Build)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10128));
            }

            #endregion

            var changeStatusCommand = new ChangeStatusCommand()
            {
                Id = param.Id,
                Status = param.Status,

                UpdatedBy = _currentUser.UserName,
                UpdatedOn = HymsonClock.Now()
            };

            #region 校验数据
            var entity = await _inteWorkCenterRepository.GetByIdAsync(changeStatusCommand.Id);
            if (entity == null || entity.IsDeleted != 0)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES12125));
            }
            if (entity.Status == changeStatusCommand.Status)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10127)).WithData("status", _localizationService.GetResource($"{typeof(SysDataStatusEnum).FullName}.{Enum.GetName(typeof(SysDataStatusEnum), entity.Status)}"));
            }
            #endregion

            #region 操作数据库
            await _inteWorkCenterRepository.UpdateStatusAsync(changeStatusCommand);
            #endregion
        }

        #endregion

    }
}