using ManagementBook.Api.Endpoints;
using ManagementBook.Api.Extensions;
using ManagementBook.Api.Handlers;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMvc(options => options.Filters.Add(new ErrorHandlerAttribute()));
builder.Services.Configure<RequestLocalizationOptions>(op =>
{
    op.DefaultRequestCulture = new Microsoft.AspNetCore.Localization.RequestCulture("pt-BR");
});
builder.Services.AddDependencies(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//endpoints
app.BookGetEndpoint();

app.Run();
