using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Shared.Behaviors;
public class LoggingBahavior<TRequest, TResponse>
    (ILogger<LoggingBahavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull, IRequest<TResponse>
    where TResponse : notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        logger.LogInformation("START Handle Request={Request} - Response= {Response} - RequestData= {RequestData}",
            typeof(TRequest).Name, typeof(TResponse).Name, request);

        var timer = new Stopwatch();
        timer.Start();

        var response = await next(cancellationToken);

        timer.Stop();
        if(timer.Elapsed.Seconds > 3)
        {
            logger.LogWarning("Long Running Request: {Request} ({ElapsedSeconds} seconds) - Response= {Response} - RequestData= {RequestData}",
                typeof(TRequest).Name, timer.Elapsed.Seconds, typeof(TResponse).Name, request);
        }

        logger.LogInformation("END Handle Request={Request} - Response= {Response} - RequestData= {RequestData} - ElapsedTime= {ElapsedTime} ms",
            typeof(TRequest).Name, typeof(TResponse).Name, request, timer.ElapsedMilliseconds);

        return response;
    }
}
