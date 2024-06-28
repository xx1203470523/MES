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
using Hymson.MES.Core.Domain.Equipment;
using Hymson.MES.Core.Domain.Integrated;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Integrated;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.MES.Data.Repositories.Equipment;
using Hymson.MES.Data.Repositories.Integrated;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Services.Dtos.Equipment;
using Hymson.MES.Services.Dtos.Process;
using Hymson.MES.Services.Dtos.Quality;
using Hymson.Snowflake;
using Hymson.SqlActuator.Services;
using Hymson.Utils;
using Hymson.Utils.Tools;
using System.Security.Policy;
using System.Transactions;

namespace Hymson.MES.Services.Services.Equipment.EquToolingManage
{
    /// <summary>
    /// 工具管理 服务业务层
    /// </summary>
    public partial class EquToolingManageService : IEquToolingManageService
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
        /// 工具管理表 仓储
        /// </summary>
        private readonly IEquToolingManageRepository _equToolingManageRepository;
        /// <summary>
        /// 工序配置作业表仓储
        /// </summary>
        private readonly IInteJobBusinessRelationRepository _jobBusinessRelationRepository;
        /// <summary>
        /// 物料维护 仓储
        /// </summary>
        private readonly IProcMaterialRepository _procMaterialRepository;

        /// <summary>
        /// 多语言服务
        /// </summary>
        private readonly ILocalizationService _localizationService;
        /// <summary>
        /// sql执行器
        /// </summary>
        private readonly ISqlExecuteTaskService _sqlExecuteTaskService;


        /// <summary>
        /// 仓储接口（工序复投设置）
        /// </summary>
        private readonly IProcProcedureRejudgeRepository _procProcedureRejudgeRepository;


        /// <summary>
        /// 构造函数
        /// </summary>
        public EquToolingManageService(
            ICurrentUser currentUser, ICurrentSite currentSite,
            IProcProcedureRepository procProcedureRepository,
            IEquToolingManageRepository equToolingManageRepository,
            IInteJobBusinessRelationRepository jobBusinessRelationRepository,
            IProcMaterialRepository procMaterialRepository,
            IProcProcedureRejudgeRepository procProcedureRejudgeRepository,
            ILocalizationService localizationService, ISqlExecuteTaskService sqlExecuteTaskService)
        {
            _equToolingManageRepository = equToolingManageRepository;
            _currentUser = currentUser;
            _currentSite = currentSite;
            _procProcedureRepository = procProcedureRepository;
            _jobBusinessRelationRepository = jobBusinessRelationRepository;
            _procMaterialRepository = procMaterialRepository;
            _localizationService = localizationService;
            _sqlExecuteTaskService = sqlExecuteTaskService;
            _procProcedureRejudgeRepository = procProcedureRejudgeRepository;
        }


        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="procProcedurePagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<EquToolingManageViewDto>> GetPageListAsync(EquToolingManagePagedQueryDto procProcedurePagedQueryDto)
        {
            var procProcedurePagedQuery = procProcedurePagedQueryDto.ToQuery<IEquToolingManagePagedQuery>();
            procProcedurePagedQuery.SiteId = _currentSite.SiteId ?? 0;
            var pagedInfo = await _equToolingManageRepository.GetPagedInfoAsync(procProcedurePagedQuery);
            //实体到DTO转换 装载数据
            List<EquToolingManageViewDto> procProcedureDtos = PrepareProcConversionFactorDtos(pagedInfo);
            return new PagedInfo<EquToolingManageViewDto>(procProcedureDtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }
      

        /// <summary>
        /// 分页实体转换
        /// </summary>
        /// <param name="pagedInfo"></param>
        /// <returns></returns>
        private static List<EquToolingManageViewDto> PrepareProcConversionFactorDtos(PagedInfo<EquToolingManageView> pagedInfo)
        {
            var procProcedureDtos = new List<EquToolingManageViewDto>();
            foreach (var procProcedureEntity in pagedInfo.Data)
            {
                var procProcedureDto = procProcedureEntity.ToModel<EquToolingManageViewDto>();
                procProcedureDtos.Add(procProcedureDto);
            }
            return procProcedureDtos;
        }

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<EquToolingManageViewDto> QueryProcConversionFactorByIdAsync(long id)
        {
            //查询工具信息
            var equToolingManageEntity = await _equToolingManageRepository.GetByIdAsync(id);
            if (equToolingManageEntity == null)
            {
                return new EquToolingManageViewDto();
            }
            //对象映射
            EquToolingManageViewDto equToolingManageViewDto = new EquToolingManageViewDto
            {
                Id = equToolingManageEntity.Id,
                Status= equToolingManageEntity.Status,
                Code= equToolingManageEntity.Code,
                Name = equToolingManageEntity.Name,
                ToolsId= equToolingManageEntity.ToolsId,
                CalibrationCycle = equToolingManageEntity.CalibrationCycle,
                IsCalibrated = equToolingManageEntity.IsCalibrated,
                RatedLife = equToolingManageEntity.RatedLife,
                RatedLifeUnit=equToolingManageEntity.RatedLifeUnit,
                ToolsTypeCode = equToolingManageEntity.ToolsTypeCode,
                CumulativeUsedLife = equToolingManageEntity.CumulativeUsedLife,
                ToolsTypeName = equToolingManageEntity.ToolsTypeName
            };
         
            //关联物料
            //var ConversionFactorLinkMaterials = await _procMaterialRepository.GetByIdAsync(procConversionFactorEntity.MaterialId);
            //if (ConversionFactorLinkMaterials != null)
            //{
            //    // 创建新的 ProcLoadPointLinkMaterialDto 对象并赋值
            //    var materialDto = new ProcLoadPointLinkMaterialDto
            //    {
            //        MaterialId = ConversionFactorLinkMaterials.Id,
            //        MaterialCode = ConversionFactorLinkMaterials.MaterialCode,
            //        MaterialName = ConversionFactorLinkMaterials.MaterialName

            //    };

            //    // 确保 LinkMaterials 不为 null
            //    if (converdionFactorDto.LinkMaterials == null)
            //    {
            //        converdionFactorDto.LinkMaterials = new List<ProcLoadPointLinkMaterialDto>();
            //    }

            //    // 将新的物料对象添加到 LinkMaterials 列表中
            //    converdionFactorDto.LinkMaterials.Add(materialDto);
            //    converdionFactorDto.MaterialCode = ConversionFactorLinkMaterials.MaterialCode;
            //    converdionFactorDto.MaterialName = ConversionFactorLinkMaterials.MaterialName;
            //    converdionFactorDto.Version = ConversionFactorLinkMaterials.Version;
            //    if (ConversionFactorLinkMaterials.Unit != null)
            //    {
            //        converdionFactorDto.Unit = ConversionFactorLinkMaterials.Unit;
            //    }
            //}
            return equToolingManageViewDto;
        }
    
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="AddConversionFactorDto"></param>
        /// <returns></returns>
        /// <exception cref="CustomerValidationException"></exception>
        public async Task<long> AddProcConversionFactorAsync(AddEquToolingManageDto AddConversionFactorDto)
        {
            //校验
            if (AddConversionFactorDto == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10100));
            }
           

