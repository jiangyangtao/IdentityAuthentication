
using IdentityAuthentication.Application;
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
    options.SerializerSettings.DateFormatString = AuthenticationConfigurationDefault.DateFormatString;
    options.SerializerSettings.ContractResolver = new DefaultContractResolver();
    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
services.AddEndpointsApiExplorer();
services.AddApiVersioning(options =>
{
    options.ReportApiVersions = true;
    options.UseApiBehavior = true;
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = AuthenticationConfigurationDefault.ApiV1;
});
services.AddVersionedApiExplorer(options =>
{
    options.GroupNameFormat = AuthenticationConfigurationDefault.GroupNameFormat;
    options.SubstituteApiVersionInUrl = true;
});
services.AddSwaggerGen();
services.AddIdentityAuthentication();


services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.SetIsOriginAllowed(_ => true).AllowAnyHeader().AllowAnyMethod().AllowCredentials();
    });
});

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
services.AddGrpc(options =>
{
    options.EnableDetailedErrors = true;
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();
app.UseCors();

app.UseAuthentication();
app.UseIdentityAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapGrpcService<TokenService>();

app.Map("/", () => "Hello, IdentityAuthentication");
app.Run();
