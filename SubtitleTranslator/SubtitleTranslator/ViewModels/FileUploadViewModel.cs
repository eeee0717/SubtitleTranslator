using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SubtitleTranslator.Models;

namespace SubtitleTranslator.ViewModels;

public partial class FileUploadViewModel : ViewModelBase
{
  [ObservableProperty]
  public ObservableCollection<ToBeTranslatedItem> _toBeTranslatedItems = null!;
  [RelayCommand]
  private async Task OpenFile(CancellationToken token)
  {
    ErrorMessages?.Clear();
    try
    {
      var files = await DoOpenFilePickerAsync();
      var storageFiles = files as IStorageFile[] ?? files.ToArray();
      if(!storageFiles.Any())
        return;
      // 读取多个文件
      foreach (var file in storageFiles)
      {
        await using var readStream = await file!.OpenReadAsync();
        using var reader = new StreamReader(readStream);
        var fileText = await reader.ReadToEndAsync(token);
        var toBeTranslatedItem = new ToBeTranslatedItem
        {
          State = "待翻译",
          FileName = file.Name,
          Description = fileText.Length.ToString()
        };
        this.ToBeTranslatedItems.Add(toBeTranslatedItem);
      }

    }
    catch (Exception e)
    {
      Console.WriteLine(e);
      throw;
    }
  }
  private async Task<IEnumerable<IStorageFile?>> DoOpenFilePickerAsync()
  {
    if (Application.Current?.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop ||
        desktop.MainWindow?.StorageProvider is not { } provider)
      throw new NullReferenceException("Missing StorageProvider instance.");

    var files = await provider.OpenFilePickerAsync(new FilePickerOpenOptions()
    {
      Title = "选择字幕文件",
      FileTypeFilter = new[] {SubtitleFileAll},
      AllowMultiple = true
    });
    return files;
  }

  private static FilePickerFileType SubtitleFileAll { get; } = new("All Subtitle Files")
  {
    Patterns = new[] { "*.srt","*.ass" },
  };
}