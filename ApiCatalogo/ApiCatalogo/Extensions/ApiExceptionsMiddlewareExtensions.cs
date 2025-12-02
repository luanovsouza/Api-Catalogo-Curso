using System.Net;
using ApiCatalogo.Model;
using Microsoft.AspNetCore.Diagnostics;

namespace ApiCatalogo.Extensions;

public static class ApiExceptionsMiddlewareExtensions
{
    public static void ConfigureExceptionsHandler(this IApplicationBuilder app)
    {
        app.UseExceptionHandler(_ =>
        {
            app.Run(async context =>
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                
                context.Response.ContentType = "application/json";
                
                var contextFeature = context.Features.Get<IExceptionHandlerFeature>();

                if (contextFeature != null)
                {
                    await context.Response.WriteAsync(new ErrorDetails()
                    {
                        StatusCode = context.Response.StatusCode,
                        Message = contextFeature.Error.Message,
                        Trace = contextFeature.Error.StackTrace
                    }.ToString());
                }
            });
        });
    }
}