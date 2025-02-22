using Elastic.Clients.Elasticsearch.QueryDsl;
using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.EventBus.Abstractions;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Domain.Mavel.Rotor;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Process;
using Hymson.MES.Core.Enums.Quality;
using Hymson.MES.CoreServices.Dtos.Process.LabelTemplate.Utility;
using Hymson.MES.CoreServices.Events.ProcessEvents.PrintEvents;
using Hymson.MES.Data.Repositories.Common;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Manufacture.Query;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.QualFqcInspectionMaval;
using Hymson.MES.HttpClients.Options;
using Hymson.MES.Services.Dtos.Manufacture;
using Hymson.Sequences;
using Hymson.Snowflake;
using Hymson.Utils;
using Minio.DataModel;
using MySqlX.XDevAPI;
using Newtonsoft.Json;
using System.Runtime.CompilerServices;

namespace Hymson.MES.Services.Services.Manufacture
{
    /// <summary>
    /// 服务（定子装箱记录表） 
    /// </summary>
    public class ManuStatorPackListService : IManuStatorPackListService
    {
        /// <summary>
        /// 当前用户
        /// </summary>
        private readonly ICurrentUser _currentUser;
        /// <summary>
        /// 当前站点
        /// </summary>
        private readonly ICurrentSite _currentSite;

        /// <summary>
        /// 仓储接口（定子装箱记录表）
        /// </summary>
        private readonly IManuStatorPackListRepository _manuStatorPackListRepository;

        /// <summary>
        /// FQC
        /// </summary>
        private readonly IQualFqcInspectionMavalRepository _qualFqcInspectionMavalRepository;

        /// <summary>
        /// 序列号
        /// </summary>
        private readonly ISequenceService _sequenceService;

        /// <summary>
        /// 事件总线
        /// </summary>
        private readonly IEventBus<EventBusInstance2> _eventBus;

        /// <summary>
        /// 系统配置
        /// </summary>
        private readonly ISysConfigRepository _sysConfigRepository;

        /// <summary>
        /// 物料
        /// </summary>
        private readonly IProcMaterialRepository _procMaterialRepository;

        /// <summary>
        /// 资源
        /// </summary>
        private readonly IProcResourceRepository _procResourceRepository;

        /// <summary>
        /// 打印机
        /// </summary>
        private readonly IProcPrintConfigRepository _procPrintConfigRepository;

        /// <summary>
        /// 工序
        /// </summary>
        private readonly IProcProcedureRepository _procProcedureRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        public ManuStatorPackListService(ICurrentUser currentUser, ICurrentSite currentSite,
            IManuStatorPackListRepository manuStatorPackListRepository,
            IQualFqcInspectionMavalRepository qualFqcInspectionMavalRepository,
            ISequenceService sequenceService,
            IEventBus<EventBusInstance2> eventBus,
            ISysConfigRepository sysConfigRepository,
            IProcMaterialRepository procMaterialRepository,
            IProcResourceRepository procResourceRepository,
            IProcPrintConfigRepository procPrintConfigRepository,
            IProcProcedureRepository procProcedureRepository)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _manuStatorPackListRepository = manuStatorPackListRepository;
            _qualFqcInspectionMavalRepository = qualFqcInspectionMavalRepository;
            _sequenceService = sequenceService;
            _eventBus = eventBus;
            _sysConfigRepository = sysConfigRepository;
            _procMaterialRepository = procMaterialRepository;
            _procResourceRepository = procResourceRepository;
            _procPrintConfigRepository = procPrintConfigRepository;
            _procProcedureRepository = procProcedureRepository;
        }


        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        public async Task<int> CreateAsync(ManuStatorPackListSaveDto saveDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0) throw new CustomerValidationException(nameof(ErrorCode.MES10101));

            //1、装箱容量手工录入，扫描SN 条码总数不可大于装箱容量（提示：当前扫描数量大于装箱容量！）。
            //2、SN条码录入无需校验该SN是否在数据库中。
            //3、当录入SN条码点击回车后自动在下面包装列表中新增一条数据，且外箱码为系统自动生成。
            //4、当SN条码扫描数量不大于装箱容量时，可点击确定打印按钮，进行打印外箱标签。

