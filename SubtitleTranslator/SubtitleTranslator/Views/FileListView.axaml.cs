using System;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using CommunityToolkit.Mvvm.DependencyInjection;
using SubtitleTranslator.ViewModels;

namespace SubtitleTranslator.Views;

public partial class FileListView : UserControl
{
  private FileUploadViewModel? _fileUploadVm;
  public FileListView()
  {
    InitializeComponent();
    this.DataContext = new FileListViewModel();
    this._fileUploadVm = Ioc.Default.GetService<FileUploadViewModel>();
  }

  // private void Button_OnClick(object? sender, RoutedEventArgs e)
  // {
  //   var fileUpLoadVM = Ioc.Default.GetService<FileUploadViewModel>();
  //   Console.WriteLine(fileUpLoadVM.Title);
  //   
  // }
}