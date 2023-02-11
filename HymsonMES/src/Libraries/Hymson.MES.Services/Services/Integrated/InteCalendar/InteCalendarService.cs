using Hymson.Infrastructure;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Domain.Integrated;
using Hymson.MES.Core.Enums.Integrated;
using Hymson.MES.Data.Repositories.Equipment.EquEquipment;
using Hymson.MES.Data.Repositories.Integrated.InteCalendar;
using Hymson.MES.Data.Repositories.Integrated.InteCalendar.Query;
using Hymson.MES.Data.Repositories.Integrated.InteClass;
using Hymson.MES.Services.Dtos.Integrated;
using Hymson.Snowflake;
using Hymson.Utils.Extensions;

namespace Hymson.MES.Services.Services.Integrated.InteCalendar
{
    /// <summary>
    /// 日历维护 服务
    /// </summary>
    public class InteCalendarService : IInteCalendarService
    {
        /// <summary>
        /// 仓储（日历）
        /// </summary>
        private readonly IInteCalendarRepository _inteCalendarRepository;

        /// <summary>
        /// 仓储（日历关联时间）
        /// </summary>
        private readonly IInteCalendarDateRepository _inteCalendarDateRepository;

        /// <summary>
        /// 仓储（日历时间详情）
        /// </summary>
        private readonly IInteCalendarDateDetailRepository _inteCalendarDateDetailRepository;

        /// <summary>
        /// 仓储（生产班制）
        /// </summary>
        private readonly IInteClassRepository _inteClassRepository;

        /// <summary>
        /// 仓储（设备注册）
        /// </summary>
        private readonly IEquEquipmentRepository _equEquipmentRepository;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inteCalendarRepository"></param>
        /// <param name="inteCalendarDateRepository"></param>
        /// <param name="inteCalendarDateDetailRepository"></param>
        /// <param name="inteClassRepository"></param>
        /// <param name="equEquipmentRepository"></param>
        public InteCalendarService(IInteCalendarRepository inteCalendarRepository,
            IInteCalendarDateRepository inteCalendarDateRepository,
            IInteCalendarDateDetailRepository inteCalendarDateDetailRepository,
            IInteClassRepository inteClassRepository,
            IEquEquipmentRepository equEquipmentRepository)
        {
            _inteCalendarRepository = inteCalendarRepository;
            _inteCalendarDateRepository = inteCalendarDateRepository;
            _inteCalendarDateDetailRepository = inteCalendarDateDetailRepository;
            _inteClassRepository = inteClassRepository;
            _equEquipmentRepository = equEquipmentRepository;
        }


        /// <summary>
        /// 添加（日历）
        /// </summary>
        /// <param name="createDto"></param>
        /// <returns></returns>
        public async Task<int> AddInteCalendarAsync(InteCalendarCreateDto createDto)
        {
            // 验证DTO

            // DTO转换实体
            var entity = createDto.ToEntity<InteCalendarEntity>();
            entity.Id = IdGenProvider.Instance.CreateId();
            entity.CreatedBy = "TODO";
            entity.UpdatedBy = "TODO";


            #region 参数校验
            if (createDto.UseStatus == true)
            {
                var isExists = await _inteCalendarRepository.IsExistsAsync(entity.EquOrLineId);
                if (isExists == true)
                {
                    // TODO 错误码
                    return 0;

                    // 判断同一线体、设备只能有一个日历
                    //var msg = "";
                    //if (entity.CalendarType == ((int)CalendarTypeEnum.Equipment).ToString())
                    //{
                    //    msg = "设备";
                    //}
                    //else
                    //{
                    //    msg = "线体";
                    //}

                    //responseDto.Msg = $"选择的{msg}在系统已经有启用的日历存在！";
                    //return responseDto;
                }
            }

            #endregion

            var resType = 0;
            if (!string.IsNullOrEmpty(createDto.Weekdays))
            {
                var weekdays = createDto.Weekdays.Split(',');
                foreach (var day in weekdays)
                {
                    resType += day.ParseToInt();
                }
            }

            List<InteCalendarDateEntity> inteCalendarDates = new();
            List<InteCalendarDateDetailEntity> dateDetails = new();
            if (!string.IsNullOrWhiteSpace(createDto.Month))
            {
                var monthArr = createDto.Month.Split(',');
                foreach (var month in monthArr)
                {
                    inteCalendarDates.Add(new InteCalendarDateEntity
                    {
                        CalendarId = entity.Id,
                        Year = createDto.Year,
                        Month = month,
                        ClassId = createDto.ClassId ?? 0,
                        RestType = resType,

                        // TODO 这里需要替换
                        //CreateBy = App.GetName(),
                        //UpdateBy = App.GetName(),
                        //SiteCode = App.GetSite()
                    });
                }

                //按选择的年月生成日历信息
                dateDetails = GenerateDateDetailByCreateDto(entity.Id, createDto);
            }

            // TODO 事务处理
            var rows = 0;

            rows += await _inteCalendarRepository.InsertAsync(entity);
            rows += await _inteCalendarDateRepository.InsertRangeAsync(inteCalendarDates);
            rows += await _inteCalendarDateDetailRepository.InsertRangeAsync(dateDetails);

            return rows;
        }

