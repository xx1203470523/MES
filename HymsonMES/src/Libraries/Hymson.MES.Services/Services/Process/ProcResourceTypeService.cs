using FluentValidation;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Data.Repositories.Process.ResourceType;
using Hymson.MES.Services.Dtos.Process;
using Hymson.MES.Services.Services.Process.IProcessService;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Extensions;
using System.Transactions;

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
        //private readonly AbstractValidator<ProcResourceTypeDto> _validationRules;

        /// <summary>
        /// 构造函数
        /// </summary>
        public ProcResourceTypeService(IProcResourceTypeRepository resourceTypeRepository)
            //AbstractValidator<ProcResourceTypeDto> validationRules)
        {
            _resourceTypeRepository = resourceTypeRepository;
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
                var whStockChangeRecordDto = entity.ToModel<ProcResourceTypeDto>();
                procResourceTypeDtos.Add(whStockChangeRecordDto);
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
            //DTO转换实体
            var entity = new ProcResourceTypeAddCommand
            {
                Id = IdGenProvider.Instance.CreateId(),
                SiteCode = "TODO",
                CreatedBy = "TODO",
                UpdatedBy = "TODO",
                UpdatedOn = HymsonClock.Now(),
                CreatedOn = HymsonClock.Now(),
                Remark = param.Remark ?? "",
                ResType = param.ResType ?? "",
                ResTypeName = param.ResTypeName ?? ""
            };
            //入库
           var count= await _resourceTypeRepository.InsertAsync(entity);

            string s = "";
        }

        /// <summary>
        /// 修改资源类型数据
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<int> UpdateProcResrouceTypeAsync(ProcResourceTypeUpdateCommandDto param)
        {
            //验证DTO
            //var dto = new ProcResourceTypeDto();
            //await _validationRules.ValidateAndThrowAsync(dto);
            //DTO转换实体
            var entity = new ProcResourceTypeUpdateCommand
            {
                Id = param.Id,
                Remark = param.Remark ?? "",
                ResTypeName = param.ResTypeName ?? "",
                UpdatedOn = HymsonClock.Now(),
                UpdatedBy = "TODO"
            };
            // 保存实体
            return await _resourceTypeRepository.UpdateAsync(entity);
        }

        /// <summary>
        /// 批量删除资源类型数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeleteProcResourceTypeAsync(string ids)
        {
            long[] idsArr = StringExtension.SpitLongArrary(ids);
            return await _resourceTypeRepository.DeleteAsync(idsArr);
        }
    }
}
