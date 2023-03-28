/*
 *creator: Karl
 *
 *describe: 产品不良录入    服务 | 代码由框架生成
 *builder:  zhaoqing
 *build datetime: 2023-03-27 03:49:17
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

namespace Hymson.MES.Services.Services.Manufacture
{
    /// <summary>
    /// 产品不良录入 服务
    /// </summary>
    public class ManuProductBadRecordService : IManuProductBadRecordService
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
        /// 产品不良录入 仓储
        /// </summary>
        private readonly IManuProductBadRecordRepository _manuProductBadRecordRepository;
        private readonly AbstractValidator<ManuProductBadRecordCreateDto> _validationCreateRules;
        private readonly AbstractValidator<ManuProductBadRecordModifyDto> _validationModifyRules;

        /// <summary>
        /// 构造函数
        /// </summary>
        public ManuProductBadRecordService(ICurrentUser currentUser, ICurrentSite currentSite,
        IManuProductBadRecordRepository manuProductBadRecordRepository,
        AbstractValidator<ManuProductBadRecordCreateDto> validationCreateRules,
        AbstractValidator<ManuProductBadRecordModifyDto> validationModifyRules)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _manuProductBadRecordRepository = manuProductBadRecordRepository;
            _validationCreateRules = validationCreateRules;
            _validationModifyRules = validationModifyRules;
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="manuProductBadRecordCreateDto"></param>
        /// <returns></returns>
        public async Task CreateManuProductBadRecordAsync(ManuProductBadRecordCreateDto manuProductBadRecordCreateDto)
        {
            //验证DTO
            await _validationCreateRules.ValidateAndThrowAsync(manuProductBadRecordCreateDto);

            //DTO转换实体
            var manuProductBadRecordEntity = manuProductBadRecordCreateDto.ToEntity<ManuProductBadRecordEntity>();
            manuProductBadRecordEntity.Id = IdGenProvider.Instance.CreateId();
            manuProductBadRecordEntity.CreatedBy = _currentUser.UserName;
            manuProductBadRecordEntity.UpdatedBy = _currentUser.UserName;

            //入库
            await _manuProductBadRecordRepository.InsertAsync(manuProductBadRecordEntity);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="idsArr"></param>
        /// <returns></returns>
        public async Task<int> DeletesManuProductBadRecordAsync(long[] idsArr)
        {
            if (idsArr.Length < 1)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10102));
            }
            var command = new DeleteCommand
            {
                UserId = _currentUser.UserName,
                DeleteOn = HymsonClock.Now(),
                Ids = idsArr
            };
            return await _manuProductBadRecordRepository.DeleteRangeAsync(command);
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="manuProductBadRecordPagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ManuProductBadRecordDto>> GetPageListAsync(ManuProductBadRecordPagedQueryDto manuProductBadRecordPagedQueryDto)
        {
            var manuProductBadRecordPagedQuery = manuProductBadRecordPagedQueryDto.ToQuery<ManuProductBadRecordPagedQuery>();
            manuProductBadRecordPagedQuery.SiteId = _currentSite.SiteId;
            var pagedInfo = await _manuProductBadRecordRepository.GetPagedInfoAsync(manuProductBadRecordPagedQuery);

            //实体到DTO转换 装载数据
            List<ManuProductBadRecordDto> manuProductBadRecordDtos = PrepareManuProductBadRecordDtos(pagedInfo);
            return new PagedInfo<ManuProductBadRecordDto>(manuProductBadRecordDtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 实体转换
        /// </summary>
        /// <param name="pagedInfo"></param>
        /// <returns></returns>
        private static List<ManuProductBadRecordDto> PrepareManuProductBadRecordDtos(PagedInfo<ManuProductBadRecordEntity> pagedInfo)
        {
            var manuProductBadRecordDtos = new List<ManuProductBadRecordDto>();
            foreach (var manuProductBadRecordEntity in pagedInfo.Data)
            {
                var manuProductBadRecordDto = manuProductBadRecordEntity.ToModel<ManuProductBadRecordDto>();
                manuProductBadRecordDtos.Add(manuProductBadRecordDto);
            }

            return manuProductBadRecordDtos;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="manuProductBadRecordModifyDto"></param>
        /// <returns></returns>
        public async Task ModifyManuProductBadRecordAsync(ManuProductBadRecordModifyDto manuProductBadRecordModifyDto)
        {
            //验证DTO
            await _validationModifyRules.ValidateAndThrowAsync(manuProductBadRecordModifyDto);

            //DTO转换实体
            var manuProductBadRecordEntity = manuProductBadRecordModifyDto.ToEntity<ManuProductBadRecordEntity>();
            manuProductBadRecordEntity.UpdatedBy = _currentUser.UserName;

            await _manuProductBadRecordRepository.UpdateAsync(manuProductBadRecordEntity);
        }

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ManuProductBadRecordDto> QueryManuProductBadRecordByIdAsync(long id)
        {
            var manuProductBadRecordEntity = await _manuProductBadRecordRepository.GetByIdAsync(id);
            if (manuProductBadRecordEntity != null)
            {
                return manuProductBadRecordEntity.ToModel<ManuProductBadRecordDto>();
            }
            return null;
        }
    }
}
