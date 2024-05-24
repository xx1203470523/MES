using Elastic.Clients.Elasticsearch;
using FluentValidation;
using FluentValidation.Results;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.Localization.Services;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Integrated;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Integrated;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Integrated;
using Hymson.MES.Data.Repositories.Integrated.IIntegratedRepository;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Manufacture.ManuFeeding;
using Hymson.MES.Data.Repositories.Parameter;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.Process.LoadPointLink.Query;
using Hymson.MES.Data.Repositories.Process.ProductSet.Query;
using Hymson.MES.Data.Repositories.Process.ResourceType;
using Hymson.MES.Data.Repositories.Quality;
using Hymson.MES.Services.Dtos.Common;
using Hymson.MES.Services.Dtos.Integrated;
using Hymson.MES.Services.Dtos.Process;
using Hymson.MES.Services.Dtos.Quality;
using Hymson.Snowflake;
using Hymson.SqlActuator.Services;
using Hymson.Utils;
using Hymson.Utils.Tools;
using Hymson.Web.Framework.Attributes;
using System.Security.Policy;
using System.Transactions;

namespace Hymson.MES.Services.Services.Process.Procedure
{
    /// <summary>
    /// 打印设置 服务
    /// </summary>
    public partial class ProcPrintSetupService : IProcPrintSetupService
    {
        /// <summary>
        /// 当前登录用户对象
        /// </summary>
        private readonly ICurrentUser _currentUser;
        /// <summary>
        /// 当前站点
        /// </summary>
        private readonly ICurrentSite _currentSite;
        /// <summary>
        /// 工序表 仓储
        /// </summary>
        private readonly IProcProcedureRepository _procProcedureRepository;
        /// <summary>
        /// 转换系数表 仓储
        /// </summary>
        private readonly IProcConversionFactorRepository _procConversionFactorRepository;
        /// <summary>
        /// 打印设置表
        /// </summary>
        private readonly IProcPrintSetupRepository _procPrintSetupRepository;
        /// <summary>
        /// 资源仓储
        /// </summary>
        private readonly IProcResourceRepository _resourceRepository;
        /// <summary>
        /// 资源类型仓储
        /// </summary>
        private readonly IProcResourceTypeRepository _resourceTypeRepository;
        /// <summary>
        /// 工序配置作业表仓储
        /// </summary>
        private readonly IInteJobBusinessRelationRepository _jobBusinessRelationRepository;
        /// <summary>
        /// 工序配置打印表仓储
        /// </summary>
        private readonly IProcProcedurePrintRelationRepository _procedurePrintRelationRepository;
        /// <summary>
        /// 物料维护 仓储
        /// </summary>
        private readonly IProcMaterialRepository _procMaterialRepository;
        /// <summary>
        /// 作业表仓储
        /// </summary>
        private readonly IInteJobRepository _inteJobRepository;
        /// <summary>
        /// 仓库标签模板 仓储
        /// </summary>
        private readonly IProcLabelTemplateRepository _procLabelTemplateRepository;
        /// <summary>
        /// 产出设置
        /// </summary>
        private readonly IProcProductSetRepository _procProductSetRepository;

        /// <summary>
        /// 多语言服务
        /// </summary>
        private readonly ILocalizationService _localizationService;
        /// <summary>
        /// sql执行器
        /// </summary>
        private readonly ISqlExecuteTaskService _sqlExecuteTaskService;

        /// <summary>
        /// 参数收集仓储
        /// </summary>
        private readonly IManuProductParameterRepository _manuProductParameterRepository;

        private readonly AbstractValidator<ProcProcedureCreateDto> _validationCreateRules;
        private readonly AbstractValidator<ProcProcedureModifyDto> _validationModifyRules;

        /// <summary>
        /// 仓储接口（工序复投设置）
        /// </summary>
        private readonly IProcProcedureRejudgeRepository _procProcedureRejudgeRepository;

        private readonly IQualUnqualifiedCodeRepository _qualUnqualifiedCodeRepository;

