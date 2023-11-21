using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Core.Enums;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.HttpClients;
using Hymson.MES.HttpClients.Requests.Print;
using Hymson.MES.Services.Dtos.Process;
using Hymson.Print.Abstractions;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;
using Newtonsoft.Json;
using System.ComponentModel;
using System.Reflection;
using System.Xml.Linq;

namespace Hymson.MES.Services.Services.Process.LabelTemplate
{
    /// <summary>
    /// 仓库标签模板 服务
    /// </summary>
    public class ProcLabelTemplateService : IProcLabelTemplateService
    {
        private readonly ICurrentUser _currentUser;
        private readonly ICurrentSite _currentSite;
        /// <summary>
        /// 仓库标签模板 仓储
        /// </summary>
        private readonly IProcLabelTemplateRepository _procLabelTemplateRepository;
        private readonly AbstractValidator<ProcLabelTemplateCreateDto> _validationCreateRules;
        private readonly AbstractValidator<ProcLabelTemplateModifyDto> _validationModifyRules;
        private readonly ILabelPrintRequest _labelPrintRequest;

        private readonly IProcLabelTemplateRelationRepository _procLabelTemplateRelationRepository;

        public ProcLabelTemplateService(ICurrentUser currentUser, ICurrentSite currentSite
            , IProcLabelTemplateRepository procLabelTemplateRepository
            , AbstractValidator<ProcLabelTemplateCreateDto> validationCreateRules
            , AbstractValidator<ProcLabelTemplateModifyDto> validationModifyRules, ILabelPrintRequest labelPrintRequest, IProcLabelTemplateRelationRepository procLabelTemplateRelationRepository)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _procLabelTemplateRepository = procLabelTemplateRepository;
            _validationCreateRules = validationCreateRules;
            _validationModifyRules = validationModifyRules;
            _labelPrintRequest = labelPrintRequest;
            _procLabelTemplateRelationRepository = procLabelTemplateRelationRepository;
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="procLabelTemplateCreateDto"></param>
        /// <returns></returns>
        public async Task CreateProcLabelTemplateAsync(ProcLabelTemplateCreateDto procLabelTemplateCreateDto)
        {
            procLabelTemplateCreateDto.Name = procLabelTemplateCreateDto.Name.ToTrimSpace();
            procLabelTemplateCreateDto.Path = procLabelTemplateCreateDto.Path.ToTrimSpace();
            procLabelTemplateCreateDto.Remark = procLabelTemplateCreateDto.Remark ?? "".Trim();
            //验证DTO
            await _validationCreateRules.ValidateAndThrowAsync(procLabelTemplateCreateDto);

            if (procLabelTemplateCreateDto.ProcLabelTemplateRelationCreateDto == null || string.IsNullOrEmpty(procLabelTemplateCreateDto.ProcLabelTemplateRelationCreateDto.PrintConfig))
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10372));
            }

            //DTO转换实体
            var procLabelTemplateEntity = procLabelTemplateCreateDto.ToEntity<ProcLabelTemplateEntity>();

            //验证模板名称是否重复
            var foo = await QueryProcLabelTemplateByNameAsync(procLabelTemplateEntity.Name);
            if (foo != null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10340)).WithData("Name", procLabelTemplateEntity.Name);
            }
            procLabelTemplateEntity.Id = IdGenProvider.Instance.CreateId();
            procLabelTemplateEntity.CreatedBy = _currentUser.UserName;
            procLabelTemplateEntity.UpdatedBy = _currentUser.UserName;
            procLabelTemplateEntity.CreatedOn = HymsonClock.Now();
            procLabelTemplateEntity.UpdatedOn = HymsonClock.Now();
            procLabelTemplateEntity.SiteId = _currentSite.SiteId ?? 0;

            //打印设计配置
            var procLabelTemplateRelationEntity = procLabelTemplateCreateDto.ProcLabelTemplateRelationCreateDto.ToEntity<ProcLabelTemplateRelationEntity>();
            procLabelTemplateRelationEntity.LabelTemplateId = procLabelTemplateEntity.Id;

            procLabelTemplateRelationEntity.Id = IdGenProvider.Instance.CreateId();
            procLabelTemplateRelationEntity.CreatedBy = _currentUser.UserName;
            procLabelTemplateRelationEntity.UpdatedBy = _currentUser.UserName;
            procLabelTemplateRelationEntity.CreatedOn = HymsonClock.Now();
            procLabelTemplateRelationEntity.UpdatedOn = HymsonClock.Now();
            procLabelTemplateRelationEntity.SiteId = _currentSite.SiteId ?? 0;

            using (var trans = TransactionHelper.GetTransactionScope())
            {

                //入库
                await _procLabelTemplateRepository.InsertAsync(procLabelTemplateEntity);

                await _procLabelTemplateRelationRepository.InsertAsync(procLabelTemplateRelationEntity);

                trans.Complete();
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteProcLabelTemplateAsync(long id)
        {
            await _procLabelTemplateRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="idsAr"></param>
        /// <returns></returns>
        public async Task<int> DeletesProcLabelTemplateAsync(long[] idsAr)
        {
            return await _procLabelTemplateRepository.DeletesAsync(idsAr);
        }

        public async Task<(string base64Str, bool result)> PreviewProcLabelTemplateAsync(long id)
        {
            var foo = await _procLabelTemplateRepository.GetByIdAsync(id);
            return await PreviewProcLabelTemplateAsync(foo.Content);


        }
        public async Task<(string base64Str, bool result)> PreviewProcLabelTemplateAsync(string content)
        {
            PreviewRequest request;
            if (string.IsNullOrEmpty(content))
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10350));
            }
            try
            {
                request = JsonConvert.DeserializeObject<PreviewRequest>(content)!;
            }
            catch
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES17005));
            }
            var result = await _labelPrintRequest.PreviewFromImageBase64Async(request ?? new PreviewRequest());
            if (!result.result)
                throw new CustomerValidationException(nameof(ErrorCode.MES17004));
            return result;


        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="procLabelTemplatePagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ProcLabelTemplateDto>> GetPageListAsync(ProcLabelTemplatePagedQueryDto procLabelTemplatePagedQueryDto)
        {
            var procLabelTemplatePagedQuery = procLabelTemplatePagedQueryDto.ToQuery<ProcLabelTemplatePagedQuery>();
            procLabelTemplatePagedQuery.SiteId = _currentSite.SiteId ?? 0;
            var pagedInfo = await _procLabelTemplateRepository.GetPagedInfoAsync(procLabelTemplatePagedQuery);

            //实体到DTO转换 装载数据
            List<ProcLabelTemplateDto> procLabelTemplateDtos = PrepareProcLabelTemplateDtos(pagedInfo);
            return new PagedInfo<ProcLabelTemplateDto>(procLabelTemplateDtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pagedInfo"></param>
        /// <returns></returns>
        private static List<ProcLabelTemplateDto> PrepareProcLabelTemplateDtos(PagedInfo<ProcLabelTemplateEntity> pagedInfo)
        {
            var procLabelTemplateDtos = new List<ProcLabelTemplateDto>();
            foreach (var procLabelTemplateEntity in pagedInfo.Data)
            {
                var procLabelTemplateDto = procLabelTemplateEntity.ToModel<ProcLabelTemplateDto>();
                procLabelTemplateDtos.Add(procLabelTemplateDto);
            }

            return procLabelTemplateDtos;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="procLabelTemplateModifyDto"></param>
        /// <returns></returns>
        public async Task ModifyProcLabelTemplateAsync(ProcLabelTemplateModifyDto procLabelTemplateModifyDto)
        {
            procLabelTemplateModifyDto.Name = procLabelTemplateModifyDto.Name.ToTrimSpace();
            procLabelTemplateModifyDto.Path = procLabelTemplateModifyDto.Path.ToTrimSpace();
            procLabelTemplateModifyDto.Remark = procLabelTemplateModifyDto.Remark ?? "".Trim();
            procLabelTemplateModifyDto.Content = procLabelTemplateModifyDto.Content ?? "".Trim();
            //验证DTO
            await _validationModifyRules.ValidateAndThrowAsync(procLabelTemplateModifyDto);

            if (procLabelTemplateModifyDto.ProcLabelTemplateRelationCreateDto == null || string.IsNullOrEmpty(procLabelTemplateModifyDto.ProcLabelTemplateRelationCreateDto.PrintConfig))
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10372));
            }

            //DTO转换实体
            var procLabelTemplateEntity = procLabelTemplateModifyDto.ToEntity<ProcLabelTemplateEntity>();
            //验证模板名称是否重复
            var foo = await QueryProcLabelTemplateByNameAsync(procLabelTemplateEntity.Name);
            if (foo != null && foo.Id != procLabelTemplateEntity.Id)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10340)).WithData("Name", procLabelTemplateEntity.Name);
            }

            procLabelTemplateEntity.UpdatedBy = _currentUser.UserName;
            procLabelTemplateEntity.UpdatedOn = HymsonClock.Now();


            //打印设计配置
            var procLabelTemplateRelationEntity = procLabelTemplateModifyDto.ProcLabelTemplateRelationCreateDto.ToEntity<ProcLabelTemplateRelationEntity>();
            procLabelTemplateRelationEntity.LabelTemplateId = procLabelTemplateEntity.Id;

            procLabelTemplateRelationEntity.Id = IdGenProvider.Instance.CreateId();
            procLabelTemplateRelationEntity.CreatedBy = _currentUser.UserName;
            procLabelTemplateRelationEntity.UpdatedBy = _currentUser.UserName;
            procLabelTemplateRelationEntity.CreatedOn = HymsonClock.Now();
            procLabelTemplateRelationEntity.UpdatedOn = HymsonClock.Now();
            procLabelTemplateRelationEntity.SiteId = _currentSite.SiteId ?? 0;

            using (var trans = TransactionHelper.GetTransactionScope())
            {
                await _procLabelTemplateRepository.UpdateAsync(procLabelTemplateEntity);

                //删除打印设计 然后新增
                await _procLabelTemplateRelationRepository.DeleteByLabelTemplateIdAsync(procLabelTemplateEntity.Id);
                await _procLabelTemplateRelationRepository.InsertAsync(procLabelTemplateRelationEntity);

                trans.Complete();
            }
        }

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ProcLabelTemplateDto> QueryProcLabelTemplateByIdAsync(long id)
        {
            var procLabelTemplateEntity = await _procLabelTemplateRepository.GetByIdAsync(id);
            if (procLabelTemplateEntity != null)
            {
                return procLabelTemplateEntity.ToModel<ProcLabelTemplateDto>();
            }
            return new ProcLabelTemplateDto();
        }
        private async Task<ProcLabelTemplateEntity> QueryProcLabelTemplateByNameAsync(string name)
        {
            return await _procLabelTemplateRepository.GetByNameAsync(new ProcLabelTemplateByNameQuery() { SiteId = _currentSite.SiteId ?? 0, Name = name });

        }

        /// <summary>
        /// 查询标签模板对应的打印设计信息
        /// </summary>
        /// <param name="labelTemplateId"></param>
        /// <returns></returns>
        public async Task<ProcLabelTemplateRelationDto?> QueryProcLabelTemplateRelationByLabelTemplateIdAsync(long labelTemplateId) 
        {
            var procLabelTemplateRelationEntity = await _procLabelTemplateRelationRepository.GetByLabelTemplateIdAsync(labelTemplateId);
            if (procLabelTemplateRelationEntity != null)
            {
                return procLabelTemplateRelationEntity.ToModel<ProcLabelTemplateRelationDto>();
            }

            return null;
        }

        /// <summary>
        /// 获取打印类对应的选项
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<PrintClassOptionDto>> GetPrintClassListAsync() 
        {
            var printClassList = new List<PrintClassOptionDto>();

             IEnumerable<Type> printClasses= await Task.FromResult(
                 Assembly.GetExecutingAssembly().GetTypes().Where(type => type.IsClass && !type.IsAbstract && typeof(BasePrintData).IsAssignableFrom(type))
            );

             foreach (var item in printClasses)
            {
                //var s = item.Attributes.GetDescription();
                //var ss = item.GetCustomAttributes(typeof(DescriptionAttribute), true)?.FirstOrDefault();

                var classDesc = "";
                var classDescriptionAttribute = Attribute.GetCustomAttribute(item, typeof(DescriptionAttribute));
                if (classDescriptionAttribute != null)
                {
                    classDesc = ((DescriptionAttribute)classDescriptionAttribute).Description;
                }
                else {
                    classDesc = item.Name;
                }

                #region 查询类下的属性
                var printClassPropertyOptions = new List<PrintClassPropertyOptionDto>();
                var properties = item.GetProperties();

                foreach (var property in properties)
                {
                    var descriptionAttribute = Attribute.GetCustomAttribute(property, typeof(DescriptionAttribute));

                    var propertyDesc = "";
                    var propertyName = string.IsNullOrEmpty(property.Name) ? "未知属性" : char.ToLower(property.Name[0]) + property.Name.Substring(1);//转为小驼峰
                    if (descriptionAttribute != null)
                    {
                        propertyDesc = ((DescriptionAttribute)descriptionAttribute).Description;
                    }
                    else
                    {
                        propertyDesc = propertyName;
                    }

                    printClassPropertyOptions.Add(new PrintClassPropertyOptionDto
                    {
                        Label = propertyDesc,
                        Value = propertyName
                    });
                }
                #endregion

                printClassList.Add(new PrintClassOptionDto
                {
                    Label= classDesc,
                    Value=item.FullName ?? item.Name,
                    PrintClassPropertyOptions = printClassPropertyOptions
                });
            }

            return  printClassList;
        }

        /// <summary>
        /// 获取测试模型对应的打印数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="CustomerValidationException"></exception>
        public async Task<TestPrintPrintDataDto> GetAboutPrintDataAsync(string id) 
        {
            if (string.IsNullOrEmpty(id)) 
            {
                throw new CustomerValidationException(ErrorCode.MES10374);
            }
            

            //获取打印模板
            var procLabelTemplateRelationEntity = await _procLabelTemplateRelationRepository.GetByLabelTemplateIdAsync(15528011388198912);

            var procDataDto = new ProcPrintTestPrintDto() {
                Id=123,
                ProcureCode="后台-1002",
                SupplierName="后台-yagao供应商",
                Num=123,
                WorkOrderType= "后台-"+PlanWorkOrderTypeEnum.TrialProduction.GetDescription(),
                OutTime=DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
            };

            return new TestPrintPrintDataDto() { ProcLabelTemplateRelationDto= procLabelTemplateRelationEntity.ToModel<ProcLabelTemplateRelationDto>(),
                ProcPrintTestPrintDto= procDataDto
            };
        }

    }
}
