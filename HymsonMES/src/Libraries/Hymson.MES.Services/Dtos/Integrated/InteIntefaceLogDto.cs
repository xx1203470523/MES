using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Integrated;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Dtos.Integrated
{

    public record InteIntefaceLogDto : BaseEntityDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public string? Id { get; set; }

        /// <summary>
        /// 接口编码
        /// </summary>
        public string? InterfaceCode { get; set; }

        /// <summary>
        /// 接口名称
        /// </summary>
        public string? InterfaceName { get; set; }

        /// <summary>
        /// 结果;0、成功 1、失败
        /// </summary>
        public ResponseResultEnum? ResponseResult { get; set; }

        /// <summary>
        /// 请求方
        /// </summary>
        public string? Requestor { get; set; }

        /// <summary>
        /// 请求方编码
        /// </summary>
        public string? RequestorCode { get; set; }

        /// <summary>
        /// 接收方
        /// </summary>
        public string? Responsetor { get; set; }

        /// <summary>
        /// 请求时间
        /// </summary>
        public string RequestTime { get; set; }

        /// <summary>
        /// 响应时间
        /// </summary>
        public string ResponseTime { get; set; }
        /// <summary>
        /// 请求入参
        /// </summary>
        public string RequestBody { get; set; }  
        /// <summary>
        /// 路径
        /// </summary>
        public string Path { get; set; }
        /// <summary>
        /// 响应内容
        /// </summary>
        public string ResponseBody { get; set; }
        /// <summary>
        /// 请求方式
        /// </summary>
        public string Method { get; set; }
        /// <summary>
        /// 请求时间
        /// </summary>
        public string Cost { get; set; }
        /// <summary>
        /// IP
        /// </summary>
        public string Ip { get; set; }  

        /// <summary>
        /// 请求时间  时间范围  数组
        /// </summary>
        public DateTime[]? TimeStamp { get; set; }

    }
     

    public class InteIntefaceLogPagedQueryDto : PagerInfo
    {
        public string? Id { get; set; }

        /// <summary>
        /// 接口编码
        /// </summary>
        public string? InterfaceCode { get; set; }

        /// <summary>
        /// 接口名称
        /// </summary>
        public string? InterfaceName { get; set; }

        /// <summary>
        /// 结果;0、成功 1、失败
        /// </summary>
        public ResponseResultEnum? ResponseResult { get; set; }

        /// <summary>
        /// 查询类型
        /// </summary>
        public InterfaceLogQueryTyeEnum? QueryType { get; set; }

        /// <summary>
        /// 请求方
        /// </summary>
        public string? Requestor { get; set; }

        /// <summary>
        /// 请求方编码
        /// </summary>
        public string? RequestorCode { get; set; }

        /// <summary>
        /// 接收方
        /// </summary>
        public string? Responsetor { get; set; }

        /// <summary>
        /// 请求时间  时间范围  数组
        /// </summary>
        public DateTime[]? TimeStamp { get; set; }

    }
}
