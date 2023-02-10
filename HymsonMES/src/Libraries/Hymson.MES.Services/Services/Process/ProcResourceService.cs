using FluentValidation;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.Process.Resource;
using Hymson.MES.Services.Dtos.Process;
using Hymson.MES.Services.Services.Process.IProcessService;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Extensions;

namespace Hymson.MES.Services.Services.Process
{
    /// <summary>
    /// 资源维护表Service业务层处理
    /// @tableName proc_resource
    /// @author zhaoqing
    /// @date 2023-02-08
    /// </summary>
    public class ProcResourceService : IProcResourceService
    {
        /// <summary>
        /// 资源仓储
        /// </summary>
        private readonly IProcResourceRepository _resourceRepository;

        /// <summary>
        /// 资源关联打印机仓储
        /// </summary>
        private readonly IProcResourceConfigPrintRepository _resourceConfigPrintRepository;

        /// <summary>
        /// 资源设置仓储
        /// </summary>
        private readonly IProcResourceConfigResRepository _procResourceConfigResRepository;

        /// <summary>
        /// 资源关联设备仓储
        /// </summary>
        private readonly IProcResourceEquipmentBindRepository _resourceEquipmentBindRepository;

        /// <summary>
        /// 资源作业配置表仓储
        /// </summary>
        private readonly IProcResourceConfigJobRepository _resourceConfigJobRepository;

        private readonly AbstractValidator<ProcResourceDto> _validationRules;

        /// <summary>
        /// 构造函数
        /// </summary>
        public ProcResourceService(IProcResourceRepository resourceRepository,
                  IProcResourceConfigPrintRepository resourceConfigPrintRepository,
                  IProcResourceConfigResRepository procResourceConfigResRepository,
                  IProcResourceEquipmentBindRepository resourceEquipmentBindRepository,
                  IProcResourceConfigJobRepository resourceConfigJobRepository,
                  AbstractValidator<ProcResourceDto> validationRules)
        {
            _resourceRepository = resourceRepository;
            _resourceConfigPrintRepository = resourceConfigPrintRepository;
            _procResourceConfigResRepository = procResourceConfigResRepository;
            _resourceEquipmentBindRepository = resourceEquipmentBindRepository;
            _resourceConfigJobRepository= resourceConfigJobRepository;
            _validationRules = validationRules;
        }

        /// <summary>
        /// 查询资源类型维护表详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ProcResourceViewDto> GetByIdAsync(long id)
        {
            var entity = await _resourceRepository.GetByIdAsync(id);
            return entity.ToModel<ProcResourceViewDto>();
        }

