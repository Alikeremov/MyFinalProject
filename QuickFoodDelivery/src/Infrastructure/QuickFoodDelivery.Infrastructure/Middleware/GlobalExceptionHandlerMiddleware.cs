using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickFoodDelivery.Infrastructure.Middleware
{
    public class GlobalExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public GlobalExceptionHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch (Exception e)
            {
                string errorpage = Path.Combine("/home", $"ErrorPage?error={e}");
                string errorMessage = e.Message;
                errorpage = Path.Combine("/home", $"ErrorPage?error={Uri.EscapeDataString(errorMessage)}");
                context.Response.Redirect(errorpage);
            }
        }
    }
}