        private readonly IProcLoadPointLinkMaterialRepository _procLoadPointLinkMaterialRepository;
        /// <summary>
        /// 构造函数
        /// </summary>
        public ProcPrintSetupService(
            ICurrentUser currentUser, ICurrentSite currentSite,
            IProcProcedureRepository procProcedureRepository,
            IProcResourceRepository resourceRepository,
            IProcConversionFactorRepository procConversionFactorRepository,
            IProcPrintSetupRepository procPrintSetupRepository,
            IProcResourceTypeRepository resourceTypeRepository,
            IInteJobBusinessRelationRepository jobBusinessRelationRepository,
            IProcProcedurePrintRelationRepository procedurePrintRelationRepository,
            IProcMaterialRepository procMaterialRepository,
            IInteJobRepository inteJobRepository,
            IProcLabelTemplateRepository procLabelTemplateRepository,
            IProcProductSetRepository procProductSetRepository,
            IManuProductParameterRepository manuProductParameterRepository,
            IQualUnqualifiedCodeRepository qualUnqualifiedCodeRepository,
            IProcProcedureRejudgeRepository procProcedureRejudgeRepository,
            AbstractValidator<ProcProcedureCreateDto> validationCreateRules,
            AbstractValidator<ProcProcedureModifyDto> validationModifyRules, ILocalizationService localizationService, ISqlExecuteTaskService sqlExecuteTaskService)
        {
            _procPrintSetupRepository = procPrintSetupRepository;
            _procConversionFactorRepository = procConversionFactorRepository;
            _currentUser = currentUser;
            _currentSite = currentSite;
            _procProcedureRepository = procProcedureRepository;
            _resourceRepository = resourceRepository;
            _resourceTypeRepository = resourceTypeRepository;
            _jobBusinessRelationRepository = jobBusinessRelationRepository;
            _procedurePrintRelationRepository = procedurePrintRelationRepository;
            _procMaterialRepository = procMaterialRepository;
            _inteJobRepository = inteJobRepository;
            _procLabelTemplateRepository = procLabelTemplateRepository;
            _procProductSetRepository = procProductSetRepository;
            _manuProductParameterRepository = manuProductParameterRepository;
            _validationCreateRules = validationCreateRules;
            _validationModifyRules = validationModifyRules;
            _localizationService = localizationService;
            _sqlExecuteTaskService = sqlExecuteTaskService;
            _procProcedureRejudgeRepository = procProcedureRejudgeRepository;
            _qualUnqualifiedCodeRepository = qualUnqualifiedCodeRepository;
        }


        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="procProcedurePagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ProcPrintSetupViewDto>> GetPageListAsync(ProcPrintSetupPagedQueryDto procProcedurePagedQueryDto)
        {
                var procProcedurePagedQuery = procProcedurePagedQueryDto.ToQuery<IProcPrintSetupPagedQuery>();
                procProcedurePagedQuery.SiteId = _currentSite.SiteId ?? 0;
                var pagedInfo = await _procPrintSetupRepository.GetPagedInfoAsync(procProcedurePagedQuery);
                //实体到DTO转换 装载数据
                List<ProcPrintSetupViewDto> procProcedureDtos = PrepareProcConversionFactorDtos(pagedInfo);
                return new PagedInfo<ProcPrintSetupViewDto>(procProcedureDtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }
      

        /// <summary>
        /// 分页实体转换
        /// </summary>
        /// <param name="pagedInfo"></param>
        /// <returns></returns>
        private static List<ProcPrintSetupViewDto> PrepareProcConversionFactorDtos(PagedInfo<ProcPrintSetupView> pagedInfo)
        {
            var procProcedureDtos = new List<ProcPrintSetupViewDto>();
            foreach (var procProcedureEntity in pagedInfo.Data)
            {
                var procProcedureDto = procProcedureEntity.ToModel<ProcPrintSetupViewDto>();
                procProcedureDtos.Add(procProcedureDto);
            }

            return procProcedureDtos;         
        }

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ProcPrintSetupDto> QueryProcPrintSetupByIdAsync(long id)
        {
            //通过ID获取proc_print_configure表数据
            var procPrintSetupEntity = await _procPrintSetupRepository.GetByIdAsync(id);

            if (procPrintSetupEntity == null)
            {
                return new ProcPrintSetupDto();

            }

            //主表数据映射
            ProcPrintSetupDto converdionFactorDto = new ProcPrintSetupDto
            {
                Id = procPrintSetupEntity.Id,
                Status= procPrintSetupEntity.Status,
                BusinessType= procPrintSetupEntity.BusinessType,
                Type=procPrintSetupEntity.Type,
                //MaterialCode=procPrintSetupEntity.MaterialCode,
                //Version=procPrintSetupEntity.Version,
                ResCode=procPrintSetupEntity.ResCode,
                Name = procPrintSetupEntity.Name,
                Content=procPrintSetupEntity.Content,
                Count=procPrintSetupEntity.Count,
                Program=procPrintSetupEntity.Program,
                Remark = procPrintSetupEntity.Remark,
                PrintName=procPrintSetupEntity.PrintName,
                LabelTemplateId = procPrintSetupEntity.LabelTemplateId,
                ResourceId = procPrintSetupEntity.ResourceId,
                PrintId = procPrintSetupEntity.PrintId,
                MaterialId=procPrintSetupEntity.MaterialId,
                Class=procPrintSetupEntity.Class
            };
            //物料
            var printerSetupMaterial = await _procMaterialRepository.GetByIdAsync(procPrintSetupEntity.MaterialId);
            if (printerSetupMaterial != null)
            {
                converdionFactorDto.MaterialCode = printerSetupMaterial.MaterialCode;
                converdionFactorDto.Version = printerSetupMaterial.Version;
            }
            return converdionFactorDto;
        }

        /// <summary>
        /// 根据MaterialId查询
        /// </summary>
        /// <param name="materialId"></param>
        /// <returns></returns>
        public async Task<ProcPrintSetupDto> QueryProcPrintSetupByMaterialIdAsync(long materialId)
        {
            //通过ID获取proc_print_configure表数据
            var procPrintSetupEntity = await _procPrintSetupRepository.GetByMaterialIdAsync(materialId);

            if (procPrintSetupEntity == null)
            {
                return new ProcPrintSetupDto();

            }

            //主表数据映射
            ProcPrintSetupDto converdionFactorDto = new ProcPrintSetupDto
            {
                Id = procPrintSetupEntity.Id,
                Status = procPrintSetupEntity.Status,
                BusinessType = procPrintSetupEntity.BusinessType,
                Type = procPrintSetupEntity.Type,
                ResCode = procPrintSetupEntity.ResCode,
                Name = procPrintSetupEntity.Name,
                Content = procPrintSetupEntity.Content,
                Count = procPrintSetupEntity.Count,
                Program = procPrintSetupEntity.Program,
                Remark = procPrintSetupEntity.Remark,
                PrintName = procPrintSetupEntity.PrintName,
                LabelTemplateId = procPrintSetupEntity.LabelTemplateId,
                ResourceId = procPrintSetupEntity.ResourceId,
                PrintId = procPrintSetupEntity.PrintId,
                MaterialId = procPrintSetupEntity.MaterialId,
                Class = procPrintSetupEntity.Class
            };
            //物料
            var printerSetupMaterial = await _procMaterialRepository.GetByIdAsync(procPrintSetupEntity.MaterialId);
            if (printerSetupMaterial != null)
            {
                converdionFactorDto.MaterialCode = printerSetupMaterial.MaterialCode;
                converdionFactorDto.Version = printerSetupMaterial.Version;
            }
            return converdionFactorDto;
        }

        public async Task<long> AddProcPrintSetupAsync(AddPrintSetupDto AddPrintSetupDto)
        {
            //校验字段空值
            if (AddPrintSetupDto == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10100));
            }            

