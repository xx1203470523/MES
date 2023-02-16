using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Hymson.MES.Api.Filters
{
    /// <summary>
    /// 
    /// </summary>
    public class AuthorizationOperationFilter : IOperationFilter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="context"></param>
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            operation.Parameters ??= new List<OpenApiParameter>();
            var attrs = context.ApiDescription.ActionDescriptor.AttributeRouteInfo;

            //先判断是否是匿名访问,
            var descriptor = context.ApiDescription.ActionDescriptor as ControllerActionDescriptor;
            if (descriptor == null) return;

            var actionAttributes = descriptor.MethodInfo.GetCustomAttributes(inherit: true);
            bool isAnonymous = actionAttributes.Any(a => a is AllowAnonymousAttribute);
            // 非匿名的方法,链接中添加accesstoken值
            if (isAnonymous == true) return;

            operation.Parameters.Add(new OpenApiParameter
            {
                //参数名称
                Name = "Authorization",
                In = ParameterLocation.Header,

                Required = true //是否必选
            });

        }

    }
}
