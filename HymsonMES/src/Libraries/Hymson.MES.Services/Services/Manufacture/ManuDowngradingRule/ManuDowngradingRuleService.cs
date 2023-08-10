/*
 *creator: Karl
 *
 *describe: 降级规则    服务 | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-08-07 02:00:57
 */
using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Integrated;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Integrated;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Services.Dtos.Manufacture;
using Hymson.Snowflake;
using Hymson.Utils;
using System.Transactions;

namespace Hymson.MES.Services.Services.Manufacture
{
    /// <summary>
    /// 降级规则 服务
    /// </summary>
    public class ManuDowngradingRuleService : IManuDowngradingRuleService
    {
        private readonly ICurrentUser _currentUser;
        private readonly ICurrentSite _currentSite;

        /// <summary>
        /// 降级规则 仓储
        /// </summary>
        private readonly IManuDowngradingRuleRepository _manuDowngradingRuleRepository;
        private readonly AbstractValidator<ManuDowngradingRuleCreateDto> _validationCreateRules;
        private readonly AbstractValidator<ManuDowngradingRuleModifyDto> _validationModifyRules;

        public ManuDowngradingRuleService(ICurrentUser currentUser, ICurrentSite currentSite, IManuDowngradingRuleRepository manuDowngradingRuleRepository, AbstractValidator<ManuDowngradingRuleCreateDto> validationCreateRules, AbstractValidator<ManuDowngradingRuleModifyDto> validationModifyRules)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _manuDowngradingRuleRepository = manuDowngradingRuleRepository;
            _validationCreateRules = validationCreateRules;
            _validationModifyRules = validationModifyRules;
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="manuDowngradingRuleCreateDto"></param>
        /// <returns></returns>
        public async Task CreateManuDowngradingRuleAsync(ManuDowngradingRuleCreateDto manuDowngradingRuleCreateDto)
        {
            //验证DTO
            await _validationCreateRules.ValidateAndThrowAsync(manuDowngradingRuleCreateDto);

            //验证是否编码唯一
            var entity = await _manuDowngradingRuleRepository.GetByCodeAsync(new ManuDowngradingRuleCodeQuery
            {
                Code = manuDowngradingRuleCreateDto.Code.Trim(),
                SiteId = _currentSite.SiteId ?? 0
            });
            if (entity != null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES21102));
            }

            //DTO转换实体
            var manuDowngradingRuleEntity = manuDowngradingRuleCreateDto.ToEntity<ManuDowngradingRuleEntity>();
            manuDowngradingRuleEntity.Id= IdGenProvider.Instance.CreateId();
            manuDowngradingRuleEntity.CreatedBy = _currentUser.UserName;
            manuDowngradingRuleEntity.UpdatedBy = _currentUser.UserName;
            manuDowngradingRuleEntity.CreatedOn = HymsonClock.Now();
            manuDowngradingRuleEntity.UpdatedOn = HymsonClock.Now();
            manuDowngradingRuleEntity.SiteId = _currentSite.SiteId ?? 0;

            //处理 排序
            var maxSeriEntity= await _manuDowngradingRuleRepository.GetMaxSerialNumberAsync(_currentSite.SiteId??0);//查询数据中最后的一个排序号
            manuDowngradingRuleEntity.SerialNumber = maxSeriEntity!=null?maxSeriEntity.SerialNumber+1:1;

