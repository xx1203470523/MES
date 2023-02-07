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
        private readonly AbstractValidator<ProcMaterialDto> _validationRules;

		public ProcMaterialService(IProcMaterialRepository procMaterialRepository, AbstractValidator<ProcMaterialDto> validationRules)
		{
			_procMaterialRepository = procMaterialRepository;
			_validationRules = validationRules;
		}

		/// <summary>
        /// 创建
        /// </summary>
        /// <param name="procMaterialDto"></param>
        /// <returns></returns>
		public async Task CreateProcMaterialAsync(ProcMaterialDto procMaterialDto)
		{
			throw new NotImplementedException();
		}

		/// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
		public Task DeleteProcMaterialAsync(long id)
		{
			throw new NotImplementedException();
		}

		/// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
		/// <param name="procMaterialPagedQueryDto"></param>
		/// <returns></returns>
		public async Task<PagedInfo<ProcMaterialDto>> GetListAsync(ProcMaterialPagedQueryDto procMaterialQueryDto)
		{
			throw new NotImplementedException();
		}

		/// <summary>
        /// 
        /// </summary>
        /// <param name="pagedInfo"></param>
        /// <returns></returns>
		private static List<ProcMaterialDto> PrepareProcMaterialDtos(PagedInfo<ProcMaterialEntity>   pagedInfo)
		{
			throw new NotImplementedException();
		}

		/// <summary>
        /// 修改
        /// </summary>
        /// <param name="procMaterialDto"></param>
        /// <returns></returns>
		public Task ModifyProcMaterialAsync(ProcMaterialDto procMaterialDto)
		{
			throw new NotImplementedException();
		}
    }
}
