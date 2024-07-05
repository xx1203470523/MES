using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Equipment;
using Hymson.MES.Core.Domain.Plan;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Data.Repositories.Equipment.EquEquipment;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Manufacture.ManuSfcCirculation.Query;
using Hymson.MES.Data.Repositories.Plan;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.SystemServices.Dtos.Manufacture;
using Hymson.MES.SystemServices.Dtos.ProductTraceReport;
using Hymson.Web.Framework.WorkContext;

namespace Hymson.MES.SystemServices.Services.Manufacture
{
    /// <summary>
    /// 条码流转查询服务
    /// </summary>
    public class ManuSfcCirculationService : IManuSfcCirculationService
    {
        private readonly ICurrentSystem _currentSystem;
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

        public ManuSfcCirculationService(ICurrentSystem currentSystem,
            IProcMaterialRepository procMaterialRepository,
            IPlanWorkOrderRepository planWorkOrderRepository,
            IManuSfcCirculationRepository manuSfcCirculationRepository,
            IEquEquipmentRepository equipmentRepository)
        {
            _currentSystem = currentSystem;
            _procMaterialRepository = procMaterialRepository;
            _planWorkOrderRepository = planWorkOrderRepository;
            _manuSfcCirculationRepository = manuSfcCirculationRepository;
            _equipmentRepository = equipmentRepository;
        }

        /// <summary>
        /// 追溯Pack条码层级信息
        /// </summary>
        /// <param name="sfc"></param>
        /// <returns></returns>
        private async Task<IEnumerable<ManuSfcCirculationViewDto>> GetProductTraceListAsync(string sfc)
        {
            var productTraceReportPagedQuery = new ProductTraceReportPagedQuery
            {
                PageIndex = 1,
                PageSize = 2000,//一个Pack不会超过的数量
                SFC = sfc,
                SiteId = _currentSystem.SiteId,
                TraceDirection = false//反向追溯Pack》模组》电芯
            };
            //追溯分页查询
            var pagedInfo = await _manuSfcCirculationRepository.GetProductTraceReportPagedInfoAsync(productTraceReportPagedQuery);
            //工单信息
            IEnumerable<PlanWorkOrderEntity> planWorkOrders = new List<PlanWorkOrderEntity>();
            var workOrderIds = pagedInfo.Data.Select(c => c.WorkOrderId).ToArray();
            if (workOrderIds.Any())
            {
                planWorkOrders = await _planWorkOrderRepository.GetByIdsAsync(workOrderIds);
            }
            //产品信息
            IEnumerable<ProcMaterialEntity> procMaterials = new List<ProcMaterialEntity>();
            var procMaterialIds = pagedInfo.Data.Select(c => c.ProductId).ToArray();
            if (procMaterialIds.Any())
            {
                procMaterials = await _procMaterialRepository.GetByIdsAsync(procMaterialIds);
            }
            //设备信息
            IEnumerable<EquEquipmentEntity> equEquipments = new List<EquEquipmentEntity>();
            var equEquipmentIds = pagedInfo.Data.Select(c => c.EquipmentId ?? -1).ToArray();
            if (equEquipmentIds.Any())
            {
                equEquipments = await _equipmentRepository.GetByIdsAsync(equEquipmentIds);
            }
            var dtos = pagedInfo.Data.Select(s =>
            {
                var returnView = s.ToModel<ManuSfcCirculationViewDto>();
                //工单信息
                var planWorkOrder = planWorkOrders.Where(c => c.Id == s.WorkOrderId).FirstOrDefault();
                if (planWorkOrder != null)
                {
                    returnView.WorkOrderCode = planWorkOrder.OrderCode;
                }
                //产品信息
                var procMaterial = procMaterials.Where(c => c.Id == s.ProductId).FirstOrDefault();
                if (procMaterial != null)
                {
                    returnView.ProductCode = procMaterial.MaterialCode;
                    returnView.ProductName = procMaterial.MaterialName;
                }
                //设备信息
                var equEquipment = equEquipments.Where(c => c.Id == s.EquipmentId).FirstOrDefault();
                if (equEquipment != null)
                {
                    returnView.EquipentCode = equEquipment.EquipmentCode;
                    returnView.EquipentName = equEquipment.EquipmentName;
                }
                return returnView;
            });
            return dtos;
        }