            //Dto值赋值给Entily
            var procPrintSetupEntity = AddPrintSetupDto.ToEntity<ProcPrintSetupEntity>();
            procPrintSetupEntity.MaterialId = AddPrintSetupDto.MaterialId;
            procPrintSetupEntity.LabelTemplateId = AddPrintSetupDto.LabelTemplateId;
            procPrintSetupEntity.ResourceId = AddPrintSetupDto.ResourceId;
            procPrintSetupEntity.Type = AddPrintSetupDto.Type;
            procPrintSetupEntity.BusinessType= AddPrintSetupDto.BusinessType;         
            procPrintSetupEntity.Count=AddPrintSetupDto.Count;           
            procPrintSetupEntity.Class = AddPrintSetupDto.Class;
            procPrintSetupEntity.Id = IdGenProvider.Instance.CreateId();
            procPrintSetupEntity.CreatedBy = _currentUser.UserName;
            procPrintSetupEntity.UpdatedBy = _currentUser.UserName;
            procPrintSetupEntity.CreatedOn = HymsonClock.Now();
            procPrintSetupEntity.UpdatedOn = HymsonClock.Now();
            procPrintSetupEntity.SiteId = _currentSite.SiteId ?? 0;

            procPrintSetupEntity.Status = AddPrintSetupDto.Status;

