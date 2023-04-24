/*
 *creator: Karl
 *
 *describe: 容器装载记录    服务 | 代码由框架生成
 *builder:  wxk
 *build datetime: 2023-04-12 02:32:21
 */
using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Services.Dtos.Manufacture;
using Hymson.Snowflake;
using Hymson.Utils;
using System.Collections.Generic;
using System.Transactions;

namespace Hymson.MES.Services.Services.Manufacture
{
    /// <summary>
    /// 容器装载记录 服务
    /// </summary>
    public class ManuContainerPackRecordService : IManuContainerPackRecordService
    {
        private readonly ICurrentUser _currentUser;
        private readonly ICurrentSite _currentSite;

        /// <summary>
        /// 容器装载记录 仓储
        /// </summary>
        private readonly IManuContainerPackRecordRepository _manuContainerPackRecordRepository;
        /// <summary>
        /// 容器条码表仓储接口
        /// </summary>
        private readonly IManuContainerBarcodeRepository _manuContainerBarcodeRepository;
        private readonly AbstractValidator<ManuContainerPackRecordCreateDto> _validationCreateRules;
        private readonly AbstractValidator<ManuContainerPackRecordModifyDto> _validationModifyRules;

        public ManuContainerPackRecordService(ICurrentUser currentUser, ICurrentSite currentSite,
            IManuContainerPackRecordRepository manuContainerPackRecordRepository,
            IManuContainerBarcodeRepository manuContainerBarcodeRepository,
            AbstractValidator<ManuContainerPackRecordCreateDto> validationCreateRules,
            AbstractValidator<ManuContainerPackRecordModifyDto> validationModifyRules)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _manuContainerPackRecordRepository = manuContainerPackRecordRepository;
            _manuContainerBarcodeRepository = manuContainerBarcodeRepository;
            _validationCreateRules = validationCreateRules;
            _validationModifyRules = validationModifyRules;
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="manuContainerPackRecordCreateDto"></param>
        /// <returns></returns>
        public async Task CreateManuContainerPackRecordAsync(ManuContainerPackRecordCreateDto manuContainerPackRecordCreateDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0)
            {
                throw new ValidationException(nameof(ErrorCode.MES10101));
            }

            //验证DTO
            await _validationCreateRules.ValidateAndThrowAsync(manuContainerPackRecordCreateDto);

            //DTO转换实体
            var manuContainerPackRecordEntity = manuContainerPackRecordCreateDto.ToEntity<ManuContainerPackRecordEntity>();
            manuContainerPackRecordEntity.Id = IdGenProvider.Instance.CreateId();
            manuContainerPackRecordEntity.CreatedBy = _currentUser.UserName;
            manuContainerPackRecordEntity.UpdatedBy = _currentUser.UserName;
            manuContainerPackRecordEntity.CreatedOn = HymsonClock.Now();
            manuContainerPackRecordEntity.UpdatedOn = HymsonClock.Now();
            manuContainerPackRecordEntity.SiteId = _currentSite.SiteId ?? 0;

