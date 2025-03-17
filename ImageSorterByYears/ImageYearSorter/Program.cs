
using Cocona;
using ImageYearSorter.App;
using ImageYearSorter.Impl;
using ImageYearSorter.Utils;
using Microsoft.Extensions.DependencyInjection;

Console.OutputEncoding = System.Text.Encoding.UTF8;
Console.InputEncoding = System.Text.Encoding.UTF8;

var coconaBuilder = CoconaApp.CreateBuilder(args);
coconaBuilder.Services.AddTransient<IPhotoDateProvider, PhotoDateProvider>();

var app = coconaBuilder.Build();
app.AddCommands<PictureInfoHandler>();
app.AddCommands<FolderReorganizationHandler>();
app.Run();
