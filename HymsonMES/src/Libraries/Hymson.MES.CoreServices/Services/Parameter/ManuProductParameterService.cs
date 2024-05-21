using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Domain.Parameter;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.CoreServices.Bos.Parameter;
using Hymson.MES.CoreServices.Dtos.Parameter;
using Hymson.MES.CoreServices.Services.Common;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Parameter;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.Process.Query;
using Hymson.Snowflake;
using Hymson.Utils.Tools;
using Org.BouncyCastle.Asn1.Ocsp;

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
        /// 在制品
        /// </summary>
        private readonly IManuSfcProduceRepository _manuSfcProduceRepository;

        private readonly IManuSfcStepRepository _manuSfcStepRepository;

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
            IProcProcedureRepository procProcedureRepository, IManuSfcProduceRepository manuSfcProduceRepository, IManuSfcStepRepository manuSfcStepRepository)
        {
            _manuCommonService = manuCommonService;
            _manuProductParameterRepository = manuProductParameterRepository;
            _procParameterRepository = procParameterRepository;
            _manuSfcProduceRepository = manuSfcProduceRepository;
            _manuSfcStepRepository = manuSfcStepRepository;
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
            //参数收集为在制品数据
            var manuSfcProduceEntities = await _manuSfcProduceRepository.GetListBySfcsAsync(new ManuSfcProduceBySfcsQuery
            {
                SiteId = bo.SiteId,
                Sfcs = bo.SFCs,
            });

            //TODO  BO结构很奇怪  在不破坏原有逻辑上修改  bywangkeming
            var manuSfcStepEntities = new List<ManuSfcStepEntity>();
            foreach (var item in manuSfcProduceEntities)
            {
                manuSfcStepEntities.Add(new ManuSfcStepEntity
                {
                    Operatetype = ManuSfcStepTypeEnum.ParameterCollect,
                    CurrentStatus = item.Status,
                    AfterOperationStatus = item.Status,
                    Id = IdGenProvider.Instance.CreateId(),
                    SFC = item.SFC,
                    ProductId = item.ProductId,
                    WorkOrderId = item.WorkOrderId,
                    WorkCenterId = item.WorkCenterId,
                    ProductBOMId = item.ProductBOMId,
                    ProcessRouteId = item.ProcessRouteId,
                    SFCInfoId = item.BarCodeInfoId,
                    Qty = item.Qty,
                    //VehicleCode = x.VehicleCode,
                    ProcedureId = item.ProcedureId,
                    ResourceId = item.ResourceId,
                    EquipmentId = item.EquipmentId,
                    OperationProcedureId = bo.ProcedureId,
                    OperationResourceId = bo.ResourceId,
                    //OperationEquipmentId = bo.EquipmentId,
                    SiteId = bo.SiteId,
                    CreatedBy = bo.UserName,
                    CreatedOn = bo.Time,
                    UpdatedBy = bo.UserName,
                    UpdatedOn = bo.Time
                });
            }

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

                list.AddRange(bo.SFCs.Select(SFC => new ManuProductParameterEntity
                {
                    ProcedureId = bo.ProcedureId,
                    SFC = SFC,
                    SfcstepId = manuSfcStepEntities.FirstOrDefault(x => x.SFC == SFC)?.Id,
                    ParameterId = parameterEntity.Id,
                    ParameterValue = parameter.ParameterValue,
                    CollectionTime = bo.Time,
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
                throw new CustomerValidationException(nameof(ErrorCode.MES19601))
                    .WithData("ParameterCodes", string.Join(",", errorParameter));
            }
            await _manuSfcStepRepository.InsertRangeAsync(manuSfcStepEntities);
            using var trans = TransactionHelper.GetTransactionScope();
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
