using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.Process.Query;
using Hymson.MES.Data.Repositories.Process.View;
using Hymson.MES.EquipmentServices.Dtos.Qkny.Formula;
using Hymson.MES.EquipmentServices.Dtos.Qkny.Manufacture;
using Hymson.Utils;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.EquipmentServices.Services.Qkny.Formula
{
    /// <summary>
    /// 配方
    /// </summary>
    public class ProcFormulaService : IProcFormulaService
    {
        /// <summary>
        /// 仓储接口（配方维护）
        /// </summary>
        private readonly IProcFormulaRepository _procFormulaRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        public ProcFormulaService(IProcFormulaRepository procFormulaRepository)
        {
            _procFormulaRepository = procFormulaRepository;
        }

        /// <summary>
        /// 获取配方列表
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        public async Task<List<ProcFormulaListViewDto>> GetFormulaListAsync(ProcFormulaListQueryDto queryDto)
        {
            var dbList = await _procFormulaRepository.GetFormulaListAsync(queryDto);
            if(dbList.IsNullOrEmpty() == true)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES45050));
            }
            return dbList;
        }

        /// <summary>
        /// 获取配方详情
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<List<ProcFormulaDetailViewDto>> GetFormulaDetailAsync(ProcFormulaDetailQueryDto query)
        {
            var dbList = await _procFormulaRepository.GetFormulaDetailAsync(query);
            if(dbList.IsNullOrEmpty() == true)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES45051));
            }
            return dbList;
        }

        /// <summary>
        /// 获取配方
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<ProcFormulaEntity> GetEntityByCodeVersion(ProcFormlaGetByCodeVersionDto dto)
        {
            ProcFormulaByCodeAndVersion query = new ProcFormulaByCodeAndVersion();
            query.SiteId = dto.SiteId;
            query.Code = dto.FormulaCode;
            query.Version = dto.Version;
            var model = await _procFormulaRepository.GetActivateByCodeAndVersionAsync(query);
            if (model == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES45052));
            }
            return model;
        }
    }
}
