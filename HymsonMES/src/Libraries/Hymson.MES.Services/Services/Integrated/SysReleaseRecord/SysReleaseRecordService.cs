/*
 *creator: Karl
 *
 *describe: 发布记录表    服务 | 代码由框架生成
 *builder:  pengxin
 *build datetime: 2023-12-19 10:03:09
 */
using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Integrated;
using Hymson.MES.Core.Enums.Integrated;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Integrated;
using Hymson.MES.Services.Dtos.Integrated;
using Hymson.Snowflake;
using Hymson.Utils;
using IdGen;

using System;
using System.Transactions;

namespace Hymson.MES.Services.Services.Integrated
{
    /// <summary>
    /// 发布记录表 服务
    /// </summary>
    public class SysReleaseRecordService : ISysReleaseRecordService
    {
        private readonly ICurrentUser _currentUser;
        private readonly ICurrentSite _currentSite;

        /// <summary>
        /// 发布记录表 仓储
        /// </summary>
        private readonly ISysReleaseRecordRepository _sysReleaseRecordRepository;
        private readonly AbstractValidator<SysReleaseRecordCreateDto> _validationCreateRules;
        private readonly AbstractValidator<SysReleaseRecordModifyDto> _validationModifyRules;

        public SysReleaseRecordService(ICurrentUser currentUser, ICurrentSite currentSite, ISysReleaseRecordRepository sysReleaseRecordRepository, AbstractValidator<SysReleaseRecordCreateDto> validationCreateRules, AbstractValidator<SysReleaseRecordModifyDto> validationModifyRules)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _sysReleaseRecordRepository = sysReleaseRecordRepository;
            _validationCreateRules = validationCreateRules;
            _validationModifyRules = validationModifyRules;
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="sysReleaseRecordCreateDto"></param>
        /// <returns></returns>
        public async Task CreateSysReleaseRecordAsync(SysReleaseRecordCreateDto sysReleaseRecordCreateDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10101));
            }
            sysReleaseRecordCreateDto.Version = sysReleaseRecordCreateDto.Version.Trim();
            var entity = await _sysReleaseRecordRepository.GetByVersionAsync(new SysReleaseRecordPagedQuery { Version = sysReleaseRecordCreateDto.Version, EnvironmentType = sysReleaseRecordCreateDto.EnvironmentType });
            if (entity != null)
            {
                // 判断版本存在
                throw new CustomerValidationException(nameof(ErrorCode.MES19301));
            }
            //验证DTO
            await _validationCreateRules.ValidateAndThrowAsync(sysReleaseRecordCreateDto);

            //DTO转换实体
            var sysReleaseRecordEntity = sysReleaseRecordCreateDto.ToEntity<SysReleaseRecordEntity>();
            sysReleaseRecordEntity.Id = IdGenProvider.Instance.CreateId();

            sysReleaseRecordEntity.CreatedBy = _currentUser.UserName;
            sysReleaseRecordEntity.Status = SysReleaseRecordStatusEnum.reserve;
            sysReleaseRecordEntity.UpdatedBy = _currentUser.UserName;
            sysReleaseRecordEntity.CreatedOn = HymsonClock.Now();
            sysReleaseRecordEntity.UpdatedOn = HymsonClock.Now();
            sysReleaseRecordEntity.SiteId = _currentSite.SiteId ?? 0;

            //入库
            await _sysReleaseRecordRepository.InsertAsync(sysReleaseRecordEntity);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteSysReleaseRecordAsync(long id)
        {
            var entity = await _sysReleaseRecordRepository.GetByIdAsync(id);
            if (entity != null && entity.Status == SysReleaseRecordStatusEnum.release)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES19303));
            }
            await _sysReleaseRecordRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeletesSysReleaseRecordAsync(long[] ids)
        {
            var list = await _sysReleaseRecordRepository.GetByIdsAsync(ids);
            foreach (var item in list)
            {
                if (item.Status == SysReleaseRecordStatusEnum.release)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES19303));
                }
            }

            return await _sysReleaseRecordRepository.DeletesAsync(new DeleteCommand { Ids = ids, DeleteOn = HymsonClock.Now(), UserId = _currentUser.UserName });
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="sysReleaseRecordPagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<SysReleaseRecordDto>> GetPagedListAsync(SysReleaseRecordPagedQueryDto sysReleaseRecordPagedQueryDto)
        {
            var sysReleaseRecordPagedQuery = sysReleaseRecordPagedQueryDto.ToQuery<SysReleaseRecordPagedQuery>();
            var pagedInfo = await _sysReleaseRecordRepository.GetPagedInfoAsync(sysReleaseRecordPagedQuery);

            //实体到DTO转换 装载数据
            List<SysReleaseRecordDto> sysReleaseRecordDtos = PrepareSysReleaseRecordDtos(pagedInfo);
            return new PagedInfo<SysReleaseRecordDto>(sysReleaseRecordDtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pagedInfo"></param>
        /// <returns></returns>
        private static List<SysReleaseRecordDto> PrepareSysReleaseRecordDtos(PagedInfo<SysReleaseRecordEntity> pagedInfo)
        {
            var sysReleaseRecordDtos = new List<SysReleaseRecordDto>();
            foreach (var sysReleaseRecordEntity in pagedInfo.Data)
            {
                var sysReleaseRecordDto = sysReleaseRecordEntity.ToModel<SysReleaseRecordDto>();
                sysReleaseRecordDtos.Add(sysReleaseRecordDto);
            }

            return sysReleaseRecordDtos;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="sysReleaseRecordModifyDto"></param>
        /// <returns></returns>
        public async Task ModifySysReleaseRecordAsync(SysReleaseRecordModifyDto sysReleaseRecordModifyDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10101));
            }
            sysReleaseRecordModifyDto.Version = (sysReleaseRecordModifyDto.Version ?? "").Trim();
            var entity = await _sysReleaseRecordRepository.GetByVersionAsync(new SysReleaseRecordPagedQuery { Version = sysReleaseRecordModifyDto.Version, EnvironmentType = sysReleaseRecordModifyDto.EnvironmentType });
            if (entity != null)
            {
                // 判断版本存在
                throw new CustomerValidationException(nameof(ErrorCode.MES19301));
            }
            //验证DTO
            await _validationModifyRules.ValidateAndThrowAsync(sysReleaseRecordModifyDto);

            //DTO转换实体
            var sysReleaseRecordEntity = sysReleaseRecordModifyDto.ToEntity<SysReleaseRecordEntity>();
            sysReleaseRecordEntity.Version = sysReleaseRecordModifyDto.Version ?? "";
            sysReleaseRecordEntity.EnvironmentType = sysReleaseRecordModifyDto.EnvironmentType;
            sysReleaseRecordEntity.PlanTime = (sysReleaseRecordModifyDto.PlanTime ?? HymsonClock.Now().ToString()).ParseToDateTime();
            sysReleaseRecordEntity.Content = sysReleaseRecordModifyDto.Content ?? "";
            sysReleaseRecordEntity.UpdatedBy = _currentUser.UserName;
            sysReleaseRecordEntity.UpdatedOn = HymsonClock.Now();

            await _sysReleaseRecordRepository.UpdateAsync(sysReleaseRecordEntity);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="sysReleaseRecordModifyDto"></param>
        /// <returns></returns>
        public async Task ModifySysReleaseRecordStatusAsync(SysReleaseRecordModifyDto sysReleaseRecordModifyDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10101));
            }

            if (sysReleaseRecordModifyDto.Status == SysReleaseRecordStatusEnum.reserve)
            {
                var entity = await _sysReleaseRecordRepository.GetByIdAsync(sysReleaseRecordModifyDto.Id ?? 0);
                if (entity.RealTime != null)
                {

                    TimeSpan time = HymsonClock.Now() - (entity.RealTime ?? HymsonClock.Now());
                    if (time.TotalHours > 24)
                    {
                        //  时间
                        throw new CustomerValidationException(nameof(ErrorCode.MES19302));
                    }
                }
            }
            //验证DTO
            await _validationModifyRules.ValidateAndThrowAsync(sysReleaseRecordModifyDto);

            //DTO转换实体
            var sysReleaseRecordEntity = sysReleaseRecordModifyDto.ToEntity<SysReleaseRecordEntity>();
            sysReleaseRecordEntity.Status = sysReleaseRecordModifyDto.Status ?? SysReleaseRecordStatusEnum.reserve;
            sysReleaseRecordEntity.RealTime = sysReleaseRecordModifyDto.Status == SysReleaseRecordStatusEnum.reserve ? null : HymsonClock.Now();
            sysReleaseRecordEntity.UpdatedBy = _currentUser.UserName;
            sysReleaseRecordEntity.UpdatedOn = HymsonClock.Now();

            await _sysReleaseRecordRepository.UpdateStatusAsync(sysReleaseRecordEntity);
        }

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<SysReleaseRecordDto> QuerySysReleaseRecordByIdAsync(long id)
        {
            var sysReleaseRecordEntity = await _sysReleaseRecordRepository.GetByIdAsync(id);
            if (sysReleaseRecordEntity != null)
            {
                return sysReleaseRecordEntity.ToModel<SysReleaseRecordDto>();
            }
            return null;
        }


        #region 内部方法


        #endregion
    }
}