            //入库
            await _manuDowngradingRuleRepository.InsertAsync(manuDowngradingRuleEntity);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteManuDowngradingRuleAsync(long id)
        {
            await _manuDowngradingRuleRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeletesManuDowngradingRuleAsync(long[] ids)
        {
            return await _manuDowngradingRuleRepository.DeletesAsync(new DeleteCommand { Ids = ids, DeleteOn = HymsonClock.Now(), UserId = _currentUser.UserName });
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="manuDowngradingRulePagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ManuDowngradingRuleDto>> GetPagedListAsync(ManuDowngradingRulePagedQueryDto manuDowngradingRulePagedQueryDto)
        {
            var manuDowngradingRulePagedQuery = manuDowngradingRulePagedQueryDto.ToQuery<ManuDowngradingRulePagedQuery>();
            manuDowngradingRulePagedQuery.SiteId = _currentSite.SiteId??0;
            var pagedInfo = await _manuDowngradingRuleRepository.GetPagedInfoAsync(manuDowngradingRulePagedQuery);

            //实体到DTO转换 装载数据
            List<ManuDowngradingRuleDto> manuDowngradingRuleDtos = PrepareManuDowngradingRuleDtos(pagedInfo);
            return new PagedInfo<ManuDowngradingRuleDto>(manuDowngradingRuleDtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pagedInfo"></param>
        /// <returns></returns>
        private static List<ManuDowngradingRuleDto> PrepareManuDowngradingRuleDtos(PagedInfo<ManuDowngradingRuleEntity>   pagedInfo)
        {
            var manuDowngradingRuleDtos = new List<ManuDowngradingRuleDto>();
            foreach (var manuDowngradingRuleEntity in pagedInfo.Data)
            {
                var manuDowngradingRuleDto = manuDowngradingRuleEntity.ToModel<ManuDowngradingRuleDto>();
                manuDowngradingRuleDtos.Add(manuDowngradingRuleDto);
            }

            return manuDowngradingRuleDtos;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="manuDowngradingRuleDto"></param>
        /// <returns></returns>
        public async Task ModifyManuDowngradingRuleAsync(ManuDowngradingRuleModifyDto manuDowngradingRuleModifyDto)
        {
             //验证DTO
            await _validationModifyRules.ValidateAndThrowAsync(manuDowngradingRuleModifyDto);

            //DTO转换实体
            var manuDowngradingRuleEntity = manuDowngradingRuleModifyDto.ToEntity<ManuDowngradingRuleEntity>();
            manuDowngradingRuleEntity.UpdatedBy = _currentUser.UserName;
            manuDowngradingRuleEntity.UpdatedOn = HymsonClock.Now();

            await _manuDowngradingRuleRepository.UpdateAsync(manuDowngradingRuleEntity);
        }

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ManuDowngradingRuleDto> QueryManuDowngradingRuleByIdAsync(long id) 
        {
           var manuDowngradingRuleEntity = await _manuDowngradingRuleRepository.GetByIdAsync(id);
           if (manuDowngradingRuleEntity != null) 
           {
               return manuDowngradingRuleEntity.ToModel<ManuDowngradingRuleDto>();
           }
            return null;
        }

        /// <summary>
        /// 获取所有降级规则数据
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<ManuDowngradingRuleEntity>> GetAllManuDowngradingRuleEntitiesAsync() 
        {
            var allRuleList= await _manuDowngradingRuleRepository.GetManuDowngradingRuleEntitiesAsync(new ManuDowngradingRuleQuery() 
            {
                SiteId=_currentSite.SiteId??0
            });

            allRuleList = allRuleList.OrderBy(x => x.SerialNumber);

            return allRuleList;
        }

        /// <summary>
        /// 修改降级规则的排序号
        /// </summary>
        /// <param name="manuDowngradingRuleChangeSerialNumberDtos"></param>
        /// <returns></returns>
        /// <exception cref="CustomerValidationException"></exception>
        public async Task UpdateSerialNumbersAsync(List<ManuDowngradingRuleChangeSerialNumberDto> manuDowngradingRuleChangeSerialNumberDtos) 
        {
            if (!manuDowngradingRuleChangeSerialNumberDtos.Any()) 
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES21110));
            }

            List<ManuDowngradingRuleEntity> entitys=new List<ManuDowngradingRuleEntity>();
            foreach (var item in manuDowngradingRuleChangeSerialNumberDtos)
            {
                entitys.Add(new ManuDowngradingRuleEntity()
                {
                    Id = item.Id,
                    SerialNumber = item.SerialNumber,

                    UpdatedBy = _currentUser.UserName,
                    UpdatedOn = HymsonClock.Now()
                });
            }
            await _manuDowngradingRuleRepository.UpdateSerialNumbersAsync(entitys);
        }
    }
}
