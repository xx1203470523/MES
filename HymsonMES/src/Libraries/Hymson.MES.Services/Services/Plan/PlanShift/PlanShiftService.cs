using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.Localization.Services;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Integrated;
using Hymson.MES.Core.Domain.Plan;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Plan;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Plan;
using Hymson.MES.Data.Repositories.Plan.Query;
using Hymson.MES.Services.Dtos.Common;
using Hymson.MES.Services.Dtos.Integrated;
using Hymson.MES.Services.Dtos.Plan;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;
using IdGen;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.Xml;
using System.Transactions;

namespace Hymson.MES.Services.Services.Plan
{
    /// <summary>
    /// 服务（班制） 
    /// </summary>
    public class PlanShiftService : IPlanShiftService
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
        private readonly AbstractValidator<PlanShiftSaveDto> _validationSaveRules;
        private readonly AbstractValidator<PlanShiftDetailModifyDto> _validationDetailRules;

        /// <summary>
        /// 仓储接口（班制）
        /// </summary>
        private readonly IPlanShiftRepository _planShiftRepository;

        private readonly ILocalizationService _localizationService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="currentSite"></param>
        /// <param name="validationSaveRules"></param>
        /// <param name="validationDetailRules"></param>
        /// <param name="planShiftRepository"></param>
        public PlanShiftService(ICurrentUser currentUser, ICurrentSite currentSite, AbstractValidator<PlanShiftSaveDto> validationSaveRules,
            AbstractValidator<PlanShiftDetailModifyDto> validationDetailRules,
            IPlanShiftRepository planShiftRepository, ILocalizationService localizationService)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _validationSaveRules = validationSaveRules;
            _validationDetailRules = validationDetailRules;
            _planShiftRepository = planShiftRepository;
            _localizationService = localizationService;
        }


        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        public async Task<int> CreateAsync(PlanShiftSaveDto saveDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0) throw new CustomerValidationException(nameof(ErrorCode.MES10101));

            // 验证DTO
            await _validationSaveRules.ValidateAndThrowAsync(saveDto);

            // 更新时间
            var updatedBy = _currentUser.UserName;
            var updatedOn = HymsonClock.Now();

            // DTO转换实体
            var entity = saveDto.ToEntity<PlanShiftEntity>();
            entity.Id = IdGenProvider.Instance.CreateId();
            entity.CreatedBy = updatedBy;
            entity.CreatedOn = updatedOn;
            entity.UpdatedBy = updatedBy;
            entity.UpdatedOn = updatedOn;
            entity.SiteId = _currentSite.SiteId ?? 0;

            // 保存
            return await _planShiftRepository.InsertAsync(entity);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="saveDto"></param>
        /// <param name="exeType"></param>
        /// <returns></returns>
        public async Task ModifyAsync(PlanShiftSaveDto saveDto, InteShiftModifyTypeEnum exeType)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0) throw new CustomerValidationException(nameof(ErrorCode.MES10101));

            // 验证DTO
            await _validationSaveRules.ValidateAndThrowAsync(saveDto);

            // DTO转换实体
            var entity = saveDto.ToEntity<PlanShiftEntity>();
            entity.UpdatedBy = _currentUser.UserName;
            entity.UpdatedOn = HymsonClock.Now();
            if (exeType == InteShiftModifyTypeEnum.create)
            {
                entity.Id = IdGenProvider.Instance.CreateId();
                entity.SiteId = _currentSite.SiteId ?? 0;
            }

            var planshiftEntity = await _planShiftRepository.GetEntitiesAsync(new PlanShiftQuery { Code = entity.Code, SiteId = entity.SiteId });
            if (planshiftEntity.Any() && exeType == InteShiftModifyTypeEnum.create)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES19505));
            }
            if (!saveDto.PlanShiftDetailList.Any()) throw new CustomerValidationException(nameof(ErrorCode.MES19508));
            #region 处理 班次详细数据
            var planShiftDetailList = new List<PlanShiftDetailEntity>();
            if (saveDto.PlanShiftDetailList.Any())
            {
                foreach (var item in saveDto.PlanShiftDetailList)
                {
                    //验证数据
                    await _validationDetailRules.ValidateAndThrowAsync(item);

                    if (!item.IsDaySpan)
                    {
                        if (CountDown(item.StartTime) > CountDown(item.EndTime))
                        {
                            throw new CustomerValidationException(nameof(ErrorCode.MES19507));
                        }
                    }

                    planShiftDetailList.Add(new PlanShiftDetailEntity()
                    {

                        ShiftType = item.ShiftType,
                        ShfitId = entity.Id,
                        StartTime = item.StartTime,
                        EndTime = item.EndTime,
                        IsDaySpan = item.IsDaySpan,
                        IsOverTime = item.IsOverTime,
                        Remark = item.Remark,
                        Id = IdGenProvider.Instance.CreateId(),
                        CreatedBy = _currentUser.UserName,
                        UpdatedBy = _currentUser.UserName,
                        CreatedOn = HymsonClock.Now(),
                        UpdatedOn = HymsonClock.Now(),
                        //SiteId = _currentSite.SiteId ?? 0,
                    });
                }
            }


            bool isRepeat = planShiftDetailList.GroupBy(i => i.ShiftType).Where(g => g.Count() > 1).Count() > 0;
            if (isRepeat) throw new CustomerValidationException(nameof(ErrorCode.MES19506));

            #endregion


            using (TransactionScope ts = TransactionHelper.GetTransactionScope())
            {
                if (exeType == InteShiftModifyTypeEnum.modify)
                {
                    await _planShiftRepository.UpdateAsync(entity);
                }
                else if (exeType == InteShiftModifyTypeEnum.create)
                {
                    await _planShiftRepository.InsertAsync(entity);
                }

                //先删除
                await _planShiftRepository.DeletesDetailByIdAsync(new long[] { entity.Id });
                if (planShiftDetailList.Any())
                    await _planShiftRepository.InsertDetailAsync(planShiftDetailList);

                ts.Complete();
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(long id)
        {
            //先删除
            await _planShiftRepository.DeletesDetailByIdAsync(new long[] { id });
            return await _planShiftRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeletesAsync(long[] ids)
        {
            //先删除
            await _planShiftRepository.DeletesDetailByIdAsync(ids);
            return await _planShiftRepository.DeletesAsync(new DeleteCommand
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
        public async Task<PlanShiftDto?> QueryByIdAsync(long id)
        {
            var planShiftEntity = await _planShiftRepository.GetByIdAsync(id);
            if (planShiftEntity == null) return null;

            return planShiftEntity.ToModel<PlanShiftDto>();
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<PlanShiftDto>> GetPagedListAsync(PlanShiftPagedQueryDto pagedQueryDto)
        {
            var pagedQuery = pagedQueryDto.ToQuery<PlanShiftPagedQuery>();
            pagedQuery.SiteId = _currentSite.SiteId ?? 0;
            var pagedInfo = await _planShiftRepository.GetPagedListAsync(pagedQuery);

            // 实体到DTO转换 装载数据
            var dtos = pagedInfo.Data.Select(s => s.ToModel<PlanShiftDto>());
            return new PagedInfo<PlanShiftDto>(dtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 根据站点获取所有数据
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<PlanShiftDto>> GetAllAsync()
        {
            var _siteId = _currentSite.SiteId ?? 0;

            var result = await _planShiftRepository.GetAllAsync(new PlanShiftQuery
            {
                SiteId = _siteId
            });

            return result.Select(m => m.ToModel<PlanShiftDto>());
        }


        /// <summary>
        /// 根据ID查询详细
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IEnumerable<PlanShiftDetailDto>> GetByMainIdAsync(long id)
        {
            var result = await _planShiftRepository.GetByMainIdAsync(id);

            return result.Select(m => m.ToModel<PlanShiftDetailDto>());
        }

        /// <summary>
        /// 状态变更
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        /// <exception cref="CustomerValidationException"></exception>
        public async Task UpdateStatusAsync(ChangeStatusDto param)
        {
            #region 参数校验
            if (param.Id == 0)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10125));
            }
            //if (!Enum.IsDefined(typeof(SysDataStatusEnum), param.Status))
            //{
            //    throw new CustomerValidationException(nameof(ErrorCode.MES10126));
            //}
            if (param.Status == SysDataStatusEnum.Build)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10128));
            }

            #endregion

            var changeStatusCommand = new ChangeStatusCommand()
            {
                Id = param.Id,
                Status = param.Status,

                UpdatedBy = _currentUser.UserName,
                UpdatedOn = HymsonClock.Now()
            };

            #region 校验数据
            var entity = await _planShiftRepository.GetByIdAsync(changeStatusCommand.Id);
            if (entity == null || entity.IsDeleted != 0)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10705));
            }

            if(entity.Status== param.Status)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10127)).WithData("status", _localizationService.GetResource($"{typeof(SysDataStatusEnum).FullName}.{Enum.GetName(typeof(SysDataStatusEnum), entity.Status)}"));
            }
 
            #endregion

            #region 操作数据库
            await _planShiftRepository.UpdateStatusAsync(changeStatusCommand);
            #endregion
        }

        /// <summary>
        /// 转时间戳
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        private int CountDown(string time)
        {
            int s = 0;
            string[] timeParts = time.Split(':');

            if (timeParts.Length == 3)
            {
                int hour = int.Parse(timeParts[0]);
                int min = int.Parse(timeParts[1]);
                int sec = int.Parse(timeParts[2]);

                s = hour * 3600 + min * 60 + sec;
            }

            return s;
        }

    }
}
