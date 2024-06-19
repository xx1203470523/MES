/*
 *creator: Karl
 *
 *describe: 设备维修记录    服务 | 代码由框架生成
 *builder:  pengxin
 *build datetime: 2024-06-12 10:56:10
 */
using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.EquRepairOrder;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.EquRepairOrder;
using Hymson.MES.Services.Dtos.EquRepairOrder;
using Hymson.Snowflake;
using Hymson.Utils;
using System.Transactions;

namespace Hymson.MES.Services.Services.EquRepairOrder
{
    /// <summary>
    /// 设备维修记录 服务
    /// </summary>
    public class EquRepairOrderService : IEquRepairOrderService
    {
        private readonly ICurrentUser _currentUser;
        private readonly ICurrentSite _currentSite;

        /// <summary>
        /// 设备维修记录 仓储
        /// </summary>
        private readonly IEquRepairOrderRepository _equRepairOrderRepository;
        private readonly AbstractValidator<EquRepairOrderCreateDto> _validationCreateRules;
        private readonly AbstractValidator<EquRepairOrderModifyDto> _validationModifyRules;

        public EquRepairOrderService(ICurrentUser currentUser, ICurrentSite currentSite, IEquRepairOrderRepository equRepairOrderRepository, AbstractValidator<EquRepairOrderCreateDto> validationCreateRules, AbstractValidator<EquRepairOrderModifyDto> validationModifyRules)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _equRepairOrderRepository = equRepairOrderRepository;
            _validationCreateRules = validationCreateRules;
            _validationModifyRules = validationModifyRules;
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="equRepairOrderCreateDto"></param>
        /// <returns></returns>
        public async Task CreateEquRepairOrderAsync(EquRepairOrderCreateDto equRepairOrderCreateDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0)
            {
                throw new ValidationException(nameof(ErrorCode.MES10101));
            }

            //验证DTO
            await _validationCreateRules.ValidateAndThrowAsync(equRepairOrderCreateDto);

            //DTO转换实体
            var equRepairOrderEntity = equRepairOrderCreateDto.ToEntity<EquRepairOrderEntity>();
            equRepairOrderEntity.Id= IdGenProvider.Instance.CreateId();
            equRepairOrderEntity.CreatedBy = _currentUser.UserName;
            equRepairOrderEntity.UpdatedBy = _currentUser.UserName;
            equRepairOrderEntity.CreatedOn = HymsonClock.Now();
            equRepairOrderEntity.UpdatedOn = HymsonClock.Now();
            equRepairOrderEntity.SiteId = _currentSite.SiteId ?? 0;

            //入库
            await _equRepairOrderRepository.InsertAsync(equRepairOrderEntity);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteEquRepairOrderAsync(long id)
        {
            await _equRepairOrderRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeletesEquRepairOrderAsync(long[] ids)
        {
            return await _equRepairOrderRepository.DeletesAsync(new DeleteCommand { Ids = ids, DeleteOn = HymsonClock.Now(), UserId = _currentUser.UserName });
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="equRepairOrderPagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<EquRepairOrderDto>> GetPagedListAsync(EquRepairOrderPagedQueryDto equRepairOrderPagedQueryDto)
        {
            var equRepairOrderPagedQuery = equRepairOrderPagedQueryDto.ToQuery<EquRepairOrderPagedQuery>();
            var pagedInfo = await _equRepairOrderRepository.GetPagedInfoAsync(equRepairOrderPagedQuery);

            //实体到DTO转换 装载数据
            List<EquRepairOrderDto> equRepairOrderDtos = PrepareEquRepairOrderDtos(pagedInfo);
            return new PagedInfo<EquRepairOrderDto>(equRepairOrderDtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pagedInfo"></param>
        /// <returns></returns>
        private static List<EquRepairOrderDto> PrepareEquRepairOrderDtos(PagedInfo<EquRepairOrderEntity>   pagedInfo)
        {
            var equRepairOrderDtos = new List<EquRepairOrderDto>();
            foreach (var equRepairOrderEntity in pagedInfo.Data)
            {
                var equRepairOrderDto = equRepairOrderEntity.ToModel<EquRepairOrderDto>();
                equRepairOrderDtos.Add(equRepairOrderDto);
            }

            return equRepairOrderDtos;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="equRepairOrderDto"></param>
        /// <returns></returns>
        public async Task ModifyEquRepairOrderAsync(EquRepairOrderModifyDto equRepairOrderModifyDto)
        {
             // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0)
            {
                throw new ValidationException(nameof(ErrorCode.MES10101));
            }

             //验证DTO
            await _validationModifyRules.ValidateAndThrowAsync(equRepairOrderModifyDto);

            //DTO转换实体
            var equRepairOrderEntity = equRepairOrderModifyDto.ToEntity<EquRepairOrderEntity>();
            equRepairOrderEntity.UpdatedBy = _currentUser.UserName;
            equRepairOrderEntity.UpdatedOn = HymsonClock.Now();

            await _equRepairOrderRepository.UpdateAsync(equRepairOrderEntity);
        }

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<EquRepairOrderDto> QueryEquRepairOrderByIdAsync(long id) 
        {
           var equRepairOrderEntity = await _equRepairOrderRepository.GetByIdAsync(id);
           if (equRepairOrderEntity != null) 
           {
               return equRepairOrderEntity.ToModel<EquRepairOrderDto>();
           }
            return null;
        }
    }
}
