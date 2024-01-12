using Avalonia.Controls;
using SubtitleTranslator.ViewModels;

namespace SubtitleTranslator.Views;

public partial class FileUploadView : UserControl
{
  public FileUploadView()
  {
    InitializeComponent();
    this.DataContext = new FileUploadViewModel();
  }
}