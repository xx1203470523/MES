/*
 *creator: Karl
 *
 *describe: 物料维护    服务 | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-02-07 11:16:51
 */
using FluentValidation;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Services.Dtos.Process;
using Hymson.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Services.Process
{
	/// <summary>
    /// 物料维护 服务
    /// </summary>
    public class ProcMaterialService : IProcMaterialService
    {
		 /// <summary>
        /// 物料维护 仓储
        /// </summary>
        private readonly IProcMaterialRepository _procMaterialRepository;
        private readonly AbstractValidator<ProcMaterialCreateDto> _validationCreateRules;
        private readonly AbstractValidator<ProcMaterialModifyDto> _validationModifyRules;

        public ProcMaterialService(IProcMaterialRepository procMaterialRepository, AbstractValidator<ProcMaterialCreateDto> validationCreateRules, AbstractValidator<ProcMaterialModifyDto> validationModifyRules)
		{
			_procMaterialRepository = procMaterialRepository;
            _validationCreateRules = validationCreateRules;
            _validationModifyRules = validationModifyRules;

        }

		/// <summary>
        /// 创建
        /// </summary>
        /// <param name="procMaterialDto"></param>
        /// <returns></returns>
		public async Task CreateProcMaterialAsync(ProcMaterialCreateDto procMaterialCreateDto)
		{
            //验证DTO
            await _validationCreateRules.ValidateAndThrowAsync(procMaterialCreateDto);

            //DTO转换实体
            var procMaterialEntity = procMaterialCreateDto.ToEntity<ProcMaterialEntity>();
            procMaterialEntity.CreatedBy = "TODO";
            procMaterialEntity.UpdatedBy = "TODO";
            procMaterialEntity.CreatedOn = HymsonClock.Now();
            procMaterialEntity.UpdatedOn = HymsonClock.Now();

            //入库
            await _procMaterialRepository.InsertAsync(procMaterialEntity);
        }

		/// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
		public async Task DeleteProcMaterialAsync(long id)
		{
            await _procMaterialRepository.DeleteAsync(id);
		}

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="idsArr"></param>
        /// <returns></returns>
        public async Task<int> DeletesProcMaterialAsync(long[] idsArr)
        {
            return await _procMaterialRepository.DeletesAsync(idsArr);
        }


        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="procMaterialPagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ProcMaterialDto>> GetPageListAsync(ProcMaterialPagedQueryDto procMaterialPagedQueryDto)
		{
            var procMaterialPagedQuery = procMaterialPagedQueryDto.ToQuery<ProcMaterialPagedQuery>();
            var pagedInfo = await _procMaterialRepository.GetPagedInfoAsync(procMaterialPagedQuery);

            //实体到DTO转换 装载数据
            List<ProcMaterialDto> procMaterialDtos = PrepareProcMaterialDtos(pagedInfo);
            return new PagedInfo<ProcMaterialDto>(procMaterialDtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

		/// <summary>
        /// 
        /// </summary>
        /// <param name="pagedInfo"></param>
        /// <returns></returns>
		private static List<ProcMaterialDto> PrepareProcMaterialDtos(PagedInfo<ProcMaterialEntity>   pagedInfo)
		{
            var procMaterialDtos = new List<ProcMaterialDto>();
            foreach (var procMaterialEntity in pagedInfo.Data)
            {
                var procMaterialDto = procMaterialEntity.ToModel<ProcMaterialDto>();
                procMaterialDtos.Add(procMaterialDto);
            }

            return procMaterialDtos;
        }

		/// <summary>
        /// 修改
        /// </summary>
        /// <param name="procMaterialDto"></param>
        /// <returns></returns>
		public async Task ModifyProcMaterialAsync(ProcMaterialModifyDto procMaterialModifyDto)
		{
            //验证DTO
            await _validationModifyRules.ValidateAndThrowAsync(procMaterialModifyDto);

            //DTO转换实体
            var procMaterialEntity = procMaterialModifyDto.ToEntity<ProcMaterialEntity>();
            procMaterialEntity.UpdatedBy = "TODO";
            procMaterialEntity.UpdatedOn = HymsonClock.Now();

            await _procMaterialRepository.UpdateAsync(procMaterialEntity);
		}

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ProcMaterialDto> QueryProcMaterialByIdAsync(long id) 
        {
           var procMaterialEntity = await _procMaterialRepository.GetByIdAsync(id);
           if (procMaterialEntity != null) 
           {
               return procMaterialEntity.ToModel<ProcMaterialDto>();
           }
            return null;
        }
    }
}
