using FluentValidation;
using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Integrated;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.CoreServices.Bos.Job;
using Hymson.MES.CoreServices.Services.Common;

using Hymson.MES.Data.Repositories.Integrated;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.Snowflake;
using Hymson.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.CoreServices.Services.Job
{
    /// <summary>
    /// 载具绑定JOB
    /// </summary>
    public class VehicleBindJobService : IJobService
    {
        private readonly IInteVehicleRepository _inteVehicleRepository;
        private readonly AbstractValidator<VehicleBo> _validateBindOperationRules;
        private readonly IInteVehicleTypeRepository _inteVehicleTypeRepository;
       
        private readonly IInteVehicleFreightRepository _inteVehicleFreightRepository;
        private readonly IInteVehiceFreightStackRepository _inteVehiceFreightStackRepository;
        private readonly IInteVehicleFreightRecordRepository _inteVehicleFreightRecordRepository;
        private readonly IInteVehicleTypeVerifyRepository _inteVehicleTypeVerifyRepository;
        private readonly IManuSfcProduceRepository _manuSfcProduceRepository;
        private readonly IManuSfcStepRepository _manuSfcStepRepository;
        private readonly IManuCommonService _manuCommonService;
        public VehicleBindJobService(IInteVehicleRepository inteVehicleRepository
            ,IInteVehiceFreightStackRepository inteVehiceFreightStackRepository
            ,IInteVehicleFreightRecordRepository inteVehicleFreightRecordRepository
            ,IManuSfcStepRepository manuSfcStepRepository
            ,IInteVehicleFreightRepository inteVehicleFreightRepository
            ,IInteVehicleTypeRepository inteVehicleTypeRepository
            ,IManuSfcProduceRepository manuSfcProduceRepository
            ,IManuCommonService manuCommonService
            , IInteVehicleTypeVerifyRepository inteVehicleTypeVerifyRepository
            , AbstractValidator<VehicleBo> validateBindOperationRules)
        {
            _inteVehicleRepository = inteVehicleRepository;
            _validateBindOperationRules = validateBindOperationRules;
            _inteVehiceFreightStackRepository = inteVehiceFreightStackRepository;
            _inteVehicleFreightRecordRepository = inteVehicleFreightRecordRepository;
            _inteVehicleFreightRepository = inteVehicleFreightRepository;
            _inteVehicleTypeRepository = inteVehicleTypeRepository;
            _manuSfcProduceRepository = manuSfcProduceRepository;
            _manuCommonService = manuCommonService;
            _manuSfcStepRepository = manuSfcStepRepository;
            _inteVehicleTypeVerifyRepository = inteVehicleTypeVerifyRepository;
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
            var jobRequestBo = param as JobRequestBo;
            if (jobRequestBo == null)
            {
                return default;
            }
            var vehicleBo = jobRequestBo.VehicleBo;
            if (vehicleBo == null) return default;

            // 待执行的命令
            VehicleBindResponseBo responseBo = new();
           
            var inteVehicleEntity = await _inteVehicleRepository.GetByCodeAsync(new InteVehicleCodeQuery()
            {
                Code = vehicleBo.PalletNo,
                SiteId = jobRequestBo.SiteId
            });
            var vehicleTypeEntity = await _inteVehicleTypeRepository.GetByIdAsync(inteVehicleEntity.VehicleTypeId);

            var vehicleFreightStackEntities = await _inteVehiceFreightStackRepository.GetInteVehiceFreightStackEntitiesAsync(new InteVehiceFreightStackQuery()
            {
                VehicleId = inteVehicleEntity.Id,
                SiteId = jobRequestBo.SiteId
            });
            var  vehicleFreightEntities = await _inteVehicleFreightRepository.GetByVehicleIdsAsync(new long[] { inteVehicleEntity.Id });
            //坑位分组，入坑
            int index = 0;
            var sfclist = vehicleBo.SFCs.Distinct().ToList();
            var manuSfcProduceEntity = await _manuSfcProduceRepository.GetBySFCAsync(new ManuSfcProduceBySfcQuery()
            {
                Sfc = sfclist[0],
                SiteId = jobRequestBo.SiteId
            });
            var vsrlst = vehicleFreightStackEntities.GroupBy(i => i.LocationId).ToList();
           
            foreach (var item in vehicleFreightEntities) //格子循环
            {
                var bar = vsrlst.Find(m => m.Key == item.Id);
                if (bar != null)//已经绑盘的格子添加条码
                {
                    if (bar.Count() < vehicleTypeEntity.CellQty)
                    {
                        var entercount = vehicleTypeEntity.CellQty - bar.Count();
                        int qty = bar.Count();
                        for (int i = 0; i < entercount; i++)
                        {
                            var stackentity = new InteVehicleFreightStackEntity()
                            {
                                BarCode = sfclist[index],
                                CreatedBy = jobRequestBo.UserName,
                                UpdatedBy = jobRequestBo.UserName,
                                CreatedOn = HymsonClock.Now(),
                                UpdatedOn = HymsonClock.Now(),
                                SiteId = jobRequestBo.SiteId,
                                Id = IdGenProvider.Instance.CreateId(),
                                LocationId = item.Id,
                                VehicleId = inteVehicleEntity.Id,
                                IsDeleted = 0
                            };
                            var msse = new ManuSfcStepEntity
                            {
                                Id = IdGenProvider.Instance.CreateId(),
                                SiteId = jobRequestBo.SiteId,
                                SFC = sfclist[index],
                                ProductId = manuSfcProduceEntity.ProductId,
                                WorkOrderId = manuSfcProduceEntity.WorkOrderId,
                                ProductBOMId = manuSfcProduceEntity.ProductBOMId,
                                WorkCenterId = manuSfcProduceEntity.WorkCenterId,
                                Qty = 1,
                                EquipmentId = vehicleBo.EquipmentId,
                                ResourceId = jobRequestBo.ResourceId,

                                ProcedureId = manuSfcProduceEntity.ProcedureId,
                                Operatetype = ManuSfcStepTypeEnum.BarcodeBinding,
                                CurrentStatus = SfcStatusEnum.lineUp,
                                CreatedBy = jobRequestBo.UserName,
                                UpdatedBy = jobRequestBo.UserName,
                            };
                            responseBo.StepList.Add(msse);
                            responseBo.Items.Add(stackentity);
                            //操作记录
                            var inteVehicleFreightRecordEntity = new InteVehicleFreightRecordEntity()
                            {
                                CreatedBy = jobRequestBo.UserName,
                                CreatedOn = HymsonClock.Now(),
                                Id = IdGenProvider.Instance.CreateId(),
                                SiteId = jobRequestBo.SiteId,
                                EquipmentId= vehicleBo.EquipmentId,
                                ResourceId= jobRequestBo.ResourceId,
                                WorkCenterId = manuSfcProduceEntity.WorkCenterId,
                                ProductId = manuSfcProduceEntity.ProductId,
                                ProcedureId= manuSfcProduceEntity.ProcedureId,
                                WorkOrderId= manuSfcProduceEntity.WorkOrderId,
                                VehicleId = inteVehicleEntity.Id
                            };

                            inteVehicleFreightRecordEntity.BarCode = sfclist[index];
                            inteVehicleFreightRecordEntity.LocationId = item.Id;
                            inteVehicleFreightRecordEntity.OperateType = (int)Core.Enums.Integrated.VehicleOperationEnum.Bind;
                            responseBo.Records.Add(inteVehicleFreightRecordEntity);
                            index++;
                            qty++;
                            if (index >= sfclist.Count)
                                break;
                        }
                        item.Qty = qty;
                        item.UpdatedBy = jobRequestBo.UserName;
                        item.UpdatedOn = HymsonClock.Now();
                        responseBo.Locations.Add(item);
                    }
                }
                else //全新的空格子 添加条码
                {
                    if (index >= sfclist.Count) 
                        break;
                    var entercount = vehicleTypeEntity.CellQty - item.Qty;
                    int qty = item.Qty;
                    for (int i = 0; i < entercount; i++)
                    {
                        var stackentity = new InteVehicleFreightStackEntity()
                        {
                            BarCode = sfclist[index],
                            CreatedBy = jobRequestBo.UserName,
                            UpdatedBy = jobRequestBo.UserName,
                            CreatedOn = HymsonClock.Now(),
                            UpdatedOn = HymsonClock.Now(),
                            SiteId = jobRequestBo.SiteId,
                            Id = IdGenProvider.Instance.CreateId(),
                            LocationId = item.Id,
                            VehicleId = inteVehicleEntity.Id,
                            IsDeleted = 0
                        };
                        var msse = new ManuSfcStepEntity
                        {
                            Id = IdGenProvider.Instance.CreateId(),
                            SiteId = jobRequestBo.SiteId,
                            SFC = sfclist[index],
                            ProductId = manuSfcProduceEntity.ProductId,
                            WorkOrderId = manuSfcProduceEntity.WorkOrderId,
                            ProductBOMId = manuSfcProduceEntity.ProductBOMId,
                            WorkCenterId = manuSfcProduceEntity.WorkCenterId,
                            Qty = 1,
                            ProcedureId = manuSfcProduceEntity.ProcedureId,
                            Operatetype = ManuSfcStepTypeEnum.BarcodeBinding,
                            CurrentStatus = SfcStatusEnum.lineUp,
                            EquipmentId = vehicleBo.EquipmentId,
                            ResourceId = jobRequestBo.ResourceId,
                            CreatedBy = jobRequestBo.UserName,
                            UpdatedBy = jobRequestBo.UserName,
                        };
                        responseBo.Items.Add(stackentity);
                        responseBo.StepList.Add(msse);
                        //操作记录
                        var inteVehicleFreightRecordEntity = new InteVehicleFreightRecordEntity()
                        {
                            CreatedBy = jobRequestBo.UserName,
                            CreatedOn = HymsonClock.Now(),
                            Id = IdGenProvider.Instance.CreateId(),
                            SiteId = jobRequestBo.SiteId,
                            VehicleId = inteVehicleEntity.Id,
                            EquipmentId = vehicleBo.EquipmentId,
                            ResourceId = jobRequestBo.ResourceId,
                            WorkCenterId = manuSfcProduceEntity.WorkCenterId,
                            ProductId = manuSfcProduceEntity.ProductId,
                            ProcedureId = manuSfcProduceEntity.ProcedureId,
                            WorkOrderId = manuSfcProduceEntity.WorkOrderId
                        };

                        inteVehicleFreightRecordEntity.BarCode = sfclist[index];
                        inteVehicleFreightRecordEntity.LocationId = item.Id;
                        inteVehicleFreightRecordEntity.OperateType = (int)Core.Enums.Integrated.VehicleOperationEnum.Bind;
                        responseBo.Records.Add(inteVehicleFreightRecordEntity);
                        index++;
                        qty++;
                        if (index >= sfclist.Count)
                            break;
                    }

                    item.Qty = qty;
                    item.UpdatedBy = jobRequestBo.UserName;
                    item.UpdatedOn = HymsonClock.Now();
                    responseBo.Locations.Add(item);
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

            responseBo.Rows += await _inteVehiceFreightStackRepository.InsertsAsync(data.Items);
            responseBo.Rows += await _inteVehicleFreightRecordRepository.InsertsAsync(data.Records);
            responseBo.Rows += await _manuSfcStepRepository.InsertRangeAsync(data.StepList);
            //入库
            await _inteVehicleFreightRepository.UpdatesAsync(data.Locations);
           
            return responseBo;
        }

        public async Task VerifyParamAsync<T>(T param) where T : JobBaseBo
        {
            var jobRequestBo = param as JobRequestBo;
            if (jobRequestBo == null)
            {
                return;
            }
            var vehicleBo = jobRequestBo.VehicleBo;
            if (vehicleBo == null) return;
            await _validateBindOperationRules.ValidateAndThrowAsync(vehicleBo);
            // 验证DTO
            if (vehicleBo.SFCs == null || !vehicleBo.SFCs.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES15400));
            }
            //校验托盘是否可用
            var vehicleEntity = await _inteVehicleRepository.GetByCodeAsync(new InteVehicleCodeQuery()
            {
                Code = vehicleBo.PalletNo,
                SiteId = jobRequestBo.SiteId
            });

            if (vehicleEntity == null || vehicleEntity.Status == DisableOrEnableEnum.Disable)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES18617));
            }

            //校验条码是否已经绑盘
            var vehicleFreightStackEntitiesCompare = await _inteVehiceFreightStackRepository.GetInteVehiceFreightStackEntitiesAsync(new InteVehiceFreightStackQuery()
            {
                Sfcs = vehicleBo.SFCs.ToArray(),
                SiteId = jobRequestBo.SiteId
            }); 
            if (vehicleFreightStackEntitiesCompare != null && vehicleFreightStackEntitiesCompare.Any())
            {
                var bindSfcs = string.Join(",", vehicleBo.SFCs);
                var vehicleIds = string.Join(",", vehicleFreightStackEntitiesCompare.Select(c => c.VehicleId));
                throw new CustomerValidationException(nameof(ErrorCode.MES18616)).WithData("sfc", bindSfcs).WithData("palletNo", vehicleIds);
            }
            
            var vehicleTypeEntity = await _inteVehicleTypeRepository.GetByIdAsync(vehicleEntity.VehicleTypeId);
            var vehicleTypeVerifyEntities = await _inteVehicleTypeVerifyRepository.GetInteVehicleTypeVerifyEntitiesByVehicleTyleIdAsync(new long[] { vehicleTypeEntity.Id });

            var manuSfcProduceEntities = await _manuSfcProduceRepository.GetManuSfcProduceEntitiesAsync(new ManuSfcProduceQuery()
            {
                Sfcs = vehicleBo.SFCs,
                SiteId = jobRequestBo.SiteId
            });
            //获取托盘所有条码记录
            var vehicleFreightStackEntities = await _inteVehiceFreightStackRepository.GetInteVehiceFreightStackEntitiesAsync(new InteVehiceFreightStackQuery()
            {
                VehicleId = vehicleEntity.Id,
                SiteId = jobRequestBo.SiteId
            });
            //校验已绑条码 产品信息和待绑条码产品信息是否一致
            long currentProductId = 0;
            if (vehicleFreightStackEntities != null && vehicleFreightStackEntities.Any())
            {
                var manuSfcProduceEntity = await _manuSfcProduceRepository.GetBySFCAsync(new ManuSfcProduceBySfcQuery()
                {
                    Sfc = vehicleFreightStackEntities.First().BarCode,
                    SiteId = jobRequestBo.SiteId
                });
                currentProductId = manuSfcProduceEntity.ProductId;
            }
            //条码校验
            foreach (var SFC in vehicleBo.SFCs)
            {
                var manuSfcProduceEntity = await _manuSfcProduceRepository.GetBySFCAsync(new ManuSfcProduceBySfcQuery()
                {
                    Sfc = SFC,
                    SiteId = jobRequestBo.SiteId
                });
                //是否有在制信息
                if (manuSfcProduceEntity == null)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES19918)).WithData("SFC", SFC);
                }//报废校验
                else if (manuSfcProduceEntity.IsScrap == TrueOrFalseEnum.Yes)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES19932)).WithData("SFC", SFC);
                }//物料一致性校验
                else if (vehicleTypeVerifyEntities != null && vehicleTypeVerifyEntities.Any())
                {
                    var bar = vehicleTypeVerifyEntities.Where(v => v.Type == Core.Enums.Integrated.VehicleTypeVerifyTypeEnum.Material).ToList();
                    if (!bar.Any(v => v.VerifyId == manuSfcProduceEntity.ProductId))
                    {
                        throw new CustomerValidationException(nameof(ErrorCode.MES19936)).WithData("SFC", SFC)
                            .WithData("P1", manuSfcProduceEntity.ProductId);
                    }
                }
                else if(currentProductId!=0&&(currentProductId != manuSfcProduceEntity.ProductId))
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES19936)).WithData("SFC", SFC)
                            .WithData("P1", manuSfcProduceEntity.ProductId);
                }
                else //条码是否被锁定校验
                {
                    await _manuCommonService.VerifySfcsLockAsync(new CoreServices.Bos.Manufacture.ManuProcedureBo()
                    {
                        SFCs = new string[] { SFC },
                        SiteId = jobRequestBo.SiteId
                    });
                }
            }
            
          
            var vehicleFreightEntities = await _inteVehicleFreightRepository.GetByVehicleIdsAsync(new long[] { vehicleEntity.Id });
            var count = vehicleFreightEntities.Where(i => i.Status == true).ToList().Count();
   
            if ((vehicleFreightStackEntities.Count() + vehicleBo.SFCs.Distinct().Count()) > count * vehicleTypeEntity.CellQty)
            {
                //提交的数量超过了承载量
                throw new CustomerValidationException(nameof(ErrorCode.MES18613));
            }
           

        }
    }
}
