using FluentValidation;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Data.Repositories.OnStock;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Services.Dtos.Process;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Services.Process
{
    public class ProcMaterialService : IProcMaterialService
    {
        private readonly IProcMaterialRepository _procMaterialRepository;
        private readonly AbstractValidator<ProcMaterialDto> _validationRules;

        public ProcMaterialService(IProcMaterialRepository procMaterialRepository, AbstractValidator<ProcMaterialDto> validationRules)
        {
            _procMaterialRepository = procMaterialRepository;
            _validationRules = validationRules;
        }

        public async Task CreateProcMaterialAsync(ProcMaterialDto procMaterialDto)
        {
            throw new NotImplementedException();
        }

        public Task DeleteProcMaterialAsync(long id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="whStockChangeRecordPagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ProcMaterialDto>> GetListAsync(ProcMaterialPagedQueryDto procMaterialQueryDto)
        {
            throw new NotImplementedException();
        }

        private static List<ProcMaterialDto> PrepareProcMaterialDtos(PagedInfo<ProcMaterialEntity> pagedInfo)
        {
            throw new NotImplementedException();
        }

        public Task ModifyProcMaterialAsync(ProcMaterialDto procMaterialDto)
        {
            throw new NotImplementedException();
        }
    }
}