        /// <summary>
        /// 更新（日历）
        /// </summary>
        /// <param name="modifyDto"></param>
        /// <returns></returns>
        public async Task<int> UpdateInteCalendarAsync(InteCalendarModifyDto modifyDto)
        {
            // DTO转换实体
            var entity = modifyDto.ToEntity<InteCalendarEntity>();
            entity.UpdatedBy = "TODO";

            entity.UseStatus = modifyDto.UseStatus == true ? (byte)CalendarUseStatusEnum.Enable : (byte)CalendarUseStatusEnum.NotEnabled;

            #region 参数校验
            //// 判断是否有获取到站点码
            //if (entity.SiteCode.IsNotEmpty() == false)
            //{
            //    responseDto.Msg = "站点码获取失败，请重新登录！";
            //    return responseDto;
            //}

            var calendar = await _inteCalendarRepository.GetByIdAsync(entity.Id);
            if (calendar == null)
            {
                // TODO 错误码
                return 0;

                //responseDto.Msg = "此日历不存在！";
                //return responseDto;
            }

            var enable = (int)CalendarUseStatusEnum.Enable;
            if (calendar.UseStatus == enable && modifyDto.UseStatus == true)
            {
                // TODO 错误码
                return 0;

                //responseDto.Msg = "启用的日历不能修改！";
                //return responseDto;
            }



            if (modifyDto.UseStatus == true)
            {
                var isExists = await _inteCalendarRepository.IsExistsAsync(entity.EquOrLineId, modifyDto.Id);
                if (isExists == true)
                {
                    // TODO 错误码
                    return 0;

                    //// 判断同一线体、设备只能有一个日历
                    //var msg = "";
                    //if (entity?.CalendarType == ((int)CalendarTypeEnum.Equipment).ToString())
                    //{
                    //    msg = "设备";
                    //}
                    //else
                    //{
                    //    msg = "线体";
                    //}

                    //responseDto.Msg = $"选择的{msg}在系统已经有启用的日历存在！";
                    //return responseDto;
                }
            }
            #endregion

            var resType = 0;
            if (string.IsNullOrEmpty(modifyDto.Weekdays) == false)
            {
                var weekdays = modifyDto.Weekdays.Split(',');
                foreach (var day in weekdays)
                {
                    resType += day.ParseToInt();
                }
            }

            List<InteCalendarDateEntity> inteCalendarDates = new();
            List<InteCalendarDateDetailEntity> dateDetails = new();
            if (string.IsNullOrWhiteSpace(modifyDto.Month) == false)
            {
                var monthArr = modifyDto.Month.Split(',');
                foreach (var month in monthArr)
                {
                    inteCalendarDates.Add(new InteCalendarDateEntity
                    {
                        CalendarId = entity.Id,
                        Year = modifyDto.Year,
                        Month = month,
                        ClassId = modifyDto.ClassId ?? 0,
                        RestType = resType,

                        // TODO 这里需要替换
                        //CreateBy = App.GetName(),
                        //UpdateBy = App.GetName(),
                        //SiteCode = App.GetSite()
                    });
                }

                dateDetails = GenerateDateDetailByModifyDto(entity.Id, modifyDto);
            }

            // TODO 事务处理
            var rows = 0;

            rows += await _inteCalendarRepository.UpdateAsync(entity);

            rows += await _inteCalendarDateRepository.DeleteByCalendarIdAsync(entity.Id);
            rows += await _inteCalendarDateRepository.InsertRangeAsync(inteCalendarDates);

            rows += await _inteCalendarDateDetailRepository.DeleteByCalendarIdAsync(entity.Id);
            rows += await _inteCalendarDateDetailRepository.InsertRangeAsync(dateDetails);

            return rows;
        }

