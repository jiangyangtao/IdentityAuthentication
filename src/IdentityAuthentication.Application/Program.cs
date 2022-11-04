
using IdentityAuthenticaion.Model;
using IdentityAuthenticaion.Model.Configurations;
using IdentityAuthentication.Application.Handlers;
using IdentityAuthentication.Application.Filters;
using IdentityAuthentication.Application.Services;
using IdentityAuthentication.Core;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

// Add services to the container.

var accessTokenConfig = configuration.GetSection("AccessToken");
var secretKeyConfig = configuration.GetSection("SecretKey");
services.Configure<AccessTokenConfiguration>(accessTokenConfig);
services.Configure<RefreshTokenConfiguration>(configuration.GetSection("RefreshToken"));
services.Configure<SecretKeyConfiguration>(secretKeyConfig);
services.Configure<AuthenticationConfiguration>(configuration.GetSection("Autnentication"));

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
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = TokenValidation.BuildAccessTokenValidationParameters(accessTokenConfig, secretKeyConfig);
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            context.Token = context.Request.Query["access_token"];
            return Task.CompletedTask;
        },
        OnAuthenticationFailed = context =>
        {
            if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
            {
                context.Response.Headers.Add("Token-Expired", "true");
            }
            return Task.CompletedTask;
        }
    };
}).AddScheme<IdentityAuthenticationSchemeOptions, IdentityAuthenticationHandler>(IdentityAuthenticationDefaults.AuthenticationScheme, options =>
{
    options.TokenValidationParameters = TokenValidation.BuildAccessTokenValidationParameters(accessTokenConfig, secretKeyConfig);
    options.Events = new IdentityAuthenticationEvents
    {
        OnMessageReceived = context =>
        {
            context.Token = context.Request.Query["access_token"];
            return Task.CompletedTask;
        },
        OnAuthenticationFailed = context =>
        {
            if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
            {
                context.Response.Headers.Add("Token-Expired", "true");
            }
            return Task.CompletedTask;
        }
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
