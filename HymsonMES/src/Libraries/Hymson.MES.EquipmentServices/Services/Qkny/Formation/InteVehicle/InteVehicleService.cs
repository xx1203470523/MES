using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Integrated;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Integrated;
using Hymson.MES.CoreServices.Dtos.Qkny;
using Hymson.MES.CoreServices.Services.Common;
using Hymson.MES.Data.Repositories.Integrated;
using Hymson.MES.Data.Repositories.Integrated.InteVehicleFreight.Command;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Quality;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;
using Microsoft.IdentityModel.Tokens;
using System.Transactions;

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
        /// 仓储接口（产品NG记录表）
        /// </summary>
        private readonly IManuProductNgRecordRepository _manuProductNgRecordRepository;

        /// <summary>
        /// 条码信息表
        /// </summary>
        private readonly IManuSfcInfoRepository _manuSfcInfoRepository;

        /// <summary>
        /// 条码表仓储
        /// </summary>
        private readonly IManuSfcRepository _manuSfcRepository;

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
            IMasterDataService masterDataService,
            IManuProductNgRecordRepository manuProductNgRecordRepository,
            IManuSfcInfoRepository manuSfcInfoRepository,
            IManuSfcRepository manuSfcRepository)
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
            _manuProductNgRecordRepository = manuProductNgRecordRepository;
            _manuSfcInfoRepository = manuSfcInfoRepository;
            _manuSfcRepository = manuSfcRepository;
        }

        /// <summary>
        /// 根据Code获取数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<InteVehicleEntity> GetByCodeAsync(InteVehicleCodeQuery query)
        {
            var dbModel = await _inteVehicleRepository.GetByCodeAsync(query);
            if (dbModel == null)
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

            //校验条码是否存在
            var sfcEntities = await _manuSfcRepository.GetListAsync(new ManuSfcQuery { SiteId = param.SiteId, SFCs = sfcList });
            if (sfcEntities == null || !sfcEntities.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES45112)).WithData("SfcListStr", string.Join(",", param.SfcList));
            }
            var noExistSfcs = sfcList.Except(sfcEntities.Select(x => x.SFC));
            if (noExistSfcs != null && noExistSfcs.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES45112)).WithData("SfcListStr", string.Join(",", noExistSfcs));
            }

            //校验条码是否已绑定
            var dbBindSfc = await _inteVehiceFreightStackRepository.GetBySfcListAsync(new InteVehiceSfcListQuery { SiteId = param.SiteId, SfcList = sfcList });
            if (dbBindSfc != null && dbBindSfc.Any())
            {
                string sfcListStr = string.Join(",", dbBindSfc.Select(m => m.BarCode).ToList());
                throw new CustomerValidationException(nameof(ErrorCode.MES45115)).WithData("SfcList", string.Join(",", sfcListStr));
            }

            //校验载具信息
            var inteVehicleEntity = await _inteVehicleRepository.GetByCodeAsync(new InteVehicleCodeQuery { SiteId = param.SiteId, Code = param.ContainerCode });
            if (inteVehicleEntity == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES45110));
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

            #region 校验载具装载数量

            //获取托盘实际能用的数量
            var vehicleDetail = await _inteVehicleFreightRepository.GetByVehicleIdsAsync(new long[] { inteVehicleEntity.Id });
            int okMaxNum = vehicleDetail.Where(i => i.Status == true).ToList().Count;
            //获取托盘已经绑定的记录
            var vehiceStatckList = await _inteVehiceFreightStackRepository.GetInteVehiceFreightStackEntitiesAsync(new InteVehiceFreightStackQuery
            {
                SiteId = param.SiteId,
                VehicleId = inteVehicleEntity.Id,
            });
            var alreadyBindCount = vehiceStatckList?.Count() ?? 0;  //已绑定数量

            //已经绑定的数量 + 当前数量 > 载具单元格总数 * 单元数量
            if (alreadyBindCount + sfcList.Count > okMaxNum * vehicleTypeEntity.CellQty)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES45119));
            }

            #endregion

            DateTime curDate = HymsonClock.Now();
            //插入记录
            List<InteVehicleFreightStackEntity> addList = new List<InteVehicleFreightStackEntity>();
            //List<InteVehicleFreightEntity> updateQtyList = new List<InteVehicleFreightEntity>();
            List<InteVehicleFreightRecordEntity> recordList = new List<InteVehicleFreightRecordEntity>();
            var paramSfcList = param.SfcList.OrderBy(x => x.Location.ParseToInt());
            var locationIds = vehicleDetail.OrderBy(x => x.Location?.ParseToInt()).Select(x => new { x.Id, x.Location }).ToList();
            if (vehiceStatckList != null && vehiceStatckList.Any())
            {
                locationIds = vehicleDetail.Where(x => !vehiceStatckList.Any(z => z.LocationId == x.Id)).OrderBy(x => x.Location?.ParseToInt())
                    .Select(x => new { x.Id, x.Location }).ToList();
            }
            foreach (var item in paramSfcList)
            {
                //获取位置号Id
                var locationId = locationIds.Where(x => x.Location == item.Location).FirstOrDefault()?.Id ?? 0;
                if (locationId == 0)
                {
                    locationId = locationIds.FirstOrDefault()?.Id ?? 0;
                }
                locationIds.RemoveAll(x => x.Id == locationId);

                InteVehicleFreightStackEntity addModel = new InteVehicleFreightStackEntity();
                addModel.Id = IdGenProvider.Instance.CreateId();
                addModel.BarCode = item.Sfc;
                addModel.LocationId = locationId;
                addModel.SiteId = param.SiteId;
                addModel.CreatedBy = param.UserName;
                addModel.UpdatedBy = param.UserName;
                addModel.CreatedOn = curDate;
                addModel.UpdatedOn = curDate;
                addModel.VehicleId = inteVehicleEntity.Id;
                addList.Add(addModel);

                //InteVehicleFreightEntity updateModel = new InteVehicleFreightEntity();
                //updateModel.VehicleId = inteVehicleEntity.Id;
                //updateModel.Qty = 1;
                //updateModel.UpdatedBy = param.UserName;
                //updateModel.UpdatedOn = curDate;
                //updateModel.Location = item.Location;
                //updateQtyList.Add(updateModel);

                InteVehicleFreightRecordEntity recordEntity = new InteVehicleFreightRecordEntity();
                recordEntity.Id = IdGenProvider.Instance.CreateId();
                recordEntity.BarCode = item.Sfc;
                recordEntity.LocationId = locationId;
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
            var sfcList = param.SfcList.Distinct().ToList();

            //查询条码绑定信息
            var bindEntities = await _inteVehiceFreightStackRepository.GetBySfcListAsync(new InteVehiceSfcListQuery { SiteId = param.SiteId, SfcList = sfcList });
            if (bindEntities == null || !bindEntities.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES45120)).WithData("sfc", string.Join(",", sfcList));
            }
            var noBindSfcs = sfcList.Except(bindEntities.Select(x => x.BarCode));
            if (noBindSfcs != null && noBindSfcs.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES45120)).WithData("sfc", string.Join(",", noBindSfcs));
            }

            //组装数据
            DateTime curDate = HymsonClock.Now();
            var updateList = new List<UpdateVehicleFreightStackCommand>();
            var recordList = new List<InteVehicleFreightRecordEntity>();
            foreach (var item in bindEntities)
            {
                var updateModel = new UpdateVehicleFreightStackCommand
                {
                    BarCode = item.BarCode,
                    VehicleId = item.VehicleId,
                    UpdatedOn = curDate,
                    UpdatedBy = param.UserName
                };
                updateList.Add(updateModel);

                var recordEntity = new InteVehicleFreightRecordEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SiteId = param.SiteId,
                    VehicleId = item.VehicleId,
                    BarCode = item.BarCode,
                    LocationId = item.LocationId,
                    OperateType = (int)VehicleOperationEnum.Unbind,
                    CreatedBy = param.UserName,
                    UpdatedBy = param.UserName,
                    CreatedOn = curDate,
                    UpdatedOn = curDate
                };
                recordList.Add(recordEntity);
            }

            //数据库操作
            using var trans = TransactionHelper.GetTransactionScope();
            var delNum = await _inteVehiceFreightStackRepository.DeleteByVehiceBarCode(updateList);
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
            //不合格代码校验（不合格代码可以为空）
            List<string> ngCodeList = param.NgSfcList.Select(x => x.NgCode).Where(x => !string.IsNullOrWhiteSpace(x)).Distinct().ToList();
            var ngEntityList = await _masterDataService.GetUnqualifiedEntitiesByCodesAsync(new QualUnqualifiedCodeByCodesQuery
            {
                SiteId = param.SiteId,
                Codes = ngCodeList
            });
            if (ngCodeList.Any())
            {
                if (ngEntityList.IsNullOrEmpty())
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES45121)).WithData("ngCode", string.Join(",", ngCodeList));
                }
                var noExistNgCodes = ngCodeList.Except(ngEntityList.Select(x => x.UnqualifiedCode));
                if (noExistNgCodes.Any())
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES45121)).WithData("ngCode", string.Join(",", noExistNgCodes));
                }
            }
            //查询条码信息表数据
            List<string> ngSfcList = param.NgSfcList.Select(m => m.Sfc).Distinct().ToList();
            List<ManuSfcInfoSfcView> sfcInfoList = (await _manuSfcInfoRepository.GetUsedBySFCsAsync(ngSfcList)).ToList();
            if (sfcInfoList.IsNullOrEmpty() || sfcInfoList.Count != ngSfcList.Count)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES45122));
            }

            //数据组装
            DateTime curDate = HymsonClock.Now();
            List<UpdateVehicleFreightStackCommand> updateList = new List<UpdateVehicleFreightStackCommand>();
            List<InteVehicleFreightRecordEntity> recordList = new List<InteVehicleFreightRecordEntity>();
            List<ManuProductBadRecordEntity> badList = new List<ManuProductBadRecordEntity>();
            List<ManuProductNgRecordEntity> ngList = new List<ManuProductNgRecordEntity>();
            foreach (var item in param.NgSfcList)
            {
                ManuProductBadRecordEntity badModel = new ManuProductBadRecordEntity();
                badModel.SiteId = param.SiteId;
                badModel.CreatedBy = param.UserName;
                badModel.CreatedOn = curDate;
                badModel.UpdatedBy = param.UserName;
                badModel.UpdatedOn = curDate;
                badModel.SFC = item.Sfc;
                badModel.SfcInfoId = sfcInfoList.Where(m => m.Sfc == item.Sfc).FirstOrDefault()?.Id;
                badModel.Id = IdGenProvider.Instance.CreateId();
                badModel.FoundBadOperationId = item.OperationId;
                badModel.FoundBadResourceId = item.ResourceId;
                badModel.OutflowOperationId = param.OperationId;
                badModel.UnqualifiedId = ngEntityList.Where(m => m.UnqualifiedCode == item.NgCode).FirstOrDefault()?.Id ?? 0;
                badModel.Status = Core.Enums.Manufacture.ProductBadRecordStatusEnum.Open;
                badModel.Source = Core.Enums.Manufacture.ProductBadRecordSourceEnum.EquipmentReBad;
                badModel.Qty = 1;
                badList.Add(badModel);

                var sfcNgCodeList = param.NgSfcList.Where(m => m.Sfc == item.Sfc).ToList();
                foreach (var sfcCode in sfcNgCodeList)
                {
                    ManuProductNgRecordEntity ngRecord = new ManuProductNgRecordEntity();
                    ngRecord.Id = IdGenProvider.Instance.CreateId();
                    ngRecord.SiteId = param.SiteId;
                    ngRecord.BadRecordId = badModel.Id;
                    ngRecord.UnqualifiedId = ngEntityList.Where(m => m.UnqualifiedCode == sfcCode.NgCode).FirstOrDefault()?.Id ?? 0;
                    ngRecord.NGCode = sfcCode.NgCode;
                    ngRecord.Remark = "托盘NG电芯上报";
                    ngRecord.CreatedOn = curDate;
                    ngRecord.CreatedBy = param.UserName;
                    ngRecord.UpdatedOn = curDate;
                    ngRecord.UpdatedBy = param.UserName;
                    ngList.Add(ngRecord);
                }
            }

            //托盘和电芯解绑
            InteVehicleUnBindDto unBind = new InteVehicleUnBindDto();
            unBind.ContainerCode = param.ContainerCode;
            unBind.SfcList = param.NgSfcList.Select(m => m.Sfc).Distinct().ToList();
            unBind.SiteId = param.SiteId;
            unBind.UserName = param.UserName;
            //电芯设备上报NG出站

            //数据库操作
            using var trans = TransactionHelper.GetTransactionScope(TransactionScopeOption.Required, IsolationLevel.ReadCommitted);
            await VehicleUnBindOperationAsync(unBind);
            await _manuProductBadRecordRepository.InsertRangeAsync(badList);
            await _manuProductNgRecordRepository.InsertRangeAsync(ngList);
            trans.Complete();
        }

    }
}
