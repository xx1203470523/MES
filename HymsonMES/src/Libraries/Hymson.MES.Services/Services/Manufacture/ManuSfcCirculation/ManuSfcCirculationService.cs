using Hymson.Authentication;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.Data.Repositories.Equipment.EquEquipment;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Plan;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Services.Dtos.Manufacture;
using Hymson.MES.Services.Services.Manufacture.ManuSfc;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;
using Hymson.Web.Framework.WorkContext;
using System.Globalization;
using System.Transactions;

namespace Hymson.MES.Services.Services.Manufacture
{
    /// <summary>
    /// 条码流转查询服务
    /// </summary>
    public class ManuSfcCirculationService : IManuSfcCirculationService
    {
        private readonly ICurrentSystem _currentSystem;
        private readonly ICurrentUser _currentUser;
        private readonly ICurrentEquipment _currentEquipment;

        /// <summary>
        /// 物料维护 仓储
        /// </summary>
        private readonly IProcMaterialRepository _procMaterialRepository;
        /// <summary>
        /// 工单信息表 仓储
        /// </summary>
        private readonly IPlanWorkOrderRepository _planWorkOrderRepository;
        /// <summary>
        /// 条码流转信息
        /// </summary>
        private readonly IManuSfcCirculationRepository _manuSfcCirculationRepository;
        /// <summary>
        /// 设备仓储
        /// </summary>
        private readonly IEquEquipmentRepository _equipmentRepository;
        private readonly IManuSfcProduceRepository _manuSfcProduceRepository;

        private readonly IManuSfcRepository _manuSfcRepository;
        private readonly IManuSfcInfoRepository _manuSfcInfoRepository;
        private readonly IProcProcedureRepository _procProcedureRepository;
        private readonly IProcResourceRepository _procResourceRepository;
        private readonly IProcResourceEquipmentBindRepository _procResourceEquipmentBindRepository;

        public ManuSfcCirculationService(ICurrentSystem currentSystem,
            ICurrentUser currentUser,
            ICurrentEquipment currentEquipment,
            IProcMaterialRepository procMaterialRepository,
            IPlanWorkOrderRepository planWorkOrderRepository,
            IManuSfcCirculationRepository manuSfcCirculationRepository,
            IEquEquipmentRepository equipmentRepository,
            IManuSfcProduceRepository manuSfcProduceRepository,
            IManuSfcRepository manuSfcRepository,
            IManuSfcInfoRepository manuSfcInfoRepository,
            IProcProcedureRepository procProcedureRepository,
            IProcResourceRepository procResourceRepository,
            IProcResourceEquipmentBindRepository procResourceEquipmentBindRepository)
        {
            _currentSystem = currentSystem;
            _currentUser = currentUser;
            _currentEquipment = currentEquipment;
            _procMaterialRepository = procMaterialRepository;
            _planWorkOrderRepository = planWorkOrderRepository;
            _manuSfcCirculationRepository = manuSfcCirculationRepository;
            _equipmentRepository = equipmentRepository;
            _manuSfcProduceRepository = manuSfcProduceRepository;
            _manuSfcRepository = manuSfcRepository;
            _manuSfcInfoRepository = manuSfcInfoRepository;
            _procProcedureRepository = procProcedureRepository;
            _procResourceRepository = procResourceRepository;
            _procResourceEquipmentBindRepository = procResourceEquipmentBindRepository;
        }

