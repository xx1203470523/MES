/*
 *creator: Karl
 *
 *describe: 供应商    服务 | 代码由框架生成
 *builder:  pengxin
 *build datetime: 2023-03-03 01:51:43
 */
using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Core.Domain.Warehouse;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.Warehouse;
using Hymson.MES.Services.Dtos.Warehouse;
using Hymson.Snowflake;
using Hymson.Utils;
using System.Text.RegularExpressions;
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
        private readonly ICurrentSite _currentSite;


        public WhSupplierService(ICurrentUser currentUser, IWhSupplierRepository whSupplierRepository, AbstractValidator<WhSupplierCreateDto> validationCreateRules, AbstractValidator<WhSupplierModifyDto> validationModifyRules, ICurrentSite currentSite)
        {
            _currentUser = currentUser;
            _whSupplierRepository = whSupplierRepository;
            _validationCreateRules = validationCreateRules;
            _validationModifyRules = validationModifyRules;
            _currentSite = currentSite;
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="whSupplierCreateDto"></param>
        /// <returns></returns>
        public async Task CreateWhSupplierAsync(WhSupplierCreateDto whSupplierCreateDto)
        {
            whSupplierCreateDto.Code = whSupplierCreateDto.Code.Replace(" ", "");
            whSupplierCreateDto.Name = whSupplierCreateDto.Name.Replace(" ", "");
            //验证DTO
            await _validationCreateRules.ValidateAndThrowAsync(whSupplierCreateDto);



            //英文和数字
            Regex reg = new Regex(@"^[A-Za-z0-9]+$");
            if (!reg.Match(whSupplierCreateDto.Code).Success)
            {
                throw new BusinessException(nameof(ErrorCode.MES15008)).WithData("Code", whSupplierCreateDto.Code);
            }
            whSupplierCreateDto.Code = whSupplierCreateDto.Code.ToUpper();
            //判断编号是否已经存在
            var exists = await _whSupplierRepository.GetWhSupplierEntitiesAsync(new WhSupplierQuery()
            {
                SiteId = _currentSite.SiteId ?? 123456,
                Code = whSupplierCreateDto.Code
            });
            if (exists != null && exists.Count() > 0)
            {
                throw new BusinessException(nameof(ErrorCode.MES15002)).WithData("Code", whSupplierCreateDto.Code);
            }

            //DTO转换实体
            var whSupplierEntity = whSupplierCreateDto.ToEntity<WhSupplierEntity>();
            whSupplierEntity.Id = IdGenProvider.Instance.CreateId();
            whSupplierEntity.CreatedBy = _currentUser.UserName;
            whSupplierEntity.UpdatedBy = _currentUser.UserName;
            whSupplierEntity.CreatedOn = HymsonClock.Now();
            whSupplierEntity.UpdatedOn = HymsonClock.Now();
            whSupplierEntity.SiteId = _currentSite.SiteId ?? 123456;


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
        public async Task<int> DeletesWhSupplierAsync(long[] ids)
        {
            if (ids == null || ids.Count() <= 0)
            {
                throw new ValidationException(nameof(ErrorCode.MES13005));
            }
            return await _whSupplierRepository.DeletesAsync(new DeleteCommand
            {
                Ids = ids,
                UserId = _currentUser.UserName,
                DeleteOn = HymsonClock.Now()
            });
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="whSupplierPagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<WhSupplierDto>> GetPageListAsync(WhSupplierPagedQueryDto whSupplierPagedQueryDto)
        {
            var whSupplierPagedQuery = whSupplierPagedQueryDto.ToQuery<WhSupplierPagedQuery>();
            whSupplierPagedQuery.SiteId = _currentSite.SiteId ?? 123456;
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
        private static List<WhSupplierDto> PrepareWhSupplierDtos(PagedInfo<WhSupplierEntity> pagedInfo)
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
        /// <param name="whSupplierModifyDto"></param>
        /// <returns></returns>
        public async Task ModifyWhSupplierAsync(WhSupplierModifyDto whSupplierModifyDto)
        {
            whSupplierModifyDto.Code = whSupplierModifyDto.Code.Replace(" ", "");
            whSupplierModifyDto.Name = whSupplierModifyDto.Name.Replace(" ", "");
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
                return new WhSupplierDto { Id = whSupplierEntity.Id, Code = whSupplierEntity.Code, Name = whSupplierEntity.Name, Remark = whSupplierEntity.Remark, CreatedBy = whSupplierEntity.CreatedBy, CreatedOn = whSupplierEntity.CreatedOn };
                //return whSupplierEntity.ToModel<WhSupplierDto>();
            }
            return null;
        }


        /// <summary>
        /// 根据ID查询(更改供应商)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<UpdateWhSupplierDto> QueryUpdateWhSupplierByIdAsync(long id)
        {
            var whSupplierEntity = await _whSupplierRepository.GetByIdAsync(id);
            if (whSupplierEntity != null)
            {
                return new UpdateWhSupplierDto { Id = whSupplierEntity.Id, Code = whSupplierEntity.Code, Name = whSupplierEntity.Name, Remark = whSupplierEntity.Remark };
            }
            return null;
        }
    }
}
