using Cocona;
using FileMerger.App.Handlers;
using FileMerger.Domain.Abstract;
using FileMerger.Settings;
using FileMerger.Sqlite;
using FilesHashComparer.Scan;
using Microsoft.Extensions.DependencyInjection;


Console.OutputEncoding = System.Text.Encoding.UTF8;

var coconaBuilder = CoconaApp.CreateBuilder(args);
// coconaBuilder.Configuration.SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
coconaBuilder.Services.Configure<CommonSettings>(coconaBuilder.Configuration);
coconaBuilder.Services.Configure<MergeSettings>(coconaBuilder.Configuration.GetSection("merging"));

coconaBuilder.Services.AddTransient<IScanner, HashScanner>();
coconaBuilder.Services.AddTransient<StatusHandler>();
coconaBuilder.Services.AddSqliteRepo();

var app = coconaBuilder.Build();
app.AddCommands<StatusController>();
app.AddCommands<ScanController>();
app.AddCommands<SearchDuplicatesController>();

app.Run();