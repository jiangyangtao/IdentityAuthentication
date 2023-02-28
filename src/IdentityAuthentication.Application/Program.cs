
using IdentityAuthentication.Application.Filters;
using IdentityAuthentication.Application.GrpcServices;
using IdentityAuthentication.Application.Handlers;
using IdentityAuthentication.Core;
using IdentityAuthentication.Model;
using IdentityAuthentication.Model.Handlers;
using IdentityAuthentication.Model.Handles;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Yangtao.Hosting.Core;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

// Add services to the container.
services.AddControllers(options =>
{
    options.Filters.Add<ExceptionFilter>();
}).AddNewtonsoftJson(options =>
{
    options.SerializerSettings.Formatting = Formatting.Indented;
    options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
    options.SerializerSettings.ContractResolver = new DefaultContractResolver();
    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();
services.AddIdentityAuthentication(configuration);

services.AddAuthentication(options =>
{
    options.DefaultScheme = IdentityAuthenticationDefaults.AuthenticationScheme;
    options.DefaultAuthenticateScheme = IdentityAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = IdentityAuthenticationDefaults.AuthenticationScheme;
    options.DefaultForbidScheme = IdentityAuthenticationDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = IdentityAuthenticationDefaults.AuthenticationScheme;
    options.DefaultSignOutScheme = IdentityAuthenticationDefaults.AuthenticationScheme;
}).AddScheme<IdentityAuthenticationSchemeOptions, IdentityAuthenticationHandler>(IdentityAuthenticationDefaults.AuthenticationScheme, options =>
{
    options.Events = new IdentityAuthenticationEvents
    {
        OnMessageReceived = context =>
        {
            context.Token = context.Request.Query[HttpHeaderKeyDefaults.AccessToken];
            return Task.CompletedTask;
        },
        OnAuthenticationFailed = context =>
        {
            if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
            {
                context.Response.Headers.Add("token-expired", "true");
            }
            return Task.CompletedTask;
        },
    };
});
services.AddGrpc();

//services.AddAuthenticationCore(options =>
//{
//    options.AddScheme<AuthenticationHandler>(nameof(AuthenticationHandler), "demo handle");
//});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseIdentityAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapGrpcService<TokenService>();

app.MapGet("/", async context =>
{
    await context.Response.WriteAsync("Hello, IdentityAuthentication");
});

app.Run();
