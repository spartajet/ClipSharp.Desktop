using Microsoft.Extensions.Logging;

namespace ClipSharp.Win.ViewModel;

public class ClipSelectViewModel
{
    private readonly ILogger<ClipSelectViewModel> logger;

    public ClipSelectViewModel(ILogger<ClipSelectViewModel> logger)
    {
        this.logger = logger;
    }
}