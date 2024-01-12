using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using CommunityToolkit.Extensions.DependencyInjection;
using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using SubtitleTranslator.ViewModels;
using SubtitleTranslator.Views;

namespace SubtitleTranslator;

public class App : Application
{
  public override void Initialize()
  {
    AvaloniaXamlLoader.Load(this);
  }
  public new static App Current => (App)Application.Current;

  public IServiceProvider Services { get; set; } = ConfigureServices();

  public override void OnFrameworkInitializationCompleted()
  {
    if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
    {
      var vm = Services.GetService<MainViewModel>();
      desktop.MainWindow = new MainWindow
      {
        DataContext = vm
      };
    }
    base.OnFrameworkInitializationCompleted();
  }

  private static IServiceProvider ConfigureServices()
  {
    var services = new ServiceCollection();
    services.AddSingleton<MainViewModel>();
    services.AddSingleton<FileUploadViewModel>();
    services.AddSingleton<FileListViewModel>();
    return services.BuildServiceProvider();
  }
}