using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.Process.Resource;
using Hymson.MES.Data.Repositories.Process.ResourceType;
using Hymson.MES.Services.Dtos.Process;
using Hymson.MES.Services.Services.Process.IProcessService;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;
using System.ComponentModel.DataAnnotations;
using System.Transactions;

namespace Hymson.MES.Services.Services.Process
{
    /// <summary>
    /// 资源类型维护表Service业务层处理
    /// @tableName proc_resource_type
    /// @author zhaoqing
    /// @date 2023-02-06
    /// </summary>
    public class ProcResourceTypeService : IProcResourceTypeService
    {
        /// <summary>
        /// 当前登录用户对象
        /// </summary>
        private readonly ICurrentUser _currentUser;
        /// <summary>
        /// 当前站点
        /// </summary>
        private readonly ICurrentSite _currentSite;
        /// <summary>
        /// 资源类型仓储对象
        /// </summary>
        private readonly IProcResourceTypeRepository _resourceTypeRepository;
        /// <summary>
        /// 资源仓储对象
        /// </summary>
        private readonly IProcResourceRepository _resourceRepository;
        //private readonly AbstractValidator<ProcResourceTypeDto> _validationRules;

        /// <summary>
        /// 构造函数
        /// </summary>
        public ProcResourceTypeService(ICurrentUser currentUser, ICurrentSite currentSite,
            IProcResourceTypeRepository resourceTypeRepository,
            IProcResourceRepository resourceRepository)
        //AbstractValidator<ProcResourceTypeDto> validationRules)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _resourceTypeRepository = resourceTypeRepository;
            _resourceRepository = resourceRepository;
            // _validationRules = validationRules;
        }

        /// <summary>
        /// 查询资源类型维护表详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ProcResourceTypeDto> GetListAsync(long id)
        {
            var entity = await _resourceTypeRepository.GetByIdAsync(id);
            var model = entity?.ToModel<ProcResourceTypeDto>() ?? new ProcResourceTypeDto();

            var ids = new List<long> { id };
            var query = new ProcResourceQuery
            {
                SiteId = _currentSite.SiteId ?? 0,
                IdsArr = ids.ToArray()
            };
            var resources = await _resourceRepository.GetByResTypeIdsAsync(query);
            model.ResourceIds = resources.Select(a => a.Id).ToArray();
            return model;
        }

