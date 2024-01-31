using Hymson.Infrastructure;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Services.Dtos.Report;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Services.Report.PackBindOtherReport
{
    public class PackBindOtherReportService : IPackBindOtherReportService
    {
        private IManuSfcCirculationRepository _manuSfcCirculationRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="manuSfcCirculationRepository"></param>
        public PackBindOtherReportService(IManuSfcCirculationRepository manuSfcCirculationRepository)
        {
            _manuSfcCirculationRepository = manuSfcCirculationRepository;
        }

        /// <summary>
        /// 分页查询外箱码
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<PagedInfo<PackBindOtherReportViewDto>> GetPagedInfoAsync(PackBindOtherPageQueryPagedDto query)
        {
            //只查询绑定类型为外箱码绑定的条码
            var manuSfcCirculationEntities = await _manuSfcCirculationRepository.GetPagedInfoAsync(new()
            {
                SFC = query.Sfc,
                CirculationBarCode = query.BindSfc,
                CirculationTypes = new SfcCirculationTypeEnum[] {  SfcCirculationTypeEnum.BindPack1,SfcCirculationTypeEnum.BindPack2, SfcCirculationTypeEnum.BindPack3, SfcCirculationTypeEnum.BindPack4 },
                PageIndex = query.PageIndex,
                PageSize = query.PageSize,
            });

            var pageInfo = new List<PackBindOtherReportViewDto>();
            foreach (var item in manuSfcCirculationEntities.Data)
            {
                PackBindOtherReportViewDto newItem = new();
                newItem.SFC = item.SFC;
                newItem.BindSfc = item.CirculationBarCode;
                newItem.CreatedBy = item.CreatedBy;
                newItem.CreatedOn = item.CreatedOn;
                newItem.CirculationType = item.CirculationType;

                pageInfo.Add(newItem);
            }

            return new PagedInfo<PackBindOtherReportViewDto>(pageInfo, query.PageIndex, query.PageSize);
        }
    }
}
