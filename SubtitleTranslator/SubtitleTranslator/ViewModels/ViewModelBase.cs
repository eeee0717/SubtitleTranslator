﻿using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace SubtitleTranslator.ViewModels;

public partial class ViewModelBase : ObservableObject
{
  protected ViewModelBase()
  {
    ErrorMessages = new ObservableCollection<string>();
  }
    
  [ObservableProperty] 
  private ObservableCollection<string>? _errorMessages;
}