        /// <summary>
        /// 删除（日历）
        /// </summary>
        /// <param name="idsArr"></param>
        /// <returns></returns>
        public async Task<int> DeleteInteCalendarAsync(long[] idsArr)
        {
            // TODO 事务处理
            var rows = 0;

            rows += await _inteCalendarDateDetailRepository.DeleteByCalendarIdsAsync(idsArr);
            rows += await _inteCalendarDateRepository.DeleteByCalendarIdsAsync(idsArr);
            rows += await _inteCalendarRepository.DeletesAsync(idsArr);

            return rows;
        }

        /// <summary>
        /// 查询列表（日历）
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<InteCalendarDto>> GetPagedListAsync(InteCalendarPagedQueryDto pagedQueryDto)
        {
            var pagedQuery = pagedQueryDto.ToQuery<InteCalendarPagedQuery>();
            var pagedInfo = await _inteCalendarRepository.GetPagedListAsync(pagedQuery);

            // 实体到DTO转换 装载数据
            var dtos = pagedInfo.Data.Select(s => s.ToModel<InteCalendarDto>());
            return new PagedInfo<InteCalendarDto>(dtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);

            /*
            parm.SiteCode = App.GetSite();
            var predicate = Expressionable.Create<InteCalendar, InteWorkCenter, EquEquipment>();
            predicate = predicate.And((a, b, c) => a.SiteCode == App.GetSite());
            predicate = predicate.AndIF(!string.IsNullOrEmpty(parm.CalendarName), (a, b, c) => a.CalendarName.Contains(parm.CalendarName));
            predicate = predicate.AndIF(!string.IsNullOrEmpty(parm.CalendarType), (a, b, c) => a.CalendarType.Contains(parm.CalendarType));
            predicate = predicate.AndIF(!string.IsNullOrEmpty(parm.Code), (a, b, c) => b.Code.Contains(parm.Code) || c.EquipmentCode.Contains(parm.Code));
            predicate = predicate.AndIF(!string.IsNullOrEmpty(parm.Name), (a, b, c) => b.Name.Contains(parm.Name) || c.EquipmentName.Contains(parm.Name));
            predicate = predicate.AndIF(parm.UseStatus != -1, (a, b, c) => a.UseStatus == parm.UseStatus);

            var calendarType = ((int)EnumCalendarType.Equipment).ToString();
            var response = await _inteCalendarRepository.Queryable()
                          .LeftJoin<InteWorkCenter>((a, b) => a.EquOrLineId == b.Id)
                          .LeftJoin<EquEquipment>((a, b, c) => a.EquOrLineId == c.Id)
                       .Where(predicate.ToExpression())
                        .Select((a, b, c) => new InteCalendarDto
                        {
                            Id = a.Id,
                            SiteCode = a.SiteCode,
                            CalendarName = a.CalendarName,
                            CalendarType = a.CalendarType,
                            Remark = a.Remark,
                            Code = a.CalendarType == calendarType ? c.EquipmentCode : b.Code,
                            Name = a.CalendarType == calendarType ? c.EquipmentName : b.Name,
                            UseStatus = a.UseStatus,
                            CreateBy = a.CreateBy,
                            UpdateBy = a.UpdateBy,
                            UpdateOn = a.UpdateOn,
                            CreateOn = a.CreateOn
                        })
                       .ToPageAsync(parm);

            return response;
            */
        }

