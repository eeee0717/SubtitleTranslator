using CommunityToolkit.Mvvm.ComponentModel;

namespace SubtitleTranslator.ViewModels;

public partial class FileListViewModel: ViewModelBase
{
  [ObservableProperty] public string[] _animals = new []{"Dog", "Cat", "Bird"};
  
}