using BankOfLeverx.Application.CQRS.Commands;
using BankOfLeverx.Application.CQRS.Queries;
using BankOfLeverx.Application.Interfaces;
using BankOfLeverx.Application.Services;
using BankOfLeverx.Application.Validators;
using BankOfLeverx.Infrastructure.Data;
using BankOfLeverx.Infrastructure.Data.Repositories;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Configuration.AddEnvironmentVariables();

// Swagger setup
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter Your Token Without Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddValidatorsFromAssemblyContaining<LoanValidator>();

builder.Services.AddApplicationInsightsTelemetry();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddHealthChecks()
    .AddSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));

builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<ILoanRepository, LoanRepository>();
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(GetAllLoansQuery).Assembly);
    cfg.RegisterServicesFromAssembly(typeof(GetLoanByIdQuery).Assembly);
    cfg.RegisterServicesFromAssembly(typeof(CreateLoanCommand).Assembly);
    cfg.RegisterServicesFromAssembly(typeof(DeleteLoanCommand).Assembly);
    cfg.RegisterServicesFromAssembly(typeof(PatchLoanCommand).Assembly);
    cfg.RegisterServicesFromAssembly(typeof(UpdateLoanCommand).Assembly);
});

builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<ILoanService, LoanService>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<ITransactionService, TransactionService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ILoanPaymentService, LoanPaymentService>();


builder.Services.AddAutoMapper(typeof(MapProfile).Assembly);

builder.Services.AddScoped<IDbConnection>(sp =>
    new Microsoft.Data.SqlClient.SqlConnection(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["JWTIssuer"],
        ValidAudience = builder.Configuration["JWTAudience"],
        IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(
            System.Text.Encoding.UTF8.GetBytes(builder.Configuration["JWTKey"]))
    };
});

builder.Services.AddAuthorization();

try
{
    var app = builder.Build();

    var logger = app.Services.GetRequiredService<ILogger<Program>>();

    logger.LogInformation("Application is starting...");

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.MapHealthChecks("health");

    app.UseHttpsRedirection();

    app.UseAuthentication();
    app.UseAuthorization();

    foreach (var kvp in builder.Configuration.AsEnumerable())
    {
        Console.WriteLine($"{kvp.Key} = {kvp.Value}");
    }

    app.MapControllers();

    logger.LogInformation("Application started successfully.");

    app.Run();
}
catch (Exception ex)
{
    using var loggerFactory = LoggerFactory.Create(builder =>
    {
        builder.AddConsole();
        builder.SetMinimumLevel(LogLevel.Critical);
    });
    var logger = loggerFactory.CreateLogger("Startup");

    logger.LogCritical(ex, "Application failed to start.");

    throw;
}
