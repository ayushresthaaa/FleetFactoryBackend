using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace FleetFactory.API.Filters
{
    public class AuthorizeCheckOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var authorizeAttributes = context.MethodInfo.DeclaringType?
                .GetCustomAttributes(true)
                .Union(context.MethodInfo.GetCustomAttributes(true))
                .OfType<AuthorizeAttribute>()
                .ToList();

            if (authorizeAttributes == null || !authorizeAttributes.Any())
                return;

            var roles = authorizeAttributes
                .Where(a => !string.IsNullOrWhiteSpace(a.Roles))
                .SelectMany(a => a.Roles!.Split(','))
                .Select(r => r.Trim())
                .Distinct()
                .ToList();

            var roleText = roles.Any()
                ? $"Required roles: {string.Join(", ", roles)}"
                : "Requires authenticated user";

            operation.Summary = string.IsNullOrWhiteSpace(operation.Summary)
                ? roleText
                : $"{operation.Summary} | {roleText}";

            operation.Description = string.IsNullOrWhiteSpace(operation.Description)
                ? roleText
                : $"{operation.Description}<br/><b>{roleText}</b>";

            operation.Responses.TryAdd("401", new OpenApiResponse
            {
                Description = "Unauthorized"
            });

            operation.Responses.TryAdd("403", new OpenApiResponse
            {
                Description = "Forbidden"
            });
        }
    }
}