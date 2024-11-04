using FluentValidation;
using FluentValidation.Results;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.Localization.Services;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Core.Enums.Integrated;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.MES.Data.Repositories.Process.MaskCode;
using Hymson.MES.Data.Repositories.Process.MaskCode.Query;
using Hymson.MES.Services.Dtos.Process;
using Hymson.Sequences;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;
using Microsoft.IdentityModel.Tokens;
using Minio.DataModel;
using System.Data;
using System.Text;

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
        private readonly ILocalizationService _localizationService;

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
        public ProcMaskCodeService(ICurrentUser currentUser, ICurrentSite currentSite, ISequenceService sequenceService,
            ILocalizationService localizationService,
            AbstractValidator<ProcMaskCodeSaveDto> validationSaveRules,
            IProcMaskCodeRepository procMaskCodeRepository,
            IProcMaskCodeRuleRepository procMaskCodeRuleRepository)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _sequenceService = sequenceService;
            _localizationService = localizationService;
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
            createDto.Code = createDto.Code.ToTrimSpace().ToUpperInvariant();
            createDto.Name = createDto.Name.Trim();
            createDto.Remark = createDto.Remark.Trim();
            // 验证DTO
            await _validationSaveRules.ValidateAndThrowAsync(createDto);

            // DTO转换实体
            var entity = createDto.ToEntity<ProcMaskCodeEntity>();
            entity.Id = IdGenProvider.Instance.CreateId();
            entity.CreatedBy = _currentUser.UserName;
            entity.UpdatedBy = _currentUser.UserName;
            entity.SiteId = _currentSite.SiteId ?? 123456;

            // 编码唯一性验证
            var checkEntity = await _procMaskCodeRepository.GetByCodeAsync(new EntityByCodeQuery { Site = entity.SiteId, Code = entity.Code });
            if (checkEntity != null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10802)).WithData("Code", entity.Code);
            }

            // 验证规则
            Verification(createDto.RuleList.ToList());

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
            modifyDto.Name = modifyDto.Name.Trim();
            modifyDto.Remark = modifyDto.Remark.Trim();
            // 验证DTO
            await _validationSaveRules.ValidateAndThrowAsync(modifyDto);

            // DTO转换实体
            var entity = modifyDto.ToEntity<ProcMaskCodeEntity>();
            entity.UpdatedBy = _currentUser.UserName;

            //验证规则
            Verification(modifyDto.RuleList.ToList());

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
            pagedQuery.SiteId = _currentSite.SiteId ?? 123456;
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

        /// <summary>
        /// 新增、修改规则校验
        /// </summary>
        /// <param name="linkeRuleList"></param>
        /// <returns></returns>
        private void Verification(List<ProcMaskCodeRuleDto> linkeRuleList)
        {
            if (linkeRuleList.Any(a => string.IsNullOrWhiteSpace(a.Rule))) throw new CustomerValidationException(nameof(ErrorCode.MES10803));
            if (linkeRuleList.Any(a => a.MatchWay < 0)) throw new CustomerValidationException(nameof(ErrorCode.MES10804));
            ////全码验证
            //if (linkeRuleList.Any(a => a.MatchWay == (int)MatchModeEnum .Whole&& a.Rule.Trim().Length != 10))
            //{
            //    throw new CustomerValidationException(nameof(ErrorCode.MES10805));
            //}
            var validationFailures = new List<FluentValidation.Results.ValidationFailure>();

            //全码:4,起始:1,结束:3,中间:2;根据匹配方式校验规则
            //全码：系统会校验所有的字符及字符长度，允许“?”
            //起始：系统只会校验掩码的起始字符，不校验长度，结束位不为‘?’；
            //结束：系统只会校验掩码的结束字符，不校验长度，起始字符不为‘?’；
            //中间：系统只会校验掩码的中间字符，不校验长度，结束和起始为不为‘?’；
            var errorMessage = new StringBuilder();
            foreach (var rule in linkeRuleList)
            {
                var validationFailure = new FluentValidation.Results.ValidationFailure();
                if (!Enum.IsDefined(typeof(MatchModeEnum), rule.MatchWay))
                {
                    validationFailure = new FluentValidation.Results.ValidationFailure();
                    if (validationFailure.FormattedMessagePlaceholderValues == null || !validationFailure.FormattedMessagePlaceholderValues.Any())
                    {
                        validationFailure.FormattedMessagePlaceholderValues = new Dictionary<string, object> {
                               { "CollectionIndex", rule.SerialNo}
                               };
                    }
                    else
                    {
                        validationFailure.FormattedMessagePlaceholderValues.Add("CollectionIndex", rule.SerialNo);
                    }
                    validationFailure.ErrorCode = nameof(ErrorCode.MES10809);
                    validationFailures.Add(validationFailure);
                    continue;
                }
                switch (rule.MatchWay)
                {
                    case MatchModeEnum.Start:
                        if (rule.Rule.EndsWith("?"))
                        {
                            validationFailure = new FluentValidation.Results.ValidationFailure();
                            if (validationFailure.FormattedMessagePlaceholderValues == null || !validationFailure.FormattedMessagePlaceholderValues.Any())
                            {
                                validationFailure.FormattedMessagePlaceholderValues = new Dictionary<string, object> {
                               { "CollectionIndex", rule.SerialNo}
                               };
                            }
                            else
                            {
                                validationFailure.FormattedMessagePlaceholderValues.Add("CollectionIndex", rule.SerialNo);
                            }
                            validationFailure.ErrorCode = nameof(ErrorCode.MES10806);
                            validationFailures.Add(validationFailure);
                            // errorMessage.Append(_localizationService.GetResource(nameof(ErrorCode.MES10806)));
                        }
                        break;
                    case MatchModeEnum.Middle:
                        if (rule.Rule.StartsWith("?") || rule.Rule.EndsWith("?"))
                        {
                            validationFailure = new FluentValidation.Results.ValidationFailure();
                            if (validationFailure.FormattedMessagePlaceholderValues == null || !validationFailure.FormattedMessagePlaceholderValues.Any())
                            {
                                validationFailure.FormattedMessagePlaceholderValues = new Dictionary<string, object> {
                               { "CollectionIndex", rule.SerialNo}
                               };
                            }
                            else
                            {
                                validationFailure.FormattedMessagePlaceholderValues.Add("CollectionIndex", rule.SerialNo);
                            }
                            validationFailure.ErrorCode = nameof(ErrorCode.MES10807);
                            validationFailures.Add(validationFailure);
                            //errorMessage.Append($"中间方式掩码首位和末尾不能为特殊字符\"?\";");
                            // errorMessage.Append(_localizationService.GetResource(nameof(ErrorCode.MES10807)));
                        }
                        break;
                    case MatchModeEnum.End:
                        if (rule.Rule.StartsWith("?"))
                        {
                            validationFailure = new FluentValidation.Results.ValidationFailure();
                            if (validationFailure.FormattedMessagePlaceholderValues == null || !validationFailure.FormattedMessagePlaceholderValues.Any())
                            {
                                validationFailure.FormattedMessagePlaceholderValues = new Dictionary<string, object> {
                               { "CollectionIndex", rule.SerialNo}
                               };
                            }
                            else
                            {
                                validationFailure.FormattedMessagePlaceholderValues.Add("CollectionIndex", rule.SerialNo);
                            }
                            validationFailure.ErrorCode = nameof(ErrorCode.MES10808);
                            validationFailures.Add(validationFailure);
                            // errorMessage.Append($"结束方式掩码首位不能为特殊字符\"?\";");
                            // errorMessage.Append(_localizationService.GetResource(nameof(ErrorCode.MES10808)));
                        }
                        break;
                    default:
                        break;
                }
            }

            if (validationFailures.Any())
            {
                throw new ValidationException(_localizationService.GetResource("MaskCodeError"), validationFailures);
            }

            //if (!string.IsNullOrWhiteSpace(errorMessage.ToString()))
            //{
            //    throw new CustomerValidationException(errorMessage.ToString());
            //}
        }
    }
}
