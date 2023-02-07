using FluentValidation;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Domain.OnStock;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Data.Repositories.OnStock;
using Hymson.MES.Data.Repositories.Process.ResourceType;
using Hymson.MES.Services.Dtos.OnStock;
using Hymson.MES.Services.Dtos.Process.ResourceType;
using Hymson.MES.Services.Services.OnStock;
using Hymson.MES.Services.Services.Process.IProcessService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public ProcResourceTypeService(IProcResourceTypeRepository resourceTypeRepository)
        {
            _resourceTypeRepository = resourceTypeRepository;
            //_validationRules = validationRules;
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

        public async Task AddProcResourceTypeAsync(ProcResourceTypeDto param)
        {
            //验证DTO
            await _validationRules.ValidateAndThrowAsync(param);
            ////DTO转换实体
            //var whStockChangeRecordEntity = param.ToEntity<ProcResourceTypeEntity>();
            //whStockChangeRecordEntity.CreateBy = "jinyi";
            //whStockChangeRecordEntity.UpdateBy = "jinyi";
            ////入库
            //await _resourceTypeRepository.InsertAsync(whStockChangeRecordEntity);
        }

    }
}
