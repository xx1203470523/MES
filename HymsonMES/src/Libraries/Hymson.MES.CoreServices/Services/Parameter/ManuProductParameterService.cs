using Force.Crc32;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Constants.Parameter;
using Hymson.MES.Core.Domain.Parameter;
using Hymson.MES.CoreServices.Bos.Parameter;
using Hymson.MES.CoreServices.Dtos.Parameter;
using Hymson.MES.CoreServices.Options;
using Hymson.MES.Data.Repositories.Parameter.ManuProductParameter;
using Hymson.MES.Data.Repositories.Parameter.ManuProductParameter.Command;
using Hymson.MES.Data.Repositories.Parameter.ManuProductParameter.Query;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.Process.Query;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;
using Microsoft.Extensions.Options;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;

namespace Hymson.MES.CoreServices.Services.Parameter
{
    /// <summary>
    /// 参数采集
    /// </summary>
    public class ManuProductParameterService : IManuProductParameterService
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly ParameterOptions _parameterOptions;

        /// <summary>
        /// 仓储接口（参数维护）
        /// </summary>
        private readonly IProcParameterRepository _procParameterRepository;

        /// <summary>
        /// 仓储接口（工序维护）
        /// </summary>
        private readonly IProcProcedureRepository _procProcedureRepository;

        /// <summary>
        /// 仓储接口（产品过程参数）
        /// </summary>
        private readonly IManuProductParameterRepository _manuProductParameterRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="parameterOptions"></param>
        /// <param name="procParameterRepository"></param>
        /// <param name="procProcedureRepository"></param>
        /// <param name="manuProductParameterRepository"></param>
        public ManuProductParameterService(IOptions<ParameterOptions> parameterOptions,
            IProcParameterRepository procParameterRepository,
            IProcProcedureRepository procProcedureRepository,
            IManuProductParameterRepository manuProductParameterRepository)
        {
            _parameterOptions = parameterOptions.Value;
            _procParameterRepository = procParameterRepository;
            _procProcedureRepository = procProcedureRepository;
            _manuProductParameterRepository = manuProductParameterRepository;
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
            var sql = await _manuProductParameterRepository.ShowCreateTableAsync(ProductParameter.ProductProcedureParameterTemplateName);
            sql = sql?.Replace(ProductParameter.ProductProcedureParameterTemplateName, tabname);
            sql = sql?.Replace($"CREATE TABLE", $"CREATE TABLE  IF NOT EXISTS");
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

        /// <summary>
        /// 准备工序维度创建数据库表sql语句
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="procedureCode"></param>
        /// <returns></returns>
        public string PrepareProductParameterProcedureCodeTableSql(long siteId, string procedureCode)
        {
            //获取目标表名
            var destinationTableName = GetTableNameByProcedureCode(siteId, procedureCode);
            string createTableSql = $"CREATE TABLE `{destinationTableName}` LIKE `{ProductParameter.ProductProcedureParameterTemplateName}`;";
            return createTableSql;
        }


        // 2023.11.06 add
        /// <summary>
        /// 参数采集
        /// </summary>
        /// <param name="bo"></param>
        /// <returns></returns>
        public async Task ProductParameterCollectAsync(ProductProcessParameterBo bo)
        {
            var parameterEntities = await _procParameterRepository.GetByCodesAsync(new ProcParametersByCodeQuery
            {
                SiteId = bo.SiteId,
                Codes = bo.Parameters.Select(x => x.ParameterCode)
            });

            List<ParameterBo> list = new();
            var errorParameter = new List<string>();
            foreach (var parameter in bo.Parameters)
            {
                var parameterEntity = parameterEntities.FirstOrDefault(x => x.ParameterCode == parameter.ParameterCode);
                if (parameterEntity == null)
                {
                    errorParameter.Add(parameter.ParameterCode);
                    continue;
                }

                list.Add(new ParameterBo
                {
                    SiteId = bo.SiteId,
                    UserName = bo.UserName,
                    Time = bo.Time,
                    SFC = bo.SFC,
                    ProcedureId = bo.ProcedureId,
                    ParameterId = parameterEntity.Id,
                    ParameterValue = parameter.ParameterValue,
                    CollectionTime = parameter.CollectionTime
                });
            }

            if (errorParameter.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES19601))
                    .WithData("ParameterCodes", string.Join(",", errorParameter));
            }

            await SaveAsync(list);
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="bos"></param>
        /// <returns></returns>
        public async Task SaveAsync(IEnumerable<ParameterBo> bos)
        {
            var dic = new Dictionary<string, List<ManuProductParameterEntity>>();

            // 查询工序集合
            var procProcedureEntites = await _procProcedureRepository.GetByIdsAsync(bos.Select(x => x.ProcedureId));
            foreach (var bo in bos)
            {
                var procProcedureEntity = procProcedureEntites.FirstOrDefault(x => x.Id == bo.ProcedureId)
                    ?? throw new CustomerValidationException(nameof(ErrorCode.MES10476));

                var entity = new ManuProductParameterEntity
                {
                    ProcedureId = procProcedureEntity.Id,
                    SFC = bo.SFC,
                    ParameterId = bo.ParameterId,
                    ParameterValue = bo.ParameterValue,
                    CollectionTime = bo.Time,
                    SiteId = bo.SiteId,
                    CreatedBy = bo.UserName,
                    UpdatedBy = bo.UserName,
                    CreatedOn = bo.Time,
                    UpdatedOn = bo.Time,
                    Id = IdGenProvider.Instance.CreateId()
                };

                // 生成表名（通过条码）
                var tableNameBySFC = GetTableNameBySFC(bo.SiteId, bo.SFC);
                if (!dic.ContainsKey(tableNameBySFC)) dic[tableNameBySFC] = new();
                dic[tableNameBySFC].Add(entity);

                // 生成表名（通过工序）
                var tableNameByProcedureCode = GetTableNameByProcedureCode(bo.SiteId, procProcedureEntity.Code);
                if (!dic.ContainsKey(tableNameByProcedureCode)) dic[tableNameByProcedureCode] = new();
                dic[tableNameByProcedureCode].Add(entity);
            }

            using var trans = TransactionHelper.GetTransactionScope();
            await Task.WhenAll(dic.Select(s => _manuProductParameterRepository.InsertRangeAsync(s.Value, s.Key)));
            trans.Complete();
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
            using SHA256 hasher = SHA256.Create();
            byte[] inputBytes = Encoding.UTF8.GetBytes(input);
            byte[] hashBytes = hasher.ComputeHash(inputBytes);

            return new(hashBytes);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static uint CalculateCrc32(string input)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(input);
            return Crc32Algorithm.Compute(bytes);
        }
        #endregion
    }
}
