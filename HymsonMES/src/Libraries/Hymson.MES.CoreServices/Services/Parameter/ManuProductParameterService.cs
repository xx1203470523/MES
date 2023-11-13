using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Parameter;
using Hymson.MES.CoreServices.Bos.Manufacture;
using Hymson.MES.CoreServices.Bos.Parameter;
using Hymson.MES.CoreServices.Dtos.Parameter;
using Hymson.MES.CoreServices.Options;
using Hymson.MES.CoreServices.Services.Common.ManuExtension;
using Hymson.MES.Data.Repositories.Parameter.ManuProductParameter;
using Hymson.MES.Data.Repositories.Parameter.ManuProductParameter.Query;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.Process.Query;
using Hymson.Snowflake;
using Hymson.Utils.Tools;
using Microsoft.Extensions.Options;

namespace Hymson.MES.CoreServices.Services.Parameter
{
    /// <summary>
    /// 参数采集
    /// </summary>
    public class ManuProductParameterService : IManuProductParameterService
    {
        /// <summary>
        /// 产品参数
        /// </summary>
        private readonly IManuProductParameterRepository _manuProductParameterRepository;

        /// <summary>
        /// 仓储接口（参数维护）
        /// </summary>
        private readonly IProcParameterRepository _procParameterRepository;

        /// <summary>
        /// 仓储接口（工序维护）
        /// </summary>
        private readonly IProcProcedureRepository _procProcedureRepository;

        /// <summary>
        /// 仓储接口（全检参数项目表）
        /// </summary>
        private readonly IProcProductParameterGroupDetailRepository _procProductParameterGroupDetailRepository;

        /// <summary>
        /// 仓储接口（产品过程参数）
        /// </summary>
        /// <param name="manuProductParameterRepository"></param>
        /// <param name="parameterOptions"></param>
        /// <param name="procProcedureRepository"></param>
        /// <param name="procProductParameterGroupDetailRepository"></param>
        /// <param name="manuProductParameterRepository"></param>
        public ManuProductParameterService(IOptions<ParameterOptions> parameterOptions,
            IProcParameterRepository procParameterRepository,
            IProcProcedureRepository procProcedureRepository,
            IProcProductParameterGroupDetailRepository procProductParameterGroupDetailRepository,
            IManuProductParameterRepository manuProductParameterRepository)
        {
            _manuProductParameterRepository = manuProductParameterRepository;
            _procParameterRepository = procParameterRepository;
            _procProcedureRepository = procProcedureRepository;
            _procProductParameterGroupDetailRepository = procProductParameterGroupDetailRepository;
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
            return await _manuProductParameterRepository.GetProductParameterByProcedureIdEntities(new ManuProductParameterByProcedureIdQuery
            {
                ProcedureId = param.ProcedureId,
                SiteId = param.SiteId,
                SFCs = param.SFCs,
            });
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
        /// 根据产品参数ID获取关联明细
        /// </summary>
        /// <param name="productParameterGroupId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcProductParameterGroupDetailBo>> GetDetailsByProductParameterGroupIdAsync(long productParameterGroupId)
        {
            List<ProcProductParameterGroupDetailBo> bos = new();

            var productParameterGroupDetailEntities = await _procProductParameterGroupDetailRepository.GetEntitiesAsync(new ProcProductParameterGroupDetailQuery
            {
                ParameterGroupId = productParameterGroupId
            });
            if (productParameterGroupDetailEntities == null || productParameterGroupDetailEntities.Any() == false) return bos;

            // 查询已经缓存的参数实体
            var parameterEntities = await _procParameterRepository.GetProcParameterEntitiesAsync(new ProcParameterQuery
            {
                SiteId = productParameterGroupDetailEntities.FirstOrDefault()?.SiteId ?? 0
            });

            foreach (var item in productParameterGroupDetailEntities)
            {
                var bo = item.ToBo<ProcProductParameterGroupDetailBo>();
                var parameterEntity = parameterEntities.FirstOrDefault(f => f.Id == item.ParameterId);
                if (bo == null) continue;

                if (parameterEntity != null)
                {
                    bo.Code = parameterEntity.ParameterCode;
                    bo.Name = parameterEntity.ParameterName;
                    bo.Unit = parameterEntity.ParameterUnit;
                    bo.DataType = parameterEntity.DataType;
                }

                bos.Add(bo);
            }

            return bos;
        }

        /// <summary>
        /// 参数采集
        /// </summary>
        /// <param name="bo"></param>
        /// <returns></returns>
        public async Task<int> ProductParameterCollectAsync(ProductProcessParameterBo bo)
        {
            var parameterEntities = await _procParameterRepository.GetByCodesAsync(new ProcParametersByCodeQuery
            {
                SiteId = bo.SiteId,
                Codes = bo.Parameters.Select(x => x.ParameterCode)
            });

            List<ManuProductParameterEntity> list = new();
            var errorParameter = new List<string>();
            foreach (var parameter in bo.Parameters)
            {
                var parameterEntity = parameterEntities.FirstOrDefault(x => x.ParameterCode == parameter.ParameterCode);
                if (parameterEntity == null)
                {
                    errorParameter.Add(parameter.ParameterCode);
                    continue;
                }
                var entity = new ManuProductParameterEntity
                {
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

            return await SaveAsync(new ManufactureBo
            {
                SiteId = bo.SiteId,
                UserName = bo.UserName,
                Time = bo.Time,
                ProcedureId = bo.ProcedureId,
                ResourceId = bo.ResourceId,
                SFC = bo.SFC
            }, list);
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="manufactureBo"></param>
        /// <param name="bos"></param>
        /// <returns></returns>
        public async Task<int> SaveAsync(ManufactureBo manufactureBo, IEnumerable<ParameterBo> bos)
        {
            var dic = new Dictionary<string, List<ManuProductParameterEntity>>();

            // 查询工序
            var procProcedureEntity = await _procProcedureRepository.GetByIdAsync(manufactureBo.ProcedureId)
                ?? throw new CustomerValidationException(nameof(ErrorCode.MES10476));

            foreach (var bo in bos)
            {
                var entity = bo.ToEntity<ManuProductParameterEntity>();
                if (entity == null) continue;

                entity.SiteId = manufactureBo.SiteId;
                entity.CreatedBy = manufactureBo.UserName;
                entity.UpdatedBy = manufactureBo.UserName;
                entity.CreatedOn = manufactureBo.Time;
                entity.UpdatedOn = manufactureBo.Time;
                entity.Id = IdGenProvider.Instance.CreateId();
                entity.ProcedureId = manufactureBo.ProcedureId;
                entity.SFC = manufactureBo.SFC;

                // 生成表名（通过条码）
                var tableNameBySFC = GetTableNameBySFC(manufactureBo.SiteId, manufactureBo.SFC);
                if (!dic.ContainsKey(tableNameBySFC)) dic[tableNameBySFC] = new();
                dic[tableNameBySFC].Add(entity);

                // 生成表名（通过工序）
                var tableNameByProcedureCode = GetTableNameByProcedureCode(manufactureBo.SiteId, procProcedureEntity.Code);
                if (!dic.ContainsKey(tableNameByProcedureCode)) dic[tableNameByProcedureCode] = new();
                dic[tableNameByProcedureCode].Add(entity);
            }
            using var trans = TransactionHelper.GetTransactionScope();
            var row = await _manuProductParameterRepository.InsertRangeAsync(list);
            trans.Complete();
            return row;
        }
    }
}
