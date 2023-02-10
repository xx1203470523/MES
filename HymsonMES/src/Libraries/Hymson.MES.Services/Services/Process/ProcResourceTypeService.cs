using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.Process.Resource;
using Hymson.MES.Data.Repositories.Process.ResourceType;
using Hymson.MES.Services.Dtos.Process;
using Hymson.MES.Services.Services.Process.IProcessService;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Extensions;
using Hymson.Utils.Tools;
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
        private readonly IProcResourceTypeRepository _resourceTypeRepository;
        private readonly IProcResourceRepository _resourceRepository;
        //private readonly AbstractValidator<ProcResourceTypeDto> _validationRules;

        /// <summary>
        /// 构造函数
        /// </summary>
        public ProcResourceTypeService(IProcResourceTypeRepository resourceTypeRepository, IProcResourceRepository resourceRepository)
        //AbstractValidator<ProcResourceTypeDto> validationRules)
        {
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
            return entity.ToModel<ProcResourceTypeDto>();
        }

        /// <summary>
        /// 查询资源类型维护表列表(关联资源：一个类型被多个资源关联就展示多条)
        /// </summary>
        /// <param name="procResourceTypePagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ProcResourceTypeViewDto>> GetPageListAsync(ProcResourceTypePagedQueryDto procResourceTypePagedQueryDto)
        {
            var procResourceTypePagedQuery = procResourceTypePagedQueryDto.ToQuery<ProcResourceTypePagedQuery>();
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
        public async Task AddProcResourceTypeAsync(ProcResourceTypeAddCommandDto param)
        {
            //验证DTO
            //var dto = new ProcResourceTypeDto();
            //await _validationRules.ValidateAndThrowAsync(dto);
            if (param == null)
            {
                throw new NotFoundException(ErrorCode.MES10100, "请求实体不能为空!");
            }

            var userId = "TODO";
            var siteCode = "TODO";
            //DTO转换实体
            var id = IdGenProvider.Instance.CreateId();
            var entity = new ProcResourceTypeAddCommand
            {
                Id = id,
                SiteCode = siteCode,
                CreatedBy = userId,
                UpdatedBy = userId,
                UpdatedOn = HymsonClock.Now(),
                CreatedOn = HymsonClock.Now(),
                Remark = param.Remark ?? "",
                ResType = param.ResType.ToUpperInvariant(),
                ResTypeName = param.ResTypeName ?? ""
            };

            //判断资源类型在系统中是否已经存在
            var resEntity = new ProcResourceTypeEntity { SiteCode = siteCode, ResType = entity.ResType };
            var resourceType = await _resourceTypeRepository.GetByCodeAsync(resEntity);
            if (resourceType != null)
            {
                throw new CustomerValidationException(ErrorCode.MES10100, $"此资源类型{param.ResType}在系统已经存在!");
            }

            var resourceIds = param.ResourceIds;
            var updateCommand = new ProcResourceUpdateCommand();
            if (resourceIds != null && resourceIds.Any() == true)
            {
                updateCommand.UpdatedOn = HymsonClock.Now();
                updateCommand.UpdatedBy = userId;
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
        public async Task UpdateProcResrouceTypeAsync(ProcResourceTypeUpdateCommandDto param)
        {
            //验证DTO
            //var dto = new ProcResourceTypeDto();
            //await _validationRules.ValidateAndThrowAsync(dto);
            //TODO
            if (param == null)
            {
                throw new NotFoundException(ErrorCode.MES10100, "请求实体不能为空!");
            }
            var entity = await _resourceTypeRepository.GetByIdAsync(param?.Id ?? 0);
            if (entity == null)
            {
                throw new NotFoundException(ErrorCode.MES10100, "此资源类型不存在!");
            }

            var userId = "TODO";
            //DTO转换实体
            var updateEntity = new ProcResourceTypeUpdateCommand
            {
                Id = param.Id,
                Remark = param.Remark ?? "",
                ResTypeName = param.ResTypeName ?? "",
                UpdatedOn = HymsonClock.Now(),
                UpdatedBy = userId
            };

            var resourceIds = param.ResourceIds;
            var updateCommand = new ProcResourceUpdateCommand();
            if (resourceIds != null && resourceIds.Any() == true)
            {
                updateCommand.UpdatedOn = HymsonClock.Now();
                updateCommand.UpdatedBy = userId;
                updateCommand.ResTypeId = param.Id;
                updateCommand.IdsArr = param.ResourceIds.ToArray();
            }

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
                await _resourceRepository.UpdateResTypeAsync(param.Id);

                //更新资源的资源类型（重新绑定）
                await _resourceRepository.UpdateResTypeAsync(updateCommand);

                ts.Complete();
            }
        }

        /// <summary>
        /// 批量删除资源类型数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeleteProcResourceTypeAsync(string ids)
        {
            long[] idsArr = StringExtension.SpitLongArrary(ids);
            if (idsArr.Length < 1)
            {
                throw new NotFoundException(ErrorCode.MES10100, "删除失败Id 不能为空!");
            }

            //查询资源类型是否关联资源
            var siteCode = "TODO";
            var query = new ProcResourceQuery
            {
                SiteCode = siteCode,
                IdsArr = idsArr
            };
            var resourceList = await _resourceRepository.GetByResTypeIdsAsync(query);
            if(resourceList!=null&& resourceList.Any())
            {
                throw new CustomerValidationException(ErrorCode.MES10100, "资源类型有被分配的资源，不允许删除!");
            }

            return await _resourceTypeRepository.DeleteAsync(idsArr);
        }
    }
}
