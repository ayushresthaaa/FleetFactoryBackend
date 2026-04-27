using System.Net; 
using System.Text.Json;
using FleetFactory.Shared.Results; 
namespace FleetFactory.API.Middleware
{
    public class ExceptionMiddleware (RequestDelegate _next, ILogger<ExceptionMiddleware> _logger)  
    { 

        public async Task InvokeAsync(HttpContext context) //this method is called for every request by ASP
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception occurred");
                
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError; //this is for the crash response

                var response = ApiResponse<string>.ErrorResponse("An unexpected error occurred. Please try again later.");               
                await context.Response.WriteAsync(JsonSerializer.Serialize(response)); 

                //writeasync is for the response body writing in the json res[pmse]
            }
        }
    }
}
