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
  public override void OnFrameworkInitializationCompleted()
  {
    if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
    {
      desktop.MainWindow = new MainWindow
      {
        DataContext = new MainViewModel()
      };
    }
    base.OnFrameworkInitializationCompleted();
  }
  
}