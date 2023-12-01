using Hymson.Authentication;
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
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Web.Framework.WorkContext;

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

        public ManuSfcCirculationService(ICurrentSystem currentSystem,
            ICurrentUser currentUser,
            ICurrentEquipment currentEquipment,
            IProcMaterialRepository procMaterialRepository,
            IPlanWorkOrderRepository planWorkOrderRepository,
            IManuSfcCirculationRepository manuSfcCirculationRepository,
            IEquEquipmentRepository equipmentRepository,
            IManuSfcProduceRepository manuSfcProduceRepository)
        {
            _currentSystem = currentSystem;
            _currentUser = currentUser;
            _currentEquipment = currentEquipment;
            _procMaterialRepository = procMaterialRepository;
            _planWorkOrderRepository = planWorkOrderRepository;
            _manuSfcCirculationRepository = manuSfcCirculationRepository;
            _equipmentRepository = equipmentRepository;
            _manuSfcProduceRepository = manuSfcProduceRepository;
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
    }
}
