using Elastic.Clients.Elasticsearch.QueryDsl;
using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Equipment;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Equipment;
using Hymson.MES.Data.Repositories.Equipment.EquEquipment;
using Hymson.MES.Data.Repositories.Equipment.Query;
using Hymson.MES.Services.Dtos.Equipment;
using Hymson.MES.Services.Dtos.Quality;
using Hymson.Snowflake;
using Hymson.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySqlX.XDevAPI.Common;
using System.Collections.Generic;

namespace Hymson.MES.Services.Services.Equipment
{
    /// <summary>
    /// 服务（点检任务） 
    /// </summary>
    public class EquSpotcheckTaskService : IEquSpotcheckTaskService
    {
        /// <summary>
        /// 当前用户
        /// </summary>
        private readonly ICurrentUser _currentUser;
        /// <summary>
        /// 当前站点
        /// </summary>
        private readonly ICurrentSite _currentSite;

        /// <summary>
        /// 参数验证器
        /// </summary>
        private readonly AbstractValidator<EquSpotcheckTaskSaveDto> _validationSaveRules;

        /// <summary>
        /// 仓储接口（点检任务）
        /// </summary>
        private readonly IEquSpotcheckTaskRepository _equSpotcheckTaskRepository;

        /// <summary>
        /// 仓储（设备注册）
        /// </summary>
        private readonly IEquEquipmentRepository _equEquipmentRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="currentSite"></param>
        /// <param name="validationSaveRules"></param>
        /// <param name="equSpotcheckTaskRepository"></param>
        public EquSpotcheckTaskService(ICurrentUser currentUser, ICurrentSite currentSite, AbstractValidator<EquSpotcheckTaskSaveDto> validationSaveRules, 
            IEquSpotcheckTaskRepository equSpotcheckTaskRepository, IEquEquipmentRepository equEquipmentRepository)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _validationSaveRules = validationSaveRules;
            _equSpotcheckTaskRepository = equSpotcheckTaskRepository;
            _equEquipmentRepository = equEquipmentRepository;
        }


        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        public async Task<int> CreateAsync(EquSpotcheckTaskSaveDto saveDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0) throw new CustomerValidationException(nameof(ErrorCode.MES10101));

            // 验证DTO
            await _validationSaveRules.ValidateAndThrowAsync(saveDto);

            // 更新时间
            var updatedBy = _currentUser.UserName;
            var updatedOn = HymsonClock.Now();

            // DTO转换实体
            var entity = saveDto.ToEntity<EquSpotcheckTaskEntity>();
            entity.Id = IdGenProvider.Instance.CreateId();
            entity.CreatedBy = updatedBy;
            entity.CreatedOn = updatedOn;
            entity.UpdatedBy = updatedBy;
            entity.UpdatedOn = updatedOn;
            entity.SiteId = _currentSite.SiteId ?? 0;

            // 保存
            return await _equSpotcheckTaskRepository.InsertAsync(entity);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        public async Task<int> ModifyAsync(EquSpotcheckTaskSaveDto saveDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0) throw new CustomerValidationException(nameof(ErrorCode.MES10101));

             // 验证DTO
            await _validationSaveRules.ValidateAndThrowAsync(saveDto);

            // DTO转换实体
            var entity = saveDto.ToEntity<EquSpotcheckTaskEntity>();
            entity.UpdatedBy = _currentUser.UserName;
            entity.UpdatedOn = HymsonClock.Now();

            return await _equSpotcheckTaskRepository.UpdateAsync(entity);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(long id)
        {
            return await _equSpotcheckTaskRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeletesAsync(long[] ids)
        {
            return await _equSpotcheckTaskRepository.DeletesAsync(new DeleteCommand
            {
                Ids = ids,
                DeleteOn = HymsonClock.Now(),
                UserId = _currentUser.UserName
            });
        }

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<EquSpotcheckTaskDto?> QueryByIdAsync(long id) 
        {
           var equSpotcheckTaskEntity = await _equSpotcheckTaskRepository.GetByIdAsync(id);
           if (equSpotcheckTaskEntity == null) return null;
           
           return equSpotcheckTaskEntity.ToModel<EquSpotcheckTaskDto>();
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<EquSpotcheckTaskDto>> GetPagedListAsync(EquSpotcheckTaskPagedQueryDto pagedQueryDto)
        {
            var pagedQuery = pagedQueryDto.ToQuery<EquSpotcheckTaskPagedQuery>();
            pagedQuery.SiteId = _currentSite.SiteId ?? 0;

            // 转换设备编码变为taskid
            if (!string.IsNullOrWhiteSpace(pagedQuery.EquipmentCode))
            {
                var equipmenEntities = await _equEquipmentRepository.GetByCodeAsync(new Data.Repositories.Common.Query.EntityByCodeQuery
                {
                    Site = pagedQuery.SiteId,
                    Code = pagedQuery.EquipmentCode,
                });
                if (equipmenEntities != null) pagedQuery.EquipmentId = equipmenEntities.Id;
                else pagedQuery.EquipmentId = default;
            }

            // 将不合格处理方式转换为检验单ID
            //if (pagedQueryDto.HandMethod.HasValue)
            //{
            //    var unqualifiedHandEntities = await _qualFqcOrderUnqualifiedHandleRepository.GetEntitiesAsync(new QualFqcOrderUnqualifiedHandleQuery
            //    {
            //        SiteId = pagedQuery.SiteId,
            //        HandMethod = pagedQueryDto.HandMethod
            //    });
            //    if (unqualifiedHandEntities != null && unqualifiedHandEntities.Any()) pagedQuery.FQCOrderIds = unqualifiedHandEntities.Select(s => s.FQCOrderId);
            //    else pagedQuery.FQCOrderIds = Array.Empty<long>();
            //}

            var result = new PagedInfo<EquSpotcheckTaskDto>(Enumerable.Empty<EquSpotcheckTaskDto>(), pagedQuery.PageIndex, pagedQuery.PageSize);

            var pagedInfo = await _equSpotcheckTaskRepository.GetPagedListAsync(pagedQuery);

            if (pagedInfo.Data != null && pagedInfo.Data.Any())
            {
                result.Data = pagedInfo.Data.Select(s => s.ToModel<EquSpotcheckTaskDto>());
                result.TotalCount = pagedInfo.TotalCount;

                var resultEquipmentIds = result.Data.Select(m => m.EquipmentId.GetValueOrDefault());
                try
                {
                    var equipmenEntities = await _equEquipmentRepository.GetByIdAsync(resultEquipmentIds);

                    result.Data = result.Data.Select(m =>
                    {
                        var equipmentEntity = equipmenEntities.FirstOrDefault(e => e.Id == m.EquipmentId);
                        if (equipmentEntity != null)
                        {
                            m.EquipmentCode = equipmentEntity.EquipmentCode;
                            m.EquipmentName = equipmentEntity.EquipmentName;
                            m.Location = equipmentEntity.Location;
                        }
                        return m;
                    });
                }catch(Exception ex) { }


            }

            return result;
        }

        /// <summary>
        /// 查询点检单明细项数据
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<IEnumerable<TaskItemInfoView>> querySnapshotItemAsync(SpotcheckTaskSnapshotItemQueryDto requestDto)
        {
            IEnumerable<TaskItemInfoView> info = null;
            return info;
        }

    }
}