        /// <summary>
        /// 根据Pack条码获取关联记录层级关系
        /// </summary>
        /// <param name="sfc"></param>
        /// <returns></returns>
        public async Task<ManuSfcCirculationDto?> GetRelationShipByPackAsync(string sfc)
        {
            if (string.IsNullOrEmpty(sfc)) { throw new CustomerValidationException(nameof(ErrorCode.MES19203)); }
            //追溯条码信息
            var manuSfcCirculationViews = await GetProductTraceListAsync(sfc);
            if (!manuSfcCirculationViews.Any())
            {
                if (sfc.IndexOf("ES") != -1)
                {
                    return new()
                    {
                        DeviceCode = "",
                        CodeId = sfc,
                        Level = 3
                    };
                }

                if (sfc.IndexOf("YT") != -1)
                {
                    return new()
                    {
                        DeviceCode = "",
                        CodeId = sfc,
                        Level = 2
                    };
                }
            }
            //递归处理层级数据
            int level = 1;
            //第一层Pack信息/第二层模组信息
            var sfcCirculationViewDto = manuSfcCirculationViews.Where(c => c.SFC == sfc).First();
            ManuSfcCirculationDto manuSfcCirculation = new()
            {
                DeviceCode = sfcCirculationViewDto.EquipentCode,
                CodeId = sfc,
                Level = level
            };
            RecursionSfcCirculation(manuSfcCirculation, manuSfcCirculationViews, sfc, level);
            return manuSfcCirculation;
        }

        /// <summary>
        /// 递归组装层级结构
        /// MES中最多三层，Pack》模组》电芯，如果涉及到模组绑定CCS或者其他组件，添加属性，而不是添加层级
        /// </summary>
        /// <param name="manuSfcCirculation"></param>
        /// <param name="manuSfcCirculationViews"></param>
        /// <param name="sfc"></param>
        /// <param name="level"></param>
        private void RecursionSfcCirculation(ManuSfcCirculationDto manuSfcCirculation, IEnumerable<ManuSfcCirculationViewDto> manuSfcCirculationViews, string sfc, int level)
        {
            if (!manuSfcCirculationViews.Any()) { return; }
            if (level > 3) { return; }//避免如果出现错误嵌套层级的无限递归
            var sfcCirculationViewDtos = manuSfcCirculationViews.Where(c => c.CirculationBarCode == sfc).ToArray();
            manuSfcCirculation.SubCode = new ManuSfcCirculationDto[sfcCirculationViewDtos.Length];
            for (int i = 0; i < sfcCirculationViewDtos.Length; i++)
            {
                var circulationViewDto = sfcCirculationViewDtos[i];
                if (manuSfcCirculation.SubCode.Any())
                {
                    var _level = level + 1;
                    manuSfcCirculation.SubCode[i] = new ManuSfcCirculationDto
                    {
                        Level = _level,
                        CodeId = circulationViewDto.SFC,
                        DeviceCode = circulationViewDto.EquipentCode
                    };
                    RecursionSfcCirculation(manuSfcCirculation.SubCode[i], manuSfcCirculationViews, circulationViewDto.SFC, _level);
                }
            }
        }

        /// <summary>
        /// 获取条码绑定关系
        /// </summary>
        /// <param name="Sfc"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuSfcCirculationOutputDto>> GetSfcCirculationBySFCAsync(string Sfc)
        {
            var result = new List<ManuSfcCirculationOutputDto>();

            //获取条码绑定的条码
            var sfcCirculationEntities = await _manuSfcCirculationRepository.GetManuSfcCirculationEntitiesAsync(new() { Sfc = Sfc });

            //获取条码绑定的主条码
            var barCodeCirculationEntities = await _manuSfcCirculationRepository.GetManuSfcCirculationBarCodeEntitiesAsync(new() { CirculationBarCode = Sfc });

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

            return result;
        }
    }
}
