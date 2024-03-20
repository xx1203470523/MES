using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Integrated;
using Hymson.MES.Core.Enums.Integrated;
using Hymson.MES.Core.Enums;
using Hymson.MES.CoreServices.Dtos.Qkny;
using Hymson.MES.Data.Repositories.Integrated;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.Snowflake;
using Hymson.Utils.Tools;
using Hymson.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Hymson.MES.Data.Repositories.Integrated.InteVehicleFreight.Command;
using Hymson.MES.EquipmentServices.Dtos.Qkny.Manufacture;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.CoreServices.Services.Common;
using Hymson.MES.Data.Repositories.Quality;
using Microsoft.IdentityModel.Tokens;
using Google.Protobuf.WellKnownTypes;

namespace Hymson.MES.EquipmentServices.Services.Qkny.InteVehicle
{
    /// <summary>
    /// 载具
    /// </summary>
    public class InteVehicleService : IInteVehicleService
    {
        /// <summary>
        /// 载具注册表 仓储
        /// </summary>
        private readonly IInteVehicleRepository _inteVehicleRepository;

        /// <summary>
        /// 在制品
        /// </summary>
        private readonly IManuSfcProduceRepository _manuSfcProduceRepository;

        /// <summary>
        /// 条码绑定明细
        /// </summary>
        private readonly IInteVehiceFreightStackRepository _inteVehiceFreightStackRepository;

        /// <summary>
        /// 载具类型
        /// </summary>
        private readonly IInteVehicleTypeRepository _inteVehicleTypeRepository;

        /// <summary>
        /// 载具装载明细
        /// </summary>
        private readonly IInteVehicleFreightRepository _inteVehicleFreightRepository;

        /// <summary>
        /// 装载记录
        /// </summary>
        private readonly IInteVehicleFreightRecordRepository _inteVehicleFreightRecordRepository;

        /// <summary>
        /// 仓储接口（产品不良录入）
        /// </summary>
        private readonly IManuProductBadRecordRepository _manuProductBadRecordRepository;

        /// <summary>
        /// 服务接口（主数据）
        /// </summary>
        private readonly IMasterDataService _masterDataService;

        /// <summary>
        /// 构造函数
        /// </summary>
        public InteVehicleService(IInteVehicleRepository inteVehicleRepository,
            IManuSfcProduceRepository manuSfcProduceRepository,
            IInteVehiceFreightStackRepository inteVehiceFreightStackRepository,
            IInteVehicleTypeRepository inteVehicleTypeRepository,
            IInteVehicleFreightRepository inteVehicleFreightRepository,
            IInteVehicleFreightRecordRepository inteVehicleFreightRecordRepository,
            IManuProductBadRecordRepository manuProductBadRecordRepository,
            IMasterDataService masterDataService)
        {
            _inteVehicleRepository = inteVehicleRepository;
            _inteVehicleRepository = inteVehicleRepository;
            _manuSfcProduceRepository = manuSfcProduceRepository;
            _inteVehiceFreightStackRepository = inteVehiceFreightStackRepository;
            _inteVehicleTypeRepository = inteVehicleTypeRepository;
            _inteVehicleFreightRepository = inteVehicleFreightRepository;
            _inteVehicleFreightRecordRepository = inteVehicleFreightRecordRepository;
            _manuProductBadRecordRepository = manuProductBadRecordRepository;
            _masterDataService = masterDataService;
        }

