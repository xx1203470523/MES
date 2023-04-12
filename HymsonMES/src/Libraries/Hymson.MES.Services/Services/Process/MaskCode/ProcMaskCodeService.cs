using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Process.MaskCode;
using Hymson.MES.Data.Repositories.Process.MaskCode.Query;
using Hymson.MES.Services.Dtos.Process;
using Hymson.Sequences;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;
using System.Data;

namespace Hymson.MES.Services.Services.Process.MaskCode
{
    /// <summary>
    /// 服务（掩码维护）
    /// </summary>
    public class ProcMaskCodeService : IProcMaskCodeService
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
        /// 序列号生成服务
        /// </summary>
        private readonly ISequenceService _sequenceService;

        /// <summary>
        /// 验证器
        /// </summary>
        private readonly AbstractValidator<ProcMaskCodeSaveDto> _validationSaveRules;

        /// <summary>
        /// 
        /// </summary>
        private readonly IProcMaskCodeRepository _procMaskCodeRepository;
        private readonly IProcMaskCodeRuleRepository _procMaskCodeRuleRepository;


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="currentSite"></param>
        /// <param name="currentUser"></param>
        /// <param name="sequenceService"></param>
        /// <param name="validationSaveRules"></param>
        /// <param name="procMaskCodeRepository"></param>
        /// <param name="procMaskCodeRuleRepository"></param>
        public ProcMaskCodeService(ICurrentUser currentUser, ICurrentSite currentSite, ISequenceService sequenceService,
            AbstractValidator<ProcMaskCodeSaveDto> validationSaveRules,
            IProcMaskCodeRepository procMaskCodeRepository,
            IProcMaskCodeRuleRepository procMaskCodeRuleRepository)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _sequenceService = sequenceService;
            _validationSaveRules = validationSaveRules;
            _procMaskCodeRepository = procMaskCodeRepository;
            _procMaskCodeRuleRepository = procMaskCodeRuleRepository;
        }

        /// <summary>
        /// 添加（掩码维护）
        /// </summary>
        /// <param name="createDto"></param>
        /// <returns></returns>
        public async Task<int> CreateAsync(ProcMaskCodeSaveDto createDto)
        {
            // 验证DTO
            await _validationSaveRules.ValidateAndThrowAsync(createDto);

            // DTO转换实体
            var entity = createDto.ToEntity<ProcMaskCodeEntity>();
            entity.Id = IdGenProvider.Instance.CreateId();
            entity.CreatedBy = _currentUser.UserName;
            entity.UpdatedBy = _currentUser.UserName;
            entity.SiteId = _currentSite.SiteId ?? 0;

            entity.Code = entity.Code.Trim().Replace(" ", string.Empty);
            entity.Code = entity.Code.ToUpper();

            // 编码唯一性验证
            if (await _procMaskCodeRepository.IsExistsAsync(entity.Code)) throw new CustomerValidationException(nameof(ErrorCode.MES10802)).WithData("Code", entity.Code);

            List<ProcMaskCodeRuleEntity> rules = new();
            for (int i = 0; i < createDto.RuleList.Count; i++)
            {
                var item = createDto.RuleList[i].ToEntity<ProcMaskCodeRuleEntity>();
                item.Id = IdGenProvider.Instance.CreateId();
                item.SiteId = entity.SiteId;
                item.MaskCodeId = entity.Id;
                item.SerialNo = $"{i + 1}"; //_sequenceService.GetSerialNumberAsync(SerialNumberTypeEnum.None,''),
                item.CreatedBy = entity.CreatedBy;
                rules.Add(item);
            }

            // 保存实体
            var rows = 0;
            using (var trans = TransactionHelper.GetTransactionScope())
            {
                rows += await _procMaskCodeRepository.InsertAsync(entity);
                rows += await _procMaskCodeRuleRepository.InsertRangeAsync(rules);
                trans.Complete();
            }
            return rows;
        }

        /// <summary>
        /// 更新（掩码维护）
        /// </summary>
        /// <param name="modifyDto"></param>
        /// <returns></returns>
        public async Task<int> ModifyAsync(ProcMaskCodeSaveDto modifyDto)
        {
            // 验证DTO
            await _validationSaveRules.ValidateAndThrowAsync(modifyDto);

            // DTO转换实体
            var entity = modifyDto.ToEntity<ProcMaskCodeEntity>();
            entity.UpdatedBy = _currentUser.UserName;

            List<ProcMaskCodeRuleEntity> rules = new();
            for (int i = 0; i < modifyDto.RuleList.Count; i++)
            {
                var item = modifyDto.RuleList[i].ToEntity<ProcMaskCodeRuleEntity>();
                item.Id = IdGenProvider.Instance.CreateId();
                item.SiteId = entity.SiteId;
                item.MaskCodeId = entity.Id;
                item.SerialNo = $"{i + 1}"; //_sequenceService.GetSerialNumberAsync(SerialNumberTypeEnum.None,''),
                item.CreatedBy = entity.CreatedBy;
                rules.Add(item);
            }

            // 更新实体
            var rows = 0;
            using (var trans = TransactionHelper.GetTransactionScope())
            {
                rows += await _procMaskCodeRuleRepository.ClearByMaskCodeId(entity.Id);
                rows += await _procMaskCodeRuleRepository.InsertRangeAsync(rules);
                rows += await _procMaskCodeRepository.UpdateAsync(entity);
                trans.Complete();
            }
            return rows;
        }

        /// <summary>
        /// 删除（掩码维护）
        /// </summary>
        /// <param name="idsArr"></param>
        /// <returns></returns>
        public async Task<int> DeletesAsync(long[] idsArr)
        {
            return await _procMaskCodeRepository.DeletesAsync(new DeleteCommand
            {
                Ids = idsArr,
                UserId = _currentUser.UserName,
                DeleteOn = HymsonClock.Now()
            });
        }

        /// <summary>
        /// 获取分页数据（掩码维护）
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ProcMaskCodeDto>> GetPagedListAsync(ProcMaskCodePagedQueryDto pagedQueryDto)
        {
            var pagedQuery = pagedQueryDto.ToQuery<ProcMaskCodePagedQuery>();
            pagedQuery.SiteId = _currentSite.SiteId;
            var pagedInfo = await _procMaskCodeRepository.GetPagedListAsync(pagedQuery);

            // 实体到DTO转换 装载数据
            var dtos = pagedInfo.Data.Select(s => s.ToModel<ProcMaskCodeDto>());
            return new PagedInfo<ProcMaskCodeDto>(dtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 查询详情（掩码维护）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ProcMaskCodeDto> GetDetailAsync(long id)
        {
            var dto = (await _procMaskCodeRepository.GetByIdAsync(id)).ToModel<ProcMaskCodeDto>();
            if (dto != null)
            {
                dto.RuleList = (await _procMaskCodeRuleRepository.GetByMaskCodeIdAsync(dto.Id)).Select(s => s.ToModel<ProcMaskCodeRuleDto>());
            }
            return dto;
        }
    }
}
