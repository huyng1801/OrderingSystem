using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System;

namespace OrderingSystemAPI.Middleware
{
    public class GlobalExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
        
            Console.WriteLine($"Exception occurred: {context.Exception}");

         
            var statusCode = 500;
            var errorMessage = "Internal Server Error";

            if (context.Exception is KeyNotFoundException)
            {
                statusCode = 404; 
                errorMessage = "Resource not found";
            }
            else if (context.Exception is InvalidOperationException)
            {
                statusCode = 400; 
                errorMessage = context.Exception.Message; 
            }
     
            context.Result = new ObjectResult(errorMessage)
            {
                StatusCode = statusCode
            };

            context.ExceptionHandled = true;
        }
    }
}
