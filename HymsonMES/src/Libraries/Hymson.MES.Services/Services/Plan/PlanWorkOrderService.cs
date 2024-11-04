using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Integrated;
using Hymson.MES.Core.Domain.Plan;
using Hymson.MES.Core.Enums;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Integrated.IIntegratedRepository;
using Hymson.MES.Data.Repositories.Integrated.InteSFCBox;
using Hymson.MES.Data.Repositories.Plan;
using Hymson.MES.Data.Repositories.Plan.PlanWorkOrder.Command;
using Hymson.MES.Data.Repositories.Plan.PlanWorkOrder.Query;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Services.Dtos.Plan;
using Hymson.MES.Services.Dtos.Report;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;
using IdGen;
using Minio.DataModel;
using System.Collections;
using System.Transactions;

namespace Hymson.MES.Services.Services.Plan
{
    /// <summary>
    /// 工单信息表 服务
    /// </summary>
    public class PlanWorkOrderService : IPlanWorkOrderService
    {
        private readonly ICurrentUser _currentUser;
        private readonly ICurrentSite _currentSite;

        /// <summary>
        /// 工单信息表 仓储
        /// </summary>
        private readonly IPlanWorkOrderRepository _planWorkOrderRepository;
        private readonly AbstractValidator<PlanWorkOrderCreateDto> _validationCreateRules;
        private readonly AbstractValidator<PlanWorkOrderModifyDto> _validationModifyRules;
        private readonly IProcMaterialRepository _procMaterialRepository;
        private readonly IProcBomRepository _procBomRepository;
        private readonly IProcProcessRouteRepository _procProcessRouteRepository;
        private readonly IInteWorkCenterRepository _inteWorkCenterRepository;
        private readonly IPlanWorkOrderStatusRecordRepository _planWorkOrderStatusRecordRepository;
        private readonly IPlanWorkOrderActivationRepository _planWorkOrderActivationRepository;
        private readonly IPlanWorkOrderActivationRecordRepository _planWorkOrderActivationRecordRepository;

        private readonly AbstractValidator<PlanWorkOrderChangeStatusDto> _validationChangeStatusRules;
        private readonly IInteSFCBoxRepository _inteSFCBoxRepository;
        private readonly IPlanWorkOrderConversionRepository _planWorkOrderConversionRepository;

        public PlanWorkOrderService(ICurrentUser currentUser, ICurrentSite currentSite,
            AbstractValidator<PlanWorkOrderCreateDto> validationCreateRules,
            AbstractValidator<PlanWorkOrderModifyDto> validationModifyRules,
            IPlanWorkOrderRepository planWorkOrderRepository,
            IProcMaterialRepository procMaterialRepository,
            IProcBomRepository procBomRepository,
            IProcProcessRouteRepository procProcessRouteRepository,
            IInteWorkCenterRepository inteWorkCenterRepository,
            IPlanWorkOrderStatusRecordRepository planWorkOrderStatusRecordRepository, IPlanWorkOrderActivationRecordRepository planWorkOrderActivationRecordRepository,
            IPlanWorkOrderActivationRepository planWorkOrderActivationRepository, AbstractValidator<PlanWorkOrderChangeStatusDto> validationChangeStatusRules,
            IInteSFCBoxRepository inteSFCBoxRepository,
            IPlanWorkOrderConversionRepository planWorkOrderConversionRepository)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _validationCreateRules = validationCreateRules;
            _validationModifyRules = validationModifyRules;
            _planWorkOrderRepository = planWorkOrderRepository;

