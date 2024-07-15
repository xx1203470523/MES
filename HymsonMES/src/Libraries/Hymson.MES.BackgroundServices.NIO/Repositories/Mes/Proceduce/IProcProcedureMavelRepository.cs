using Hymson.Infrastructure;
using Hymson.MES.BackgroundServices.NIO.Repositories.Mes.Proceduce.View;
using Hymson.MES.Core.Domain.Process;

namespace Hymson.MES.BackgroundServices.NIO.Repositories.Mes.Proceduce
{
    /// <summary>
    /// 工序表仓储接口
    /// </summary>
    public interface IProcProcedureMavelRepository
    {
        /// <summary>
        /// 获取站点下的所有工序
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<IEnumerable<ProcProcedureEntity>> GetList(MavelProducreQuery param);
    }
}