        /// <summary>
        /// 查询资源维护表列表(关联资源类型，展示资源类型名称)
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ProcResourceViewDto>> GetPageListAsync(ProcResourcePagedQueryDto query)
        {
            var resourcePagedQuery = query.ToQuery<ProcResourcePagedQuery>();
            var pagedInfo = await _resourceRepository.GetPageListAsync(resourcePagedQuery);

            //实体到DTO转换 装载数据
            var procResourceDtos = new List<ProcResourceViewDto>();
            foreach (var entity in pagedInfo.Data)
            {
                var resourceViewDto = entity.ToModel<ProcResourceViewDto>();
                procResourceDtos.Add(resourceViewDto);
            }
            return new PagedInfo<ProcResourceViewDto>(procResourceDtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 获取资源分页列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ProcResourceDto>> GetListAsync(ProcResourcePagedQueryDto query)
        {
            var resourcePagedQuery = query.ToQuery<ProcResourcePagedQuery>();
            var pagedInfo = await _resourceRepository.GetListAsync(resourcePagedQuery);

            //实体到DTO转换 装载数据
            var procResourceDtos = new List<ProcResourceDto>();
            foreach (var entity in pagedInfo.Data)
            {
                var resourceTypeDto = entity.ToModel<ProcResourceDto>();
                procResourceDtos.Add(resourceTypeDto);
            }
            return new PagedInfo<ProcResourceDto>(procResourceDtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 查询资源类型下关联的资源(资源类型详情：可用资源，已分配资源)
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ProcResourceDto>> GetListForGroupAsync(ProcResourcePagedQueryDto query)
        {
            var resourcePagedQuery = query.ToQuery<ProcResourcePagedQuery>();
            var pagedInfo = await _resourceRepository.GetListForGroupAsync(resourcePagedQuery);

            //实体到DTO转换 装载数据
            var procResourceDtos = new List<ProcResourceDto>();
            foreach (var entity in pagedInfo.Data)
            {
                var resourceTypeDto = entity.ToModel<ProcResourceDto>();
                procResourceDtos.Add(resourceTypeDto);
            }
            return new PagedInfo<ProcResourceDto>(procResourceDtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 资源关联打印机数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ProcResourceConfigPrintViewDto>> GetcResourceConfigPrintAsync(ProcResourceConfigPrintPagedQueryDto query)
        {
            var resourcePagedQuery = query.ToQuery<ProcResourceConfigPrintPagedQuery>();
            var pagedInfo = await _resourceConfigPrintRepository.GetPagedInfoAsync(resourcePagedQuery);

            //实体到DTO转换 装载数据
            var procResourceConfigPrintViewDtos = new List<ProcResourceConfigPrintViewDto>();
            foreach (var entity in pagedInfo.Data)
            {
                var resourceTypeDto = entity.ToModel<ProcResourceConfigPrintViewDto>();
                procResourceConfigPrintViewDtos.Add(resourceTypeDto);
            }
            return new PagedInfo<ProcResourceConfigPrintViewDto>(procResourceConfigPrintViewDtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 资源设置数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ProcResourceConfigResDto>> GetcResourceConfigResAsync(ProcResourceConfigResPagedQueryDto query)
        {
            var resPagedQuery = query.ToQuery<ProcResourceConfigResPagedQuery>();
            var pagedInfo = await _procResourceConfigResRepository.GetPagedInfoAsync(resPagedQuery);

            //实体到DTO转换 装载数据
            var resourceConfigResDtos = new List<ProcResourceConfigResDto>();
            foreach (var entity in pagedInfo.Data)
            {
                var resourceTypeDto = entity.ToModel<ProcResourceConfigResDto>();
                resourceConfigResDtos.Add(resourceTypeDto);
            }
            return new PagedInfo<ProcResourceConfigResDto>(resourceConfigResDtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 获取资源关联设备数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ProcResourceEquipmentBindViewDto>> GetcResourceConfigEquAsync(ProcResourceEquipmentBindPagedQueryDto query)
        {
            var resPagedQuery = query.ToQuery<ProcResourceEquipmentBindPagedQuery>();
            var pagedInfo = await _resourceEquipmentBindRepository.GetPagedInfoAsync(resPagedQuery);

            //实体到DTO转换 装载数据
            var procResourceEquipmentBinds = new List<ProcResourceEquipmentBindViewDto>();
            foreach (var entity in pagedInfo.Data)
            {
                var resourceTypeDto = entity.ToModel<ProcResourceEquipmentBindViewDto>();
                procResourceEquipmentBinds.Add(resourceTypeDto);
            }
            return new PagedInfo<ProcResourceEquipmentBindViewDto>(procResourceEquipmentBinds, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 获取资源关联作业
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ProcResourceConfigJobViewDto>> GetcResourceConfigJoAsync(ProcResourceEquipmentBindPagedQueryDto query)
        {
            var resPagedQuery = query.ToQuery<ProcResourceEquipmentBindPagedQuery>();
            var pagedInfo = await _resourceEquipmentBindRepository.GetPagedInfoAsync(resPagedQuery);

            //实体到DTO转换 装载数据
            var procResourceConfigJobViews = new List<ProcResourceConfigJobViewDto>();
            foreach (var entity in pagedInfo.Data)
            {
                var resourceTypeDto = entity.ToModel<ProcResourceConfigJobViewDto>();
                procResourceConfigJobViews.Add(resourceTypeDto);
            }
            return new PagedInfo<ProcResourceConfigJobViewDto>(procResourceConfigJobViews, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 添加资源数据
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task AddProcResourceAsync(ProcResourceDto param)
        {
            //验证DTO
            //var dto = new ProcResourceTypeDto();
            //await _validationRules.ValidateAndThrowAsync(dto);
            //DTO转换实体
            var entity = new ProcResourceEntity
            {
                Id = IdGenProvider.Instance.CreateId(),
                SiteCode = "TODO",
                CreatedBy = "TODO",
                UpdatedBy = "TODO",
                UpdatedOn = HymsonClock.Now(),
                CreatedOn = HymsonClock.Now(),
                Status = param.Status,
                ResTypeId = param.ResTypeId,
                Remark = param.Remark ?? "",
                ResCode = param.ResCode ?? "",
                ResName = param.ResName ?? ""
            };
            //入库
            await _resourceRepository.InsertAsync(entity);
        }

        /// <summary>
        /// 修改资源数据
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<int> UpdateProcResrouceAsync(ProcResourceDto param)
        {
            //验证DTO
            //var dto = new ProcResourceTypeDto();
            //await _validationRules.ValidateAndThrowAsync(dto);
            //DTO转换实体
            var entity = new ProcResourceEntity
            {
                Id = param.Id,
                Remark = param.Remark ?? "",
                UpdatedOn = HymsonClock.Now(),
                UpdatedBy = "TODO"
            };
            // 保存实体
            return await _resourceRepository.UpdateAsync(entity);
        }

        /// <summary>
        /// 批量删除资源类型数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeleteProcResourceAsync(string ids)
        {
            long[] idsArr = StringExtension.SpitLongArrary(ids);
            if (idsArr.Length < 1)
            {
                throw new NotFoundException(ErrorCode.MES10100, "删除失败Id 不能为空!");
            }

            var siteCode = "TODO";
            //不能删除启用状态的资源
            var query = new ProcResourceQuery
            {
                IdsArr = idsArr,
                SiteCode = siteCode,
                //TODO
                Status = "1"
            };
            var resourceList = await _resourceRepository.GetByIdsAsync(query);
            if (resourceList != null && resourceList.Any())
            {
                throw new CustomerValidationException(ErrorCode.MES10100, "不能删除启用状态的资源!");
            }

            return await _resourceRepository.DeleteAsync(idsArr);
        }
    }
}
