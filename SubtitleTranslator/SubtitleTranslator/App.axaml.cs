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

public partial class App : Application
{
  public override void Initialize()
  {
    AvaloniaXamlLoader.Load(this);
  }

  public override void OnFrameworkInitializationCompleted()
  {
    var locator = new ViewLocator();
    DataTemplates.Add(locator);
    if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
    {
      // desktop.MainWindow = new MainWindow
      // {
      //   DataContext = new MainViewModel()
      // };
      
      var services = new ServiceCollection();
      ConfigureViewModels(services);
      ConfigureViews(services);
      var provider = services.BuildServiceProvider();

      Ioc.Default.ConfigureServices(provider);

      var vm = Ioc.Default.GetService<MainViewModel>();
      desktop.MainWindow = new MainWindow
      {
        DataContext = vm
      };
    }

    base.OnFrameworkInitializationCompleted();
  }
  [Singleton(typeof(FileListViewModel))]
  [Singleton(typeof(FileUploadViewModel))]
  [Singleton(typeof(MainViewModel))]
  internal static partial void ConfigureViewModels(IServiceCollection services);
  
  [Singleton(typeof(FileListView))]
  [Singleton(typeof(FileUploadView))]
  [Singleton(typeof(MainView))]
  internal static partial void ConfigureViews(IServiceCollection services);

}