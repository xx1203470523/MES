using Hymson.MES.Core.Domain.Integrated;
using Hymson.MES.CoreServices.Bos.Job;

using Hymson.MES.Data.Repositories.Integrated;
using Hymson.Utils.Tools;
using Hymson.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Core.Constants;
using Hymson.Snowflake;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Attribute.Job;
using Hymson.MES.Core.Enums.Job;

namespace Hymson.MES.CoreServices.Services.Job
{
    /// <summary>
    /// 
    /// </summary>
    [Job("载具解绑", JobTypeEnum.Standard)]
    public class VehicleUnBindJobService : IJobService
    {
        private readonly IInteVehicleFreightRepository _inteVehicleFreightRepository;
        private readonly IInteVehicleRepository _inteVehicleRepository;
        private readonly IInteVehiceFreightStackRepository _inteVehiceFreightStackRepository;
        private readonly IInteVehicleFreightRecordRepository _inteVehicleFreightRecordRepository;
        private readonly IManuSfcProduceRepository _manuSfcProduceRepository;
        private readonly IManuSfcStepRepository _manuSfcStepRepository;
        public VehicleUnBindJobService(IInteVehiceFreightStackRepository inteVehiceFreightStackRepository
            , IInteVehicleRepository inteVehicleRepository
            ,IManuSfcStepRepository  manuSfcStepRepository
            ,IManuSfcProduceRepository manuSfcProduceRepository
            ,IInteVehicleFreightRecordRepository inteVehicleFreightRecordRepository
            ,IInteVehicleFreightRepository inteVehicleFreightRepository) 
        { 
            _manuSfcProduceRepository = manuSfcProduceRepository;
            _manuSfcStepRepository = manuSfcStepRepository;
            _inteVehiceFreightStackRepository = inteVehiceFreightStackRepository;
            _inteVehicleFreightRepository = inteVehicleFreightRepository;
            _inteVehicleRepository = inteVehicleRepository;
            _inteVehicleFreightRecordRepository = inteVehicleFreightRecordRepository;
        }
        public async Task<IEnumerable<JobBo>?> AfterExecuteAsync<T>(T param) where T : JobBaseBo
        {
            return null;
        }

        public async Task<IEnumerable<JobBo>?> BeforeExecuteAsync<T>(T param) where T : JobBaseBo
        {
            return null;
        }