        /// <summary>
        /// 根据Code获取数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<InteVehicleEntity> GetByCodeAsync(InteVehicleCodeQuery query)
        {
            var dbModel = await _inteVehicleRepository.GetByCodeAsync(query);
            if(dbModel == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES45110));
            }
            return dbModel!;
        }

        /// <summary>
        /// 载具和电芯绑定
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task VehicleBindOperationAsync(InteVehicleBindDto param)
        {
            List<string> sfcList = param.SfcList.Select(m => m.Sfc).ToList();
            ManuSfcProduceBySfcsQuery sfcQuery = new ManuSfcProduceBySfcsQuery();
            sfcQuery.SiteId = param.SiteId;
            sfcQuery.Sfcs = sfcList;
            //查数据库中在制品条码
            var sfcProductList = (await _manuSfcProduceRepository.GetListBySfcsAsync(sfcQuery)).ToList(); ;
            if (sfcProductList == null || sfcProductList.Count == 0)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES45112));
            }
            //数量校验
            var dbSfcList = sfcProductList.Select(m => m.SFC).ToList();
            if (dbSfcList.Count != param.SfcList.Count)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES45113));
            }
            //条码差异校验
            var exceptList = dbSfcList.Except(sfcList).ToList();
            if (exceptList != null && exceptList.Count > 0)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES45114));
            }

            //校验条码是否已绑定
            //绑盘前校验 该条码是否已绑盘
            InteVehiceSfcListQuery bindSfcQuery = new InteVehiceSfcListQuery();
            bindSfcQuery.SiteId = param.SiteId;
            bindSfcQuery.SfcList = sfcList;
            var dbBindSfc = (await _inteVehiceFreightStackRepository.GetBySfcListAsync(bindSfcQuery)).ToList();
            if (dbBindSfc != null && dbBindSfc.Count > 0)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES45115));
            }

            //托盘数量校验
            InteVehicleCodeQuery vehicleQuery = new InteVehicleCodeQuery();
            vehicleQuery.SiteId = param.SiteId;
            vehicleQuery.Code = param.ContainerCode;
            var inteVehicleEntity = await _inteVehicleRepository.GetByCodeAsync(vehicleQuery);
            if (inteVehicleEntity == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES45112));
            }
            if (inteVehicleEntity.VehicleTypeId == 0)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES45116));
            }
            var vehicleTypeEntity = await _inteVehicleTypeRepository.GetByIdAsync(inteVehicleEntity.VehicleTypeId);
            if (vehicleTypeEntity == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES45117));
            }
            if (vehicleTypeEntity.Status == DisableOrEnableEnum.Disable)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES45118));
            }
            //获取托盘实际能用的数量
            var vehicleDetail = await _inteVehicleFreightRepository.GetByVehicleIdsAsync(new long[] { inteVehicleEntity.Id });
            int okMaxNum = vehicleDetail.Where(i => i.Status == true).ToList().Count;
            //获取托盘已经绑定的记录
            InteVehiceFreightStackQuery vehicelStackQuery = new InteVehiceFreightStackQuery();
            vehicelStackQuery.SiteId = param.SiteId;
            vehicelStackQuery.VehicleId = inteVehicleEntity.Id;
            var vehiceStatckList = (await _inteVehiceFreightStackRepository.GetInteVehiceFreightStackEntitiesAsync(vehicelStackQuery)).ToList();
            //校验数量
            if (vehiceStatckList != null && vehiceStatckList.Count > 0)
            {
                //已经绑定的数量+当前数量
                var curNum = vehiceStatckList.Count * vehicleTypeEntity.CellQty + sfcList.Count;
                if (curNum > okMaxNum)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES45119));
                }
            }

            DateTime curDate = HymsonClock.Now();
            //插入记录
            List<InteVehicleFreightStackEntity> addList = new List<InteVehicleFreightStackEntity>();
            //List<InteVehicleFreightEntity> updateQtyList = new List<InteVehicleFreightEntity>();
            List<InteVehicleFreightRecordEntity> recordList = new List<InteVehicleFreightRecordEntity>();
            foreach (var item in param.SfcList)
            {
                InteVehicleFreightStackEntity addModel = new InteVehicleFreightStackEntity();
                addModel.Id = IdGenProvider.Instance.CreateId();
                addModel.BarCode = item.Sfc;
                addModel.LocationId = 0;
                addModel.SiteId = param.SiteId;
                addModel.CreatedBy = param.UserName;
                addModel.UpdatedBy = param.UserName;
                addModel.CreatedOn = curDate;
                addModel.UpdatedOn = curDate;
                addModel.VehicleId = inteVehicleEntity.Id;
                addList.Add(addModel);

                //InteVehicleFreightEntity updateModel = new InteVehicleFreightEntity();
                //updateModel.VehicleId = inteVehicleEntity.Id;
                //updateModel.Qty = vehicleTypeEntity.CellQty;
                //updateModel.UpdatedBy = param.UserName;
                //updateModel.UpdatedOn = curDate;
                //updateModel.Location = item.Location;
                //updateQtyList.Add(updateModel);

                InteVehicleFreightRecordEntity recordEntity = new InteVehicleFreightRecordEntity();
                recordEntity.Id = IdGenProvider.Instance.CreateId();
                recordEntity.BarCode = item.Sfc;
                recordEntity.LocationId = 0;
                recordEntity.SiteId = param.SiteId;
                recordEntity.CreatedBy = param.UserName;
                recordEntity.UpdatedBy = param.UserName;
                recordEntity.CreatedOn = curDate;
                recordEntity.UpdatedOn = curDate;
                recordEntity.VehicleId = inteVehicleEntity.Id;
                recordEntity.OperateType = (int)VehicleOperationEnum.Bind;
                recordList.Add(recordEntity);
            }

            //数据库操作
            using var trans = TransactionHelper.GetTransactionScope(TransactionScopeOption.Required, IsolationLevel.ReadCommitted);
            //await _inteVehicleFreightRepository.UpdateQtyByLocationAsync(updateQtyList);
            await _inteVehiceFreightStackRepository.InsertsAsync(addList);
            await _inteVehicleFreightRecordRepository.InsertsAsync(recordList);
            trans.Complete();
        }

        /// <summary>
        /// 载具解绑
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<int> VehicleUnBindOperationAsync(InteVehicleUnBindDto param)
        {
            //获取托盘
            InteVehicleCodeQuery vehicleQuery = new InteVehicleCodeQuery();
            vehicleQuery.SiteId = param.SiteId;
            vehicleQuery.Code = param.ContainerCode;
            var inteVehicleEntity = await _inteVehicleRepository.GetByCodeAsync(vehicleQuery);
            if (inteVehicleEntity == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES45112));
            }
            //组装数据
            DateTime curDate = HymsonClock.Now();
            List<UpdateVehicleFreightStackCommand> updateList = new List<UpdateVehicleFreightStackCommand>();
            List<InteVehicleFreightRecordEntity> recordList = new List<InteVehicleFreightRecordEntity>();
            foreach (var item in param.SfcList)
            {
                UpdateVehicleFreightStackCommand updateModel = new UpdateVehicleFreightStackCommand();
                updateModel.BarCode = item;
                updateModel.VehicleId = inteVehicleEntity.Id;
                updateModel.UpdatedOn = curDate;
                updateModel.UpdatedBy = param.UserName;
                updateList.Add(updateModel);

                InteVehicleFreightRecordEntity recordEntity = new InteVehicleFreightRecordEntity();
                recordEntity.Id = IdGenProvider.Instance.CreateId();
                recordEntity.BarCode = item;
                recordEntity.LocationId = 0;
                recordEntity.SiteId = param.SiteId;
                recordEntity.CreatedBy = param.UserName;
                recordEntity.UpdatedBy = param.UserName;
                recordEntity.CreatedOn = curDate;
                recordEntity.UpdatedOn = curDate;
                recordEntity.VehicleId = inteVehicleEntity.Id;
                recordEntity.OperateType = (int)VehicleOperationEnum.Unbind;
                recordList.Add(recordEntity);
            }
            int delNum = 0;
            //数据库操作
            using var trans = TransactionHelper.GetTransactionScope(TransactionScopeOption.Required, IsolationLevel.ReadCommitted);
            delNum = await _inteVehiceFreightStackRepository.DeleteByVehiceBarCode(updateList);
            if(delNum != updateList.Count)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES45120));
            }
            await _inteVehicleFreightRecordRepository.InsertsAsync(recordList);
            trans.Complete();

            return delNum;
        }

        /// <summary>
        /// 托盘NG电芯上报
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task ContainerNgReportAsync(InteVehicleNgSfcDto param)
        {
            //获取托盘
            InteVehicleCodeQuery vehicleQuery = new InteVehicleCodeQuery();
            vehicleQuery.SiteId = param.SiteId;
            vehicleQuery.Code = param.ContainerCode;
            var inteVehicleEntity = await _inteVehicleRepository.GetByCodeAsync(vehicleQuery);
            if (inteVehicleEntity == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES45112));
            }

            List<string> ngCodeList = param.NgSfcList.Select(m => m.NgCode).Distinct().ToList();
            QualUnqualifiedCodeByCodesQuery ngCodeQuery = new QualUnqualifiedCodeByCodesQuery();
            ngCodeQuery.SiteId = param.SiteId;
            ngCodeQuery.Codes = ngCodeList;
            var ngEntityList = await _masterDataService.GetUnqualifiedEntitiesByCodesAsync(ngCodeQuery);
            if(ngEntityList.IsNullOrEmpty() == true)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES45121));
            }
            if(ngEntityList.Count() != ngCodeList.Count())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES45121));
            }

            //数据组装
            DateTime curDate = HymsonClock.Now();
            List<UpdateVehicleFreightStackCommand> updateList = new List<UpdateVehicleFreightStackCommand>();
            List<InteVehicleFreightRecordEntity> recordList = new List<InteVehicleFreightRecordEntity>();
            List<ManuProductBadRecordEntity> badList = new List<ManuProductBadRecordEntity>();
            foreach (var item in param.NgSfcList)
            {
                UpdateVehicleFreightStackCommand updateModel = new UpdateVehicleFreightStackCommand();
                updateModel.BarCode = item.Sfc;
                updateModel.VehicleId = inteVehicleEntity.Id;
                updateModel.UpdatedOn = curDate;
                updateModel.UpdatedBy = param.UserName;
                updateList.Add(updateModel);

                InteVehicleFreightRecordEntity recordEntity = new InteVehicleFreightRecordEntity();
                recordEntity.Id = IdGenProvider.Instance.CreateId();
                recordEntity.BarCode = item.Sfc;
                recordEntity.LocationId = 0;
                recordEntity.SiteId = param.SiteId;
                recordEntity.CreatedBy = param.UserName;
                recordEntity.UpdatedBy = param.UserName;
                recordEntity.CreatedOn = curDate;
                recordEntity.UpdatedOn = curDate;
                recordEntity.VehicleId = inteVehicleEntity.Id;
                recordEntity.OperateType = (int)VehicleOperationEnum.NgUnbind;
                recordList.Add(recordEntity);

                ManuProductBadRecordEntity badModel = new ManuProductBadRecordEntity();
                badModel.SiteId = param.SiteId;
                badModel.CreatedBy = param.UserName;
                badModel.CreatedOn = curDate;
                badModel.Id = IdGenProvider.Instance.CreateId();
                badModel.FoundBadOperationId = param.OperationId;
                badModel.FoundBadResourceId = param.ResourceId;
                badModel.Status = Core.Enums.Manufacture.ProductBadRecordStatusEnum.Open;
                badModel.Source = Core.Enums.Manufacture.ProductBadRecordSourceEnum.EquipmentReBad;
                badModel.Qty = 1;
            }

            //数据库操作
            using var trans = TransactionHelper.GetTransactionScope(TransactionScopeOption.Required, IsolationLevel.ReadCommitted);
            await _inteVehiceFreightStackRepository.DeleteByVehiceBarCode(updateList);
            await _inteVehicleFreightRecordRepository.InsertsAsync(recordList);
            await _manuProductBadRecordRepository.InsertRangeAsync(badList);
            trans.Complete();
        }

    }
}
