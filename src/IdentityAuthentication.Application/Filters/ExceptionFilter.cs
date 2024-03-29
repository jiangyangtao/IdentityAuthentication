﻿using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace IdentityAuthentication.Application.Filters
{
    public class ExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            context.ExceptionHandled = true;
            context.Result = new ActionResult(context);
        }
    }

    internal class ActionResult : IActionResult
    {
        private readonly ExceptionContext ErrorContext;

        public ActionResult(ExceptionContext errorContext)
        {
            ErrorContext = errorContext;
        }

        public async Task ExecuteResultAsync(ActionContext context)
        {
            context.HttpContext.Response.Clear();
            context.HttpContext.Response.StatusCode = 400;
            context.HttpContext.Response.ContentType = $"{MediaTypeNames.Text.Plain};charset=utf-8";

            //var result = ErrorContext.Exception.Message;
            await context.HttpContext.Response.CompleteAsync();
        }
    }
}