            if(string.IsNullOrEmpty(saveDto.BoxCode) == true || string.IsNullOrEmpty(saveDto.ProductCode) == true)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES17520));
            }
            saveDto.BoxCode = saveDto.BoxCode.ToUpper();
            saveDto.ProductCode = saveDto.ProductCode.ToUpper();
            //查询箱体码已经装箱数量，并且判断数量
            var statorList = await _manuStatorPackListRepository.GetByBoxcodeAsync(saveDto.BoxCode);
            if(statorList != null && statorList.Count() > 0)
            {
                var firstStator = statorList.First();
                //判断容量是否和之前一致
                if (saveDto.BoxNum != firstStator.BoxNum)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES17522))
                        .WithData("BoxNum1", saveDto.BoxNum)
                        .WithData("BoxNum2", firstStator.BoxNum);
                }
                //数量是否已经超出
                if (statorList.Count() >= firstStator.BoxNum)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES17521)).WithData("BoxNum", firstStator.BoxNum);
                }
                
                // 检验不能混装
                foreach (var packEntity in statorList)
                {
                    if (!string.Equals(firstStator.QualStatus,packEntity.QualStatus))
                    {
                        throw new CustomerValidationException(nameof(ErrorCode.MES10247)).WithData("Status", firstStator.QualStatus);
                    }
                }
                
            }
            //查询成品码是否已经装箱
            var dbSfc = await _manuStatorPackListRepository.GetBySfcAsync(saveDto.ProductCode);
            if(dbSfc != null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES17523))
                    .WithData("sfc", saveDto.ProductCode)
                    .WithData("BoxCode", dbSfc.BoxCode);
            }
            //查询成品FQC状态
            List<string> sfcs = new List<string>() { saveDto.ProductCode };
            var qualFqcs = await _qualFqcInspectionMavalRepository.GetQualFqcInspectionMavalEntitiesAsync(new QualFqcInspectionMavalQuery
            {
                SiteId = _currentSite.SiteId ?? 0,
                SFCs = sfcs,
            });
            var qualFqc = qualFqcs.FirstOrDefault(x => x.SFC == saveDto.ProductCode);
            var type = ProductReceiptQualifiedStatusEnum.ToBeBnspected;
            if (qualFqc != null)
            {
                if (qualFqc.JudgmentResults == FqcJudgmentResultsEnum.Unqualified)
                {
                    type = ProductReceiptQualifiedStatusEnum.Unqualified;
                }
                if (qualFqc.JudgmentResults == FqcJudgmentResultsEnum.Qualified)
                {
                    type = ProductReceiptQualifiedStatusEnum.Qualified;
                }
            }

            // 更新时间
            var updatedBy = _currentUser.UserName;
            var updatedOn = HymsonClock.Now();

            // DTO转换实体
            var entity = saveDto.ToEntity<ManuStatorPackListEntity>();
            entity.Id = IdGenProvider.Instance.CreateId();
            entity.QualStatus = type.GetDescription();
            entity.CreatedBy = updatedBy;
            entity.CreatedOn = updatedOn;
            entity.UpdatedBy = updatedBy;
            entity.UpdatedOn = updatedOn;
            entity.SiteId = _currentSite.SiteId ?? 0;

            // 保存
            return await _manuStatorPackListRepository.InsertAsync(entity);
        }

        /// <summary>
        /// 生成箱体码
        /// </summary>
        /// <returns></returns>
        public async Task<string> AddBoxAsync()
        {
            string dateStr = HymsonClock.Now().ToString("yyyyMMdd");
            string serialNumKey = $"DBOX{dateStr}";
            int curKeyNum = await _sequenceService.GetSerialNumberAsync(Sequences.Enums.SerialNumberTypeEnum.ByDay, serialNumKey);
            if (curKeyNum == 0)
            {
                ++curKeyNum;
            }
            string boxCode = $"DBOX{dateStr}{curKeyNum.ToString().PadLeft(3, '0')}";
            return boxCode;
        }

        /// <summary>
        /// 打印
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<int> PrintAsync(ManuStatorPackPrintDto param)
        {
            //根据配置取工序，资源，打印机，产品
            if(string.IsNullOrEmpty(param.BoxCode) == true)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES17529));
            }

            //根据配置取产品数据
            SysConfigQuery configQuery = new SysConfigQuery();
            configQuery.Type = SysConfigEnum.NioBaseConfig;
            configQuery.Codes = new List<string>() { "NioStatorConfig" };
            var baseConfigList = await _sysConfigRepository.GetEntitiesAsync(configQuery);
            if (baseConfigList == null || !baseConfigList.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10139)).WithData("name", "NioStatorConfig");
            }
            string configValue = baseConfigList.ElementAt(0).Value;
            NIOConfigBaseDto curConfig = JsonConvert.DeserializeObject<NIOConfigBaseDto>(configValue);

            //获取定子打印配置
            var configEntities = await _sysConfigRepository.GetEntitiesAsync(new SysConfigQuery { Type = SysConfigEnum.StatorBox });
            if (configEntities == null || !configEntities.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10139)).WithData("name", "StatorBox");
            }
            string printConfig = configEntities.ElementAt(0).Value;
            List<string> printConfigList = printConfig.Split("&").ToList();

            string materialCode = curConfig.VendorProductCode;
            string procedureCode = printConfigList[0];
            string resourceCode = printConfigList[1];
            string printCode = printConfigList[2];

            EntityByCodeQuery matQuery = new EntityByCodeQuery() { Site = _currentSite.SiteId };

            matQuery.Code = materialCode;
            var dbMat = await _procMaterialRepository.GetByCodeAsync(matQuery);
            if (dbMat == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES17525)).WithData("mat", materialCode);
            }

            matQuery.Code = procedureCode;
            var dbProcedure = await _procProcedureRepository.GetByCodeAsync(matQuery);
            if (dbProcedure == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES17526)).WithData("op", procedureCode);
            }

            matQuery.Code = resourceCode;
            var dbResource = await _procResourceRepository.GetByCodeAsync(matQuery);
            if (dbResource == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES17527)).WithData("res", resourceCode);
            }

            matQuery.Code = printCode;
            var dbPrint = await _procPrintConfigRepository.GetByPrintNameAsync(matQuery);
            if (dbPrint == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES17528)).WithData("print", printCode);
            }

            _eventBus.Publish(new PrintIntegrationEvent
            {
                CurrencyTemplateType = CurrencyTemplateTypeEnum.DBox,
                SiteId = _currentSite.SiteId ?? 0,
                PrintId = dbPrint.Id,
                ProcedureId = dbProcedure.Id,
                ResourceId = dbResource.Id,
                BarCodes = new List<LabelTemplateBarCodeDto>
                {
                    new LabelTemplateBarCodeDto
                    {
                        BarCode = param.BoxCode,
                        MateriaId = dbMat.Id
                    }
                },
                UserName = _currentUser.UserName
            });

            return 0;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        public async Task<int> ModifyAsync(ManuStatorPackListSaveDto saveDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0) throw new CustomerValidationException(nameof(ErrorCode.MES10101));

            // DTO转换实体
            var entity = saveDto.ToEntity<ManuStatorPackListEntity>();
            entity.UpdatedBy = _currentUser.UserName;
            entity.UpdatedOn = HymsonClock.Now();

            return await _manuStatorPackListRepository.UpdateAsync(entity);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(long id)
        {
            return await _manuStatorPackListRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeletesAsync(long[] ids)
        {
            return await _manuStatorPackListRepository.DeletesAsync(new DeleteCommand
            {
                Ids = ids,
                DeleteOn = HymsonClock.Now(),
                UserId = _currentUser.UserName
            });
        }

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ManuStatorPackListDto?> QueryByIdAsync(long id) 
        {
           var manuStatorPackListEntity = await _manuStatorPackListRepository.GetByIdAsync(id);
           if (manuStatorPackListEntity == null) return null;
           
           return manuStatorPackListEntity.ToModel<ManuStatorPackListDto>();
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ManuStatorPackListDto>> GetPagedListAsync(ManuStatorPackListPagedQueryDto pagedQueryDto)
        {
            if(string.IsNullOrEmpty(pagedQueryDto.ProductCode) == true && string.IsNullOrEmpty(pagedQueryDto.BoxCode) == true)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES17524));
            }

            var pagedQuery = pagedQueryDto.ToQuery<ManuStatorPackListPagedQuery>();
            pagedQuery.SiteId = _currentSite.SiteId ?? 0;
            pagedQuery.PageSize = 10000;
            var pagedInfo = await _manuStatorPackListRepository.GetPagedListAsync(pagedQuery);

            // 实体到DTO转换 装载数据
            var dtos = pagedInfo.Data.Select(s => s.ToModel<ManuStatorPackListDto>());
            return new PagedInfo<ManuStatorPackListDto>(dtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }
    }
}
