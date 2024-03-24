﻿using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace MinimalWebAPIGlobalizationAndLocalization.Swagger;

public class AcceptLanguageHeaderOperationFilter : IOperationFilter
{
    private readonly List<IOpenApiAny>? supportedLanguages;

    public AcceptLanguageHeaderOperationFilter(IOptions<RequestLocalizationOptions> requestLocalizationOptions)
    {
        supportedLanguages = requestLocalizationOptions.Value
            .SupportedCultures?.Select(c => new OpenApiString(c.TwoLetterISOLanguageName))
            .Cast<IOpenApiAny>()
            .ToList();
    }

    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (supportedLanguages?.Any() ?? false)
        {
            operation.Parameters ??= new List<OpenApiParameter>();

            operation.Parameters.Add(new OpenApiParameter
            {
                Name = HeaderNames.AcceptLanguage,
                In = ParameterLocation.Header,
                Required = false,
                Schema = new OpenApiSchema
                {
                    Type = "string",
                    Enum = supportedLanguages,
                    Default = supportedLanguages.First()
                }
            });
        }
    }
}