            _procMaterialRepository = procMaterialRepository;
            _procBomRepository = procBomRepository;
            _procProcessRouteRepository = procProcessRouteRepository;
            _inteWorkCenterRepository = inteWorkCenterRepository;
            _planWorkOrderStatusRecordRepository = planWorkOrderStatusRecordRepository;
            _planWorkOrderActivationRecordRepository = planWorkOrderActivationRecordRepository;
            _planWorkOrderActivationRepository = planWorkOrderActivationRepository;
            _validationChangeStatusRules = validationChangeStatusRules;
            _inteSFCBoxRepository = inteSFCBoxRepository;
            _planWorkOrderConversionRepository = planWorkOrderConversionRepository;
        }


        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public async Task CreatePlanWorkOrderAsync(PlanWorkOrderCreateDto command)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0) throw new ValidationException(nameof(ErrorCode.MES10101));

            // 验证DTO
            await _validationCreateRules.ValidateAndThrowAsync(command);

            command.OrderCode = command.OrderCode.ToUpper();
            // 判断编号是否已存在
            var haveEntities = await _planWorkOrderRepository.GetEqualPlanWorkOrderEntitiesAsync(new PlanWorkOrderQuery()
            {
                SiteId = _currentSite.SiteId ?? 123456,
                OrderCode = command.OrderCode
            });
            if (haveEntities != null && haveEntities.Any() == true)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16001)).WithData("orderCode", command.OrderCode);
            }

            // DTO转换实体
            var planWorkOrderEntity = command.ToEntity<PlanWorkOrderEntity>();
            planWorkOrderEntity.Id = IdGenProvider.Instance.CreateId();
            planWorkOrderEntity.CreatedBy = _currentUser.UserName;
            planWorkOrderEntity.UpdatedBy = _currentUser.UserName;
            planWorkOrderEntity.CreatedOn = HymsonClock.Now();
            planWorkOrderEntity.UpdatedOn = HymsonClock.Now();
            planWorkOrderEntity.SiteId = _currentSite.SiteId ?? 123456;

            //关联批次箱码
            var boxCodeBindWorkOrder = new List<InteSFCBoxWorkOrderEntity>();
            if (command.SFCBox != null)
            {
                var batchno = command.SFCBox.Select(x => x.BatchNo).ToArray();

                if (batchno == null)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES19303));
                }

                if (batchno.Count() > 2)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES19307));
                }
                //按批次取OCVB IMPB
                var getBatchNo = await _inteSFCBoxRepository.GetByBoxCodesAsync(batchno);

                if (getBatchNo?.Any() == true)
                {
                    var current = getBatchNo.FirstOrDefault();
                    int ocvbrange = 8;
                    double impbrange = 0.25;

                    if (current.OCVBDiff > ocvbrange)
                    {
                        throw new CustomerValidationException(nameof(ErrorCode.MES19304)).WithData("OCVBDiff", ocvbrange);
                    }
                    if (current.MaxIMPB > (decimal)impbrange)
                    {
                        throw new CustomerValidationException(nameof(ErrorCode.MES19305)).WithData("MaxIMPB", impbrange);
                    }

                    boxCodeBindWorkOrder.Add(new InteSFCBoxWorkOrderEntity()
                    {
                        Id = IdGenProvider.Instance.CreateId(),
                        Siteid = _currentSite.SiteId ?? 123456,
                        UpdatedBy = _currentUser.UserName,
                        UpdatedOn = HymsonClock.Now(),
                        CreatedOn = HymsonClock.Now(),
                        CreatedBy = _currentUser.UserName,
                        BoxCode = current.BatchNo,
                        BatchNo = current.BatchNo,
                        Grade = current.Grade ?? string.Empty,
                        WorkOrderId = planWorkOrderEntity.Id,
                    });
                }

                //foreach (var item in command.SFCBox)
                //{
                //    if (item.BatchNo != null)
                //    {
                //        //校验最大与最小,暂时为8范围
                //        var currentboxCode = getBatchNo.Where(x => x.BoxCode == item.BoxCode).FirstOrDefault();
                //        int ocvbrange = 8;
                //        double impbrange = 0.25;

                //        if (currentboxCode != null)
                //        {
                //            if (currentboxCode.OCVBDiff > ocvbrange)
                //            {
                //                throw new CustomerValidationException(nameof(ErrorCode.MES19304)).WithData("OCVBDiff", ocvbrange);
                //            }
                //            if (currentboxCode.MaxIMPB > (decimal)impbrange)
                //            {
                //                throw new CustomerValidationException(nameof(ErrorCode.MES19305)).WithData("MaxIMPB", impbrange);
                //            }
                //        }

                //        boxCodeBindWorkOrder.Add(new InteSFCBoxWorkOrderEntity()
                //        {
                //            Id = IdGenProvider.Instance.CreateId(),
                //            Siteid = _currentSite.SiteId ?? 123456,
                //            UpdatedBy = _currentUser.UserName,
                //            UpdatedOn = HymsonClock.Now(),
                //            CreatedOn = HymsonClock.Now(),
                //            CreatedBy = _currentUser.UserName,
                //            BoxCode = item.BoxCode,
                //            Grade = item.Grade ?? string.Empty,
                //            WorkOrderId = planWorkOrderEntity.Id,
                //        });
                //    }
                //}
            }

            var planWorkOrderRecordEntity = new PlanWorkOrderRecordEntity()
            {
                Id = IdGenProvider.Instance.CreateId(),
                UpdatedBy = _currentUser.UserName,
                CreatedBy = _currentUser.UserName,
                SiteId = _currentSite.SiteId ?? 123456,
                WorkOrderId = planWorkOrderEntity.Id,
                InputQty = 0,
                UnqualifiedQuantity = 0,
                FinishProductQuantity = 0,
                PassDownQuantity = 0
            };

            var planWorkOrderConversionCreateCommand = new PlanWorkOrderConversionCreateCommand()
            {
                Id = IdGenProvider.Instance.CreateId(),
                PlanWorkOrderId = planWorkOrderEntity.Id,
                ModuleConversion = command.ModuleConversion.GetValueOrDefault(),
                PackConversion = command.PackConversion.GetValueOrDefault(),
                CreatedBy = _currentUser.UserName,
                UpdatedBy = _currentUser.UserName,
                CreatedOn = HymsonClock.Now(),
                UpdatedOn = HymsonClock.Now(),
                IsDeleted = 0
            };

            // 检查工艺路线
            var procProcessRouteEntity = await _procProcessRouteRepository.GetByIdAsync(planWorkOrderEntity.ProcessRouteId)
                ?? throw new CustomerValidationException(nameof(ErrorCode.MES10438));

            // 工艺路线状态校验
            if (procProcessRouteEntity.Status != SysDataStatusEnum.Enable && procProcessRouteEntity.Status != SysDataStatusEnum.Retain)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10448));
            }

            // 入库
            using var ts = TransactionHelper.GetTransactionScope();
            var response = await _planWorkOrderRepository.InsertAsync(planWorkOrderEntity);

            //工单关联SFCbox信息保存
            if (boxCodeBindWorkOrder != null && boxCodeBindWorkOrder.Count > 0)
            {
                await _inteSFCBoxRepository.InsertSFCBoxWorkOrderAsync(boxCodeBindWorkOrder);
            }


            if (response == 0)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16002));
            }
            await _planWorkOrderRepository.InsertPlanWorkOrderRecordAsync(planWorkOrderRecordEntity);

            await _planWorkOrderConversionRepository.InsertAsync(planWorkOrderConversionCreateCommand);

            ts.Complete();
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public async Task ModifyPlanWorkOrderAsync(PlanWorkOrderModifyDto command)
        {
            if (_currentSite.SiteId == 0) throw new CustomerValidationException(nameof(ErrorCode.MES10101));

            // 验证DTO
            await _validationModifyRules.ValidateAndThrowAsync(command);

            //获取当前最新的数据  进行状态判断
            var current = await _planWorkOrderRepository.GetByIdAsync(command.Id);
            if (current != null)
            {
                //判断当前状态  
                if (!(current.Status == PlanWorkOrderStatusEnum.NotStarted || current.Status == PlanWorkOrderStatusEnum.InProduction))
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES16046));
                }
            }
            else
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16003));
            }


            // DTO转换实体
            var planWorkOrderEntity = command.ToEntity<PlanWorkOrderEntity>();
            planWorkOrderEntity.UpdatedBy = _currentUser.UserName;
            planWorkOrderEntity.UpdatedOn = HymsonClock.Now();



            var planWorkOrderConversionUpdateCommand = new PlanWorkOrderConversionUpdateCommand()
            {
                Id = IdGenProvider.Instance.CreateId(),
                PlanWorkOrderId = planWorkOrderEntity.Id,
                ModuleConversion = command.ModuleConversion.GetValueOrDefault(),
                PackConversion = command.PackConversion.GetValueOrDefault(),
                UpdatedBy = _currentUser.UserName,
                UpdatedOn = HymsonClock.Now(),
                IsDeleted = 0
            };


            //关联箱码
            var boxCodeBindWorkOrder = new List<InteSFCBoxWorkOrderEntity>();
            if (command.SFCBox != null)
            {
                var batchno = command.SFCBox.Select(x => x.BatchNo).ToArray();

                if (batchno == null)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES19303));
                }

                if (batchno.Count() > 1)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES19307));
                }
                //批量查询
                var getBatchNo = await _inteSFCBoxRepository.GetByBoxCodesAsync(batchno);

                if (getBatchNo != null)
                {
                    var getBatchNoCurrent = getBatchNo.FirstOrDefault();
                    int ocvbrange = 8;
                    double impbrange = 0.25;

                    if (getBatchNoCurrent.OCVBDiff > ocvbrange)
                    {
                        throw new CustomerValidationException(nameof(ErrorCode.MES19304)).WithData("OCVBDiff", ocvbrange);
                    }
                    if (getBatchNoCurrent.MaxIMPB > (decimal)impbrange)
                    {
                        throw new CustomerValidationException(nameof(ErrorCode.MES19305)).WithData("MaxIMPB", impbrange);
                    }

                    boxCodeBindWorkOrder.Add(new InteSFCBoxWorkOrderEntity()
                    {
                        Id = IdGenProvider.Instance.CreateId(),
                        Siteid = _currentSite.SiteId ?? 123456,
                        UpdatedBy = _currentUser.UserName,
                        UpdatedOn = HymsonClock.Now(),
                        CreatedOn = HymsonClock.Now(),
                        CreatedBy = _currentUser.UserName,
                        BoxCode = getBatchNoCurrent.BatchNo,
                        BatchNo = getBatchNoCurrent.BatchNo,
                        Grade = getBatchNoCurrent.Grade ?? string.Empty,
                        WorkOrderId = planWorkOrderEntity.Id,
                    });
                }

                //    var boxCodes = command.SFCBox.Select(x => x.BoxCode).ToArray();

                //    if (boxCodes == null)
                //    {
                //        throw new CustomerValidationException(nameof(ErrorCode.MES19303));
                //    }
                //    //批量查询
                //    var getBoxCodes = await _inteSFCBoxRepository.GetByBoxCodesAsync(boxCodes);

                //    foreach (var item in command.SFCBox)
                //    {
                //        if (item.BoxCode != null)
                //        {
                //            //校验最大与最小,暂时为8范围
                //            var currentboxCode = getBoxCodes.Where(x => x.BoxCode == item.BoxCode).FirstOrDefault();
                //            int ocvbrange = 8;
                //            double impbrange = 0.25;

                //            if (currentboxCode != null)
                //            {
                //                if (currentboxCode.OCVBDiff > ocvbrange)
                //                {
                //                    throw new CustomerValidationException(nameof(ErrorCode.MES19304)).WithData("OCVBDiff", ocvbrange);
                //                }
                //                if (currentboxCode.MaxIMPB > (decimal)impbrange)
                //                {
                //                    throw new CustomerValidationException(nameof(ErrorCode.MES19305)).WithData("MaxIMPB", impbrange);
                //                }
                //            }

                //            boxCodeBindWorkOrder.Add(new InteSFCBoxWorkOrderEntity()
                //            {
                //                Id = IdGenProvider.Instance.CreateId(),
                //                Siteid = _currentSite.SiteId ?? 123456,
                //                UpdatedBy = _currentUser.UserName,
                //                UpdatedOn = HymsonClock.Now(),
                //                CreatedOn = HymsonClock.Now(),
                //                CreatedBy = _currentUser.UserName,
                //                BoxCode = item.BoxCode,
                //                Grade = item.Grade ?? string.Empty,
                //                WorkOrderId = planWorkOrderEntity.Id,
                //            });
                //        }
                //    }
            }


            using var ts = TransactionHelper.GetTransactionScope();
            var response = await _planWorkOrderRepository.UpdateAsync(planWorkOrderEntity);
            if (response == 0)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16004));
            }
            //删除关联表数据
            await _inteSFCBoxRepository.DeleteSFCBoxWorkOrderAsync(planWorkOrderEntity.Id);

            if (boxCodeBindWorkOrder != null && boxCodeBindWorkOrder.Count > 0)
            {
                //工单关联SFCbox信息保存
                response = await _inteSFCBoxRepository.InsertSFCBoxWorkOrderAsync(boxCodeBindWorkOrder);
            }

            await _planWorkOrderConversionRepository.InsertOrUpdateAsync(planWorkOrderConversionUpdateCommand);

            ts.Complete();
        }

        /// <summary>
        /// 修改工单状态
        /// </summary>
        /// <param name="parms"></param>
        /// <returns></returns>
        public async Task ModifyWorkOrderStatusAsync(List<PlanWorkOrderChangeStatusDto> parms)
        {
            if (parms == null || !parms.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10100));
            }

            foreach (var item in parms)
            {
                //校验
                await _validationChangeStatusRules.ValidateAndThrowAsync(item);
            }

            //查询需要改变的工单
            var workOrders = await _planWorkOrderRepository.GetByIdsAsync(parms.Select(x => x.Id).ToArray());
            if (workOrders == null || !workOrders.Any() || workOrders.Any(x => x.IsDeleted > 0) || workOrders.Count() != parms.Count())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16014));
            }

            #region//判断订单是否可以继续修改状态 

            if (parms.First().Status == PlanWorkOrderStatusEnum.SendDown) //需要修改为已下达的
            {
                foreach (var item in workOrders)
                {
                    if (item.Status != PlanWorkOrderStatusEnum.NotStarted)//判断是否有不是未开始的则无法更改状态
                    {
                        throw new CustomerValidationException(nameof(ErrorCode.MES16006));
                    }
                }
            }
            if (parms.First().Status == PlanWorkOrderStatusEnum.Finish) //需要修改为完工的
            {
                foreach (var item in workOrders)
                {
                    if (item.Status != PlanWorkOrderStatusEnum.InProduction)//判断是否有不是生产中的则无法更改状态
                    {
                        throw new CustomerValidationException(nameof(ErrorCode.MES16011));
                    }
                }
            }
            if (parms.First().Status == PlanWorkOrderStatusEnum.Closed) //需要修改为关闭的
            {
                foreach (var item in workOrders)
                {
                    if (item.Status != PlanWorkOrderStatusEnum.Finish)//判断是否有不是完工的则无法更改状态
                    {
                        throw new CustomerValidationException(nameof(ErrorCode.MES16012));
                    }
                }
            }

            #endregion

            List<UpdateStatusCommand> planWorkOrderEntities = new List<UpdateStatusCommand>();
            List<long> updateWorkOrderRealEndList = new List<long>();
            List<long> deleteActivationWorkOrderIds = new List<long>();//需要取消激活工单

            foreach (var item in parms)
            {
                var workOrder = workOrders.FirstOrDefault(x => x.Id == item.Id);
                if (workOrder != null)
                {
                    planWorkOrderEntities.Add(new UpdateStatusCommand()
                    {
                        Id = item.Id,
                        Status = item.Status,
                        BeforeStatus = workOrder.Status,
                        UpdatedBy = _currentUser.UserName,
                        UpdatedOn = HymsonClock.Now()
                    });
                }

                //对是需要修改为关闭状态的做特殊处理： 给 工单记录表 更新 真实结束时间
                if (item.Status == PlanWorkOrderStatusEnum.Closed)
                {
                    updateWorkOrderRealEndList.Add(item.Id);
                }

                //对是需要修改为关闭状态的做特殊处理： 取消掉 对应工单激活的信息
                if (item.Status == PlanWorkOrderStatusEnum.Closed)
                {
                    deleteActivationWorkOrderIds.Add(item.Id);
                }
            }

            List<PlanWorkOrderActivationRecordEntity> planWorkOrderActivationRecordEntitys = new List<PlanWorkOrderActivationRecordEntity>();//对取消激活的做记录
            if (deleteActivationWorkOrderIds.Any())
            {
                var deleteActivationWorkOrders = await _planWorkOrderActivationRepository.GetByWorkOrderIdsAsync(deleteActivationWorkOrderIds.ToArray());
                foreach (var item in deleteActivationWorkOrders)
                {
                    planWorkOrderActivationRecordEntitys.Add(new PlanWorkOrderActivationRecordEntity()
                    {
                        Id = IdGenProvider.Instance.CreateId(),
                        CreatedBy = _currentUser.UserName,
                        UpdatedBy = _currentUser.UserName,
                        CreatedOn = HymsonClock.Now(),
                        UpdatedOn = HymsonClock.Now(),
                        SiteId = _currentSite.SiteId ?? 123456,

                        WorkOrderId = item.WorkOrderId,
                        LineId = item.LineId,

                        ActivateType = PlanWorkOrderActivateTypeEnum.CancelActivate
                    });
                }
            }


            //组装工单状态变化记录
            List<PlanWorkOrderStatusRecordEntity> planWorkOrderStatusRecordEntities = new List<PlanWorkOrderStatusRecordEntity>();
            foreach (var item in workOrders)
            {
                var record = AutoMapperConfiguration.Mapper.Map<PlanWorkOrderStatusRecordEntity>(item);
                record.Id = IdGenProvider.Instance.CreateId();
                record.CreatedBy = _currentUser.UserName;
                record.UpdatedBy = _currentUser.UserName;
                record.CreatedOn = HymsonClock.Now();
                record.UpdatedOn = HymsonClock.Now();
                record.SiteId = _currentSite.SiteId ?? 123456;
                record.IsDeleted = 0;

                planWorkOrderStatusRecordEntities.Add(record);
            }

            using (TransactionScope ts = new TransactionScope())
            {
                var response = await _planWorkOrderRepository.ModifyWorkOrderStatusAsync(planWorkOrderEntities);
                if (response != planWorkOrderEntities.Count)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES16037));
                }

                if (updateWorkOrderRealEndList.Any()) //对是需要修改为关闭状态的做特殊处理： 给 工单记录表 更新 真实结束时间
                {
                    UpdateWorkOrderRealTimeCommand command = new UpdateWorkOrderRealTimeCommand()
                    {
                        UpdatedBy = _currentUser.UserName,
                        UpdatedOn = HymsonClock.Now(),
                        WorkOrderIds = updateWorkOrderRealEndList.ToArray()
                    };
                    await _planWorkOrderRepository.UpdatePlanWorkOrderRealEndByWorkOrderIdAsync(command);
                }

                //对是需要修改为关闭状态的做特殊处理： 取消掉 对应工单激活的信息
                if (deleteActivationWorkOrderIds.Any())
                {
                    await _planWorkOrderActivationRepository.DeletesTrueByWorkOrderIdsAsync(deleteActivationWorkOrderIds.ToArray());
                    if (planWorkOrderActivationRecordEntitys.Any())
                    {
                        await _planWorkOrderActivationRecordRepository.InsertsAsync(planWorkOrderActivationRecordEntitys);
                    }
                }

                await _planWorkOrderStatusRecordRepository.InsertsAsync(planWorkOrderStatusRecordEntities);

                ts.Complete();
            }
        }

        /// <summary>
        /// 修改工单是否锁定
        /// </summary>
        /// <param name="parms"></param>
        /// <returns></returns>
        public async Task ModifyWorkOrderLockedAsync(List<PlanWorkOrderLockedDto> parms)
        {
            if (parms == null || parms.Count == 0)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10100));
            }

            #region//判断订单是否可以继续修改为锁定/解锁  且组装数据
            //查询需要改变的工单
            var workOrders = await _planWorkOrderRepository.GetByIdsAsync(parms.Select(x => x.Id).ToArray());
            if (workOrders == null || workOrders.Count() == 0 || workOrders.Any(x => x.IsDeleted > 0) || workOrders.Count() != parms.Count())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16014));
            }

            List<UpdateLockedCommand> updateLockedCommands = new List<UpdateLockedCommand>();

            if (parms.First().IsLocked == YesOrNoEnum.Yes) //需要修改为锁定
            {
                if (workOrders.Any(x => x.Status != PlanWorkOrderStatusEnum.InProduction))//判断是否有不是生产中的则无法更改为锁定
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES16007));
                }

                foreach (var item in workOrders)
                {
                    updateLockedCommands.Add(new UpdateLockedCommand()
                    {
                        Id = item.Id,
                        Status = PlanWorkOrderStatusEnum.Pending,
                        LockedStatus = item.Status,

                        UpdatedBy = _currentUser.UserName,
                        UpdatedOn = HymsonClock.Now()
                    });
                }
            }
            else  //解锁操作
            {
                if (workOrders.Any(x => x.Status != PlanWorkOrderStatusEnum.Pending))//判断是否是锁定中
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES16008));
                }

                if (workOrders.Any(x => !x.LockedStatus.HasValue))//判断是否锁定前是否有值
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES16015));
                }

                foreach (var item in workOrders)
                {
                    updateLockedCommands.Add(new UpdateLockedCommand()
                    {
                        Id = item.Id,
                        Status = item.LockedStatus.Value,
                        LockedStatus = null,

                        UpdatedBy = _currentUser.UserName,
                        UpdatedOn = HymsonClock.Now()
                    });
                }
            }

            #endregion

            //组装工单状态变化记录
            List<PlanWorkOrderStatusRecordEntity> planWorkOrderStatusRecordEntities = new List<PlanWorkOrderStatusRecordEntity>();
            foreach (var item in workOrders)
            {
                var record = AutoMapperConfiguration.Mapper.Map<PlanWorkOrderStatusRecordEntity>(item);
                record.Id = IdGenProvider.Instance.CreateId();
                record.CreatedBy = _currentUser.UserName;
                record.UpdatedBy = _currentUser.UserName;
                record.CreatedOn = HymsonClock.Now();
                record.UpdatedOn = HymsonClock.Now();
                record.SiteId = _currentSite.SiteId ?? 123456;
                record.IsDeleted = 0;

                planWorkOrderStatusRecordEntities.Add(record);
            }

            using (TransactionScope ts = new TransactionScope())
            {
                var response = await _planWorkOrderRepository.ModifyWorkOrderLockedAsync(updateLockedCommands);

                await _planWorkOrderStatusRecordRepository.InsertsAsync(planWorkOrderStatusRecordEntities);//新增工单变化记录表

                if (response == parms.Count)
                {
                    ts.Complete();
                }
                else
                {
                    var errCode = parms.First().IsLocked == YesOrNoEnum.Yes ? ErrorCode.MES16009 : ErrorCode.MES16010;
                    throw new CustomerValidationException(nameof(errCode));
                }
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeletePlanWorkOrderAsync(long id)
        {
            await _planWorkOrderRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="idsArr"></param>
        /// <returns></returns>
        public async Task<int> DeletesPlanWorkOrderAsync(long[] idsArr)
        {
            //检查工单状态
            var workOrders = await _planWorkOrderRepository.GetByIdsAsync(idsArr);
            foreach (var item in workOrders)
            {
                if (item.Status != PlanWorkOrderStatusEnum.NotStarted)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES16013));
                }
            }

            return await _planWorkOrderRepository.DeletesAsync(new DeleteCommand { Ids = idsArr, DeleteOn = HymsonClock.Now(), UserId = _currentUser.UserName });
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<PlanWorkOrderListDetailViewDto>> GetPageListAsync(PlanWorkOrderPagedQueryDto pagedQueryDto)
        {
            var pagedQuery = pagedQueryDto.ToQuery<PlanWorkOrderPagedQuery>();
            pagedQuery.SiteId = _currentSite.SiteId ?? 123456;

            var planWorkOrderConversionEntities = await _planWorkOrderConversionRepository.GetListAsync(new() { });

            var pagedInfo = await _planWorkOrderRepository.GetPagedInfoAsync(pagedQuery);

            // 实体到DTO转换 装载数据
            var dtos = pagedInfo.Data.Select(s => s.ToModel<PlanWorkOrderListDetailViewDto>());

            foreach (var item in dtos)
            {
                var planWorkOrderConversionEntity = planWorkOrderConversionEntities.FirstOrDefault(a => a.PlanWorkOrderId == item.Id);

                if (planWorkOrderConversionEntity != null)
                {
                    item.ModuleConversion = planWorkOrderConversionEntity.ModuleConversion;
                    item.PackConversion = planWorkOrderConversionEntity.PackConversion;
                }

            }

            return new PagedInfo<PlanWorkOrderListDetailViewDto>(dtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 查询剩余可下单条码数量
        /// </summary>
        /// <param name="workOrderCode"></param>
        /// <returns></returns>
        public async Task<decimal> GetPlanWorkOrderByWorkOrderCodeAsync(string workOrderCode)
        {
            var query = new PlanWorkOrderQuery
            {
                OrderCode = workOrderCode,
                SiteId = _currentSite.SiteId ?? 123456
            };
            var workOrderEntity = await _planWorkOrderRepository.GetByCodeAsync(query)
                ?? throw new CustomerValidationException(nameof(ErrorCode.MES16003));

            // 应下达数量
            var residue = Math.Ceiling(workOrderEntity.Qty * (1 + workOrderEntity.OverScale / 100));

            // 查询已下发数量
            var workOrderRecordEntity = await _planWorkOrderRepository.GetByWorkOrderIdAsync(workOrderEntity.Id);
            if (workOrderRecordEntity != null && workOrderRecordEntity.PassDownQuantity.HasValue == true)
            {
                // 减掉已下达数量
                residue -= workOrderRecordEntity.PassDownQuantity.Value;
            }

            if (residue < 0) residue = 0;
            return residue;
        }

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<PlanWorkOrderDetailViewDto> QueryPlanWorkOrderByIdAsync(long id)
        {
            PlanWorkOrderDetailViewDto result = new();

            var planWorkOrderEntity = await _planWorkOrderRepository.GetByIdAsync(id);

            if (planWorkOrderEntity == null) return result;

            var planWorkOrderDetailView = planWorkOrderEntity.ToModel<PlanWorkOrderDetailViewDto>();

            //关联物料
            var material = await _procMaterialRepository.GetByIdAsync(planWorkOrderEntity.ProductId, planWorkOrderEntity.SiteId);
            if (material != null)
            {
                planWorkOrderDetailView.MaterialCode = material.MaterialCode;
                planWorkOrderDetailView.MaterialVersion = material.Version;
            }

            //关联BOM
            var bom = await _procBomRepository.GetByIdAsync(planWorkOrderEntity.ProductBOMId);
            if (bom != null)
            {
                planWorkOrderDetailView.BomCode = bom.BomCode;
                planWorkOrderDetailView.BomVersion = bom.Version;
            }

            //关联BOM
            var processRoute = await _procProcessRouteRepository.GetByIdAsync(planWorkOrderEntity.ProcessRouteId);
            if (processRoute != null)
            {
                planWorkOrderDetailView.ProcessRouteCode = processRoute.Code;
                planWorkOrderDetailView.ProcessRouteVersion = processRoute.Version;
            }

            //关联工作中心
            var workCenter = await _inteWorkCenterRepository.GetByIdAsync(planWorkOrderEntity.WorkCenterId ?? 0);
            if (workCenter != null)
            {
                planWorkOrderDetailView.WorkCenterCode = workCenter.Code;
            }

            var sfcBox = await _inteSFCBoxRepository.GetByWorkOrderAsync(id);
            if (sfcBox.Any())
            {
                planWorkOrderDetailView.SFCBox = sfcBox;
            }

            var planWorkOrderConversionEntity = await _planWorkOrderConversionRepository.GetOneAsync(new() { PlanWorkOrderId = id });
            if (planWorkOrderConversionEntity != null)
            {
                planWorkOrderDetailView.ModuleConversion = planWorkOrderConversionEntity.ModuleConversion;
                planWorkOrderDetailView.PackConversion = planWorkOrderConversionEntity.PackConversion;
            }

            result = planWorkOrderDetailView;

            return result;
        }

        /// <summary>
        /// 工单模糊查询
        /// </summary>
        /// <param name="workOrderCode"></param>
        /// <returns></returns>
        public async Task<IEnumerable<PlanWorkOrderDto>> QueryPlanWorkOrderByWorkOrderCodeAsync(string workOrderCode)
        {
            var query = new PlanWorkOrderQuery()
            {
                SiteId = _currentSite.SiteId ?? 123456,
                OrderCode = workOrderCode

            };
            var dtos = await _planWorkOrderRepository.GetByWorderOrderAsync(query);
            return dtos.Select(s => s.ToModel<PlanWorkOrderDto>());
        }

        /// <summary>
        /// 工单产量报表分页查询
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<PlanWorkOrderProductionReportViewDto>> GetPlanWorkOrderProductionReportPageListAsync(PlanWorkOrderProductionReportPagedQueryDto queryDto)
        {
            var pagedQuery = queryDto.ToQuery<PlanWorkOrderProductionReportPagedQuery>();
            pagedQuery.SiteId = _currentSite.SiteId ?? 123456;
            var pagedInfo = await _planWorkOrderRepository.GetPlanWorkOrderProductionReportPageListAsync(pagedQuery);
            var dtos = pagedInfo.Data.Select(s =>
            {
                if (s.FinishProductQuantity > 0)
                {
                    s.NoPassQty = s.UnqualifiedQuantity + s.NGQty;
                    //合格数量 完工数量-不良数量-NG数量
                    s.PassQty = s.FinishProductQuantity - s.NoPassQty;
                    //计算合格率 合格数量/完工数量 *100
                    s.PassRate = s.PassQty / s.FinishProductQuantity * 100;
                }
                return s.ToModel<PlanWorkOrderProductionReportViewDto>();
            });
            return new PagedInfo<PlanWorkOrderProductionReportViewDto>(dtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

    }
}
