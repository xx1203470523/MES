using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Plan;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Core.Domain.QualEnvOrder;
using Hymson.MES.Core.Domain.QualEnvOrderDetail;
using Hymson.MES.Core.Domain.Quality;
using Hymson.MES.Core.Enums;
using Hymson.MES.CoreServices.Bos.Quality;
using Hymson.MES.CoreServices.Dtos.Quality;
using Hymson.MES.CoreServices.Extension;
using Hymson.MES.CoreServices.Services.Manufacture.ManuGenerateBarcode;
using Hymson.MES.Data.Repositories.Integrated;
using Hymson.MES.Data.Repositories.Plan;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.QualEnvOrder;
using Hymson.MES.Data.Repositories.QualEnvOrderDetail;
using Hymson.MES.Data.Repositories.Quality;
using Hymson.MES.Data.Repositories.Quality.Query;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;

namespace Hymson.MES.CoreServices.Services.Quality
{
    public class EnvOrderCreateService : IEnvOrderCreateService
    {
        private readonly IQualEnvOrderRepository _qualEnvOrderRepository;
        private readonly IQualEnvOrderDetailRepository _qualEnvOrderDetailRepository;
        private readonly IQualEnvParameterGroupRepository _qualEnvParameterGroupRepository;
        private readonly IQualEnvParameterGroupDetailRepository _qualEnvParameterGroupDetailRepository;
        private readonly IQualEnvParameterGroupSnapshootRepository _qualEnvParameterGroupSnapshootRepository;
        private readonly IQualEnvParameterGroupDetailSnapshootRepository _qualEnvParameterGroupDetailSnapshootRepository;
        private readonly IPlanCalendarRepository _planCalendarRepository;
        private readonly IPlanCalendarDetailRepository _planCalendarDetailRepository;
        private readonly IPlanShiftRepository _planShiftRepository;
        private readonly IProcParameterRepository _procParameterRepository;
        private readonly IInteCodeRulesRepository _inteCodeRulesRepository;

        private readonly IManuGenerateBarcodeService _manuGenerateBarcodeService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="qualEnvOrderRepository"></param>
        /// <param name="qualEnvOrderDetailRepository"></param>
        /// <param name="qualEnvParameterGroupRepository"></param>
        /// <param name="qualEnvParameterGroupDetailRepository"></param>
        /// <param name="qualEnvParameterGroupSnapshootRepository"></param>
        /// <param name="qualEnvParameterGroupDetailSnapshootRepository"></param>
        /// <param name="planCalendarRepository"></param>
        /// <param name="planCalendarDetailRepository"></param>
        /// <param name="planShiftRepository"></param>
        /// <param name="procParameterRepository"></param>
        /// <param name="inteCodeRulesRepository"></param>
        /// <param name="manuGenerateBarcodeService"></param>
        public EnvOrderCreateService(IQualEnvOrderRepository qualEnvOrderRepository,
            IQualEnvOrderDetailRepository qualEnvOrderDetailRepository,
            IQualEnvParameterGroupRepository qualEnvParameterGroupRepository,
            IQualEnvParameterGroupDetailRepository qualEnvParameterGroupDetailRepository,
            IQualEnvParameterGroupSnapshootRepository qualEnvParameterGroupSnapshootRepository,
            IQualEnvParameterGroupDetailSnapshootRepository qualEnvParameterGroupDetailSnapshootRepository,
            IPlanCalendarRepository planCalendarRepository,
            IPlanCalendarDetailRepository planCalendarDetailRepository,
            IPlanShiftRepository planShiftRepository,
            IProcParameterRepository procParameterRepository,
            IInteCodeRulesRepository inteCodeRulesRepository,
            IManuGenerateBarcodeService manuGenerateBarcodeService)
        {
            _qualEnvOrderRepository = qualEnvOrderRepository;
            _qualEnvOrderDetailRepository = qualEnvOrderDetailRepository;
            _qualEnvParameterGroupRepository = qualEnvParameterGroupRepository;
            _qualEnvParameterGroupDetailRepository = qualEnvParameterGroupDetailRepository;
            _qualEnvParameterGroupSnapshootRepository = qualEnvParameterGroupSnapshootRepository;
            _qualEnvParameterGroupDetailSnapshootRepository = qualEnvParameterGroupDetailSnapshootRepository;
            _planCalendarRepository = planCalendarRepository;
            _planCalendarDetailRepository = planCalendarDetailRepository;
            _planShiftRepository = planShiftRepository;
            _procParameterRepository = procParameterRepository;
            _inteCodeRulesRepository = inteCodeRulesRepository;
            _manuGenerateBarcodeService = manuGenerateBarcodeService;
        }

