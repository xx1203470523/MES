using Hymson.Authentication;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Domain.Integrated;
using Hymson.MES.Data.Repositories.Integrated.InteClass;
using Hymson.MES.Data.Repositories.Integrated.InteClass.Query;
using Hymson.MES.Services.Dtos.Integrated;
using Hymson.Snowflake;
using Hymson.Utils.Tools;
using System.Diagnostics;
using System.Transactions;

namespace Hymson.MES.Services.Services.Integrated.InteClass
{
    /// <summary>
    /// 班制维护 服务
    /// </summary>
    public class InteClassService : IInteClassService
    {
        /// <summary>
        /// 当前登录用户对象
        /// </summary>
        private readonly ICurrentUser _currentUser;

        /// <summary>
        /// 仓储（班制维护）
        /// </summary>
        private readonly IInteClassRepository _inteClassRepository;

        /// <summary>
        /// 仓储（班制维护明细）
        /// </summary>
        private readonly IInteClassDetailRepository _inteClassDetailRepository;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="inteClassRepository"></param>
        /// <param name="inteClassDetailRepository"></param>
        public InteClassService(ICurrentUser currentUser,
            IInteClassRepository inteClassRepository,
            IInteClassDetailRepository inteClassDetailRepository)
        {
            _currentUser = currentUser;
            _inteClassRepository = inteClassRepository;
            _inteClassDetailRepository = inteClassDetailRepository;
        }


        /// <summary>
        /// 添加班制维护详情
        /// </summary>
        /// <param name="createDto"></param>
        /// <returns></returns>
        public async Task<int> CreateAsync(InteClassCreateDto createDto)
        {
            // 验证DTO


            // DTO转换实体
            var entity = createDto.ToEntity<InteClassEntity>();
            entity.Id = IdGenProvider.Instance.CreateId();
            entity.CreatedBy = _currentUser.UserName;
            entity.UpdatedBy = _currentUser.UserName;

            // 参数校验
            if (createDto.DetailList.Any(a => TimeComparison(a.StartTime, a.EndTime) == false) == true)
            {
                // TODO
                //return Error(ResultCode.PARAM_ERROR, "开始时间不能大于于结束时间");
            }

            List<InteClassDetailEntity> details = new();
            foreach (var item in createDto.DetailList)
            {
                var classDetailEntity = item.ToEntity<InteClassDetailEntity>();
                classDetailEntity.ClassId = entity.Id;
                classDetailEntity.CreatedBy = entity.CreatedBy;
                classDetailEntity.UpdatedBy = entity.UpdatedBy;
                details.Add(classDetailEntity);
            }

            var rows = 0;
            using (var trans = TransactionHelper.GetTransactionScope())
            {
                rows += await _inteClassRepository.InsertAsync(entity);
                rows += await _inteClassDetailRepository.InsertRangeAsync(details);
                trans.Complete();
            }
            return rows;
        }

        /// <summary>
        /// 更新班制维护详情
        /// </summary>
        /// <param name="modifyDto"></param>
        /// <returns></returns>
        public async Task<int> ModifyAsync(InteClassModifyDto modifyDto)
        {
            // DTO转换实体
            var entity = modifyDto.ToEntity<InteClassEntity>();
            entity.UpdatedBy = _currentUser.UserName;

            List<InteClassDetailEntity> details = new();
            foreach (var item in modifyDto.DetailList)
            {
                if (TimeComparison(item.StartTime, item.EndTime) == false)
                {
                    // TODO
                    //return Error(ResultCode.PARAM_ERROR, "开始时间不能大于于结束时间");
                }
                var classDetailEntity = item.ToEntity<InteClassDetailEntity>();
                classDetailEntity.ClassId = entity.Id;
                details.Add(classDetailEntity);
            }

            var rows = 0;
            using (var trans = TransactionHelper.GetTransactionScope())
            {
                rows += await _inteClassDetailRepository.DeleteByClassIdAsync(entity.Id);
                rows += await _inteClassRepository.UpdateAsync(entity);
                rows += await _inteClassDetailRepository.InsertRangeAsync(details);
                trans.Complete();
            }
            return rows;
        }

        /// <summary>
        /// 删除班制维护详情
        /// </summary>
        /// <param name="idsArr"></param>
        /// <returns></returns>
        public async Task<int> DeletesAsync(long[] idsArr)
        {
            return await _inteClassRepository.DeletesAsync(idsArr);
        }

        /// <summary>
        /// 查询班制维护详情列表
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<InteClassDto>> GetPagedListAsync(InteClassPagedQueryDto pagedQueryDto)
        {
            var pagedQuery = pagedQueryDto.ToQuery<InteClassPagedQuery>();
            var pagedInfo = await _inteClassRepository.GetPagedListAsync(pagedQuery);

            // 实体到DTO转换 装载数据
            var dtos = pagedInfo.Data.Select(s => s.ToModel<InteClassDto>());
            return new PagedInfo<InteClassDto>(dtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 获取班次数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<InteClassWithDetailDto> GetDetailAsync(long id)
        {
            InteClassWithDetailDto response = new();
            response.ClassInfo = (await _inteClassRepository.GetByIdAsync(id)).ToModel<InteClassDto>();

            var detailList = await _inteClassDetailRepository.GetListByClassIdAsync(id);
            foreach (var item in detailList)
            {
                response.DetailList.Add(new InteClassDetailDto
                {
                    Id = item.Id,
                    CreatedBy = item.CreatedBy,
                    CreatedOn = item.CreatedOn,
                    UpdatedBy = item.UpdatedBy,
                    UpdatedOn = item.UpdatedOn,
                    Remark = item.Remark,
                    DetailClassType = item.DetailClassType,
                    ProjectContent = item.ProjectContent,
                    StartTime = item.StartTime.ToString("HH:mm:ss"),
                    EndTime = item.EndTime.ToString("HH:mm:ss")
                });
            }
            return response;
        }




        /// <summary>
        /// 
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        private static bool TimeComparison(string startTime, string endTime)
        {
            if (startTime == endTime)
            {
                return false;
            }
            if (endTime == "00:00:00")
            {
                return true;
            }
            string[] startTimeArry = startTime.Split(':');
            string[] endTimeArry = endTime.Split(':');
            if ((int.Parse(startTimeArry[0]) * 60 + int.Parse(startTimeArry[1]) * 60 + int.Parse(startTimeArry[2])) > (int.Parse(endTimeArry[0]) * 60 + int.Parse(endTimeArry[1]) * 60 + int.Parse(endTimeArry[2])))
            {
                return false;
            }
            return true;
        }
    }
}
