using Application;
using Application.Entities.ApplicationContext;
using Application.Repositories;
using Application.Services;
using Application.Validations;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Sinks.MSSqlServer;

var builder = WebApplication.CreateBuilder(args);

AppSettings.Initialize(builder.Configuration);
builder.Services.AddDbContext<AppDbContext>(options =>
       options.UseSqlServer(AppSettings.DatabaseConnectionString)
);

# region ADD SERVICES
builder.Services.AddControllersWithViews();

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();

# endregion ADD SERVICES

# region REGISTER SERVICES TO DI CONTAINER

builder.Services.AddRepositoryServices();
builder.Services.AddApplicationServicesFromAssembly();

//builder.Services.AddTransient<IValidator<BorrowViewModel>, BorrowValidator>();
builder.Services.AddValidatorsFromAssembly(typeof(BorrowValidator).Assembly);

# endregion REGISTER SERVICES TO DI CONTAINER

# region CONFIGURE SERILOGS
var columnOptions = new ColumnOptions();
columnOptions.Store.Remove(StandardColumn.Properties);

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Warning()
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.MSSqlServer(
        connectionString: AppSettings.DatabaseConnectionString,
        sinkOptions: new MSSqlServerSinkOptions
        {
            TableName = "ApplicationLogs",
            AutoCreateSqlTable = false
        },
        columnOptions: columnOptions
    )
    .CreateLogger();
builder.Host.UseSerilog();

# endregion CONFIGURE SERILOGS

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseSerilogRequestLogging();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
