using System.Net.Mime;

using MinimalWebAPISwagger.Filter;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new()
    {
        Title = builder.Environment.ApplicationName,
        Version = "v1",
        Contact = new() { Name = "Author", Email = "author@fuckMyLife.com", Url = new Uri("https://www.sadlife.com/") },
        Description = "Minimal API - Swagger",
        License = new Microsoft.OpenApi.Models.OpenApiLicense(),
        TermsOfService = new("https://www.sadlife.com/")
    });
    c.OperationFilter<CorrelationIdOperationFilter>();
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    // app.UseSwaggerUI();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", $"{builder.Environment.ApplicationName} v1"));
}

app.UseHttpsRedirection();

app.MapGet("/sampleresponse", () =>
    {
        return Results.Ok(new ResponseData("My Response"));
    })
    .Produces<ResponseData>(StatusCodes.Status200OK)
    .WithTags("Sample")
    .WithName("SampleResponseOperation"); // operation ids to Open API

app.MapGet("/sampleresponseskipped", () =>
    {
        return Results.Ok(new ResponseData("My Response Skipped"));
    })
    .ExcludeFromDescription();

app.MapGet("/{id}", (int id) => Results.Ok(id));
app.MapPost("/", (ResponseData data) => Results.Ok(data))
    .Accepts<ResponseData>(MediaTypeNames.Application.Json);
app.MapPost("/complex", (ComplexResponseData data) => Results.Ok(data))
    .Accepts<ComplexResponseData>(MediaTypeNames.Application.Json);

app.Run();

internal record ResponseData(string Value);

internal record ComplexResponseData(string Value, int Number, decimal Money, DateTimeOffset Date);