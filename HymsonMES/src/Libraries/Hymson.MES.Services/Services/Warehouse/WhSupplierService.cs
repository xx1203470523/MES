/*
 *creator: Karl
 *
 *describe: 供应商    服务 | 代码由框架生成
 *builder:  pengxin
 *build datetime: 2023-03-03 01:51:43
 */
using FluentValidation;
using Hymson.Authentication;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Warehouse;
using Hymson.MES.Data.Repositories.Warehouse;
using Hymson.MES.Services.Dtos.Warehouse;
using Hymson.Snowflake;
using Hymson.Utils;
//using Hymson.Utils.Extensions;
using System.Transactions;

namespace Hymson.MES.Services.Services.Warehouse
{
    /// <summary>
    /// 供应商 服务
    /// </summary>
    public class WhSupplierService : IWhSupplierService
    {
        private readonly ICurrentUser _currentUser;

        /// <summary>
        /// 供应商 仓储
        /// </summary>
        private readonly IWhSupplierRepository _whSupplierRepository;
        private readonly AbstractValidator<WhSupplierCreateDto> _validationCreateRules;
        private readonly AbstractValidator<WhSupplierModifyDto> _validationModifyRules;

        public WhSupplierService(ICurrentUser currentUser,IWhSupplierRepository whSupplierRepository, AbstractValidator<WhSupplierCreateDto> validationCreateRules, AbstractValidator<WhSupplierModifyDto> validationModifyRules)
        {
            _currentUser = currentUser;
            _whSupplierRepository = whSupplierRepository;
            _validationCreateRules = validationCreateRules;
            _validationModifyRules = validationModifyRules;
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="whSupplierDto"></param>
        /// <returns></returns>
        public async Task CreateWhSupplierAsync(WhSupplierCreateDto whSupplierCreateDto)
        {
            //验证DTO
            await _validationCreateRules.ValidateAndThrowAsync(whSupplierCreateDto);

            //DTO转换实体
            var whSupplierEntity = whSupplierCreateDto.ToEntity<WhSupplierEntity>();
            whSupplierEntity.Id= IdGenProvider.Instance.CreateId();
            whSupplierEntity.CreatedBy = _currentUser.UserName;
            whSupplierEntity.UpdatedBy = _currentUser.UserName;
            whSupplierEntity.CreatedOn = HymsonClock.Now();
            whSupplierEntity.UpdatedOn = HymsonClock.Now();

            //入库
            await _whSupplierRepository.InsertAsync(whSupplierEntity);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteWhSupplierAsync(long id)
        {
            await _whSupplierRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeletesWhSupplierAsync(string ids)
        {
            var idsArr = StringExtension.SpitLongArrary(ids);
            return await _whSupplierRepository.DeletesAsync(idsArr);
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="whSupplierPagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<WhSupplierDto>> GetPageListAsync(WhSupplierPagedQueryDto whSupplierPagedQueryDto)
        {
            var whSupplierPagedQuery = whSupplierPagedQueryDto.ToQuery<WhSupplierPagedQuery>();
            var pagedInfo = await _whSupplierRepository.GetPagedInfoAsync(whSupplierPagedQuery);

            //实体到DTO转换 装载数据
            List<WhSupplierDto> whSupplierDtos = PrepareWhSupplierDtos(pagedInfo);
            return new PagedInfo<WhSupplierDto>(whSupplierDtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pagedInfo"></param>
        /// <returns></returns>
        private static List<WhSupplierDto> PrepareWhSupplierDtos(PagedInfo<WhSupplierEntity>   pagedInfo)
        {
            var whSupplierDtos = new List<WhSupplierDto>();
            foreach (var whSupplierEntity in pagedInfo.Data)
            {
                var whSupplierDto = whSupplierEntity.ToModel<WhSupplierDto>();
                whSupplierDtos.Add(whSupplierDto);
            }

            return whSupplierDtos;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="whSupplierDto"></param>
        /// <returns></returns>
        public async Task ModifyWhSupplierAsync(WhSupplierModifyDto whSupplierModifyDto)
        {
             //验证DTO
            await _validationModifyRules.ValidateAndThrowAsync(whSupplierModifyDto);

            //DTO转换实体
            var whSupplierEntity = whSupplierModifyDto.ToEntity<WhSupplierEntity>();
            whSupplierEntity.UpdatedBy = _currentUser.UserName;
            whSupplierEntity.UpdatedOn = HymsonClock.Now();

            await _whSupplierRepository.UpdateAsync(whSupplierEntity);
        }

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<WhSupplierDto> QueryWhSupplierByIdAsync(long id) 
        {
           var whSupplierEntity = await _whSupplierRepository.GetByIdAsync(id);
           if (whSupplierEntity != null) 
           {
               return whSupplierEntity.ToModel<WhSupplierDto>();
           }
            return null;
        }
    }
}