        /// <summary>
        /// 查询详情（日历）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<InteCalendarDetailDto> GetInteCalendarAsync(long id)
        {
            var calendar = await _inteCalendarRepository.GetByIdAsync(id);
            if (calendar == null)
            {
                // TODO 错误码
                return null;
            }

            var calendarDates = await _inteCalendarDateRepository.GetEntitiesAsync(calendar.Id);
            var calendarDateDetails = await _inteCalendarDateDetailRepository.GetEntitiesAsync(calendar.Id);

            var model = new InteCalendarDetailDto
            {
                Id = calendar.Id,
                CalendarName = calendar.CalendarName,
                CalendarType = calendar.CalendarType,
                EquOrLineId = calendar.EquOrLineId,
                Remark = calendar.Remark,
                UseStatus = calendar.UseStatus == (int)CalendarUseStatusEnum.Enable,
                CalendarDataList = new List<QueryInteCalendarDetailDto>()
            };

            // 设备
            if (calendar.CalendarType == ((int)CalendarTypeEnum.Equipment).ToString())
            {
                var equ = await _equEquipmentRepository.GetByIdAsync(model.EquOrLineId);
                if (equ != null)
                {
                    model.Code = equ.EquipmentCode;
                    model.Name = equ.EquipmentName;
                }
            }

            // 线体
            if (calendar.CalendarType == ((int)CalendarTypeEnum.WorkCenter).ToString())
            {
                // TODO 工作中心
                /*
                var workCenter = await _inteWorkCenterRepository.GetByIdAsync(model.EquOrLineId);
                if (workCenter != null)
                {
                    model.Code = workCenter.Code;
                    model.Name = workCenter.Name;
                }
                */
            }

            // 日历详情
            if (calendarDates != null && calendarDates.Any() == true)
            {
                var first = calendarDates.FirstOrDefault();

                var month = calendarDates.Select(a => a.Month).ToArray();
                model.Year = first.Year;
                model.ClassId = first.ClassId;
                if (model.ClassId > 0)
                {
                    var inteClass = await _inteClassRepository.GetByIdAsync(model.ClassId.Value);
                    model.ClassName = inteClass.ClassName;
                    model.ClassRemark = inteClass.Remark;
                }
                model.Smonth = month;
                if (first.RestType > 0)
                {
                    //十进制转化为二进制
                    var weekDay = ConvertDecimalToBinary(first.RestType);
                    model.WeekDay = weekDay;
                }
            }

            // 日历时间详情
            if (calendarDateDetails != null && calendarDateDetails.Any() == true)
            {
                foreach (var item in calendarDateDetails)
                {
                    model.CalendarDataList.Add(new QueryInteCalendarDetailDto
                    {
                        Day = item.Day.Value.ToShortDateString(), //item.Day.ToString("yyyy-MM-dd"),
                        RestType = item.RestType.ToString(),
                        ClassId = item.ClassId.ToString()
                    });
                }
            }

            return model;
        }




        /// <summary>
        /// 十进制转二进制
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        private static string[] ConvertDecimalToBinary(int number)
        {
            int[] a = new int[7];
            for (int i = 0; i < 7; i++)
            {
                a[i] = number % 2;
                number = number / 2;
            }
            var strs = new List<string>();
            if (a[0] == 1)
                strs.Add("1");
            if (a[1] == 1)
                strs.Add("2");
            if (a[2] == 1)
                strs.Add("4");
            if (a[3] == 1)
                strs.Add("8");
            if (a[4] == 1)
                strs.Add("16");
            if (a[5] == 1)
                strs.Add("32");
            if (a[6] == 1)
                strs.Add("64");
            return strs.ToArray();
        }

