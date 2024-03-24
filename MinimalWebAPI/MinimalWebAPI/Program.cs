using System.Reflection;
using System.Security.Claims;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;

using MinimalWebAPI.Extensions;
using MinimalWebAPI.Routing;
using MinimalWebAPI.Services;
using MinimalWebAPI.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options =>
{
    options.SerializerOptions.IgnoreReadOnlyProperties = true;
    options.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.AddScoped<PeopleService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPut("/people/{id:int}",
    (int id, bool notify, Person person, PeopleService service) => { });

app.MapGet("/search", ([FromQuery(Name = "q")] string searchText) => { });

// This won't compile
//app.MapGet("/people", (int pageIndex = 0, int itemsPerPage = 50) => { });

static string SearchMethod(int pageIndex = 0, int itemsPerPage = 50, string? orderBy = null)
    => $"Sample result for page {pageIndex} getting {itemsPerPage} elements (ordered by {orderBy})";

app.MapGet("/people", SearchMethod);

app.MapGet("/products", (HttpContext context, HttpRequest req, HttpResponse res, ClaimsPrincipal user) => { });

// GET /navigate?location=43.8427,7.8527
// Uncomment the TryParse method and comment the BindAsync method on the Location class to make this example work.
app.MapGet("/navigate", (Location location) => $"Location: {location.Latitude}, {location.Longitude}");

// POST /navigate?lat=43.8427&lon=7.8527
// Uncomment the BindAsync method and comment the TryParse method on the Location class to make this example work.
app.MapPost("/navigate", (Location location) => $"Location: {location.Latitude}, {location.Longitude}");

app.MapGet("/ok", () => Results.Ok(new Person("Donald", "Duck")));

app.MapGet("/notfound", () => Results.NotFound());

app.MapPost("/badrequest", () =>
{
    // Creates a 400 response with a JSON body.
    return Results.BadRequest(new { ErrorMessage = "Unable to complete the request" });
});

app.MapGet("/download", (string fileName) => Results.File(fileName));

app.MapGet("/xml", () => Results.Extensions.Xml(new City { Name = "Taggia" }));

app.MapGet("/product", () =>
{
    var product = new Product("Apple", null, 0.42, 6);
    return Results.Ok(product);
});

// This method automatically registers all the handlers that implement the IEndpointRouteHandler interface.
app.MapEndpoints(Assembly.GetExecutingAssembly());

app.Run();
