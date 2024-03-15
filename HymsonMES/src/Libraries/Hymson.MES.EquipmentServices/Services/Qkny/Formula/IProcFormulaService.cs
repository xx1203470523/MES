using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Data.Repositories.Process.Query;
using Hymson.MES.Data.Repositories.Process.View;
using Hymson.MES.EquipmentServices.Dtos.Qkny.Formula;
using Hymson.MES.EquipmentServices.Dtos.Qkny.Manufacture;
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
    public interface IProcFormulaService
    {
        /// <summary>
        /// 获取详情
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        Task<List<ProcFormulaListViewDto>> GetFormulaListAsync(ProcFormulaListQueryDto queryDto);

        /// <summary>
        /// 获取配方详情
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<List<ProcFormulaDetailViewDto>> GetFormulaDetailAsync(ProcFormulaDetailQueryDto query);

        /// <summary>
        /// 配方版本校验
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<ProcFormulaEntity> GetEntityByCodeVersion(ProcFormlaGetByCodeVersionDto dto);
    }
}
