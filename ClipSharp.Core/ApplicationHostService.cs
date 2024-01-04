using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ClipSharp.Core;

public class ApplicationHostService : IHostedService
{
    private readonly IServiceProvider _serviceProvider;

    private ILogger<ApplicationHostService> logger;

    public ApplicationHostService(IServiceProvider serviceProvider, ILogger<ApplicationHostService> logger)
    {
        this._serviceProvider = serviceProvider;
        this.logger = logger;
    }

    /// <inheritdoc />
    public Task StartAsync(CancellationToken cancellationToken)
    {
        // bool result = NativeMethods.AddClipboardFormatListener(IntPtr.Zero);
        this.logger.LogInformation("ClipSharp Started.  {Time:u}", DateTimeOffset.Now);
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public Task StopAsync(CancellationToken cancellationToken)
    {
        this.logger.LogInformation("ClipSharp Stopped.  {Time:u}", DateTimeOffset.Now);
        return Task.CompletedTask;
    }
}