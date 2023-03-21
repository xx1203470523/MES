using FluentValidation;
using FluentValidation.Results;
using Hymson.Infrastructure.Exceptions;
using Hymson.Localization.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Data;
using System.Net;

namespace Hymson.WebApi.Filters
{
    /// <summary>
    /// 全局异常过滤器
    /// </summary>
    public class HttpGlobalExceptionFilter : IExceptionFilter
    {
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<HttpGlobalExceptionFilter> _logger;
        private readonly ILocalizationService _localizationService;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="env"></param>
        /// <param name="logger"></param>
        public HttpGlobalExceptionFilter(IWebHostEnvironment env,
            ILogger<HttpGlobalExceptionFilter> logger, ILocalizationService localizationService)
        {
            _env = env;
            _logger = logger;
            _localizationService = localizationService;
        }

        private string TryToLocalizeExceptionMessage(Exception exception, string localizedValue)
        {
            if (exception.Data != null && exception.Data.Count > 0)
            {
                foreach (var key in exception.Data.Keys)
                {
                    localizedValue = localizedValue.Replace("{" + key + "}", exception.Data[key]?.ToString());
                }
            }
            return localizedValue;
        }

        private string TryToLocalizeValidationFailureMessage(ValidationFailure validationFailure)
        {
            string localizedValue = _localizationService.GetResource(validationFailure.ErrorCode);
            if (validationFailure.FormattedMessagePlaceholderValues != null && validationFailure.FormattedMessagePlaceholderValues.Any())
            {
                foreach (var keyValuePair in validationFailure.FormattedMessagePlaceholderValues)
                {
                    localizedValue = localizedValue.Replace("{" + keyValuePair.Key + "}", keyValuePair.Value.ToString());
                }
            }
            return localizedValue;
        }
        private void PrepareBadRequestObjectResult(BaseException exception, ExceptionContext context)
        {
            var problemDetails = new ValidationProblemDetails()
            {
                Instance = context.HttpContext.Request.Path,
                Status = StatusCodes.Status400BadRequest,
                Detail = "Please refer to the errors property for additional details."
            };
            problemDetails.Errors.Add(exception.ErrorCode,
                new string[] {
                    TryToLocalizeExceptionMessage(exception, _localizationService.GetResource(exception.ErrorCode)) });
            context.Result = new BadRequestObjectResult(problemDetails);
            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
        }
        private void PrepareInternalServerErrorObjectResult(BaseException exception, ExceptionContext context)
        {
            var problemDetails = new InternalServerProblemDetails()
            {
                Instance = context.HttpContext.Request.Path,
                Status = StatusCodes.Status500InternalServerError,
            };
            if (_env.IsDevelopment())
            {
                problemDetails.Detail = context.Exception.Message;
            }
            problemDetails.Errors.Add(exception.ErrorCode,
                new string[] {
                    TryToLocalizeExceptionMessage(exception, _localizationService.GetResource(exception.ErrorCode)) });
            context.Result = new InternalServerErrorObjectResult(problemDetails);
            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        }
        /// <summary>
        /// 异常处理
        /// </summary>
        /// <param name="context"></param>
        public void OnException(ExceptionContext context)
        {
            _logger.LogError(new EventId(context.Exception.HResult),
            context.Exception,
            context.Exception.Message);

            if (context.Exception is ValidationException validationException)
            {
                var problemDetails = new ValidationProblemDetails()
                {
                    Instance = context.HttpContext.Request.Path,
                    Status = StatusCodes.Status400BadRequest,
                    Detail = "Please refer to the errors property for additional details."
                };
                foreach (var validationFailure in validationException.Errors)
                {
                    var a = validationFailure.FormattedMessagePlaceholderValues;
                    problemDetails.Errors.Add(validationFailure.ErrorCode, new string[] { TryToLocalizeValidationFailureMessage(validationFailure) });
                }
                context.Result = new BadRequestObjectResult(problemDetails);
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            }
            else if (context.Exception is CustomerValidationException customerValidationException)
            {
                PrepareBadRequestObjectResult(customerValidationException, context);
            }
            else if (context.Exception is NotFoundException notFoundException)
            {
                PrepareBadRequestObjectResult(notFoundException, context);

            }
            else if (context.Exception is BusinessException businessException)
            {
                PrepareInternalServerErrorObjectResult(businessException, context);
            }
            else if (context.Exception is CustomerDataException dataException)
            {
                PrepareInternalServerErrorObjectResult(dataException, context);
            }
            else
            {
                var problemDetails = new InternalServerProblemDetails()
                {
                    Instance = context.HttpContext.Request.Path,
                    Status = StatusCodes.Status500InternalServerError,
                };

                if (_env.IsDevelopment())
                {
                    problemDetails.Detail = context.Exception.Message;
                }
                problemDetails.Errors.Add("服务异常",
                    new string[] { _localizationService.GetResource("服务异常") });
                context.Result = new InternalServerErrorObjectResult(problemDetails);
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }
            context.ExceptionHandled = true;
        }


    }
    public class InternalServerProblemDetails : ValidationProblemDetails
    {

    }
    public class InternalServerErrorObjectResult : ObjectResult
    {
        public InternalServerErrorObjectResult(object error)
            : base(error)
        {
            StatusCode = StatusCodes.Status500InternalServerError;
        }
    }


}
