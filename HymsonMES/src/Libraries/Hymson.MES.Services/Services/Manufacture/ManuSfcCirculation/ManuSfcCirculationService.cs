using Hymson.Authentication;
using Hymson.Excel.Abstractions;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Equipment;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Manufacture.ManuSfcCirculation.Query;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Services.Dtos.Common;
using Hymson.MES.Services.Dtos.Manufacture;
using Hymson.MES.Services.Dtos.Report;
using Hymson.MES.Services.Extension;
using Hymson.Minio;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;

namespace Hymson.MES.Services.Services.Manufacture;

public class ManuSfcCirculationService : IManuSfcCirculationService
{
    private readonly ICurrentUser _currentUser;
    private readonly IExcelService _excelService;
    private readonly IMinioService _minioService;
    private readonly IManuSfcRepository _manuSfcRepository;
    private readonly IManuSfcInfoRepository _manuSfcInfoRepository;
    private readonly IManuSfcCirculationRepository _manuSfcCirculationRepository;
    private readonly IProcProcedureRepository _procProcedureRepository;
    private readonly IProcResourceRepository _procResourceRepository;
    private readonly IProcResourceEquipmentBindRepository _procResourceEquipmentBindRepository;

    private readonly IManuSfcSummaryRepository _manuSfcSummaryRepository;
    private readonly IManuSfcProduceRepository _manuSfcProduceRepository;

    public ManuSfcCirculationService(ICurrentUser currentUser,
        IMinioService minioService,
        IExcelService excelService,
        IManuSfcRepository manuSfcRepository,
        IManuSfcInfoRepository manuSfcInfoRepository,
        IManuSfcCirculationRepository manuSfcCirculationRepository,
        IProcProcedureRepository procProcedureRepository,
        IProcResourceRepository procResourceRepository,
        IProcResourceEquipmentBindRepository procResourceEquipmentBindRepository,
        IManuSfcSummaryRepository manuSfcSummaryRepository,
        IManuSfcProduceRepository manuSfcProduceRepository)
    {
        _currentUser = currentUser;
        _excelService = excelService;
        _minioService = minioService;
        _manuSfcRepository = manuSfcRepository;
        _manuSfcInfoRepository = manuSfcInfoRepository;
        _manuSfcCirculationRepository = manuSfcCirculationRepository;
        _procProcedureRepository = procProcedureRepository;
        _procResourceRepository = procResourceRepository;
        _procResourceEquipmentBindRepository = procResourceEquipmentBindRepository;
        _manuSfcSummaryRepository = manuSfcSummaryRepository;
        _manuSfcProduceRepository = manuSfcProduceRepository;
    }

