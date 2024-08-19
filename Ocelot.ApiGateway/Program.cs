using Ocelot.Cache.CacheManager;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddOcelot()
    .AddCacheManager(o => o.WithDictionaryHandle());

var app = builder.Build();


app.UseRouting();

app.MapGet("/", async context =>
{
    await context.Response.WriteAsync("Hello Ocelot");
});

await app.UseOcelot();


app.Run();