        /// <summary>
        /// 查询资源类型维护表列表(关联资源：一个类型被多个资源关联就展示多条)
        /// </summary>
        /// <param name="procResourceTypePagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ProcResourceTypeViewDto>> GetPageListAsync(ProcResourceTypePagedQueryDto procResourceTypePagedQueryDto)
        {
            var procResourceTypePagedQuery = procResourceTypePagedQueryDto.ToQuery<ProcResourceTypePagedQuery>();
            procResourceTypePagedQuery.SiteId = _currentSite.SiteId ?? 0;
            var pagedInfo = await _resourceTypeRepository.GetPageListAsync(procResourceTypePagedQuery);

            //实体到DTO转换 装载数据
            var procResourceTypeDtos = new List<ProcResourceTypeViewDto>();
            foreach (var entity in pagedInfo.Data)
            {
                var resourceTypeViewDto = entity.ToModel<ProcResourceTypeViewDto>();
                procResourceTypeDtos.Add(resourceTypeViewDto);
            }
            return new PagedInfo<ProcResourceTypeViewDto>(procResourceTypeDtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 获取资源类型分页列表(不关联资源、只展示资源类型信息)
        /// </summary>
        /// <param name="procResourceTypePagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ProcResourceTypeDto>> GetListAsync(ProcResourceTypePagedQueryDto procResourceTypePagedQueryDto)
        {
            var procResourceTypePagedQuery = procResourceTypePagedQueryDto.ToQuery<ProcResourceTypePagedQuery>();
            procResourceTypePagedQuery.SiteId = _currentSite.SiteId ?? 0;
            var pagedInfo = await _resourceTypeRepository.GetListAsync(procResourceTypePagedQuery);

            //实体到DTO转换 装载数据
            var procResourceTypeDtos = new List<ProcResourceTypeDto>();
            foreach (var entity in pagedInfo.Data)
            {
                var resourceTypeDto = entity.ToModel<ProcResourceTypeDto>();
                procResourceTypeDtos.Add(resourceTypeDto);
            }
            return new PagedInfo<ProcResourceTypeDto>(procResourceTypeDtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 添加资源类型数据
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task AddProcResourceTypeAsync(ProcResourceTypeAddDto param)
        {
            //验证DTO
            //var dto = new ProcResourceTypeDto();
            //await _validationRules.ValidateAndThrowAsync(dto);
            if (param == null)
            {
                throw new ValidationException(nameof(ErrorCode.MES10100));
            }

            var userName = _currentUser.UserName;
            var siteId = _currentSite.SiteId ?? 0;
            //DTO转换实体
            var id = IdGenProvider.Instance.CreateId();
            var resType = param.ResType.ToUpperInvariant();
            var entity = new ProcResourceTypeAddCommand
            {
                Id = id,
                SiteId = siteId,
                CreatedBy = userName,
                UpdatedBy = userName,
                Remark = param.Remark ?? "",
                ResType = resType,
                ResTypeName = param.ResTypeName ?? ""
            };

            //判断资源类型在系统中是否已经存在
            var resEntity = new ProcResourceTypeEntity { SiteId = siteId, ResType = resType };
            var resourceType = await _resourceTypeRepository.GetByCodeAsync(resEntity);
            if (resourceType != null)
            {
                throw new BusinessException(nameof(ErrorCode.MES10311)).WithData("ResType", param.ResType);
            }

            var resourceIds = param.ResourceIds;
            var updateCommand = new ProcResourceUpdateCommand();
            if (resourceIds != null && resourceIds.Any() == true)
            {
                updateCommand.UpdatedBy = userName;
                updateCommand.ResTypeId = id;
                updateCommand.IdsArr = param.ResourceIds.ToArray();
            }

            using (TransactionScope ts = TransactionHelper.GetTransactionScope())
            {
                //入库
                await _resourceTypeRepository.InsertAsync(entity);

                //更新资源的资源类型
                await _resourceRepository.UpdateResTypeAsync(updateCommand);

                ts.Complete();
            }
        }

        /// <summary>
        /// 修改资源类型数据
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task UpdateProcResrouceTypeAsync(ProcResourceTypeUpdateDto param)
        {
            //验证DTO
            //var dto = new ProcResourceTypeDto();
            //await _validationRules.ValidateAndThrowAsync(dto);
            if (param == null)
            {
                throw new ValidationException(nameof(ErrorCode.MES10100));
            }
            var entity = await _resourceTypeRepository.GetByIdAsync(param?.Id ?? 0);
            if (entity == null)
            {
                throw new NotFoundException(nameof(ErrorCode.MES10309));
            }

            var userName = _currentUser.UserName;
            //DTO转换实体
            var updateEntity = new ProcResourceTypeUpdateCommand
            {
                Id = param.Id,
                Remark = param.Remark ?? "",
                ResTypeName = param.ResTypeName ?? "",
                UpdatedBy = userName
            };

            var resourceIds = param.ResourceIds;
            var updateCommand = new ProcResourceUpdateCommand();
            if (resourceIds == null)
            {
                resourceIds = new List<long>();
            }
            updateCommand.UpdatedBy = userName;
            updateCommand.ResTypeId = param.Id;
            updateCommand.IdsArr = resourceIds.ToArray();

            //var resources = _procResourceRepository.GetProcResrouces(ids, parm.Id);
            //if (resources.Count > 0)
            //{
            //    apiResult.Code = (int)ResultCode.PARAM_ERROR;
            //    apiResult.Msg = "一个资源只能关联一个资源类型！";
            //    return apiResult;
            //}

            using (TransactionScope ts = TransactionHelper.GetTransactionScope())
            {
                //更新
                await _resourceTypeRepository.UpdateAsync(updateEntity);

                //清除之前的绑定关系
                await _resourceRepository.ResetResTypeAsync(updateCommand);

                if (resourceIds != null && resourceIds.Any() == true)
                {
                    //更新资源的资源类型（重新绑定）
                    await _resourceRepository.UpdateResTypeAsync(updateCommand);
                }
                ts.Complete();
            }
        }

        /// <summary>
        /// 批量删除资源类型数据
        /// </summary>
        /// <param name="idsArr"></param>
        /// <returns></returns>
        public async Task<int> DeleteProcResourceTypeAsync(long[] idsArr)
        {
            if (idsArr.Length < 1)
            {
                throw new ValidationException(nameof(ErrorCode.MES10102));
            }

            //查询资源类型是否关联资源
            var siteId = _currentSite.SiteId ?? 0;
            var query = new ProcResourceQuery
            {
                SiteId = siteId,
                IdsArr = idsArr
            };
            var resourceList = await _resourceRepository.GetByResTypeIdsAsync(query);
            if (resourceList != null && resourceList.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10312));
            }

            var command = new DeleteCommand
            {
                UserId = _currentUser.UserName,
                DeleteOn = HymsonClock.Now(),
                Ids = idsArr
            };
            return await _resourceTypeRepository.DeleteRangeAsync(command);
        }
    }
}
