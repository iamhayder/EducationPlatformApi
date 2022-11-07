using EducationPlatformApi.Data;
using EducationPlatformApi.Models;
using EducationPlatformApi.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using NSwag;
using NSwag.Generation.Processors.Security;
using System.Text;


var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();


builder.Services
    .AddIdentityCore<ApplicationUser>(options =>
    {
        options.SignIn.RequireConfirmedAccount = false;
        options.User.RequireUniqueEmail = true;
        options.Password.RequireDigit = false;
        options.Password.RequiredLength = 6;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = false;
        options.Password.RequireLowercase = false;
    })
    .AddEntityFrameworkStores<EducationPlatformApiContext>();

builder.Services.AddScoped<JwtService>();

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidAudience = builder.Configuration["Jwt:Audience"],
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])
            )
        };
    });


builder.Services.AddEndpointsApiExplorer();

// Register the Swagger services
builder.Services.AddSwaggerDocument(config =>
{
    config.PostProcess = document =>
    {
        document.Info.Version = "v1";
        document.Info.Title = "Education Platform Api";
        document.Info.TermsOfService = "None";
        document.Info.Contact = new OpenApiContact
        {
            Name = "test",
            Email = string.Empty,
            Url = "anything"
        };
        document.Info.License = new OpenApiLicense
        {
            Name = "Use under Test",
            Url = "https://example.com/license"
        };
    };
    // Add an authenticate button to Swagger for JWT tokens
    config.DocumentProcessors.Add(new SecurityDefinitionAppender("JWT", new OpenApiSecurityScheme
    {
        Type = OpenApiSecuritySchemeType.ApiKey,
        Name = "Authorization",
        In = OpenApiSecurityApiKeyLocation.Header,
        Scheme = "bearer",
        BearerFormat = "jwt",
        Description = "Type into the text box: Bearer {token}",
    }));
    config.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor("JWT"));

});


builder.Services.AddSqlite<EducationPlatformApiContext>("Data Source=EducationPlatform.db");

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();

    app.UseSwaggerUi3();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.CreateDbIfNotExists();

app.Run();
