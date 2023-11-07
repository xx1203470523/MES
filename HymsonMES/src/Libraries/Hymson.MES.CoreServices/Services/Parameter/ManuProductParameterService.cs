using Hymson.MES.Core.Domain.Parameter;
using Hymson.MES.CoreServices.Dtos.Parameter;
using Hymson.MES.Data.Repositories.Parameter.ManuProductParameter;
using Hymson.MES.Data.Repositories.Parameter.ManuProductParameter.Query;
using Hymson.MES.Data.Repositories.Process;
using Microsoft.Extensions.Options;

namespace Hymson.MES.CoreServices.Services.Parameter
{
    /// <summary>
    /// 参数采集
    /// </summary>
    public class ManuProductParameterService : IManuProductParameterService
    {
        private readonly IManuProductParameterRepository _manuProductParameterRepository;

        /// <summary>
        /// 参数采集
        /// </summary>
        /// <param name="manuProductParameterRepository"></param>
        /// <param name="parameterOptions"></param>
        /// <param name="procProcedureRepository"></param>
        public ManuProductParameterService(IManuProductParameterRepository manuProductParameterRepository)
        {
            _manuProductParameterRepository = manuProductParameterRepository;
        }

        /// <summary>
        /// 根据工序参数信息
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuProductParameterEntity>?> GetProductParameterListByProcedureAsync(QueryParameterByProcedureDto param)
        {
            return await _manuProductParameterRepository.GetProductParameterByProcedureIdEntities(new ManuProductParameterByProcedureIdQuery
            {
                ProcedureId = param.ProcedureId,
                SiteId = param.SiteId,
                SFCs = param.SFCs,
            });
        }
    }
}
