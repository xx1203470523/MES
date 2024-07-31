using Dapper;
using Hymson.MES.BackgroundServices.NIO.Repositories.Mes.Param.View;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories;
using Hymson.MES.Data.Repositories.Process;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.BackgroundServices.NIO.Repositories.Mes.Param
{
    /// <summary>
    /// 马威参数
    /// </summary>
    public class ProcProductParameterGroupMavelRepository : BaseRepository, IProcProductParameterGroupMavelRepository
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ProcProductParameterGroupMavelRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions) { }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcedureParamMavelView>> GetParamListAsync(MavelParamQuery query)
        {
            string sql = $@"
                select t1.Code ,t1.Name ,t1.Version ,t2.UpperLimit ,t2.CenterValue ,t2.LowerLimit ,t1.UpdatedOn ,
	                t3.Code procedureCode,t3.Name procedureName,t4.ParameterName ,t4.ParameterCode,t4.DataType ,
                    t4.Remark,t4.ParameterUnit  
                from proc_product_parameter_group t1
                inner join proc_product_parameter_group_detail t2 on t1.Id = t2.ParameterGroupId  and t2.IsDeleted  = 0
                inner join proc_procedure t3 on t3.Id  = t1.ProcedureId and t3.IsDeleted = 0
                inner join proc_parameter t4 on t4.Id = t2.ParameterId and t4.IsDeleted = 0
                where t1.SiteId  = {query.SiteId}
                and t1.IsDeleted  = 0
            ";

            using var conn = GetMESDbConnection();
            var dbList = await conn.QueryAsync<ProcedureParamMavelView>(sql);

            return dbList;
        }
    }
}