            #region 数据库验证
            var isExists = (await _procPrintSetupRepository.GetProcConversionFactorEntitiesAsync(new ProcPrintSetupQuery()
            {
                SiteId = procPrintSetupEntity.SiteId,
                MaterialId= procPrintSetupEntity.MaterialId
            })).Any();
            if (isExists)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10242)).WithData("MaterialId", procPrintSetupEntity.MaterialId);
            }
            #endregion

            using TransactionScope trans = TransactionHelper.GetTransactionScope();
            int response = 0;
            // 入库
            response = await _procPrintSetupRepository.InsertAsync(procPrintSetupEntity);
            if (response == 0)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10704));
            }

            trans.Complete();

            return procPrintSetupEntity.Id;


        }
        


        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="idsAr"></param>
        /// <returns></returns>
        public async Task<int> DeleteProcPrintSetupAsync(long[] idsAr)
        {
            if (idsAr.Length < 1)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10102));
            }

            var entitys = await _procPrintSetupRepository.GetByIdsAsync(idsAr);
            if (entitys != null && entitys.Any(a => a.Status != DisableOrEnableEnum.Disable))
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10135));
            }

            int rows = 0;
            using (TransactionScope ts = TransactionHelper.GetTransactionScope())
            {
                rows += await _procPrintSetupRepository.DeletesAsync(new DeleteCommand
                {
                    Ids = idsAr,
                    DeleteOn = HymsonClock.Now(),
                    UserId = _currentUser.UserName
                });
                ts.Complete();
            }
            return rows;
        }


        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="procPrintSetupModifyDto"></param>
        /// <returns></returns>
        public async Task ModifyProcPrintSetupAsync(ProcPrintSetupModifyDto procPrintSetupModifyDto)
        {
            if (procPrintSetupModifyDto == null) throw new CustomerValidationException(nameof(ErrorCode.MES10100));          

            // DTO转换实体
            var procConversionFactorEntity = procPrintSetupModifyDto.ToEntity<ProcPrintSetupEntity>();
            procConversionFactorEntity.UpdatedBy = _currentUser.UserName;
            procConversionFactorEntity.UpdatedOn = HymsonClock.Now();
            procConversionFactorEntity.SiteId = _currentSite.SiteId ?? 0;
            #region 数据库验证
            //var isExists = (await _procPrintSetupRepository.GetProcConversionFactorEntitiesAsync(new ProcConversionFactorQuery()
            //{
            //    SiteId = procConversionFactorEntity.SiteId,
            //    ProcedureId = procConversionFactorEntity.ProcedureId,
            //    MaterialId = procConversionFactorEntity.MaterialId
            //})).Any();
            //if (isExists)
            //{
            //    throw new CustomerValidationException(nameof(ErrorCode.MES10477)).WithData("ProcedureId", procConversionFactorEntity.ProcedureId);
            //}
            #endregion
            


          

            using TransactionScope ts = TransactionHelper.GetTransactionScope();
            int rows = 0;
            // 入库
            rows = await _procPrintSetupRepository.UpdateAsync(procConversionFactorEntity);
            if (rows == 0)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10704));
            }
            ts.Complete();
        }
    }

}