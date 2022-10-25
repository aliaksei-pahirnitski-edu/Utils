using Cocona;
using FileMerger.App.Handlers;
using FileMerger.Domain.Abstract;
using FilesHashComparer.Scan;
using Microsoft.Extensions.DependencyInjection;

var coconaBuilder = CoconaApp.CreateBuilder(args);
coconaBuilder.Services.AddTransient<IScanner, HashScanner>();
coconaBuilder.Services.AddTransient<StatusHandler>();


var app = coconaBuilder.Build();
app.AddCommands<StatusController>();
app.AddCommands<ScanController>();

app.Run();