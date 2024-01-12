using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;

namespace SubtitleTranslator.ViewModels;

public partial class FileListViewModel: ViewModelBase
{
  [ObservableProperty]
  private FileUploadViewModel? _fileUploadVm = App.Current.Services.GetService<FileUploadViewModel>();

  [RelayCommand]
  private void ButtonClicked()
  {
    Console.WriteLine(FileUploadVm?.ToBeTranslatedItems.Count);
  }
  

}