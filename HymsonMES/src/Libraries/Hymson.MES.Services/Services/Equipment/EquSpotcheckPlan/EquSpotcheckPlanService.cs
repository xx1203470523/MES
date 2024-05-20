/*
 *creator: Karl
 *
 *describe: 设备点检计划    服务 | 代码由框架生成
 *builder:  pengxin
 *build datetime: 2024-05-16 02:14:30
 */
using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.EquSpotcheckPlan;
using Hymson.MES.Core.Domain.EquSpotcheckPlanEquipmentRelation;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.EquSpotcheckPlan;
using Hymson.MES.Data.Repositories.EquSpotcheckPlanEquipmentRelation;
using Hymson.MES.Services.Dtos.EquSpotcheckPlan;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;
using OfficeOpenXml;
using System.Collections.Generic;
using System.Transactions;

namespace Hymson.MES.Services.Services.EquSpotcheckPlan
{
    /// <summary>
    /// 设备点检计划 服务
    /// </summary>
    public class EquSpotcheckPlanService : IEquSpotcheckPlanService
    {
        private readonly ICurrentUser _currentUser;
        private readonly ICurrentSite _currentSite;

        /// <summary>
        /// 设备点检计划 仓储
        /// </summary>
        private readonly IEquSpotcheckPlanRepository _equSpotcheckPlanRepository;
        private readonly IEquSpotcheckPlanEquipmentRelationRepository _equSpotcheckPlanEquipmentRelationRepository;
        private readonly AbstractValidator<EquSpotcheckPlanCreateDto> _validationCreateRules;
        private readonly AbstractValidator<EquSpotcheckPlanModifyDto> _validationModifyRules;

        public EquSpotcheckPlanService(ICurrentUser currentUser, ICurrentSite currentSite, IEquSpotcheckPlanRepository equSpotcheckPlanRepository, AbstractValidator<EquSpotcheckPlanCreateDto> validationCreateRules, AbstractValidator<EquSpotcheckPlanModifyDto> validationModifyRules, IEquSpotcheckPlanEquipmentRelationRepository equSpotcheckPlanEquipmentRelationRepository)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _equSpotcheckPlanRepository = equSpotcheckPlanRepository;
            _validationCreateRules = validationCreateRules;
            _validationModifyRules = validationModifyRules;
            _equSpotcheckPlanEquipmentRelationRepository = equSpotcheckPlanEquipmentRelationRepository;
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="equSpotcheckPlanCreateDto"></param>
        /// <returns></returns>
        public async Task CreateEquSpotcheckPlanAsync(EquSpotcheckPlanCreateDto equSpotcheckPlanCreateDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0)
            {
                throw new ValidationException(nameof(ErrorCode.MES10101));
            }

            //验证DTO
            await _validationCreateRules.ValidateAndThrowAsync(equSpotcheckPlanCreateDto);

            //DTO转换实体
            var equSpotcheckPlanEntity = equSpotcheckPlanCreateDto.ToEntity<EquSpotcheckPlanEntity>();
            equSpotcheckPlanEntity.Id = IdGenProvider.Instance.CreateId();
            equSpotcheckPlanEntity.CreatedBy = _currentUser.UserName;
            equSpotcheckPlanEntity.UpdatedBy = _currentUser.UserName;
            equSpotcheckPlanEntity.CreatedOn = HymsonClock.Now();
            equSpotcheckPlanEntity.UpdatedOn = HymsonClock.Now();
            equSpotcheckPlanEntity.SiteId = _currentSite.SiteId ?? 0;


            List<EquSpotcheckPlanEquipmentRelationEntity> equSpotcheckPlanEquipmentRelationList = new();

            foreach (var item in equSpotcheckPlanCreateDto.RelationDto)
            {
                EquSpotcheckPlanEquipmentRelationEntity equSpotcheckPlanEquipmentRelation = new()
                {
                    EquipmentId = item.EquipmentId,
                    SpotCheckPlanId = equSpotcheckPlanEntity.Id,
                    SpotCheckTemplateId = item.SpotCheckTemplateId,

                    Id = IdGenProvider.Instance.CreateId(),
                    CreatedBy = _currentUser.UserName,
                    UpdatedBy = _currentUser.UserName,
                    CreatedOn = HymsonClock.Now(),
                    UpdatedOn = HymsonClock.Now(),
                };
                equSpotcheckPlanEquipmentRelationList.Add(equSpotcheckPlanEquipmentRelation);
            }

