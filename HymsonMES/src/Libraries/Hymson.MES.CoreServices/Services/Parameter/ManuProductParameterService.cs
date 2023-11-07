using Force.Crc32;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Constants.Parameter;
using Hymson.MES.Core.Domain.Parameter;
using Hymson.MES.CoreServices.Dtos.Parameter;
using Hymson.MES.CoreServices.Options;
using Hymson.MES.Data.Repositories.Parameter.ManuProductParameter;
using Hymson.MES.Data.Repositories.Parameter.ManuProductParameter.Command;
using Hymson.MES.Data.Repositories.Parameter.ManuProductParameter.Query;
using Hymson.MES.Data.Repositories.Process;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Xml.Linq;

namespace Hymson.MES.CoreServices.Services.Parameter
{
    /// <summary>
    /// 参数采集
    /// </summary>
    public class ManuProductParameterService : IManuProductParameterService
    {
        private readonly IManuProductParameterRepository _manuProductParameterRepository;
        private readonly IProcProcedureRepository _procProcedureRepository;
        private readonly ParameterOptions _parameterOptions;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="manuProductParameterRepository"></param>
        /// <param name="parameterOptions"></param>
        /// <param name="procProcedureRepository"></param>
        public ManuProductParameterService(IManuProductParameterRepository manuProductParameterRepository, IOptions<ParameterOptions> parameterOptions, IProcProcedureRepository procProcedureRepository)
        {
            _manuProductParameterRepository = manuProductParameterRepository;
            _procProcedureRepository = procProcedureRepository;
            _parameterOptions = parameterOptions.Value;
        }

        /// <summary>
        /// 根据工序参数信息
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuProductParameterEntity>?> GetProductParameterListByProcedureAsync(QueryParameterByProcedureDto param)
        {
            var procProcedureEntity = await _procProcedureRepository.GetByIdAsync(param.ProcedureId);
            if (procProcedureEntity == null)
            {
                return null;
            }
            var tableNameByProcedureCode = GetTableNameByProcedureCode(param.SiteId, procProcedureEntity.Code);
            return await _manuProductParameterRepository.GetProductParameterEntities(new ManuProductParameterBySfcQuery
            {
                SiteId = param.SiteId,
                SFCs = param.SFCs,
            }, tableNameByProcedureCode);
        }

        #region 内部方法
        /// <summary>
        /// 更具SFC获取表名
        /// </summary>
        /// <param name="paran"></param>
        /// <returns></returns>
        private string GetTableNameBySFC(long siteId, string sfc)
        {
            var key = CalculateCrc32($"{siteId}{sfc}");

            return $"{ProductParameter.ProductParameterPrefix}{key % _parameterOptions.ParameterDelivery}";
        }

        /// <summary>
        /// 更具工序编码获取表名
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="procedureCode"></param>
        /// <returns></returns>.
        private static string GetTableNameByProcedureCode(long siteId, string procedureCode)
        {
            var key = $"{siteId}_{procedureCode}";

            return $"{ProductParameter.ProductProcedureParameterPrefix}{key}";
        }

        /// <summary>
        /// SHA256 hash
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static BigInteger CalculateSHA256Hash(string input)
        {
            using (SHA256 hasher = SHA256.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = hasher.ComputeHash(inputBytes);

                BigInteger hashValue = new BigInteger(hashBytes);

                return hashValue;
            }
        }

        public static uint CalculateCrc32(string input)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(input);
            return Crc32Algorithm.Compute(bytes);
        }
        #endregion
    }
}
