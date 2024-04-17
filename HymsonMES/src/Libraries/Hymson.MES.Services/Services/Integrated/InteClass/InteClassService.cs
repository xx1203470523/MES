using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Domain.Integrated;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Integrated.InteClass;
using Hymson.MES.Data.Repositories.Integrated.InteClass.Query;
using Hymson.MES.Services.Dtos.Integrated;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;

namespace Hymson.MES.Services.Services.Integrated.InteClass
{
    /// <summary>
    /// 班制维护 服务
    /// </summary>
    public class InteClassService : IInteClassService
    {
        /// <summary>
        /// 当前对象（登录用户）
        /// </summary>
        private readonly ICurrentUser _currentUser;

        /// <summary>
        /// 当前对象（站点）
        /// </summary>
        private readonly ICurrentSite _currentSite;

        /// <summary>
        /// 
        /// </summary>
        private readonly AbstractValidator<InteClassSaveDto> _validationSaveRules;

        /// <summary>
        /// 仓储（班制维护）
        /// </summary>
        private readonly IInteClassRepository _inteClassRepository;

        /// <summary>
        /// 仓储（班制维护明细）
        /// </summary>
        private readonly IInteClassDetailRepository _inteClassDetailRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="currentSite"></param>
        /// <param name="currentUser"></param>
        /// <param name="validationSaveRules"></param>
        /// <param name="inteClassRepository"></param>
        /// <param name="inteClassDetailRepository"></param>
        public InteClassService(ICurrentUser currentUser, ICurrentSite currentSite,
            AbstractValidator<InteClassSaveDto> validationSaveRules,
            IInteClassRepository inteClassRepository,
            IInteClassDetailRepository inteClassDetailRepository)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _validationSaveRules = validationSaveRules;
            _inteClassRepository = inteClassRepository;
            _inteClassDetailRepository = inteClassDetailRepository;
        }


        /// <summary>
        /// 添加班制维护详情
        /// </summary>
        /// <param name="createDto"></param>
        /// <returns></returns>
        public async Task<int> CreateAsync(InteClassSaveDto createDto)
        {
            // 验证DTO

            // DTO转换实体
            var entity = createDto.ToEntity<InteClassEntity>();
            entity.Id = IdGenProvider.Instance.CreateId();
            entity.CreatedBy = _currentUser.UserName;
            entity.UpdatedBy = _currentUser.UserName;
            entity.SiteId = _currentSite.SiteId;


            List<InteClassDetailEntity> details = new();
            foreach (var item in createDto.DetailList)
            {
                var classDetailEntity = item.ToEntity<InteClassDetailEntity>();
                classDetailEntity.Id = IdGenProvider.Instance.CreateId();
                classDetailEntity.ClassId = entity.Id;
                classDetailEntity.CreatedBy = entity.CreatedBy;
                classDetailEntity.UpdatedBy = entity.UpdatedBy;
                details.Add(classDetailEntity);
            }

            var rows = 0;
            using (var trans = TransactionHelper.GetTransactionScope())
            {
                rows += await _inteClassDetailRepository.InsertRangeAsync(details);
                rows += await _inteClassRepository.InsertAsync(entity);
                trans.Complete();
            }

            return rows;
        }

        /// <summary>
        /// 更新班制维护详情
        /// </summary>
        /// <param name="modifyDto"></param>
        /// <returns></returns>
        public async Task<int> ModifyAsync(InteClassSaveDto modifyDto)
        {
            // DTO转换实体
            var entity = modifyDto.ToEntity<InteClassEntity>();
            entity.UpdatedBy = _currentUser.UserName;

            List<InteClassDetailEntity> details = new();
            foreach (var item in modifyDto.DetailList)
            {
                var classDetailEntity = item.ToEntity<InteClassDetailEntity>();
                classDetailEntity.Id = IdGenProvider.Instance.CreateId();
                classDetailEntity.ClassId = entity.Id;
                classDetailEntity.CreatedBy = entity.CreatedBy;
                classDetailEntity.UpdatedBy = entity.UpdatedBy;
                details.Add(classDetailEntity);
            }

            var rows = 0;
            using (var trans = TransactionHelper.GetTransactionScope())
            {
                rows += await _inteClassDetailRepository.DeleteByClassIdAsync(entity.Id);
                rows += await _inteClassDetailRepository.InsertRangeAsync(details);
                rows += await _inteClassRepository.UpdateAsync(entity);
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
            return await _inteClassRepository.DeletesAsync(new DeleteCommand
            {
                Ids = idsArr,
                UserId = _currentUser.UserName,
                DeleteOn = HymsonClock.Now()
            });
        }

        /// <summary>
        /// 查询班制维护详情列表
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<InteClassDto>> GetPagedListAsync(InteClassPagedQueryDto pagedQueryDto)
        {
            var pagedQuery = pagedQueryDto.ToQuery<InteClassPagedQuery>();
            pagedQuery.SiteId = _currentSite.SiteId;
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

            var index = 1;
            var detailList = await _inteClassDetailRepository.GetListByClassIdAsync(id);
            foreach (var item in detailList)
            {
                response.DetailList.Add(new InteClassDetailDto
                {
                    Id = item.Id,
                    SerialNo = index,
                    DetailClassType = item.DetailClassType,
                    ProjectContent = item.ProjectContent,
                    StartTime = item.StartTime,
                    EndTime = item.EndTime
                });
                index++;
            }

            return response;
        }
    }
}
