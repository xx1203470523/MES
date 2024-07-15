using Hymson.MES.BackgroundServices.NIO.Repositories.Mes.Material.View;
using Hymson.MES.Core.Domain.Process;

namespace Hymson.MES.BackgroundServices.NIO.Repositories.Mes.Material
{
    /// <summary>
    /// 物料维护仓储接口
    /// </summary>
    public interface IProcMaterialMavelRepository
    {
        /// <summary>
        /// 获取自制品列表
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<IEnumerable<ProcMaterialEntity>> GetSelfControlListAsync(MavelMaterialQuery param);
    }
}