    /// <summary>
    /// 分页查询数据
    /// </summary>
    /// <param name="pageQueryDto"></param>
    /// <returns></returns>
    public async Task<PagedInfo<ManuSfcCirculationViewDto>> GetPageInfoAsync(ManuSfcCirculationPagedQueryDto pageQueryDto)
    {
        var result = new PagedInfo<ManuSfcCirculationViewDto>(new List<ManuSfcCirculationViewDto>(), pageQueryDto.PageIndex, pageQueryDto.PageSize);
        var pageQuery = pageQueryDto.ToQuery<ManuSfcCirculationPagedQuery>();

        var pageInfo = await _manuSfcCirculationRepository.GetPagedInfoAsync(pageQuery);

        var procedureIds = pageInfo.Data.Select(a => a.ProcedureId);
        var procedureEntities = await _procProcedureRepository.GetByIdsAsync(procedureIds.ToArray());

        var listData = new List<ManuSfcCirculationViewDto>();
        foreach (var item in pageInfo.Data)
        {
            var manuSfcCirculation = item.ToModel<ManuSfcCirculationViewDto>();

            var procedureEntity = procedureEntities.FirstOrDefault(a => a.Id == item.ProcedureId);
            manuSfcCirculation.ProcedureName = procedureEntity?.Name ?? "";

            listData.Add(manuSfcCirculation);
        }

        result.Data = listData;
        result.TotalCount = pageInfo.TotalCount;

        return result;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="queryDto"></param>
    /// <returns></returns>
    public async Task<ExportResultDto> ExportBindSfcAsync(ManuSfcCirculationQueryDto queryDto)
    {
        var fileName = "条码绑定报表";
        ExportResultDto result = new();

        var query = queryDto.ToQuery<ManuSfcCirculationQuery>();
        var dataList = await _manuSfcCirculationRepository.GetListAsync(query);

        var procedureIds = dataList.Select(a => a.ProcedureId);
        var procedureEntities = await _procProcedureRepository.GetByIdsAsync(procedureIds.ToArray());

        var excelData = new List<ManuSfcCirculationExcelDto>();
        foreach (var item in dataList)
        {
            var manuSfcCirculation = item.ToExcelModel<ManuSfcCirculationExcelDto>();

            var procedureEntity = procedureEntities.FirstOrDefault(a => a.Id == item.ProcedureId);
            manuSfcCirculation.ProcedureName = procedureEntity?.Name ?? "";

            excelData.Add(manuSfcCirculation);
        }

        if (excelData?.Any() == true)
        {
            var filePath = await _excelService.ExportAsync(excelData, fileName, fileName);
            //上传到文件服务器  
            var uploadResult = await _minioService.PutObjectAsync(filePath);
            result.FileName = fileName;
            result.Path = uploadResult.AbsoluteUrl;
            result.RelativePath = uploadResult.RelativeUrl;
        }

        return result;
    }

    /// <summary>
    /// 根据绑定类型绑定Sfc和circulationBarCode
    /// </summary>
    /// <param name="modifyDto"></param>
    /// <returns></returns>
    public async Task BindSfcAsync(ManuSfcCirculationModifyDto modifyDto)
    {

        //如果是合并，需要校验条码是否在系统中存在
        if (modifyDto.CirculationType == SfcCirculationTypeEnum.Merge)
        {
            var manuSfcEntities = await _manuSfcRepository.GetBySFCsAsync(new string[] { modifyDto.SFC })
                ?? throw new CustomerValidationException(nameof(ErrorCode.MES17401)).WithData("SFC", modifyDto.SFC);
        }

    }

    public async Task<int> DeteleteManuSfcCirculationAsync(long id)
    {
        return await _manuSfcCirculationRepository.DeleteRangeAsync(new()
        {
            Ids = new long[] { id },
            DeleteOn = HymsonClock.Now(),
            UserId = _currentUser.UserId?.ToString()
        });
    }

    /// <summary>
    /// 绑定条码关系
    /// </summary>
    /// <param name="bindDto"></param>
    /// <returns></returns>
    public async Task CreateManuSfcCirculationAsync(ManuSfcCirculationBindDto bindDto)
    {
        if (bindDto.CirculationType == SfcCirculationTypeEnum.Merge)
        {
            //获取条码信息
            var bindManuSfc = await _manuSfcRepository.GetBySFCAsync(new() { SFC = bindDto.CirculationBarCode, SiteId = 123456 })
                ?? throw new CustomerValidationException(nameof(ErrorCode.MES17401)).WithData("SFC", bindDto.CirculationBarCode);

            var bindManuSfcInfo = await _manuSfcInfoRepository.GetBySFCAsync(bindManuSfc.Id)
                ?? throw new CustomerValidationException(nameof(ErrorCode.MES17401)).WithData("BindSFC", bindDto.CirculationBarCode);

            var procedureEntity = await _procProcedureRepository.GetByIdAsync(bindDto.ProcedureId.GetValueOrDefault())
                ?? throw new CustomerValidationException(nameof(ErrorCode.MES17311));

            //获取资源
            var resourceEntities = await _procResourceRepository.GetByResTypeIdsAsync(new() { IdsArr = new long[] { procedureEntity.ResourceTypeId.GetValueOrDefault() }, SiteId = 123456 });
            var resourceEntity = resourceEntities.FirstOrDefault() ?? throw new CustomerValidationException(nameof(ErrorCode.MES10388));

            //根据工序获取资源跟设备
            var equEuipmentEntities = await _procResourceEquipmentBindRepository.GetByResourceIdAsync(new() { ResourceId = resourceEntity.Id })
                ?? throw new CustomerValidationException(nameof(ErrorCode.MES10480));
            var equEuipmentEntity = equEuipmentEntities.FirstOrDefault();

            //校验被绑定条码是否存在Ng
            var manuSfcSummaryEntities = await _manuSfcSummaryRepository.GetListAsync(new() { SFCS = new string[] { bindDto.CirculationBarCode ?? "" } });
            var hasNg = manuSfcSummaryEntities.Any(a => a.QualityStatus == 0);
            if (hasNg) throw new CustomerValidationException(nameof(ErrorCode.MES19147)).WithData("SFC", bindDto.CirculationBarCode ?? "");

            //校验绑定条码在制是否处于所选工序排队中的状态
            var manuSfcProdureEntity = await _manuSfcProduceRepository.GetBySFCAsync(new() { Sfc = bindDto.CirculationBarCode ?? "", SiteId = 123456 });
            if (manuSfcProdureEntity != null)
            {
                if (!(manuSfcProdureEntity.ProcedureId == procedureEntity.Id && manuSfcProdureEntity.Status == Core.Enums.SfcProduceStatusEnum.lineUp))
                    throw new CustomerValidationException(nameof(ErrorCode.MES19148)).WithData("SFC", bindDto.CirculationBarCode ?? "").WithData("ProcedureName", procedureEntity.Name);
            }

            //获取条码流转记录
            var bindSfcCirculationEntities = await _manuSfcCirculationRepository.GetManuSfcCirculationBarCodeEntitiesAsync(new()
            {
                CirculationBarCode = bindDto.SFC,
                SiteId = 123456
            });

            //获取条码流转记录
            var sfcCirculationEntities = await _manuSfcCirculationRepository.GetManuSfcCirculationBarCodeEntitiesAsync(new()
            {
                Sfcs = new string[] { bindDto.SFC },
                SiteId = 123456
            });

            //获取绑定条码的流转记录
            var bindSfcCirculationEntities2 = await _manuSfcCirculationRepository.GetManuSfcCirculationBarCodeEntitiesAsync(new()
            {
                Sfcs = new string[] { bindDto.CirculationBarCode },
                SiteId = 123456
            });

            if (bindSfcCirculationEntities2.Any(a => !string.IsNullOrEmpty(a.CirculationBarCode)))
                throw new CustomerValidationException(nameof(ErrorCode.MES17402));

            var locationId = 0;

            //获取最后一个位置码+1
            if (bindSfcCirculationEntities.Any())
            {
                var location = bindSfcCirculationEntities.Max(a => a.Location);

                if (int.TryParse(location, out locationId))
                    locationId += 1;
            }

            ManuSfcCirculationCreateDto sfcData = new ManuSfcCirculationCreateDto();

            //目前绑定只添加绑定关系，不增加条码信息和在制信息
            ManuSfcCirculationCreateDto bindSfcData = new ManuSfcCirculationCreateDto()
            {
                Id = IdGenProvider.Instance.CreateId(),
                SiteId = 123456,
                ProcedureId = procedureEntity.Id,
                ResourceId = resourceEntity.Id,
                EquipmentId = equEuipmentEntity.Id,
                FeedingPointId = null,
                SFC = bindDto.CirculationBarCode,
                WorkOrderId = bindManuSfcInfo.WorkOrderId,
                ProductId = bindManuSfcInfo.ProductId,
                Location = locationId.ToString(),
                CirculationBarCode = bindDto.SFC,
                CirculationWorkOrderId = bindManuSfcInfo.WorkOrderId, //暂不考虑流转后工单变更场景
                CirculationProductId = bindManuSfcInfo.ProductId, //产品同上
                CirculationQty = 1,
                CirculationType = bindDto.CirculationType.GetValueOrDefault(),
                IsDisassemble = Core.Enums.TrueOrFalseEnum.No,
                CreatedBy = _currentUser.UserName ?? "system",
                CreatedOn = HymsonClock.Now(),
                UpdatedBy = _currentUser.UserName,
                UpdatedOn = HymsonClock.Now(),
                IsDeleted = 0
            };

            if (!sfcCirculationEntities.Any())
            {
                //添加绑定条码信息
                sfcData = new ManuSfcCirculationCreateDto()
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SiteId = 123456,
                    ProcedureId = procedureEntity.Id,
                    ResourceId = resourceEntity.Id,
                    EquipmentId = equEuipmentEntity.Id,
                    FeedingPointId = null,
                    SFC = bindDto.SFC,
                    WorkOrderId = bindManuSfcInfo.WorkOrderId,
                    ProductId = bindManuSfcInfo.ProductId,
                    Location = "0",
                    CirculationBarCode = "",
                    CirculationWorkOrderId = bindManuSfcInfo.WorkOrderId, //暂不考虑流转后工单变更场景
                    CirculationProductId = bindManuSfcInfo.ProductId, //产品同上
                    CirculationQty = 1,
                    CirculationType = bindDto.CirculationType.GetValueOrDefault(),
                    IsDisassemble = Core.Enums.TrueOrFalseEnum.No,
                    CreatedBy = _currentUser.UserName ?? "system",
                    CreatedOn = HymsonClock.Now(),
                    UpdatedBy = _currentUser.UserName,
                    UpdatedOn = HymsonClock.Now(),
                    IsDeleted = 0
                };
            }

            using var tran = TransactionHelper.GetTransactionScope();

            //插入条码记录
            if (sfcData.Id != 0)
            {
                var sfcEntity = sfcData.ToEntity<ManuSfcCirculationEntity>();
                var insertSfc = await _manuSfcCirculationRepository.InsertAsync(sfcEntity);
            }

            //插入绑定条码绑定记录
            var BindSfcEntity = bindSfcData.ToEntity<ManuSfcCirculationEntity>();
            var insertBindSfc = await _manuSfcCirculationRepository.InsertAsync(BindSfcEntity);

            tran.Complete();
        }
        else
        {
            //如果是外箱码绑定逻辑，则只用校验是否有重复绑定关系

            var procedureEntity = await _procProcedureRepository.GetByIdAsync(bindDto.ProcedureId.GetValueOrDefault())
                ?? throw new CustomerValidationException(nameof(ErrorCode.MES17311));

            //校验被绑定条码是否存在Ng
            var manuSfcSummaryEntities = await _manuSfcSummaryRepository.GetListAsync(new() { SFCS = new string[] { bindDto.CirculationBarCode ?? "" } });
            var hasNg = manuSfcSummaryEntities.Any(a => a.QualityStatus == 0);
            if (hasNg) throw new CustomerValidationException(nameof(ErrorCode.MES19147)).WithData("SFC", bindDto.CirculationBarCode ?? "");

            //校验绑定条码在制是否处于所选工序排队中的状态
            var manuSfcProdureEntity = await _manuSfcProduceRepository.GetBySFCAsync(new() { Sfc = bindDto.CirculationBarCode ?? "", SiteId = 123456 });
            if (manuSfcProdureEntity != null)
            {
                if (!(manuSfcProdureEntity.ProcedureId == procedureEntity.Id && manuSfcProdureEntity.Status == Core.Enums.SfcProduceStatusEnum.lineUp))
                    throw new CustomerValidationException(nameof(ErrorCode.MES19148)).WithData("SFC", bindDto.CirculationBarCode ?? "").WithData("ProcedureName", procedureEntity.Name);
            }


            //获取条码流转记录
            var bindSfcCirculationEntities = await _manuSfcCirculationRepository.GetManuSfcCirculationBarCodeEntitiesAsync(new()
            {
                CirculationBarCode = bindDto.SFC,
                SiteId = 123456
            });

            //获取条码流转记录
            var sfcCirculationEntities = await _manuSfcCirculationRepository.GetManuSfcCirculationBarCodeEntitiesAsync(new()
            {
                Sfcs = new string[] { bindDto.SFC },
                SiteId = 123456
            });

            //获取绑定条码的流转记录
            var bindSfcCirculationEntities2 = await _manuSfcCirculationRepository.GetManuSfcCirculationBarCodeEntitiesAsync(new()
            {
                Sfcs = new string[] { bindDto.CirculationBarCode },
                SiteId = 123456
            });

            if (bindSfcCirculationEntities2.Any(a => !string.IsNullOrEmpty(a.CirculationBarCode)))
                throw new CustomerValidationException(nameof(ErrorCode.MES17402));

            var locationId = 0;

            //获取最后一个位置码+1
            if (bindSfcCirculationEntities.Any())
            {
                var location = bindSfcCirculationEntities.Max(a => a.Location);

                if (int.TryParse(location, out locationId))
                    locationId += 1;
            }

            ManuSfcCirculationCreateDto sfcData = new ManuSfcCirculationCreateDto();

            //目前绑定只添加绑定关系，不增加条码信息和在制信息
            ManuSfcCirculationCreateDto bindSfcData = new ManuSfcCirculationCreateDto()
            {
                Id = IdGenProvider.Instance.CreateId(),
                SiteId = 123456,
                ProcedureId = bindDto.ProcedureId.GetValueOrDefault(),
                ResourceId = 0,
                EquipmentId = 0,
                FeedingPointId = null,
                SFC = bindDto.CirculationBarCode,
                WorkOrderId = 0,
                ProductId = 0,
                Location = locationId.ToString(),
                CirculationBarCode = bindDto.SFC,
                CirculationWorkOrderId = 0, //暂不考虑流转后工单变更场景
                CirculationProductId = 0, //产品同上
                CirculationQty = 1,
                CirculationType = bindDto.CirculationType.GetValueOrDefault(),
                IsDisassemble = Core.Enums.TrueOrFalseEnum.No,
                CreatedBy = _currentUser.UserName ?? "system",
                CreatedOn = HymsonClock.Now(),
                UpdatedBy = _currentUser.UserName,
                UpdatedOn = HymsonClock.Now(),
                IsDeleted = 0
            };

            //if (!sfcCirculationEntities.Any())
            //{
            //    //添加绑定条码信息
            //    sfcData = new ManuSfcCirculationCreateDto()
            //    {
            //        Id = IdGenProvider.Instance.CreateId(),
            //        SiteId = 123456,
            //        ProcedureId = bindDto.ProcedureId.GetValueOrDefault(),
            //        ResourceId = 0,
            //        EquipmentId = 0,
            //        FeedingPointId = 0,
            //        SFC = bindDto.SFC,
            //        WorkOrderId = 0,
            //        ProductId = 0,
            //        Location = "0",
            //        CirculationBarCode = "",
            //        CirculationWorkOrderId = 0, //暂不考虑流转后工单变更场景
            //        CirculationProductId = 0, //产品同上
            //        CirculationQty = 1,
            //        CirculationType = bindDto.CirculationType.GetValueOrDefault(),
            //        IsDisassemble = Core.Enums.TrueOrFalseEnum.No,
            //        CreatedBy = _currentUser.UserName ?? "system",
            //        CreatedOn = HymsonClock.Now(),
            //        UpdatedBy = _currentUser.UserName,
            //        UpdatedOn = HymsonClock.Now(),
            //        IsDeleted = 0
            //    };
            //}

            using var tran = TransactionHelper.GetTransactionScope();

            ////插入条码记录
            //if (sfcData.Id != 0)
            //{
            //    var sfcEntity = sfcData.ToEntity<ManuSfcCirculationEntity>();
            //    var insertSfc = await _manuSfcCirculationRepository.InsertAsync(sfcEntity);
            //}

            //插入绑定条码绑定记录
            var BindSfcEntity = bindSfcData.ToEntity<ManuSfcCirculationEntity>();
            var insertBindSfc = await _manuSfcCirculationRepository.InsertAsync(BindSfcEntity);

            tran.Complete();


        }
    }