        /// <summary>
        /// 获取条码绑定关系
        /// </summary>
        /// <param name="Sfc"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuSfcCirculationOutputDto>> GetManuSfcCirculationBySFCAsync(string Sfc)
        {
            var result = new List<ManuSfcCirculationOutputDto>();

            //获取条码绑定的条码
            var sfcCirculationEntities = await _manuSfcCirculationRepository.GetManuSfcCirculationEntitiesAsync(new() { Sfc = Sfc, SiteId = 123456 });

            //获取条码绑定的主条码
            var barCodeCirculationEntities = await _manuSfcCirculationRepository.GetManuSfcCirculationBarCodeEntitiesAsync(new() { CirculationBarCode = Sfc, SiteId = 123456 });

            foreach (var item in sfcCirculationEntities)
            {
                result.Add(new()
                {
                    Id = item.Id,
                    BindSFC = item.SFC,
                    SFC = item.CirculationBarCode,
                    UpdatedBy = item.UpdatedBy,
                    UpdatedOn = item.UpdatedOn,
                });
            }

            foreach (var item in barCodeCirculationEntities)
            {
                result.Add(new()
                {
                    Id = item.Id,
                    BindSFC = item.CirculationBarCode,
                    SFC = item.SFC,
                    UpdatedBy = item.UpdatedBy,
                    UpdatedOn = item.UpdatedOn,
                });
            }

            if (result?.Any() == false)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES19106));
            }


            return result;
        }

        public async Task<int> DeteleteManuSfcCirculationAsync(long id)
        {
            return await _manuSfcCirculationRepository.DeleteRangeAsync(new()
            {
                Ids = new long[] { id },
                DeleteOn = HymsonClock.Now(),
                UserId = _currentUser.UserId?.ToString()
            });
        }

        public async Task<int> CreateManuSfcCirculationAsync(ManuSfcCirculationCreateDto createDto)
        {

            var manuSfcProduceEntities = await _manuSfcProduceRepository.GetManuSfcProduceEntitiesAsync(new()
            {
                Sfcs = new string[] { createDto.SFC },
                SiteId = 123456
            }) ?? throw new CustomerValidationException(nameof(ErrorCode.MES16600));

            var manuSfcProduceEntity = manuSfcProduceEntities.FirstOrDefault();

            //记录流转信息
            var createCommand = new ManuSfcCirculationEntity
            {
                Id = IdGenProvider.Instance.CreateId(),
                SiteId = 123456,
                ProcedureId = manuSfcProduceEntity.ProcedureId,
                ResourceId = manuSfcProduceEntity.ResourceId,
                SFC = createDto.CirculationBarCode,
                WorkOrderId = manuSfcProduceEntity.WorkOrderId,
                ProductId = manuSfcProduceEntity.ProductId,
                EquipmentId = null,
                CirculationBarCode = createDto.SFC,
                CirculationProductId = manuSfcProduceEntity.ProductId,//暂时使用原有产品ID
                CirculationMainProductId = manuSfcProduceEntity.ProductId,
                Location = "0",
                CirculationQty = 1,
                CirculationType = createDto.CirculationBarCode.Contains("CCS") ? SfcCirculationTypeEnum.BindCCS : SfcCirculationTypeEnum.Merge,
                CreatedBy = _currentUser.UserName,
                CreatedOn = HymsonClock.Now(),
                UpdatedBy = _currentEquipment.Name,
                UpdatedOn = HymsonClock.Now(),
                ModelCode = ""
            };

            return await _manuSfcCirculationRepository.InsertAsync(createCommand);

        }

        /// <summary>
        /// 绑定条码关系
        /// </summary>
        /// <param name="bindDto"></param>
        /// <returns></returns>
        public async Task CreateManuSfcCirculationAsync(ManuSfcCirculationBindDto bindDto)
        {
            var manuSfc = await _manuSfcRepository.GetBySFCAsync(new() { SFC = bindDto.SFC, SiteId = 123456 });
            //?? throw new CustomerValidationException(nameof(ErrorCode.MES16371)).WithData("BindSFC", bindDto.SFC);

            var manuSfcInfo = await _manuSfcInfoRepository.GetBySFCAsync(manuSfc.Id);
            //?? throw new CustomerValidationException(nameof(ErrorCode.MES16371)).WithData("BindSFC", bindDto.SFC);

            ////获取条码信息
            //var bindManuSfc = await _manuSfcRepository.GetBySFCAsync(new() { SFC = bindDto.BindSFC, SiteId = 123456 })
            //    ?? throw new CustomerValidationException(nameof(ErrorCode.MES16371)).WithData("BindSFC", bindDto.BindSFC);

            //var bindManuSfcInfo = await _manuSfcInfoRepository.GetBySFCAsync(bindManuSfc.Id)
            //    ?? throw new CustomerValidationException(nameof(ErrorCode.MES16371)).WithData("BindSFC", bindDto.BindSFC);

            var procedureEntity = await _procProcedureRepository.GetByIdAsync(bindDto.procedureId)
                ?? throw new CustomerValidationException(nameof(ErrorCode.MES17311));

            //获取资源
            var resourceEntities = await _procResourceRepository.GetByResTypeIdsAsync(new() { IdsArr = new long[] { procedureEntity.ResourceTypeId.GetValueOrDefault() }, SiteId = 123456 });
            var resourceEntity = resourceEntities.FirstOrDefault() ?? throw new CustomerValidationException(nameof(ErrorCode.MES10389));

            //根据工序获取资源跟设备
            var equEuipmentEntities = await _procResourceEquipmentBindRepository.GetByResourceIdAsync(new() { ResourceId = resourceEntity.Id })
                ?? throw new CustomerValidationException(nameof(ErrorCode.MES12620));
            var equEuipmentEntity = equEuipmentEntities.FirstOrDefault();

            //获取条码流转记录
            var bindSfcCirculationEntities = await _manuSfcCirculationRepository.GetManuSfcCirculationBarCodeEntitiesAsync(new()
            {
                CirculationBarCode = bindDto.SFC,
                SiteId = 123456
            });

            //获取条码流转记录
            var sfcCirculationEntities = await _manuSfcCirculationRepository.GetManuSfcCirculationBarCodeEntitiesAsync(new()
            {
                Sfcs = new string[] { bindDto.SFC },
                SiteId = 123456
            });

            var locationId = 0;

            //获取最后一个位置码+1
            if (bindSfcCirculationEntities.Any())
            {
                var location = bindSfcCirculationEntities.Max(a => a.Location);

                if (int.TryParse(location, out locationId))
                    locationId += 1;
            }

            ManuSfcCirculationCreateDto sfcData = new ManuSfcCirculationCreateDto();

            //目前绑定只添加绑定关系，不增加条码信息和在制信息
            ManuSfcCirculationCreateDto bindSfcData = new ManuSfcCirculationCreateDto()
            {
                Id = IdGenProvider.Instance.CreateId(),
                SiteId = 123456,
                ProcedureId = procedureEntity.Id,
                ResourceId = resourceEntity.Id,
                EquipmentId = equEuipmentEntity?.Id,
                FeedingPointId = null,
                SFC = bindDto.BindSFC,
                WorkOrderId = manuSfcInfo?.WorkOrderId ?? 0,
                ProductId = manuSfcInfo?.ProductId ?? 0,
                Location = locationId.ToString(),
                CirculationBarCode = bindDto.SFC,
                CirculationWorkOrderId = manuSfcInfo?.WorkOrderId ?? 0, //暂不考虑流转后工单变更场景
                CirculationProductId = manuSfcInfo?.ProductId ?? 0, //产品同上
                CirculationQty = 1,
                CirculationType = "2",
                IsDisassemble = Core.Enums.TrueOrFalseEnum.No,
                CreatedBy = _currentUser.UserName ?? "system",
                CreatedOn = HymsonClock.Now(),
                UpdatedBy = _currentUser.UserName,
                UpdatedOn = HymsonClock.Now(),
                IsDeleted = 0
            };

            if (!sfcCirculationEntities.Any())
            {
                //添加绑定条码信息
                sfcData = new ManuSfcCirculationCreateDto()
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SiteId = 123456,
                    ProcedureId = procedureEntity.Id,
                    ResourceId = resourceEntity.Id,
                    EquipmentId = equEuipmentEntity?.Id,
                    FeedingPointId = null,
                    SFC = bindDto.SFC,
                    WorkOrderId = manuSfcInfo?.WorkOrderId ?? 0,
                    ProductId = manuSfcInfo?.ProductId ?? 0,
                    Location = "0",
                    CirculationBarCode = "",
                    CirculationWorkOrderId = manuSfcInfo?.WorkOrderId ?? 0, //暂不考虑流转后工单变更场景
                    CirculationProductId = manuSfcInfo?.ProductId ?? 0, //产品同上
                    CirculationQty = 1,
                    CirculationType = "2",
                    IsDisassemble = Core.Enums.TrueOrFalseEnum.No,
                    CreatedBy = _currentUser.UserName ?? "system",
                    CreatedOn = HymsonClock.Now(),
                    UpdatedBy = _currentUser.UserName,
                    UpdatedOn = HymsonClock.Now(),
                    IsDeleted = 0
                };
            }

            if (bindDto.BindSFC.IndexOf("CCS") != -1)
            {
                bindSfcData.CirculationType = "6";
            }

            using var tran = TransactionHelper.GetTransactionScope();

            //插入条码记录
            if (sfcData.Id != 0)
            {
                var sfcEntity = sfcData.ToEntity<ManuSfcCirculationEntity>();
                var insertSfc = await _manuSfcCirculationRepository.InsertAsync(sfcEntity);
            }

            //插入绑定条码绑定记录
            var BindSfcEntity = bindSfcData.ToEntity<ManuSfcCirculationEntity>();
            var insertBindSfc = await _manuSfcCirculationRepository.InsertAsync(BindSfcEntity);

            tran.Complete();
        }
    }
}