            var equToolsEntity = AddConversionFactorDto.ToEntity<EquToolsEntity>();
            equToolsEntity.Id = IdGenProvider.Instance.CreateId();
            equToolsEntity.CreatedBy = _currentUser.UserName;
            equToolsEntity.UpdatedBy = _currentUser.UserName;
            equToolsEntity.CreatedOn = HymsonClock.Now();
            equToolsEntity.UpdatedOn = HymsonClock.Now();
            equToolsEntity.SiteId = _currentSite.SiteId ?? 0;


            #region 数据库验证
            var checkEntity = await _equToolingManageRepository.GetByCodeAsync(new EntityByCodeQuery
            {
                Site = equToolsEntity.SiteId,
                Code = equToolsEntity.Code
            });
            if (checkEntity != null) throw new CustomerValidationException(nameof(ErrorCode.MES10521)).WithData("Code", equToolsEntity.Code);
            #endregion

            using TransactionScope trans = TransactionHelper.GetTransactionScope();
            int response = 0;
            // 入库
            response = await _equToolingManageRepository.InsertAsync(equToolsEntity);
            if (response == 0)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10704));
            }

            trans.Complete();

            return equToolsEntity.Id;


        }
        


        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="idsAr"></param>
        /// <returns></returns>
        public async Task<int> DeleteProcConversionFactorAsync(long[] idsAr)
        {
            if (idsAr.Length < 1)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10102));
            }

            var entitys = await _equToolingManageRepository.GetByIdsAsync(idsAr);
            if (entitys != null && entitys.Any(a => a.Status != DisableOrEnableEnum.Disable))
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10135));
            }

            int rows = 0;
            using (TransactionScope ts = TransactionHelper.GetTransactionScope())
            {
                rows += await _equToolingManageRepository.DeletesAsync(new DeleteCommand
                {
                    Ids = idsAr,
                    DeleteOn = HymsonClock.Now(),
                    UserId = _currentUser.UserName
                });
                //rows += await _procProcedureRejudgeRepository.DeleteByParentIdAsync(idsAr);
                //rows += await _jobBusinessRelationRepository.DeleteByBusinessIdRangeAsync(idsAr);
                ts.Complete();
            }
            return rows;
        }


        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="equToolingManageModifyDto"></param>
        /// <returns></returns>
        public async Task ModifyProcConversionFactorAsync(EquToolingManageModifyDto equToolingManageModifyDto)
        {
            if (equToolingManageModifyDto == null) throw new CustomerValidationException(nameof(ErrorCode.MES10100));          

            // DTO转换实体
            var equToolsEntity = equToolingManageModifyDto.ToEntity<EquToolsEntity>();
            equToolsEntity.UpdatedBy = _currentUser.UserName;
            equToolsEntity.UpdatedOn = HymsonClock.Now();
            equToolsEntity.SiteId = _currentSite.SiteId ?? 0;
            //procConversionFactorEntity.MaterialId = procLoadPointModifyDto.LinkMaterials[0].MaterialId;
            #region 数据库验证
            //var isExists = (await _equToolingManageRepository.GetProcConversionFactorEntitiesAsync(new ProcConversionFactorQuery()
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
            rows = await _equToolingManageRepository.UpdateAsync(equToolsEntity);

            if (rows == 0)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10704));
            }

            ts.Complete();
        }


    }

}