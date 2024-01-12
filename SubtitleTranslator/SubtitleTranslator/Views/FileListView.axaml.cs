using System;
using System.Collections.ObjectModel;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using SubtitleTranslator.Models;
using SubtitleTranslator.ViewModels;

namespace SubtitleTranslator.Views;

public partial class FileListView : UserControl
{
  public FileListView()
  {
    InitializeComponent();
  }
  
}