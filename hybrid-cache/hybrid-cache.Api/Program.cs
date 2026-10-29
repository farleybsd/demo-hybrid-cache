using FastEndpoints;
using hybrid_cache.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSqlServerDb(builder.Configuration);
builder.Services.AddRedis(builder.Configuration);
if (builder.Environment.IsDevelopment())
    builder.Services.AddReadableCacheMirror(builder.Configuration);
builder.Services.AddHybridCacheConfig();
builder.Services.AddRepository();
builder.Services.AddFastEndpoints();
builder.Services.AddSwaggerConfig();
var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseFastEndpoints();
app.UseSwaggerConfig();

app.UseHttpsRedirection();

app.Run();