    /// <summary>
    /// 更新条码流转记录表
    /// </summary>
    /// <param name="bindDto"></param>
    /// <returns></returns>
    public async Task UpdateManuSfcCirculationAsync(ManuSfcCirculationBindDto bindDto)
    {
        var procedureEntity = await _procProcedureRepository.GetByIdAsync(bindDto.ProcedureId.GetValueOrDefault())
            ?? throw new CustomerValidationException(nameof(ErrorCode.MES17311));

        //校验被绑定条码是否存在Ng
        var manuSfcSummaryEntities = await _manuSfcSummaryRepository.GetListAsync(new() { SFCS = new string[] {  bindDto.SFC } });
        var hasNg = manuSfcSummaryEntities.Any(a => a.QualityStatus == 0);
        if (hasNg) throw new CustomerValidationException(nameof(ErrorCode.MES19147)).WithData("SFC", bindDto.CirculationBarCode ?? "");

        //校验绑定条码在制是否处于所选工序排队中的状态
        var manuSfcProdureEntity = await _manuSfcProduceRepository.GetBySFCAsync(new() { Sfc = bindDto.CirculationBarCode ?? "", SiteId = 123456 });
        if (manuSfcProdureEntity != null)
        {
            if (!(manuSfcProdureEntity.ProcedureId == procedureEntity.Id && manuSfcProdureEntity.Status == Core.Enums.SfcProduceStatusEnum.lineUp))
                throw new CustomerValidationException(nameof(ErrorCode.MES19148)).WithData("SFC", bindDto.CirculationBarCode ?? "").WithData("ProcedureName", procedureEntity.Name);
        }

        await _manuSfcCirculationRepository.UpdateSfcAsync(new()
        {
            Id = bindDto.Id,
            SFC = bindDto.SFC,
        });
    }
}