        public async Task<object?> DataAssemblingAsync<T>(T param) where T : JobBaseBo
        {
            var jobRequest = param as JobRequestBo;
            if (jobRequest == null)
            {
                return default;
            }
            var vehicleBo = jobRequest.VehicleBo;
            if (vehicleBo == null) return default;
            // 待执行的命令
            VehicleBindResponseBo responseBo = new();
            //查找到已经绑盘的条码集合
            var stackEntities = await _inteVehiceFreightStackRepository.GetInteVehiceFreightStackEntitiesAsync(new InteVehiceFreightStackQuery()
            {
                Sfcs = vehicleBo.SFCs,
                SiteId = jobRequest.SiteId,
            });
            responseBo.Items = stackEntities.ToList();
            if (stackEntities.Any())
            {
                var grouplist = stackEntities.GroupBy(s => s.LocationId).ToList();
                
                //把条码按位置分组后，进行解盘
                foreach (var groupitem in grouplist)
                {
                    var loc = await _inteVehicleFreightRepository.GetByIdAsync(groupitem.Key);
                    loc.Qty -= groupitem.Count();
                    loc.UpdatedBy = jobRequest.UserName;
                    loc.UpdatedOn = HymsonClock.Now();
                    responseBo.Locations.Add(loc);
                    foreach (var item1 in groupitem)
                    {
                        var manuSfcProduceEntity = await _manuSfcProduceRepository.GetBySFCAsync(new ManuSfcProduceBySfcQuery()
                        {
                            Sfc = item1.BarCode,
                            SiteId = jobRequest.SiteId
                        });
                        if(manuSfcProduceEntity != null)
                        {
                            var inteVehicleFreightRecordEntity = new InteVehicleFreightRecordEntity()
                            {
                                CreatedBy = jobRequest.UserName,
                                CreatedOn = HymsonClock.Now(),
                                Id = IdGenProvider.Instance.CreateId(),
                                SiteId = jobRequest.SiteId,
                                EquipmentId = vehicleBo.EquipmentId,
                                ResourceId = jobRequest.ResourceId,
                                WorkCenterId = manuSfcProduceEntity.WorkCenterId,
                                ProductId = manuSfcProduceEntity.ProductId,
                                ProcedureId = manuSfcProduceEntity.ProcedureId,
                                WorkOrderId = manuSfcProduceEntity.WorkOrderId,
                                VehicleId = item1.VehicleId
                            };

                            inteVehicleFreightRecordEntity.BarCode = item1.BarCode;
                            inteVehicleFreightRecordEntity.LocationId = item1.LocationId;
                            inteVehicleFreightRecordEntity.OperateType = (int)Core.Enums.Integrated.VehicleOperationEnum.Unbind;
                            responseBo.Records.Add(inteVehicleFreightRecordEntity);
                            var msse = new ManuSfcStepEntity
                            {
                                Id = IdGenProvider.Instance.CreateId(),
                                SiteId = jobRequest.SiteId,
                                SFC = item1.BarCode,
                                ProductId = manuSfcProduceEntity.ProductId,
                                WorkOrderId = manuSfcProduceEntity.WorkOrderId,
                                ProductBOMId = manuSfcProduceEntity.ProductBOMId,
                                WorkCenterId = manuSfcProduceEntity.WorkCenterId,
                                Qty = 1,
                                EquipmentId = vehicleBo.EquipmentId,
                                ResourceId = jobRequest.ResourceId,
                                ProcedureId = manuSfcProduceEntity.ProcedureId,
                                Operatetype = ManuSfcStepTypeEnum.BarcodeUnbinding,
                                CurrentStatus = SfcStatusEnum.lineUp,
                                CreatedBy = jobRequest.UserName,
                                UpdatedBy = jobRequest.UserName,
                            };
                            responseBo.StepList.Add(msse);
                        }
                        
                    }
                   
                }
            }
            return responseBo;
        }

        public async Task<JobResponseBo> ExecuteAsync(object obj)
        {
            JobResponseBo responseBo = new();
            if (obj is not VehicleBindResponseBo data)
            {
                return responseBo;
            }

            responseBo.Rows += await _inteVehiceFreightStackRepository.DeletesAsync(data.Items.Select(d=>d.Id).ToArray());
            responseBo.Rows += await _inteVehicleFreightRecordRepository.InsertsAsync(data.Records);
            responseBo.Rows += await _manuSfcStepRepository.InsertRangeAsync(data.StepList);
            //入库
            await _inteVehicleFreightRepository.UpdatesAsync(data.Locations);
           
            return responseBo;
        }

        public async Task VerifyParamAsync<T>(T param) where T : JobBaseBo
        {
            /*校验 待解绑的条码集合是否在当前托盘中
             * 
             */
            var jobRequestBo = param as JobRequestBo;
            if (jobRequestBo == null)
            {
                return;
            }
            var vehicleBo = jobRequestBo.VehicleBo;
            if (vehicleBo == null) return;
            if (!vehicleBo.SFCs.Any()) 
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES18622));
            }
            var inteVehicleEntity = await _inteVehicleRepository.GetByCodeAsync(new InteVehicleCodeQuery()
            {
                Code = vehicleBo.PalletNo,
                SiteId = jobRequestBo.SiteId
            });
            var vehicleFreightStackEntities = await _inteVehiceFreightStackRepository.GetInteVehiceFreightStackEntitiesAsync(new InteVehiceFreightStackQuery()
            {
                VehicleId = inteVehicleEntity.Id,
                SiteId = jobRequestBo.SiteId
            });
            if (vehicleFreightStackEntities == null || !vehicleFreightStackEntities.Any())
                throw new CustomerValidationException(nameof(ErrorCode.MES18643));
            var check1 = vehicleBo.SFCs.Except(vehicleFreightStackEntities.Select(v => v.BarCode));
            if (check1.Any()) { throw new CustomerValidationException(nameof(ErrorCode.MES18642)); }

        }
    }
}