            //入库
            await _manuContainerPackRecordRepository.InsertAsync(manuContainerPackRecordEntity);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteManuContainerPackRecordAsync(long id)
        {
            await _manuContainerPackRecordRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeletesManuContainerPackRecordAsync(long[] ids)
        {
            return await _manuContainerPackRecordRepository.DeletesAsync(new DeleteCommand { Ids = ids, DeleteOn = HymsonClock.Now(), UserId = _currentUser.UserName });
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="manuContainerPackRecordPagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ManuContainerPackRecordDto>> GetPagedListAsync(ManuContainerPackRecordPagedQueryDto manuContainerPackRecordPagedQueryDto)
        {
            var manuContainerPackRecordPagedQuery = manuContainerPackRecordPagedQueryDto.ToQuery<ManuContainerPackRecordPagedQuery>();
            manuContainerPackRecordPagedQuery.SiteId = _currentSite.SiteId ?? 0;
            var pagedInfo = await _manuContainerPackRecordRepository.GetPagedInfoAsync(manuContainerPackRecordPagedQuery);

            var containerIds = pagedInfo.Data.Select(x => x.ContainerBarCodeId.GetValueOrDefault()).ToArray();
            var containerBarcodeEntities = new List<ManuContainerBarcodeEntity>();
            if (containerIds.Any())
            {
                containerBarcodeEntities = (await _manuContainerBarcodeRepository.GetByIdsAsync(containerIds)).ToList();
            }

            //实体到DTO转换 装载数据
            List<ManuContainerPackRecordDto> manuContainerPackRecordDtos = PrepareManuContainerPackRecordDtos(pagedInfo, containerBarcodeEntities);
   
            return new PagedInfo<ManuContainerPackRecordDto>(manuContainerPackRecordDtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pagedInfo"></param>
        /// <returns></returns>
        private static List<ManuContainerPackRecordDto> PrepareManuContainerPackRecordDtos(PagedInfo<ManuContainerPackRecordEntity> pagedInfo, List<ManuContainerBarcodeEntity> containerBarcodeEntities)
        {
            var manuContainerPackRecordDtos = new List<ManuContainerPackRecordDto>();
            foreach (var manuContainerPackRecordEntity in pagedInfo.Data)
            {
                var manuContainerPackRecordDto = manuContainerPackRecordEntity.ToModel<ManuContainerPackRecordDto>();

                var containerBarcodeEntity=containerBarcodeEntities.FirstOrDefault(x => x.Id == manuContainerPackRecordEntity.ContainerBarCodeId);
                manuContainerPackRecordDto.BarCode = containerBarcodeEntity?.BarCode ?? "";

                manuContainerPackRecordDtos.Add(manuContainerPackRecordDto);
            }

            return manuContainerPackRecordDtos;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="manuContainerPackRecordDto"></param>
        /// <returns></returns>
        public async Task ModifyManuContainerPackRecordAsync(ManuContainerPackRecordModifyDto manuContainerPackRecordModifyDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0)
            {
                throw new ValidationException(nameof(ErrorCode.MES10101));
            }

            //验证DTO
            await _validationModifyRules.ValidateAndThrowAsync(manuContainerPackRecordModifyDto);

            //DTO转换实体
            var manuContainerPackRecordEntity = manuContainerPackRecordModifyDto.ToEntity<ManuContainerPackRecordEntity>();
            manuContainerPackRecordEntity.UpdatedBy = _currentUser.UserName;
            manuContainerPackRecordEntity.UpdatedOn = HymsonClock.Now();

            await _manuContainerPackRecordRepository.UpdateAsync(manuContainerPackRecordEntity);
        }

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ManuContainerPackRecordDto> QueryManuContainerPackRecordByIdAsync(long id)
        {
            var manuContainerPackRecordEntity = await _manuContainerPackRecordRepository.GetByIdAsync(id);
            if (manuContainerPackRecordEntity != null)
            {
                return manuContainerPackRecordEntity.ToModel<ManuContainerPackRecordDto>();
            }
            return null;
        }

        public async Task CreateManuContainerPackRecordsAsync(List<ManuContainerPackRecordCreateDto> manuContainerPackRecordCreateDtos)
        {
            if (manuContainerPackRecordCreateDtos != null && manuContainerPackRecordCreateDtos.Any())
            {
                // 判断是否有获取到站点码 
                if (_currentSite.SiteId == 0)
                {
                    throw new ValidationException(nameof(ErrorCode.MES10101));
                }
                var lst = new List<ManuContainerPackRecordEntity>();
                foreach (var manuContainerPackRecordCreateDto in manuContainerPackRecordCreateDtos)
                {

                    //验证DTO
                    await _validationCreateRules.ValidateAndThrowAsync(manuContainerPackRecordCreateDto);

                    //DTO转换实体
                    var manuContainerPackRecordEntity = manuContainerPackRecordCreateDto.ToEntity<ManuContainerPackRecordEntity>();
                    manuContainerPackRecordEntity.Id = IdGenProvider.Instance.CreateId();
                    manuContainerPackRecordEntity.CreatedBy = _currentUser.UserName;
                    manuContainerPackRecordEntity.UpdatedBy = _currentUser.UserName;
                    manuContainerPackRecordEntity.CreatedOn = HymsonClock.Now();
                    manuContainerPackRecordEntity.UpdatedOn = HymsonClock.Now();
                    manuContainerPackRecordEntity.SiteId = _currentSite.SiteId ?? 0;

                    lst.Add(manuContainerPackRecordEntity);
                }
                await _manuContainerPackRecordRepository.InsertsAsync(lst);
            }
        }
    }
}
