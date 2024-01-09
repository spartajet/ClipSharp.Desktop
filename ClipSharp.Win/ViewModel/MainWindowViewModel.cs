using Microsoft.Extensions.Logging;

namespace ClipSharp.Win.ViewModel;

public class MainWindowViewModel
{
    private ILogger<MainWindowViewModel> logger;

    public MainWindowViewModel(ILogger<MainWindowViewModel> logger)
    {
        this.logger = logger;
    }
}