        /// <summary>
        /// 根据选择的年月自动生成日历
        /// </summary>
        private static List<InteCalendarDateDetailEntity> GenerateDateDetailByCreateDto(long calendarId, InteCalendarCreateDto createDto)
        {
            var monthArr = createDto.Month.Split(',');
            var weekDays = new[] { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday" };
            var weekends = new[] { "Saturday", "Sunday" };
            var day = DateTime.MinValue;
            int days = 0;
            var dateDetails = new List<InteCalendarDateDetailEntity>();
            foreach (var month in monthArr)
            {
                //哪年哪月有多少天，周几，按默认值存进去，把编辑了的数据再替换掉
                days = DateTime.DaysInMonth(Convert.ToInt32(createDto.Year), Convert.ToInt32(month));
                for (int a = 1; a <= days; a++)
                {
                    if (a < 10)
                    {
                        day = DateTime.Parse(createDto.Year + "-" + month + "-0" + a);
                    }
                    else
                    {
                        day = DateTime.Parse(createDto.Year + "-" + month + "-" + a);
                    }

                    var week = day.DayOfWeek.ToString();
                    //如果是工作日
                    var resType = 2;
                    long classId = 0;
                    if (weekDays.Contains(week))
                    {
                        resType = 1;
                        classId = createDto.ClassId ?? 0;
                    }
                    dateDetails.Add(new InteCalendarDateDetailEntity
                    {
                        CalendarId = calendarId,
                        Day = day,
                        ClassId = classId,
                        RestType = resType

                        // TODO 这里需要替换
                        //CreateBy = App.GetName(),
                        //UpdateBy = App.GetName(),
                        //SiteCode = App.GetSite()
                    });
                }
            }

            //编辑过的日历
            if (createDto.CalendarDataList != null && createDto.CalendarDataList.Any())
            {
                var details = createDto.CalendarDataList.OrderBy(a => a.Day);
                foreach (var item in details)
                {
                    // TODO 原句是  var calendarData = dateDetails.FirstOrDefault(a => a.Day.ToString("yyyy-MM-dd") == item.Day.ToString("yyyy-MM-dd"));
                    var calendarData = dateDetails.FirstOrDefault(a => a.Day.Value.ToShortDateString() == item.Day.ToShortDateString());
                    if (calendarData != null)
                    {
                        calendarData.ClassId = string.IsNullOrWhiteSpace(item.ClassId) == true ? 0 : item.ClassId.ParseToLong(0);
                        calendarData.RestType = item.RestType ?? 0;
                    }
                }
            }
            return dateDetails;
        }

        /// <summary>
        /// 根据选择的年月自动生成日历
        /// </summary>
        private static List<InteCalendarDateDetailEntity> GenerateDateDetailByModifyDto(long calendarId, InteCalendarModifyDto modifyDto)
        {
            var monthArr = modifyDto.Month.Split(',');
            var weekDays = new[] { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday" };
            var weekends = new[] { "Saturday", "Sunday" };
            var day = DateTime.MinValue;
            int days = 0;
            var dateDetails = new List<InteCalendarDateDetailEntity>();
            foreach (var month in monthArr)
            {
                //哪年哪月有多少天，周几，按默认值存进去，把编辑了的数据再替换掉
                days = DateTime.DaysInMonth(Convert.ToInt32(modifyDto.Year), Convert.ToInt32(month));
                for (int a = 1; a <= days; a++)
                {
                    if (a < 10)
                    {
                        day = DateTime.Parse(modifyDto.Year + "-" + month + "-0" + a);
                    }
                    else
                    {
                        day = DateTime.Parse(modifyDto.Year + "-" + month + "-" + a);
                    }

                    var week = day.DayOfWeek.ToString();
                    //如果是工作日
                    var resType = 2;
                    long classId = 0;
                    if (weekDays.Contains(week))
                    {
                        resType = 1;
                        classId = modifyDto.ClassId ?? 0;
                    }
                    dateDetails.Add(new InteCalendarDateDetailEntity
                    {
                        CalendarId = calendarId,
                        Day = day,
                        ClassId = classId,
                        RestType = resType

                        // TODO 这里需要替换
                        //CreateBy = App.GetName(),
                        //UpdateBy = App.GetName(),
                        //SiteCode = App.GetSite()
                    });
                }
            }

            // 编辑过的日历
            if (modifyDto.CalendarDataList != null && modifyDto.CalendarDataList.Any())
            {
                var details = modifyDto.CalendarDataList.OrderBy(a => a.Day);
                foreach (var item in details)
                {
                    // TODO 原句是  var calendarData = dateDetails.FirstOrDefault(a => a.Day.ToString("yyyy-MM-dd") == item.Day.ToString("yyyy-MM-dd"));
                    var calendarData = dateDetails.FirstOrDefault(a => a.Day.Value.ToShortDateString() == item.Day.ToShortDateString());
                    if (calendarData != null)
                    {
                        calendarData.ClassId = string.IsNullOrWhiteSpace(item.ClassId) == true ? 0 : item.ClassId.ParseToLong(0);
                        calendarData.RestType = item.RestType ?? 0;
                    }
                }
            }
            return dateDetails;
        }


    }
}
