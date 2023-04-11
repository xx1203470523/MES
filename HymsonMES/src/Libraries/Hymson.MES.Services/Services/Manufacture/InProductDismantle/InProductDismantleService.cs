using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.Core.Enums.QualUnqualifiedCode;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Manufacture.ManuProductBadRecord.Command;
using Hymson.MES.Data.Repositories.Manufacture.ManuSfcProduce.Command;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.Quality.IQualityRepository;
using Hymson.MES.Services.Dtos.Manufacture;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;

namespace Hymson.MES.Services.Services.Manufacture
{
    /// <summary>
    /// 在制品拆解服务类
    /// </summary>
    public class InProductDismantleService : IInProductDismantleService
    {
        /// <summary>
        /// 当前对象（登录用户）
        /// </summary>
        private readonly ICurrentUser _currentUser;

        /// <summary>
        /// 当前对象（站点）
        /// </summary>
        private readonly ICurrentSite _currentSite;

        private readonly IProcBomDetailRepository _procBomDetailRepository;
        /// <summary>
        /// 物料维护 仓储
        /// </summary>
        private readonly IProcMaterialRepository _procMaterialRepository;
        /// <summary>
        /// 工序表 仓储
        /// </summary>
        private readonly IProcProcedureRepository _procProcedureRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        public InProductDismantleService(ICurrentUser currentUser, ICurrentSite currentSite,
        IProcBomDetailRepository procBomDetailRepository,
        IProcMaterialRepository procMaterialRepository,
        IProcProcedureRepository procProcedureRepository)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;

            _procBomDetailRepository = procBomDetailRepository;
            _procMaterialRepository = procMaterialRepository;
            _procProcedureRepository = procProcedureRepository;
        }

        /// <summary>
        /// 根据ID查询Bom 详情
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        public async Task<List<InProductDismantleDto>> GetProcBomDetailAsync(InProductDismantleQueryDto queryDto)
        {
            if (queryDto == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10100));
            }

            var bomDetailViews = new List<InProductDismantleDto>();
            var bomDetails = await _procBomDetailRepository.GetByBomIdAsync(queryDto.BomId);
            if (!bomDetails.Any())
            {
                return bomDetailViews;
            }

            var materialIds = bomDetails.Select(item => item.MaterialId).ToArray();
            var procMaterials = await _procMaterialRepository.GetByIdsAsync(materialIds);

            var procedureIds = bomDetails.Select(item => item.ProcedureId).ToArray();
            var procProcedures = await _procProcedureRepository.GetByIdsAsync(procedureIds);

            foreach (var detailEntity in bomDetails)
            {
                var material = procMaterials.FirstOrDefault(item => item.Id == detailEntity.MaterialId);
                var procedures = procProcedures.FirstOrDefault(item => item.Id == detailEntity.ProcedureId);

                bomDetailViews.Add(new InProductDismantleDto
                {
                    BomDetailId = detailEntity.Id,
                    Usages = detailEntity.Usages,
                    MaterialId = detailEntity.MaterialId,
                    ProcedureId = detailEntity.ProcedureId,
                    MaterialCode = material?.MaterialCode ?? "",
                    MaterialName = material?.MaterialName ?? "",
                    Version = material?.Version ?? "",
                    SerialNumber = material?.SerialNumber,
                    Code = procedures?.Code ?? "",
                    Name = procedures?.Name ?? ""
                });
            }


            //查询子组件
            return bomDetailViews;
        }
    }
}
