using Hymson.MES.BackgroundServices.CoreServices.Model.Rotor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.BackgroundServices.CoreServices.Repository.Rotor
{
    /// <summary>
    /// 铁芯码和轴码绑定仓储
    /// </summary>
    public class WorkOrderRelationRepository : BaseRepository<WorkOrderRelationEntity>, IWorkOrderRelationRepository
    {
        /// <summary>
        /// 获取绑定关系数据
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public async Task<List<WorkOrderRelationEntity>> GetList(string sql)
        {
            var result = await SqlQueryToListAsync(sql);

            return result;
        }
    }
}