            try
            {
            using var trans = TransactionHelper.GetTransactionScope();

            //入库
            await _equSpotcheckPlanRepository.InsertAsync(equSpotcheckPlanEntity);
            await _equSpotcheckPlanEquipmentRelationRepository.InsertsAsync(equSpotcheckPlanEquipmentRelationList);

            trans.Complete();
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteEquSpotcheckPlanAsync(long id)
        {
            await _equSpotcheckPlanRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeletesEquSpotcheckPlanAsync(long[] ids)
        {

            int row = 0;
            using var trans = TransactionHelper.GetTransactionScope();

            //入库
            row += await _equSpotcheckPlanRepository.DeletesAsync(new DeleteCommand { Ids = ids, DeleteOn = HymsonClock.Now(), UserId = _currentUser.UserName });

            await _equSpotcheckPlanEquipmentRelationRepository.PhysicalDeletesAsync(ids);

            trans.Complete();

            return row;
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="equSpotcheckPlanPagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<EquSpotcheckPlanDto>> GetPagedListAsync(EquSpotcheckPlanPagedQueryDto equSpotcheckPlanPagedQueryDto)
        {
            var equSpotcheckPlanPagedQuery = equSpotcheckPlanPagedQueryDto.ToQuery<EquSpotcheckPlanPagedQuery>();
            equSpotcheckPlanPagedQuery.SiteId = _currentSite.SiteId ?? 0;
            var pagedInfo = await _equSpotcheckPlanRepository.GetPagedInfoAsync(equSpotcheckPlanPagedQuery);

            //实体到DTO转换 装载数据
            List<EquSpotcheckPlanDto> equSpotcheckPlanDtos = PrepareEquSpotcheckPlanDtos(pagedInfo);
            return new PagedInfo<EquSpotcheckPlanDto>(equSpotcheckPlanDtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pagedInfo"></param>
        /// <returns></returns>
        private static List<EquSpotcheckPlanDto> PrepareEquSpotcheckPlanDtos(PagedInfo<EquSpotcheckPlanEntity> pagedInfo)
        {
            var equSpotcheckPlanDtos = new List<EquSpotcheckPlanDto>();
            foreach (var equSpotcheckPlanEntity in pagedInfo.Data)
            {
                var equSpotcheckPlanDto = equSpotcheckPlanEntity.ToModel<EquSpotcheckPlanDto>();
                equSpotcheckPlanDtos.Add(equSpotcheckPlanDto);
            }

            return equSpotcheckPlanDtos;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="equSpotcheckPlanModifyDto"></param>
        /// <returns></returns>
        public async Task ModifyEquSpotcheckPlanAsync(EquSpotcheckPlanModifyDto equSpotcheckPlanModifyDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0)
            {
                throw new ValidationException(nameof(ErrorCode.MES10101));
            }

            //验证DTO
            await _validationModifyRules.ValidateAndThrowAsync(equSpotcheckPlanModifyDto);

            //DTO转换实体
            var equSpotcheckPlanEntity = equSpotcheckPlanModifyDto.ToEntity<EquSpotcheckPlanEntity>();
            equSpotcheckPlanEntity.UpdatedBy = _currentUser.UserName;
            equSpotcheckPlanEntity.UpdatedOn = HymsonClock.Now();




            List<EquSpotcheckPlanEquipmentRelationEntity> equSpotcheckPlanEquipmentRelationList = new();
            List<long> spotCheckPlanIds = new() { equSpotcheckPlanModifyDto.Id };

            foreach (var item in equSpotcheckPlanModifyDto.RelationDto)
            {
                EquSpotcheckPlanEquipmentRelationEntity equSpotcheckPlanEquipmentRelation = new()
                {
                    EquipmentId = item.EquipmentId,
                    SpotCheckPlanId = equSpotcheckPlanModifyDto.Id,
                    SpotCheckTemplateId = item.SpotCheckTemplateId,

                    Id = IdGenProvider.Instance.CreateId(),
                    CreatedBy = _currentUser.UserName,
                    UpdatedBy = _currentUser.UserName,
                    CreatedOn = HymsonClock.Now(),
                    UpdatedOn = HymsonClock.Now(),
                };
                equSpotcheckPlanEquipmentRelationList.Add(equSpotcheckPlanEquipmentRelation);
            }

            using var trans = TransactionHelper.GetTransactionScope();

            //入库
            await _equSpotcheckPlanRepository.UpdateAsync(equSpotcheckPlanEntity);

            await _equSpotcheckPlanEquipmentRelationRepository.PhysicalDeletesAsync(spotCheckPlanIds);
            await _equSpotcheckPlanEquipmentRelationRepository.InsertsAsync(equSpotcheckPlanEquipmentRelationList);

            trans.Complete();

        }

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<EquSpotcheckPlanDto> QueryEquSpotcheckPlanByIdAsync(long id)
        {
            var equSpotcheckPlanEntity = await _equSpotcheckPlanRepository.GetByIdAsync(id);
            if (equSpotcheckPlanEntity != null)
            {
                return equSpotcheckPlanEntity.ToModel<EquSpotcheckPlanDto>();
            }
            return null;
        }
    }
}
