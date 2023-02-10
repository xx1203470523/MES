/*
 *creator: Karl
 *
 *describe: 物料组维护表    服务 | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-02-10 03:54:07
 */
using FluentValidation;
using Google.Protobuf.WellKnownTypes;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Services.Dtos.Process;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Extensions;
using System.Transactions;

namespace Hymson.MES.Services.Services.Process
{
    /// <summary>
    /// 物料组维护表 服务
    /// </summary>
    public class ProcMaterialGroupService : IProcMaterialGroupService
    {
        /// <summary>
        /// 物料组维护表 仓储
        /// </summary>
        private readonly IProcMaterialGroupRepository _procMaterialGroupRepository;
        private readonly AbstractValidator<ProcMaterialGroupCreateDto> _validationCreateRules;
        private readonly AbstractValidator<ProcMaterialGroupModifyDto> _validationModifyRules;

        public ProcMaterialGroupService(IProcMaterialGroupRepository procMaterialGroupRepository, AbstractValidator<ProcMaterialGroupCreateDto> validationCreateRules, AbstractValidator<ProcMaterialGroupModifyDto> validationModifyRules)
        {
            _procMaterialGroupRepository = procMaterialGroupRepository;
            _validationCreateRules = validationCreateRules;
            _validationModifyRules = validationModifyRules;
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="procMaterialGroupDto"></param>
        /// <returns></returns>
        public async Task CreateProcMaterialGroupAsync(ProcMaterialGroupCreateDto procMaterialGroupCreateDto)
        {
            //验证DTO
            await _validationCreateRules.ValidateAndThrowAsync(procMaterialGroupCreateDto);

            //DTO转换实体
            var procMaterialGroupEntity = procMaterialGroupCreateDto.ToEntity<ProcMaterialGroupEntity>();
            procMaterialGroupEntity.Id= IdGenProvider.Instance.CreateId();
            procMaterialGroupEntity.CreatedBy = "TODO";
            procMaterialGroupEntity.UpdatedBy = "TODO";
            procMaterialGroupEntity.CreatedOn = HymsonClock.Now();
            procMaterialGroupEntity.UpdatedOn = HymsonClock.Now();

            //入库
            await _procMaterialGroupRepository.InsertAsync(procMaterialGroupEntity);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteProcMaterialGroupAsync(long id)
        {
            await _procMaterialGroupRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeletesProcMaterialGroupAsync(string ids)
        {
            var idsArr = StringExtension.SpitLongArrary(ids);
            return await _procMaterialGroupRepository.DeletesAsync(idsArr);
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="procMaterialGroupPagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ProcMaterialGroupDto>> GetPageListAsync(ProcMaterialGroupPagedQueryDto procMaterialGroupPagedQueryDto)
        {
            //TODO 
            procMaterialGroupPagedQueryDto.SiteCode = "";

            var procMaterialGroupPagedQuery = procMaterialGroupPagedQueryDto.ToQuery<ProcMaterialGroupPagedQuery>();
            var pagedInfo = await _procMaterialGroupRepository.GetPagedInfoAsync(procMaterialGroupPagedQuery);

            //实体到DTO转换 装载数据
            List<ProcMaterialGroupDto> procMaterialGroupDtos = PrepareProcMaterialGroupDtos(pagedInfo);
            return new PagedInfo<ProcMaterialGroupDto>(procMaterialGroupDtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pagedInfo"></param>
        /// <returns></returns>
        private static List<ProcMaterialGroupDto> PrepareProcMaterialGroupDtos(PagedInfo<ProcMaterialGroupEntity>   pagedInfo)
        {
            var procMaterialGroupDtos = new List<ProcMaterialGroupDto>();
            foreach (var procMaterialGroupEntity in pagedInfo.Data)
            {
                var procMaterialGroupDto = procMaterialGroupEntity.ToModel<ProcMaterialGroupDto>();
                procMaterialGroupDtos.Add(procMaterialGroupDto);
            }

            return procMaterialGroupDtos;
        }

        /// <summary>
        /// 获取分页自定义List
        /// </summary>
        /// <param name="customProcMaterialGroupPagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<CustomProcMaterialGroupViewDto>> GetPageCustomListAsync(CustomProcMaterialGroupPagedQueryDto customProcMaterialGroupPagedQueryDto) 
        {
            //TODO 
            customProcMaterialGroupPagedQueryDto.SiteCode = "";

            var procMaterialGroupCustomPagedQuery = customProcMaterialGroupPagedQueryDto.ToQuery<ProcMaterialGroupCustomPagedQuery>();
            var pagedInfo = await _procMaterialGroupRepository.GetPagedCustomInfoAsync(procMaterialGroupCustomPagedQuery);

            //实体到DTO转换 装载数据
            List<CustomProcMaterialGroupViewDto> procMaterialGroupDtos = PrepareCustomProcMaterialGroupDtos(pagedInfo);
            return new PagedInfo<CustomProcMaterialGroupViewDto>(procMaterialGroupDtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pagedInfo"></param>
        /// <returns></returns>
        private static List<CustomProcMaterialGroupViewDto> PrepareCustomProcMaterialGroupDtos(PagedInfo<CustomProcMaterialGroupView> pagedInfo)
        {
            var customProcMaterialGroupViewDtos = new List<CustomProcMaterialGroupViewDto>();
            foreach (var customProcMaterialGroupView in pagedInfo.Data)
            {
                var customProcMaterialGroupViewDto = customProcMaterialGroupView.ToModel<CustomProcMaterialGroupViewDto>();
                customProcMaterialGroupViewDtos.Add(customProcMaterialGroupViewDto);
            }

            return customProcMaterialGroupViewDtos;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="procMaterialGroupDto"></param>
        /// <returns></returns>
        public async Task ModifyProcMaterialGroupAsync(ProcMaterialGroupModifyDto procMaterialGroupModifyDto)
        {
             //验证DTO
            await _validationModifyRules.ValidateAndThrowAsync(procMaterialGroupModifyDto);

            //DTO转换实体
            var procMaterialGroupEntity = procMaterialGroupModifyDto.ToEntity<ProcMaterialGroupEntity>();
            procMaterialGroupEntity.UpdatedBy = "TODO";
            procMaterialGroupEntity.UpdatedOn = HymsonClock.Now();

            await _procMaterialGroupRepository.UpdateAsync(procMaterialGroupEntity);
        }

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ProcMaterialGroupDto> QueryProcMaterialGroupByIdAsync(long id) 
        {
           var procMaterialGroupEntity = await _procMaterialGroupRepository.GetByIdAsync(id);
           if (procMaterialGroupEntity != null) 
           {
               return procMaterialGroupEntity.ToModel<ProcMaterialGroupDto>();
           }
            return null;
        }
    }
}