        /// <summary>
        /// 手动生成
        /// </summary>
        /// <param name="bo"></param>
        /// <returns></returns>
        /// <exception cref="CustomerValidationException"></exception>
        public async Task<int> ManualCreateAsync(EnvOrderManualCreateBo bo)
        {
            if (bo == null) throw new CustomerValidationException(nameof(ErrorCode.MES10111));

            // 更新时间
            var updatedBy = bo.UserName;
            var updatedOn = HymsonClock.Now();

            //获取检验项目
            var parameterGroupEntities = await _qualEnvParameterGroupRepository.GetEntitiesAsync(new QualEnvParameterGroupQuery
            {
                SiteId = bo.SiteId,
                WorkCenterId = bo.WorkCenterId,
                ProcedureId = bo.ProcedureId,
                Status = Core.Enums.SysDataStatusEnum.Enable
            });
            if (parameterGroupEntities == null || !parameterGroupEntities.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES13610));
            }

            var parameterGroupEntity = parameterGroupEntities.First();

            //获取检验项目明细
            var parameterGroupDetails = await _qualEnvParameterGroupDetailRepository.GetEntitiesAsync(new QualEnvParameterGroupDetailQuery
            {
                ParameterGroupId = parameterGroupEntity.Id
            });
            if (parameterGroupDetails == null || !parameterGroupDetails.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES13611)).WithData("Code", parameterGroupEntity.Code);
            }
            if (parameterGroupDetails.Any(x => x.Frequency == null))
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES13612)).WithData("Code", parameterGroupEntity.Code);
            }

            //班制
            var shiftDetails = await GetCalenderShiftList(updatedOn, bo.SiteId);

            //标准参数
            var parameters = await _procParameterRepository.GetByIdsAsync(parameterGroupDetails.Select(x => x.ParameterId).Distinct());

            //组装检验单数据
            var param = await AssembleParamsAsync(new EnvOrderCreateReqDto
            {
                SiteId = bo.SiteId,
                UserName = updatedBy,
                OperateTime = updatedOn,
                ParameterGroupEntity = parameterGroupEntity,
                ParameterGroupDetails = parameterGroupDetails,
                ParameterEntities = parameters,
                ShiftDetails = shiftDetails
            });

            // 保存
            var rows = 0;
            using (var trans = TransactionHelper.GetTransactionScope())
            {
                rows += await _qualEnvOrderRepository.InsertAsync(param.EnvOrderEntity);
                rows += await _qualEnvOrderDetailRepository.InsertsAsync(param.EnvOrderDetailEntities.ToList());
                rows += await _qualEnvParameterGroupSnapshootRepository.InsertAsync(param.ParameterGroupSnapshootEntity);
                rows += await _qualEnvParameterGroupDetailSnapshootRepository.InsertRangeAsync(param.ParameterGroupDetailSnapshootEntities);

                trans.Complete();
            }

            return rows;
        }

        /// <summary>
        /// 自动生成(指定站点)
        /// </summary>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public async Task<int> AutoCreateAsync(long siteId)
        {
            var currentTime = HymsonClock.Now();
            var userName = "System";

            //班制
            var shiftDetails = await GetCalenderShiftList(currentTime, siteId);

            //获取检验项目
            var parameterGroupEntities = await _qualEnvParameterGroupRepository.GetEntitiesAsync(new QualEnvParameterGroupQuery
            {
                SiteId = siteId,
                Status = SysDataStatusEnum.Enable
            });
            if (parameterGroupEntities == null || !parameterGroupEntities.Any())
            {
                return 0;  //没有检验项目
            }

            //待写入数据
            var orderEntities = new List<QualEnvOrderEntity>();
            var orderDetailEntities = new List<QualEnvOrderDetailEntity>();
            var parameterGroupSnapshoots = new List<QualEnvParameterGroupSnapshootEntity>();
            var parameterGroupDetailSnapshoots = new List<QualEnvParameterGroupDetailSnapshootEntity>();

            foreach (var parameterGroupEntity in parameterGroupEntities)
            {
                //获取检验项目明细
                var parameterGroupDetails = await _qualEnvParameterGroupDetailRepository.GetEntitiesAsync(new QualEnvParameterGroupDetailQuery
                {
                    ParameterGroupId = parameterGroupEntity.Id
                });
                if (parameterGroupDetails == null || !parameterGroupDetails.Any() || parameterGroupDetails.Any(x => x.Frequency == null))
                {
                    continue;
                }

                //标准参数
                var parameters = await _procParameterRepository.GetByIdsAsync(parameterGroupDetails.Select(x => x.ParameterId).Distinct());

                //组装检验单数据
                var param = await AssembleParamsAsync(new EnvOrderCreateReqDto
                {
                    SiteId = siteId,
                    UserName = userName,
                    OperateTime = currentTime,
                    ParameterGroupEntity = parameterGroupEntity,
                    ParameterGroupDetails = parameterGroupDetails,
                    ParameterEntities = parameters,
                    ShiftDetails = shiftDetails
                });

                orderEntities.Add(param.EnvOrderEntity);
                orderDetailEntities.AddRange(param.EnvOrderDetailEntities);
                parameterGroupSnapshoots.Add(param.ParameterGroupSnapshootEntity);
                parameterGroupDetailSnapshoots.AddRange(param.ParameterGroupDetailSnapshootEntities);
            }

            // 保存
            var rows = 0;
            using (var trans = TransactionHelper.GetTransactionScope())
            {
                rows += await _qualEnvOrderRepository.InsertsAsync(orderEntities);
                rows += await _qualEnvOrderDetailRepository.InsertsAsync(orderDetailEntities);
                rows += await _qualEnvParameterGroupSnapshootRepository.InsertRangeAsync(parameterGroupSnapshoots);
                rows += await _qualEnvParameterGroupDetailSnapshootRepository.InsertRangeAsync(parameterGroupDetailSnapshoots);

                trans.Complete();
            }

            return rows;
        }

        /// <summary>
        /// 自动生成(所有站点)
        /// </summary>
        /// <returns></returns>
        public async Task<int> AutoCreateAsync()
        {
            var currentTime = HymsonClock.Now();
            var userName = "Auto";

            //所有站点班制
            var shiftDetailsDic = await GetCalenderShiftList(currentTime);
            if (!shiftDetailsDic.Any())
            {
                return 0;  //没有班制信息，无需生成
            }

            //获取检验项目
            var parameterGroupEntities = await _qualEnvParameterGroupRepository.GetEntitiesAsync(new QualEnvParameterGroupQuery
            {
                Status = SysDataStatusEnum.Enable
            });
            if (parameterGroupEntities == null || !parameterGroupEntities.Any())
            {
                return 0;  //没有检验项目，无需生成
            }

            //待写入数据
            var orderEntities = new List<QualEnvOrderEntity>();
            var orderDetailEntities = new List<QualEnvOrderDetailEntity>();
            var parameterGroupSnapshoots = new List<QualEnvParameterGroupSnapshootEntity>();
            var parameterGroupDetailSnapshoots = new List<QualEnvParameterGroupDetailSnapshootEntity>();

            foreach (var parameterGroupEntity in parameterGroupEntities)
            {
                if (!shiftDetailsDic.ContainsKey(parameterGroupEntity.SiteId))
                {
                    continue;
                }
                var shiftDetails = shiftDetailsDic[parameterGroupEntity.SiteId];

                //获取检验项目明细
                var parameterGroupDetails = await _qualEnvParameterGroupDetailRepository.GetEntitiesAsync(new QualEnvParameterGroupDetailQuery
                {
                    ParameterGroupId = parameterGroupEntity.Id
                });
                if (parameterGroupDetails == null || !parameterGroupDetails.Any() || parameterGroupDetails.Any(x => x.Frequency == null))
                {
                    continue;
                }

                //标准参数
                var parameters = await _procParameterRepository.GetByIdsAsync(parameterGroupDetails.Select(x => x.ParameterId).Distinct());

                //组装检验单数据
                var param = await AssembleParamsAsync(new EnvOrderCreateReqDto
                {
                    SiteId = parameterGroupEntity.SiteId,
                    UserName = userName,
                    OperateTime = currentTime,
                    ParameterGroupEntity = parameterGroupEntity,
                    ParameterGroupDetails = parameterGroupDetails,
                    ParameterEntities = parameters,
                    ShiftDetails = shiftDetails
                });

                orderEntities.Add(param.EnvOrderEntity);
                orderDetailEntities.AddRange(param.EnvOrderDetailEntities);
                parameterGroupSnapshoots.Add(param.ParameterGroupSnapshootEntity);
                parameterGroupDetailSnapshoots.AddRange(param.ParameterGroupDetailSnapshootEntities);
            }

            // 保存
            var rows = 0;
            using (var trans = TransactionHelper.GetTransactionScope())
            {
                rows += await _qualEnvOrderRepository.InsertsAsync(orderEntities);
                rows += await _qualEnvOrderDetailRepository.InsertsAsync(orderDetailEntities);
                rows += await _qualEnvParameterGroupSnapshootRepository.InsertRangeAsync(parameterGroupSnapshoots);
                rows += await _qualEnvParameterGroupDetailSnapshootRepository.InsertRangeAsync(parameterGroupDetailSnapshoots);

                trans.Complete();
            }

            return rows;
        }

        /// <summary>
        /// 组装参数
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        private async Task<EnvOrderCreateRspDto> AssembleParamsAsync(EnvOrderCreateReqDto dto)
        {
            //检验项目快照
            var parameterGroupSnapshoot = dto.ParameterGroupEntity.ToEntity<QualEnvParameterGroupSnapshootEntity>();
            parameterGroupSnapshoot.Id = IdGenProvider.Instance.CreateId();
            //检验项目明细快照
            var parameterGroupDetailSnapshoots = new List<QualEnvParameterGroupDetailSnapshootEntity>();
            foreach (var parameterGroupDetail in dto.ParameterGroupDetails)
            {
                var parameterEntity = dto.ParameterEntities.First(x => x.Id == parameterGroupDetail.ParameterId);

                var parameterGroupDetailSnapshoot = parameterGroupDetail.ToEntity<QualEnvParameterGroupDetailSnapshootEntity>();
                parameterGroupDetailSnapshoot.Id = IdGenProvider.Instance.CreateId();
                parameterGroupDetailSnapshoot.ParameterGroupId = parameterGroupSnapshoot.Id;
                parameterGroupDetailSnapshoot.ParameterCode = parameterEntity.ParameterCode;
                parameterGroupDetailSnapshoot.ParameterName = parameterEntity.ParameterName;
                parameterGroupDetailSnapshoot.ParameterDataType = parameterEntity.DataType;
                parameterGroupDetailSnapshoot.ParameterUnit = parameterEntity.ParameterUnit ?? "";
                parameterGroupDetailSnapshoots.Add(parameterGroupDetailSnapshoot);
            }
            //检验单
            var orderEntity = new QualEnvOrderEntity
            {
                Id = IdGenProvider.Instance.CreateId(),
                SiteId = dto.SiteId,
                InspectionOrder = await GenerateEnvOrderCodeAsync(dto.SiteId, dto.UserName),
                GroupSnapshootId = parameterGroupSnapshoot.Id,
                WorkCenterId = dto.ParameterGroupEntity.WorkCenterId,
                ProcedureId = dto.ParameterGroupEntity.ProcedureId,
                CreatedBy = dto.UserName,
                CreatedOn = dto.OperateTime,
                UpdatedBy = dto.UserName,
                UpdatedOn = dto.OperateTime
            };
            //检验单明细
            var orderDetails = new List<QualEnvOrderDetailEntity>();
            foreach (var item in dto.ParameterGroupDetails)
            {
                //班次
                if (item.Frequency == Core.Enums.FrequencyEnum.Classes)
                {
                    foreach (var shift in dto.ShiftDetails)
                    {
                        var startTime = (dto.OperateTime.ToString("yyyy-MM-dd ") + shift.StartTime).ParseToDateTime();

                        orderDetails.Add(new QualEnvOrderDetailEntity
                        {
                            Id = IdGenProvider.Instance.CreateId(),
                            SiteId = dto.SiteId,
                            EnvOrderId = orderEntity.Id,
                            GroupDetailSnapshootId = parameterGroupDetailSnapshoots.Where(x => x.ParameterId == item.ParameterId).Select(x => x.Id).FirstOrDefault(),
                            StartTime = startTime,
                            EndTime = startTime.AddMinutes(30),
                            CreatedBy = dto.UserName,
                            CreatedOn = dto.OperateTime,
                            UpdatedBy = dto.UserName,
                            UpdatedOn = dto.OperateTime
                        });
                    }
                }
                //天
                else if (item.Frequency == Core.Enums.FrequencyEnum.Day)
                {
                    var startTime = (dto.OperateTime.ToString("yyyy-MM-dd ") + dto.ShiftDetails.Select(x => x.StartTime).Min()).ParseToDateTime();

                    orderDetails.Add(new QualEnvOrderDetailEntity
                    {
                        Id = IdGenProvider.Instance.CreateId(),
                        SiteId = dto.SiteId,
                        EnvOrderId = orderEntity.Id,
                        GroupDetailSnapshootId = parameterGroupDetailSnapshoots.Where(x => x.ParameterId == item.ParameterId).Select(x => x.Id).FirstOrDefault(),
                        StartTime = startTime,
                        EndTime = startTime.AddMinutes(30),
                        CreatedBy = dto.UserName,
                        CreatedOn = dto.OperateTime,
                        UpdatedBy = dto.UserName,
                        UpdatedOn = dto.OperateTime
                    });
                }
                //小时
                else
                {
                    var interval = (int)item.Frequency.GetValueOrDefault();  //间隔
                    var count = 24 / interval; //生成数量
                    var startTime = (dto.OperateTime.ToString("yyyy-MM-dd ") + dto.ShiftDetails.Select(x => x.StartTime).Min()).ParseToDateTime();
                    for (int i = 0; i < count; i++)
                    {
                        orderDetails.Add(new QualEnvOrderDetailEntity
                        {
                            Id = IdGenProvider.Instance.CreateId(),
                            SiteId = dto.SiteId,
                            EnvOrderId = orderEntity.Id,
                            GroupDetailSnapshootId = parameterGroupDetailSnapshoots.Where(x => x.ParameterId == item.ParameterId).Select(x => x.Id).FirstOrDefault(),
                            StartTime = startTime,
                            EndTime = startTime.AddMinutes(30),
                            CreatedBy = dto.UserName,
                            CreatedOn = dto.OperateTime,
                            UpdatedBy = dto.UserName,
                            UpdatedOn = dto.OperateTime
                        });

                        startTime = startTime.AddHours(interval);
                    }
                }
            }

            return new EnvOrderCreateRspDto
            {
                EnvOrderEntity = orderEntity,
                EnvOrderDetailEntities = orderDetails,
                ParameterGroupSnapshootEntity = parameterGroupSnapshoot,
                ParameterGroupDetailSnapshootEntities = parameterGroupDetailSnapshoots
            };
        }

        /// <summary>
        /// 获取工作日历班制信息
        /// </summary>
        /// <param name="date"></param>
        /// <param name="siteId"></param>
        /// <returns></returns>
        /// <exception cref="CustomerValidationException"></exception>
        private async Task<IEnumerable<PlanShiftDetailEntity>> GetCalenderShiftList(DateTime date, long siteId)
        {
            //日历
            var calendarEntity = await _planCalendarRepository.GetOneAsync(new PlanCalendarQuery
            {
                SiteId = siteId,
                Year = date.Year,
                Month = date.Month - 1,
                Status = Core.Enums.YesOrNoEnum.Yes
            });
            if (calendarEntity == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES13613)).WithData("Year", date.Year).WithData("Month", date.Month);
            }
            //日历详情
            var calendarDetails = await _planCalendarDetailRepository.GetListAsync(new PlanCalendarDetailQuery
            {
                PlanCalendarId = calendarEntity.Id
            });
            if (calendarDetails == null || !calendarDetails.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES13614));
            }
            var calendarDetail = calendarDetails.FirstOrDefault(x => x.Day == date.Day);
            if (calendarDetail == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES13614));
            }
            //班制
            var shiftEntity = await _planShiftRepository.GetByIdAsync(calendarDetail.ShiftId.GetValueOrDefault());
            if (shiftEntity == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES13615));
            }
            //班制详情
            var shiftDetails = await _planShiftRepository.GetByMainIdAsync(shiftEntity.Id);
            if (shiftDetails == null || !shiftDetails.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES13616));
            }

            return shiftDetails;
        }

        /// <summary>
        /// 获取所有站点指定日期班制信息
        /// </summary>
        /// <param name="date"></param>
        /// <returns>返回值为空集合代表没有配置班制信息或当前日期为休息日</returns>
        private async Task<Dictionary<long, IEnumerable<PlanShiftDetailEntity>>> GetCalenderShiftList(DateTime date)
        {
            var shiftDetailsDic = new Dictionary<long, IEnumerable<PlanShiftDetailEntity>>();

            //日历
            var calendarEntities = await _planCalendarRepository.GetListAsync(new PlanCalendarQuery
            {
                Year = date.Year,
                Month = date.Month,
                Status = Core.Enums.YesOrNoEnum.Yes
            });
            if (calendarEntities == null || !calendarEntities.Any())
            {
                return shiftDetailsDic;
            }
            //日历详情
            var calendarDetails = await _planCalendarDetailRepository.GetListAsync(new PlanCalendarDetailQuery
            {
                PlanCalendarIds = calendarEntities.Select(x => x.Id),

            });
            if (calendarDetails == null || !calendarDetails.Any(x => x.Day == date.Day))
            {
                return shiftDetailsDic;
            }
            var calendarDetailIds = calendarDetails.Where(x => x.Day == date.Day).Select(x => x.ShiftId.GetValueOrDefault());
            //班制
            var shiftEntities = await _planShiftRepository.GetByIdsAsync(calendarDetailIds.ToArray());
            if (shiftEntities == null || !shiftEntities.Any())
            {
                return shiftDetailsDic;
            }
            foreach (var shiftEntity in shiftEntities)
            {
                //班制详情
                var shiftDetails = await _planShiftRepository.GetByMainIdAsync(shiftEntity.Id);
                if (shiftDetails == null || !shiftDetails.Any())
                {
                    continue;
                }
                shiftDetailsDic.Add(shiftEntity.SiteId, shiftDetails);
            }

            return shiftDetailsDic;
        }

        /// <summary>
        /// 检验单号生成
        /// </summary>
        /// <param name="bo"></param>
        /// <returns></returns>
        /// <exception cref="CustomerValidationException"></exception>
        private async Task<string> GenerateEnvOrderCodeAsync(long siteId, string userName)
        {
            var codeRules = await _inteCodeRulesRepository.GetListAsync(new InteCodeRulesReQuery
            {
                SiteId = siteId,
                CodeType = Core.Enums.Integrated.CodeRuleCodeTypeEnum.OQC
            });
            if (codeRules == null || !codeRules.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES13617));
            }
            if (codeRules.Count() > 1)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES13618));
            }

            var orderCodes = await _manuGenerateBarcodeService.GenerateBarcodeListByIdAsync(new Bos.Manufacture.ManuGenerateBarcode.GenerateBarcodeBo
            {
                SiteId = siteId,
                UserName = userName,
                CodeRuleId = codeRules.First().Id,
                Count = 1
            });

            return orderCodes.First();
        }
    }
}
