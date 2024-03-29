using Hymson.Authentication.JwtBearer.Security;
using Hymson.Authentication;
using Hymson.MES.Services.Dtos.Manufacture.ManuSFCScrap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Common.Query;
using Minio.DataModel;

namespace Hymson.MES.Services.Services.Manufacture.ManuSfcScrapservice
{
    /// <summary>
    /// 部分报废实现
    /// </summary>
    public class ManuSFCPartialScrapService : IManuSFCPartialScrapService
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
        /// 条码信息表 仓储
        /// </summary>
        private readonly IManuSfcRepository _manuSfcRepository;

        /// <summary>
        /// 条码信息表  仓储
        /// </summary>
        private readonly IManuSfcInfoRepository _manuSfcInfoRepository;

        /// <summary>
        /// 条码生产信息（物理删除） 仓储
        /// </summary>
        private readonly IManuSfcProduceRepository _manuSfcProduceRepository;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="currentSite"></param>
        /// <param name="manuSfcRepository"></param>
        public ManuSFCPartialScrapService(ICurrentUser currentUser, ICurrentSite currentSite, IManuSfcRepository manuSfcRepository)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _manuSfcRepository = manuSfcRepository;
        }

        /// <summary>
        /// 报废
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task PartialScrapAsync(ManuSFCPartialScrapDto param)
        {
            //条码表
            var sfcEntities = await _manuSfcRepository.GetManuSfcEntitiesAsync(new EntityBySFCsQuery { SFCs = param.BarcodeScrapList.Select(x=>x.SFC), SiteId = _currentSite.SiteId ?? 0 });
            //条码信息表
            var sfcInfoEntities = await _manuSfcInfoRepository.GetBySFCIdsAsync(sfcEntities.Select(x => x.Id));
            var manuSfcProducePagedQuery = new ManuSfcProduceQuery { Sfcs = param.BarcodeScrapList.Select(x => x.SFC), SiteId = _currentSite.SiteId ?? 00 };
            //在制品信息
            var manuSfcProduces = await _manuSfcProduceRepository.GetManuSfcProduceEntitiesAsync(manuSfcProducePagedQuery);
            foreach (var barcodeItem in param.BarcodeScrapList)
            { 
            
            }
            throw new NotImplementedException("未实现");
        }
    }
}
