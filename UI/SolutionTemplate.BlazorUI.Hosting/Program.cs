var builder = WebApplication.CreateBuilder(args);

//builder.Logging.AddSerilog();

//var configuration = builder.Configuration;
var services = builder.Services;

//var db_type = configuration["Database"];
//switch (db_type)
//{
//    default: throw new InvalidOperationException($"База данных {db_type} не поддерживается");

//    case "InMemory":
//        services.AddDbContext<SolutionTemplateDB>(opt => opt.UseInMemoryDatabase("SolutionTemplateDb"))
//           .AddTransient<IDbInitializer>(
//                s => new SolutionTemplateDBInitializer(
//                        s.GetRequiredService<SolutionTemplateDB>(),
//                        s.GetRequiredService<ILogger<SolutionTemplateDBInitializer>>())
//                    { Ignore = true })
//           .AddSolutionTemplateRepositories();
//        break;

//    case "SqlServer":
//        services.AddSolutionTemplateDbContextSqlServer(configuration.GetConnectionString(db_type));
//        break;

//    case "Sqlite":
//        services.AddSolutionTemplateDbContextSqlite(configuration.GetConnectionString(db_type));
//        break;
//}

//services.AddApiVersioning(opt =>
//{
//    opt.ReportApiVersions = true;
//    opt.DefaultApiVersion = new ApiVersion(1, 0);
//    opt.AssumeDefaultVersionWhenUnspecified = true;
//    //opt.ApiVersionReader = new MediaTypeApiVersionReader("x-api-version");
//    //opt.ApiVersionReader = new QueryStringApiVersionReader("x-api-version");
//    opt.ApiVersionReader = new HeaderApiVersionReader("x-api-version");

//});

//services.AddSignalR();
//services.AddMediatR(typeof(Program));
//services.AddAutoMapper(typeof(Program));

//services.AddSwaggerGen(c => c.SwaggerDoc("v1", new OpenApiInfo { Title = "SolutionTemplate.WEB.API", Version = "v1" }));

services.AddControllersWithViews();
services.AddRazorPages();

//services.AddResponseCompression();

var app = builder.Build();
//await using (var scope = app.Services.CreateAsyncScope())
//{
//    var initializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
//    await initializer.InitializeAsync();
//}

//app.UseResponseCompression();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseWebAssemblyDebugging();
    //app.UseSwagger();
    //app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "SolutionTemplate.WEB.API v1"));
}
else
{
    app.UseExceptionHandler("/Error");
}

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

//app.UseSerilogRequestLogging();

app.UseRouting();

app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

await app.RunAsync();