using FluentValidation;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Data.Repositories.Process.ResourceType;
using Hymson.MES.Services.Dtos.Process.ResourceType;
using Hymson.MES.Services.Services.Process.IProcessService;
using Hymson.Snowflake;

namespace Hymson.MES.Services.Services.Process
{
    /// <summary>
    /// 资源维护表Service业务层处理
    /// @tableName proc_resource_type
    /// @author zhaoqing
    /// @date 2023-02-06
    /// </summary>
    public class ProcResourceTypeService : IProcResourceTypeService
    {
        private readonly IProcResourceTypeRepository _resourceTypeRepository;
        private readonly AbstractValidator<ProcResourceTypeDto> _validationRules;

        /// <summary>
        /// 构造函数
        /// </summary>
        public ProcResourceTypeService(IProcResourceTypeRepository resourceTypeRepository,AbstractValidator<ProcResourceTypeDto> validationRules)
        {
            _resourceTypeRepository = resourceTypeRepository;
            _validationRules = validationRules;
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
                var whStockChangeRecordDto = entity.ToModel<ProcResourceTypeDto>();
                procResourceTypeDtos.Add(whStockChangeRecordDto);
            }
            return new PagedInfo<ProcResourceTypeDto>(procResourceTypeDtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        public async Task AddProcResourceTypeAsync(ProcResourceTypeAddCommandDto param)
        {
            //验证DTO
            var dto = new ProcResourceTypeDto();
            await _validationRules.ValidateAndThrowAsync(dto);
            //DTO转换实体
            var entity = param.ToEntity<ProcResourceTypeAddCommand>();
            entity.Id = IdGenProvider.Instance.CreateId();
            entity.CreatedBy = "TODO";
            entity.UpdatedBy = "TODO";
            //入库
            await _resourceTypeRepository.InsertAsync(entity);
        }

    }
}
