using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Parameter;
using Hymson.MES.CoreServices.Bos.Parameter;
using Hymson.MES.CoreServices.Dtos.Parameter;
using Hymson.MES.CoreServices.Services.Common;
using Hymson.MES.Data.Repositories.Parameter;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.Process.Query;
using Hymson.Snowflake;
using Hymson.Utils.Tools;
using System.Transactions;

namespace Hymson.MES.CoreServices.Services.Parameter
{
    /// <summary>
    /// 参数采集
    /// </summary>
    public class ManuProductParameterService : IManuProductParameterService
    {
        /// <summary>
        /// 服务接口（生产通用）
        /// </summary>
        private readonly IManuCommonService _manuCommonService;

        /// <summary>
        /// 产品参数
        /// </summary>
        private readonly IManuProductParameterRepository _manuProductParameterRepository;

        /// <summary>
        /// 仓储接口（参数维护）
        /// </summary>
        private readonly IProcParameterRepository _procParameterRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="manuCommonService"></param>
        /// <param name="manuProductParameterRepository"></param>
        /// <param name="procParameterRepository"></param>
        /// <param name="procProcedureRepository"></param>
        public ManuProductParameterService(IManuCommonService manuCommonService,
            IManuProductParameterRepository manuProductParameterRepository,
            IProcParameterRepository procParameterRepository,
            IProcProcedureRepository procProcedureRepository)
        {
            _manuCommonService = manuCommonService;
            _manuProductParameterRepository = manuProductParameterRepository;
            _procParameterRepository = procParameterRepository;
        }


        /// <summary>
        /// 根据工序参数信息
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuProductParameterEntity>?> GetProductParameterListByProcedureAsync(QueryParameterByProcedureDto param)
        {
            return await _manuProductParameterRepository.GetProductParameterByProcedureIdEntitiesAsync(new ManuProductParameterByProcedureIdQuery
            {
                ProcedureId = param.ProcedureId,
                SiteId = param.SiteId,
                SFCs = param.SFCs,
            });
        }

        /// <summary>
        /// 参数采集（产品过程参数）
        /// </summary>
        /// <param name="bo"></param>
        /// <returns></returns>
        public async Task<int> ProductProcessCollectAsync(ProductProcessParameterBo bo)
        {
            if (bo == null || !bo.Parameters.Any())
            {
                return 0;
            }
            var parameterEntities = await _procParameterRepository.GetByCodesAsync(new ProcParametersByCodeQuery
            {
                SiteId = bo.SiteId,
                Codes = bo.Parameters.Select(x => x.ParameterCode).Distinct()
            });

            List<ManuProductParameterEntity> list = new();
            var errorParameter = new List<string>();

            foreach (var parameter in bo.Parameters)
            {
                var parameterEntity = parameterEntities.FirstOrDefault(x => parameter.ParameterCode.Equals(x.ParameterCode, StringComparison.OrdinalIgnoreCase));
                //if (parameterEntity == null)
                //{
                //    errorParameter.Add(parameter.ParameterCode);
                //    continue;
                //}

                list.AddRange(bo.SFCs.Select(SFC => new ManuProductParameterEntity
                {
                    ProcedureId = bo.ProcedureId,
                    SFC = SFC,
                    ParameterId = parameterEntity?.Id ?? 0,
                    ParameterValue = parameter.ParameterValue,
                    CollectionTime = parameter.CollectionTime,
                    SiteId = bo.SiteId,
                    CreatedBy = bo.UserName,
                    UpdatedBy = bo.UserName,
                    CreatedOn = bo.Time,
                    UpdatedOn = bo.Time,
                    Id = IdGenProvider.Instance.CreateId()
                }));
            }

            if (errorParameter.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES19606))
                    .WithData("ParameterCodes", string.Join(",", errorParameter));
            }

            using var trans = TransactionHelper.GetTransactionScope(TransactionScopeOption.RequiresNew);
            var row = await _manuProductParameterRepository.InsertRangeAsync(list);
            trans.Complete();
            return row;
        }

        /// <summary>
        /// 参数采集（产品过程参数）
        /// </summary>
        /// <param name="bo"></param>
        /// <returns></returns>
        public async Task<int> ProductProcessCollectAsync(ProductParameterCollectBo bo)
        {
            if (bo == null || !bo.SFCList.Any(x => x.Parameters.Any()))
            {
                return 0;
            }
            //校验参数是否存在
            var parameterCodes = bo.SFCList.SelectMany(x => x.Parameters).Select(x => x.ParameterCode.ToUpper()).Distinct();
            var parameterEntities = await _procParameterRepository.GetByCodesAsync(new ProcParametersByCodeQuery
            {
                SiteId = bo.SiteId,
                Codes = parameterCodes
            });
            //if (parameterEntities == null || !parameterEntities.Any())
            //{
            //    throw new CustomerValidationException(nameof(ErrorCode.MES19606)).WithData("ParameterCodes", string.Join(",", parameterCodes));
            //}
            //var noExistCodes = parameterCodes.Except(parameterEntities.Select(x => x.ParameterCode.ToUpper()));
            //if (noExistCodes != null && noExistCodes.Any())
            //{
            //    throw new CustomerValidationException(nameof(ErrorCode.MES19606)).WithData("ParameterCodes", string.Join(",", noExistCodes));
            //}

            List<ManuProductParameterEntity> list = new();

            foreach (var item in bo.SFCList)
            {
                list.AddRange(item.Parameters.Select(parameter => new ManuProductParameterEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SiteId = bo.SiteId,
                    ProcedureId = bo.ProcedureId,
                    SFC = item.SFC,
                    ParameterId = parameterEntities.FirstOrDefault(x => parameter.ParameterCode.Equals(x.ParameterCode, StringComparison.OrdinalIgnoreCase))?.Id ?? 0,
                    ParameterValue = parameter.ParameterValue,
                    CollectionTime = parameter.CollectionTime,
                    CreatedBy = bo.UserName,
                    UpdatedBy = bo.UserName,
                    CreatedOn = bo.Time,
                    UpdatedOn = bo.Time
                }));
            }
            if(list == null || list.Count == 0)
            {
                return 0;
            }

            using var trans = TransactionHelper.GetTransactionScope(TransactionScopeOption.RequiresNew);
            var row = await _manuProductParameterRepository.InsertRangeAsync(list);
            trans.Complete();
            return row;
        }

        /// <summary>
        /// 根据条码获取参数信息
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuProductParameterEntity>> GetProductParameterBySFCListAsync(QueryParameterByProcedureDto param)
        {
            return await _manuProductParameterRepository.GetProductParameterBySFCEntitiesAsync(new ManuProductParameterBySfcQuery
            {
                SiteId = param.SiteId,
                SFCs = param.SFCs
            });
        }

    }
}
