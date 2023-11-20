using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Core.Constants;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Services.Dtos.Manufacture;

namespace Hymson.MES.Services.Services.Manufacture
{
    /// <summary>
    /// 服务（条码档位表） 
    /// </summary>
    public class ManuSfcGradeService : IManuSfcGradeService
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
        /// 仓储接口（条码档位表）
        /// </summary>
        private readonly IManuSfcGradeRepository _manuSfcGradeRepository;

        private readonly IManuSfcGradeDetailRepository _manuSfcGradeDetailRepository;

        private readonly IProcProcedureRepository _procProcedureRepository;
        private readonly IProcParameterRepository _procParameterRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="currentSite"></param>
        /// <param name="manuSfcGradeRepository"></param>
        /// <param name="manuSfcGradeDetailRepository"></param>
        /// <param name="procProcedureRepository"></param>
        /// <param name="procParameterRepository"></param>
        public ManuSfcGradeService(ICurrentUser currentUser, ICurrentSite currentSite,
            IManuSfcGradeRepository manuSfcGradeRepository, IManuSfcGradeDetailRepository manuSfcGradeDetailRepository, IProcProcedureRepository procProcedureRepository, IProcParameterRepository procParameterRepository)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _manuSfcGradeRepository = manuSfcGradeRepository;
            _manuSfcGradeDetailRepository = manuSfcGradeDetailRepository;
            _procProcedureRepository = procProcedureRepository;
            _procParameterRepository = procParameterRepository;
        }


        /// <summary>
        /// 根据SFC获取档位信息
        /// </summary>
        /// <param name="sfc"></param>
        /// <returns></returns>
        /// <exception cref="CustomerValidationException"></exception>
        public async Task<ManuSfcGradeViewDto?> GetBySFCAsync(string sfc) 
        {
            ManuSfcGradeViewDto manuSfcGradeViewDto = new ManuSfcGradeViewDto();

            if (string.IsNullOrEmpty(sfc)) 
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES11315));
            }

            var sfcGradeEntity = (await _manuSfcGradeRepository.GetManuSfcGradeEntitiesAsync(new ManuSfcGradeQuery
            {
                SiteId = _currentSite.SiteId??0,
                Sfcs = new[] { sfc }
            })).OrderByDescending(x=>x.CreatedOn).FirstOrDefault();

            if (sfcGradeEntity == null)
            {
                return null;
            }

            //查询明细
            var manuSfcGradeDetails = await _manuSfcGradeDetailRepository.GetByGradeIdAsync(new ManuSfcGradeDetailByGradeIdQuery
            {
                SiteId = _currentSite.SiteId ?? 0,
                GadeId = sfcGradeEntity.Id
            });

            var manuSfcDetailViews = new List<ManuSfcGradeDetailViewDto>();
            var geadeGroup = "";
            if (manuSfcGradeDetails.Any()) 
            {
                //查询工序
                var procedures= await _procProcedureRepository.GetByIdsAsync(manuSfcGradeDetails.Select(x => x.ProduceId).Distinct().ToList());

                //查询参数
                var parameters= await _procParameterRepository.GetByIdsAsync(manuSfcGradeDetails.Select(x => x.ParamId).Distinct().ToList());


                foreach (var item in manuSfcGradeDetails)
                {
                    var procedure = procedures.FirstOrDefault(x => x.Id == item.ProduceId);

                    var parameter = parameters.FirstOrDefault(x => x.Id == item.ParamId);

                    manuSfcDetailViews.Add(new ManuSfcGradeDetailViewDto
                    {
                        ProduceId = item.ProduceId,
                        ProduceCode = procedure?.Code??"",

                        SFC=item.SFC,
                        Grade=item.Grade,

                        ParamId = item.ParamId,
                        ParamCode=parameter?.ParameterCode??"",
                        ParamName=parameter?.ParameterName??"",
                        ParamUnit=parameter?.ParameterUnit??"",
                        ParamValue=item.ParamValue,

                        CenterValue=item.CenterValue,
                        MaxValue=item.MaxValue,
                        MinValue=item.MinValue,
                        MinContainingType=item.MinContainingType,
                        MaxContainingType=item.MaxContainingType,
                        Remark=item.Remark,
                    }) ;

                }

                geadeGroup = string.Join("", manuSfcDetailViews.Select(x => x.Grade));
            }


            manuSfcGradeViewDto.SFC = sfcGradeEntity.SFC;
            manuSfcGradeViewDto.Grade=sfcGradeEntity.Grade;
            manuSfcGradeViewDto.GeadeGroup = geadeGroup;
            manuSfcGradeViewDto.manuSfcGradeDetails = manuSfcDetailViews;

            return manuSfcGradeViewDto;
        }
    }
}
