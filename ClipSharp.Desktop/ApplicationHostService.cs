using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace ClipSharp.Desktop;

public class ApplicationHostService:IHostedService
{
    private readonly IServiceProvider _serviceProvider;
    public ApplicationHostService(IServiceProvider serviceProvider)
    {
        this._serviceProvider = serviceProvider;
    }

    /// <inheritdoc />
    public Task StartAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}

