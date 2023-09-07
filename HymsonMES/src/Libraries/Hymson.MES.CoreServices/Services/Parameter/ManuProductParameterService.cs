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
        /// 插入数据
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task InsertRangeAsync(IEnumerable<ParameterDto> param)
        {
            var dic = new Dictionary<string, List<ManuProductParameterEntity>>();

            var procProcedureList = await _procProcedureRepository.GetByIdsAsync(param.Select(x => x.ProcedureId).ToList<long>());

            foreach (var paramDto in param)
            {
                var entity = paramDto.ToEntity<ManuProductParameterEntity>();
                entity.CreatedBy = paramDto.UserName;
                entity.UpdatedBy = paramDto.UserName;
                entity.CreatedOn = paramDto.Date;
                entity.UpdatedOn = paramDto.Date;
                entity.Id = IdGenProvider.Instance.CreateId();

                var tableNameBySFC = GetTableNameBySFC(paramDto.SiteId, paramDto.SFC);
                if (!dic.ContainsKey(tableNameBySFC))
                {
                    dic[tableNameBySFC] = new List<ManuProductParameterEntity>();
                }
                dic[tableNameBySFC].Add(entity);

                var procProcedure = procProcedureList.FirstOrDefault(x => x.Id == paramDto.ProcedureId);
                if (procProcedure != null)
                {
                    var tableNameByProcedureCode = GetTableNameByProcedureCode(paramDto.SiteId, procProcedure.Code);

                    if (!dic.ContainsKey(tableNameByProcedureCode))
                    {
                        dic[tableNameByProcedureCode] = new List<ManuProductParameterEntity>();
                    }
                    dic[tableNameByProcedureCode].Add(entity);
                }
                else
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES10476));
                }
            }

            using (var trans = TransactionHelper.GetTransactionScope())
            {
                // 更新数据
                List<Task<int>> tasks = new();
                foreach (var dicItem in dic)
                {
                    tasks.Add(_manuProductParameterRepository.InsertRangeAsync(dicItem.Value, dicItem.Key));
                }
                await Task.WhenAll(tasks);
                trans.Complete();
            }
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

        /// <summary>
        /// 根据条码获取参数信息
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuProductParameterEntity>> GetProductParameterListBySFCAsync(QueryParameterBySfcDto param)
        {
            var list = new List<ManuProductParameterEntity>();
            var dic = new Dictionary<string, List<string>>();

            foreach (var sfc in param.SFCs)
            {
                var tableNameBySFC = GetTableNameBySFC(param.SiteId, sfc);
                if (!dic.ContainsKey(tableNameBySFC))
                {
                    dic[tableNameBySFC] = new List<string>();
                }
                dic[tableNameBySFC].Add(sfc);
            }

            // 更新数据
            List<Task<IEnumerable<ManuProductParameterEntity>>> tasks = new();
            foreach (var dicItem in dic)
            {
                tasks.Add(_manuProductParameterRepository.GetProductParameterEntities(new ManuProductParameterBySfcQuery { SFCs = dicItem.Value, SiteId = param.SiteId }, dicItem.Key));
            }
            var result = await Task.WhenAll(tasks);
            foreach (var item in result)
            {
                list.AddRange(item);
            }
            return list;
        }

        /// <summary>
        /// 修改参数
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task UpdateProductParameterByIdAsync(List<UpdateParameterDto> param)
        {
            var dic = new Dictionary<string, List<ManuProductParameterUpdateCommand>>();

            var procProcedureList = await _procProcedureRepository.GetByIdsAsync(param.Select(x => x.ProcedureId).ToList<long>());

            foreach (var paramDto in param)
            {
                var tableNameBySFC = GetTableNameBySFC(paramDto.SiteId, paramDto.SFC);
                if (!dic.ContainsKey(tableNameBySFC))
                {
                    dic[tableNameBySFC] = new List<ManuProductParameterUpdateCommand>();
                }
                dic[tableNameBySFC].Add(new ManuProductParameterUpdateCommand
                {
                    Id = paramDto.Id,
                    ParameterValue = paramDto.ParameterValue,
                    UserId = paramDto.UserId,
                    UpdatedOn = paramDto.UpdatedOn,
                });
                var procProcedure = procProcedureList.FirstOrDefault(x => x.Id == paramDto.ProcedureId);
                if (procProcedure != null)
                {
                    var tableNameByProcedureCode = GetTableNameByProcedureCode(paramDto.SiteId, procProcedure.Code);

                    if (!dic.ContainsKey(tableNameByProcedureCode))
                    {
                        dic[tableNameByProcedureCode] = new List<ManuProductParameterUpdateCommand>();
                    }
                    dic[tableNameByProcedureCode].Add(new ManuProductParameterUpdateCommand
                    {
                        Id = paramDto.Id,
                        ParameterValue = paramDto.ParameterValue,
                        UserId = paramDto.UserId,
                        UpdatedOn = paramDto.UpdatedOn,
                    });
                }
                else
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES10476));
                }
            }

            using (var trans = TransactionHelper.GetTransactionScope())
            {
                // 更新数据
                List<Task<int>> tasks = new();
                foreach (var dicItem in dic)
                {
                    tasks.Add(_manuProductParameterRepository.UpdateRangeAsync(dicItem.Value, dicItem.Key));
                }

                await Task.WhenAll(tasks);

                trans.Complete();
            }
        }

        /// <summary>
        ///创建数据库表
        /// </summary>
        /// <returns></returns>
        public async Task CreateProductParameterTableAsync(string tabname)
        {
            var sql = await _manuProductParameterRepository.ShowCreateTableAsync(ProductParameter.ProductProcedureParameterTemplateNmae);
            sql= sql?.Replace(ProductParameter.ProductProcedureParameterTemplateNmae, tabname);
            sql= sql?.Replace($"CREATE TABLE", $"CREATE TABLE  IF NOT EXISTS");
            await _manuProductParameterRepository.CreateProductParameterTableAsync(sql ?? "");
        }

        /// <summary>
        /// 根据工序创建数据库表
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="procedureCode"></param>
        /// <returns></returns>
        public async Task CreateProductParameterProcedureCodeTableAsync(long siteId, string procedureCode)
        {
            var tabname = GetTableNameByProcedureCode(siteId, procedureCode);

            await CreateProductParameterTableAsync(tabname);
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
